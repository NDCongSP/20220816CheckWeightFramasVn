using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    [Table("tblApprovedPrintLabel")]
    public class tblApprovedPrintLabel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid QrCode { get; set; }

        public string IdLabel { get; set; }

        public string OC { get; set; }

        public string BoxNo { get; set; }

        public string QRLabel { get; set; }

        public string ApproveType { get; set; }

        public double? GrossWeight { get; set; }

        public double? NetWeight { get; set; }

        public int? Quantity { get; set; }

        public double? CalculatorPrs { get; set; }

        public double? Deviation { get; set; }

        public double? DeviationPairs { get; set; }

        public double? ActualDeviationPairs { get; set; }

        public StationEnum? Station { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedMachine { get; set; }

        public Guid? ScanDataId { get; set; }

        public string Reason { get; set; }
    }
}
