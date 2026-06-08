using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using MES.DAL;

namespace MES.Forms
{
    /// <summary>
    /// 생산오더 조회 화면 (MSSQL 전환 버전)
    /// 변경 사항: ProductionOrderOracleDAL → ProductionOrderDAL
    /// </summary>
    public partial class FrmWorkOrder : DevExpress.XtraEditors.XtraForm
    {
        // Oracle DAL → MSSQL DAL 교체 (클래스명만 변경, 메서드 시그니처 동일)
        private readonly ProductionOrderDAL _dal = new ProductionOrderDAL();

        public FrmWorkOrder()
        {
            InitializeComponent();   // 디자이너
            InitGridColumns();       // 컬럼 구성
            InitControls();          // 초기값
        }

        // ── 그리드 컬럼 구성 ───────────────────────────
        private void InitGridColumns()
        {
            gv_Main.Columns.Clear();

            AddCol("WORK_ORDER_NO", "작업오더번호", 150);
            AddCol("BOM_ITEM_CODE", "품목코드", 150);
            AddCol("BOM_ITEM_DESCRIPTION", "품목명", 150);
            AddCol("WORK_ORDER_QTY", "생산지시량", 100, "N0");
            AddCol("WORK_ORDER_NET_QTY", "순투입량", 80, "N0");
            AddCol("WORK_ORDER_STATUS_DESC", "상태", 80);
            AddCol("REMARK", "비고", 200);

            gv_Main.OptionsView.ShowGroupPanel = false;
            gv_Main.OptionsBehavior.Editable = false;
            gv_Main.OptionsView.ColumnAutoWidth = false;
            gv_Main.Appearance.FocusedRow.BackColor = Color.FromArgb(204, 229, 255);
        }

        private void AddCol(string field, string caption,
            int width, string format = null)
        {
            var col = new GridColumn
            {
                FieldName = field,
                Caption = caption,
                Width = width,
                Visible = true
            };
            if (format != null)
                col.DisplayFormat.FormatString = format;
            gv_Main.Columns.Add(col);
        }

        // ── 초기값 ─────────────────────────────────────
        private void InitControls()
        {
            txt_OrderLineId.EditValue = null;
        }

        // ── 조회 ───────────────────────────────────────
        private void SearchData()
        {
            if (string.IsNullOrWhiteSpace(
                txt_OrderLineId.EditValue?.ToString()))
            {
                XtraMessageBox.Show(
                    "오더라인ID를 입력하세요.", "확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_OrderLineId.Focus();
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                // SOB_ID=1, ORG_ID=1 → 로컬 테스트 데이터 기준
                var dt = _dal.SelectList(
                    sobId: 1,
                    orgId: 1,
                    orderLineId: Convert.ToDecimal(txt_OrderLineId.EditValue));

                grid_Main.DataSource = dt;
                this.Text = $"생산오더 조회 - {dt.Rows.Count}건";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    $"조회 실패\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // ── 이벤트 ─────────────────────────────────────
        private void btn_Search_Click(object sender, EventArgs e)
            => SearchData();

        // ── 단축키 ─────────────────────────────────────
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8) { SearchData(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
