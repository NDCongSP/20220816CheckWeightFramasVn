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
    public partial class frmAddSpecialCase : DevExpress.XtraEditors.XtraForm
    {

        public frmAddSpecialCase()
        {
            InitializeComponent();
            Load += FrmAddSpecialCase_Load;
            _btnSave.Click += _btnSave_Click;
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_txtMainItem.Text) && !_txtMainItem.Text.Contains(" ") && _txtMainItem.Text.Length == 10)
            {
                using (var connection = GlobalVariables.GetDbConnection())
                {
                    var para = new DynamicParameters();

                    para.Add("MainItem", _txtMainItem.Text);

                    var res = connection.Execute("sp_tblSpecialCaseInsert", para, commandType: CommandType.StoredProcedure);

                    if (res == 0)
                    {
                        MessageBox.Show("Không thành công.", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    GlobalVariables.MyEvent.RefreshStatus = true;
                }
            }
            else
            {
                MessageBox.Show("Main Item chưa đúng định dạng. Vui lòng kiểm tra lại.", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmAddSpecialCase_Load(object sender, EventArgs e)
        {

        }
    }
}