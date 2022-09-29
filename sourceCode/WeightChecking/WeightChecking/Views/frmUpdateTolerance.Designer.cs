
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;

namespace WeightChecking
{
    partial class frmUpdateTolerance
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.txtToleranceAfterPrint = new DevExpress.XtraEditors.TextEdit();
            this.txtToleranceBeforePrint = new DevExpress.XtraEditors.TextEdit();
            this.txtTolerance = new DevExpress.XtraEditors.TextEdit();
            this.labProductName = new DevExpress.XtraEditors.LabelControl();
            this.labProductCode = new DevExpress.XtraEditors.LabelControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtToleranceAfterPrint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToleranceBeforePrint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTolerance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnUpdate);
            this.layoutControl1.Controls.Add(this.txtToleranceAfterPrint);
            this.layoutControl1.Controls.Add(this.txtToleranceBeforePrint);
            this.layoutControl1.Controls.Add(this.txtTolerance);
            this.layoutControl1.Controls.Add(this.labProductName);
            this.layoutControl1.Controls.Add(this.labProductCode);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1457, 337);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btnUpdate.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.btnUpdate.Appearance.Options.UseBackColor = true;
            this.btnUpdate.Appearance.Options.UseFont = true;
            this.btnUpdate.Location = new System.Drawing.Point(12, 250);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(1433, 45);
            this.btnUpdate.StyleController = this.layoutControl1;
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtToleranceAfterPrint
            // 
            this.txtToleranceAfterPrint.Location = new System.Drawing.Point(400, 200);
            this.txtToleranceAfterPrint.Name = "txtToleranceAfterPrint";
            this.txtToleranceAfterPrint.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.txtToleranceAfterPrint.Properties.Appearance.Options.UseFont = true;
            this.txtToleranceAfterPrint.Size = new System.Drawing.Size(1045, 46);
            this.txtToleranceAfterPrint.StyleController = this.layoutControl1;
            this.txtToleranceAfterPrint.TabIndex = 8;
            // 
            // txtToleranceBeforePrint
            // 
            this.txtToleranceBeforePrint.Location = new System.Drawing.Point(400, 150);
            this.txtToleranceBeforePrint.Name = "txtToleranceBeforePrint";
            this.txtToleranceBeforePrint.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.txtToleranceBeforePrint.Properties.Appearance.Options.UseFont = true;
            this.txtToleranceBeforePrint.Size = new System.Drawing.Size(1045, 46);
            this.txtToleranceBeforePrint.StyleController = this.layoutControl1;
            this.txtToleranceBeforePrint.TabIndex = 7;
            // 
            // txtTolerance
            // 
            this.txtTolerance.Location = new System.Drawing.Point(400, 100);
            this.txtTolerance.Name = "txtTolerance";
            this.txtTolerance.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.txtTolerance.Properties.Appearance.Options.UseFont = true;
            this.txtTolerance.Size = new System.Drawing.Size(1045, 46);
            this.txtTolerance.StyleController = this.layoutControl1;
            this.txtTolerance.TabIndex = 6;
            // 
            // labProductName
            // 
            this.labProductName.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.labProductName.Appearance.Options.UseFont = true;
            this.labProductName.Location = new System.Drawing.Point(400, 56);
            this.labProductName.Name = "labProductName";
            this.labProductName.Size = new System.Drawing.Size(191, 40);
            this.labProductName.StyleController = this.layoutControl1;
            this.labProductName.TabIndex = 5;
            this.labProductName.Text = "labelControl1";
            // 
            // labProductCode
            // 
            this.labProductCode.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.labProductCode.Appearance.Options.UseFont = true;
            this.labProductCode.Location = new System.Drawing.Point(400, 12);
            this.labProductCode.Name = "labProductCode";
            this.labProductCode.Size = new System.Drawing.Size(191, 40);
            this.labProductCode.StyleController = this.layoutControl1;
            this.labProductCode.TabIndex = 4;
            this.labProductCode.Text = "labelControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1457, 337);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 25F);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.labProductCode;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1437, 44);
            this.layoutControlItem1.Text = "Product Code";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(376, 40);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 287);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(1437, 30);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 25F);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.labProductName;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 44);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1437, 44);
            this.layoutControlItem2.Text = "Product Name";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(376, 40);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 25F);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.txtTolerance;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 88);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(1437, 50);
            this.layoutControlItem3.Text = "Tolerance (g)";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(376, 40);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 25F);
            this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem4.Control = this.txtToleranceBeforePrint;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 138);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(1437, 50);
            this.layoutControlItem4.Text = "Tolerance Before Print (g)";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(376, 40);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 25F);
            this.layoutControlItem5.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem5.Control = this.txtToleranceAfterPrint;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 188);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(1437, 50);
            this.layoutControlItem5.Text = "Tolerance After Print (g)";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(376, 40);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnUpdate;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 238);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(1437, 49);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // frmUpdateTolerance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1457, 337);
            this.Controls.Add(this.layoutControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateTolerance";
            this.Text = "frmUpdateTolerance";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtToleranceAfterPrint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToleranceBeforePrint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTolerance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private SimpleButton btnUpdate;
        private TextEdit txtToleranceAfterPrint;
        private TextEdit txtToleranceBeforePrint;
        private TextEdit txtTolerance;
        private LabelControl labProductName;
        private LabelControl labProductCode;
        private LayoutControlItem layoutControlItem1;
        private EmptySpaceItem emptySpaceItem1;
        private LayoutControlItem layoutControlItem2;
        private LayoutControlItem layoutControlItem3;
        private LayoutControlItem layoutControlItem4;
        private LayoutControlItem layoutControlItem5;
        private LayoutControlItem layoutControlItem6;
    }
}