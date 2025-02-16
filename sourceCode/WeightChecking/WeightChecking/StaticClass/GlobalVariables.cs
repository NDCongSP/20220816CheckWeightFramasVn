﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using PLCPiProject;
using WeightChecking.Models;

namespace WeightChecking
{
    public static class GlobalVariables
    {
        public static string ConnectionString { get; set; }
        public static string ConStringWinline { get; set; }
        public static string ConStringDogeWh { get; set; }

        #region Tao các connection DB
        public static IDbConnection GetDbConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static IDbConnection GetDbConnectionWinline()
        {
            return new SqlConnection(ConStringWinline);
        }

        public static IDbConnection GetDbConnectionDogeWh()
        {
            return new SqlConnection(ConStringDogeWh);
        }
        #endregion

        //chứa các thông tin cần lưu lại để khi mở phần mềm lên thì sẽ đọc lên để tiếp tục làm việc.
        public static RememberInfo RememberInfo { get; set; } = new RememberInfo();

        public static RefreshEvent MyEvent = new RefreshEvent();

        //biến cấu hình cân
        public static string IpScale = "";
        public static string PortScale = "23";
        public static string ScaleStatus = "Disconnected";
        public static int ScaleDelay = 300;
        public static int UnitScale { get; set; } = 0;

        public static tblUsers UserLoginInfo { get; set; } = new tblUsers();

        public static PLCPi MyDriver = new PLCPi();
        public static byte[] ReadHoldingArr { get; set; }
        public static bool ModbusStatus { get; set; }
        public static string ComPort { get; set; }

        public static bool IsScale { get; set; } = false;
        public static bool IsCounter { get; set; } = false;
        public static StationEnum Station { get; set; }

        /// <summary>
        /// Tỉ lệ khối lượng calculate deviation / standar Gross weight.
        /// % = [|CalculateDeviation (prs)| * AveWeight/Prs (g)]/SdtGrossWeight (g).
        /// </summary>
        public static double RatioFailWeight { get; set; } = 0;

        /// <summary>
        /// Bao can o tram truoc son hay sau son.0-trước sơn (IDC1); 1-sau sơn (IDC2,Kerry).
        /// </summary>
        public static int AfterPrinting { get; set; } = 0;

        #region Printing
        // Print the file.
        public static void Printing(string content, string idLabel, bool pass, string createdDate)
        {
            //content of the QR code "OC283225,6112012227-2094-2651,28,13,P,1/56,160506,1/1|1,30.2022"
            if (pass)
            {
                var rptRe = new rptLabel();
                //rptRe.DataSource = ds;

                rptRe.Parameters["Weight"].Value = content;
                rptRe.Parameters["IdLabel"].Value = idLabel;
                rptRe.Parameters["CreatedDate"].Value = createdDate;

                rptRe.CreateDocument();
                ReportPrintTool printToolCrush = new ReportPrintTool(rptRe);
                printToolCrush.Print();
            }
            else
            {
                var rptRe = new rptLabelFail();
                //rptRe.DataSource = ds;

                rptRe.Parameters["DeviationPrs"].Value = content;
                rptRe.Parameters["IdLabel"].Value = idLabel;
                rptRe.Parameters["CreatedDate"].Value = createdDate;

                rptRe.CreateDocument();
                ReportPrintTool printToolCrush = new ReportPrintTool(rptRe);
                printToolCrush.Print();
            }
        }

        public static double RealWeight { get; set; } = 0;
        public static string IdLabel { get; set; } = null;
        public static string OcNo { get; set; } = null;
        public static string BoxNo { get; set; } = null;
        public static bool PrintApprove { get; set; } = false;
        public static DateTime CreatedDate { get; set; }//dung de chua thoi gian tạo, để đồng bộ giữa in tem và log DB. dung trong Confirm in tem

        public static List<tblSpecialCaseModel> SpecialCaseList { get; set; } = new List<tblSpecialCaseModel>();
        public static string UpdatePath { get; internal set; }
        public static List<OcUsingModel> OcUsingList { get; set; } = new List<OcUsingModel>();//get ra danh sách tất cả các OcNo đang sử dụng

        /// <summary>
        /// Chứa thông tin các đầu OC được sử dụng trong hệ thống.
        /// Hiện tại chỉ quản lý những đầu ko có trên WL vì nó ko có voucherType: buffer HC.
        /// </summary>
        public static List<tblSystemOC> SystemOC { get; set; }=new List<tblSystemOC>();

        public static ResultPostingModel ResultPosting { get; set; } = new ResultPostingModel();
        #endregion

        public static void InvokeIfRequired(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
