using MaterialDesignThemes.Wpf;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Common;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
using Constants = MDFusionLabHaute.Common.Constants;
using MDFusionLabHaute.DesktopSurface;
using MDFusionLabHaute.DesktopSurface.Views.Pages;
using MDFusionLabHaute.DesktopSurface.Helper;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Utilities.Net;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Net.Mail;
using Application = System.Windows.Application;
using System.Management;

namespace MDFusionLabHaute.DesktopSurface.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        #region Declaration
        public static string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];
        #endregion

        #region Constructor
        public Login()
        {
            InitializeComponent();

            WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Mica);
            WPFUI.Appearance.Accent.Apply(WPFUI.Appearance.Accent.GetColorizationColor(), WPFUI.Appearance.ThemeType.Dark);
            lblPassword.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Hidden;
            btnBack.Visibility = Visibility.Hidden;
            btnSubmit.Visibility = Visibility.Hidden;
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
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtPassword.Password))
                {
                    Loader.Visibility = Visibility.Visible;
                    Users user = new Users();
                    user.Email = txtEmailID.Text;
                    user.Password = txtPassword.Password;
                    _ = SignInApiCall(user);
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "PLEASE ENTER PASSWORD.";
                    MySnackbar.Show();
                }
            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG.";
                MySnackbar.Show();
            }
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtEmailID.Text))
                {
                    if (Helper.Helper.IsValidEmailAddress(txtEmailID.Text))
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
                    else
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "INVALID!";
                        MySnackbar.Message = "PLEASE ENTER VALID EMAIL";
                        MySnackbar.Show();
                    }
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "PLEASE ENTER EMAIL";
                    MySnackbar.Show();
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
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
        #endregion

        #region Methods
        public async Task SignInApiCall(Users user)
        {
            try
            {
                string apiUrl = _apiURL + "Account/SignInWithEmailAndPasswordDesktop";
                user.MacAddress = GetMacAddress();
                string inputJson = (new JavaScriptSerializer()).Serialize(user);
                using (var wc = new WebClient())
                {
                    wc.Headers["Content-type"] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                    ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                    if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                    {
                        PublicFile.users = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(responseObjectForAnything.ResultObject.ToString());

                        lblEmail.Visibility = Visibility.Visible;
                        txtEmailID.Visibility = Visibility.Visible;
                        btnExit.Visibility = Visibility.Visible;
                        btnNext.Visibility = Visibility.Visible;

                        btnBack.Visibility = Visibility.Hidden;
                        btnSubmit.Visibility = Visibility.Hidden;
                        lblPassword.Visibility = Visibility.Hidden;
                        txtPassword.Visibility = Visibility.Hidden;
                        Loader.Visibility = Visibility.Hidden;
                        //container.Loader.Visibility = Visibility.Visible;

                        //Log.Logger();

                        Container container = new Container();
                        this.Close();
                        container.Show();
                    }
                    else
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "INCORRECT!";
                        MySnackbar.Message = "INCORRECT USERNAME OR PASSWORD";
                        MySnackbar.Show();
                        Loader.Visibility = Visibility.Hidden;
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

        public string GetMacAddress()
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
        #endregion
    }
}
