using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MDFusionLabHaute.DesktopSurface.Views.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        #region Declaration
            private string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];
        #endregion

        #region Constructor
        public Login()
        {
            InitializeComponent();
            InitializeUI();
        }
        private void InitializeUI()
        {
            HideControls();
            ClearControls();

            btnExit.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Visible;
            lblEmail.Visibility = Visibility.Visible;
            txtEmailID.Visibility = Visibility.Visible;
            txtEmailID.Focus();

        }
        #endregion

        #region Events
        private void txtEmailID_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    btnNext_Click(sender, e);
                }
            }
            catch (Exception)
            {
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    btnSubmit_Click(sender, e);
                }
            }
            catch (Exception)
            {
            }
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPassword.Password))
                {
                    ShowSnackbar("REQUIRED!", "PLEASE ENTER PASSWORD.");
                    return;
                }

                imgCenterTop.Visibility = Visibility.Hidden;
                imgCenterBottom.Visibility = Visibility.Hidden;
                Loader.Visibility = Visibility.Visible;

                Users user = new Users
                {
                    Email = txtEmailID.Text,
                    Password = txtPassword.Password
                };

                lblPassword.Visibility = Visibility.Hidden;
                txtPassword.Visibility = Visibility.Hidden;

                await Task.Run(() => SignInApiCall(user));
            }
            catch (Exception ex)
            {
                HandleError();
            }
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEmailID.Text))
                {
                    ShowSnackbar("REQUIRED!", "PLEASE ENTER EMAIL.");
                    return;
                }

                if (!Helper.Helper.IsValidEmailAddress(txtEmailID.Text))
                {
                    ShowSnackbar("INVALID!", "PLEASE ENTER VALID EMAIL.");
                    return;
                }

                ToggleControlsVisibility();
            }
            catch (Exception ex)
            {
                HandleError();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ShowInitialControls();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Methods

        private async Task SignInApiCall(Users user)
        {
            try
            {
                string apiUrl = _apiURL + "Account/SignInWithEmailAndPasswordDesktop";
                user.MacAddress = await Task.Run(() => GetMacAddress());
                string inputJson = (new JavaScriptSerializer()).Serialize(user);

                using (var wc = new WebClient())
                {
                    wc.Headers["Content-type"] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                    ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                    Dispatcher.Invoke(() =>
                    {
                        if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                        {
                            PublicFile.users = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(responseObjectForAnything.ResultObject.ToString());
                            HandleSuccessfulSignIn();
                        }
                        else
                        {
                            ShowSnackbar("INCORRECT!", "INCORRECT USERNAME OR PASSWORD");
                            Loader.Visibility = Visibility.Hidden;
                            lblPassword.Visibility = Visibility.Visible;
                            txtPassword.Visibility = Visibility.Visible;
                            imgCenterTop.Visibility = Visibility.Visible;
                            imgCenterBottom.Visibility = Visibility.Visible;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => HandleError());
            }
        }

        private void HandleSuccessfulSignIn()
        {
            ShowInitialControls();
            Loader.Visibility = Visibility.Hidden;

            CustomDispense customDispense = new CustomDispense();
            customDispense.Loader.Visibility = Visibility.Visible;
            this.NavigationService.Navigate(customDispense);
        }

        private void ShowInitialControls()
        {
            lblEmail.Visibility = Visibility.Visible;
            txtEmailID.Visibility = Visibility.Visible;
            btnExit.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Visible;

            btnBack.Visibility = Visibility.Hidden;
            btnSubmit.Visibility = Visibility.Hidden;
            lblPassword.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Hidden;
        }

        private void ShowSnackbar(string title, string message)
        {
            MySnackbar.Timeout = 4000;
            MySnackbar.Title = title;
            MySnackbar.Message = message;
            MySnackbar.Show();
        }

        private void HandleError()
        {
            Loader.Visibility = Visibility.Hidden;
            ShowSnackbar("ERROR!", "SOMETHING WENT WRONG.");
        }

        private string GetMacAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            string macAddress = string.Empty;
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    macAddress = mo["MacAddress"].ToString();
                    break;
                }
            }

            return macAddress;
        }

        private void HideControls()
        {
            lblEmail.Visibility = Visibility.Hidden;
            txtEmailID.Visibility = Visibility.Hidden;
            lblPassword.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Hidden;
        }

        private void ClearControls()
        {
            txtEmailID.Text = string.Empty;
            txtPassword.Password = string.Empty;
        }

        private void ToggleControlsVisibility()
        {
            lblEmail.Visibility = Visibility.Hidden;
            txtEmailID.Visibility = Visibility.Hidden;
            btnExit.Visibility = Visibility.Hidden;
            btnNext.Visibility = Visibility.Hidden;

            btnBack.Visibility = Visibility.Visible;
            btnSubmit.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Focus();
        }

        #endregion
    }
}