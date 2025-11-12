using MDFusionLabHaute.Domain.ResponseObject;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfUIPickerLib;

namespace MDFusionLabHaute.DesktopSurface.Views
{
    /// <summary>
    /// Interaction logic for Refill_Bag_Tracking.xaml
    /// </summary>
    public partial class Refill_Bag_Tracking : Window
    {
        #region Declaration
        public static string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];
        BackgroundWorker _backgroundWorker = new BackgroundWorker();
        #endregion
        static int _stepNo = 0;
        #region Action
        public Refill_Bag_Tracking()
        {
            InitializeComponent();
            _stepNo = 0;
            txtUserName.Visibility = Visibility.Visible;
            lblTitle.Content = "ENTER USER ID";

            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.WorkerReportsProgress = true;
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                //Loader.Visibility = Visibility.Visible;
                //_backgroundWorker.RunWorkerAsync();
            }));
        }
        #endregion

        #region Events
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            _stepNo = 0;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (_stepNo == 0)
            {

            }
            else if (_stepNo == 1)
            {
                txtMdFusionLabNo.Visibility = Visibility.Hidden;
                lblTitle.Content = "ENTER USER ID";
                txtUserName.Visibility = Visibility.Visible;
                _stepNo = 0;
            }
            else if (_stepNo == 2)
            {

            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_stepNo == 0)
            {
                if (txtUserName.Password != null)
                {
                    txtUserName.Visibility = Visibility.Hidden;
                    lblTitle.Content = "MD FUSION LAB NO";
                    txtMdFusionLabNo.Visibility = Visibility.Visible;
                    _stepNo = 1;
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "PLEASE ENTER USERID";
                    MySnackbar.Show();
                }
            }
            else if (_stepNo == 1)
            {
                if (txtMdFusionLabNo.Text != null)
                {
                    txtMdFusionLabNo.Visibility = Visibility.Hidden;
                    lblTitle.Content = "CANISTER NO";
                    spnlCanisterNo.Visibility = Visibility.Visible;
                    _stepNo = 2;
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "PLEASE ENTER MD FUSION LAB NO";
                    MySnackbar.Show();
                }
            }
            else if (_stepNo == 2)
            {
                if (txtMdFusionLabNo.Text != null)
                {
                    spnlCanisterNo.Visibility = Visibility.Hidden;
                    lblTitle.Content = "NEXT SANITIZING REMINDER";
                    spnlSanitizingReminder.Visibility = Visibility.Visible;
                    lblSanitizingReminder.Visibility = Visibility.Visible;
                    _stepNo = 3;
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "PLEASE ENTER MD FUSION LAB NO";
                    MySnackbar.Show();
                }
            }
            else if (_stepNo == 3)
            {
                if (txtMdFusionLabNo.Text != null)
                {
                    spnlCanisterNo.Visibility = Visibility.Hidden;
                    lblTitle.Content = "PRODUCT";
                    spnlSanitizingReminder.Visibility = Visibility.Visible;
                    lblSanitizingReminder.Visibility = Visibility.Visible;
                    _stepNo = 4;
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "PLEASE ENTER MD FUSION LAB NO";
                    MySnackbar.Show();
                }
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            _stepNo = 0;
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
            BindDataAsync();
        }
        #endregion

        #region Methods
        public List<TumblerData> DateTimeTumblers
        {
            get
            {
                List<TumblerData> retVal = new List<TumblerData>();
                List<int> years = new List<int>();
                for (int i = 1990; i < 2012; ++i)
                    years.Add(i);

                //List<string> months = new List<string>();
                //for (int i = 1; i <= 12; ++i)
                //    months.Add(String.Format("{0:d2}", i));

                //List<string> days = new List<string>();
                //for (int i = 1; i <= 31; ++i)
                //    days.Add(String.Format("{0:d2}", i));

                //List<string> hours = new List<string>();
                //for (int i = 1; i <= 12; ++i)
                //    hours.Add(String.Format("{0:d2}", i));

                //List<string> min = new List<string>();
                //for (int i = 0; i < 60; ++i)
                //    min.Add(String.Format("{0:d2}", i));

                //retVal.Add(new TumblerData(months, 8, "/"));
                //retVal.Add(new TumblerData(days, 4, "/"));
                retVal.Add(new TumblerData(years, 12, ""));
                //retVal.Add(new TumblerData(hours, 10, ":"));
                //retVal.Add(new TumblerData(min, 0, ""));
                //retVal.Add(new TumblerData(new string[] { "AM", "PM" }.ToList<object>(), 0, ""));
                return retVal;
            }
        }

        public async Task BindDataAsync()
        {
            try
            {
                string apiUrl = _apiURL + "Account/GetDesktopDataByID";
                string inputJson = (new JavaScriptSerializer()).Serialize(PublicFile.users);
                using (var wc = new WebClient())
                {
                    wc.Headers["Content-type"] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                    ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                    if (responseObjectForAnything.ResultCode == MDFusionLabHaute.Common.Constants.RESPONSE_SUCCESS)
                    {
                        _ = Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {

                        }));
                    }
                }
            }
            catch
            {

            }
        }
        #endregion
    }
}
