using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class ApprovedModel
    {
        public Guid Id { get; set; }
        public Guid QrCode { get; set; }
        public string UserName { get; set; }
        public string IdLabel { get; set; }
        public string OC { get; set; }
        public string BoxNo { get; set; }
        public double GrossWeight { get; set; }
        public StationEnum Station { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedMachine { get; set; }
        public string QRLabel { get; set; }
        public string ApproveType { get; set; }
        public double NetWeight { get; set; }
        public int Quantity { get; set; }
        public int CalculatorPrs { get; set; }
        [DisplayName("DeviationWeight(g)")]
        public double Deviation { get; set; }
        [DisplayName("CalculatedDeviationPairs")]
        public double DeviationPairs { get; set; }
        public double ActualDeviationPairs { get; set; }
        public string Reason { get; set; }
        public Guid ScanDataId { get; set; }
    }
}
