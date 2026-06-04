
namespace ProductionOrder.Forms
{
    partial class FrmProductionOrder_new
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
            this.grid_Main = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnl_Search = new DevExpress.XtraEditors.PanelControl();
            this.lue_ItemId = new DevExpress.XtraEditors.LookUpEdit();
            this.lbl_Item = new DevExpress.XtraEditors.LabelControl();
            this.pnl_Toolbar = new DevExpress.XtraEditors.PanelControl();
            this.btn_New = new DevExpress.XtraEditors.SimpleButton();
            this.grp_Input = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Search)).BeginInit();
            this.pnl_Search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lue_ItemId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Toolbar)).BeginInit();
            this.pnl_Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grp_Input)).BeginInit();
            this.SuspendLayout();
            // 
            // grid_Main
            // 
            this.grid_Main.Location = new System.Drawing.Point(10, 125);
            this.grid_Main.MainView = this.gridView1;
            this.grid_Main.Name = "grid_Main";
            this.grid_Main.Size = new System.Drawing.Size(820, 720);
            this.grid_Main.TabIndex = 0;
            this.grid_Main.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.grid_Main;
            this.gridView1.Name = "gridView1";
            // 
            // pnl_Search
            // 
            this.pnl_Search.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnl_Search.Controls.Add(this.lue_ItemId);
            this.pnl_Search.Controls.Add(this.lbl_Item);
            this.pnl_Search.Location = new System.Drawing.Point(10, 10);
            this.pnl_Search.Name = "pnl_Search";
            this.pnl_Search.Size = new System.Drawing.Size(1360, 65);
            this.pnl_Search.TabIndex = 1;
            // 
            // lue_ItemId
            // 
            this.lue_ItemId.Location = new System.Drawing.Point(63, 14);
            this.lue_ItemId.Name = "lue_ItemId";
            this.lue_ItemId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lue_ItemId.Size = new System.Drawing.Size(200, 20);
            this.lue_ItemId.TabIndex = 1;
            // 
            // lbl_Item
            // 
            this.lbl_Item.Location = new System.Drawing.Point(21, 17);
            this.lbl_Item.Name = "lbl_Item";
            this.lbl_Item.Size = new System.Drawing.Size(20, 14);
            this.lbl_Item.TabIndex = 0;
            this.lbl_Item.Text = "품목";
            // 
            // pnl_Toolbar
            // 
            this.pnl_Toolbar.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnl_Toolbar.Controls.Add(this.btn_New);
            this.pnl_Toolbar.Location = new System.Drawing.Point(10, 80);
            this.pnl_Toolbar.Name = "pnl_Toolbar";
            this.pnl_Toolbar.Size = new System.Drawing.Size(1360, 36);
            this.pnl_Toolbar.TabIndex = 2;
            // 
            // btn_New
            // 
            this.btn_New.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btn_New.Appearance.Options.UseBackColor = true;
            this.btn_New.Location = new System.Drawing.Point(21, 10);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(75, 23);
            this.btn_New.TabIndex = 0;
            this.btn_New.Text = "simpleButton1";
            // 
            // grp_Input
            // 
            this.grp_Input.Location = new System.Drawing.Point(840, 125);
            this.grp_Input.Name = "grp_Input";
            this.grp_Input.Size = new System.Drawing.Size(530, 720);
            this.grp_Input.TabIndex = 3;
            this.grp_Input.Text = "생산지시 정보";
            // 
            // FrmProductionOrder_new
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1390, 868);
            this.Controls.Add(this.grp_Input);
            this.Controls.Add(this.pnl_Toolbar);
            this.Controls.Add(this.pnl_Search);
            this.Controls.Add(this.grid_Main);
            this.Name = "FrmProductionOrder_new";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "생산지시 관리";
            ((System.ComponentModel.ISupportInitialize)(this.grid_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Search)).EndInit();
            this.pnl_Search.ResumeLayout(false);
            this.pnl_Search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lue_ItemId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Toolbar)).EndInit();
            this.pnl_Toolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grp_Input)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grid_Main;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl pnl_Search;
        private DevExpress.XtraEditors.LookUpEdit lue_ItemId;
        private DevExpress.XtraEditors.LabelControl lbl_Item;
        private DevExpress.XtraEditors.PanelControl pnl_Toolbar;
        private DevExpress.XtraEditors.SimpleButton btn_New;
        private DevExpress.XtraEditors.GroupControl grp_Input;
    }
}