using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeightChecking
{
    public partial class frmSelectReason : DevExpress.XtraEditors.XtraForm
    {
        public string Reason { get; set; } = string.Empty;
        bool _isClickButton = false;

        public frmSelectReason()
        {
            InitializeComponent();

            this.FormClosing += FrmSelectReason_FormClosing;
            _btnSave.Click += _btnSave_Click;

            #region Check reason
            _ckOverQty.CheckedChanged += (s, o) =>
            {
                if (_ckOverQty.Checked)
                {
                    Reason = "Over Quantity";

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            _ckLackOfQty.Checked = false;
                            _ckWrongArticle.Checked = false;
                            _ckWrongBox.Checked = false;
                            _ckOther.Checked = false;
                        }));
                    }
                }
                else
                {
                    Reason = string.Empty;
                }
            };

            _ckLackOfQty.CheckedChanged += (s, o) =>
            {
                if (_ckLackOfQty.Checked)
                {
                    Reason = "Lack Of Quantity";

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            _ckOverQty.Checked = false;
                            _ckWrongArticle.Checked = false;
                            _ckWrongBox.Checked = false;
                            _ckOther.Checked = false;
                        }));
                    }
                }
                else
                {
                    Reason = string.Empty;
                }
            };

            _ckWrongArticle.CheckedChanged += (s, o) =>
            {
                if (_ckWrongArticle.Checked)
                {
                    Reason = "Wrong Article";

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            _ckOverQty.Checked = false;
                            _ckLackOfQty.Checked = false;
                            _ckWrongBox.Checked = false;
                            _ckOther.Checked = false;
                        }));
                    }
                }
                else
                {
                    Reason = string.Empty;
                }
            };

            _ckWrongBox.CheckedChanged += (s, o) =>
            {
                if (_ckWrongBox.Checked)
                {
                    Reason = "Wrong Box";

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            _ckOverQty.Checked = false;
                            _ckWrongArticle.Checked = false;
                            _ckLackOfQty.Checked = false;
                            _ckOther.Checked = false;
                        }));
                    }
                }
                else
                {
                    Reason = string.Empty;
                }
            };

            _ckOther.CheckedChanged += (s, o) =>
            {
                if (_ckOther.Checked)
                {
                    Reason = "Other";

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            _ckOverQty.Checked = false;
                            _ckWrongArticle.Checked = false;
                            _ckWrongBox.Checked = false;
                            _ckLackOfQty.Checked = false;
                        }));
                    }
                }
                else
                {
                    Reason = string.Empty;
                }
            };
            #endregion
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (Reason == string.Empty)
            {
                MessageBox.Show($"Bạn chưa chọn lý do. Mời quét tem lại.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _isClickButton = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FrmSelectReason_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Reason == string.Empty)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}