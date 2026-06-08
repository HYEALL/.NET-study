
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
            this.btn_Search = new DevExpress.XtraEditors.SimpleButton();
            this.txt_OrderLineId = new DevExpress.XtraEditors.TextEdit();
            this.lbl_OrderLineId = new DevExpress.XtraEditors.LabelControl();
            this.grid_Main = new DevExpress.XtraGrid.GridControl();
            this.gv_Main = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Search)).BeginInit();
            this.pnl_Search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_OrderLineId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_Search
            // 
            this.pnl_Search.Controls.Add(this.btn_Search);
            this.pnl_Search.Controls.Add(this.txt_OrderLineId);
            this.pnl_Search.Controls.Add(this.lbl_OrderLineId);
            this.pnl_Search.Location = new System.Drawing.Point(22, 25);
            this.pnl_Search.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnl_Search.Name = "pnl_Search";
            this.pnl_Search.Size = new System.Drawing.Size(1608, 114);
            this.pnl_Search.TabIndex = 0;
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(397, 25);
            this.btn_Search.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(139, 48);
            this.btn_Search.TabIndex = 2;
            this.btn_Search.Text = "조회(F8)";
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // txt_OrderLineId
            // 
            this.txt_OrderLineId.Location = new System.Drawing.Point(180, 31);
            this.txt_OrderLineId.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_OrderLineId.Name = "txt_OrderLineId";
            this.txt_OrderLineId.Size = new System.Drawing.Size(186, 44);
            this.txt_OrderLineId.TabIndex = 1;
            // 
            // lbl_OrderLineId
            // 
            this.lbl_OrderLineId.Location = new System.Drawing.Point(48, 37);
            this.lbl_OrderLineId.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lbl_OrderLineId.Name = "lbl_OrderLineId";
            this.lbl_OrderLineId.Size = new System.Drawing.Size(109, 29);
            this.lbl_OrderLineId.TabIndex = 0;
            this.lbl_OrderLineId.Text = "오더라인ID";
            // 
            // grid_Main
            // 
            this.grid_Main.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grid_Main.Location = new System.Drawing.Point(22, 294);
            this.grid_Main.MainView = this.gv_Main;
            this.grid_Main.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grid_Main.Name = "grid_Main";
            this.grid_Main.Size = new System.Drawing.Size(1608, 414);
            this.grid_Main.TabIndex = 1;
            this.grid_Main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_Main});
            // 
            // gv_Main
            // 
            this.gv_Main.DetailHeight = 725;
            this.gv_Main.GridControl = this.grid_Main;
            this.gv_Main.Name = "gv_Main";
            this.gv_Main.OptionsEditForm.PopupEditFormWidth = 1486;
            // 
            // FrmWorkOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1653, 1177);
            this.Controls.Add(this.grid_Main);
            this.Controls.Add(this.pnl_Search);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmWorkOrder";
            this.Text = "생산오더조회";
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Search)).EndInit();
            this.pnl_Search.ResumeLayout(false);
            this.pnl_Search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_OrderLineId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Main)).EndInit();
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