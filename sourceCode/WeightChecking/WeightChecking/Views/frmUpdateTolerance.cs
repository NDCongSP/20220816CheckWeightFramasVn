using Dapper;
using DevExpress.XtraEditors;
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
    public partial class frmUpdateTolerance : DevExpress.XtraEditors.XtraForm
    {
        public string Id { get; set; }

        tblWinlineProductsInfoModel _info;

        public frmUpdateTolerance()
        {
            InitializeComponent();

            Load += FrmUpdateTolerance_Load;
        }

        private void FrmUpdateTolerance_Load(object sender, EventArgs e)
        {
            var para = new DynamicParameters();
            para.Add("@Id",Id);

            using (var connection=GlobalVariables.GetDbConnection())
            {
                _info = connection.Query<tblWinlineProductsInfoModel>("sp_tblWinlineProductsInfoGetId",para,commandType:CommandType.StoredProcedure).FirstOrDefault();

                if (_info!=null)
                {
                    labProductCode.Text = _info.ProductNunmber;
                    labProductName.Text = _info.Name;
                    txtTolerance.Text = _info.Tolerance.ToString();
                    txtToleranceBeforePrint.Text = _info.ToleranceBeforePrint.ToString();
                    txtToleranceAfterPrint.Text = _info.ToleranceAfterPrint.ToString();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var para = new DynamicParameters();
                    para.Add("@Id", Id);
                    para.Add("@Tolerance", txtTolerance.Text);
                    para.Add("@ToleranceBeforePrint", txtToleranceBeforePrint.Text);
                    para.Add("@ToleranceAfterPrint", txtToleranceAfterPrint.Text);

                    var res = connection.Execute("sp_tblWinlineProductsInfoUpdateTolerance", para, commandType: CommandType.StoredProcedure);

                    XtraMessageBox.Show("Update tolerance successfull.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Update tolerance Fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(ex,"Update tolerance Fail exception.");
            }
            finally
            {
                GlobalVariables.MyEvent.RefreshStatus = true;
            }
        }
    }
}