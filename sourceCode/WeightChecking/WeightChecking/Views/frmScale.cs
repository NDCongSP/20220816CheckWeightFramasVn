using DevExpress.XtraEditors;
using Serilog;
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
    public partial class frmScale : DevExpress.XtraEditors.XtraForm
    {
        ScaleHelper _scaleHelper;
        private Task _ckTask;

        public frmScale()
        {
            InitializeComponent();

            Load += FrmScale_Load;
        }

        private void FrmScale_Load(object sender, EventArgs e)
        {
            this.txtQrCode.Text = "aaa";
            this.txtQrCode.Focus();
            this.txtQrCode.KeyDown += TxtQrCode_KeyDown;

            #region Register events Scale value change
            _scaleHelper = new ScaleHelper()
            {
                Ip = GlobalVariables.IpScale,
                Port = Convert.ToInt32(GlobalVariables.PortScale),
                ScaleDelay = GlobalVariables.ScaleDelay,
                StopScale = false
            };

            _scaleHelper.StatusChanged += (s, o) =>
            {
                GlobalVariables.ScaleStatus = o.StatusConnection;
                Console.WriteLine($"Scale {o}");
            };

            _ckTask = new Task(() => _scaleHelper.CheckConnect());
            _ckTask.Start();

            _scaleHelper.ValueChanged += (s, o) =>
            {
                try
                {
                    if (labWeight.InvokeRequired)
                    {
                        labWeight.Invoke(new Action(() =>
                        {
                            labWeight.Text = (o.Value * GlobalVariables.UnitScale).ToString();
                        }));
                    }
                    else
                    {
                        labWeight.Text = o.Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Scale event error.");
                }
            };
            _scaleHelper.ScaleValue = 1;//tac động để đọc cân lần đầu tiên
            #endregion
        }

        private void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextEdit sen = sender as TextEdit;
                Console.WriteLine(sen.Text);

                if (sen.InvokeRequired)
                {
                    sen?.Invoke(new Action(() =>
                    {
                        sen.Text = null;
                    }));
                }
                else
                {
                    sen.Text = null;
                }
                sen.Focus();
            }
        }

        private void frmScale_FormClosing(object sender, FormClosingEventArgs e)
        {
            //huy doi tuong can
            _scaleHelper.StopScale = true;
            _ckTask.Wait();
            _ckTask.Dispose();
            _scaleHelper.Dispose();
            GlobalVariables.ScaleStatus = "Disconnect";
        }
    }
}