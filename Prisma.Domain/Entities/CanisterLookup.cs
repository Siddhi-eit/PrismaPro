using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.Entities
{
    public class CanisterLookup : Audit
    {
        public int ID { get; set; }

        public string? Name { get; set; }
        public int? IsActive { get; set; }
        public string? CanisterCode { get; set; }
        public string? SKU { get; set; }
    }
}
