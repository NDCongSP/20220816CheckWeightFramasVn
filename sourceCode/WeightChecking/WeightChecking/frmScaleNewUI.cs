using AutoUpdaterDotNET;
using Dapper;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Model.History;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightChecking.Models.Entities;

namespace WeightChecking
{
    public partial class frmScaleNewUI : DevExpress.XtraEditors.XtraForm
    {
        // Import để cho phép kéo form
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private Panel titleBar;
        private Button btnClose;
        private Button btnMaximize;
        private Button btnMinimize;
        private Button btnUpdateVersion;
        private Label titleText;

        private bool isUpdateClicked = false;

        byte[] _readHoldingRegisterArr = { 0, 0 };
        byte[] _writeHoldingRegisterArr = { 0, 0 };
        int _countDisconnectPlc = 0;
        private System.Threading.Tasks.Task _tskModbus;

        private bool _resetCounter = false;

        private ScaleHelper _scaleHelper;
        private Task _ckTask;

        private tblScanData _scanData = new tblScanData();

        private string _idLabel = null;
        private string _plr = null;// kiểu đóng thùng, P-đôi; L/R-left right

        private double _weight = 0, _boxWeight = 0, _accessoriesWeight = 0;

        private bool _approveUpdateActMetalScan = false;

        private BoxTypeEnum _boxType;

        private bool _resetUI = false;

        private string _unitLabel = string.Empty;
        private string _color = string.Empty;
        private string _sizeName = string.Empty;

        private CancellationTokenSource _resetUiCts;
        private Task _resetUiTask;

        private CancellationTokenSource _readModbus;
        private Task _readModbusTask;

        private CancellationTokenSource _timer;
        private Task _timerTask;

        private string _version = string.Empty;
        //private MesoInfoModel _mesoinfo = new MesoInfoModel();

        public frmScaleNewUI()
        {
            InitializeComponent();

            #region add header
            // Cấu hình form
            this.Text = "Custom Title Bar";
            this.FormBorderStyle = FormBorderStyle.None; // Bỏ header mặc định
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1920, 1080);

            // Tạo panel làm thanh tiêu đề
            titleBar = new Panel();
            titleBar.Dock = DockStyle.Top;
            titleBar.Height = 40;
            titleBar.BackColor = Color.Black;
            titleBar.MouseDown += TitleBar_MouseDown;
            this.Controls.Add(titleBar);

            // Nút Close
            btnClose = new Button();
            btnClose.Text = "";
            btnClose.ForeColor = Color.White;
            btnClose.BackColor = Color.Black;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Size = new Size(40, 40);
            btnClose.Location = new Point(this.Width - 40, 0);
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            // 1) Gán icon từ Resources (đặt tên hình là "updateVersion" như trong Resource)
            btnClose.Image = Properties.Resources.close_white_30;  // PNG từ Resources
            btnClose.ImageAlign = ContentAlignment.MiddleCenter;  // căn giữa
            btnClose.Padding = new Padding(0);                    // tránh lệch
            btnClose.TextImageRelation = TextImageRelation.Overlay; // chỉ icon
            btnClose.Click += BtnClose_Click;
            titleBar.Controls.Add(btnClose);

            // Nút Maximize
            btnMaximize = new Button();
            btnMaximize.Text = "";
            btnMaximize.ForeColor = Color.White;
            btnMaximize.BackColor = Color.Black;
            btnMaximize.FlatStyle = FlatStyle.Flat;
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.Size = new Size(40, 40);
            btnMaximize.Location = new Point(this.Width - 80, 0);
            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            // 1) Gán icon từ Resources (đặt tên hình là "updateVersion" như trong Resource)
            btnMaximize.Image = Properties.Resources.maximize_white_30;  // PNG từ Resources
            btnMaximize.ImageAlign = ContentAlignment.MiddleCenter;  // căn giữa
            btnMaximize.Padding = new Padding(0);                    // tránh lệch
            btnMaximize.TextImageRelation = TextImageRelation.Overlay; // chỉ icon
            btnMaximize.Click += BtnMaximize_Click;
            titleBar.Controls.Add(btnMaximize);

            // Nút Minimize
            btnMinimize = new Button();
            btnMinimize.Text = "";
            btnMinimize.ForeColor = Color.White;
            btnMinimize.BackColor = Color.Black;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.Size = new Size(40, 40);
            btnMinimize.Location = new Point(this.Width - 120, 0);
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            // 1) Gán icon từ Resources (đặt tên hình là "updateVersion" như trong Resource)
            btnMinimize.Image = Properties.Resources.minimize_white_30;  // PNG từ Resources
            btnMinimize.ImageAlign = ContentAlignment.MiddleCenter;  // căn giữa
            btnMinimize.Padding = new Padding(0);                    // tránh lệch
            btnMinimize.TextImageRelation = TextImageRelation.Overlay; // chỉ icon
            btnMinimize.Click += BtnMinimize_Click;
            titleBar.Controls.Add(btnMinimize);


            // Nút update version
            btnUpdateVersion = new Button();
            btnUpdateVersion.Text = "";                      // Không cần chữ, chỉ hiển thị icon
            btnUpdateVersion.ForeColor = Color.White;
            btnUpdateVersion.BackColor = Color.Black;
            btnUpdateVersion.FlatStyle = FlatStyle.Flat;
            btnUpdateVersion.FlatAppearance.BorderSize = 0;
            btnUpdateVersion.Size = new Size(40, 40);
            btnUpdateVersion.Location = new Point(this.Width - 160, 0);
            btnUpdateVersion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdateVersion.Cursor = Cursors.Hand;

            // 1) Gán icon từ Resources (đặt tên hình là "updateVersion" như trong Resource)
            btnUpdateVersion.Image = Properties.Resources.arrow_upward_white_30;  // PNG từ Resources
            btnUpdateVersion.ImageAlign = ContentAlignment.MiddleCenter;  // căn giữa
            btnUpdateVersion.Padding = new Padding(0);                    // tránh lệch
            btnUpdateVersion.TextImageRelation = TextImageRelation.Overlay; // chỉ icon

            // Tùy chọn: scale icon nếu quá lớn/nhỏ (WinForms Button không có ImageLayout)
            // => bạn có thể dùng phiên bản icon 24x24 hoặc 32x32 trong file PNG để vừa với nút 40x40.

            // 2) Tooltip khi hover
            var tip = new ToolTip();
            tip.AutoPopDelay = 5000;     // hiển thị tối đa 5 giây
            tip.InitialDelay = 300;      // trễ 300ms
            tip.ReshowDelay = 100;       // xuất hiện lại nhanh
            tip.ShowAlways = true;       // luôn hiển thị tooltip
            tip.SetToolTip(btnUpdateVersion, "Click to update version");  // nội dung tooltip

            // Tùy chọn: hiệu ứng hover (đổi nền cho dễ nhìn)
            btnUpdateVersion.MouseEnter += (s, e) => btnUpdateVersion.BackColor = Color.FromArgb(30, 30, 30);
            btnUpdateVersion.MouseLeave += (s, e) => btnUpdateVersion.BackColor = Color.Black;

            // Sự kiện Click (giữ nguyên như bạn đã có)
            btnUpdateVersion.Click += BtnUpdateVersion_Click; ; // hoặc sự kiện update version thực tế của bạn
            titleBar.Controls.Add(btnUpdateVersion);


            // Đảm bảo tất cả có cùng Height = 30 và Y = 5
            btnClose.Size = btnMaximize.Size = btnMinimize.Size = btnUpdateVersion.Size = new Size(30, 30);


            // Anchor cho cả 3 nút
            btnClose.Anchor = btnMaximize.Anchor = btnMinimize.Anchor = btnUpdateVersion.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Logo
            PictureBox logo = new PictureBox();
            logo.Image = Properties.Resources.framas__white_; // logo từ Resources
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Size = new Size(100, 30); // kích thước logo
            logo.Location = new Point(10, 5); // vị trí bên trái
            titleBar.Controls.Add(logo);

