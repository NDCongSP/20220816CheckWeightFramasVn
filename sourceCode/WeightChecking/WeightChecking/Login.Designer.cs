
namespace WeightChecking
{
    partial class Login
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.chkRemember = new DevExpress.XtraEditors.CheckEdit();
            this.txtPass = new DevExpress.XtraEditors.TextEdit();
            this.txtUseName = new DevExpress.XtraEditors.TextEdit();
            this.labPass = new DevExpress.XtraEditors.LabelControl();
            this.labUserName = new DevExpress.XtraEditors.LabelControl();
            this.labStatus = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labTime = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemember.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUseName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.chkRemember);
            this.panelControl1.Controls.Add(this.txtPass);
            this.panelControl1.Controls.Add(this.txtUseName);
            this.panelControl1.Controls.Add(this.labPass);
            this.panelControl1.Controls.Add(this.labUserName);
            this.panelControl1.Location = new System.Drawing.Point(225, 165);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(520, 355);
            this.panelControl1.TabIndex = 0;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(179, 274);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(301, 60);
            this.simpleButton1.TabIndex = 3;
            this.simpleButton1.Text = "Submit";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // chkRemember
            // 
            this.chkRemember.Location = new System.Drawing.Point(357, 224);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.chkRemember.Properties.Appearance.Options.UseFont = true;
            this.chkRemember.Properties.Caption = "Remember";
            this.chkRemember.Size = new System.Drawing.Size(123, 28);
            this.chkRemember.TabIndex = 2;
            // 
            // txtPass
            // 
            this.txtPass.EditValue = "";
            this.txtPass.Location = new System.Drawing.Point(179, 152);
            this.txtPass.Name = "txtPass";
            this.txtPass.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtPass.Properties.Appearance.Options.UseFont = true;
            this.txtPass.Properties.NullText = "UserName";
            this.txtPass.Size = new System.Drawing.Size(301, 54);
            this.txtPass.TabIndex = 1;
            // 
            // txtUseName
            // 
            this.txtUseName.EditValue = "";
            this.txtUseName.Location = new System.Drawing.Point(179, 69);
            this.txtUseName.Name = "txtUseName";
            this.txtUseName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtUseName.Properties.Appearance.Options.UseFont = true;
            this.txtUseName.Properties.NullText = "UserName";
            this.txtUseName.Size = new System.Drawing.Size(301, 54);
            this.txtUseName.TabIndex = 1;
            // 
            // labPass
            // 
            this.labPass.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labPass.Appearance.Options.UseFont = true;
            this.labPass.Location = new System.Drawing.Point(30, 155);
            this.labPass.Name = "labPass";
            this.labPass.Size = new System.Drawing.Size(84, 24);
            this.labPass.TabIndex = 0;
            this.labPass.Text = "Password";
            // 
            // labUserName
            // 
            this.labUserName.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labUserName.Appearance.Options.UseFont = true;
            this.labUserName.Location = new System.Drawing.Point(30, 72);
            this.labUserName.Name = "labUserName";
            this.labUserName.Size = new System.Drawing.Size(96, 24);
            this.labUserName.TabIndex = 0;
            this.labUserName.Text = "User name";
            // 
            // labStatus
            // 
            this.labStatus.Location = new System.Drawing.Point(925, 711);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(63, 13);
            this.labStatus.TabIndex = 1;
            this.labStatus.Text = "labelControl1";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 50F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(190, 68);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(590, 81);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "WEIGHT CHECKING";
            // 
            // labTime
            // 
            this.labTime.Location = new System.Drawing.Point(24, 709);
            this.labTime.Name = "labTime";
            this.labTime.Size = new System.Drawing.Size(63, 13);
            this.labTime.TabIndex = 3;
            this.labTime.Text = "labelControl2";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 736);
            this.Controls.Add(this.labTime);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labStatus);
            this.Controls.Add(this.panelControl1);
            this.DoubleBuffered = true;
            this.Name = "Login";
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemember.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUseName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.CheckEdit chkRemember;
        private DevExpress.XtraEditors.TextEdit txtPass;
        private DevExpress.XtraEditors.TextEdit txtUseName;
        private DevExpress.XtraEditors.LabelControl labPass;
        private DevExpress.XtraEditors.LabelControl labUserName;
        private DevExpress.XtraEditors.LabelControl labStatus;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labTime;
    }
}