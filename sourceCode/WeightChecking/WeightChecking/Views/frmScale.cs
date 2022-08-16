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
    public partial class frmScale : DevExpress.XtraEditors.XtraForm
    {
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
    }
}