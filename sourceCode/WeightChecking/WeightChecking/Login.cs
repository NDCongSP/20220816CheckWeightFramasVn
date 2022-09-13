using Dapper;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BC = BCrypt.Net.BCrypt;

namespace WeightChecking
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        private Timer _timer = new Timer() { Interval = 100 };
        private bool _saveInfo = false;

        public Login()
        {
            InitializeComponent();

            Load += Login_Load;
            labStatus.Text = Application.ProductVersion;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (GlobalVariables.ReInfo.Remember)
            {
                this.txtUseName.Text = GlobalVariables.ReInfo.UserName;
                this.txtPass.Text = GlobalVariables.ReInfo.Pass;
                this.chkRemember.Checked = GlobalVariables.ReInfo.Remember;
            }
           
            this.txtUseName.Focus();
            this.chkRemember.CheckedChanged += (s, o) =>
            {
                CheckEdit ck = (CheckEdit)s;
                GlobalVariables.ReInfo.Remember = ck.Checked;
            };

            _timer.Enabled = true;
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Timer s = (Timer)sender;
            s.Enabled = false;
            labTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            s.Enabled = true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUseName.Text) && !string.IsNullOrEmpty(txtPass.Text))
            {
                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var para = new DynamicParameters();
                    para.Add("@userName", txtUseName.Text);

                    var result = connection.Query<tblUsers>("sp_UsersLogin", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (result != null)
                    {
                        if (BC.Verify(txtPass.Text, result.Password))
                        {
                            //log thong tin dang nhap vao rememberInfo
                            if (GlobalVariables.ReInfo.Remember)
                            {
                                GlobalVariables.ReInfo.UserName = EncodeMD5.EncryptString(txtUseName.Text, "ITFramasBDVN");
                                GlobalVariables.ReInfo.Pass = EncodeMD5.EncryptString(txtPass.Text, "ITFramasBDVN");

                                string json = JsonConvert.SerializeObject(GlobalVariables.ReInfo);

                                File.WriteAllText(@"./RememberInfo.json", json);
                            }
                            else
                            {
                                GlobalVariables.ReInfo.UserName = null;
                                GlobalVariables.ReInfo.Pass = null;

                                string json = JsonConvert.SerializeObject(GlobalVariables.ReInfo);

                                File.WriteAllText(@"./RememberInfo.json", json);
                            }

                            frmMain nf = new frmMain();
                            nf.ShowDialog();
                        }
                        else
                        {
                            XtraMessageBox.Show("Mật khẩu không chính xác.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Thông tin đăng nhập không chính xác.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("Nhập thiếu thông tin, vui lòng nhập lại đầy đủ thông tin.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}