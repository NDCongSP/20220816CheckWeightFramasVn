﻿using Dapper;
using DevExpress.Data;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            grv.SelectionChanged += Grv_SelectionChanged;
        }

        private void Grv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView gv = (GridView)sender;
            try
            {
                _productNumber = gv.GetRowCellValue(gv.FocusedRowHandle, "ProductNumber").ToString();
                _codeItemZise = gv.GetRowCellValue(gv.FocusedRowHandle, "CodeItemSize") != null ? gv.GetRowCellValue(gv.FocusedRowHandle, "CodeItemSize").ToString() : string.Empty;
            }
            catch (Exception ex)
            {

            }
        }

        private void FrmMasterData_Load(object sender, EventArgs e)
        {
            GlobalVariables.MyEvent.EventHandlerRefreshMasterData += MyEvent_RefreshActionevent;
            this.grv.PopupMenuShowing += Grv_PopupMenuShowing;
            //this.grvSpecialCase.PopupMenuShowing += GrvSpecialCase_PopupMenuShowing;

            GlobalVariables.MyEvent.RefreshStatus = true;
        }

        private void GrvSpecialCase_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                e.Menu.Items.Add(new DXMenuItem("Update Item Infomation", new EventHandler(UpdateItemInfomation)));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi Get Data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grv_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                e.Menu.Items.Add(new DXMenuItem("Export Excel", new EventHandler(ExportExcelGrid)));
                e.Menu.Items.Add(new DXMenuItem("Update Item Infomation", new EventHandler(UpdateItemInfomation)));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi Get Data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateItemInfomation(object sender, EventArgs e)
        {
            try
            {
                frmUpdateTolerance frmUpdate = new frmUpdateTolerance();
                frmUpdate.ItemInfo.ProductNumber = _productNumber;
                frmUpdate.ItemInfo.CodeItemSize = _codeItemZise;

                frmUpdate.ShowDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi MixingLisr: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportExcelGrid(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel File|*.xlsx";
                    sfd.Title = "Chọn chổ để lưu.";
                    sfd.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}MasterData.xlsx";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        #region cach 1
                        //grcForecast.DefaultView.ExportToXlsx(sfd.FileName);
                        //if (File.Exists(sfd.FileName))
                        //{
                        //    var mbr = XtraMessageBox.Show("Export thành công !!!\n Bạn có muốn mở file?", "Thông báo", MessageBoxButtons.YesNo);
                        //    if (mbr == DialogResult.Yes)
                        //    {
                        //        Process.Start(sfd.FileName);
                        //    }
                        //}
                        //else
                        //{
                        //    XtraMessageBox.Show("Không tìm thấy file.");
                        //}
                        #endregion

                        // Create a PrintingSystem component.
                        DevExpress.XtraPrinting.PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();

                        // Create a link that will print a control.
                        DevExpress.XtraPrinting.PrintableComponentLink link = new PrintableComponentLink(ps);

                        // Specify the control to be printed.
                        link.Component = grc;
                        // Generate a report.
                        link.CreateDocument();

                        // Export the report to a PDF file.
                        link.PrintingSystem.ExportToXlsx(sfd.FileName);

                        Process.Start(sfd.FileName);//open file
                    }
                }
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
                    #region Master data
                    var winlineInfo = connection.Query<ProductInfoModel>("sp_vProductItemInfoGets").ToList();

                    if (winlineInfo != null && winlineInfo.Count > 0)
                    {
                        Console.WriteLine($"Get data from winline ok.");

                        if (grc.InvokeRequired)
                        {
                            grc.Invoke(new Action(() =>
                            {
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

                        grv.Columns["CreatedDate"].DisplayFormat.FormatString = "YYYY-MM-dd HH:mm:ss";
                    }
                    else
                    {
                        if (grc.InvokeRequired)
                        {
                            grc.Invoke(new Action(() =>
                            {
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
                    #endregion

                    #region Special case
                    GlobalVariables.SpecialCaseList = connection.Query<tblSpecialCaseModel>("sp_tblSpecialCaseGets").ToList();

                    this.Invoke((MethodInvoker)delegate
                    {
                        grcSpecialCase.DataSource = null;
                        grcSpecialCase.DataSource = GlobalVariables.SpecialCaseList;

                        grvSpecialCase.Columns["CreatedDate"].DisplayFormat.FormatString = "YYYY-MM-dd HH:mm:ss";
                    });
                    #endregion
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
            try
            {
                _productNumber = !string.IsNullOrEmpty(gv.GetRowCellValue(gv.FocusedRowHandle, "ProductNumber").ToString()) ? gv.GetRowCellValue(gv.FocusedRowHandle, "ProductNumber").ToString() : null;
                _codeItemZise = !string.IsNullOrEmpty(gv.GetRowCellValue(gv.FocusedRowHandle, "CodeItemSize").ToString()) ? gv.GetRowCellValue(gv.FocusedRowHandle, "CodeItemSize").ToString() : null;
            }
            catch (Exception ex)
            {

            }
        }
    }
}