using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class tblCoreDataCodeItemSizeModel
    {
        public Guid Id { get; set; }
        public string CodeItemSize { get; set; }
        public string MainItemName { get; set; }
        public int MetalScan { get; set; } //1 co; 0 ko
        public string Color { get; set; } = null;
        public int Printing { get; set; }//1 co; 0 ko
        public string Date { get; set; }
        public string Size { get; set; }
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
        public double PlasticBoxWeight { get; set; } = 1210;

        /// <summary>
        /// Non-HC. QtyOfBox/PartitionQty = qty of partition.
        /// </summary>
        public double PartitionQty { get; set; }

        /// <summary>
        /// HC partition qty.
        /// </summary>
        public double PartitionQtyOfBX1A { get; set; }
        /// <summary>
        /// HC partition qty.
        /// </summary>
        public double PartitionQtyOfBX2 { get; set; }
        /// <summary>
        /// HC partition qty.
        /// </summary>
        public double PartitionQtyOfBX3 { get; set; }
        [DisplayName("PlasticBag1Qty")]
        public double PlasticBag1Qty { get; set; }
        [DisplayName("PlasticBag2Qty")]
        public double PlasticBag2Qty { get; set; }
        public double WrapSheetQty { get; set; }
        public double  FoamSheetQty { get; set; }
        public double PartitionWeight { get; set; } = 0;
        [DisplayName("PlasticBag1Weight")]
        public double PlasticBag1Weight { get; set; }
        [DisplayName("PlasticBag2Weight")]
        public double PlasticBag2Weight { get; set; }
        public double WrapSheetWeight { get; set; }
        public double FoamSheetWeight { get; set; }
        [DisplayName("PlasticBoxWeight")]

        public double LowerToleranceOfCartonBox { get; set; } = 0;
        public double UpperToleranceOfCartonBox { get; set; } = 0;
        public double LowerToleranceOfPlasticBox { get; set; } = 0;
        public double UpperToleranceOfPlasticBox { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        [Browsable(false)]
        public bool IsActived { get; set; }
    }
}
