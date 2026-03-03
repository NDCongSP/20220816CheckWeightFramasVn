
namespace WeightChecking
{
    partial class frmDeleteBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDeleteBox));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this._btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this._txtBoxId = new DevExpress.XtraEditors.TextEdit();
            this._txtOc = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this._txtUnit = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtBoxId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtOc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._btnDelete);
            this.layoutControl1.Controls.Add(this._txtBoxId);
            this.layoutControl1.Controls.Add(this._txtOc);
            this.layoutControl1.Controls.Add(this._txtUnit);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(-650, 377, 650, 400);
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(996, 94);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // _btnDelete
            // 
            this._btnDelete.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            this._btnDelete.Appearance.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this._btnDelete.Appearance.Options.UseBackColor = true;
            this._btnDelete.Appearance.Options.UseFont = true;
            this._btnDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("_btnDelete.ImageOptions.Image")));
            this._btnDelete.Location = new System.Drawing.Point(672, 12);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(312, 70);
            this._btnDelete.StyleController = this.layoutControl1;
            this._btnDelete.TabIndex = 9;
            this._btnDelete.Text = "Delete";
            // 
            // _txtBoxId
            // 
            this._txtBoxId.Location = new System.Drawing.Point(227, 34);
            this._txtBoxId.Name = "_txtBoxId";
            this._txtBoxId.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this._txtBoxId.Properties.Appearance.Options.UseFont = true;
            this._txtBoxId.Size = new System.Drawing.Size(251, 26);
            this._txtBoxId.StyleController = this.layoutControl1;
            this._txtBoxId.TabIndex = 11;
            // 
            // _txtOc
            // 
            this._txtOc.Location = new System.Drawing.Point(12, 34);
            this._txtOc.Name = "_txtOc";
            this._txtOc.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this._txtOc.Properties.Appearance.Options.UseFont = true;
            this._txtOc.Size = new System.Drawing.Size(211, 26);
            this._txtOc.StyleController = this.layoutControl1;
            this._txtOc.TabIndex = 12;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem3,
            this.layoutControlItem6});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(996, 94);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._btnDelete;
            this.layoutControlItem6.Location = new System.Drawing.Point(660, 0);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(133, 42);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(316, 74);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem7.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem7.Control = this._txtBoxId;
            this.layoutControlItem7.Location = new System.Drawing.Point(215, 0);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(132, 24);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(255, 74);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "Box Id:";
            this.layoutControlItem7.TextLocation = DevExpress.Utils.Locations.Top;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem8.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem8.Control = this._txtOc;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(132, 24);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(215, 74);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.Text = "Oc No:";
            this.layoutControlItem8.TextLocation = DevExpress.Utils.Locations.Top;
            // 
            // _txtUnit
            // 
            this._txtUnit.Location = new System.Drawing.Point(482, 34);
            this._txtUnit.Name = "_txtUnit";
            this._txtUnit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this._txtUnit.Properties.Appearance.Options.UseFont = true;
            this._txtUnit.Size = new System.Drawing.Size(186, 26);
            this._txtUnit.StyleController = this.layoutControl1;
            this._txtUnit.TabIndex = 14;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this._txtUnit;
            this.layoutControlItem3.Location = new System.Drawing.Point(470, 0);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(67, 52);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(190, 74);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Text = "Unit:";
            this.layoutControlItem3.TextLocation = DevExpress.Utils.Locations.Top;
            // 
            // frmDeleteBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 94);
            this.Controls.Add(this.layoutControl1);
            this.IconOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("frmDeleteBox.IconOptions.LargeImage")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDeleteBox";
            this.Text = "Delete Box";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._txtBoxId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtOc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._txtUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton _btnDelete;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.TextEdit _txtBoxId;
        private DevExpress.XtraEditors.TextEdit _txtOc;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.TextEdit _txtUnit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}