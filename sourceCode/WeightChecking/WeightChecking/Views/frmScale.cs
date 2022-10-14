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
            //this.labWeight.TextChanged += LabWeight_TextChanged;

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
                    var _w = o.Value * GlobalVariables.UnitScale;

                    if (_w <= 30000)
                    {
                        _scanData.RealWeight = _w;
                    }

                    if (labWeight.InvokeRequired)
                    {
                        labWeight.Invoke(new Action(() =>
                        {
                            labWeight.Text = _scanData.RealWeight.ToString();
                        }));
                    }
                    else
                    {
                        labWeight.Text = _scanData.RealWeight.ToString();
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

        private void TxtTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextEdit _txt = (TextEdit)sender;
                _scaleHelper.ScaleValue = Convert.ToDouble(_txt.Text);
            }
        }

        private void LabWeight_TextChanged(object sender, EventArgs e)
        {
            LabelControl _lab = (LabelControl)sender;
            var _w = double.TryParse(_lab.Text, out double value) ? value : 0;

            if (_w <= 30)
            {
                _scanData.RealWeight = _w;
            }

            Console.WriteLine($"Real weight: {_scanData.RealWeight}");
        }

        private void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            //content of the QR code "OC283225,6112042007-P062-3301,28,13,P,1/56,160506,1/1|1,30.2022,1,0,1"

            if (e.KeyCode == Keys.Enter)
            {
                Thread.Sleep(1000);
                TextEdit _sen = sender as TextEdit;
                Console.WriteLine(_sen.Text);

                #region xử lý barcode lấy ra các giá trị theo code
                _scanData.BarcodeString = _sen.Text;

                var s = _sen.Text.Split('|');
                var s1 = s[0].Split(',');
                _plr = s1[4];//get Thung này đóng theo đôi (P) hay L/R
                _scanData.OcNo = s1[0];
                _scanData.ProductNo = s1[1];
                
                _scanData.Quantity = Convert.ToInt32(s1[2]);
                _scanData.LinePosNo = s1[3];
                _scanData.BoxNo = s1[5];
                _scanData.CustomerNo = s1[6];
                _scanData.BoxPosNo = s1[7];

                if (s1[1].Contains(","))
                {
                    var s2 = s[1].Split(',');

                    GlobalVariables.IdLabel = s2[1];
                    _scanData.IdLable = GlobalVariables.IdLabel;
                    _scanData.Location = Convert.ToInt32(s2[0]);
                }
                else
                {
                    _scanData.Location = Convert.ToInt32(s1[1]);
                }
                #endregion


                #region truy vấn data và xử lý
                //truy vấn thông tin 
                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var para = new DynamicParameters();
                    para.Add("@ProductNumber", _scanData.ProductNo);

                    var res = connection.Query<ProductInfoModel>("sp_vProduct_infoInfoGet", para, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (res != null)
                    {
                        #region Fill data from coreData to scanData
                        _scanData.ProductName = res.ProductName;
                        _scanData.Decoration = res.Decoration;
                        _scanData.MetalScan = res.MetalScan;
                        _scanData.Brand = res.Brand;

                        #region tinh toán standardWeight theo Pair/Left/Right
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
                        if (labOcNo.InvokeRequired)
                        {
                            labOcNo.Invoke(new Action(() => { labOcNo.Text = _scanData.OcNo; }));
                        }
                        else labOcNo.Text = _scanData.OcNo;

                        if (labProductCode.InvokeRequired)
                        {
                            labProductCode.Invoke(new Action(() => { labProductCode.Text = _scanData.ProductNo; }));
                        }
                        else labProductCode.Text = _scanData.ProductNo;

                        if (labProductName.InvokeRequired)
                        {
                            labProductName.Invoke(new Action(() => { labProductName.Text = _scanData.ProductName; }));
                        }
                        else labProductName.Text = _scanData.ProductName;

                        if (labWeightPlan.InvokeRequired)
                        {
                            labWeightPlan.Invoke(new Action(() => { labWeightPlan.Text = _scanData.GrossdWeight.ToString(); }));
                        }
                        else labWeightPlan.Text = _scanData.GrossdWeight.ToString();
                        #endregion

                        #region xử lý so sánh khối lượng cân thực tế với kế hoạch để xử lý
                        //thung hang Pass
                        if (_scanData.RealWeight >= _scanData.GrossdWeight - res.Tolerance
                            && _scanData.RealWeight <= _scanData.GrossdWeight + res.Tolerance)
                        {
                            if (_scanData.Decoration == 1)
                            {
                                GlobalVariables.RememberInfo.GoodBoxPrinting += 1;
                                _scanData.Status = 1;
                            }
                            else
                            {
                                GlobalVariables.RememberInfo.GoodBoxNoPrinting += 1;
                                _scanData.Status = 2;
                            }
                            //hien thi mau label
                            if (labWeight.InvokeRequired)
                            {
                                labWeight.Invoke(new Action(()=> {
                                    labWeight.BackColor = Color.Green;
                                }));
                            }
                            else
                            {
                                labWeight.BackColor = Color.Green;
                            }
                            _scanData.Pass = 1;
                            //Printing
                            GlobalVariables.Printing(_scanData.RealWeight, GlobalVariables.IdLabel);
                            GlobalVariables.RealWeight = _scanData.RealWeight;
                            GlobalVariables.PrintApprove = true;
                        }
                        else//thung fail
                        {
                            GlobalVariables.PrintApprove = false;
                            if (_scanData.Decoration == 1)
                            {
                                GlobalVariables.RememberInfo.FailBoxPrinting += 1;
                            }
                            else
                            {
                                GlobalVariables.RememberInfo.FailBoxNoPrinting += 1;
                            }

                            //hien thi mau label
                            if (labWeight.InvokeRequired)
                            {
                                labWeight.Invoke(new Action(() => {
                                    labWeight.BackColor = Color.Red;
                                }));
                            }
                            else
                            {
                                labWeight.BackColor = Color.Red;
                            }
                        }

                        #region Log data

                        #endregion
                        #endregion
                    }
                    else
                    {
                        XtraMessageBox.Show($"Product number {_scanData.ProductNo} không có trong hệ thống. Xin hãy kiểm tra lại thông tin.", "CẢNH BÁO.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                #region tính toán các thông số cộng dồn số lượng và luu vao file remember

                string json = JsonConvert.SerializeObject(GlobalVariables.RememberInfo);
                File.WriteAllText(@"./RememberInfo.json", json);
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (GlobalVariables.PrintApprove)
            {
                GlobalVariables.PrintApprove = false;
            }
            else
            {
                GlobalVariables.PrintApprove = true;
            }
        }
    }
}