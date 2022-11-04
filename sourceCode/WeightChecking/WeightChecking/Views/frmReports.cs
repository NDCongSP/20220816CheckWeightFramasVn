using Dapper;
using DevExpress.XtraSplashScreen;
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
        public frmReports()
        {
            InitializeComponent();
            Load += FrmReports_Load;
        }

        private void FrmReports_Load(object sender, EventArgs e)
        {
            GlobalVariables.MyEvent.RefreshActionevent += (s, o) => {
                RefreshData();
            };

            RefreshData();
        }

        void RefreshData() {
            try
            {
                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var res = connection.Query<tblScanDataModel>("sp_tblScanDataGets").ToList();

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

            }
            finally
            {
                
            }
        }
    }
}
