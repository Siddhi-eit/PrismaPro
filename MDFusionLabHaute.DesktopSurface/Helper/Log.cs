using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MDFusionLabHaute.DesktopSurface.Helper
{
    public static class Log
    {
        public static string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];

        public static async void Logger(int userId,int machineId,string ExceptionMessage, string ExceptionSource)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            string apiUrl = _apiURL + "Log/InsertLog";
            String[] input = new String[2];
            input[0] = userId.ToString();
            input[1] = machineId.ToString();
            input[2] = ExceptionMessage;
            input[3] = ExceptionSource;
            string inputJson = (new JavaScriptSerializer()).Serialize(input);

            using (var wc = new WebClient())
            {
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);
                responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result.ToString());
            }
        }
    }
}
