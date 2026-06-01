using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using MES.DAL;
using MES.Models;
using System.Collections.Generic;
using MES.ProductionOrder;

namespace MES.Forms
{
    /// <summary>
    /// 생산지시 관리 메인 화면
    /// DevExpress XtraGrid + XtraEditors 기반
    /// </summary>
    public partial class FrmProductionOrder : XtraForm
    {
        // ───────────────────────────────────────
        // 필드
        // ───────────────────────────────────────
        private readonly ProductionOrderDAL _dal    = new ProductionOrderDAL();
        private readonly string             _userId = AppSession.UserId;  // 로그인 세션

        // 검색 패널 컨트롤
        private LookUpEdit          cboItemId;
        private ComboBoxEdit        cboStatus;
        private ComboBoxEdit        cboPriority;
        private DateEdit            dtpStartFrom;
        private DateEdit            dtpStartTo;
        private SimpleButton        btnSearch;
        private SimpleButton        btnNew;
        private SimpleButton        btnSave;
        private SimpleButton        btnCancel;
        private SimpleButton        btnConfirm;
        private SimpleButton        btnCancelOrder;

        // 그리드
        private GridControl         gridList;
        private GridView            viewList;

        // 입력 패널 컨트롤
        private LabelControl        lblOrderId;
        private LookUpEdit          cboItemInput;
        private SpinEdit            spnOrderQty;
        private DateEdit            dtpPlanStart;
        private DateEdit            dtpPlanEnd;
        private ComboBoxEdit        cboPriorityInput;
        private TextEdit            txtCustomerOrderId;
        private MemoEdit            txtRemark;
        private LabelControl        lblStatus;

        // 현재 편집 상태
        private bool _isNew     = false;
        private bool _isDirty   = false;
        private string _editingId = null;

        // ───────────────────────────────────────
        // 생성자
        // ───────────────────────────────────────
        public FrmProductionOrder()
        {
            InitializeComponent();
            InitializeUI();
            //LoadComboData();
            //SearchData();

            LoadDummyData(); // test용
        }
        //---
        //테스트용
        //---
        private void LoadDummyData()
        {
            var dt = new DataTable();
            dt.Columns.Add("PROD_ORDER_ID");
            dt.Columns.Add("ITEM_ID");
            dt.Columns.Add("ITEM_NAME");
            dt.Columns.Add("ORDER_QTY");
            dt.Columns.Add("PLAN_START_DATE");
            dt.Columns.Add("PLAN_END_DATE");
            dt.Columns.Add("PRIORITY_NM");
            dt.Columns.Add("STATUS");
            dt.Columns.Add("STATUS_NM");
            dt.Columns.Add("TOTAL_LOT_CNT");
            dt.Columns.Add("GOOD_QTY");
            dt.Columns.Add("CUSTOMER_ORDER_ID");
            dt.Columns.Add("REMARK");
            dt.Columns.Add("PRIORITY");

            dt.Rows.Add("PO-20241101-0001", "ITM-1001", "전자제어모듈 A형",
                        500, "2024-11-01", "2024-11-10",
                        "긴급", "DONE", "완료", 3, 490, "CO-88821", "", "HIGH");

            dt.Rows.Add("PO-20241105-0001", "ITM-1001", "전자제어모듈 A형",
                        1200, "2024-11-05", "2024-11-20",
                        "보통", "RUN", "진행중", 5, 300, "CO-88850", "", "NORMAL");

            dt.Rows.Add("PO-20241115-0001", "ITM-2045", "센서 하우징 B형",
                        300, "2024-11-15", "2024-11-22",
                        "낮음", "WAIT", "대기", 0, 0, "", "", "LOW");

            gridList.DataSource = dt;
        }
        // ───────────────────────────────────────
        // UI 초기화 (Designer 대신 코드로 구성)
        // ───────────────────────────────────────
        private void InitializeUI()
        {
            this.Text           = "생산지시 관리";
            this.Size           = new Size(1400, 900);
            this.StartPosition  = FormStartPosition.CenterScreen;

            BuildSearchPanel();
            BuildToolbar();
            BuildGrid();
            BuildInputPanel();

            SetButtonState(false, false);
        }

