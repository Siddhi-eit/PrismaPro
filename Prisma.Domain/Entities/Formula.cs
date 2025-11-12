using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.Entities
{
    public class Formula
    {
        public int? formulaID { get; set; }
        public string? ProductCode { get; set; }

        public int? UserID { get; set; }
        public string? Amount { get; set; }

        public List<colorDetail> colorDetail { get; set; }
        public int? UnitID { get; set; }

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

    }

    public class colorDetail
    {
        public string? ColorCode { get; set; }

        public string? DispenseAmount { get; set; }

        
    }
}
