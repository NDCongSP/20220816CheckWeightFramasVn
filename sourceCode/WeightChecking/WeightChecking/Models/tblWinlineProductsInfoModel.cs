using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
  public  class tblWinlineProductsInfoModel
    {
        public Guid Id { get; set; }
        public string ProductNunmber { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int? Decoration { get; set; }
        public string MainProductNo { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public double? Weight { get; set; }
        public string PackingMethod { get; set; }
        public double? LeftWeight { get; set; }
        public double? RightWeight { get; set; }
        public string BoxType { get; set; }
        public string ToolingNo { get; set; }
        public string PackingBoxType { get; set; }
        public string CustomeUsePb { get; set; }
        public int? PlacticBox { get; set; }
        public double? PPbagWeight { get; set; }
        public double? Bx1Weight { get; set; }
        public double? Bx1AWeight { get; set; }
        public double? Bx2Weight { get; set; }
        public double? Bx3Weight { get; set; }
        public double? Bx4Weight { get; set; }
        public double? Bx5Weight { get; set; }
        public int? Bx1_50_40_34 { get; set; }
        public int? Bx1A { get; set; }
        public int? Bx2_50_40_17 { get; set; }
        public int? Bx3_41_32_31 { get; set; }
        public int? Bx4_32_23_15 { get; set; }
        public int? Bx5_41_32_31 { get; set; }
        public double? PlaticBoxWeight { get; set; }
        public int? PEUW { get; set; }
        public double? BagWeight { get; set; }
        public double? PartitionWeight { get; set; }
        public int? QtyPerbag { get; set; }
        public int? QtyPerPartition { get; set; }
        public int? QtyPerWrapSheet { get; set; }
        public double? WrapSheetWeight { get; set; }
        public string ToleranceBeforePrint { get; set; }
        public string ToleranceAfterPrint { get; set; }
        public string ProductFillter { get; set; }
        public bool? Actived { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedMachine { get; set; }
        public int MetalScan { get; set; }

    }
}
