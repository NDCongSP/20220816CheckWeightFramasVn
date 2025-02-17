using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public int Printing { get; set; }//1 co; 0 ko
        public string MainProductNo { get; set; }
        public string MainProductName { get; set; }
        public string Color { get; set; }
        public string SizeName { get; set; }
        public string ToolingNo { get; set; }
        public string MainItemName { get; set; }
        public double AveWeight1Prs { get; set; }
        public double BoxQtyBx1 { get; set; }
        public double BoxQtyBx1A { get; set; }
        public double BoxQtyBx2 { get; set; }
        public double BoxQtyBx3 { get; set; }
        public double BoxQtyBx4 { get; set; }
        public double BoxQtyBx5 { get; set; }
        public double BoxQtyBx6 { get; set; }
        public double BoxWeightBx1 { get; set; }
        public double BoxWeightBx1A { get; set; }
        public double BoxWeightBx2 { get; set; }
        public double BoxWeightBx3 { get; set; }
        public double BoxWeightBx4 { get; set; }
        public double BoxWeightBx5 { get; set; }
        public double BoxWeightBx6 { get; set; }
        public double PartitionQty { get; set; }
        public double PartitionQtyOfBX1A { get; set; }
        public double PartitionQtyOfBX2 { get; set; }
        public double PartitionQtyOfBX3 { get; set; }
        public double PlasticBag1Qty { get; set; }
        public double PlasticBag2Qty { get; set; }
        public double WrapSheetQty { get; set; }
        public double FoamSheetQty { get; set; }
        public double PartitionWeight { get; set; }
        public double PlasticBag1Weight { get; set; }
        public double PlasticBag2Weight { get; set; }
        public double WrapSheetWeight { get; set; }
        public double FoamSheetWeight { get; set; }
        public double PlasticBoxWeight { get; set; }
        [DisplayName("LowerToleranceOfTotalCartonBox")]
        public double LowerToleranceOfCartonBox { get; set; } = 0;
        [DisplayName("UpperToleranceOfTotalCartonBox")]
        public double UpperToleranceOfCartonBox { get; set; } = 0;
        [DisplayName("LowerToleranceOfTotalPlasticBox")]
        public double LowerToleranceOfPlasticBox { get; set; } = 0;
        [DisplayName("UpperToleranceOfTotalPlasticBox")]
        public double UpperToleranceOfPlasticBox { get; set; } = 0;
        public DateTime CreatedDate { get; set; }//Thời gian item được get từ WL về
    }
}
