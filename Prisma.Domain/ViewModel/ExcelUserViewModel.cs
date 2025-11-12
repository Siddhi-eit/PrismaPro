using MDFusionLabHaute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ViewModel
{

    public class ExcelUserViewModel
    {
        public int? ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public string? Phone { get; set; }
        //public string? ProfileImage { get; set; }
        //public string? ResetCode { get; set; }
        //public string? ResetCodeExpiry { get; set; }
        public string? CreatedDate { get; set; }
        //public string? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        //public bool? IsDeleted { get; set; }

    }
}