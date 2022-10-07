using Dapper;
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

            _timer.Enabled = true;
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Timer t = (Timer)sender;
            t.Enabled = false;

            barStaticItemStatus.Caption = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} | {GlobalVariables.UserLoginInfo.UserName} | ScaleStatus: {GlobalVariables.ScaleStatus}";

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
                        foreach (var item in res)
                        {
                            var para = new DynamicParameters();
                            para.Add("@productCode", item.ProductNumber);
                            using (var con=GlobalVariables.GetDbConnection())
                            {
                                var res1 = con.Query<tblWinlineProductsInfoModel>("sp_tblWinlineProductsInfoGet", para, commandType: CommandType.StoredProcedure).FirstOrDefault();
                                if (res1 != null)
                                {

                                }
                                else
                                {
                                    #region Create parametters
                                    para = null;
                                    para = new DynamicParameters();

                                    para.Add("@ProductNunmber", item.ProductNumber);
                                    para.Add("@Name", item.ProductName);
                                    para.Add("@ProductCategory", item.ProductCategory);
                                    para.Add("@Brand", item.Brand);
                                    para.Add("@Decoration", item.Decoration);//int
                                    para.Add("@MetalScan", item.MetalScan);//int
                                    para.Add("@MainProductNo", item.MainProductNo);
                                    para.Add("@Color", item.Color);
                                    para.Add("@Size", item.Size);
                                    para.Add("@Weight", item.Weight);//float
                                    para.Add("@PackingMethod", item.PackingMethod);
                                    para.Add("@LeftWeight", item.LeftWeight);//float
                                    para.Add("@RightWeight", item.RightWeight);//float
                                    para.Add("@BoxType", item.BoxType);
                                    para.Add("@ToolingNo", item.ToolingNo);
                                    para.Add("@PackingBoxType", item.PackingBoxType);
                                    para.Add("@CustomeUsePb", item.CustomeUsePb);
                                    para.Add("@PlacticBox", item.PlacticBox);//int
                                    para.Add("@PPbagWeight", item.PPbagWeight);//float
                                    para.Add("@Bx1Weight", item.Bx1Weight);//float
                                    para.Add("@Bx1AWeight", item.Bx1AWeight);//float
                                    para.Add("@Bx2Weight", item.Bx2Weight);//float
                                    para.Add("@Bx3Weight", item.Bx3Weight);//float
                                    para.Add("@Bx4Weight", item.Bx4Weight);//float
                                    para.Add("@Bx5Weight", item.Bx5Weight);//float
                                    para.Add("@Bx1_50_40_34", item.Bx1_50_40_34);//int
                                    para.Add("@Bx1A", item.Bx1A);//int
                                    para.Add("@Bx2_50_40_17", item.Bx2_50_40_17);//int
                                    para.Add("@Bx3_41_32_31", item.Bx3_41_32_31);//int
                                    para.Add("@Bx4_32_23_15", item.Bx4_32_23_15);//int
                                    para.Add("@Bx5_41_32_31", item.Bx5_41_32_31);//int
                                    para.Add("@PlaticBoxWeight", item.PlaticBoxWeight);//float
                                    para.Add("@PEUW", item.PEUW);//int
                                    para.Add("@BagWeight", item.BagWeight);//float
                                    para.Add("@PartitionWeight", item.PartitionWeight);//float
                                    para.Add("@QtyPerbag", item.QtyPerbag);//int
                                    para.Add("@QtyPerPartition", item.QtyPerPartition);//int
                                    para.Add("@QtyPerWrapSheet", item.QtyPerWrapSheet);//int
                                    para.Add("@WrapSheetWeight", item.WrapSheetWeight);//float
                                    para.Add("@Tolerance", 0);//float
                                    para.Add("@ToleranceBeforePrint", 0);//float
                                    para.Add("@ToleranceAfterPrint", 0);//float
                                    para.Add("@ProductFillter", string.Empty);
                                    #endregion

                                    var resInsert = con.Execute("sp_tblWinlineProductsInfoInsert", para, commandType: CommandType.StoredProcedure);
                                }
                            }
                        }
                        XtraMessageBox.Show("Get data from winline Ok.", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            string json = JsonConvert.SerializeObject(GlobalVariables.RememberInfo);
            File.WriteAllText(@"./RememberInfo.json", json);
        }
    }
    #endregion
}