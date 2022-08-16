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
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
       private Timer _timer = new Timer() { Interval = 100 };

        public Login()
        {
            InitializeComponent();
   
            Load += Login_Load;
            labStatus.Text = Application.ProductVersion;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.txtUseName.Focus();
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmMain nf = new frmMain();
            nf.ShowDialog();
        }
    }
}