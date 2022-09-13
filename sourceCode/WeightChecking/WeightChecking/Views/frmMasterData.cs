using Dapper;
using DevExpress.XtraEditors;
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
        public frmMasterData()
        {
            InitializeComponent();

            Load += FrmMasterData_Load;
        }

        private void FrmMasterData_Load(object sender, EventArgs e)
        {
            GlobalVariables.MyEvent.RefreshActionevent += MyEvent_RefreshActionevent;
        }

        private void MyEvent_RefreshActionevent(object sender, EventArgs e)
        {
            using (var connection = GlobalVariables.GetDbConnectionWinline())
            {
                var winlineInfo = connection.Query<tblWinlineProductsInfoModel>("sp_IdcScanScaleGetCoreData").ToList();

                if (winlineInfo != null && winlineInfo.Count > 0)
                {
                    Console.WriteLine($"Get data from winline ok.");
                }
                else
                {
                    Console.WriteLine($"Get data from winline fail.");
                }
            }

            GlobalVariables.MyEvent.RefreshStatus = false;
        }
    }
}