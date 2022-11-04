using Dapper;
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
    public partial class frmConfirmPrint : Form
    {
        public ConfirmPrintModel ConfirmPrintInfo = new ConfirmPrintModel();
        public frmConfirmPrint()
        {
            InitializeComponent();
            Load += FrmConfirmPrint_Load;
            btnConfirm.Click += BtnConfirm_Click;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
           using(var connection = GlobalVariables.GetDbConnection())
            {
                var res = connection.Query<tblUsers>("").FirstOrDefault();
                if (res!=null)
                {

                }
            }
        }

        private void FrmConfirmPrint_Load(object sender, EventArgs e)
        {
            labIdLabel.Text = ConfirmPrintInfo.IdLabel;
            labOcNo.Text = ConfirmPrintInfo.OcNo;
            labBoxNo.Text = ConfirmPrintInfo.BoxNo;
            labWeight.Text = ConfirmPrintInfo.Weight.ToString();
        }
    }

    public class ConfirmPrintModel
    {
        public string IdLabel { get; set; } = null;
        public string OcNo { get; set; } = null;
        public string BoxNo { get; set; } = null;
        public double Weight { get; set; } = 0;
    }
}
