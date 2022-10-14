using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class ProductInfoModel
    {
        public string CodeItemSize { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public int ProductCategory { get; set; }
        public string Brand { get; set; }
        public int Decoration { get; set; }
        public int MetalScan { get; set; }
        public string MainProductNo { get; set; }
        public string MainProductName { get; set; }
        public string Color { get; set; }
        public string SizeName { get; set; }
        public string ToolingNo { get; set; }
        public string MainItemName { get; set; }
        public double AveWeight1Prs { get; set; }
        public int BoxQtyBx1 { get; set; }
        public int BoxQtyBx2 { get; set; }
        public int BoxQtyBx3 { get; set; }
        public int BoxQtyBx4 { get; set; }
        public double BoxWeightBx1 { get; set; }
        public double BoxWeightBx2 { get; set; }
        public double BoxWeightBx3 { get; set; }
        public double BoxWeightBx4 { get; set; }
        public int PartitionQty { get; set; }
        public int PlasicBagQty { get; set; }
        public int WrapSheetQty { get; set; }
        public double PartitionWeight { get; set; }
        public double PlasicBagWeight { get; set; }
        public double WrapSheetWeight { get; set; }
        public double PlasicBoxWeight { get; set; }
        public double Tolerance { get; set; }
        public double ToleranceAfterPrint { get; set; }
    }
}
