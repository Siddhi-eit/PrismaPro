using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Common
{
    public class ServerSettings
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public ServerSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            APIURL = _configuration.GetSection("APIURL").ToString();
            WEBPHYSICALUPLOADPATH = _configuration.GetSection("WebPhysicalUploadPath").ToString();
            UPLOADFOLDERNAME = _configuration.GetSection("UploadFolderName").ToString();
            IMAGEFILEPATH = _configuration.GetSection("ImageFilePath").ToString();
            JWTSECRETKEY = _configuration.GetSection("JWTSecretKey").ToString();
        }
        #endregion

        public static string APIURL;
        public static string WEBPHYSICALUPLOADPATH;
        public static string UPLOADFOLDERNAME;
        public static string IMAGEFILEPATH;
        public static string JWTSECRETKEY;
    }
}