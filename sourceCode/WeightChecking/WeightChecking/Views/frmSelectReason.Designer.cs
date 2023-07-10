
namespace WeightChecking
{
    partial class frmSelectReason
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
            this._btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this._ckOther = new DevExpress.XtraEditors.CheckEdit();
            this._ckWrongBox = new DevExpress.XtraEditors.CheckEdit();
            this._ckLackOfQty = new DevExpress.XtraEditors.CheckEdit();
            this._ckWrongArticle = new DevExpress.XtraEditors.CheckEdit();
            this._ckOverQty = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ckOther.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckWrongBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckLackOfQty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckWrongArticle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckOverQty.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnSave
            // 
            this._btnSave.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this._btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 35F);
            this._btnSave.Appearance.Options.UseBackColor = true;
            this._btnSave.Appearance.Options.UseFont = true;
            this._btnSave.Location = new System.Drawing.Point(458, 219);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(412, 79);
            this._btnSave.TabIndex = 3;
            this._btnSave.Text = "LƯU";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this._ckOther);
            this.groupControl1.Controls.Add(this._ckWrongBox);
            this.groupControl1.Controls.Add(this._ckLackOfQty);
            this.groupControl1.Controls.Add(this._ckWrongArticle);
            this.groupControl1.Controls.Add(this._ckOverQty);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(858, 192);
            this.groupControl1.TabIndex = 10;
            this.groupControl1.Text = "Danh sách lỗi";
            // 
            // _ckOther
            // 
            this._ckOther.Location = new System.Drawing.Point(557, 40);
            this._ckOther.Name = "_ckOther";
            this._ckOther.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this._ckOther.Properties.Appearance.Options.UseFont = true;
            this._ckOther.Properties.Caption = "Other (Lỗi khác)";
            this._ckOther.Size = new System.Drawing.Size(172, 28);
            this._ckOther.TabIndex = 0;
            // 
            // _ckWrongBox
            // 
            this._ckWrongBox.Location = new System.Drawing.Point(299, 113);
            this._ckWrongBox.Name = "_ckWrongBox";
            this._ckWrongBox.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this._ckWrongBox.Properties.Appearance.Options.UseFont = true;
            this._ckWrongBox.Properties.Caption = "Wrong Box";
            this._ckWrongBox.Size = new System.Drawing.Size(124, 28);
            this._ckWrongBox.TabIndex = 0;
            // 
            // _ckLackOfQty
            // 
            this._ckLackOfQty.Location = new System.Drawing.Point(32, 113);
            this._ckLackOfQty.Name = "_ckLackOfQty";
            this._ckLackOfQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this._ckLackOfQty.Properties.Appearance.Options.UseFont = true;
            this._ckLackOfQty.Properties.Caption = "Lack of Qty";
            this._ckLackOfQty.Size = new System.Drawing.Size(137, 28);
            this._ckLackOfQty.TabIndex = 0;
            // 
            // _ckWrongArticle
            // 
            this._ckWrongArticle.Location = new System.Drawing.Point(299, 40);
            this._ckWrongArticle.Name = "_ckWrongArticle";
            this._ckWrongArticle.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this._ckWrongArticle.Properties.Appearance.Options.UseFont = true;
            this._ckWrongArticle.Properties.Caption = "Wrong Article";
            this._ckWrongArticle.Size = new System.Drawing.Size(159, 28);
            this._ckWrongArticle.TabIndex = 0;
            // 
            // _ckOverQty
            // 
            this._ckOverQty.Location = new System.Drawing.Point(32, 40);
            this._ckOverQty.Name = "_ckOverQty";
            this._ckOverQty.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this._ckOverQty.Properties.Appearance.Options.UseFont = true;
            this._ckOverQty.Properties.Caption = "Over Qty";
            this._ckOverQty.Size = new System.Drawing.Size(111, 28);
            this._ckOverQty.TabIndex = 0;
            // 
            // frmSelectReason
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 314);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this._btnSave);
            this.IconOptions.Image = global::WeightChecking.Properties.Resources.framas_mini__black_;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectReason";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chọn lý do có sự chênh lệch";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ckOther.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckWrongBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckLackOfQty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckWrongArticle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ckOverQty.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton _btnSave;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit _ckOther;
        private DevExpress.XtraEditors.CheckEdit _ckWrongBox;
        private DevExpress.XtraEditors.CheckEdit _ckLackOfQty;
        private DevExpress.XtraEditors.CheckEdit _ckWrongArticle;
        private DevExpress.XtraEditors.CheckEdit _ckOverQty;
    }
}