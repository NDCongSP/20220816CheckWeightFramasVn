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

            Console.WriteLine($"Path app: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            GlobalVariables.ReInfo = JsonConvert.DeserializeObject<RememberInfo>(File.ReadAllText(@"./RememberInfo.json"));

            GlobalVariables.ReInfo.UserName = EncodeMD5.DecryptString(GlobalVariables.ReInfo.UserName, "ITFramasBDVN");
            GlobalVariables.ReInfo.Pass = EncodeMD5.DecryptString(GlobalVariables.ReInfo.Pass, "ITFramasBDVN");
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
