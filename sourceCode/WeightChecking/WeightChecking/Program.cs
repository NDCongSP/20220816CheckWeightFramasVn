using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace WeightChecking
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region Đọc các thông số cấu hình ban đầu từ settings
            GlobalVariables.ConnectionString = EncodeMD5.DecryptString(Properties.Settings.Default.conString, "ITFramasBDVN");
            GlobalVariables.ConStringWinline = EncodeMD5.DecryptString(Properties.Settings.Default.conStringWL, "ITFramasBDVN");
            GlobalVariables.ScaleIp = Properties.Settings.Default.ipScale;
            GlobalVariables.UnitScale = int.TryParse(Properties.Settings.Default.UnitScale, out int value) ? value : 0;

            Console.WriteLine($"Path app: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            GlobalVariables.RememberInfo = JsonConvert.DeserializeObject<RememberInfo>(File.ReadAllText(@"./RememberInfo.json"));

            if (GlobalVariables.RememberInfo.Remember)
            {
                GlobalVariables.RememberInfo.UserName = EncodeMD5.DecryptString(GlobalVariables.RememberInfo.UserName, "ITFramasBDVN");
                GlobalVariables.RememberInfo.Pass = EncodeMD5.DecryptString(GlobalVariables.RememberInfo.Pass, "ITFramasBDVN");
            }
            #endregion

            //Log các hành động của user thì tự log bằng tay vào bảng tblLog
            //tạo serilog để log Error exception.
            MSSqlServerSinkOptions sinkOption = new MSSqlServerSinkOptions()
            {
                TableName = "tblLog",
                AutoCreateSqlTable = true,
            };
            Log.Logger = new LoggerConfiguration().WriteTo.MSSqlServer(

              connectionString: GlobalVariables.ConnectionString,
              sinkOptions: sinkOption

              ).MinimumLevel.Error().CreateLogger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
