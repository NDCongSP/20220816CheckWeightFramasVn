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
        int _actualDeviatio = 0;
        Regex _regex = new Regex(@"^[A-Z0-9]\d{2}[A-Z0-9](-\d{3}){2}[A-Z0-9]$", RegexOptions.IgnoreCase);

        public frmTypingDeviation()
        {
            InitializeComponent();

            FormClosing += FrmTypingDeviation_FormClosing;
            this.btnSave.Click += BtnSave_Click;
        }

        public frmTypingDeviation(ref int actualDeviation)
        {
            _actualDeviatio = actualDeviation;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            
            var match = _regex.Match(txtActualDeviation.Text);
            if (match.Success)
            {
                _isClickButton = true;
            }
            else
            {
                _isClickButton = false;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FrmTypingDeviation_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}