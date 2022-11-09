
namespace WeightChecking
{
    partial class frmReports
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
            this.grcReports = new DevExpress.XtraGrid.GridControl();
            this.grvReports = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.grcReports)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvReports)).BeginInit();
            this.SuspendLayout();
            // 
            // grcReports
            // 
            this.grcReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grcReports.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.grcReports.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grcReports.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.grcReports.Location = new System.Drawing.Point(0, 0);
            this.grcReports.MainView = this.grvReports;
            this.grcReports.Name = "grcReports";
            this.grcReports.Size = new System.Drawing.Size(1410, 785);
            this.grcReports.TabIndex = 0;
            this.grcReports.UseEmbeddedNavigator = true;
            this.grcReports.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvReports});
            // 
            // grvReports
            // 
            this.grvReports.GridControl = this.grcReports;
            this.grvReports.Name = "grvReports";
            this.grvReports.OptionsDetail.ShowEmbeddedDetailIndent = DevExpress.Utils.DefaultBoolean.False;
            // 
            // frmReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1410, 785);
            this.Controls.Add(this.grcReports);
            this.Name = "frmReports";
            this.Text = "Report";
            ((System.ComponentModel.ISupportInitialize)(this.grcReports)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvReports)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grcReports;
        private DevExpress.XtraGrid.Views.Grid.GridView grvReports;
    }
}