
namespace WeightChecking
{
    partial class frmMasterData
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
            this.grc = new DevExpress.XtraGrid.GridControl();
            this.grv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.grcSpecialCase = new DevExpress.XtraGrid.GridControl();
            this.grvSpecialCase = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.grc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grcSpecialCase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvSpecialCase)).BeginInit();
            this.SuspendLayout();
            // 
            // grc
            // 
            this.grc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grc.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.grc.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grc.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.grc.Location = new System.Drawing.Point(0, 0);
            this.grc.MainView = this.grv;
            this.grc.Name = "grc";
            this.grc.Size = new System.Drawing.Size(1297, 786);
            this.grc.TabIndex = 0;
            this.grc.UseEmbeddedNavigator = true;
            this.grc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grv});
            // 
            // grv
            // 
            this.grv.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 10F);
            this.grv.Appearance.HeaderPanel.Options.UseFont = true;
            this.grv.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 10F);
            this.grv.Appearance.Row.Options.UseFont = true;
            this.grv.GridControl = this.grc;
            this.grv.Name = "grv";
            this.grv.OptionsBehavior.ReadOnly = true;
            this.grv.OptionsView.ColumnAutoWidth = false;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.xtraTabControl1.Appearance.Options.UseFont = true;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1299, 817);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Appearance.Header.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.xtraTabPage1.Appearance.Header.Options.UseFont = true;
            this.xtraTabPage1.Controls.Add(this.grc);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1297, 786);
            this.xtraTabPage1.Text = "Master Data";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Appearance.Header.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.xtraTabPage2.Appearance.Header.Options.UseFont = true;
            this.xtraTabPage2.Controls.Add(this.grcSpecialCase);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1297, 786);
            this.xtraTabPage2.Text = "Special Case";
            // 
            // grcSpecialCase
            // 
            this.grcSpecialCase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grcSpecialCase.Location = new System.Drawing.Point(0, 0);
            this.grcSpecialCase.MainView = this.grvSpecialCase;
            this.grcSpecialCase.Name = "grcSpecialCase";
            this.grcSpecialCase.Size = new System.Drawing.Size(1297, 786);
            this.grcSpecialCase.TabIndex = 0;
            this.grcSpecialCase.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvSpecialCase});
            // 
            // grvSpecialCase
            // 
            this.grvSpecialCase.GridControl = this.grcSpecialCase;
            this.grvSpecialCase.Name = "grvSpecialCase";
            this.grvSpecialCase.OptionsBehavior.ReadOnly = true;
            this.grvSpecialCase.OptionsView.ColumnAutoWidth = false;
            // 
            // frmMasterData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1299, 817);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "frmMasterData";
            this.Text = "Master Data";
            ((System.ComponentModel.ISupportInitialize)(this.grc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grcSpecialCase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvSpecialCase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grc;
        private DevExpress.XtraGrid.Views.Grid.GridView grv;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl grcSpecialCase;
        private DevExpress.XtraGrid.Views.Grid.GridView grvSpecialCase;
    }
}