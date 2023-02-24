
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
            this.txtActualDeviation = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtActualDeviation.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtActualDeviation
            // 
            this.txtActualDeviation.EditValue = "";
            this.txtActualDeviation.Location = new System.Drawing.Point(29, 81);
            this.txtActualDeviation.Name = "txtActualDeviation";
            this.txtActualDeviation.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 55F);
            this.txtActualDeviation.Properties.Appearance.Options.UseFont = true;
            this.txtActualDeviation.Properties.Appearance.Options.UseTextOptions = true;
            this.txtActualDeviation.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtActualDeviation.Size = new System.Drawing.Size(443, 94);
            this.txtActualDeviation.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 55F);
            this.btnSave.Appearance.Options.UseBackColor = true;
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(225, 192);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(247, 79);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "LƯU";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(29, 34);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(329, 41);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Số chênh lệch thực tế";
            // 
            // frmTypingDeviation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 303);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtActualDeviation);
            this.IconOptions.Image = global::WeightChecking.Properties.Resources.framas_mini__black_;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTypingDeviation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhập số đôi chênh lệch thực tế";
            ((System.ComponentModel.ISupportInitialize)(this.txtActualDeviation.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtActualDeviation;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}