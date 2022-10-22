using Dapper;
using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting.Export;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WeightChecking
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region Static Properties
        frmScale _frmScale;
        frmSettings _frmSettings;
        frmMasterData _frmMasterData;

        string _scale = null;
        string _settings = null;
        string _masterData = null;

        Timer _timer = new Timer() { Interval = 500 };

        byte[] _readHoldingRegisterArr = { 0, 0 };
        int _countDisconnectPlc = 0;
        #endregion

        public frmMain()
        {
            InitializeComponent();

            Load += frmMain_Load;

            FormClosing += MainForm_FormClosing;
        }

        #region Events
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            barStaticItemVersion.Caption = Application.ProductVersion;

            //register ribbon barButton click
            this.barButtonItemMain.ItemClick += BarButtonItemMain_ItemClick;
            this.barButtonItemSettings.ItemClick += BarButtonItemSettings_ItemClick;
            this.barButtonItemPrint.ItemClick += BarButtonItemPrint_ItemClick;
            this.barButtonItemResetCountMetal.ItemClick += BarButtonItemResetCountMetal_ItemClick;

            //chon chế độ chỉ hiển thị tab ribbon, ẩn chi tiết group
            //ribbonControl1.Minimized = true;//show tabs
            //this.RibbonVisibility = DevExpress.XtraBars.Ribbon.RibbonVisibility.Hidden;

            this.ribbonControl1.RibbonCaptionAlignment = DevExpress.XtraBars.Ribbon.RibbonCaptionAlignment.Center;

            //đăng ký sự kiện documentClose để xóa cờ báo document đang actived đi, để lần sau bấm chọn barButtonItem mở lại document đó.
            tabbedView1.DocumentClosing += (s, o) =>
            {
                try
                {
                    //TabbedView tabbed = s as TabbedView;
                    if (o.Document.Caption == "Weight Checking")
                    {
                        if (_scale != null)
                        {
                            _scale = null;
                        }
                    }
                    else if (o.Document.Caption == "Settings")
                    {
                        if (_settings != null)
                        {
                            _settings = null;
                        }
                    }
                    else if (o.Document.Caption == "Master Data")
                    {
                        if (_masterData != null)
                        {
                            _masterData = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi MainForm: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            };

            #region Check role
            if (GlobalVariables.UserLoginInfo.Role == RolesEnum.Operator)
            {
                #region actived form scale
                if (_scale == null)
                {
                    _scale = "Actived";

                    _frmScale = new frmScale();
                    tabbedView1.AddDocument(_frmScale);
                    tabbedView1.ActivateDocument(_frmScale);
                }
                else
                {
                    tabbedView1.ActivateDocument(_frmScale);
                }
                #endregion

                ribbonPageMasterData.Visible = false;
            }
            else//Admin
            {
                if (_masterData == null)
                {
                    _masterData = "Actived";

                    _frmMasterData = new frmMasterData();
                    tabbedView1.AddDocument(_frmMasterData);
                    tabbedView1.ActivateDocument(_frmMasterData);
                }
                else
                {
                    tabbedView1.ActivateDocument(_frmMasterData);
                }

                ribbonControl1.SelectedPage = ribbonPageMasterData;
            }

            #endregion

            #region Ket noi modbus RTU PLC metalScan counter
            //GlobalVariables.ModbusStatus = GlobalVariables.MyDriver.ModbusRTUMaster.KetNoi(GlobalVariables.ComPort, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);

            //Console.WriteLine($"PLC Status: {GlobalVariables.ModbusStatus}");
            //if (GlobalVariables.ModbusStatus == false)
            //{
            //    MessageBox.Show($"Không thể kết nối được bộ đếm dò kim loại.{Environment.NewLine}Tắt phần mềm, kiểm tra lại kết nối với PLC rồi mở lại phần mềm.",
            //                    "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            #endregion
            _timer.Enabled = true;
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Timer t = (Timer)sender;
            t.Enabled = false;

            barStaticItemStatus.Caption = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} " +
                $"| {GlobalVariables.UserLoginInfo.UserName} | ScaleStatus: {GlobalVariables.ScaleStatus}" +
                $" | CounterStatus: {GlobalVariables.ModbusStatus}";

            #region Đọc các giá trị từ PLC
            //if (GlobalVariables.ModbusStatus)
            //{
            //    //thanh ghi D0 cua PLC Delta DPV14SS2 co dia chi la 4596
            //    GlobalVariables.ModbusStatus = GlobalVariables.MyDriver.ModbusRTUMaster.ReadHoldingRegisters(1, 4596, 1, ref _readHoldingRegisterArr);

            //    GlobalVariables.RememberInfo.CountMetalScan = GlobalVariables.MyDriver.GetUshortAt(_readHoldingRegisterArr, 0);
            //    //update gia tri count vao sự kiện để trong frmScal  nó update lên giao diện
            //    GlobalVariables.MyEvent.CountValue = GlobalVariables.RememberInfo.CountMetalScan;
            //}
            //else
            //{
            //    _countDisconnectPlc += 1;
            //    if (_countDisconnectPlc==3)
            //    {
            //        GlobalVariables.MyDriver.ModbusRTUMaster.NgatKetNoi();

            //        GlobalVariables.ModbusStatus = GlobalVariables.MyDriver.ModbusRTUMaster.KetNoi(GlobalVariables.ComPort, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
            //    }
            //}
            #endregion

            t.Enabled = true;
        }

        private void BarButtonItemSettings_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (_settings == null)
                {
                    _settings = "Actived";

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Vui lòng chờ trong giây lát");
                    SplashScreenManager.Default.SetWaitFormDescription("Loading...");

                    _frmSettings = new frmSettings();
                    tabbedView1.AddDocument(_frmSettings);
                    tabbedView1.ActivateDocument(_frmSettings);

                    SplashScreenManager.CloseForm(false);
                }
                else
                {
                    tabbedView1.ActivateDocument(_frmSettings);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi MainForm: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BarButtonItemMain_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (_scale == null)
                {
                    _scale = "Actived";

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Vui lòng chờ trong giây lát");
                    SplashScreenManager.Default.SetWaitFormDescription("Loading...");

                    _frmScale = new frmScale();
                    tabbedView1.AddDocument(_frmScale);
                    tabbedView1.ActivateDocument(_frmScale);

                    SplashScreenManager.CloseForm(false);
                }
                else
                {
                    tabbedView1.ActivateDocument(_frmScale);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi MainForm: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BarButtonItemPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalVariables.PrintApprove)
            {
                GlobalVariables.Printing(25.68, "3000000000.2022");
            }
        }

        private void BarButtonItemResetCountMetal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        //Get data from winline update to local Databases.
        private void barButtonItemGetDataWL_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (var connection = GlobalVariables.GetDbConnectionWinline())
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Vui lòng chờ trong giây lát");
                    SplashScreenManager.Default.SetWaitFormDescription("Loading...");

                    var res = connection.Query<WinlineDataModel>("sp_IdcScanScaleGetCoreData").ToList();

                    if (res != null && res.Count > 0)
                    {
                        using (var con = GlobalVariables.GetDbConnection())
                        {
                            //truncate data
                            con.Execute("truncate table tblWinlineProductsInfo");

                            var _insertCount = con.Execute($"Insert into tblWinlineProductsInfo (CodeItemSize,ProductNumber," +
                            $"ProductName,ProductCategory,Brand,Decoration,MainProductNo,MainProductName,Color,SizeCode," +
                            $"SizeName,Weight,LeftWeight,RightWeight,BoxType,ToolingNo,PackingBoxType,CustomeUsePb) " +
                       $"values (@CodeItemSize,@ProductNumber,@ProductName,@ProductCategory,@Brand,@Decoration,@MainProductNo," +
                       $"@MainProductName,@Color,@SizeCode,@SizeName,@Weight,@LeftWeight,@RightWeight,@BoxType,@ToolingNo" +
                       $",@PackingBoxType,@CustomeUsePb)", res);

                            if (_insertCount == res.Count)
                            {
                                XtraMessageBox.Show($"Get data from winline Ok.  Rows inserted {_insertCount}/{ res.Count}.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                XtraMessageBox.Show($"Get data from winline fail. Rows inserted {_insertCount}/{ res.Count}.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        GlobalVariables.MyEvent.RefreshStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Get data from winline exception.");
                XtraMessageBox.Show("Lỗi MainForm Get data from winline: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
                GlobalVariables.MyEvent.RefreshStatus = true;
            }
        }

        private void barButtonItemRefreshData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (_masterData == null)
                {
                    _masterData = "Actived";

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Vui lòng chờ trong giây lát");
                    SplashScreenManager.Default.SetWaitFormDescription("Loading...");

                    _frmMasterData = new frmMasterData();
                    tabbedView1.AddDocument(_frmMasterData);
                    tabbedView1.ActivateDocument(_frmMasterData);

                    //SplashScreenManager.CloseForm(false);
                }
                else
                {
                    tabbedView1.ActivateDocument(_frmMasterData);
                    GlobalVariables.MyEvent.RefreshStatus = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi MainForm ReFresh masterData exception.");
                XtraMessageBox.Show("Lỗi MainForm: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void barButtonItemResetCountMetal_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalVariables.RememberInfo.GoodBoxPrinting = 0;
            GlobalVariables.RememberInfo.GoodBoxNoPrinting = 0;
            GlobalVariables.RememberInfo.FailBoxPrinting = 0;
            GlobalVariables.RememberInfo.FailBoxNoPrinting = 0;
            GlobalVariables.RememberInfo.MetalScan = 0;
            GlobalVariables.RememberInfo.NoMetalScan = 0;
            GlobalVariables.RememberInfo.CountMetalScan = 0;

            GlobalVariables.MyEvent.RefreshStatus = true;

            string json = JsonConvert.SerializeObject(GlobalVariables.RememberInfo);
            File.WriteAllText(@"./RememberInfo.json", json);
        }

        private void barButtonItemImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Excel File|*.xlsx";
                ofd.Title = "Import Excel File";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Vui lòng chờ trong giây lát");
                    SplashScreenManager.Default.SetWaitFormDescription("Loading...");
                    List<tblCoreDataCodeItemSizeModel> coreData = new List<tblCoreDataCodeItemSizeModel>();

                    #region Get data from template excel
                    using (Workbook wb = new Workbook())
                    {
                        wb.LoadDocument(ofd.FileName);

                        if (wb.Worksheets.Count > 0)
                        {
                            Worksheet ws = wb.Worksheets[0];

                            //Get ra số hàng và cột có data
                            //index bắt đầu từ 1
                            var _usedRange = ws.GetUsedRange();
                            int _rowUsed = _usedRange.RowCount;
                            int _colUsed = _usedRange.ColumnCount;

                            for (int i = 5; i < _rowUsed - 5; i++)
                            {
                                Row _row = ws.Rows[i];
                                if (!string.IsNullOrEmpty(_row[$"A{i}"].Value.TextValue))
                                {
                                    coreData.Add(new tblCoreDataCodeItemSizeModel()
                                    {
                                        CodeItemSize = _row[$"A{i}"].Value.TextValue,
                                        MainItemName = _row[$"B{i}"].Value.TextValue,
                                        MetalScan = (int)_row[$"c{i}"].Value.NumericValue,
                                        Date = _row[$"F{i}"].Value.DateTimeValue.ToString(),
                                        Size = _row[$"G{i}"].Value.TextValue,
                                        AveWeight1Prs = _row[$"M{i}"].Value.NumericValue,
                                        BoxQtyBx1 = (int)_row[$"O{i}"].Value.NumericValue,
                                        BoxQtyBx2 = (int)_row[$"P{i}"].Value.NumericValue,
                                        BoxQtyBx3 = (int)_row[$"Q{i}"].Value.NumericValue,
                                        BoxQtyBx4 = (int)_row[$"R{i}"].Value.NumericValue,
                                        BoxWeightBx1 = _row[$"S{i}"].Value.NumericValue,
                                        BoxWeightBx2 = _row[$"T{i}"].Value.NumericValue,
                                        BoxWeightBx3 = _row[$"U{i}"].Value.NumericValue,
                                        BoxWeightBx4 = _row[$"V{i}"].Value.NumericValue,
                                        PartitionQty = (int)_row[$"W{i}"].Value.NumericValue,
                                        PlasicBagQty = (int)_row[$"X{i}"].Value.NumericValue,
                                        WrapSheetQty = (int)_row[$"Y{i}"].Value.NumericValue,
                                        PlasicBagWeight = _row[$"AB{i}"].Value.NumericValue,
                                        WrapSheetWeight = _row[$"AC{i}"].Value.NumericValue
                                    });
                                }
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show($"File temple is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion

                    #region Log DB
                    using (var con = GlobalVariables.GetDbConnection())
                    {
                        foreach (var item in coreData)
                        {
                            var para = new DynamicParameters();

                            var sfdsf = con.Query<tblCoreDataCodeItemSizeModel>($"select * from tblCoreDataCodeItemSize where CodeItemSize = '{item.CodeItemSize}'").FirstOrDefault();
                            if (sfdsf == null)
                            {
                                para.Add("@CodeItemSize", item.CodeItemSize);
                                para.Add("@MainItemName", item.MainItemName);
                                para.Add("@MetalScan", item.MetalScan);
                                para.Add("@Date", item.Date);
                                para.Add("@Size", item.Size);
                                para.Add("@AveWeight1Prs", item.AveWeight1Prs);
                                para.Add("@BoxQtyBx1", item.BoxQtyBx1);
                                para.Add("@BoxQtyBx2", item.BoxQtyBx2);
                                para.Add("@BoxQtyBx3", item.BoxQtyBx3);
                                para.Add("@BoxQtyBx4", item.BoxQtyBx4);
                                para.Add("@BoxWeightBx1", item.BoxWeightBx1);
                                para.Add("@BoxWeightBx2", item.BoxWeightBx2);
                                para.Add("@BoxWeightBx3", item.BoxWeightBx3);
                                para.Add("@BoxWeightBx4", item.BoxWeightBx4);
                                para.Add("@PartitionQty", item.PartitionQty);
                                para.Add("@PlasicBagQty", item.PlasicBagQty);
                                para.Add("@WrapSheetQty", item.WrapSheetQty);
                                para.Add("@PartitionWeight", item.PartitionWeight);
                                para.Add("@PlasicBagWeight", item.PlasicBagWeight);
                                para.Add("@WrapSheetWeight", item.WrapSheetWeight);
                                para.Add("@PlasicBoxWeight", item.PlasicBoxWeight);
                                para.Add("@Tolerance", item.Tolerance);
                                para.Add("@ToleranceAfterPrint", item.ToleranceAfterPrint);
                                con.Execute("sp_tblCoreDataCodeitemSizeInsert", para, commandType: CommandType.StoredProcedure);
                            }
                            else
                            {
                                para.Add("@CodeItemSize", item.CodeItemSize);
                                para.Add("@MainItemName", item.MainItemName);
                                para.Add("@MetalScan", item.MetalScan);
                                para.Add("@Date", item.Date);
                                para.Add("@Size", item.Size);
                                para.Add("@AveWeight1Prs", item.AveWeight1Prs);
                                para.Add("@BoxQtyBx1", item.BoxQtyBx1);
                                para.Add("@BoxQtyBx2", item.BoxQtyBx2);
                                para.Add("@BoxQtyBx3", item.BoxQtyBx3);
                                para.Add("@BoxQtyBx4", item.BoxQtyBx4);
                                para.Add("@BoxWeightBx1", item.BoxWeightBx1);
                                para.Add("@BoxWeightBx2", item.BoxWeightBx2);
                                para.Add("@BoxWeightBx3", item.BoxWeightBx3);
                                para.Add("@BoxWeightBx4", item.BoxWeightBx4);
                                para.Add("@PartitionQty", item.PartitionQty);
                                para.Add("@PlasicBagQty", item.PlasicBagQty);
                                para.Add("@WrapSheetQty", item.WrapSheetQty);
                                para.Add("@PartitionWeight", item.PartitionWeight);
                                para.Add("@PlasicBagWeight", item.PlasicBagWeight);
                                para.Add("@WrapSheetWeight", item.WrapSheetWeight);
                                para.Add("@PlasicBoxWeight", item.PlasicBoxWeight);
                                para.Add("@Tolerance", item.Tolerance);
                                para.Add("@ToleranceAfterPrint", item.ToleranceAfterPrint);
                                con.Execute("sp_tblCoreDataCodeitemSizeUpdate", para, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Update fail.{Environment.NewLine}{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
    }
    #endregion
}