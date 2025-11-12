using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ResponseObject
{
    public class GridResultObject
    {
        public int currentPage { get; set; }
        public object data { get; set; }
        public int pageSize{ get; set; }
        public bool status{ get; set; }
        public int totalItem{ get; set; }
        public int totalPage{ get; set; }


    }
}
