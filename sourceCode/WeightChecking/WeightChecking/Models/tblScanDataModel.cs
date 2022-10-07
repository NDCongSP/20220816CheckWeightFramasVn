using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class tblScanDataModel
    {
        public Guid Id { get; set; }
        public string BarcodeString { get; set; }
        public string OcNo { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string LinePosNo { get; set; }
        public string Unit { get; set; }
        public string BoxNo { get; set; }
        public string CustomerNo { get; set; }
        public int Location { get; set; }
        public string BoxPosNo { get; set; }
        public int Pass { get; set; }
        public string Note { get; set; }
        public string Brand { get; set; }
        public int Decoration { get; set; }
        public int MetalScan { get; set; }
        public string MainProductNo { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public double Weight { get; set; }
        public string PackingMethod { get; set; }
        public double LeftWeight { get; set; }
        public double RightWeight { get; set; }
        public string BoxType { get; set; }
        public string ToolingNo { get; set; }
        public string PackingBoxType { get; set; }
        public string CustomerUsePlactixBox { get; set; }
        public int PlacticBox { get; set; }
        public double PPbagWeight { get; set; }
        public double BX1Weight { get; set; }
        public double BX2Weight { get; set; }
        public double BX3Weight { get; set; }
        public double BX4Weight { get; set; }
        public double BX1AWeight { get; set; }
        public double BX5Weight { get; set; }
        public double PlacticBoxWeight { get; set; }
        public int BX1 { get; set; }
        public int BX2 { get; set; }
        public int BX3 { get; set; }
        public int BX4 { get; set; }
        public int BX1A { get; set; }
        public int BX5 { get; set; }
        public int PEUW { get; set; }
        public double BagWeight { get; set; }
        public double PartitionWeight { get; set; }
        public int QtyBag { get; set; }
        public int QtyPartition { get; set; }
        public int QtyWrapSheet { get; set; }
        public double WrapSheetWeight { get; set; }
        public double Tolerance { get; set; }
        public double StandardWeight { get; set; }
        public double RealWeight { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Actived { get; set; }
        public string CreatedBy { get; set; }

    }
}
