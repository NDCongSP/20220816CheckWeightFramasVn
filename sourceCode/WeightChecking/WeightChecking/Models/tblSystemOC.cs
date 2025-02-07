using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking.Models
{
    public class tblSystemOC
    {
        public Guid Id { get; set; }
        public string FirstChar { get; set; }
        public string Description { get; set; }
        public bool Actived { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
