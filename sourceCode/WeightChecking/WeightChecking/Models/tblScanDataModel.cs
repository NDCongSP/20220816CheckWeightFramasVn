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
        public string IdLable { get; set; }
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
        public string Note { get; set; }
        public string Brand { get; set; }
        public int Decoration { get; set; }
        public int MetalScan { get; set; }
       
        public double NetWeight { get; set; }
        public double BoxWeight { get; set; }
        public double AccessoriesWeight { get; set; }
        public double GrossdWeight { get; set; }
        public double RealWeight { get; set; }
        public int Pass { get; set; }
        //Báo trạng thái: 0- thùng fail; 1- chờ đi sơn; 2- Done hàng FG qua kho Kerry.
        //Ở trạm IDC check nêu hàng noPrinting thì set =2. nếu printing set =1.
        //Khi hàng đi sơn về, vào trạm check afterPrinting, quét OK set =2
        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public int Actived { get; set; }
        public string CreatedBy { get; set; }

    }
}
