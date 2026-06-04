
namespace MES.Forms
{
    partial class FrmWorkOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_Search = new DevExpress.XtraEditors.PanelControl();
            this.grid_Main = new DevExpress.XtraGrid.GridControl();
            this.gv_Main = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lbl_OrderLineId = new DevExpress.XtraEditors.LabelControl();
            this.txt_OrderLineId = new DevExpress.XtraEditors.TextEdit();
            this.btn_Search = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Search)).BeginInit();
            this.pnl_Search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_OrderLineId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_Search
            // 
            this.pnl_Search.Controls.Add(this.btn_Search);
            this.pnl_Search.Controls.Add(this.txt_OrderLineId);
            this.pnl_Search.Controls.Add(this.lbl_OrderLineId);
            this.pnl_Search.Location = new System.Drawing.Point(12, 12);
            this.pnl_Search.Name = "pnl_Search";
            this.pnl_Search.Size = new System.Drawing.Size(866, 55);
            this.pnl_Search.TabIndex = 0;
            // 
            // grid_Main
            // 
            this.grid_Main.Location = new System.Drawing.Point(12, 142);
            this.grid_Main.MainView = this.gv_Main;
            this.grid_Main.Name = "grid_Main";
            this.grid_Main.Size = new System.Drawing.Size(400, 200);
            this.grid_Main.TabIndex = 1;
            this.grid_Main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_Main});
            // 
            // gv_Main
            // 
            this.gv_Main.GridControl = this.grid_Main;
            this.gv_Main.Name = "gv_Main";
            // 
            // lbl_OrderLineId
            // 
            this.lbl_OrderLineId.Location = new System.Drawing.Point(26, 18);
            this.lbl_OrderLineId.Name = "lbl_OrderLineId";
            this.lbl_OrderLineId.Size = new System.Drawing.Size(52, 14);
            this.lbl_OrderLineId.TabIndex = 0;
            this.lbl_OrderLineId.Text = "오더라인ID";
            // 
            // txt_OrderLineId
            // 
            this.txt_OrderLineId.Location = new System.Drawing.Point(97, 15);
            this.txt_OrderLineId.Name = "txt_OrderLineId";
            this.txt_OrderLineId.Size = new System.Drawing.Size(100, 20);
            this.txt_OrderLineId.TabIndex = 1;
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(214, 12);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(75, 23);
            this.btn_Search.TabIndex = 2;
            this.btn_Search.Text = "조회(F8)";
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // FrmWorkOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 568);
            this.Controls.Add(this.grid_Main);
            this.Controls.Add(this.pnl_Search);
            this.Name = "FrmWorkOrder";
            this.Text = "생산오더조회";
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Search)).EndInit();
            this.pnl_Search.ResumeLayout(false);
            this.pnl_Search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_OrderLineId.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnl_Search;
        private DevExpress.XtraGrid.GridControl grid_Main;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_Main;
        private DevExpress.XtraEditors.SimpleButton btn_Search;
        private DevExpress.XtraEditors.TextEdit txt_OrderLineId;
        private DevExpress.XtraEditors.LabelControl lbl_OrderLineId;
    }
}