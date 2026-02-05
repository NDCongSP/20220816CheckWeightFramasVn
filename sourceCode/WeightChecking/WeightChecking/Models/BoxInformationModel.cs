using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class BoxInformationModel
    {
        public string? BoxType { get; set; }

        public string? Dimension { get; set; }

        public double? BoxWeight { get; set; } = 0;
    }
}
