
namespace WeightChecking
{
    partial class frmScanQRConfirm
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
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.labQrCode = new DevExpress.XtraEditors.LabelControl();
            this.txtQrCode = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQrCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.btnConfirm.Appearance.Options.UseBackColor = true;
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.Location = new System.Drawing.Point(12, 84);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(738, 51);
            this.btnConfirm.TabIndex = 6;
            this.btnConfirm.Text = "Confirm";
            // 
            // labQrCode
            // 
            this.labQrCode.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labQrCode.Appearance.Options.UseFont = true;
            this.labQrCode.Location = new System.Drawing.Point(13, 5);
            this.labQrCode.Name = "labQrCode";
            this.labQrCode.Size = new System.Drawing.Size(171, 24);
            this.labQrCode.TabIndex = 4;
            this.labQrCode.Text = "Scan QR Xác Nhận:";
            // 
            // txtQrCode
            // 
            this.txtQrCode.Location = new System.Drawing.Point(12, 38);
            this.txtQrCode.Name = "txtQrCode";
            this.txtQrCode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.txtQrCode.Properties.Appearance.Options.UseFont = true;
            this.txtQrCode.Properties.AutoHeight = false;
            this.txtQrCode.Properties.PasswordChar = '*';
            this.txtQrCode.Properties.UseSystemPasswordChar = true;
            this.txtQrCode.Size = new System.Drawing.Size(738, 40);
            this.txtQrCode.TabIndex = 5;
            // 
            // frmScanQRConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 166);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.labQrCode);
            this.Controls.Add(this.txtQrCode);
            this.IconOptions.Image = global::WeightChecking.Properties.Resources.framas_mini__black_;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScanQRConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quét QR Xác Nhận Cảnh Báo Sai - In Lại Tem";
            ((System.ComponentModel.ISupportInitialize)(this.txtQrCode.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.LabelControl labQrCode;
        private DevExpress.XtraEditors.TextEdit txtQrCode;
    }
}