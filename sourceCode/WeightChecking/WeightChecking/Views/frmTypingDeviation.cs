﻿using Dapper;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeightChecking
{
    public partial class frmTypingDeviation : DevExpress.XtraEditors.XtraForm
    {
        bool _isClickButton = false;
        public double ActualDeviation { get; set; } = 0;
        public Guid QrConfirm { get; set; }
        public string Reason { get; set; } = string.Empty;

        public frmTypingDeviation()
        {
            InitializeComponent();

            FormClosing += FrmTypingDeviation_FormClosing;
            this.btnSave.Click += BtnSave_Click;
            txtQR.KeyDown += TxtQR_KeyDown;

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

        private void TxtQR_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TextEdit _s = (TextEdit)sender;
                    QrConfirm = Guid.TryParse(_s.Text, out Guid value) ? value : Guid.Empty;

                    CheckCode();
                }
            }
            catch (Exception)
            {
                _isClickButton = false;
                MessageBox.Show($"Lỗi.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            CheckCode();
        }

        private void FrmTypingDeviation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isClickButton || Reason == string.Empty)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void CheckCode()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtQR.Text) && !string.IsNullOrEmpty(txtActualDeviation.Text) && !string.IsNullOrEmpty(Reason))
                {
                    using (var connection = GlobalVariables.GetDbConnection())
                    {
                        var para = new DynamicParameters();
                        para.Add("Id", QrConfirm);

                        var res = connection.Query<tblUsers>("sp_tblUserGet", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        if (res != null)
                        {
                            if (res.Approved == 1)
                            {
                                _isClickButton = true;
                                ActualDeviation = Math.Round(Convert.ToDouble(txtActualDeviation.Text), 2);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    txtQR.Text = string.Empty;
                                    txtQR.Focus();
                                });
                                MessageBox.Show("Bạn không có quyền thực hiện chức năng này", "THÔNG BÁO", MessageBoxButtons.OK
                                    , MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin.", "THÔNG BÁO", MessageBoxButtons.OK
                                , MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Thiếu thông tin, kiểm tra lại.", "THÔNG BÁO", MessageBoxButtons.OK
                                , MessageBoxIcon.Warning);
                    this.Invoke((MethodInvoker)delegate { txtActualDeviation.Focus(); });
                }
            }
            catch (Exception)
            {
                _isClickButton = false;
                MessageBox.Show($"Chỉ được nhập số, không nhập chữ ở đây.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                _isClickButton = false;
            }
        }
    }
}