        // ── 검색 패널 ──────────────────────────
        private void BuildSearchPanel()
        {
            var pnlSearch = new PanelControl
            {
                Location = new Point(10, 10),
                Size     = new Size(1360, 65),
                BorderStyle = BorderStyles.NoBorder
            };
            this.Controls.Add(pnlSearch);

            int x = 10;
            AddSearchLabel(pnlSearch, "품목",   x);
            cboItemId = new LookUpEdit { Location = new Point(x + 45, 20), Size = new Size(200, 24) };
            cboItemId.Properties.DisplayMember    = "DisplayName";
            cboItemId.Properties.ValueMember      = "ItemId";
            cboItemId.Properties.SearchMode       = SearchMode.AutoFilter;
            pnlSearch.Controls.Add(cboItemId);

            x += 260;
            AddSearchLabel(pnlSearch, "상태",   x);
            cboStatus = new ComboBoxEdit { Location = new Point(x + 45, 20), Size = new Size(110, 24) };
            cboStatus.Properties.Items.AddRange(new[] { "(전체)", "대기", "진행중", "완료", "취소", "보류" });
            cboStatus.EditValue = "(전체)";
            pnlSearch.Controls.Add(cboStatus);

            x += 170;
            AddSearchLabel(pnlSearch, "우선순위", x);
            cboPriority = new ComboBoxEdit { Location = new Point(x + 60, 20), Size = new Size(100, 24) };
            cboPriority.Properties.Items.AddRange(new[] { "(전체)", "긴급", "보통", "낮음" });
            cboPriority.EditValue = "(전체)";
            pnlSearch.Controls.Add(cboPriority);

            x += 175;
            AddSearchLabel(pnlSearch, "계획시작", x);
            dtpStartFrom = new DateEdit { Location = new Point(x + 60, 20), Size = new Size(110, 24) };
            dtpStartFrom.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dtpStartFrom.Properties.EditFormat.FormatString    = "yyyy-MM-dd";
            dtpStartFrom.EditValue = DateTime.Today.AddMonths(-1);
            pnlSearch.Controls.Add(dtpStartFrom);

            var lblTilde = new LabelControl { Text = "~", Location = new Point(x + 178, 23) };
            pnlSearch.Controls.Add(lblTilde);

            dtpStartTo = new DateEdit { Location = new Point(x + 192, 20), Size = new Size(110, 24) };
            dtpStartTo.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dtpStartTo.Properties.EditFormat.FormatString    = "yyyy-MM-dd";
            dtpStartTo.EditValue = DateTime.Today.AddMonths(1);
            pnlSearch.Controls.Add(dtpStartTo);

            btnSearch = new SimpleButton
            {
                Text     = "조회",
                Location = new Point(x + 320, 18),
                Size     = new Size(80, 28),
                Image    = null  // 아이콘 설정 시 DevExpress ImageCollection 사용
            };
            btnSearch.Click += BtnSearch_Click;
            pnlSearch.Controls.Add(btnSearch);
        }

        // ── 툴바 버튼 ───────────────────────────
        private void BuildToolbar()
        {
            var pnlToolbar = new PanelControl
            {
                Location = new Point(10, 80),
                Size     = new Size(1360, 36),
                BorderStyle = BorderStyles.NoBorder
            };
            this.Controls.Add(pnlToolbar);

            int x = 0;
            btnNew = CreateToolBtn(pnlToolbar, "신규(F4)",    x, Color.FromArgb(0, 122, 204));
            btnNew.Click += BtnNew_Click;

            x += 95;
            btnSave = CreateToolBtn(pnlToolbar, "저장(F5)",   x, Color.FromArgb(39, 174, 96));
            btnSave.Click += BtnSave_Click;

            x += 95;
            btnCancel = CreateToolBtn(pnlToolbar, "취소(ESC)", x, Color.FromArgb(149, 165, 166));
            btnCancel.Click += BtnCancel_Click;

            x += 110;
            var sep = new LabelControl { Text = "|", Location = new Point(x, 5), ForeColor = Color.LightGray };
            pnlToolbar.Controls.Add(sep);
            x += 15;

            btnConfirm = CreateToolBtn(pnlToolbar, "확정",    x, Color.FromArgb(230, 126, 34));
            btnConfirm.Click += BtnConfirm_Click;

            x += 95;
            btnCancelOrder = CreateToolBtn(pnlToolbar, "취소처리", x, Color.FromArgb(192, 57, 43));
            btnCancelOrder.Click += BtnCancelOrder_Click;
        }

