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
using BC = BCrypt.Net.BCrypt;

namespace WeightChecking
{
    public partial class frmConfirmPrint : Form
    {
        public ConfirmPrintModel ConfirmPrintInfo = new ConfirmPrintModel();
        string _code = null;
        Timer _timer = new Timer();
        int _checkCount = 0;//đếm số lần scan QR code. ban đầu vào scan QR code label, sau đó scan QR code approve. rồi mới cho in lại tem
        tblScanDataModel _scanData = new tblScanDataModel();
        double _scaleValue = 0;
        Guid _qrApproved;
        double _actualDeviation = 0;
        double _ratio = 0;

        public frmConfirmPrint()
        {
            InitializeComponent();
            Load += FrmConfirmPrint_Load;
            btnConfirm.Click += BtnConfirm_Click;
            txtQrCode.KeyDown += TxtQrCode_KeyDown;
            btnConfirm.Visible = false;
        }

        private void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            TextEdit _sen = sender as TextEdit;

            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (_checkCount == 0)
                    {
                        _scanData.BarcodeString = _sen.Text;

                        #region Get thông tin của thùng trong bảng ScanData
                        using (var connection = GlobalVariables.GetDbConnection())
                        {
                            var para = new DynamicParameters();
                            para = null;
                            para = new DynamicParameters();
                            para.Add("QRLabel", _scanData.BarcodeString);

                            var resultData = connection.Query<tblScanDataModel>("sp_tblScanDataGetForApprovedPrint", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

                            if (resultData != null)
                            {
                                _scanData = resultData;
                                //tính tỷ lệ
                                _ratio = Math.Round((Math.Abs(_scanData.DeviationPairs) * _scanData.AveWeight1Prs) / _scanData.StdGrossWeight, 3);

                                //nếu tỷ lệ lớn hơn quy định thì popup form nhập deviation thực tế
                                if (GlobalVariables.Station != StationEnum.IDC_2
                                    && (GlobalVariables.Station == StationEnum.IDC_1 && _scanData.OcNo.Substring(0, 2) != "PR"))
                                {
                                    using (var formDeviation = new frmDeviationForFalseAlarm())
                                    {
                                        var resultForm = formDeviation.ShowDialog();

                                        if (resultForm == DialogResult.OK)
                                        {
                                            _scanData.ActualDeviationPairs = formDeviation.ActualDeviation;
                                        }
                                        else
                                        {
                                            MessageBox.Show($"Bạn chưa nhập chênh lệch thực tế cho thùng này. Mời quét tem lại.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            goto returnLoop;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Thùng này không đươc phép dùng tính năng này, do thùng đã Pass hoặc chưa được cân lần nào.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                goto returnLoop;
                            }
                        }
                        #endregion

                        #region show info labels
                        this.Invoke((MethodInvoker)delegate
                        {
                            labProductCode.Text = !string.IsNullOrEmpty(_scanData.ProductNumber) ? _scanData.ProductNumber : string.Empty;
                            labIdLabel.Text = !string.IsNullOrEmpty(_scanData.IdLabel) ? _scanData.IdLabel : string.Empty;
                            labOcNo.Text = !string.IsNullOrEmpty(_scanData.OcNo) ? _scanData.OcNo : string.Empty;
                            labBoxNo.Text = !string.IsNullOrEmpty(_scanData.BoxNo) ? _scanData.BoxNo : string.Empty;
                            labQty.Text = _scanData.Quantity.ToString();
                            labQrCode.Text = "Scan QR Code Approved:";
                        });
                        #endregion

                        _checkCount = 1;
                    }
                    else if (_checkCount == 1)
                    {
                        _qrApproved = Guid.TryParse(_sen.Text, out Guid valueD) ? valueD : Guid.Empty;

                        CheckingInfoUpdate();

                        _checkCount = 0;
                    }

                returnLoop:
                    #region reset txtQrcode để quét mã tiếp
                    _sen.Text = null;
                    _sen.Focus();
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Quét sai mã QR. Mời quét lại.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _sen.Text = null;
                    _sen.Focus();
                }
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
                    para.Add("Id", _qrApproved);

                    var res = connection.Query<tblUsers>("sp_tblUserGet", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (res != null)
                    {
                        if (res.Approved == 1)
                        {
                            var dialogResult = MessageBox.Show($"Bạn có chắc chắn xác nhận thùng với thông tin sau:" +
                                     $"{Environment.NewLine}{_scanData.IdLabel}|{_scanData.OcNo}|{_scanData.BoxNo}{Environment.NewLine}" +
                                     $" là cảnh báo sai và in lại tem với chênh lệch thực tế là {_scanData.ActualDeviationPairs}?", "CẢNH BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (dialogResult == DialogResult.Yes)
                            {
                                para = null;
                                para = new DynamicParameters();
                                para.Add("Id", _scanData.Id);
                                para.Add("ApproveBy", _qrApproved);
                                para.Add("ActualDeviationPairs", _scanData.ActualDeviationPairs);
                                para.Add("GrossWeight", _scaleValue);

                                connection.Execute("sp_tblScanDataUpdateApproveBy", para, commandType: CommandType.StoredProcedure);

                                #region Log
                                para = null;
                                para = new DynamicParameters();
                                para.Add("QrCode", _qrApproved);
                                para.Add("IdLabel", _scanData.IdLabel);
                                para.Add("OC", _scanData.OcNo);
                                para.Add("BoxNo", _scanData.BoxNo);
                                para.Add("GrossWeight", _scanData.GrossWeight);
                                para.Add("Station", GlobalVariables.Station);
                                para.Add("QRLabel", _scanData.BarcodeString);
                                para.Add("ApproveType", _scanData.ActualDeviationPairs == 0 ? "False alarm" : "Actual deviation");
                                para.Add("CalculatorDeviationPairs", _scanData.DeviationPairs);
                                para.Add("ActualDeviationPairs", _scanData.ActualDeviationPairs);

                                connection.Execute("sp_tblApprovedPrintLabelInsert", para, commandType: CommandType.StoredProcedure);
                                #endregion

                                GlobalVariables.Printing((_scaleValue / 1000).ToString("#,#0.00")
                                          , !string.IsNullOrEmpty(_scanData.IdLabel) ? _scanData.IdLabel : $"{_scanData.OcNo}|{_scanData.BoxNo}", true
                                          , _scanData.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
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
            //labIdLabel.Text = GlobalVariables.IdLabel;
            //labOcNo.Text = GlobalVariables.OcNo;
            //labBoxNo.Text = GlobalVariables.BoxNo;
            //labWeight.Text = (GlobalVariables.RealWeight / 1000).ToString("#,#0.00");

            txtActualDeviation.Visible = false;
            labelControl1.Visible = false;

            txtQrCode.Focus();

            _timer.Interval = 200;
            _timer.Tick += _timer_Tick;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Timer t = (Timer)sender;
            t.Enabled = false;

            _scaleValue = GlobalVariables.RealWeight;

            if (labWeight.InvokeRequired)
            {
                labWeight.Text = (_scaleValue / 1000).ToString("#,#0.00");
            }
            else
            {
                labWeight.Text = (_scaleValue / 1000).ToString("#,#0.00");
            }

            t.Enabled = true;
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
