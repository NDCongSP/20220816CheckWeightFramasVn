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
    public partial class frmDeviationForFalseAlarm : DevExpress.XtraEditors.XtraForm
    {
        bool _isClickButton = false;
        public double ActualDeviation { get; set; } = 0;
        public frmDeviationForFalseAlarm()
        {
            InitializeComponent();
            FormClosing += FrmDeviationForFalseAlarm_FormClosing;
            this.btnSave.Click += BtnSave_Click;
            txtActualDeviation.TextChanged += TxtActualDeviation_TextChanged;
        }

        private void TxtActualDeviation_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextEdit t = (TextEdit)sender;

                if (!string.IsNullOrEmpty(t.Text))
                {
                    ActualDeviation = double.TryParse(t.Text, out double value) ? value : 0;
                }
                else
                {
                    MessageBox.Show("Thiếu thông tin, kiểm tra lại.", "THÔNG BÁO", MessageBoxButtons.OK
                                , MessageBoxIcon.Warning);
                    this.Invoke((MethodInvoker)delegate { t.Focus(); });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            _isClickButton = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FrmDeviationForFalseAlarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isClickButton)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}