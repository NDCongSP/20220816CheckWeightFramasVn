
namespace WeightChecking
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItemMain = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSettings = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItemVersion = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemStatus = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItemPrint = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupHome = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barButtonItemResetHistory = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.barButtonItemMain,
            this.barButtonItemSettings,
            this.barStaticItemVersion,
            this.barStaticItemStatus,
            this.barButtonItemPrint,
            this.barButtonItemResetHistory});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 8;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageHome});
            this.ribbonControl1.Size = new System.Drawing.Size(1918, 177);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            // 
            // barButtonItemMain
            // 
            this.barButtonItemMain.Caption = "Main";
            this.barButtonItemMain.Id = 1;
            this.barButtonItemMain.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItemMain.ImageOptions.SvgImage")));
            this.barButtonItemMain.Name = "barButtonItemMain";
            // 
            // barButtonItemSettings
            // 
            this.barButtonItemSettings.Caption = "Settings";
            this.barButtonItemSettings.Id = 2;
            this.barButtonItemSettings.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItemSettings.ImageOptions.SvgImage")));
            this.barButtonItemSettings.Name = "barButtonItemSettings";
            // 
            // barStaticItemVersion
            // 
            this.barStaticItemVersion.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItemVersion.Caption = "Version";
            this.barStaticItemVersion.Id = 3;
            this.barStaticItemVersion.Name = "barStaticItemVersion";
            // 
            // barStaticItemStatus
            // 
            this.barStaticItemStatus.Caption = "Status";
            this.barStaticItemStatus.Id = 4;
            this.barStaticItemStatus.Name = "barStaticItemStatus";
            // 
            // barButtonItemPrint
            // 
            this.barButtonItemPrint.Caption = "Print";
            this.barButtonItemPrint.Id = 6;
            this.barButtonItemPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItemPrint.ImageOptions.SvgImage")));
            this.barButtonItemPrint.Name = "barButtonItemPrint";
            // 
            // ribbonPageHome
            // 
            this.ribbonPageHome.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupHome});
            this.ribbonPageHome.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ribbonPageHome.ImageOptions.SvgImage")));
            this.ribbonPageHome.Name = "ribbonPageHome";
            this.ribbonPageHome.Text = "Home";
            // 
            // ribbonPageGroupHome
            // 
            this.ribbonPageGroupHome.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ribbonPageGroupHome.ImageOptions.SvgImage")));
            this.ribbonPageGroupHome.ItemLinks.Add(this.barButtonItemMain);
            this.ribbonPageGroupHome.ItemLinks.Add(this.barButtonItemSettings);
            this.ribbonPageGroupHome.ItemLinks.Add(this.barButtonItemPrint);
            this.ribbonPageGroupHome.ItemLinks.Add(this.barButtonItemResetHistory);
            this.ribbonPageGroupHome.Name = "ribbonPageGroupHome";
            this.ribbonPageGroupHome.Text = "Actions";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItemVersion);
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItemStatus);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 1055);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1918, 24);
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "ribbonPage2";
            // 
            // documentManager1
            // 
            this.documentManager1.MdiParent = this;
            this.documentManager1.MenuManager = this.ribbonControl1;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // barButtonItemResetHistory
            // 
            this.barButtonItemResetHistory.Caption = "Reset Count";
            this.barButtonItemResetHistory.Id = 7;
            this.barButtonItemResetHistory.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItemResetHistory.ImageOptions.SvgImage")));
            this.barButtonItemResetHistory.Name = "barButtonItemResetHistory";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1918, 1079);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Main";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupHome;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemMain;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSettings;
        private DevExpress.XtraBars.BarStaticItem barStaticItemVersion;
        private DevExpress.XtraBars.BarStaticItem barStaticItemStatus;
        private DevExpress.XtraBars.BarButtonItem barButtonItemPrint;
        private DevExpress.XtraBars.BarButtonItem barButtonItemResetHistory;
    }
}

