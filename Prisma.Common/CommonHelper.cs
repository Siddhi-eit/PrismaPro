using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Common
{
    public class CommonHelper
    {
        // THIS IS USED WHEN CONVERTING DATA GETTING FROM DATABASE 
        public static T FromDB<T>(object pValue)
        {
            return pValue == DBNull.Value ? default(T) : (T)pValue;
        }

        // THIS IS USED WHEN WE HAVE PASS PERAMETER IN DB
        public static object ToDB<T>(T pValue)
        {
            return pValue == null ? (object)DBNull.Value : pValue;
        }

        // THIS IS USED WHEN CONVERTING RESPONSE OBJECT TO MODEL
        public static T FromResponse<T>(object pValue)
        {
            return pValue == null ? default(T) : (T)pValue;
        }

        // THIS IS USED WHEN CONVERTING DATA IN PARTICULER TYPE 
        public static T ConvertTo<T>(object pValue)
        {
            return pValue == null ? default(T) : (T)Convert.ChangeType(pValue, typeof(T));
        }

        // THIS IS USED FOR CLEANING FILE NAME 
        public static string CleanFileName(string pInput)
        {
            string result = pInput;
            if (pInput != null)
            {

                result = result.Replace(" ", "-");
                result = result.Replace("\"", "");
                result = result.Replace("&", "and");
                result = result.Replace("?", "");
                result = result.Replace("=", "");
                result = result.Replace("/", "");
                result = result.Replace("\\", "");
                result = result.Replace("%", "");
                result = result.Replace("#", "");
                result = result.Replace("*", "");
                result = result.Replace("!", "");
                result = result.Replace("$", "");
                result = result.Replace("+", "-plus-");
                result = result.Replace(",", "-");
                result = result.Replace("@", "-at-");
                result = result.Replace(":", "-");
                result = result.Replace(";", "-");
                result = result.Replace(">", "");
                result = result.Replace("<", "");
                result = result.Replace("{", "");
                result = result.Replace("}", "");
                result = result.Replace("~", "");
                result = result.Replace("|", "-");
                result = result.Replace("^", "");
                result = result.Replace("[", "");
                result = result.Replace("]", "");
                result = result.Replace("`", "");
                result = result.Replace("'", "");
                result = result.Replace("©", "");
                result = result.Replace("™", "");
                result = result.Replace("®", "");


            }
            return result;
        }

        // THIS IS USED FOR SLUGIFY NAME

        public static DataTable ToDataTable<T>(List<T> pItems)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in pItems)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static ExcelPackage CreateExcelPackage(DataTable dataTable, MemoryStream stream)
        {
            //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // Add headers
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName;
                }

                // Add data rows
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                    }
                }

                // Save the ExcelPackage to the stream
                excelPackage.SaveAs(stream);

                // Set the position of the stream to the beginning
                stream.Position = 0;

                return excelPackage;
            }
        }

        public static DataTable ToDataTable<T>(IList<T> pData)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in pData)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static string ProccessFolderName(string pMfgname)
        {
            return pMfgname.Replace(" ", "-").Replace(".", "").ToString();
        }

        public static string GeneratePassword(int length)
        {
            string result = string.Empty;

            System.Random r = new System.Random();
            r.Next();

            for (int i = 0; i < length; i++)
            {
                r.Next();
                if (i == 0)
                {
                    result += GetRandomPrintableLetter(r);
                }
                else
                {
                    result += GetRandomPrintableCharacter(r);
                }
            }

            return result;
        }

        private static string GetRandomPrintableCharacter(System.Random pRandom)
        {
            const string passwordCharacters = "abcdefghijkmnopqrstuvwxyz23456789";
            int location = pRandom.Next(passwordCharacters.Length - 1);
            return passwordCharacters.Substring(location, 1);
        }

        private static string GetRandomPrintableLetter(System.Random pRandom)
        {
            const string passwordCharacters = "abcdefghijkmnopqrstuvwxyz";
            int location = pRandom.Next(passwordCharacters.Length - 1);
            return passwordCharacters.Substring(location, 1);
        }

        public static string RandomString(int pLength)
        {
            //System.Random r = new System.Random();
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //return new string(Enumerable.Repeat(chars, length)
            //  .Select(s => s[r.Next(s.Length)]).ToArray());

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + small_alphabets + numbers;

            string otp = string.Empty;
            for (int i = 0; i < pLength; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

        public static string RandomOTP(int pLength)
        {
            //System.Random r = new System.Random();
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //return new string(Enumerable.Repeat(chars, length)
            //  .Select(s => s[r.Next(s.Length)]).ToArray());

            string numbers = "1234567890";

            string characters = numbers;

            string otp = string.Empty;
            for (int i = 0; i < pLength; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

        /// <summary>
        /// Genrate list of string to xml
        /// </summary>
        /// <param name="xmlRootName"> xmlRootName for root name</param>
        /// <param name="values">values for list of string</param>
        /// <returns></returns>
        /// Change gvtech(22-8-2014)
        public static string BuildXmlString(string pXmlRootName, List<string> pValues)
        {
            StringBuilder xmlString = new StringBuilder();

            xmlString.AppendFormat("<{0}>", pXmlRootName);
            foreach (var item in pValues)
            {
                xmlString.AppendFormat("<id>{0}</id>", item);
            }

            xmlString.AppendFormat("</{0}>", pXmlRootName);

            return xmlString.ToString();
        }

        /// <summary>
        /// Get a File Size in Bytes or kb or mb or gb with formatted string
        /// </summary>
        /// <param name="size"></param>
        /// <param name="formatString"></param>
        /// <returns></returns>
        public static string SizeFormat(long pSize, string pFormatString)
        {
            if (pSize < 1024)
            {
                return string.Format("{0} bytes", pSize.ToString(pFormatString));
            }

            if (pSize < Math.Pow(1024, 2))
            {
                return string.Format("{0} kb", (pSize / 1024).ToString(pFormatString));
            }

            if (pSize < Math.Pow(1024, 3))
            {
                return string.Format("{0} mb", (pSize / Math.Pow(1024, 2)).ToString(pFormatString));
            }

            if (pSize < Math.Pow(1024, 4))
            {
                return string.Format("{0} gb", (pSize / Math.Pow(1024, 3)).ToString(pFormatString));
            }

            return pSize.ToString(pFormatString);
        }

        public static string TimeAgo(DateTime pDateTime)
        {
            TimeSpan span = DateTime.Now - pDateTime;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("{0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("{0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("{0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("{0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("{0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        public static string ConvertDataTableToXML(DataTable pTable)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            pTable.WriteXml(memoryStream, true);
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            System.IO.StreamReader sr = new System.IO.StreamReader(memoryStream);
            return sr.ReadToEnd();
        }

        public static string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);

        public static string GetClientMAC(string strClientIP)
        {
            string mac_dest = "";
            try
            {
                Int32 ldest = inet_addr(strClientIP);
                Int32 lhost = inet_addr("");
                Int64 macinfo = new Int64();
                Int32 len = 6;
                int res = SendARP(ldest, 0, ref macinfo, ref len);
                string mac_src = macinfo.ToString("X");

                while (mac_src.Length < 12)
                {
                    mac_src = mac_src.Insert(0, "0");
                }

                for (int i = 0; i < 11; i++)
                {
                    if (0 == (i % 2))
                    {
                        if (i == 10)
                        {
                            mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                        else
                        {
                            mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("L?i " + err.Message);
            }
            return mac_dest;
        }

        //public static void ImageResize(string sourcePath, string destinationPath, string genratedFilename, string imageVersions)
        //{
        //    Dictionary<string, string> versions = new Dictionary<string, string>();
        //    //Define the versions to generate
        //    versions.Add(Constants.THUMBNAILIMAGERESIZER, "maxwidth=100&maxheight=100&crop=auto&format=jpg"); //Crop to square thumbnail
        //    versions.Add(Constants.LARGEIMAGERESIZER, "maxwidth=375&maxheight=500&format=jpg&mode=max&quality=50"); //Fit inside 1900x1200 area
        //    versions.Add(Constants.LARGEDININGIMAGERESIZER, "width=1140&height=374&crop=auto&format=jpg"); //Fit inside 1140x374 area
        //    versions.Add(Constants.MEDIUMIMAGERESIZER, "width=200&height=200&crop=auto&format=jpg");
        //    versions.Add(Constants.MEDIUMROOMIMAGERESIZER, "width=368&height=226&crop=auto&format=jpg"); //Fit inside 368x226 area
        //    versions.Add(Constants.MEDIUMEVENTIMAGERESIZER, "width=368&height=226&crop=auto&format=jpg"); //Fit inside 368x226 area
        //    versions.Add(Constants.SMALLIMAGERESIZER, "width=64&height=64&crop=auto&format=jpg"); //Fit inside 1900x1200 area
        //    versions.Add(Constants.SMALLHOMESLIDERIMAGERESIZER, "width=500&height=175&format=jpg"); //Fit inside 500x175 area
        //    versions.Add(Constants.MEDIUMPACKAGEIMAGERESIZER, "width=360&height=221&crop=auto&format=jpg"); //Fit inside 360x221 area

        //    var splitString = "";
        //    for (int i = 0; i < imageVersions.Split(',').Length; i++)
        //    {
        //        splitString = imageVersions.Split(',')[i];
        //        if (splitString == Constants.THUMBNAILIMAGERESIZER)
        //        {
        //            FileInfo fiThumbnail = new FileInfo(sourcePath);
        //            FileStream fsThumbnail = fiThumbnail.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsThumbnail, destinationPath + Constants.THUMBNAILIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.THUMBNAILIMAGERESIZER]));
        //        }

        //        if (splitString == Constants.MEDIUMIMAGERESIZER)
        //        {
        //            FileInfo fiThumbnail = new FileInfo(sourcePath);
        //            FileStream fsThumbnail = fiThumbnail.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsThumbnail, destinationPath + Constants.MEDIUMIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.MEDIUMIMAGERESIZER]));
        //        }

        //        if (splitString == Constants.MEDIUMROOMIMAGERESIZER)
        //        {
        //            FileInfo fiThumbnail = new FileInfo(sourcePath);
        //            FileStream fsThumbnail = fiThumbnail.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsThumbnail, destinationPath + Constants.MEDIUMROOMIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.MEDIUMROOMIMAGERESIZER]));
        //        }

        //        if (splitString == Constants.MEDIUMEVENTIMAGERESIZER)
        //        {
        //            FileInfo fiThumbnail = new FileInfo(sourcePath);
        //            FileStream fsThumbnail = fiThumbnail.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsThumbnail, destinationPath + Constants.MEDIUMEVENTIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.MEDIUMEVENTIMAGERESIZER]));
        //        }

        //        if (splitString == Constants.LARGEDININGIMAGERESIZER)
        //        {
        //            FileInfo fiThumbnail = new FileInfo(sourcePath);
        //            FileStream fsThumbnail = fiThumbnail.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsThumbnail, destinationPath + Constants.LARGEDININGIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.LARGEDININGIMAGERESIZER]));
        //        }

        //        if (splitString == Constants.LARGEIMAGERESIZER)
        //        {
        //            FileInfo fiLarge = new FileInfo(sourcePath);
        //            FileStream fsLarge = fiLarge.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsLarge, destinationPath + "/" + Constants.LARGEIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.LARGEIMAGERESIZER]));
        //        }

        //        if (splitString == Constants.SMALLIMAGERESIZER)
        //        {
        //            FileInfo fiSmall = new FileInfo(sourcePath);
        //            FileStream fsSmall = fiSmall.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsSmall, destinationPath + Constants.SMALLIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.SMALLIMAGERESIZER]));
        //        }

        //        if (splitString == Constants.SMALLHOMESLIDERIMAGERESIZER)
        //        {
        //            FileInfo fiSmall = new FileInfo(sourcePath);
        //            FileStream fsSmall = fiSmall.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsSmall, destinationPath + "/" + Constants.SMALLHOMESLIDERIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.SMALLHOMESLIDERIMAGERESIZER]));
        //        }

        //        if (splitString == Constants.MEDIUMPACKAGEIMAGERESIZER)
        //        {
        //            FileInfo fiThumbnail = new FileInfo(sourcePath);
        //            FileStream fsThumbnail = fiThumbnail.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        //            ImageBuilder.Current.Build(fsThumbnail, destinationPath + Constants.MEDIUMPACKAGEIMAGERESIZER + genratedFilename, new ResizeSettings(versions[Constants.MEDIUMPACKAGEIMAGERESIZER]));
        //        }
        //    }
        //}
        //public static string CompressImage(string destPath, string genratedFilename, string fileName)
        //{
        //    string destinationPath = destPath;
        //    destPath += "/" + genratedFilename;
        //    string fileExtension = Path.GetExtension(fileName);
        //    string genratedNewFilename = Guid.NewGuid().ToString() + fileExtension;
        //    using (Bitmap bm = new Bitmap(destPath))
        //    {
        //        string physicalPath = destinationPath + "/" + genratedNewFilename;

        //        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        //        ImageCodecInfo ici = null;
        //        foreach (ImageCodecInfo codec in codecs)
        //        {
        //            if (codec.MimeType == "image/jpeg")
        //                ici = codec;
        //        }
        //        EncoderParameters ep = new EncoderParameters();
        //        ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)50);
        //        bm.Save(physicalPath, ici, ep);
        //    }
        //    File.Delete(destPath);
        //    return genratedNewFilename;
        //}

    }
}
