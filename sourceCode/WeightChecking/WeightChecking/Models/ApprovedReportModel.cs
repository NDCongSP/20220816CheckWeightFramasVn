using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class ApprovedReportModel
    {
        public string UserName { get; set; }
        public string IdLabel { get; set; }
        public string OC { get; set; }
        public string BoxNo { get; set; }
        public double GrossWeight { get; set; }
        public string Station { get; set; }
        public DateTime CreatedDate { get; set; }
        public string QRLabel { get; set; }
        public string ApproveType { get; set; }
        public double NetWeight { get; set; }
        public int Quantity { get; set; }
        public int CalculatorPairs { get; set; }
        public double Deviation { get; set; }
        public double DeviationPairs { get; set; }
        public double ActualDeviation { get; set; }
        public string Reason { get; set; }
        public Guid ScanDataId { get; set; }
    }
}
