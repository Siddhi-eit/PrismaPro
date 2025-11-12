using MaterialDesignThemes.Wpf;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUI.Controls;

namespace MDFusionLabHaute.DesktopSurface.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Window
    {
        #region Declaration
        public static string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];
        private static Container _instance;
        BackgroundWorker _backgroundWorker = new BackgroundWorker();
        public static Container Instance
        {
            get { return _instance ?? (_instance = new Container()); }
        }
        #endregion

        #region Constructor
        public Container()
        {
            InitializeComponent();

            WPFUI.Appearance.Background.Apply(
                this,
                WPFUI.Appearance.BackgroundType.Mica);
            WPFUI.Appearance.Accent.Apply(
                WPFUI.Appearance.Accent.GetColorizationColor(),
                WPFUI.Appearance.ThemeType.Dark);

            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.WorkerReportsProgress = true;
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                Loader.Visibility = Visibility.Visible;
                _backgroundWorker.RunWorkerAsync();
            }));

            Helper.SignalRConnectionHelper.Connect_To_Server();
        }
        #endregion

        #region Events
        private void NavigationItem_Click(object sender, RoutedEventArgs e)
        {
            Users user = new Users();
            PublicFile.users = user;
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void SanitizationDialog_OnButtonRightClick(object sender, RoutedEventArgs e)
        {
            Loader.Visibility = Visibility.Visible;
            //SetSanitizationDone();
            Loader.Visibility = Visibility.Hidden;
            SanitizationDialog.Hide();
        }

        private void SanitizationDialog_OnButtonLeftClick(object sender, RoutedEventArgs e)
        {
            SanitizationDialog.Hide();
        }

        private void RefllDialog_OnButtonRightClick(object sender, RoutedEventArgs e)
        {
            RefillDialog.Hide();
        }
        #endregion

        #region Methods
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
                            DesktopAlertViewModel desktopAlertViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DesktopAlertViewModel>(responseObjectForAnything.ResultObject.ToString());
                            foreach (var item in desktopAlertViewModel.refillAlertViewModelList)
                            {
                                StackPanel stackPanel = new StackPanel();
                                Label canister = new Label();
                                canister.Margin = new Thickness(5, 5, 5, 5);
                                canister.HorizontalAlignment = HorizontalAlignment.Stretch;
                                canister.VerticalAlignment = VerticalAlignment.Stretch;
                                canister.FontSize = 14;
                                if (item.CurrentAmount >= item.MinimumAmount)
                                {
                                    canister.Background = Brushes.Red;
                                }
                                canister.Content = "CANISTER -- " + item.CanisterCode.Replace("\r\n", "").Trim().ToUpper() + " -- " + item.Color.ToUpper() + "   :   " + (item.CurrentAmount * 1000).ToString().ToUpper() + "ml        -- REFILL REQUIRED!";
                                canister.HorizontalAlignment = HorizontalAlignment.Center;
                                canister.BorderBrush = Brushes.WhiteSmoke;
                                pnlRefillDialogText.Children.Add(canister);
                                RefillDialog.IsShown = true;
                            }
                            foreach (var item in desktopAlertViewModel.sanitizationAlertViewModelList)
                            {
                                Label sanitization = new Label();
                                sanitization.Margin = new Thickness(5, 5, 5, 5);
                                sanitization.HorizontalAlignment = HorizontalAlignment.Stretch;
                                sanitization.VerticalAlignment = VerticalAlignment.Stretch;
                                sanitization.FontSize = 14;
                                if (item.DateSanitised < DateTime.Now.AddDays(+1) && item.DateSanitised > DateTime.Now)
                                {
                                    sanitization.Background = Brushes.Red;
                                }
                                sanitization.Content = "CANISTER -- " + item.CanisterCode.Replace("\r\n", "").Trim().ToUpper() + " -- " + item.Color.ToUpper() + "   :   " + (item.CurrentAmount * 1000).ToString().ToUpper() + "ml      -- SANITIZATION REQUIRED!";
                                sanitization.HorizontalAlignment = HorizontalAlignment.Center;
                                sanitization.BorderBrush = Brushes.WhiteSmoke;
                                pnlSanitizationDialogText.Children.Add(sanitization);
                                SanitizationDialog.IsShown = true;
                            }

                            Loader.Visibility = Visibility.Hidden;
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG!";
                MySnackbar.Show();
            }
        }

        public async Task SetSanitizationDone()
        {
            try
            {
                string apiUrl = _apiURL + "SanitisingTraking/SetSanitizationDone";
                string inputJson = "UserId=" + PublicFile.users.ID;
                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Encoding = Encoding.UTF8;
                    var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                    ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result.ToString());

                    if (responseObjectForAnything.ResultCode == MDFusionLabHaute.Common.Constants.RESPONSE_SUCCESS)
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "SUCCEED!";
                        MySnackbar.Message = "DATA SUCCESSFULLY SAVED";
                        MySnackbar.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG!";
                MySnackbar.Show();
            }
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
            BindDataAsync();
        }
        #endregion
    }
}
