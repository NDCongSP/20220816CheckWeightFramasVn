using Dapper;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
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
    public partial class frmReports : Form
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Station { get; set; } = "All";

        public frmReports()
        {
            InitializeComponent();
            Load += FrmReports_Load;
        }

        private void FrmReports_Load(object sender, EventArgs e)
        {
            GlobalVariables.MyEvent.EventHandlerRefreshReport += (s, o) => {
                RefreshData();
            };

            RefreshData();
        }

        void RefreshData() {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SplashScreenManager.Default.SetWaitFormCaption("Vui lòng chờ trong giây lát");
                SplashScreenManager.Default.SetWaitFormDescription("Loading...");

                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var parametters = new DynamicParameters();
                    parametters.Add("FromDate", FromDate);
                    parametters.Add("ToDate", ToDate);
                    parametters.Add("Station", Station);

                    var res = connection.Query<tblScanDataModel>("sp_tblScanDataGets",parametters,commandType:CommandType.StoredProcedure).ToList();

                    if (grcReports.InvokeRequired)
                    {
                        grcReports.Invoke(new Action(() => {
                            grcReports.DataSource = res;
                        }));
                    }
                    else
                    {
                        grcReports.DataSource = res;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi Report exception.");
                XtraMessageBox.Show("Lỗi Report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
    }
}
