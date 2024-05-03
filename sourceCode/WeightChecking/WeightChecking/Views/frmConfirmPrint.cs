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
        double _scaleValue = 0;//đơn vị gram
        Guid _qrApproved;
        double _actualDeviation = 0;
        double _ratio = 0;
        string _reason = string.Empty;

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
                                    && GlobalVariables.Station == StationEnum.IDC_1)
                                //&& (GlobalVariables.Station == StationEnum.IDC_1 && _scanData.OcNo.Substring(0, 2) != "PR"))
                                {
                                    using (var formDeviation = new frmDeviationForFalseAlarm())
                                    {
                                        var resultForm = formDeviation.ShowDialog();

                                        if (resultForm == DialogResult.OK)
                                        {
                                            _scanData.ActualDeviationPairs = formDeviation.ActualDeviation;

                                            //popup form chon reason
                                            if (_scanData.ActualDeviationPairs != 0)
                                            {
                                                using (var nfReason = new frmSelectReason())
                                                {
                                                    if (nfReason.ShowDialog() == DialogResult.OK)
                                                    {
                                                        _reason = nfReason.Reason;
                                                    }
                                                }
                                            }
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

                                #region Log vao bang approved
                                para.Add("QrCode", _qrApproved);
                                para.Add("IdLabel", _scanData.IdLabel);
                                para.Add("OC", _scanData.OcNo);
                                para.Add("BoxNo", _scanData.BoxNo);
                                para.Add("GrossWeight", _scanData.GrossWeight);
                                para.Add("Station", GlobalVariables.Station);
                                para.Add("QRLabel", _scanData.BarcodeString);
                                para.Add("ApproveType", _scanData.ActualDeviationPairs == 0 ? "False alarm" : "Actual deviation");
                                para.Add("DeviationPairs", _scanData.DeviationPairs);
                                para.Add("ActualDeviationPairs", _scanData.ActualDeviationPairs);
                                para.Add("NetWeight", _scanData.NetWeight);
                                para.Add("Deviation", _scanData.Deviation);
                                para.Add("CalculatorPrs", _scanData.CalculatedPairs);
                                para.Add("ScanDataId", _scanData.Id);
                                para.Add("Quantity", _scanData.Quantity);
                                para.Add("CreatedDate", _scanData.CreatedDate);
                                para.Add("Reason", _reason);

                                connection.Execute("sp_tblApprovedPrintLabelInsert", para, commandType: CommandType.StoredProcedure);
                                #endregion

                                //update lại thông tin cho thùng fail trong bảng scanData
                                #region tính toán lại các thông số theo khối lượng grossWeight mới.
                                _scanData.GrossWeight = _scaleValue;
                                _scanData.NetWeight = Math.Round(_scanData.GrossWeight - _scanData.BoxWeight - _scanData.PackageWeight, 3);
                                _scanData.Deviation = Math.Round(_scanData.NetWeight - _scanData.StdNetWeight, 3);

                                var nwPlus = _scanData.StdNetWeight + _scanData.UpperTolerance;
                                var nwSub = _scanData.StdNetWeight - _scanData.LowerTolerance;

                                if (((_scanData.NetWeight > nwPlus) && (_scanData.NetWeight - nwPlus < _scanData.AveWeight1Prs / 2))
                                || ((_scanData.NetWeight < nwSub) && (nwSub - _scanData.NetWeight < _scanData.AveWeight1Prs / 2))
                                )
                                {
                                    _scanData.CalculatedPairs = _scanData.Quantity;
                                }
                                else if (_scanData.NetWeight > nwPlus)//roundDown
                                {
                                    _scanData.CalculatedPairs = (int)(_scanData.Quantity + Math.Floor((_scanData.NetWeight - nwPlus) / _scanData.AveWeight1Prs));
                                }
                                else if (_scanData.NetWeight < nwSub)//RoundUp
                                {
                                    _scanData.CalculatedPairs = (int)(_scanData.Quantity - Math.Ceiling((nwSub - _scanData.NetWeight) / _scanData.AveWeight1Prs));
                                }
                                else
                                {
                                    _scanData.CalculatedPairs = _scanData.Quantity;
                                }

                                _scanData.DeviationPairs = _scanData.CalculatedPairs - _scanData.Quantity;
                                _scanData.RatioFailWeight = Math.Round((Math.Abs(_scanData.DeviationPairs) * _scanData.AveWeight1Prs) / _scanData.StdGrossWeight, 3);
                                #endregion

                                para = null;
                                para = new DynamicParameters();
                                para.Add("Id", _scanData.Id);
                                para.Add("ApproveBy", _qrApproved);
                                para.Add("ActualDeviationPairs", _scanData.ActualDeviationPairs);
                                para.Add("GrossWeight", _scaleValue);
                                para.Add("Status", (_scanData.OcNo.Substring(0, 3) == "PRT"|| _scanData.OcNo.Substring(0, 3) == "PRS") && GlobalVariables.Station == 0 ? 1 : 2);
                                para.Add("NetWeight", _scanData.NetWeight);
                                para.Add("Calculatorpairs", _scanData.CalculatedPairs);
                                para.Add("Deviation", _scanData.Deviation);
                                para.Add("DeviationPairs", _scanData.DeviationPairs);
                                para.Add("RatioFailWeight", _scanData.RatioFailWeight);

                                connection.Execute("sp_tblScanDataUpdateApproveBy", para, commandType: CommandType.StoredProcedure);

                                GlobalVariables.Printing((_scaleValue / 1000).ToString("#,#0.00")
                                          , !string.IsNullOrEmpty(_scanData.IdLabel) ? _scanData.IdLabel : $"{_scanData.OcNo}|{_scanData.BoxNo}", true
                                          , _scanData.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                
                                #region Auto posting
                                //hàng từ production qua: decoration = 0 (OC). transfer từ kho 3--> 64

                                GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 64, 41, GlobalVariables.GetDbConnectionDogeWh(), null);

                                if (GlobalVariables.ResultPosting.Message == "Successful")
                                {
                                    GlobalVariables.ResultPosting.Message = $"Hàng Production OK (Transfer 64-->41): {GlobalVariables.ResultPosting.Message}";
                                }
                                else
                                {
                                    GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 63, 41, GlobalVariables.GetDbConnectionDogeWh(), null);
                                    GlobalVariables.ResultPosting.Message = $"Hàng Production OK (Metal - Transfer 63-->41): {GlobalVariables.ResultPosting.Message}";
                                }
                                #endregion

                                #region Get thong tin boxParent nếu có, và update lại actual deviation cho thùng mẹ ở bảng scanData và ApprovedPrint
                                using (var connectionDWH = GlobalVariables.GetDbConnectionDogeWh())
                                {
                                    para = null;
                                    para = new DynamicParameters();
                                    para.Add("@OcNo", _scanData.OcNo);

                                    var boxParent = connectionDWH.Query<BoxParentModel>("sp_IdcSsfgPrintedLabels_OC_IndexCheck", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

                                    if (boxParent != null)
                                    {
                                        if (boxParent != null)
                                        {
                                            _scanData.ParentOc = boxParent.ParentOc;
                                            _scanData.ParentBoxId = boxParent.ParentBoxCode;

                                            para = null;
                                            para = new DynamicParameters();
                                            para.Add("_OcNo", boxParent.ParentOc);
                                            para.Add("_BoxId", boxParent.ParentBoxCode);

                                            var resultCheckBoxInfo = connection.Query<tblScanDataModel>("sp_tblScanDataGetByQrCode", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

                                            if (resultCheckBoxInfo != null)
                                            {
                                                if (resultCheckBoxInfo.Quantity + resultCheckBoxInfo.ActualDeviationPairs > resultCheckBoxInfo.Quantity)
                                                {
                                                    //cập nhật actual deviation cho thùng mẹ
                                                    para = null;
                                                    para = new DynamicParameters();
                                                    para.Add("Id", resultCheckBoxInfo.Id);
                                                    para.Add("ApproveBy", resultCheckBoxInfo.ApprovedBy);
                                                    para.Add("ActualDeviationPairs", _scanData.Quantity);//chính là qtyChildBox
                                                    para.Add("GrossWeight", resultCheckBoxInfo.GrossWeight);
                                                    para.Add("Status", 2);
                                                    para.Add("NetWeight", resultCheckBoxInfo.NetWeight);
                                                    para.Add("Calculatorpairs", resultCheckBoxInfo.CalculatedPairs);
                                                    para.Add("Deviation", resultCheckBoxInfo.Deviation);
                                                    para.Add("DeviationPairs", resultCheckBoxInfo.DeviationPairs);
                                                    para.Add("RatioFailWeight", resultCheckBoxInfo.RatioFailWeight);

                                                    connection.Execute("sp_tblScanDataUpdateApproveBy", para, commandType: CommandType.StoredProcedure);

                                                    #region Update actual deviation for approvedPrint
                                                    para = null;
                                                    para = new DynamicParameters();
                                                    para.Add("@ScanDataId", resultCheckBoxInfo.Id);
                                                    para.Add("@ActualDeviation", _scanData.Quantity);

                                                    connection.Execute("[sp_tblApprovedPrintLabelUpdate]", para, commandType: CommandType.StoredProcedure);
                                                    #endregion
                                                }
                                                //Trường hợp 2: thùng mẹ bị thiếu hàng
                                                //khi đó sẽ in tem lụi 1 con tem mới. thay cho tem mẹ
                                                //tem mẹ sẽ bị hủy.
                                                else
                                                {
                                                    //cập nhật actual deviation cho thùng mẹ
                                                    para = null;
                                                    para = new DynamicParameters();
                                                    para.Add("Id", resultCheckBoxInfo.Id);
                                                    para.Add("ApproveBy", resultCheckBoxInfo.ApprovedBy);
                                                    para.Add("ActualDeviationPairs", _scanData.Quantity - resultCheckBoxInfo.Quantity);//qtyChildBox - qtyMotherBox
                                                    para.Add("GrossWeight", resultCheckBoxInfo.GrossWeight);
                                                    para.Add("Status", 2);
                                                    para.Add("NetWeight", resultCheckBoxInfo.NetWeight);
                                                    para.Add("Calculatorpairs", resultCheckBoxInfo.CalculatedPairs);
                                                    para.Add("Deviation", resultCheckBoxInfo.Deviation);
                                                    para.Add("DeviationPairs", resultCheckBoxInfo.DeviationPairs);
                                                    para.Add("RatioFailWeight", resultCheckBoxInfo.RatioFailWeight);

                                                    connection.Execute("sp_tblScanDataUpdateApproveBy", para, commandType: CommandType.StoredProcedure);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
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