        private SimpleButton CreateToolBtn(Control parent, string text, int x, Color backColor)
        {
            var btn = new SimpleButton
            {
                Text      = text,
                Location  = new Point(x, 4),
                Size      = new Size(88, 28),
                BackColor = backColor,
                ForeColor = Color.White
            };
            parent.Controls.Add(btn);
            return btn;
        }

        // ── 그리드 ─────────────────────────────
        private void BuildGrid()
        {
            gridList = new GridControl
            {
                Location = new Point(10, 125),
                Size     = new Size(820, 720)
            };
            viewList = new GridView();
            gridList.MainView = viewList;
            gridList.ViewCollection.AddRange(new BaseView[] { viewList });
            this.Controls.Add(gridList);

            // 뷰 옵션
            viewList.OptionsView.ShowGroupPanel        = false;
            viewList.OptionsView.ColumnAutoWidth       = false;
            viewList.OptionsBehavior.Editable          = false;
            viewList.OptionsSelection.EnableAppearanceFocusedRow = true;
            viewList.OptionsView.ShowIndicator         = true;
            viewList.Appearance.FocusedRow.BackColor   = Color.FromArgb(204, 229, 255);

            // 컬럼 정의
            AddGridColumn(viewList, "PROD_ORDER_ID",     "생산지시번호", 140);
            AddGridColumn(viewList, "ITEM_ID",           "품목코드",     90);
            AddGridColumn(viewList, "ITEM_NAME",         "품목명",       160);
            AddGridColumn(viewList, "ORDER_QTY",         "지시수량",     80,  "N0", HorzAlignment.Far);
            AddGridColumn(viewList, "PLAN_START_DATE",   "계획시작",     90,  "yyyy-MM-dd");
            AddGridColumn(viewList, "PLAN_END_DATE",     "계획종료",     90,  "yyyy-MM-dd");
            AddGridColumn(viewList, "PRIORITY_NM",       "우선순위",     70);
            AddGridColumn(viewList, "STATUS_NM",         "상태",         70);
            AddGridColumn(viewList, "TOTAL_LOT_CNT",     "LOT수",        60,  "N0", HorzAlignment.Center);
            AddGridColumn(viewList, "GOOD_QTY",          "양품수량",     80,  "N0", HorzAlignment.Far);
            AddGridColumn(viewList, "CUSTOMER_ORDER_ID", "고객오더",     110);

            // 상태 셀 컬러링
            viewList.RowCellStyle += ViewList_RowCellStyle;
            viewList.FocusedRowChanged += ViewList_FocusedRowChanged;
        }

        private void AddGridColumn(GridView view, string fieldName, string caption,
            int width, string formatStr = null, HorzAlignment align = HorzAlignment.Default)
        {
            var col = new GridColumn
            {
                FieldName = fieldName,
                Caption   = caption,
                Width     = width,
                Visible   = true
            };
            if (!string.IsNullOrEmpty(formatStr))
                col.DisplayFormat.FormatString = formatStr;
            if (align != HorzAlignment.Default)
                col.AppearanceCell.TextOptions.HAlignment = align;
            view.Columns.Add(col);
        }

