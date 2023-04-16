
namespace WeightChecking
{
    partial class frmDeviationForFalseAlarm
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtActualDeviation = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtActualDeviation.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 25F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(97, 27);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(423, 41);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "Số chênh lệch thực tế (đôi):";
            // 
            // btnSave
            // 
            this.btnSave.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 35F);
            this.btnSave.Appearance.Options.UseBackColor = true;
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(102, 175);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(412, 79);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "LƯU";
            // 
            // txtActualDeviation
            // 
            this.txtActualDeviation.EditValue = "";
            this.txtActualDeviation.Location = new System.Drawing.Point(102, 87);
            this.txtActualDeviation.Name = "txtActualDeviation";
            this.txtActualDeviation.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 35F);
            this.txtActualDeviation.Properties.Appearance.Options.UseFont = true;
            this.txtActualDeviation.Properties.Appearance.Options.UseTextOptions = true;
            this.txtActualDeviation.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtActualDeviation.Size = new System.Drawing.Size(412, 64);
            this.txtActualDeviation.TabIndex = 4;
            // 
            // frmDeviationForFalseAlarm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 307);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtActualDeviation);
            this.IconOptions.Image = global::WeightChecking.Properties.Resources.framas_mini__black_;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDeviationForFalseAlarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhập chênh lệch thực tế";
            ((System.ComponentModel.ISupportInitialize)(this.txtActualDeviation.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.TextEdit txtActualDeviation;
    }
}