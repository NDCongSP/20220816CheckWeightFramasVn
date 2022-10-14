using Dapper;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeightChecking
{
    public partial class frmMasterData : DevExpress.XtraEditors.XtraForm
    {
        string _productNumber;
        string _codeItemZise;
        public frmMasterData()
        {
            InitializeComponent();

            Load += FrmMasterData_Load;
        }

        private void FrmMasterData_Load(object sender, EventArgs e)
        {
            GlobalVariables.MyEvent.RefreshActionevent += MyEvent_RefreshActionevent;
            this.grv.PopupMenuShowing += Grv_PopupMenuShowing;

            GlobalVariables.MyEvent.RefreshStatus = true;
        }

        private void Grv_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                e.Menu.Items.Add(new DXMenuItem("Update Tolerance", new EventHandler(UpdateTolerance)));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi Get Data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTolerance(object sender, EventArgs e)
        {
            try
            {
                frmUpdateTolerance frmUpdate = new frmUpdateTolerance();
                frmUpdate._info.ProductNumber = _productNumber;
                frmUpdate._info.CodeItemSize = _codeItemZise;

                frmUpdate.ShowDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi MixingLisr: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MyEvent_RefreshActionevent(object sender, EventArgs e)
        {
            try
            {
                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var winlineInfo = connection.Query<ProductInfoModel>("sp_vProductItemInfoGets").ToList();

                    if (winlineInfo != null && winlineInfo.Count > 0)
                    {
                        Console.WriteLine($"Get data from winline ok.");

                        if (grc.InvokeRequired)
                        {
                            grc.Invoke(new Action(()=> {
                                grc.DataSource = null;
                                grc.DataSource = winlineInfo;

                                grv.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
                                grv.OptionsView.ColumnAutoWidth = true;
                                //grv.Columns["Id"].Visible = false;
                            }));
                        }
                        else
                        {
                            grc.DataSource = null;
                            grc.DataSource = winlineInfo;
                        }
                    }
                    else
                    {
                        if (grc.InvokeRequired)
                        {
                            grc.Invoke(new Action(() => {
                                grc.DataSource = null;
                            }));
                        }
                        else
                        {
                            grc.DataSource = null;
                        }

                        Console.WriteLine($"Refresh master data.");
                        Log.Error("Refresh master data fail.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Get data from winline exception.");
            }
            finally
            {
                GlobalVariables.MyEvent.RefreshStatus = false;
            }
        }

        private void grv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView gv = (GridView)sender;

            _productNumber = gv.GetRowCellValue(gv.FocusedRowHandle,"ProductNumber").ToString();
            _codeItemZise = gv.GetRowCellValue(gv.FocusedRowHandle,"CodeItemSize").ToString();
        }
    }
}