        // ── 입력 패널 ──────────────────────────
        private void BuildInputPanel()
        {
            var pnlInput = new GroupControl
            {
                Text     = "생산지시 정보",
                Location = new Point(840, 125),
                Size     = new Size(530, 720)
            };
            this.Controls.Add(pnlInput);

            int y = 35;
            const int LBL_W = 90, CTL_X = 110, CTL_W = 390;

            // 생산지시번호 (읽기전용)
            AddInputLabel(pnlInput, "생산지시번호", 10, y);
            lblOrderId = new LabelControl
            {
                Text     = "(자동채번)",
                Location = new Point(CTL_X, y + 2),
                AutoSizeMode = LabelAutoSizeMode.None,
                Size     = new Size(CTL_W, 22),
                Appearance = { Font = new Font("맑은 고딕", 9f, FontStyle.Bold) }
            };
            pnlInput.Controls.Add(lblOrderId);

            // 상태
            y += 35;
            AddInputLabel(pnlInput, "상태", 10, y);
            lblStatus = new LabelControl
            {
                Text     = "-",
                Location = new Point(CTL_X, y + 2),
                AutoSizeMode = LabelAutoSizeMode.None,
                Size     = new Size(100, 22)
            };
            pnlInput.Controls.Add(lblStatus);

            // 품목
            y += 35;
            AddInputLabel(pnlInput, "품목 *", 10, y);
            cboItemInput = new LookUpEdit
            {
                Location = new Point(CTL_X, y),
                Size     = new Size(CTL_W, 24)
            };
            cboItemInput.Properties.DisplayMember = "DisplayName";
            cboItemInput.Properties.ValueMember   = "ItemId";
            cboItemInput.Properties.SearchMode    = SearchMode.AutoFilter;
            cboItemInput.EditValueChanged         += CboItemInput_EditValueChanged;
            pnlInput.Controls.Add(cboItemInput);

            // 지시수량
            y += 35;
            AddInputLabel(pnlInput, "지시수량 *", 10, y);
            spnOrderQty = new SpinEdit
            {
                Location = new Point(CTL_X, y),
                Size     = new Size(150, 24)
            };
            spnOrderQty.Properties.MinValue    = 1;
            spnOrderQty.Properties.MaxValue    = 9999999;
            spnOrderQty.Properties.IsFloatValue = false;
            pnlInput.Controls.Add(spnOrderQty);

            // 계획시작일
            y += 35;
            AddInputLabel(pnlInput, "계획시작 *", 10, y);
            dtpPlanStart = new DateEdit
            {
                Location = new Point(CTL_X, y),
                Size     = new Size(150, 24)
            };
            dtpPlanStart.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dtpPlanStart.EditValue = DateTime.Today;
            pnlInput.Controls.Add(dtpPlanStart);

            // 계획종료일
            y += 35;
            AddInputLabel(pnlInput, "계획종료 *", 10, y);
            dtpPlanEnd = new DateEdit
            {
                Location = new Point(CTL_X, y),
                Size     = new Size(150, 24)
            };
            dtpPlanEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dtpPlanEnd.EditValue = DateTime.Today.AddDays(7);
            pnlInput.Controls.Add(dtpPlanEnd);

            // 우선순위
            y += 35;
            AddInputLabel(pnlInput, "우선순위 *", 10, y);
            cboPriorityInput = new ComboBoxEdit
            {
                Location = new Point(CTL_X, y),
                Size     = new Size(150, 24)
            };
            cboPriorityInput.Properties.Items.AddRange(new[]
                { "HIGH|긴급", "NORMAL|보통", "LOW|낮음" });
            cboPriorityInput.EditValue = "NORMAL|보통";
            pnlInput.Controls.Add(cboPriorityInput);

            // 고객오더번호
            y += 35;
            AddInputLabel(pnlInput, "고객오더번호", 10, y);
            txtCustomerOrderId = new TextEdit
            {
                Location = new Point(CTL_X, y),
                Size     = new Size(CTL_W, 24)
            };
            pnlInput.Controls.Add(txtCustomerOrderId);

            // 비고
            y += 35;
            AddInputLabel(pnlInput, "비고", 10, y);
            txtRemark = new MemoEdit
            {
                Location = new Point(CTL_X, y),
                Size     = new Size(CTL_W, 80)
            };
            pnlInput.Controls.Add(txtRemark);
        }

