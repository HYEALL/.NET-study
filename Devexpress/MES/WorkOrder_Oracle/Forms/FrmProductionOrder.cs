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
using MES.Models;
using System.Collections.Generic;
using MES.ProductionOrder;
using static MES.DAL.OracleBaseDAL;

namespace MES.Forms
{
    /// <summary>
    /// 생산지시 관리 메인 화면
    /// DevExpress XtraGrid + XtraEditors 기반
    /// </summary>
    public partial class FrmProductionOrder : XtraForm
    {
        private readonly ProductionOrderOracleDAL _dal = new ProductionOrderOracleDAL();
        private TextEdit txt_OrderLineId;
        private SimpleButton btn_Search;
        private GridControl grid_Main;
        private GridView gv_Main;

        public FrmProductionOrder()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "생산오더 조회";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 검색 영역
            var pnl = new PanelControl
            {
                Location = new Point(10, 10),
                Size = new Size(860, 45),
                BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
            };
            this.Controls.Add(pnl);

            pnl.Controls.Add(new LabelControl
            { Text = "오더라인ID", Location = new Point(10, 13) });

            txt_OrderLineId = new TextEdit
            { Location = new Point(85, 10), Size = new Size(150, 24) };
            pnl.Controls.Add(txt_OrderLineId);

            btn_Search = new SimpleButton
            {
                Text = "조회(F8)",
                Location = new Point(245, 8),
                Size = new Size(80, 28)
            };
            btn_Search.Click += (s, e) => SearchData();
            pnl.Controls.Add(btn_Search);

            // 그리드
            grid_Main = new GridControl
            { Location = new Point(10, 65), Size = new Size(860, 480) };
            gv_Main = new GridView();
            grid_Main.MainView = gv_Main;
            grid_Main.ViewCollection.AddRange(
                new DevExpress.XtraGrid.Views.Base.BaseView[] { gv_Main });

            gv_Main.OptionsView.ShowGroupPanel = false;
            gv_Main.OptionsBehavior.Editable = false;
            gv_Main.OptionsView.ColumnAutoWidth = false;

            // 5개 컬럼만
            AddCol("WORK_ORDER_NO", "작업오더번호", 150);
            AddCol("BOM_ITEM_CODE", "품목코드", 150);
            AddCol("BOM_ITEM_DESCRIPTION", "품목명", 250);
            AddCol("WORK_ORDER_QTY", "생산지시량", 100, "N0");
            AddCol("WORK_ORDER_NET_QTY", "순투입량", 80, "NO");
            AddCol("WORK_ORDER_STATUS_DESC", "상태", 80);
            AddCol("REMARK", "비고", 250);

            this.Controls.Add(grid_Main);
        }

        private void AddCol(string field, string caption, int width, string format = null)
        {
            var col = new GridColumn
            { FieldName = field, Caption = caption, Width = width, Visible = true };
            if (format != null)
                col.DisplayFormat.FormatString = format;
            gv_Main.Columns.Add(col);
        }

        private void SearchData()
        {
            if (string.IsNullOrWhiteSpace(txt_OrderLineId.EditValue?.ToString()))
            {
                XtraMessageBox.Show("오더라인ID를 입력하세요.", "확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                var dt = _dal.SelectList(70, 701,
                                   Convert.ToDecimal(txt_OrderLineId.EditValue));
                grid_Main.DataSource = dt;
                this.Text = $"생산오더 조회 - {dt.Rows.Count}건";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"조회 실패\n{ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8) { SearchData(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ResumeLayout(false);
        }
    }
}
