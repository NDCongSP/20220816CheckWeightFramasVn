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
using BC = BCrypt.Net.BCrypt;

namespace WeightChecking
{
    public partial class frmConfirmPrint : Form
    {
        public ConfirmPrintModel ConfirmPrintInfo = new ConfirmPrintModel();
        string _code = null;
        public frmConfirmPrint()
        {
            InitializeComponent();
            Load += FrmConfirmPrint_Load;
            btnConfirm.Click += BtnConfirm_Click;
            txtQrCode.KeyDown += TxtQrCode_KeyDown;
        }

        private void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckingInfoUpdate();
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            CheckingInfoUpdate();
        }

        private void CheckingInfoUpdate()
        {
            try
            {
                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var para = new DynamicParameters();
                    para.Add("Id", txtQrCode.Text);

                    var res = connection.Query<tblUsers>("sp_tblUserGet", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (res != null)
                    {
                        if (res.Approved == 1)
                        {
                            GlobalVariables.Printing((GlobalVariables.RealWeight / 1000).ToString("#,#0.00")
                                                   , !string.IsNullOrEmpty(GlobalVariables.IdLabel) ? GlobalVariables.IdLabel : $"{GlobalVariables.OcNo}|{GlobalVariables.BoxNo}", true
                                                   ,GlobalVariables.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss"));

                            #region Log
                            para = null;
                            para = new DynamicParameters();
                            para.Add("QrCode", txtQrCode.Text);
                            para.Add("IdLabel",GlobalVariables.IdLabel);
                            para.Add("OC",GlobalVariables.OcNo);
                            para.Add("BoxNo",GlobalVariables.BoxNo);
                            para.Add("GrossWeight", (GlobalVariables.RealWeight / 1000).ToString("#,#0.00"));
                            para.Add("Station", GlobalVariables.Station);

                            connection.Execute("sp_tblApprovedPrintLabelInsert", para, commandType: CommandType.StoredProcedure);
                            #endregion
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Close();
            }
        }

        private void FrmConfirmPrint_Load(object sender, EventArgs e)
        {
            labIdLabel.Text = GlobalVariables.IdLabel;
            labOcNo.Text = GlobalVariables.OcNo;
            labBoxNo.Text = GlobalVariables.BoxNo;
            labWeight.Text = (GlobalVariables.RealWeight / 1000).ToString("#,#0.00");
        }
    }

    public class ConfirmPrintModel
    {
        public string IdLabel { get; set; } = null;
        public string OcNo { get; set; } = null;
        public string BoxNo { get; set; } = null;
        public double Weight { get; set; } = 0;
        public string CreatedDate { get; set; }
    }
}
