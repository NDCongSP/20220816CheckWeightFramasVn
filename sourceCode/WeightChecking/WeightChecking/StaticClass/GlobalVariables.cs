using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeightChecking
{
    public static class GlobalVariables
    {
        public static string ConnectionString { get; set; }
        public static string ScaleIp { get; set; }

        public static IDbConnection GetDbConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        //chứa các thông tin cần lưu lại để khi mở phần mềm lên thì sẽ đọc lên để tiếp tục làm việc.
        public static RememberInfo ReInfo { get; set; } = new RememberInfo();

        public static RefreshEvent MyEvent = new RefreshEvent();
    }
}
