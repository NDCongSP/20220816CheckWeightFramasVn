using Dapper;
using DevExpress.Data;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightChecking.Models.Entities;

namespace WeightChecking
{
    public partial class frmReports : Form
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Station { get; set; } = "All";

        Guid _id = Guid.Empty;
        string _idLabel, _oc, _boxId = string.Empty;
        string _passFail = "0";

        public frmReports()
        {
            InitializeComponent();

            grvReports.OptionsView.ShowAutoFilterRow = true;
            grvReports.OptionsCustomization.AllowFilter = true;
            grvReports.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            grvReports.OptionsView.ColumnAutoWidth = false;
            grvReports.OptionsCustomization.AllowSort = true;
            grvReports.OptionsBehavior.ReadOnly = true;
            grvReports.BestFitColumns();

            grvApprove.OptionsView.ShowAutoFilterRow = true;
            grvApprove.OptionsCustomization.AllowFilter = true;
            grvApprove.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            grvApprove.OptionsView.ColumnAutoWidth = false;
            grvApprove.OptionsCustomization.AllowSort = true;
            grvApprove.OptionsBehavior.ReadOnly = true;
            grvApprove.BestFitColumns();

            Load += FrmReports_Load;
        }

        private void FrmReports_Load(object sender, EventArgs e)
        {
            GlobalVariables.MyEvent.EventHandlerRefreshReport += (s, o) =>
            {
                RefreshData();
            };

            grvReports.PopupMenuShowing += GrvReports_PopupMenuShowing;
            grvReports.SelectionChanged += GrvReports_SelectionChanged;

            RefreshData();
        }

        private void GrvReports_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            GridView gv = (GridView)sender;
            try
            {
                _id = (Guid)gv.GetRowCellValue(gv.FocusedRowHandle, "Id");
                _idLabel = gv.GetRowCellValue(gv.FocusedRowHandle, "IdLabel") != null ? gv.GetRowCellValue(gv.FocusedRowHandle, "IdLabel").ToString() : string.Empty;
                _oc = (string)gv.GetRowCellValue(gv.FocusedRowHandle, "OcNo");
                _boxId = (string)gv.GetRowCellValue(gv.FocusedRowHandle, "BoxNo");
                _passFail = gv.GetRowCellValue(gv.FocusedRowHandle, "Pass").ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void GrvReports_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                e.Menu.Items.Add(new DXMenuItem("Delete Box", new EventHandler(DeleteBox)));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi Get Data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async void RefreshData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Please wait a moment.");
                SplashScreenManager.Default.SetWaitFormDescription("Loading...");


                DateTime from = Convert.ToDateTime(FromDate);
                DateTime to = Convert.ToDateTime(ToDate);


                using var dbContext = new ApplicationDbContext(GlobalVariables.ConnectionString);

                #region Scan Data
                var res = await dbContext.TblScanDatas
                    .Where(x =>
                        x.Actived == 1 &&
                        x.CreatedDate >= from &&
                        x.CreatedDate <= to
                    )
                    .ToListAsync();


                if (grcReports.InvokeRequired)
                {
                    grcReports.Invoke(new Action(() =>
                    {
                        grcReports.DataSource = res;
                    }));
                }
                else
                {
                    grcReports.DataSource = res;
                }

                grvReports.Columns["CreatedDate"].DisplayFormat.FormatString = "YYYY-MM-dd HH:mm:ss";
                grvReports.Columns["ProductNumber"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                grvReports.Columns["IdLabel"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                grvReports.Columns["OcNo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                grvReports.Columns["BoxNo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                grvReports.Columns["Station"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
                grvReports.Columns["ApprovedName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
                grvReports.Columns["ActualDeviationPairs"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
                grvReports.Columns["DeviationPairs"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
                grvReports.Columns["CalculatedPairs"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
                grvReports.Columns["CreatedDate"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
                #endregion

                #region Approved Print
                //var resApproved = connection.Query<ApprovedModel>("sp_tblApprovedPrintLableSelect", parametters, commandType: CommandType.StoredProcedure).ToList();
                var resApproved = dbContext.TblApprovedPrintLabels
                     .Where(x =>
                         x.CreatedDate >= from &&
                        x.CreatedDate <= to
                    )
                    .ToListAsync();

                if (grcApprove.InvokeRequired)
                {
                    grcApprove.Invoke(new Action(() =>
                    {
                        grcApprove.DataSource = resApproved;
                    }));
                }
                else
                {
                    grcApprove.DataSource = resApproved;
                }

               // grvApprove.Columns["CreatedDate"].DisplayFormat.FormatString = "YYYY-MM-dd HH:mm:ss";
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Report exception.");
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void DeleteBox(object sender, EventArgs e)
        {
            try
            {
                frmDeleteBox frmUpdate = new frmDeleteBox()
                {
                    Id = _id,
                    BoxId = _boxId,
                    Oc = _oc,
                    IdLabel = _idLabel,
                    PassFail = _passFail
                };
                frmUpdate.StartPosition = FormStartPosition.CenterScreen;
                frmUpdate.ShowDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Delete Box Fail." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
