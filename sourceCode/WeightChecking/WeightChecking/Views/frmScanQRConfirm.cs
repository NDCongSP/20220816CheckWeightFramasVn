using Dapper;
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
    public partial class frmScanQRConfirm : DevExpress.XtraEditors.XtraForm
    {
        private bool _isOk = false;
        public Guid QrApproved { get; set; }

        public frmScanQRConfirm()
        {
            InitializeComponent();

            btnConfirm.Click += BtnConfirm_Click;
            this.FormClosing += FrmScanQRConfirm_FormClosing;
            this.txtQrCode.KeyDown += TxtQrCode_KeyDown;
        }

        private void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                TextEdit _sen = sender as TextEdit;

                QrApproved= Guid.TryParse(_sen.Text, out Guid valueD) ? valueD : Guid.Empty;

                CheckCode();
            }
        }

        private void FrmScanQRConfirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isOk)
            {
                this.DialogResult = DialogResult.Cancel;
            } 
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            CheckCode();
        }

        private void CheckCode()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtQrCode.Text))
                {
                    using (var connection = GlobalVariables.GetDbConnection())
                    {
                        var para = new DynamicParameters();
                        para.Add("Id", QrApproved);

                        var res = connection.Query<tblUsers>("sp_tblUserGet", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        if (res != null)
                        {
                            if (res.Approved == 1)
                            {
                                _isOk = true;
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
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
                    MessageBox.Show("Mã QR trống, mời quét lại.", "THÔNG BÁO", MessageBoxButtons.OK
                                , MessageBoxIcon.Warning);
                }
            }
            catch (Exception)
            {
                _isOk = false;
                MessageBox.Show($"Chỉ được nhập số, không nhập chữ ở đây.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}