using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class BoxParentModel
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Prefix { get; set; }
        public string ParentOc { get; set; }
        public string ParentBoxCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
