using Dapper;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeightChecking
{
    public partial class frmScale : DevExpress.XtraEditors.XtraForm
    {
        private ScaleHelper _scaleHelper;
        private Task _ckTask;

        private tblScanDataModel _scanData = new tblScanDataModel();

        private string _idLabel = null;
        private string _plr = null;// kiểu đóng thùng, P-đôi; L/R-left right

        private double _weight = 0, _boxWeight = 0, _accessoriesWeight = 0;

        public frmScale()
        {
            InitializeComponent();

            Load += FrmScale_Load;
        }

        private void FrmScale_Load(object sender, EventArgs e)
        {
            this.txtQrCode.Focus();
            this.txtQrCode.KeyDown += TxtQrCode_KeyDown;
            //txtTest.KeyDown += TxtTest_KeyDown1;

            #region Register events Scale value change
            if (GlobalVariables.IsScale)
            {
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
                        var w = o.Value * GlobalVariables.UnitScale;
                        //if (w.ToString().Length >= 4 || w == 0)
                        {
                            if (labRealWeight.InvokeRequired)
                            {
                                labRealWeight.Invoke(new Action(() =>
                                {
                                    labScaleValue.Text = w.ToString();
                                }));
                            }
                            else
                            {
                                labScaleValue.Text = w.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Scale event error.");
                    }
                };
                _scaleHelper.ScaleValue = 1;// 5.545;//tac động để đọc cân lần đầu tiên
            }
            #endregion

            #region hien thi cac thong so dem
            if (labGoodBox.InvokeRequired)
            {
                labGoodBox.Invoke(new Action(() => { labGoodBox.Text = (GlobalVariables.RememberInfo.GoodBoxNoPrinting + GlobalVariables.RememberInfo.GoodBoxPrinting).ToString(); }));
            }
            else labGoodBox.Text = (GlobalVariables.RememberInfo.GoodBoxNoPrinting + GlobalVariables.RememberInfo.GoodBoxPrinting).ToString();

            if (labGoodNoPrint.InvokeRequired)
            {
                labGoodNoPrint.Invoke(new Action(() =>
                {
                    labGoodNoPrint.Text = GlobalVariables.RememberInfo.GoodBoxNoPrinting.ToString();
                }));
            }
            else labGoodNoPrint.Text = GlobalVariables.RememberInfo.GoodBoxNoPrinting.ToString();

            if (labGoodPrint.InvokeRequired)
            {
                labGoodPrint.Invoke(new Action(() =>
                {
                    labGoodPrint.Text = GlobalVariables.RememberInfo.GoodBoxPrinting.ToString();
                }));
            }
            else labGoodPrint.Text = GlobalVariables.RememberInfo.GoodBoxPrinting.ToString();

            if (labFailBox.InvokeRequired)
            {
                labFailBox.Invoke(new Action(() =>
                {
                    labFailBox.Text = (GlobalVariables.RememberInfo.FailBoxNoPrinting
                    + GlobalVariables.RememberInfo.FailBoxPrinting).ToString();
                }));
            }
            else labFailBox.Text = (GlobalVariables.RememberInfo.FailBoxNoPrinting + GlobalVariables.RememberInfo.FailBoxPrinting).ToString();

            if (labFailNoPrint.InvokeRequired)
            {
                labFailNoPrint.Invoke(new Action(() =>
                {
                    labFailNoPrint.Text = GlobalVariables.RememberInfo.FailBoxNoPrinting.ToString();
                }));
            }
            else labFailNoPrint.Text = GlobalVariables.RememberInfo.FailBoxNoPrinting.ToString();
            if (labFailPrint.InvokeRequired)
            {
                labFailPrint.Invoke(new Action(() =>
                {
                    labFailPrint.Text = GlobalVariables.RememberInfo.FailBoxPrinting.ToString();
                }));
            }
            else labFailPrint.Text = GlobalVariables.RememberInfo.FailBoxPrinting.ToString();

            if (labMetalScanBox.InvokeRequired)
            {
                labMetalScanBox.Invoke(new Action(() =>
                {
                    labMetalScanBox.Text = GlobalVariables.RememberInfo.MetalScan.ToString();
                }));
            }
            else labMetalScanBox.Text = GlobalVariables.RememberInfo.MetalScan.ToString();

            if (labMetalScanCount.InvokeRequired)
            {
                labMetalScanCount.Invoke(new Action(() =>
                {
                    labMetalScanCount.Text = GlobalVariables.RememberInfo.CountMetalScan.ToString();
                }));
            }
            else labMetalScanCount.Text = GlobalVariables.RememberInfo.CountMetalScan.ToString();

            #endregion

            #region đăng ký sự kiện cập nhật giá trị MetalScan counter
            GlobalVariables.MyEvent.EventHandlerCount += (s, o) =>
            {
                GlobalVariables.RememberInfo.CountMetalScan = o.CountValue;

                if (labMetalScanCount.InvokeRequired)
                {
                    labMetalScanCount.Invoke(new Action(() =>
                    {
                        labMetalScanCount.Text = o.CountValue.ToString();
                    }));
                }
                else
                {
                    labMetalScanCount.Text = o.CountValue.ToString();
                }
            };

            GlobalVariables.MyEvent.EventHandlerRefreshMasterData += (s, o) =>
            {
                #region hien thi cac thong so dem
                if (labGoodBox.InvokeRequired)
                {
                    labGoodBox.Invoke(new Action(() => { labGoodBox.Text = (GlobalVariables.RememberInfo.GoodBoxNoPrinting + GlobalVariables.RememberInfo.GoodBoxPrinting).ToString(); }));
                }
                else labGoodBox.Text = (GlobalVariables.RememberInfo.GoodBoxNoPrinting + GlobalVariables.RememberInfo.GoodBoxPrinting).ToString();

                if (labGoodNoPrint.InvokeRequired)
                {
                    labGoodNoPrint.Invoke(new Action(() =>
                    {
                        labGoodNoPrint.Text = GlobalVariables.RememberInfo.GoodBoxNoPrinting.ToString();
                    }));
                }
                else labGoodNoPrint.Text = GlobalVariables.RememberInfo.GoodBoxNoPrinting.ToString();

                if (labGoodPrint.InvokeRequired)
                {
                    labGoodPrint.Invoke(new Action(() =>
                    {
                        labGoodPrint.Text = GlobalVariables.RememberInfo.GoodBoxPrinting.ToString();
                    }));
                }
                else labGoodPrint.Text = GlobalVariables.RememberInfo.GoodBoxPrinting.ToString();

                if (labFailBox.InvokeRequired)
                {
                    labFailBox.Invoke(new Action(() =>
                    {
                        labFailBox.Text = (GlobalVariables.RememberInfo.FailBoxNoPrinting
                        + GlobalVariables.RememberInfo.FailBoxPrinting).ToString();
                    }));
                }
                else labFailBox.Text = (GlobalVariables.RememberInfo.FailBoxNoPrinting + GlobalVariables.RememberInfo.FailBoxPrinting).ToString();

                if (labFailNoPrint.InvokeRequired)
                {
                    labFailNoPrint.Invoke(new Action(() =>
                    {
                        labFailNoPrint.Text = GlobalVariables.RememberInfo.FailBoxNoPrinting.ToString();
                    }));
                }
                else labFailNoPrint.Text = GlobalVariables.RememberInfo.FailBoxNoPrinting.ToString();
                if (labFailPrint.InvokeRequired)
                {
                    labFailPrint.Invoke(new Action(() =>
                    {
                        labFailPrint.Text = GlobalVariables.RememberInfo.FailBoxPrinting.ToString();
                    }));
                }
                else labFailPrint.Text = GlobalVariables.RememberInfo.FailBoxPrinting.ToString();

                if (labMetalScanBox.InvokeRequired)
                {
                    labMetalScanBox.Invoke(new Action(() =>
                    {
                        labMetalScanBox.Text = GlobalVariables.RememberInfo.MetalScan.ToString();
                    }));
                }
                else labMetalScanBox.Text = GlobalVariables.RememberInfo.MetalScan.ToString();

                if (labMetalScanCount.InvokeRequired)
                {
                    labMetalScanCount.Invoke(new Action(() =>
                    {
                        labMetalScanCount.Text = GlobalVariables.RememberInfo.CountMetalScan.ToString();
                    }));
                }
                else labMetalScanCount.Text = GlobalVariables.RememberInfo.CountMetalScan.ToString();

                #endregion
            };
            #endregion
        }

        private void TxtTest_KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextEdit _txt = (TextEdit)sender;

                if (labRealWeight.InvokeRequired)
                {
                    labRealWeight.Invoke(new Action(() =>
                    {
                        labScaleValue.Text = _txt.Text;
                    }));
                }
                else
                {
                    labScaleValue.Text = _txt.Text;
                }
            }
        }

        private void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            //content of the QR code "OC283225,6112042102-P237-2351,42,13,P,1/56,160506,1/1|1,30.2022,0,0,1" no print
            //content of the QR code "OC283225,6112042102-P232-2351,42,13,P,1/56,160506,1/1|1,30.2022,1,0,1" no print
            //content of the QR code "OCA6915,6112042103-PAJ2-3201,42,13,P,1/56,160506,1/1|1,30.2022,1,0,1" print
            //content of the QR code "OPRT4383,6116322202-NAZG-0014,60,1,P,3/7,170000,3/4|1,,,," print
            //content of the QR code "PRTA516,6117012206-2462-D213,100,5,P,31/86,170000,13/22|1,340.2022,1,1,99" print
            //|1,30.2022,1,0,1-->Location,IdLable,Decoration,MetalScan,Category(phân biệt hàng HC hay ko)

            _scanData.GrossWeight = double.TryParse(labScaleValue.Text, out double value) ? value : 0;

            if (e.KeyCode == Keys.Enter)
            {
                TextEdit _sen = sender as TextEdit;
                Console.WriteLine(_sen.Text);

                #region xử lý barcode lấy ra các giá trị theo code
                _scanData.BarcodeString = _sen.Text;
                if (_scanData.BarcodeString.Contains("|"))
                {
                    var s = _sen.Text.Split('|');
                    var s1 = s[0].Split(',');
                    _plr = s1[4];//get Thung này đóng theo đôi (P) hay L/R
                    _scanData.OcNo = s1[0];
                    _scanData.ProductNumber = s1[1];

                    _scanData.Quantity = Convert.ToInt32(s1[2]);
                    _scanData.LinePosNo = s1[3];
                    _scanData.BoxNo = s1[5];
                    _scanData.CustomerNo = s1[6];
                    _scanData.BoxPosNo = s1[7];

                    if (s[1].Contains(","))
                    {
                        var s2 = s[1].Split(',');

                        GlobalVariables.IdLabel = s2[1];
                        _scanData.IdLable = GlobalVariables.IdLabel;

                        if (s2[0] == "1")
                        {
                            _scanData.Location = LocationEnum.fVN;
                        }
                        else if (s2[0] == "2")
                        {
                            _scanData.Location = LocationEnum.fFT;
                        }
                        else if (s2[0] == "3")
                        {
                            _scanData.Location = LocationEnum.fKV;
                        }
                    }
                    else
                    {
                        if (s[1] == "1")
                        {
                            _scanData.Location = LocationEnum.fVN;
                        }
                        else if (s[1] == "2")
                        {
                            _scanData.Location = LocationEnum.fFT;
                        }
                        else if (s[1] == "3")
                        {
                            _scanData.Location = LocationEnum.fKV;
                        }
                    }
                }
                else
                {
                    var s1 = _scanData.BarcodeString.Split(',');
                    _plr = s1[4];//get Thung này đóng theo đôi (P) hay L/R
                    _scanData.OcNo = s1[0];
                    _scanData.ProductNumber = s1[1];

                    _scanData.Quantity = Convert.ToInt32(s1[2]);
                    _scanData.LinePosNo = s1[3];
                    _scanData.BoxNo = s1[5];
                }

                GlobalVariables.OcNo = _scanData.OcNo;
                GlobalVariables.BoxNo = _scanData.BoxNo;
                #endregion


                #region truy vấn data và xử lý
                //truy vấn thông tin 
                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var para = new DynamicParameters();
                    para.Add("@ProductNumber", _scanData.ProductNumber);

                    var res = connection.Query<ProductInfoModel>("sp_vProductItemInfoGet", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (res != null)
                    {
                        _scanData.ProductName = res.ProductName;
                        _scanData.Decoration = res.Decoration;
                        _scanData.MetalScan = res.MetalScan;
                        _scanData.Brand = res.Brand;
                        _scanData.AveWeight1Prs = res.AveWeight1Prs;

                        if (_scanData.AveWeight1Prs != 0)
                        {

                            #region Fill data from coreData to scanData, tính toán ra NetWeight và GrossWeight
                            //Xét điều kiện để lấy boxWeight. Nếu là hàng đi sơn thì dùng thùng nhựa
                            if (_scanData.Decoration == 0 || _scanData.Decoration == 1 && _scanData.OcNo.Contains("OPRT")
                                || _scanData.Decoration == 1 && _scanData.OcNo.Contains("OCA"))
                            {
                                _scanData.Status = 2;//báo trạng thái hàng ko đi sơn, hoặc hàng sơn đã được sơn rồi

                                if (_scanData.Quantity <= res.BoxQtyBx4)
                                {
                                    _scanData.BoxWeight = res.BoxWeightBx4;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx4 && _scanData.Quantity <= res.BoxQtyBx3)
                                {
                                    _scanData.BoxWeight = res.BoxWeightBx3;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx3 && _scanData.Quantity <= res.BoxQtyBx2)
                                {
                                    _scanData.BoxWeight = res.BoxWeightBx2;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx2 && _scanData.Quantity <= res.BoxQtyBx1)
                                {
                                    _scanData.BoxWeight = res.BoxWeightBx1;
                                }

                                if (_scanData.Decoration == 0)
                                {
                                    if (labDecoration.InvokeRequired)
                                    {
                                        labDecoration.Invoke(new Action(() =>
                                        {
                                            labDecoration.BackColor = Color.Gray;
                                        }));
                                    }
                                    else
                                    {
                                        labDecoration.BackColor = Color.Gray;
                                    }
                                }
                                else
                                {
                                    if (labDecoration.InvokeRequired)
                                    {
                                        labDecoration.Invoke(new Action(() =>
                                        {
                                            labDecoration.BackColor = Color.Green;
                                        }));
                                    }
                                    else
                                    {
                                        labDecoration.BackColor = Color.Green;
                                    }
                                }
                            }
                            else if (_scanData.Decoration == 1 && _scanData.OcNo.Contains("PRT"))
                            {
                                _scanData.Status = 1;// báo trạng thái hàng cần đưa đi sơn

                                _scanData.BoxWeight = res.PlasicBoxWeight;

                                if (labDecoration.InvokeRequired)
                                {
                                    labDecoration.Invoke(new Action(() =>
                                    {
                                        labDecoration.BackColor = Color.Gold;
                                    }));
                                }
                                else
                                {
                                    labDecoration.BackColor = Color.Gold;
                                }
                            }

                            if (_scanData.MetalScan == 0)
                            {
                                if (labMetalScan.InvokeRequired)
                                {
                                    labMetalScan.Invoke(new Action(() =>
                                    {
                                        labMetalScan.BackColor = Color.Gray;
                                    }));
                                }
                                else
                                {
                                    labMetalScan.BackColor = Color.Gray;
                                }
                            }
                            else
                            {
                                GlobalVariables.RememberInfo.MetalScan += 1;
                                if (labMetalScan.InvokeRequired)
                                {
                                    labMetalScan.Invoke(new Action(() =>
                                    {
                                        labMetalScan.BackColor = Color.Gold;
                                    }));
                                }
                                else
                                {
                                    labMetalScan.BackColor = Color.Gold;
                                }
                            }

                            _scanData.StdNetWeight = Math.Round(_scanData.Quantity * _scanData.AveWeight1Prs, 3);
                            _scanData.Tolerance = Math.Round(_scanData.StdNetWeight * (res.Tolerance / 100), 3);

                            //luu ý các Quantity partition-Plasic-WrapSheet trên DB nó là tính số Prs
                            //sau khi đọc về phải lấy QtyPrs quét trên label / Quantity partition-Plasic-WrapSheet ==> qty * weight ==> Weight packing accessories
                            var partitionWeight = res.PartitionQty != 0 ? (_scanData.Quantity / res.PartitionQty) * res.PartitionWeight : 0;
                            var plasicBagWeight = res.PlasicBagQty != 0 ? (_scanData.Quantity / res.PlasicBagQty) * res.PlasicBagWeight : 0;
                            var wrapSheetWeight = res.WrapSheetQty != 0 ? (_scanData.Quantity / res.WrapSheetQty) * res.WrapSheetWeight : 0;

                            _scanData.PackageWeight = Math.Round(partitionWeight + plasicBagWeight + wrapSheetWeight, 3);

                            _scanData.StdGrossWeight = Math.Round(_scanData.StdNetWeight + _scanData.PackageWeight + _scanData.BoxWeight, 3);

                            #region tinh toán standardWeight theo Pair/Left/Right. lưu ý để sau này có áp dụng thì làm
                            //if (_plr == "P")
                            //{
                            //    _scanData.GrossdWeight = res.Weight * res.QtyPerbag + res.BagWeight;
                            //}
                            //else if (_plr == "L")
                            //{
                            //    if (res.LeftWeight == 0)
                            //    {
                            //        _scanData.StandardWeight = res.Weight * res.QtyPerbag + res.BagWeight;
                            //    }
                            //    else
                            //    {
                            //        _scanData.StandardWeight = res.LeftWeight * res.QtyPerbag + res.BagWeight;
                            //    }
                            //}
                            //else if (_plr == "R")
                            //{
                            //    if (res.RightWeight == 0)
                            //    {
                            //        _scanData.StandardWeight = res.Weight * res.QtyPerbag + res.BagWeight;
                            //    }
                            //    else
                            //    {
                            //        _scanData.StandardWeight = res.RightWeight * res.QtyPerbag + res.BagWeight;
                            //    }
                            //}
                            #endregion

                            #endregion

                            #region hiển thị thông tin
                            if (labRealWeight.InvokeRequired)
                            {
                                labRealWeight.Invoke(new Action(() =>
                                {
                                    labRealWeight.Text = _scanData.GrossWeight.ToString();
                                }));
                            }
                            else labRealWeight.Text = _scanData.GrossWeight.ToString();

                            if (labNetWeight.InvokeRequired)
                            {
                                labNetWeight.Invoke(new Action(() =>
                                {
                                    labNetWeight.Text = _scanData.StdNetWeight.ToString();
                                }));
                            }
                            else labNetWeight.Text = _scanData.StdNetWeight.ToString();

                            if (labOcNo.InvokeRequired)
                            {
                                labOcNo.Invoke(new Action(() => { labOcNo.Text = _scanData.OcNo; }));
                            }
                            else labOcNo.Text = _scanData.OcNo;

                            if (labProductCode.InvokeRequired)
                            {
                                labProductCode.Invoke(new Action(() => { labProductCode.Text = _scanData.ProductNumber; }));
                            }
                            else labProductCode.Text = _scanData.ProductNumber;

                            if (labProductName.InvokeRequired)
                            {
                                labProductName.Invoke(new Action(() => { labProductName.Text = _scanData.ProductName; }));
                            }
                            else labProductName.Text = _scanData.ProductName;

                            if (labQuantity.InvokeRequired)
                            {
                                labQuantity.Invoke(new Action(() => { labQuantity.Text = _scanData.Quantity.ToString(); }));
                            }
                            else labQuantity.Text = _scanData.Quantity.ToString();

                            if (labColor.InvokeRequired)
                            {
                                labColor.Invoke(new Action(() => { labColor.Text = res.Color; }));
                            }
                            else labColor.Text = res.Color;

                            if (labSize.InvokeRequired)
                            {
                                labSize.Invoke(new Action(() => { labSize.Text = res.SizeName; }));
                            }
                            else labSize.Text = res.SizeName;

                            if (labAveWeight.InvokeRequired)
                            {
                                labAveWeight.Invoke(new Action(() => { labAveWeight.Text = _scanData.AveWeight1Prs.ToString(); }));
                            }
                            else labAveWeight.Text = _scanData.AveWeight1Prs.ToString();

                            if (labToloren.InvokeRequired)
                            {
                                labToloren.Invoke(new Action(() => { labToloren.Text = _scanData.Tolerance.ToString(); }));
                            }
                            else labToloren.Text = _scanData.Tolerance.ToString();

                            if (labBoxWeight.InvokeRequired)
                            {
                                labBoxWeight.Invoke(new Action(() => { labBoxWeight.Text = _scanData.BoxWeight.ToString(); }));
                            }
                            else labBoxWeight.Text = _scanData.BoxWeight.ToString();

                            if (labAccessoriesWeight.InvokeRequired)
                            {
                                labAccessoriesWeight.Invoke(new Action(() => { labAccessoriesWeight.Text = _scanData.PackageWeight.ToString(); }));
                            }
                            else labAccessoriesWeight.Text = _scanData.PackageWeight.ToString();

                            if (labGrossWeight.InvokeRequired)
                            {
                                labGrossWeight.Invoke(new Action(() => { labGrossWeight.Text = _scanData.StdGrossWeight.ToString(); }));
                            }
                            else labGrossWeight.Text = _scanData.StdGrossWeight.ToString();
                            #endregion

                            #region xử lý so sánh khối lượng cân thực tế với kế hoạch để xử lý
                            _scanData.NetWeight = Math.Round(_scanData.GrossWeight - _scanData.BoxWeight - _scanData.PackageWeight, 3);
                            _scanData.Deviation = Math.Round(_scanData.NetWeight - _scanData.StdNetWeight, 3);

                            #region tính toán số pairs chênh lệch và hiển thị label
                            var nwPlus = _scanData.StdNetWeight + _scanData.Tolerance;
                            var nwSub = _scanData.StdNetWeight - _scanData.Tolerance;

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

                            if (labCalculatedPairs.InvokeRequired)
                            {
                                labCalculatedPairs.Invoke(new Action(() =>
                                {
                                    labCalculatedPairs.Text = _scanData.CalculatedPairs.ToString();
                                }));
                            }
                            else
                            {
                                labCalculatedPairs.Text = _scanData.CalculatedPairs.ToString();
                            }

                            if (labDeviationPairs.InvokeRequired)
                            {
                                labDeviationPairs.Invoke(new Action(() =>
                                {
                                    labDeviationPairs.Text = _scanData.DeviationPairs.ToString();
                                }));
                            }
                            else
                            {
                                labDeviationPairs.Text = _scanData.DeviationPairs.ToString();
                            }
                            #endregion

                            //thung hang Pass
                            if (_scanData.DeviationPairs == 0)
                            {
                                if (_scanData.Decoration == 0)
                                {
                                    GlobalVariables.RememberInfo.GoodBoxPrinting += 1;
                                    //_scanData.Status = 1;
                                }
                                else
                                {
                                    GlobalVariables.RememberInfo.GoodBoxNoPrinting += 1;
                                    //_scanData.Status = 2;
                                }

                                #region hien thi mau label
                                if (labResult.InvokeRequired)
                                {
                                    labResult.Invoke(new Action(() =>
                                    {
                                        labResult.Text = "Pass";
                                        labResult.BackColor = Color.Green;
                                        labResult.ForeColor = Color.White;
                                    }));
                                }
                                else
                                {
                                    labResult.Text = "Pass";
                                    labResult.BackColor = Color.Green;
                                    labResult.ForeColor = Color.White;
                                }
                                #endregion
                                _scanData.Pass = 1;
                                //Printing
                                GlobalVariables.Printing(_scanData.GrossWeight
                                            , !string.IsNullOrEmpty(GlobalVariables.IdLabel) ? GlobalVariables.IdLabel : $"{_scanData.OcNo}|{_scanData.BoxNo}");
                                GlobalVariables.RealWeight = _scanData.GrossWeight;
                                GlobalVariables.PrintApprove = true;
                            }
                            else//thung fail
                            {
                                _scanData.Pass = 0;
                                _scanData.Status = 0;

                                GlobalVariables.PrintApprove = false;
                                if (_scanData.Decoration == 1)
                                {
                                    GlobalVariables.RememberInfo.FailBoxPrinting += 1;
                                }
                                else
                                {
                                    GlobalVariables.RememberInfo.FailBoxNoPrinting += 1;
                                }

                                #region hien thi mau label
                                if (labResult.InvokeRequired)
                                {
                                    labResult.Invoke(new Action(() =>
                                    {
                                        labResult.Text = "Fail";
                                        labResult.BackColor = Color.Red;
                                        labResult.ForeColor = Color.White;
                                    }));
                                }
                                else
                                {
                                    labResult.Text = "Fail";
                                    labResult.BackColor = Color.Red;
                                    labResult.ForeColor = Color.White;
                                }
                                #endregion
                            }
                            #endregion

                            #region hien thi cac thong so dem
                            if (labGoodBox.InvokeRequired)
                            {
                                labGoodBox.Invoke(new Action(() => { labGoodBox.Text = (GlobalVariables.RememberInfo.GoodBoxNoPrinting + GlobalVariables.RememberInfo.GoodBoxPrinting).ToString(); }));
                            }
                            else labGoodBox.Text = (GlobalVariables.RememberInfo.GoodBoxNoPrinting + GlobalVariables.RememberInfo.GoodBoxPrinting).ToString();

                            if (labGoodNoPrint.InvokeRequired)
                            {
                                labGoodNoPrint.Invoke(new Action(() =>
                                {
                                    labGoodNoPrint.Text = GlobalVariables.RememberInfo.GoodBoxNoPrinting.ToString();
                                }));
                            }
                            else labGoodNoPrint.Text = GlobalVariables.RememberInfo.GoodBoxNoPrinting.ToString();

                            if (labGoodPrint.InvokeRequired)
                            {
                                labGoodPrint.Invoke(new Action(() =>
                                {
                                    labGoodPrint.Text = GlobalVariables.RememberInfo.GoodBoxPrinting.ToString();
                                }));
                            }
                            else labGoodPrint.Text = GlobalVariables.RememberInfo.GoodBoxPrinting.ToString();

                            if (labFailBox.InvokeRequired)
                            {
                                labFailBox.Invoke(new Action(() =>
                                {
                                    labFailBox.Text = (GlobalVariables.RememberInfo.FailBoxNoPrinting
                                    + GlobalVariables.RememberInfo.FailBoxPrinting).ToString();
                                }));
                            }
                            else labFailBox.Text = (GlobalVariables.RememberInfo.FailBoxNoPrinting + GlobalVariables.RememberInfo.FailBoxPrinting).ToString();

                            if (labFailNoPrint.InvokeRequired)
                            {
                                labFailNoPrint.Invoke(new Action(() =>
                                {
                                    labFailNoPrint.Text = GlobalVariables.RememberInfo.FailBoxNoPrinting.ToString();
                                }));
                            }
                            else labFailNoPrint.Text = GlobalVariables.RememberInfo.FailBoxNoPrinting.ToString();
                            if (labFailPrint.InvokeRequired)
                            {
                                labFailPrint.Invoke(new Action(() =>
                                {
                                    labFailPrint.Text = GlobalVariables.RememberInfo.FailBoxPrinting.ToString();
                                }));
                            }
                            else labFailPrint.Text = GlobalVariables.RememberInfo.FailBoxPrinting.ToString();

                            if (labMetalScanBox.InvokeRequired)
                            {
                                labMetalScanBox.Invoke(new Action(() =>
                                {
                                    labMetalScanBox.Text = GlobalVariables.RememberInfo.MetalScan.ToString();
                                }));
                            }
                            else labMetalScanBox.Text = GlobalVariables.RememberInfo.MetalScan.ToString();

                            if (labMetalScanCount.InvokeRequired)
                            {
                                labMetalScanCount.Invoke(new Action(() =>
                                {
                                    labMetalScanCount.Text = GlobalVariables.RememberInfo.CountMetalScan.ToString();
                                }));
                            }
                            else labMetalScanCount.Text = GlobalVariables.RememberInfo.CountMetalScan.ToString();

                            if (labDeviation.InvokeRequired)
                            {
                                labDeviation.Invoke(new Action(() =>
                                {
                                    labDeviation.Text = _scanData.Deviation.ToString();
                                }));
                            }
                            else labDeviation.Text = _scanData.Deviation.ToString();

                            if (labNetRealWeight.InvokeRequired)
                            {
                                labNetRealWeight.Invoke(new Action(() =>
                                {
                                    labNetRealWeight.Text = _scanData.NetWeight.ToString();
                                }));
                            }
                            else labNetRealWeight.Text = _scanData.NetWeight.ToString();
                            #endregion

                            #region Log data
                            para = null;
                            para = new DynamicParameters();
                            para.Add("@BarcodeString", _scanData.BarcodeString);
                            para.Add("@IdLable", _scanData.IdLable);
                            para.Add("@OcNo", _scanData.OcNo);
                            para.Add("@ProductNumber", _scanData.ProductNumber);
                            para.Add("@ProductName", _scanData.ProductName);
                            para.Add("@Quantity", _scanData.Quantity);
                            para.Add("@LinePosNo", _scanData.LinePosNo);
                            para.Add("@Unit", _scanData.Unit);
                            para.Add("@BoxNo", _scanData.BoxNo);
                            para.Add("@CustomerNo", _scanData.CustomerNo);
                            para.Add("@Location", _scanData.Location);
                            para.Add("@BoxPosNo", _scanData.BoxPosNo);
                            para.Add("@Note", _scanData.Note);
                            para.Add("@Brand", _scanData.Brand);
                            para.Add("@Decoration", _scanData.Decoration);
                            para.Add("@MetalScan", _scanData.MetalScan);
                            para.Add("@AveWeight1Prs", _scanData.AveWeight1Prs);
                            para.Add("@StdNetWeight", _scanData.StdNetWeight);
                            para.Add("@Tolerance", _scanData.Tolerance);
                            para.Add("@Boxweight", _scanData.BoxWeight);
                            para.Add("@PackageWeight", _scanData.PackageWeight);
                            para.Add("@StdGrossWeight", _scanData.StdGrossWeight);
                            para.Add("@GrossWeight", _scanData.GrossWeight);
                            para.Add("@NetWeight", _scanData.NetWeight);
                            para.Add("@Deviation", _scanData.Deviation);
                            para.Add("@Pass", _scanData.Pass);
                            para.Add("Status", _scanData.Status);
                            para.Add("CalculatedPairs", _scanData.CalculatedPairs);
                            para.Add("DeviationPairs", _scanData.DeviationPairs);

                            var insertResult = connection.Execute("sp_tblScanDataInsert", para, commandType: CommandType.StoredProcedure);
                            #endregion

                            string json = JsonConvert.SerializeObject(GlobalVariables.RememberInfo);
                            File.WriteAllText(@"./RememberInfo.json", json);
                        }
                        else
                        {
                            XtraMessageBox.Show($"Item '{_scanData.ProductNumber}' không có khối lượng/1 đôi. Xin hãy kiểm tra lại thông tin."
                                , "CẢNH BÁO.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show($"Product number {_scanData.ProductNumber} không có trong hệ thống. Xin hãy kiểm tra lại thông tin."
                            , "CẢNH BÁO.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                #endregion

                #region reset txtQrcode để quét mã tiếp
                if (_sen.InvokeRequired)
                {
                    _sen?.Invoke(new Action(() =>
                    {
                        _sen.Text = null;
                    }));
                }
                else
                {
                    _sen.Text = null;
                }
                _sen.Focus();
                #endregion
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