using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.ResponseObject
{
    public class ResponseObjectForAnything
    {
        #region Properties

        /// <summary>
        /// Result code either Success/Error.
        /// </summary>
        public string ResultCode { get; set; }
        /// <summary>
        /// Error message to be returned in case
        /// of error.
        /// </summary>
        public string ResultMessage { get; set; }
        /// <summary>
        /// ID of the result object. 
        /// </summary>
        public int ResultObjectID { get; set; }
        /// <summary>
        /// Result object returned.  
        /// </summary>
        public object ResultObject { get; set; }

        public string ResultNotes { get; set; }

        public static implicit operator ResponseObjectForAnything(string v)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
