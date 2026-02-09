using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeightChecking
{
    [Table("tblConfig")]
    public partial class tblConfig
    {
        [Key]
        public Guid Id { get; set; }

        public StationEnum Location { get; set; } 

        /// <summary>
        /// Chính là ConfigJsonModel.
        /// </summary>
        public string ConfigJson { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string CreatedMachine { get; set; } = string.Empty;
    }

    public class ConfigJsonModel
    {
        [Description("Connection string to database SSFG.")]
        public string ConStringDogeWH { get; set; } = "ed3YbBgz3fEdyTkRahthFY5ktQmH2er+ubV7i40QDz2jV8uuycc9LsTSR22vhbKhFKqwBS0vr6oQqsdO70hZk3qfE4J1XeLCiQ11XIAwyoVU6JH7Uqs1cnz54UD5SAyUuepHTyDPxBSwysZYaZtRX9R2BQUDXTzYsMrGWGmbUV/Q2S0w2U+T2iLkT2IThDNOInazRVku4GwKrY+JotqRuP5bw/Zs1yGq8wCbDlnf/IA=";

        [Description("Connection string to database DOGE_WH.")]
        public string ConStringWL { get; set; } = "ed3YbBgz3fEdyTkRahthFY5ktQmH2er+ubV7i40QDz2+hAazJukJ2KdBD28UEGTZpJCCeedpXvxIaU3kh+lTExB/npJz2Uw5lNuwB800UZyeKbvZFrfjjijyurbNaMoE7IXQkLVO+pvLS+9V/AFxNrE1qo49bGzvAnZUvm1Uo3o1fPqAH5rwrQmiR/MZQW0YE/hEqI8KBWqqEiZJeE8Dbt/Bw1H8THChh6y2CqZBLz4=";

        [Description("The IP address of scale.")]
        public string IpScale { get; set; } = "10.11.17.163";

        public double UnitScale { get; set; } = 1000;

        [Description("Enable to scale.")]
        public bool IsScale { get; set; } = true;

        [Description("Enable to scale.")]
        public bool IsCounter { get; set; } = false;

        public int AfterPrinting { get; set; } = 0;

        [Description("The path folder to update version for application.")]
        public string UpdatePath { get; set; } = "\\\\192.168.1.241\\FramasPublic\\PUBLIC_Able to deleted\\22 IT\\01-UpdateApp\\11-SSFG_IDC\\1.Station1BeforePrint\\Update.xml";

        public double RatioFailWeight { get; set; } = 0.5;

        //dùng để kết nối với PLC cảnh báo
        public string ComPort { get; set; } = "COM2";

        /// <summary>
        /// Thời gian đếm ngược để reset UI dau khi thực hiện xong.
        /// đơn vị (s).
        /// </summary>
        public int ResetUiInterval { get; set; } = 60;

        /// <summary>
        /// Giá trị dung sai dưới của thùng carton (%).
        /// </summary>
        public double? LowerToleranceOfCartonBox { get; set; } = 1;

        /// <summary>
        /// Giá trị dung sai trên của thùng carton (%).
        /// </summary>
        public double? UpperToleranceOfCartonBox { get; set; } = 1;

        /// <summary>
        /// Dùng để ẩn hiện  textbox mô phỏng nhập giá trị cân để test.
        /// </summary>
        public bool IsTest { get; set; } = false;
    }
}
