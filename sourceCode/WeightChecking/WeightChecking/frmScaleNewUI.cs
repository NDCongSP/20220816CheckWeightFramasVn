using AutoUpdaterDotNET;
using Dapper;
using DevExpress.DirectX.Common.DirectWrite;
using DevExpress.DirectX.Common.DXGI;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraSpreadsheet.Model;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
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
        private Button btnSettings;
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

        private CancellationTokenSource _timer;
        private Task _timerTask;

        private readonly AsyncAutoResetEvent _triggerInspectionWeight = new AsyncAutoResetEvent();
        private CancellationTokenSource _inspectionWeightCts;

        private string _qrScan = string.Empty;
        private string _version = string.Empty;
        private MesoInfoModel _mesoinfo = new MesoInfoModel();
        private bool _errorFlag = false;

        private List<BoxInformationModel> _boxInformations = new List<BoxInformationModel>();

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
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.FromArgb(167, 201, 87);
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.Black;
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
            btnMaximize.MouseEnter += (s, e) => btnMaximize.BackColor = Color.FromArgb(167, 201, 87);
            btnMaximize.MouseLeave += (s, e) => btnMaximize.BackColor = Color.Black;
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
                                                                       // Tùy chọn: hiệu ứng hover (đổi nền cho dễ nhìn)
            btnMinimize.MouseEnter += (s, e) => btnMinimize.BackColor = Color.FromArgb(167, 201, 87);
            btnMinimize.MouseLeave += (s, e) => btnMinimize.BackColor = Color.Black;
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
            btnUpdateVersion.MouseEnter += (s, e) => btnUpdateVersion.BackColor = Color.FromArgb(167, 201, 87);
            btnUpdateVersion.MouseLeave += (s, e) => btnUpdateVersion.BackColor = Color.Black;

            // Sự kiện Click (giữ nguyên như bạn đã có)
            btnUpdateVersion.Click += BtnUpdateVersion_Click; ; // hoặc sự kiện update version thực tế của bạn
            titleBar.Controls.Add(btnUpdateVersion);

            // Nút Settings
            btnSettings = new Button();
            btnSettings.Text = "";                      // Không cần chữ, chỉ hiển thị icon
            btnSettings.ForeColor = Color.White;
            btnSettings.BackColor = Color.Black;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.Size = new Size(40, 40);
            btnSettings.Location = new Point(this.Width - 200, 0);
            btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSettings.Cursor = Cursors.Hand;

            // 1) Gán icon từ Resources (đặt tên hình là "updateVersion" như trong Resource)
            btnSettings.Image = Properties.Resources.icons8_installing_updates_30_white;  // PNG từ Resources
            btnSettings.ImageAlign = ContentAlignment.MiddleCenter;  // căn giữa
            btnSettings.Padding = new Padding(0);                    // tránh lệch
            btnSettings.TextImageRelation = TextImageRelation.Overlay; // chỉ icon

            // Tùy chọn: scale icon nếu quá lớn/nhỏ (WinForms Button không có ImageLayout)
            // => bạn có thể dùng phiên bản icon 24x24 hoặc 32x32 trong file PNG để vừa với nút 40x40.

            // 2) Tooltip khi hover
            tip = new ToolTip();
            tip.AutoPopDelay = 5000;     // hiển thị tối đa 5 giây
            tip.InitialDelay = 300;      // trễ 300ms
            tip.ReshowDelay = 100;       // xuất hiện lại nhanh
            tip.ShowAlways = true;       // luôn hiển thị tooltip
            tip.SetToolTip(btnSettings, "Click to update the config parametters system");  // nội dung tooltip

            // Tùy chọn: hiệu ứng hover (đổi nền cho dễ nhìn)
            btnSettings.MouseEnter += (s, e) => btnSettings.BackColor = Color.FromArgb(167, 201, 87);
            btnSettings.MouseLeave += (s, e) => btnSettings.BackColor = Color.Black;

            // Sự kiện Click (giữ nguyên như bạn đã có)
            btnSettings.Click += BtnSettings_Click; ; // hoặc sự kiện update version thực tế của bạn
            titleBar.Controls.Add(btnSettings);


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

                _timer?.Dispose();
                _timer = null;
                _timerTask = null;
            }
        }



        private void FrmScaleNewUI_Load(object sender, EventArgs e)
        {
            using var dbContext = new ApplicationDbContext(GlobalVariables.ConnectionString);
            _mesoinfo = dbContext.Database.SqlQuery<MesoInfoModel>($"sp_GetMesoInfo").AsEnumerable().FirstOrDefault();
            _boxInformations = dbContext.Database.SqlQuery<BoxInformationModel>($"sp_GetBoxInformation").AsEnumerable().ToList();

            var location = _mesoinfo.MESOCOMP == "VNT1" ? "fVN" :
                          _mesoinfo.MESOCOMP == "FKV" ? "fKV" :
                          _mesoinfo.MESOCOMP == "FTT1" ? "fFT" :
                          _mesoinfo.MESOCOMP == "05FI" ? "fIN" :
                          _mesoinfo.MESOCOMP == "01FG" ? "fGE" : "Unknown";

            if (Enum.TryParse<EnumLocation>(location, ignoreCase: true, out var loc))
            {
                titleText.Text = $"{loc} - SSFG Station";
            }
            _version = System.Windows.Forms.Application.ProductVersion.Split('+')[0];

            _labLastResultMessage.Text = string.Empty;

            ResetControl();

            _labLastResultMessage.Text = null;

            this.txtQrCode.Focus();
            txtQrCode.KeyDown += TxtQrCode_KeyDown;
            _btnReprint.Click += _btnReprint_Click;

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
                        var w = Math.Round(o.Value * GlobalVariables.ConfigJson.UnitScale, 2);
                        GlobalVariables.RealWeight = w;

                        GlobalVariables.InvokeIfRequired(this, () =>
                        {
                            labScaleValue.Text = w.ToString();
                        });
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

            _inspectionWeightCts = new CancellationTokenSource();
            _ = TaskInspectionWeightAsync(_inspectionWeightCts.Token);

            ////unit gram
            //labScaleValue.Text = "7450";
            //_txtScale.KeyDown += (s, o) =>
            //{

            //    if (o.KeyCode == Keys.Enter)
            //    {
            //        GlobalVariables.InvokeIfRequired(this, () =>
            //        {
            //            labScaleValue.Text = _txtScale.Text.Trim();
            //        });
            //    }
            //};


            //string input = "SU S     2.2777 kg";

            //// Regex bắt số kg
            //string pattern = @"([0-9]+(?:\.[0-9]+)?)\s*kg";

            //Match match = Regex.Match(input, pattern);
            //if (match.Success)
            //{
            //    double weight = double.Parse(match.Groups[1].Value);
            //    Console.WriteLine("Weight: " + weight);
            //}
            //else
            //{
            //    Console.WriteLine("Không đọc được khối lượng!");
            //}
        }

        private void _txtScale_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void TxtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            //AB95281,1111011303-ADSN-D167,300,3,1/6,BX2,L
            if (e.KeyCode == Keys.Enter)
            {
                var errorFlag = false;
                TextBox _sen = sender as TextBox;

                _qrScan = _sen.Text.Trim();
                Console.WriteLine(_qrScan);

                _triggerInspectionWeight.Set();
            }
        }

        private async Task TaskTimerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    GlobalVariables.InvokeIfRequired(this, () =>
                    {
                        _labStatus.Text = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} " +
                              $"| {GlobalVariables.UserLoginInfo.UserName} | {GlobalVariables.DbName} | Scale: {GlobalVariables.ScaleStatus}";
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

        private async Task TaskInspectionWeightAsync(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    // chờ sự kiện → không tốn CPU, không Sleep
                    await _triggerInspectionWeight.WaitAsync(token);

                    // === xử lý 1 lần duy nhất mỗi lần được kích ===
                    Debug.WriteLine("TaskInspectionWeightAsync triggered.");

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



                        #region xử lý barcode lấy ra các giá trị theo code
                        _scanData.BarcodeString = _qrScan;

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

                        //AB95281,1111011303-ADSN-D167,300,3,1/6,BX2,L
                        if (_scanData.BarcodeString.Contains(","))
                        {
                            var s1 = _qrScan.Split(',');
                            _scanData.Unit = _plr = s1[6];//get Thung này đóng theo đôi (P) hay L/R
                            _scanData.OcNo = s1[0].Trim();
                            _scanData.ProductNumber = s1[1];

                            _scanData.Quantity = Convert.ToInt32(s1[2]) * 2;
                            _scanData.LinePosNo = s1[3];
                            _scanData.BoxNo = s1[4];
                            _scanData.BoxType = s1[5];
                            _scanData.BoxWeight = _boxWeight = _boxInformations.FirstOrDefault(x => x.BoxType == _scanData.BoxType)?.BoxWeight ?? 0;
                            _scanData.Location = EnumLocation.fGE;

                            if (Enum.TryParse<BoxTypeEnum>(_scanData.BoxType, out var boxTypeValue))
                            {
                                _boxType = boxTypeValue;
                            }
                            else
                            {
                                throw new Exception("The Box type was not corecct format.");
                            }

                            if (resultCheckOc == null)
                            {
                                throw new Exception("The QR code is not in the correct format. Please scan the correct FG label.");
                            }
                        }
                        else
                        {
                            throw new Exception("The QR code is not in the correct format. Please scan the correct FG label.");
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
                            var checkExists = dbContext.TblScanDatas
                                .Where(x => x.Actived == 1 &&
                                        x.OcNo == _scanData.OcNo &&
                                        x.BoxNo == _scanData.BoxNo &&
                                        x.Unit == _scanData.Unit
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
                                            (item.Pass == 1 &&
                                                (item.Status == 2 || GlobalVariables.Station == StationEnum.IDC)
                                            )
                                            //|| (item.Pass == 0 && item.ActualDeviationPairs == 0 && item.ApprovedBy != Guid.Empty)
                                            || (item.Pass == 0 &&
                                                item.Status == 2 &&
                                                item.ActualDeviationPairs == 0
                                            )
                                        )
                                    {
                                        isPass = true;
                                    }
                                    else if (
                                                (item.Pass == 0 && item.Status == 0)// && item.ActualDeviationPairs != 0 && item.ApprovedBy != Guid.Empty)
                                                || (item.Pass == 0 && item.Status == 2 && item.ActualDeviationPairs != 0)
                                            )
                                    {
                                        isFail = true;
                                        //tính tỷ lệ khối lượng số đôi lỗi/ StdGrossWeight
                                        ratioFailWeight = Math.Round((Math.Abs(item.DeviationPairs) * item.AveWeight1Prs) / item.StdGrossWeight, 2);

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

                            //Get data WL
                            var res = dbContext.Database.SqlQuery<ProductInfoFgeModel>("sp_ssfgGetCoreDataByFgCode @fgItemCode= {0}", _scanData.ProductNumber)
                                .FirstOrDefault();

                            if (res != null)
                            {
                                //có thể dựa vào category để biết được thùng đó đóng theo đôi hay L/R
                                //_scanData.Category = 1 là HC; 0 là Non-HC
                                _scanData.IsHc = res.ProductCategory!=1?false:true;

                                _unitLabel = _scanData.Unit == "P" ? "prs" : "pcs";
                                _color = res.Color;
                                _sizeName = res.SizeName;

                                _scanData.ProductName = res.ProductName;
                                _scanData.Decoration = (int)res.Decoration;
                                _scanData.MetalScan = (int)res.MetalScan;
                                _scanData.Brand = res.Brand;
                                _scanData.AveWeight1Prs = (double)res.AveWeight1Prs;
                                _scanData.ProductCategory = res.ProductCategory;

                                if (_scanData.AveWeight1Prs != 0)
                                {
                                    #region Fill data from coreData to scanData, tính toán ra NetWeight và GrossWeight
                                    //Xét điều kiện để lấy boxWeight. Nếu là hàng đi sơn thì dùng thùng nhựa
                                    if ((_scanData.Decoration == 0 || (_scanData.Decoration == 1 && checkOc != null)) && ocFirstChar != "PR")
                                    {
                                        _scanData.Status = 2;//báo trạng thái hàng ko đi sơn, hoặc hàng sơn đã được sơn rồi

                                        //lấy tolerance theo thùng giấy
                                        lowerToleranceOfBox = (double)GlobalVariables.ConfigJson.LowerToleranceOfCartonBox;
                                        upperToleranceOfBox = (double)GlobalVariables.ConfigJson.UpperToleranceOfCartonBox;

                                        #region get box weight
                                        //_scanData.BoxWeight = (double)res.BoxWeight;

                                        //if (Enum.TryParse<BoxTypeEnum>(res.BoxType, out var boxTypeValue))
                                        //{
                                        //    _boxType = boxTypeValue;
                                        //}
                                        //else
                                        //{
                                        //    throw new Exception("The Box type was not corecct format.");
                                        //}
                                        #endregion
                                    }

                                    if (_scanData.MetalScan == 0)
                                    {
                                        _approveUpdateActMetalScan = false;
                                    }
                                    else
                                    {
                                        GlobalVariables.RememberInfo.MetalScan += 1;

                                        _approveUpdateActMetalScan = true;
                                    }

                                    _scanData.StdNetWeight = Math.Round(_scanData.Quantity * _scanData.AveWeight1Prs, 2);
                                    //_scanData.Tolerance = Math.Round(_scanData.StdNetWeight * (res.Tolerance / 100), 2);
                                    _scanData.LowerTolerance = -Math.Round(_scanData.StdNetWeight * (lowerToleranceOfBox / 100), 2);
                                    _scanData.UpperTolerance = Math.Round(_scanData.StdNetWeight * (upperToleranceOfBox / 100), 2);

                                    //luu ý các Quantity partition-Plasic-WrapSheet trên DB nó là tính số Prs
                                    //sau khi đọc về phải lấy QtyPrs quét trên label / Quantity partition-Plasic-WrapSheet ==> qty * weight ==> Weight package weight
                                    double partitionWeight = 0;

                                    #region Tính số tấm lót partition
                                    double p = 0;

                                    double partitionQty = 0;
                                    switch (_boxType)
                                    {
                                        case BoxTypeEnum.BX4:
                                            partitionQty = (double)res.PartitionBX4Qty;
                                            break;
                                        case BoxTypeEnum.BX3:
                                            partitionQty = (double)res.PartitionBX3Qty;
                                            break;
                                        case BoxTypeEnum.BX2:
                                            partitionQty = (double)res.PartitionBX2Qty;
                                            break;
                                        case BoxTypeEnum.BX1A:
                                            partitionQty = (double)res.PartitionBX1AQty;
                                            break;
                                        case BoxTypeEnum.BX1:
                                            partitionQty = (double)res.PartitionBX1Qty;
                                            break;
                                    }

                                    //với hàng FG outsole thì tính ra được số lượng partition thì trừ đi 1 để ra số đúng
                                    if (res.ProductCategory != 1)//OS - 1:HC
                                    {
                                        p = partitionQty != 0 ? ((double)_scanData.Quantity / (double)partitionQty) : 0;
                                        p = p - 1;
                                        if (p < 0) p = 0;

                                        partitionWeight = Math.Floor(p) * (double)res.PartitionWeight;
                                    }
                                    //với hàng HC thì lấy số lượng partition = DB.
                                    else if (res.ProductCategory == 1)
                                    {
                                        p = partitionQty != 0 ? ((double)res.QtyOfBox / (double)partitionQty) : 0;

                                        partitionWeight = Math.Round(Math.Ceiling(p) * (double)res.PartitionWeight, 2);
                                    }
                                    #endregion

                                    ////partitionWeight = res.PartitionQty != 0 ? (_scanData.Quantity / res.PartitionQty) * res.PartitionWeight : 0;
                                    //var plasicBag1Weight = res.PlasticBag1Qty != 0 ? Math.Ceiling(((double)_scanData.Quantity / (double)res.PlasticBag1Qty)) * res.PlasticBag1Weight : 0;
                                    //var plasicBag2Weight = res.PlasticBag2Qty != 0 ? Math.Ceiling(((double)_scanData.Quantity / (double)res.PlasticBag2Qty)) * res.PlasticBag2Weight : 0;
                                    //var wrapSheetWeight = res.WrapSheetQty != 0 ? Math.Ceiling(((double)_scanData.Quantity / (double)res.WrapSheetQty)) * res.WrapSheetWeight : 0;
                                    //var foamSheetWeight = res.FoamSheetQty != 0 ? Math.Ceiling(((double)_scanData.Quantity / (double)res.FoamSheetQty)) * res.FoamSheetWeight : 0;

                                    //_scanData.PackageWeight = Math.Round((double)partitionWeight + (double)plasicBag1Weight + (double)plasicBag2Weight + (double)wrapSheetWeight + (double)foamSheetWeight, 2);
                                    _scanData.PackageWeight = (double)partitionWeight;

                                    _scanData.StdGrossWeight = Math.Round(_scanData.StdNetWeight + _scanData.PackageWeight + _scanData.BoxWeight, 2);

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
                                    _scanData.NetWeight = Math.Round(_scanData.GrossWeight - _scanData.BoxWeight - _scanData.PackageWeight, 2);
                                    _scanData.Deviation = Math.Round(_scanData.NetWeight - _scanData.StdNetWeight, 2);

                                    #region tính toán số pairs chênh lệch và hiển thị label
                                    //var nwPlus = _scanData.StdNetWeight + _scanData.Tolerance;
                                    //var nwSub = _scanData.StdNetWeight - _scanData.Tolerance;
                                    var nwPlus = _scanData.StdNetWeight + _scanData.UpperTolerance;
                                    var nwSub = _scanData.StdNetWeight + _scanData.LowerTolerance;

                                    var wPcs = _scanData.ProductCategory == 1 ? _scanData.AveWeight1Prs : _scanData.AveWeight1Prs / 2;

                                    if (((_scanData.NetWeight > nwPlus) && (_scanData.NetWeight - nwPlus < wPcs))
                                    || ((_scanData.NetWeight < nwSub) && (nwSub - _scanData.NetWeight < wPcs))
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
                                    _scanData.RatioFailWeight = Math.Round((Math.Abs(_scanData.DeviationPairs) * _scanData.AveWeight1Prs) / _scanData.StdGrossWeight, 2);

                                    //hien thi cac thong so dem
                                    ShowUI();

                                    //thung hang Pass
                                    if (_scanData.DeviationPairs == 0)
                                    {
                                        _errorFlag = false;
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

                                        //kiểm tra xem data đã có trên hệ thống hay chưa
                                        //Check fail recorded?
                                        if (statusLogData == 0)
                                        {
                                            GlobalVariables.Printing((_scanData.GrossWeight / 1000).ToString("#,#0.00")
                                             , !string.IsNullOrEmpty(GlobalVariables.IdLabel) ? GlobalVariables.IdLabel : $"{_scanData.OcNo}|{_scanData.BoxNo}", true
                                              , _scanData.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                                              ,isHC: _scanData.IsHc
                                              , _unitLabel);
                                        }
                                        //với thùng Pass mà trước đó đã cân và báo fail thì popup form nhập deviation
                                        else if (statusLogData == 1)
                                        {
                                            resultCheckBoxInfo = await dbContext.TblScanDatas
                                          .FirstOrDefaultAsync(x => x.BarcodeString == _scanData.BarcodeString &&
                                          x.Actived == 1 && x.Pass == 0);

                                            using (var formDeviation = new frmTypingDeviation())
                                            {
                                                var resultForm = formDeviation.ShowDialog();

                                                if (resultForm == DialogResult.OK)
                                                {
                                                    if (resultCheckBoxInfo != null)
                                                    {
                                                        var dialogResult = MessageBox.Show($"Can you confirm the update of the actual quantity discrepancy for the box based on the information:" +
                                                                                             $"{Environment.NewLine}{_scanData.IdLabel}|{_scanData.OcNo}|{_scanData.BoxNo}.{Environment.NewLine}" +
                                                                                             $"The actual quantity discrepancy is: {formDeviation.ActualDeviation}?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                                        if (dialogResult == DialogResult.Yes)
                                                        {
                                                            //gán giá trị trả về từ form nhập deviation vào model get data
                                                            resultCheckBoxInfo.ActualDeviationPairs = formDeviation.ActualDeviation;
                                                            resultCheckBoxInfo.ApprovedBy = formDeviation.QrConfirm;

                                                            resultCheckBoxInfo.Status = 2;
                                                            dbContext.TblScanDatas.AddOrUpdate(resultCheckBoxInfo);
                                                            dbContext.SaveChanges();

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
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    throw new Exception($"The actual quantity discrepancy for the box has not been entered. Please rescan the label and input it again.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception($"{_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel} - {_scanData.IdLabel}.The box’s weight has been successfully recorded. Please do not rescan.");
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

                                            #region hien thi mau label
                                            GlobalVariables.InvokeIfRequired(this, () =>
                                            {
                                                _labResultMessage.Text = $"Discrepancy quantity: {_scanData.DeviationPairs} ({_unitLabel})";
                                                _labResult.Text = "FAILED";
                                                _labResult.BackColor = Color.Red;
                                                _labResult.ForeColor = Color.White;
                                            });
                                            #endregion

                                            _errorFlag = true;
                                        }
                                        else if (statusLogData == 2)
                                        {
                                            //throw new Exception($"This carton has already been scanned and its weight recorded as OK, it is not allowed to be weighed again. {_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel}");
                                            throw new Exception($"{_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel} - {_scanData.IdLabel}. The box’s weight has been successfully recorded. Please do not rescan.");
                                        }
                                        else
                                        {
                                            //throw new Exception($"This carton has already been scanned and its weight recorded as error, it is not allowed to be weighed again. {_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel}");
                                            throw new Exception($"{_scanData.OcNo} - {_scanData.BoxNo} - {_unitLabel} - {_scanData.IdLabel}. The box's weight is incorrect. Please verify and correct it before rescanning.");
                                        }
                                    }
                                    #endregion

                                    ////hien thi cac thong so dem
                                    //ShowUI();

                                    #region Log data
                                    //mỗi thùng chỉ cho log vào tối da là 2 dòng trong scanData, 1 dòng pass và fail (nếu có)
                                    //tính lại tỷ lệ khối lượng số đôi lỗi/ StdGrossWeight của lần scan này để log
                                    //_scanData.RatioFailWeight = Math.Round((Math.Abs(_scanData.DeviationPairs) * _scanData.AveWeight1Prs) / _scanData.StdGrossWeight, 2);

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

                                    throw new Exception($"Product number '{_scanData.ProductNumber}' No weight is available per pair/piece. Please verify the information.");
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

                                throw new Exception($"Product number {_scanData.ProductNumber} does not exist in the system. Please check the information again.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, "Lỗi scale form");

                        _errorFlag = true;
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
                        ShowUI(_errorFlag);

                        _scanData = new tblScanData();
                        _resetUI = true;
                    }

                    // xong việc → quay lại vòng chờ (không cần Delay hay poll)
                }
            }
            catch (OperationCanceledException) { /* thoát êm */ }
        }

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

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                using (var nf = new frmSettings())
                {
                    nf.StartPosition = FormStartPosition.CenterParent;
                    nf.ShowDialog();
                }
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