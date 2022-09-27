using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeightChecking
{
    public partial class frmScale : DevExpress.XtraEditors.XtraForm
    {
        private ScaleHelper _scaleHelper;
        private Task _ckTask;

        private tblScanDataModel scanData = new tblScanDataModel();
        private double realWeight = 0;

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
            this.labWeight.TextChanged += LabWeight_TextChanged;
            this.txtTest.KeyDown += TxtTest_KeyDown;

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
                    if (labWeight.InvokeRequired)
                    {
                        labWeight.Invoke(new Action(() =>
                        {
                            labWeight.Text = (o.Value * GlobalVariables.UnitScale).ToString();
                        }));
                    }
                    else
                    {
                        labWeight.Text = o.Value.ToString();
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
                scanData.RealWeight = _w;
            }

            Console.WriteLine($"Real weight: {scanData.RealWeight}");
        }

        private void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            //content of the QR code "OC283225,6112012227-2094-2651,28,13,P,1/56,160506,1/1|1,30.2022"

            if (e.KeyCode == Keys.Enter)
            {
                TextEdit _sen = sender as TextEdit;
                Console.WriteLine(_sen.Text);

                var _s = _sen.Text.Split('|');
                var _s1 = _s[0].Split(',');

                scanData.OcNo = _s1[0];
                scanData.ProductNo = _s1[1];

                #region truy vấn data và hiển thị giá trị lên các control
                //truy vấn thông tin 

                #region hiển thị thông tin
                if (labOcNo.InvokeRequired)
                {
                    labOcNo.Invoke(new Action(() => { labOcNo.Text = scanData.OcNo; }));
                }
                else labOcNo.Text = scanData.OcNo;

                if (labProductCode.InvokeRequired)
                {
                    labProductCode.Invoke(new Action(() => { labProductCode.Text = scanData.ProductNo; }));
                }
                else labProductCode.Text = scanData.ProductNo;
                #endregion

                #endregion

                #region xử lý so sánh khối lượng cân thực tế với kế hoạch để xử lý
                //thung hang Pass
                if (scanData.RealWeight >= scanData.StandardWeight - scanData.Tolerance
                    && scanData.RealWeight <= scanData.StandardWeight + scanData.Tolerance)
                {
                    if (scanData.Decoration == 1)
                    {
                        GlobalVariables.ReInfo.GoodBoxPrinting += 1;
                    }
                    else
                    {
                        GlobalVariables.ReInfo.GoodBoxNoPrinting += 1;
                    }
                }
                else//thung fail
                {
                    if (scanData.Decoration == 1)
                    {
                        GlobalVariables.ReInfo.FailBoxPrinting += 1;
                    }
                    else
                    {
                        GlobalVariables.ReInfo.FailBoxNoPrinting += 1;
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

        #region Printing
        // Print the file.
        public void Printing(double weight, string qrCode)
        {
            //content of the QR code "OC283225,6112012227-2094-2651,28,13,P,1/56,160506,1/1|1,30.2022"
            var _idLabel = qrCode.Split('|')[1].Split(',')[1];
            var rptRe = new rptLabel();
            //rptRe.DataSource = ds;

            rptRe.Parameters["Weight"].Value = weight;
            rptRe.Parameters["IdLabel"].Value = _idLabel;

            rptRe.CreateDocument();
            ReportPrintTool printToolCrush = new ReportPrintTool(rptRe);
            printToolCrush.Print();
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Printing(25.68, "OC283225,6112012227-2094-2651,28,13,P,1/56,160506,1/1|1,3000000000.2022");
        }
    }
}