        private void AddSearchLabel(Control parent, string text, int x)
        {
            parent.Controls.Add(new LabelControl
            {
                Text     = text,
                Location = new Point(x, 24)
            });
        }

        private void AddInputLabel(Control parent, string text, int x, int y)
        {
            parent.Controls.Add(new LabelControl
            {
                Text     = text,
                Location = new Point(x, y + 4),
                AutoSizeMode = LabelAutoSizeMode.None,
                Size     = new Size(100, 22)
            });
        }

        // ───────────────────────────────────────
        // 콤보 데이터 로드
        // ───────────────────────────────────────
        private void LoadComboData()
        {
            try
            {
                var items = _dal.SelectItemCombo();

                // 검색용
                var allItem = new ItemComboModel { ItemId = null, DisplayName = "(전체)" };
                var searchList = new List<ItemComboModel> { allItem };
                searchList.AddRange(items);
                cboItemId.Properties.DataSource = searchList;
                cboItemId.EditValue = null;

                // 입력용
                cboItemInput.Properties.DataSource = items;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"콤보 데이터 로드 실패\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ───────────────────────────────────────
        // 조회
        // ───────────────────────────────────────
        private void SearchData()
        {
            try
            {
                var search = new ProductionOrderSearchModel
                {
                    ItemId        = cboItemId.EditValue?.ToString(),
                    Status        = MapStatusToCode(cboStatus.EditValue?.ToString()),
                    Priority      = MapPriorityToCode(cboPriority.EditValue?.ToString()),
                    PlanStartFrom = dtpStartFrom.DateTime == DateTime.MinValue
                                    ? (DateTime?)null : dtpStartFrom.DateTime,
                    PlanStartTo   = dtpStartTo.DateTime == DateTime.MinValue
                                    ? (DateTime?)null : dtpStartTo.DateTime,
                };

                var dt = _dal.SelectList(search);
                gridList.DataSource = dt;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"조회 실패\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ───────────────────────────────────────
        // 그리드 행 선택 → 입력 패널 바인딩
        // ───────────────────────────────────────
        private void ViewList_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (_isDirty) return;   // 편집 중에는 그리드 이동 무시

            var row = viewList.GetFocusedDataRow();
            if (row == null) return;

            _editingId = row["PROD_ORDER_ID"].ToString();
            BindInputPanel(row);
            SetButtonState(editable: false, selected: true);
        }

        private void BindInputPanel(DataRow row)
        {
            lblOrderId.Text             = row["PROD_ORDER_ID"].ToString();
            lblStatus.Text              = row["STATUS_NM"].ToString();
            cboItemInput.EditValue      = row["ITEM_ID"].ToString();
            spnOrderQty.EditValue       = Convert.ToDecimal(row["ORDER_QTY"]);
            dtpPlanStart.EditValue      = Convert.ToDateTime(row["PLAN_START_DATE"]);
            dtpPlanEnd.EditValue        = Convert.ToDateTime(row["PLAN_END_DATE"]);
            cboPriorityInput.EditValue  = MapPriorityToDisplay(row["PRIORITY"].ToString());
            txtCustomerOrderId.EditValue= row["CUSTOMER_ORDER_ID"];
            txtRemark.EditValue         = row["REMARK"];

            // 상태 컬러 표시
            /*lblStatus.ForeColor = row["STATUS"].ToString() switch
            {
                "RUN"    => Color.FromArgb(39, 174, 96),
                "DONE"   => Color.FromArgb(52, 152, 219),
                "CANCEL" => Color.FromArgb(192, 57, 43),
                "HOLD"   => Color.FromArgb(230, 126, 34),
                _        => Color.FromArgb(127, 140, 141)
            };*/

            SetEditEnabled(false);
        }

        // ───────────────────────────────────────
        // 버튼 이벤트
        // ───────────────────────────────────────
        private void BtnSearch_Click(object sender, EventArgs e) => SearchData();

        private void BtnNew_Click(object sender, EventArgs e)
        {
            _isNew      = true;
            _isDirty    = true;
            _editingId  = null;

            ClearInputPanel();
            lblOrderId.Text = "(자동채번)";
            lblStatus.Text  = "대기";
            SetEditEnabled(true);
            SetButtonState(editable: true, selected: false);
            cboItemInput.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var model = BuildModelFromInput();

                if (_isNew)
                {
                    string newId = _dal.Insert(model, _userId);
                    XtraMessageBox.Show($"생산지시가 등록되었습니다.\n생산지시번호: {newId}",
                        "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    model.ProdOrderId = _editingId;
                    _dal.Update(model, _userId);
                    XtraMessageBox.Show("생산지시가 수정되었습니다.",
                        "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                _isNew   = false;
                _isDirty = false;
                SetEditEnabled(false);
                SetButtonState(editable: false, selected: true);
                SearchData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"저장 실패\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_isDirty &&
                XtraMessageBox.Show("변경 내용을 취소하시겠습니까?", "확인",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _isNew   = false;
            _isDirty = false;
            SetEditEnabled(false);
            SetButtonState(editable: false, selected: _editingId != null);

            // 그리드에서 다시 바인딩
            var row = viewList.GetFocusedDataRow();
            if (row != null) BindInputPanel(row);
            else ClearInputPanel();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_editingId)) return;

            if (XtraMessageBox.Show(
                $"생산지시 [{_editingId}]을(를) 확정하시겠습니까?\n확정 후에는 LOT이 생성됩니다.",
                "확정 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _dal.UpdateStatus(_editingId, "RUN", _userId, "생산지시 확정");
                XtraMessageBox.Show("확정 처리되었습니다.", "완료",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                SearchData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"확정 실패\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_editingId)) return;

            if (XtraMessageBox.Show(
                $"생산지시 [{_editingId}]을(를) 취소하시겠습니까?",
                "취소 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                _dal.UpdateStatus(_editingId, "CANCEL", _userId, "생산지시 취소");
                XtraMessageBox.Show("취소 처리되었습니다.", "완료",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                SearchData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"취소 실패\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ───────────────────────────────────────
        // 그리드 셀 스타일 (상태별 컬러)
        // ───────────────────────────────────────
        private void ViewList_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "STATUS_NM") return;

            var row = viewList.GetDataRow(e.RowHandle);
            if (row == null) return;

            switch (row["STATUS"]?.ToString())
            {
                case "RUN":
                    e.Appearance.BackColor = Color.FromArgb(232, 245, 233);
                    e.Appearance.ForeColor = Color.FromArgb(27, 94, 32);
                    break;
                case "DONE":
                    e.Appearance.BackColor = Color.FromArgb(227, 242, 253);
                    e.Appearance.ForeColor = Color.FromArgb(13, 71, 161);
                    break;
                case "CANCEL":
                    e.Appearance.BackColor = Color.FromArgb(255, 235, 238);
                    e.Appearance.ForeColor = Color.FromArgb(183, 28, 28);
                    break;
                case "HOLD":
                    e.Appearance.BackColor = Color.FromArgb(255, 248, 225);
                    e.Appearance.ForeColor = Color.FromArgb(230, 81, 0);
                    break;
            }
        }

        // ───────────────────────────────────────
        // 품목 선택 시 단위 표시
        // ───────────────────────────────────────
        private void CboItemInput_EditValueChanged(object sender, EventArgs e)
        {
            _isDirty = true;
        }

        // ───────────────────────────────────────
        // 유효성 검사
        // ───────────────────────────────────────
        private bool ValidateInput()
        {
            if (cboItemInput.EditValue == null || string.IsNullOrWhiteSpace(cboItemInput.EditValue.ToString()))
            { ShowValidationError(cboItemInput, "품목을 선택하세요."); return false; }

            if (Convert.ToDecimal(spnOrderQty.EditValue) <= 0)
            { ShowValidationError(spnOrderQty, "지시수량을 입력하세요."); return false; }

            if (dtpPlanStart.DateTime > dtpPlanEnd.DateTime)
            { ShowValidationError(dtpPlanStart, "계획시작일이 종료일보다 늦습니다."); return false; }

            return true;
        }

        private void ShowValidationError(Control ctl, string msg)
        {
            XtraMessageBox.Show(msg, "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ctl.Focus();
        }

        // ───────────────────────────────────────
        // 입력 패널 → Model 변환
        // ───────────────────────────────────────
        private ProductionOrderModel BuildModelFromInput()
        {
            // "HIGH|긴급" 형태에서 코드만 추출
            var priorityRaw = cboPriorityInput.EditValue?.ToString() ?? "NORMAL|보통";
            var priorityCode = priorityRaw.Contains("|")
                ? priorityRaw.Split('|')[0] : priorityRaw;

            return new ProductionOrderModel
            {
                ItemId           = cboItemInput.EditValue?.ToString(),
                OrderQty         = Convert.ToDecimal(spnOrderQty.EditValue),
                PlanStartDate    = dtpPlanStart.DateTime,
                PlanEndDate      = dtpPlanEnd.DateTime,
                Priority         = priorityCode,
                CustomerOrderId  = txtCustomerOrderId.EditValue?.ToString(),
                Remark           = txtRemark.EditValue?.ToString()
            };
        }

        // ───────────────────────────────────────
        // UI 상태 제어
        // ───────────────────────────────────────
        private void SetButtonState(bool editable, bool selected)
        {
            btnNew.Enabled         = !editable;
            btnSave.Enabled        = editable;
            btnCancel.Enabled      = editable;
            btnConfirm.Enabled     = selected && !editable;
            btnCancelOrder.Enabled = selected && !editable;
        }

        private void SetEditEnabled(bool enabled)
        {
            cboItemInput.Properties.ReadOnly        = !enabled;
            spnOrderQty.Properties.ReadOnly         = !enabled;
            dtpPlanStart.Properties.ReadOnly        = !enabled;
            dtpPlanEnd.Properties.ReadOnly          = !enabled;
            cboPriorityInput.Properties.ReadOnly    = !enabled;
            txtCustomerOrderId.Properties.ReadOnly  = !enabled;
            txtRemark.Properties.ReadOnly           = !enabled;
        }

        private void ClearInputPanel()
        {
            lblOrderId.Text             = "(자동채번)";
            lblStatus.Text              = "-";
            cboItemInput.EditValue      = null;
            spnOrderQty.EditValue       = 0;
            dtpPlanStart.EditValue      = DateTime.Today;
            dtpPlanEnd.EditValue        = DateTime.Today.AddDays(7);
            cboPriorityInput.EditValue  = "NORMAL|보통";
            txtCustomerOrderId.EditValue= null;
            txtRemark.EditValue         = null;
        }

        // ───────────────────────────────────────
        // 코드 매핑 헬퍼
        // ───────────────────────────────────────
        private string MapStatusToCode(string nm)
        {
            switch (nm)
            {
                case "대기": return "WAIT";
                case "진행중": return "RUN";
                case "완료": return "DONE";
                case "취소": return "CANCEL";
                case "보류": return "HOLD";
                default: return null;
            }
        }

        private string MapPriorityToCode(string nm)
        {
            switch (nm)
            {
                case "긴급": return "HIGH";
                case "보통": return "NORMAL";
                case "낮음": return "LOW";
                default: return null;
            }
        }

        private string MapPriorityToDisplay(string code)
        {
            switch (code)
            {
                case "HIGH": return "HIGH|긴급";
                case "NORMAL": return "NORMAL|보통";
                case "LOW": return "LOW|낮음";
                default: return "NORMAL|보통";
            }
        }

        // 키보드 단축키
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F4)  { BtnNew_Click(null, null);    return true; }
            if (keyData == Keys.F5)  { BtnSave_Click(null, null);   return true; }
            if (keyData == Keys.F8)  { SearchData();                 return true; }
            if (keyData == Keys.Escape && _isDirty)
                { BtnCancel_Click(null, null); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Designer 필수 메서드 (실제 프로젝트에선 .Designer.cs로 분리)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ResumeLayout(false);
        }
    }
}