            // Text
            titleText = new Label();
            titleText.Text = $"fVN - SSFG Station {GlobalVariables.Station.ToString()}";
            titleText.ForeColor = Color.White;
            titleText.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            titleText.AutoSize = true;
            titleText.Location = new Point(120, 10); // ngay sau logo
            titleBar.Controls.Add(titleText);
            #endregion

            Load += FrmScaleNewUI_Load;
            FormClosing += FrmScaleNewUI_FormClosing;
        }

        private void FrmScaleNewUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _resetUiCts?.Cancel();
                _resetUiTask?.Wait(1000); // đợi nhẹ, tránh treo UI

                _readModbus?.Cancel();
                _readModbusTask?.Wait(1000); // đợi nhẹ, tránh treo UI

                _timer?.Cancel();
                _timerTask?.Wait(1000); // đợi nhẹ, tránh treo UI

                //huy doi tuong can
                _scaleHelper.StopScale = true;
                _ckTask.Wait();
                _ckTask.Dispose();
                _scaleHelper.Dispose();
                GlobalVariables.ScaleStatus = "Disconnect";
            }
            catch { /* ignore */ }
            finally
            {
                _resetUiCts?.Dispose();
                _resetUiCts = null;
                _resetUiTask = null;

                _readModbus?.Dispose();
                _readModbus = null;
                _readModbusTask = null;

                _timer?.Dispose();
                _timer = null;
                _timerTask = null;
            }
        }



        private void FrmScaleNewUI_Load(object sender, EventArgs e)
        {
            //using var dbContext = new ApplicationDbContextSSFG(GlobalVariables.ConnectionString);
            //_mesoinfo = dbContext.Database.SqlQuery<MesoInfoModel>($"sp_GetMesoInfo").AsEnumerable().FirstOrDefault();

            //var location = _mesoinfo.MESOCOMP == "VNT1" ? "fVN" :
            //              _mesoinfo.MESOCOMP == "FKV" ? "fKV" :
            //              _mesoinfo.MESOCOMP == "FTT1" ? "fFT" :
            //              _mesoinfo.MESOCOMP == "05FI" ? "fIN" :
            //              _mesoinfo.MESOCOMP == "fGE" ? "fGE" : "Unknown";

            //if (Enum.TryParse<EnumLocation>(location, ignoreCase: true, out var loc))
            //{
            //    titleText.Text = $"{loc} - SSFG Station";
            //}
            _version = System.Windows.Forms.Application.ProductVersion.Split('+')[0];

            _labLastResultMessage.Text = string.Empty;

            ResetControl();

            _labLastResultMessage.Text = null;

            this.txtQrCode.Focus();
            txtQrCode.KeyDown += TxtQrCode_KeyDown;
            _btnReprint.Click += _btnReprint_Click;

            #region Ket noi modbus RTU PLC metalScan counter
            if (GlobalVariables.ConfigJson.IsCounter)
            {
                GlobalVariables.ModbusStatus = GlobalVariables.MyDriver.ModbusRTUMaster.KetNoi(GlobalVariables.ConfigJson.ComPort, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);

                Console.WriteLine($"PLC Status: {GlobalVariables.ModbusStatus}");

                if (GlobalVariables.ModbusStatus)
                {
                    //_tskModbus = new System.Threading.Tasks.Task(() => TaskReadModbus());
                    //_tskModbus.Start();
                    //_ = TaskReadModbus();
                    _readModbus = new CancellationTokenSource();
                    _readModbusTask = Task.Run(() => TaskReadModbusAsync(_readModbus.Token));
                }
                else
                {
                    MessageBox.Show($"Không thể kết nối được bộ đếm dò kim loại.{Environment.NewLine}Tắt phần mềm, kiểm tra lại kết nối với PLC rồi mở lại phần mềm.",
                                    "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //chi đăng ký sự kiện bật tắt đèn tháp báo thùng pass/fail cho trạm kerry
                if (GlobalVariables.Station == StationEnum.Kerry)
                {
                    GlobalVariables.MyEvent.EventHandleStatusLightPLC += (s, o) =>
                    {
                        if (o.StatusLight)
                        {
                            GlobalVariables.ModbusStatus = GlobalVariables.MyDriver.ModbusRTUMaster.WriteMultipleCoils(1, 2048, 2, new bool[] { false, true });
                        }
                        else
                        {
                            GlobalVariables.ModbusStatus = GlobalVariables.MyDriver.ModbusRTUMaster.WriteMultipleCoils(1, 2048, 2, new bool[] { true, false });
                        }
                    };
                }
            }
            #endregion

            #region Register events Scale value change
            if (GlobalVariables.ConfigJson.IsScale)
            {
                _scaleHelper = new ScaleHelper()
                {
                    Ip = GlobalVariables.ConfigJson.IpScale,
                    Port = Convert.ToInt32(GlobalVariables.PortScale),
                    ScaleDelay = GlobalVariables.ScaleDelay,
                    StopScale = false
                };

                _scaleHelper.StatusChanged += (s, o) =>
                {
                    GlobalVariables.ScaleStatus = o.StatusConnection;
                    Console.WriteLine($"Scale {o}");
                };

                //tamm ngung doc can
                _ckTask = new Task(() => _scaleHelper.CheckConnect());
                _ckTask.Start();

                _scaleHelper.ValueChanged += (s, o) =>
                {
                    try
                    {
                        var w = Math.Round(o.Value * GlobalVariables.ConfigJson.UnitScale, 3);
                        GlobalVariables.RealWeight = w;
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

            this.ActiveControl = null;
            this.ActiveControl = txtQrCode;

            //tạo 1 task chạy độc lập
            // Fire-and-forget background task
            _timer = new CancellationTokenSource();
            _timerTask = Task.Run(() => TaskTimerAsync(_timer.Token));

            _resetUiCts = new CancellationTokenSource();
            _resetUiTask = Task.Run(() => TaskCheckResetUIAsync(_resetUiCts.Token));
        }

        private async void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                var errorFlag = false;
                try
                {
                    GlobalVariables.InvokeIfRequired(this, () =>
                    {
                        _labLastResultMessage.Text = null;
                    });

                    _scanData = new tblScanData();

                    _scanData.GrossWeight = double.TryParse(labScaleValue.Text, out double value) ? value : 0;
                    GlobalVariables.RealWeight = _scanData.GrossWeight;
                    _scanData.CreatedBy = GlobalVariables.UserLoginInfo.Id;
                    _scanData.Station = GlobalVariables.Station;

                    bool specialCase = false;//dùng có các trường hợp hàng PU, trên WL decpration là 0, nhưng QC phân ra printing 0-1. beforePrinting thì get theo
                                             //printing=0; afterPrinting thì get theo printing=1. 6112012228

                    //biến dùng để check xem thùng đó có trong bảng scanData hay chưa.
                    int statusLogData = 0;//0-chưa có;1-đã có dòng fail;2-đã có dòng pass;3-đã có cả fail và pass
                    bool isFail = false;
                    bool isPass = false;

                    double lowerToleranceOfBox = 0, upperToleranceOfBox = 0;

                    double ratioFailWeight = 0;//biến chứa ratioFailWeight của lần fail trước

                    BoxParentModel boxParent;// = new BoxParentModel();//biến báo là thùng này đã được in lại tem rồi, cần xử lý theo hướng in lại tem

                    TextBox _sen = sender as TextBox;
                    Console.WriteLine(_sen.Text);

                    #region xử lý barcode lấy ra các giá trị theo code
                    _scanData.BarcodeString = _sen.Text.Trim();

                    string ocFirstChar = string.Empty;

                    ocFirstChar = _scanData.BarcodeString.Substring(0, 2);

                    GlobalVariables.SystemOC.ForEach(o =>
                    {
                        //OC HC P241226018
                        if ((ocFirstChar.Contains(o.FirstChar) && Regex.IsMatch(ocFirstChar, @"\d")) || ocFirstChar == "HC")
                        {
                            ocFirstChar = _scanData.BarcodeString.Substring(0, 1);
                        }
                    });

                    //Check xem  QR code quét vào có đúng định dạng hay ko
                    var resultCheckOc = GlobalVariables.OcUsingList.FirstOrDefault(x => x.OcFirstChar == ocFirstChar);

                    if (_scanData.BarcodeString.Contains("|"))
                    {
                        var s = _sen.Text.Split('|');
                        var s1 = s[0].Split(',');
                        _scanData.Unit = _plr = s1[4];//get Thung này đóng theo đôi (P) hay L/R

                        ////Check xem  QR code quét vào có đúng định dạng hay ko
                        //var resultCheckOc = GlobalVariables.OcUsingList.FirstOrDefault(x => x.OcFirstChar == ocFirstChar);

                        if (resultCheckOc != null)
                        {
                            _scanData.OcNo = s1[0].Trim();

                            #region kiểm tra xem thùng này có bị in tem lụi lại tem hay không để xử lý cho đúng với flowChart
                            using (var dbContextDogeWH = new ApplicationDbContextDogeWH(GlobalVariables.ConfigJson.ConStringDogeWH))
                            {
                                boxParent = dbContextDogeWH.Database
                                    .SqlQuery<BoxParentModel>("sp_IdcSsfgPrintedLabels_OC_IndexCheck @OcNo ={0}", _scanData.OcNo)
                                    .FirstOrDefault();

                                if (boxParent != null)
                                {
                                    _scanData.ParentOc = boxParent.ParentOc;
                                    _scanData.ParentBoxId = boxParent.ParentBoxCode;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            //throw new Exception("The QR code is incorrect. Delete it and scan again.");
                            throw new Exception("QR code không đúng định dang. Vui lòng quét đúng tem FG.");
                        }

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
                            _scanData.IdLabel = GlobalVariables.IdLabel;

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
                        _scanData.Unit = _plr = s1[4];//get Thung này đóng theo đôi (P) hay L/R

                        //Check xem  QR code quét vào có đúng định dạng hay ko
                        //var resultCheckOc = GlobalVariables.OcUsingList.FirstOrDefault(x => x.OcFirstChar == ocFirstChar);

                        if (resultCheckOc != null)
                        {
                            _scanData.OcNo = s1[0].Trim();

                            #region kiểm tra xem thùng này có bị in tem lụi lại tem hay không để xử lý cho đúng với flowChart
                            using (var dbContext = new ApplicationDbContextDogeWH(GlobalVariables.ConfigJson.ConStringDogeWH))
                            {
                                boxParent = await dbContext.Database.SqlQuery<BoxParentModel>("sp_IdcSsfgPrintedLabels_OC_IndexCheck @OcNo = {0}", _scanData.OcNo)
                                    .FirstOrDefaultAsync();

                                if (boxParent != null)
                                {
                                    _scanData.ParentOc = boxParent.ParentOc;
                                    _scanData.ParentBoxId = boxParent.ParentBoxCode;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            GlobalVariables.ResultPosting.Message = string.Empty;
                            //throw new Exception("The QR code is incorrect. Delete it and scan again.");
                            throw new Exception("QR code không đúng định dang. Vui lòng quét đúng tem FG.");
                        }

                        //_scanData.OcNo = s1[0];
                        _scanData.ProductNumber = s1[1];

                        _scanData.Quantity = Convert.ToInt32(s1[2]);
                        _scanData.LinePosNo = s1[3];
                        _scanData.BoxNo = s1[5];
                    }

                    #region check special case
                    foreach (var item in GlobalVariables.SpecialCaseList)
                    {
                        if (_scanData.ProductNumber.Split('-')[0].Equals(item.MainItem))
                        {
                            specialCase = true;
                            break;
                        }
                    }

                    //if (_scanData.ProductNumber.Contains("6112012228"))
                    //{
                    //    specialCase = true;
                    //}
                    #endregion

                    GlobalVariables.OcNo = _scanData.OcNo;
                    GlobalVariables.BoxNo = _scanData.BoxNo;
                    #endregion

                    #region truy vấn data và xử lý
                    //truy vấn thông tin 
                    using (var dbContext = new ApplicationDbContext(GlobalVariables.ConnectionString))
                    {
                        #region Kiểm tra xem thùng này đã được log vào scanData chưa
                        var qrSplit = _scanData.BarcodeString.Split('|')[0].Split(',');
                        var oc = qrSplit[0];
                        var unit = qrSplit[4];
                        var boxNo = qrSplit[5];
                        var checkExists = dbContext.TblScanDatas
                            .Where(x => x.Actived == 1 &&
                                    x.OcNo == oc &&
                                    x.BoxNo == boxNo &&
                                    x.Unit == unit
                                   )
                            .ToList();

                        if (checkExists != null && checkExists?.Count > 0)
                        {
                            checkExists.Where(x => x.BarcodeString != _scanData.BarcodeString)
                                .ToList()
                                .ForEach(x => x.Actived = 0);
                            dbContext.SaveChanges();
                        }

                        var checkInfo = checkExists?.Where(x => x.Actived == 1).ToList();

                        foreach (var item in checkInfo)
                        {
                            if (item.Actived == 1)
                            {
                                if (
                                    (item.Pass == 1 && (item.Status == 2 || GlobalVariables.Station == StationEnum.IDC))
                                    //|| (item.Pass == 0 && item.ActualDeviationPairs == 0 && item.ApprovedBy != Guid.Empty)
                                    || (item.Pass == 0 && item.Status == 2 && item.ActualDeviationPairs == 0)
                                    )
                                {
                                    //if (!_scanData.OcNo.Contains("PR"))
                                    //{
                                    //    isPass = true;
                                    //}
                                    //else if (_scanData.OcNo.Contains("PR") && GlobalVariables.AfterPrinting == 0 && item.Status == 1)
                                    //{
                                    //    isPass = true;
                                    //}
                                    //else if (_scanData.OcNo.Contains("PR") && GlobalVariables.AfterPrinting == 1 && item.Status == 2)
                                    //{
                                    //    isPass = true;
                                    //}

                                    isPass = true;
                                }
                                else if (
                                            (item.Pass == 0 && item.Status == 0)// && item.ActualDeviationPairs != 0 && item.ApprovedBy != Guid.Empty)
                                            || (item.Pass == 0 && item.Status == 2 && item.ActualDeviationPairs != 0)
                                        )
                                {
                                    isFail = true;
                                    //tính tỷ lệ khối lượng số đôi lỗi/ StdGrossWeight
                                    ratioFailWeight = Math.Round((Math.Abs(item.DeviationPairs) * item.AveWeight1Prs) / item.StdGrossWeight, 3);

                                    //this.Invoke((MethodInvoker)delegate { labRatioFail.Text = ratioFailWeight.ToString(); });
                                    //if (!_scanData.OcNo.Contains("PR"))
                                    //{
                                    //    isFail = true;
                                    //}
                                    //else if (_scanData.OcNo.Contains("PR") && GlobalVariables.AfterPrinting == 0 && item.Station == 0)
                                    //{
                                    //    isFail = true;
                                    //}
                                    //else if (_scanData.OcNo.Contains("PR") && GlobalVariables.AfterPrinting == 1 && item.Station != 0)
                                    //{
                                    //    isFail = true;
                                    //}
                                }
                            }
                        }

                        if (!isPass && !isFail)
                        {
                            statusLogData = 0;
                        }
                        else if (!isPass && isFail)
                        {
                            statusLogData = 1;
                        }
                        else if (isPass && !isFail)
                        {
                            statusLogData = 2;
                        }
                        else if (isPass && isFail)
                        {
                            statusLogData = 3;
                        }
                        #endregion

                        //đối với hàng sơn PU, thì trước sơn lấy các giá trị theo printing =0. Sau sơn thì lấy các giá trị theo printing = 1.
                        //nếu checkOc == null --> hàng sơn- trước sơn (PRT).
                        var checkOc = GlobalVariables.OcUsingList.FirstOrDefault(x => x.OcFirstChar == ocFirstChar && ocFirstChar != "PR");

                        var printingCheck = 0;
                        if (specialCase)
                        {
                            //after printing
                            if (checkOc != null || (ocFirstChar == "PR" && GlobalVariables.ConfigJson.AfterPrinting != 0))
                            {
                                printingCheck = 1;
                            }
                            else//before printing
                            {
                                printingCheck = 0;
                            }
                        }

                        var res = dbContext.Database.SqlQuery<ProductInfoModel>("sp_vProductItemInfoGet @ProductNumber= {0}, @SpecialCase = {1}, @Printing = {2}",
                                _scanData.ProductNumber, specialCase, printingCheck
                            )
                            .FirstOrDefault();

                        if (res != null)
                        {
                            //có thể dựa vào category để biết được thùng đó đóng theo đôi hay L/R
                            //_scanData.Category = 1 là HC; 0 là Non-HC
                            _scanData.IsHc = res.ProductCategory == 1 ? true : false;

                            _unitLabel = _scanData.Unit == "P" ? "prs" : "pcs";
                            _color = res.Color;
                            _sizeName = res.SizeName;

                            _scanData.ProductName = res.ProductName;
                            _scanData.Decoration = (int)res.Decoration;
                            _scanData.MetalScan = (int)res.MetalScan;
                            _scanData.Brand = res.Brand;
                            _scanData.AveWeight1Prs = (double)res.AveWeight1Prs;

                            if (_scanData.AveWeight1Prs != 0)
                            {
                                #region Fill data from coreData to scanData, tính toán ra NetWeight và GrossWeight
                                //Xét điều kiện để lấy boxWeight. Nếu là hàng đi sơn thì dùng thùng nhựa

                                _scanData.Status = 2;//báo trạng thái hàng ko đi sơn, hoặc hàng sơn đã được sơn rồi

                                //lấy tolerance theo thùng giấy
                                lowerToleranceOfBox = (double)res.LowerToleranceOfCartonBox;
                                upperToleranceOfBox = (double)res.UpperToleranceOfCartonBox;

                                #region get box weight
                                if (_scanData.Quantity <= res.BoxQtyBx6)
                                {
                                    _scanData.BoxWeight = (double)res.BoxWeightBx6;
                                    _boxType = BoxTypeEnum.BX6;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx6 && _scanData.Quantity <= res.BoxQtyBx5)
                                {
                                    _scanData.BoxWeight = (double)res.BoxWeightBx5;
                                    _boxType = BoxTypeEnum.BX5;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx5 && _scanData.Quantity <= res.BoxQtyBx4)
                                {
                                    _scanData.BoxWeight = (double)res.BoxWeightBx4;
                                    _boxType = BoxTypeEnum.BX4;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx4 && _scanData.Quantity <= res.BoxQtyBx3)
                                {
                                    _scanData.BoxWeight = (double)res.BoxWeightBx3;
                                    _boxType = BoxTypeEnum.BX3;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx3 && _scanData.Quantity <= res.BoxQtyBx2)
                                {
                                    _scanData.BoxWeight = (double)res.BoxWeightBx2;
                                    _boxType = BoxTypeEnum.BX2;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx2 && _scanData.Quantity <= res.BoxQtyBx1A)
                                {
                                    _scanData.BoxWeight = (double)res.BoxWeightBx1A;
                                    _boxType = BoxTypeEnum.BX1A;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx1A && _scanData.Quantity <= res.BoxQtyBx1)
                                {
                                    _scanData.BoxWeight = (double)res.BoxWeightBx1;
                                    _boxType = BoxTypeEnum.BX1;
                                }
                                else if (_scanData.Quantity > res.BoxQtyBx1)
                                {
                                    var itemInserrt = new tblItemMissingInfo()
                                    {
                                        Id = Guid.NewGuid(),
                                        CreatedDate = DateTime.Now,
                                        IsActive = true,
                                        ProductNumber = _scanData.ProductNumber,
                                        ProductName = _scanData.ProductName,
                                        OcNum = _scanData.OcNo,
                                        Note = $"Quantity over the BX1 box limit ({res.BoxQtyBx1}).",
                                        QrCode = _scanData.BarcodeString
                                    };

                                    dbContext.TblItemMissingInfos.Add(itemInserrt);
                                    dbContext.SaveChanges();

                                    #region Auto posting
                                    //hàng từ production qua: decoration = 0 (OC)  và dcoration = 1 (PRT). transfer từ kho 3--> 64
                                    //if (_scanData.Decoration == 0)
                                    //{
                                    //    GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 3, 64, GlobalVariables.GetDbConnectionDogeWh(), null);
                                    //    GlobalVariables.ResultPosting.Message = $"Hàng Production lỗi đóng gói (Transfer 3-->64): {GlobalVariables.ResultPosting.Message}";
                                    //}
                                    ////hàng sơn-sau sơn
                                    //else if (_scanData.Decoration == 1 && checkOc != null)
                                    //{
                                    //    GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 32, 64, GlobalVariables.GetDbConnectionDogeWh(), null);
                                    //    GlobalVariables.ResultPosting.Message = $"Hàng QC lỗi đóng gói (Transfer 32-->64): {GlobalVariables.ResultPosting.Message}";
                                    //}
                                    #endregion

                                    throw new Exception($"Product number {_scanData.ProductNumber} có số lượng vượt quá giới hạn thùng BX1. Số lượng đóng gói thùng BX1: ({res.BoxQtyBx1})");
                                }
                                #endregion

                                if (_scanData.MetalScan == 0)
                                {
                                    _approveUpdateActMetalScan = false;
                                }
                                else
                                {
                                    GlobalVariables.RememberInfo.MetalScan += 1;

                                    _approveUpdateActMetalScan = true;
                                }

                                _scanData.StdNetWeight = Math.Round(_scanData.Quantity * _scanData.AveWeight1Prs, 3);
                                //_scanData.Tolerance = Math.Round(_scanData.StdNetWeight * (res.Tolerance / 100), 3);
                                _scanData.LowerTolerance = -Math.Round(_scanData.StdNetWeight * (lowerToleranceOfBox / 100), 3);
                                _scanData.UpperTolerance = Math.Round(_scanData.StdNetWeight * (upperToleranceOfBox / 100), 3);

                                //luu ý các Quantity partition-Plasic-WrapSheet trên DB nó là tính số Prs
                                //sau khi đọc về phải lấy QtyPrs quét trên label / Quantity partition-Plasic-WrapSheet ==> qty * weight ==> Weight package weight
                                double partitionWeight = 0;

                                #region Tính số tấm lót partition
                                double p = 0;

                                //với hàng FG outsole thì tính ra được số lượng partition thì trừ đi 1 để ra số đúng
                                if (res.ProductCategory != 1)//OS - 1:HC
                                {
                                    p = res.PartitionQty != 0 ? ((double)_scanData.Quantity / (double)res.PartitionQty) : 0;
                                    p = p - 1;
                                    if (p < 0) p = 0;

                                    partitionWeight = Math.Floor(p) * (double)res.PartitionWeight;
                                }
                                //với hàng HC thì lấy số lượng partition = DB.
                                else if (res.ProductCategory == 1)
                                {
                                    switch (_boxType)
                                    {
                                        case BoxTypeEnum.BX3:
                                            p = (double)res.PartitionQtyOfBX3;
                                            break;
                                        case BoxTypeEnum.BX2:
                                            p = (double)res.PartitionQtyOfBX2;
                                            break;
                                        case BoxTypeEnum.BX1A:
                                            p = (double)res.PartitionQtyOfBX1A;
                                            break;
                                    }

                                    partitionWeight = p * (double)res.PartitionWeight;
                                }
                                #endregion

                                //partitionWeight = res.PartitionQty != 0 ? (_scanData.Quantity / res.PartitionQty) * res.PartitionWeight : 0;
                                var plasicBag1Weight = res.PlasticBag1Qty != 0 ? Math.Ceiling(((double)_scanData.Quantity / (double)res.PlasticBag1Qty)) * res.PlasticBag1Weight : 0;
                                var plasicBag2Weight = res.PlasticBag2Qty != 0 ? Math.Ceiling(((double)_scanData.Quantity / (double)res.PlasticBag2Qty)) * res.PlasticBag2Weight : 0;
                                var wrapSheetWeight = res.WrapSheetQty != 0 ? Math.Ceiling(((double)_scanData.Quantity / (double)res.WrapSheetQty)) * res.WrapSheetWeight : 0;
                                var foamSheetWeight = res.FoamSheetQty != 0 ? Math.Ceiling(((double)_scanData.Quantity / (double)res.FoamSheetQty)) * res.FoamSheetWeight : 0;

                                _scanData.PackageWeight = Math.Round((double)partitionWeight + (double)plasicBag1Weight + (double)plasicBag2Weight + (double)wrapSheetWeight + (double)foamSheetWeight, 3);

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

                                #region xử lý so sánh khối lượng cân thực tế với kế hoạch để xử lý
                                _scanData.NetWeight = Math.Round(_scanData.GrossWeight - _scanData.BoxWeight - _scanData.PackageWeight, 3);
                                _scanData.Deviation = Math.Round(_scanData.NetWeight - _scanData.StdNetWeight, 3);

                                #region tính toán số pairs chênh lệch và hiển thị label
                                //var nwPlus = _scanData.StdNetWeight + _scanData.Tolerance;
                                //var nwSub = _scanData.StdNetWeight - _scanData.Tolerance;
                                var nwPlus = _scanData.StdNetWeight + _scanData.UpperTolerance;
                                var nwSub = _scanData.StdNetWeight + _scanData.LowerTolerance;

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
                                #endregion

                                //tính lại tỷ lệ khối lượng số đôi lỗi/ StdGrossWeight của lần scan này để log
                                _scanData.RatioFailWeight = Math.Round((Math.Abs(_scanData.DeviationPairs) * _scanData.AveWeight1Prs) / _scanData.StdGrossWeight, 3);

                                //hien thi cac thong so dem
                                ShowUI();

                                //thung hang Pass
                                if (_scanData.DeviationPairs == 0)
                                {
                                    _scanData.Pass = 1;//báo thùng pass
                                    _scanData.CreatedDate = GlobalVariables.CreatedDate = DateTime.Now;//lấy thời gian để đồng bộ giữa in tem và log DB
                                                                                                       //Printing
                                                                                                       //bật tín hiệu để PLC on đèn xanh
                                    GlobalVariables.MyEvent.StatusLightPLC = true;

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
                                    GlobalVariables.InvokeIfRequired(this, () =>
                                    {
                                        _labResultMessage.Text = "Everything OK";
                                        _labResult.Text = "PASSED";
                                        _labResult.BackColor = Color.Green;
                                        _labResult.ForeColor = Color.White;
                                    });
                                    #endregion

                                    //lấy lại ID của thùng lỗi này trong hệ thống để cho in lại tem rồi cập nhật thông tin người approved vào.
                                    tblScanData resultCheckBoxInfo = new tblScanData();

                                    //nếu ko phải là thùng bị in tem lụi (in lại tem)
                                    if (boxParent == null)
                                    {
                                        //resultCheckBoxInfo = dbContext.Query<tblScanDataModel>("sp_tblScanDataGetByQrCode", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                                        resultCheckBoxInfo = dbContext.TblScanDatas
                                            .FirstOrDefault(x => x.BarcodeString == _scanData.BarcodeString &&
                                            x.Actived == 1 && x.Pass == 0);
                                    }
                                    else
                                    {
                                        //resultCheckBoxInfo = dbContext.Query<tblScanDataModel>("sp_tblScanDataGetByQrCode", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                                        resultCheckBoxInfo = dbContext.TblScanDatas
                                            .FirstOrDefault(x => x.Actived == 1 &&
                                                x.OcNo == boxParent.ParentOc &&
                                                x.BoxNo == boxParent.ParentBoxCode
                                             );
                                    }

                                    //kiểm tra xem data đã có trên hệ thống hay chưa
                                    //Check fail recorded?
                                    if (statusLogData == 0)
                                    {
                                        #region  trường hợp in tem lụi lại tem thì vào xử lý để cập nhật deviation cho đúng theo motherBox
                                        if (boxParent != null)
                                        {
                                            //Trường hợp 1: thùng mẹ bị dư hàng
                                            //khi đó sẽ lấy số lượng dư in tem lụi 1 con tem mới và đóng 1 thùng mới với số lượng dư đó
                                            //tem mẹ vẫn lưu hành
                                            if (resultCheckBoxInfo.Quantity + resultCheckBoxInfo.ActualDeviationPairs > resultCheckBoxInfo.Quantity)
                                            {
                                                //cập nhật actual deviation cho thùng mẹ

                                                resultCheckBoxInfo.ActualDeviationPairs = _scanData.Quantity;
                                                resultCheckBoxInfo.Status = 2;
                                                dbContext.TblScanDatas.AddOrUpdate(resultCheckBoxInfo);

                                                #region Update actual deviation for approvedPrint
                                                var checkUpdate = dbContext.TblApprovedPrintLabels
                                                      .Where(x => x.ScanDataId == resultCheckBoxInfo.Id).ToList();
                                                checkUpdate?.ForEach(x => x.ActualDeviationPairs = resultCheckBoxInfo.Quantity);
                                                #endregion
                                            }
                                            //Trường hợp 2: thùng mẹ bị thiếu hàng
                                            //khi đó sẽ in tem lụi 1 con tem mới. thay cho tem mẹ
                                            //tem mẹ sẽ bị hủy.
                                            else
                                            {
                                                //cập nhật actual deviation cho thùng mẹ
                                                resultCheckBoxInfo.ActualDeviationPairs = _scanData.Quantity - resultCheckBoxInfo.Quantity;
                                                resultCheckBoxInfo.Status = 2;
                                                dbContext.TblScanDatas.AddOrUpdate(resultCheckBoxInfo);
                                            }
                                            dbContext.SaveChanges();
                                        }
                                        #endregion

                                        GlobalVariables.Printing((_scanData.GrossWeight / 1000).ToString("#,#0.00")
                                                    , !string.IsNullOrEmpty(GlobalVariables.IdLabel) ? GlobalVariables.IdLabel : $"{_scanData.OcNo}|{_scanData.BoxNo}", true
                                                     , _scanData.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                                                     , _scanData.IsHc);

                                        #region Auto posting
                                        //hàng từ production qua: decoration = 0 (OC). transfer từ kho 3--> 64
                                        //if (_scanData.Decoration == 0)
                                        //{
                                        //    GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 3, 41, GlobalVariables.GetDbConnectionDogeWh(), null);

                                        //    if (GlobalVariables.ResultPosting.Message == "Successful")
                                        //    {
                                        //        GlobalVariables.ResultPosting.Message = $"Hàng Production OK (Transfer 3-->41): {GlobalVariables.ResultPosting.Message}";
                                        //    }
                                        //    else
                                        //    {
                                        //        GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 63, 41, GlobalVariables.GetDbConnectionDogeWh(), null);
                                        //        GlobalVariables.ResultPosting.Message = $"Hàng Production OK (Metal - Transfer 63-->41): {GlobalVariables.ResultPosting.Message}";
                                        //    }
                                        //}
                                        ////hàng sơn-sau sơn
                                        //else if (_scanData.Decoration == 1 && checkOc != null)
                                        //{
                                        //    GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 32, 41, GlobalVariables.GetDbConnectionDogeWh(), null);

                                        //    if (GlobalVariables.ResultPosting.Message == "Successful")
                                        //    {
                                        //        GlobalVariables.ResultPosting.Message = $"Hàng QC OK (Transfer 32-->41): {GlobalVariables.ResultPosting.Message}";
                                        //    }
                                        //    else
                                        //    {
                                        //        GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 63, 41, GlobalVariables.GetDbConnectionDogeWh(), null);
                                        //        GlobalVariables.ResultPosting.Message = $"Hàng QC OK (Metal - Transfer 63-->41): {GlobalVariables.ResultPosting.Message}";
                                        //    }
                                        //}
                                        #endregion
                                    }
                                    //với thùng Pass mà trước đó đã cân và báo fail thì popup form nhập deviation
                                    else if (statusLogData == 1)
                                    {
                                        using (var formDeviation = new frmTypingDeviation())
                                        {
                                            var resultForm = formDeviation.ShowDialog();

                                            if (resultForm == DialogResult.OK)
                                            {
                                                if (resultCheckBoxInfo != null)
                                                {
                                                    var dialogResult = MessageBox.Show($"Bạn có chắc chắn xác nhận cập nhật số lượng chênh lệch thực tế cho thùng với thông tin sau:" +
                                                                                         $"{Environment.NewLine}{_scanData.IdLabel}|{_scanData.OcNo}|{_scanData.BoxNo}.{Environment.NewLine}" +
                                                                                         $"Số lượng lệch thực tế là: {formDeviation.ActualDeviation}?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                                    if (dialogResult == DialogResult.Yes)
                                                    {
                                                        //gán giá trị trả về từ form nhập deviation vào model get data
                                                        resultCheckBoxInfo.ActualDeviationPairs = formDeviation.ActualDeviation;
                                                        resultCheckBoxInfo.ApprovedBy = formDeviation.QrConfirm;

                                                        #region  trường hợp in tem lụi lại tem thì vào xử lý để cập nhật deviation cho đúng theo motherBox
                                                        if (boxParent != null)
                                                        {
                                                            //Trường hợp 1: thùng mẹ bị dư hàng
                                                            //khi đó sẽ lấy số lượng dư in tem lụi 1 con tem mới và đóng 1 thùng mới với số lượng dư đó
                                                            //tem mẹ vẫn lưu hành
                                                            if (resultCheckBoxInfo.Quantity + resultCheckBoxInfo.ActualDeviationPairs > resultCheckBoxInfo.Quantity)
                                                            {
                                                                //cập nhật actual deviation cho thùng mẹ
                                                                resultCheckBoxInfo.ActualDeviationPairs = _scanData.Quantity;
                                                                resultCheckBoxInfo.Status = 2;
                                                                dbContext.TblScanDatas.AddOrUpdate(resultCheckBoxInfo);

                                                                //Update actual deviation for approvedPrint
                                                                var checkUpdate = dbContext.TblApprovedPrintLabels
                                                                                        .Where(x => x.ScanDataId == resultCheckBoxInfo.Id).ToList();
                                                                checkUpdate?.ForEach(x => x.ActualDeviationPairs = resultCheckBoxInfo.Quantity);
                                                            }
                                                            //Trường hợp 2: thùng mẹ bị thiếu hàng
                                                            //khi đó sẽ in tem lụi 1 con tem mới. thay cho tem mẹ
                                                            //tem mẹ sẽ bị hủy.
                                                            else
                                                            {
                                                                resultCheckBoxInfo.ActualDeviationPairs = _scanData.Quantity - resultCheckBoxInfo.Quantity;
                                                                resultCheckBoxInfo.Status = 2;
                                                                dbContext.TblScanDatas.AddOrUpdate(resultCheckBoxInfo);
                                                            }
                                                            dbContext.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            resultCheckBoxInfo.Status = 2;
                                                            dbContext.TblScanDatas.AddOrUpdate(resultCheckBoxInfo);

                                                            #region Log approvedPrint
                                                            var itemInsert = new tblApprovedPrintLabel()
                                                            {
                                                                Id = Guid.NewGuid(),
                                                                CreatedDate = DateTime.Now,
                                                                CreatedMachine = Environment.MachineName,
                                                                QrCode = resultCheckBoxInfo.ApprovedBy,
                                                                IdLabel = resultCheckBoxInfo.IdLabel,
                                                                OC = resultCheckBoxInfo.OcNo,
                                                                BoxNo = resultCheckBoxInfo.BoxNo,
                                                                GrossWeight = resultCheckBoxInfo.GrossWeight,
                                                                NetWeight = resultCheckBoxInfo.NetWeight,
                                                                CalculatorPrs = resultCheckBoxInfo.CalculatedPairs,
                                                                Deviation = resultCheckBoxInfo.Deviation,
                                                                DeviationPairs = resultCheckBoxInfo.DeviationPairs,
                                                                ActualDeviationPairs = resultCheckBoxInfo.ActualDeviationPairs,
                                                                QRLabel = resultCheckBoxInfo.BarcodeString,
                                                                ApproveType = "Actual deviation",
                                                                Station = GlobalVariables.Station,
                                                                ScanDataId = resultCheckBoxInfo.Id,
                                                                Quantity = resultCheckBoxInfo.Quantity,
                                                                Reason = formDeviation.Reason
                                                            };
                                                            dbContext.TblApprovedPrintLabels.Add(itemInsert);
                                                            dbContext.SaveChanges();
                                                            #endregion
                                                        }
                                                        #endregion

                                                        //lấy lại ID của thùng lỗi này trong hệ thống để cho in lại tem rồi cập nhật thông tin người approved vào.
                                                        resultCheckBoxInfo = null;
                                                        resultCheckBoxInfo = new tblScanData();

                                                        //in tem
                                                        GlobalVariables.Printing((_scanData.GrossWeight / 1000).ToString("#,#0.00")
                                                            , !string.IsNullOrEmpty(GlobalVariables.IdLabel) ? GlobalVariables.IdLabel : $"{_scanData.OcNo}|{_scanData.BoxNo}", true
                                                             , _scanData.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                                                             , isHC: _scanData.IsHc
                                                             , _unitLabel);

                                                        #region Auto posting
                                                        //hàng từ production qua: decoration = 0 (OC). transfer từ kho 3--> 64
                                                        //if (_scanData.Decoration == 0 || (_scanData.Decoration == 1 && checkOc != null))
                                                        //{
                                                        //    GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 64, 41, GlobalVariables.GetDbConnectionDogeWh(), null);

                                                        //    if (GlobalVariables.ResultPosting.Message == "Successful")
                                                        //    {
                                                        //        GlobalVariables.ResultPosting.Message = $"Hàng cân lại OK (Transfer 64-->41): {GlobalVariables.ResultPosting.Message}";
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 63, 41, GlobalVariables.GetDbConnectionDogeWh(), null);
                                                        //        GlobalVariables.ResultPosting.Message = $"Hàng cân lại OK (Metal - Transfer 63-->41): {GlobalVariables.ResultPosting.Message}";
                                                        //    }
                                                        //}
                                                        #endregion
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //throw new Exception($"You haven’t entered the actual discrepancy for this box. Please scan the label again.");
                                                throw new Exception($"Bạn chưa nhập chênh lệch thực tế của thùng. Vui lòng quét lại tem và nhập lại.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //throw new Exception($"This carton has already been scanned and its weight recorded as OK; it is not allowed to be weighed again. {_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel}");
                                        throw new Exception($"{_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel} - {_scanData.IdLabel}. Thùng đã ghi nhận khối lượng OK, vui lòng không quét lại.");
                                    }
                                }
                                else//thung fail
                                {
                                    //bật đèn đỏ
                                    GlobalVariables.MyEvent.StatusLightPLC = false;

                                    _scanData.Pass = 0;
                                    _scanData.Status = 0;
                                    _scanData.CreatedDate = GlobalVariables.CreatedDate = DateTime.Now;//lấy thời gian để đồng bộ giữa in tem và log DB

                                    GlobalVariables.PrintApprove = false;
                                    if (_scanData.Decoration == 1)
                                    {
                                        GlobalVariables.RememberInfo.FailBoxPrinting += 1;
                                    }
                                    else
                                    {
                                        GlobalVariables.RememberInfo.FailBoxNoPrinting += 1;
                                    }



                                    if (statusLogData == 0)
                                    {
                                        GlobalVariables.Printing(_scanData.DeviationPairs.ToString()
                                                    , !string.IsNullOrEmpty(GlobalVariables.IdLabel) ? GlobalVariables.IdLabel : $"{_scanData.OcNo}|{_scanData.BoxNo}", false
                                                    , _scanData.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                                                    , isHC: _scanData.IsHc
                                                    , _unitLabel);

                                        #region Auto posting
                                        //hàng từ production qua: decoration = 0 (OC)  và dcoration = 1 (PRT). transfer từ kho 3--> 64
                                        //if (_scanData.Decoration == 0)
                                        //{
                                        //    GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 3, 64, GlobalVariables.GetDbConnectionDogeWh(), null);

                                        //    GlobalVariables.ResultPosting.Message = $"Hàng production Fail (Transfer 3-->41): {GlobalVariables.ResultPosting.Message}";
                                        //}
                                        ////hàng sơn-sau sơn
                                        //else if (_scanData.Decoration == 1 && checkOc != null)
                                        //{
                                        //    GlobalVariables.ResultPosting = AutoPostingHelper.AutoTransfer(_scanData.ProductNumber, _scanData.BarcodeString, 32, 64, GlobalVariables.GetDbConnectionDogeWh(), null);

                                        //    GlobalVariables.ResultPosting.Message = $"Hàng QC Fail (Transfer 32-->41): {GlobalVariables.ResultPosting.Message}";
                                        //}
                                        #endregion

                                        #region hien thi mau label
                                        GlobalVariables.InvokeIfRequired(this, () =>
                                        {
                                            _labResultMessage.Text = $"Số lượng chênh lệch: {_scanData.DeviationPairs} ({_unitLabel})";
                                            _labResult.Text = "FAILED";
                                            _labResult.BackColor = Color.Red;
                                            _labResult.ForeColor = Color.White;
                                        });
                                        #endregion

                                        errorFlag = true;
                                    }
                                    else if (statusLogData == 2)
                                    {
                                        //throw new Exception($"This carton has already been scanned and its weight recorded as OK, it is not allowed to be weighed again. {_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel}");
                                        throw new Exception($"{_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel} - {_scanData.IdLabel}. Thùng đã ghi nhận khối lượng OK, vui lòng không quét lại.");
                                    }
                                    else
                                    {
                                        //throw new Exception($"This carton has already been scanned and its weight recorded as error, it is not allowed to be weighed again. {_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel}");
                                        throw new Exception($"{_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel} - {_scanData.IdLabel}. Thùng đã ghi nhận khối lượng lỗi, vui lòng kiểm tra điều chỉnh lại đúng trước khi quét lại.");
                                    }
                                }
                                #endregion

                                ////hien thi cac thong so dem
                                //ShowUI();

                                #region Log data
                                //mỗi thùng chỉ cho log vào tối da là 2 dòng trong scanData, 1 dòng pass và fail (nếu có)
                                //tính lại tỷ lệ khối lượng số đôi lỗi/ StdGrossWeight của lần scan này để log
                                //_scanData.RatioFailWeight = Math.Round((Math.Abs(_scanData.DeviationPairs) * _scanData.AveWeight1Prs) / _scanData.StdGrossWeight, 3);

                                _scanData.Id = Guid.NewGuid();
                                _scanData.CreatedDate = DateTime.Now;
                                _scanData.Actived = 1;
                                dbContext.TblScanDatas.Add(_scanData);
                                dbContext.SaveChanges();
                                #endregion

                                string json = JsonConvert.SerializeObject(GlobalVariables.RememberInfo);
                                File.WriteAllText(@"./RememberInfo.json", json);
                            }
                            else
                            {
                                var itemInsert = new tblItemMissingInfo()
                                {
                                    Id = Guid.NewGuid(),
                                    CreatedDate = DateTime.Now,
                                    IsActive = true,
                                    ProductNumber = _scanData.ProductNumber,
                                    ProductName = _scanData.ProductName,
                                    OcNum = _scanData.OcNo,
                                    Note = $"Item '{_scanData.ProductNumber}' has no weight/pair.",
                                    QrCode = _scanData.BarcodeString
                                };
                                dbContext.TblItemMissingInfos.Add(itemInsert);
                                dbContext.SaveChanges();

                                throw new Exception($"Product number '{_scanData.ProductNumber}' không có khối lượng trên 1 prs/pcs. Vui lòng kiểm tra lại thông tin.");
                            }
                        }
                        else
                        {
                            GlobalVariables.ResultPosting.Message = string.Empty;

                            var itemInsert = new tblItemMissingInfo()
                            {
                                Id = Guid.NewGuid(),
                                CreatedDate = DateTime.Now,
                                IsActive = true,
                                ProductNumber = _scanData.ProductNumber,
                                ProductName = _scanData.ProductName,
                                OcNum = _scanData.OcNo,
                                Note = $"Product item '{_scanData.ProductNumber}' does not exist in the system.",
                                QrCode = _scanData.BarcodeString
                            };
                            dbContext.TblItemMissingInfos.Add(itemInsert);
                            dbContext.SaveChanges();

                            //throw new Exception($"Product number {_scanData.ProductNumber} does not exist in the system. Please check the information again.");
                            throw new Exception($"Product number {_scanData.ProductNumber} không tìm thấy trong dữ liệu chính. Vui lòng kiểm tra lại thông tin.");
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, "Lỗi scale form");

                    errorFlag = true;
                    //MessageBox.Show($"Quantity over the BX1 box limit ({res.BoxQtyBx1}).", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    GlobalVariables.InvokeIfRequired(this, () =>
                    {
                        _labResultMessage.Text = ex.Message;
                        _labResult.Text = "FAILED";
                        _labResult.BackColor = Color.Red;
                        _labResult.ForeColor = Color.White;
                    });
                }
                finally
                {
                    GlobalVariables.InvokeIfRequired(this, () =>
                    {
                        txtQrCode.Text = null;
                        txtQrCode.Clear();
                        txtQrCode.Focus();
                    });

                    //hien thi cac thong so dem
                    ShowUI(errorFlag);

                    _scanData = new tblScanData();
                    _resetUI = true;
                }
            }
        }

        #region Tasks
        private async Task TaskTimerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    GlobalVariables.InvokeIfRequired(this, () =>
                    {
                        _labStatus.Text = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} " +
                              $"| {GlobalVariables.UserLoginInfo.UserName} | {GlobalVariables.DbName}";
                        _labDateTime.Text = $"{Application.ProductVersion}";
                    });

                    await Task.Delay(300, token); // nhịp kiểm tra, đủ nhẹ nhàng
                }
                catch (OperationCanceledException)
                {
                    // token.Cancel() => thoát vòng lặp
                    break;
                }
                catch (Exception ex)
                {
                    // Không để task chết âm thầm
                    Log.Error(ex, "TaskTimerAsync loop error.");
                    await Task.Delay(500, token); // tạm nghỉ rồi thử lại
                }
            }
        }
        private async Task TaskReadModbusAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    #region Đọc các giá trị từ PLC
                    if (GlobalVariables.ModbusStatus)
                    {
                        if (_resetCounter)
                        {
                            if (GlobalVariables.MyDriver.ModbusRTUMaster.WriteSingleCoil(1, 2, true))
                            {
                                System.Threading.Thread.Sleep(10);
                                if (GlobalVariables.MyDriver.ModbusRTUMaster.WriteSingleCoil(1, 2, false))
                                {
                                    _resetCounter = false;
                                }
                            }
                        }

                        if (GlobalVariables.Station == StationEnum.IDC)
                        {
                            //thanh ghi D0 cua PLC Delta DPV14SS2 co dia chi la 4596
                            GlobalVariables.ModbusStatus = GlobalVariables.MyDriver.ModbusRTUMaster.ReadHoldingRegisters(1, 4596, 1, ref _readHoldingRegisterArr);

                            //GlobalVariables.RememberInfo.CountMetalScan = GlobalVariables.MyDriver.GetUshortAt(_readHoldingRegisterArr, 0);
                            ////update gia tri count vao sự kiện để trong frmScal  nó update lên giao diện
                            //GlobalVariables.MyEvent.CountValue = GlobalVariables.RememberInfo.CountMetalScan;

                            GlobalVariables.MyEvent.CountValue = GlobalVariables.MyDriver.GetUshortAt(_readHoldingRegisterArr, 0);
                        }
                    }
                    else
                    {
                        _countDisconnectPlc += 1;
                        if (_countDisconnectPlc >= 3)
                        {
                            GlobalVariables.MyDriver.ModbusRTUMaster.NgatKetNoi();

                            GlobalVariables.ModbusStatus = GlobalVariables.MyDriver.ModbusRTUMaster.KetNoi(GlobalVariables.ConfigJson.ComPort, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
                        }
                    }
                    #endregion

                    await Task.Delay(200, token); // nhịp kiểm tra, đủ nhẹ nhàng
                }
                catch (OperationCanceledException)
                {
                    // token.Cancel() => thoát vòng lặp
                    break;
                }
                catch (Exception ex)
                {
                    // Không để task chết âm thầm
                    Log.Error(ex, "TaskReadModbusAsync loop error.");
                    await Task.Delay(500, token); // tạm nghỉ rồi thử lại
                }
            }
        }
        private async Task TaskCheckResetUIAsync(CancellationToken token)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var lastResetAt = TimeSpan.Zero;
            var intervalSeconds = GlobalVariables.ConfigJson.ResetUiInterval; // ví dụ: 2 giây

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_resetUI)
                    {
                        var now = sw.Elapsed;
                        var canReset = (now - lastResetAt).TotalSeconds >= intervalSeconds;

                        if (canReset)
                        {
                            // Chuyển về UI thread để reset control
                            if (this.IsHandleCreated && !this.IsDisposed)
                            {
                                this.BeginInvoke(new Action(() =>
                                {
                                    try
                                    {
                                        ResetControl(); // đảm bảo hàm này không ném exception
                                    }
                                    catch (Exception ex)
                                    {
                                        // Log nếu cần
                                        Log.Error(ex, "ResetControl error.");
                                    }
                                }));
                            }

                            lastResetAt = now;
                            _resetUI = false; // tiêu thụ yêu cầu reset
                        }
                        else
                        {
                            // Vẫn trong thời gian chặn reset, bỏ qua lần này
                            //_resetUI = false; // tuỳ: nếu muốn giữ yêu cầu, đừng reset flag
                        }
                    }
                    else
                    {
                        lastResetAt = sw.Elapsed;
                    }

                    await Task.Delay(200, token); // nhịp kiểm tra, đủ nhẹ nhàng
                }
                catch (OperationCanceledException)
                {
                    // token.Cancel() => thoát vòng lặp
                    break;
                }
                catch (Exception ex)
                {
                    // Không để task chết âm thầm
                    Log.Error(ex, "TaskCheckResetUIAsync loop error.");
                    await Task.Delay(500, token); // tạm nghỉ rồi thử lại
                }
            }
        }
        #endregion

        private void ResetControl(bool resetQrCode = true)
        {
            GlobalVariables.InvokeIfRequired(this, () =>
            {
                if (resetQrCode)
                {
                    txtQrCode.Text = null;
                    txtQrCode.Clear();
                    txtQrCode.Focus();
                }

                _labLastResultMessage.Text = _labResultMessage.Text;
                _labLastResultMessage.ForeColor = _labResultMessage.ForeColor;

                #region Standard
                _labBoxId.Text = string.Empty;
                labOcNo.Text = string.Empty;
                labProductCode.Text = string.Empty;
                labProductName.Text = string.Empty;
                labQuantity.Text = "0";
                labColor.Text = string.Empty;
                labSize.Text = string.Empty;
                labAveWeight.Text = "0";
                //labLowerTolerance.Text = "0";
                //labUpperTolerance.Text = "0";
                labBoxWeight.Text = "0";
                labAccessoriesWeight.Text = "0";
                labGrossWeight.Text = "0";
                _labCheckMetal.Text = _scanData.MetalScan == 0 ? "NO" : "YES";
                _labPrinting.Text = _scanData.Decoration == 0 ? "NO" : "YES";

                _labQtyStandard.Text = $"Quantity (-)";
                _labUnitCalculatQty.Text = $"Calculated Qty (-)";
                _labUnitDeviation.Text = $"Deviation (-)";

                _labUnitStandard.Text = string.Empty;
                _labFGW.Text = $"Weight (g)/-";

                _labBoxType.Text = null;
                _labLableId.Text = string.Empty;
                #endregion

                #region Scaled
                labRealWeight.Text = "0";
                labNetWeight.Text = "0";

                labNetRealWeight.Text = "0";
                labDeviation.Text = "0 (g)";

                labCalculatedPairs.Text = "0";
                labDeviationPairs.Text = "0 (-)";

                _labResultMessage.Text = string.Empty;
                _labResult.Text = string.Empty;
                _labResult.BackColor = Color.Gray;

                labDeviationPairs.ForeColor = default;
                #endregion
            });
        }

        private void ShowUI(bool errorFlag = false)
        {
            #region hien thi cac thong so dem
            this.Invoke((MethodInvoker)delegate
            {
                #region Standard
                labRealWeight.Text = _scanData.GrossWeight.ToString();
                labNetWeight.Text = _scanData.StdNetWeight.ToString();
                _labBoxId.Text = _scanData.BoxNo;
                labOcNo.Text = _scanData.OcNo.Trim();
                labProductCode.Text = _scanData.ProductNumber;
                labProductName.Text = _scanData.ProductName;
                labQuantity.Text = _scanData.Quantity.ToString();
                labColor.Text = _color;
                labSize.Text = _sizeName;
                labAveWeight.Text = _scanData.AveWeight1Prs.ToString();
                //labLowerTolerance.Text = _scanData.LowerTolerance.ToString();
                //labUpperTolerance.Text = _scanData.UpperTolerance.ToString();
                labBoxWeight.Text = _scanData.BoxWeight.ToString();
                labAccessoriesWeight.Text = _scanData.PackageWeight.ToString();
                labGrossWeight.Text = _scanData.StdGrossWeight.ToString();
                _labCheckMetal.Text = _scanData.MetalScan == 0 ? "NO" : "YES";
                _labPrinting.Text = _scanData.Decoration == 0 ? "NO" : "YES";

                _labQtyStandard.Text = $"Quantity ({_unitLabel})";
                _labUnitCalculatQty.Text = $"Calculated Qty ({_unitLabel})";
                _labUnitDeviation.Text = $"Deviation ({_unitLabel})";

                _labUnitStandard.Text = _unitLabel;
                _labFGW.Text = $"Weight (g)/{_unitLabel}";

                _labBoxType.Text = _boxType.ToString();
                _labLableId.Text = _scanData.IdLabel;
                #endregion

                #region Scaled

                labNetRealWeight.Text = $"{_scanData.NetWeight}";
                labDeviation.Text = $"{_scanData.Deviation} (g)";
                labCalculatedPairs.Text = _scanData.CalculatedPairs.ToString();
                labDeviationPairs.Text = $"{_scanData.DeviationPairs.ToString()} ({_unitLabel})";

                labDeviationPairs.ForeColor = errorFlag == false ? Color.Green : Color.Red;
                _labResultMessage.ForeColor = errorFlag == false ? Color.Green : Color.Red;
                #endregion

                errorFlag = false;
            });
            #endregion
        }

        #region Events
        // Cho phép kéo form bằng panel
        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void labAveWeight_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void BtnUpdateVersion_Click(object sender, EventArgs e)
        {
            try
            {
                isUpdateClicked = true;
                string UUrl = GlobalVariables.ConfigJson.UpdatePath;
                SplashScreenManager.ShowForm(typeof(WaitForm1));
                System.Threading.Thread.Sleep(3000);
                AutoUpdater.Start(UUrl);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"{ex.Message}", "Error");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void _btnReprint_Click(object sender, EventArgs e)
        {
            frmConfirmPrint nf = new frmConfirmPrint();
            //nf.ConfirmPrintInfo.IdLabel = GlobalVariables.IdLabel;
            //nf.ConfirmPrintInfo.OcNo = GlobalVariables.OcNo;
            //nf.ConfirmPrintInfo.BoxNo = GlobalVariables.BoxNo;
            //nf.ConfirmPrintInfo.Weight = GlobalVariables.RealWeight;
            nf.ShowDialog();

            GlobalVariables.InvokeIfRequired(this, () =>
            {
                txtQrCode.Text = null;
                txtQrCode.Clear();
                txtQrCode.Focus();
            });
        }
        #endregion
    }
}