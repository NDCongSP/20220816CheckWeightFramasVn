﻿using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        string _scale = null;
        string _seetings = null;

        Timer _timer = new Timer() { Interval = 500 };
        #endregion

        public frmMain()
        {
            InitializeComponent();

            Load += frmMain_Load;
        }

        #region Events
        private void frmMain_Load(object sender, EventArgs e)
        {
            barStaticItemVersion.Caption = Application.ProductVersion;

            this.barButtonItemMain.ItemClick += BarButtonItemMain_ItemClick;
            this.barButtonItemSettings.ItemClick += BarButtonItemSettings_ItemClick;

            ribbonControl1.Minimized = true;//show tabs
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
                    else if (o.Document.Caption == "Settings") //Đặt nguyên liệu
                    {
                        if (_frmSettings != null)
                        {
                            _frmSettings = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi MainForm: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            };

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

            _timer.Enabled = true;
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Timer t = (Timer)sender;
            t.Enabled = false;

            barStaticItemStatus.Caption = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            t.Enabled = true;
        }

        private void BarButtonItemSettings_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (_seetings == null)
                {
                    _seetings = "Actived";

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
        #endregion

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }
    }
}