
namespace WeightChecking
{
    partial class frmTypingDeviation
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
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtActualDeviation = new DevExpress.XtraEditors.TextEdit();
            this.txtQR = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtActualDeviation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQR.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 35F);
            this.btnSave.Appearance.Options.UseBackColor = true;
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(458, 261);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(412, 79);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "LƯU";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(29, 34);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(423, 41);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Số chênh lệch thực tế (đôi):";
            // 
            // txtActualDeviation
            // 
            this.txtActualDeviation.EditValue = "0";
            this.txtActualDeviation.Location = new System.Drawing.Point(458, 17);
            this.txtActualDeviation.Name = "txtActualDeviation";
            this.txtActualDeviation.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 35F);
            this.txtActualDeviation.Properties.Appearance.Options.UseFont = true;
            this.txtActualDeviation.Properties.Appearance.Options.UseTextOptions = true;
            this.txtActualDeviation.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtActualDeviation.Size = new System.Drawing.Size(412, 64);
            this.txtActualDeviation.TabIndex = 1;
            // 
            // txtQR
            // 
            this.txtQR.EditValue = "";
            this.txtQR.Location = new System.Drawing.Point(20, 173);
            this.txtQR.Name = "txtQR";
            this.txtQR.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 35F);
            this.txtQR.Properties.Appearance.Options.UseFont = true;
            this.txtQR.Size = new System.Drawing.Size(850, 64);
            this.txtQR.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(29, 126);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(260, 41);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Mã QR xác nhận:";
            // 
            // frmTypingDeviation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 376);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtQR);
            this.Controls.Add(this.txtActualDeviation);
            this.IconOptions.Image = global::WeightChecking.Properties.Resources.framas_mini__black_;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTypingDeviation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhập số đôi chênh lệch thực tế";
            ((System.ComponentModel.ISupportInitialize)(this.txtActualDeviation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQR.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtActualDeviation;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtQR;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}