using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class RememberInfo
    {
        public string UserName { get; set; }
        public string Pass { get; set; }
        public bool Remember { get; set; }
        public int GoodBoxNoPrinting { get; set; }
        public int GoodBoxPrinting { get; set; }
        public int FailBoxNoPrinting { get; set; }
        public int FailBoxPrinting { get; set; }
    }
}
