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
using System.Configuration;
using System.Reflection.PortableExecutable;
//using VirtualKeyboard.Wpf;
using System.ComponentModel;

namespace MDFusionLabHaute.DesktopSurface.Views
{
    /// <summary>
    /// Interaction logic for MachineProfile.xaml
    /// </summary>
    public partial class MachineProfile : Window
    {
        #region Declaration
        public static string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];
        public static string destinationFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MDFusionLabHaute");
        public static string machineDataFileName = destinationFile + "\\" + "machinedata.json";

        Container container = new Container();
        #endregion

        #region Constructor
        public MachineProfile()
        {
            InitializeComponent();

            //VKeyboard.Listen<TextBox>(e => e.Text);
            //VKeyboard.Listen<PasswordBox>(e => e.Password);

            WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Mica);
            WPFUI.Appearance.Accent.Apply(WPFUI.Appearance.Accent.GetColorizationColor(), WPFUI.Appearance.ThemeType.Dark);

            if (File.Exists(machineDataFileName))
            {
                string json = File.ReadAllText(machineDataFileName);
                Domain.Entities.Machine machineData = JsonConvert.DeserializeObject<Domain.Entities.Machine>(json);

                if (machineData != null)
                {
                    PublicFile.machine = machineData;
                    this.Close();
                    container.Show();
                }
            }

            txtMachineRegNo.Focus();
        }
        #endregion

        #region Events

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            TextBox txtMachineRegNoValues = (TextBox)txtMachineRegNo.Template.FindName("txtMachineRegNoValues", txtMachineRegNo);
            TextBox txtShopNameValues = (TextBox)txtShopName.Template.FindName("txtShopNameValues", txtShopName);
            TextBox txtShopAddressValues = (TextBox)txtShopAddress.Template.FindName("txtShopAddressValues", txtShopAddress);
            TextBox txtCityValues = (TextBox)txtCity.Template.FindName("txtCityValues", txtCity);
            TextBox txtStateValues = (TextBox)txtState.Template.FindName("txtStateValues", txtState);

            Loader.Visibility = Visibility.Visible;
            try
            {
                if (string.IsNullOrEmpty(txtMachineRegNoValues.Text))
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "ERROR!";
                    MySnackbar.Message = "PLEASE ENTER MACHINE REGISTRATION NUMBER";
                    MySnackbar.Show();
                }
                else if (string.IsNullOrEmpty(txtShopNameValues.Text))
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "ERROR!";
                    MySnackbar.Message = "PLEASE ENTER SHOP NAME";
                    MySnackbar.Show();
                }
                else if (string.IsNullOrEmpty(txtShopAddressValues.Text))
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "ERROR!";
                    MySnackbar.Message = "PLEASE ENTER SHOP ADDRESS";
                    MySnackbar.Show();
                }
                else if (string.IsNullOrEmpty(txtCityValues.Text))
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "ERROR!";
                    MySnackbar.Message = "PLEASE ENTER CITY";
                    MySnackbar.Show();
                }
                else if (string.IsNullOrEmpty(txtStateValues.Text))
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "ERROR!";
                    MySnackbar.Message = "PLEASE ENTER STATE";
                    MySnackbar.Show();
                }
                else
                {
                    Domain.Entities.Machine machine = new Domain.Entities.Machine();
                    machine.MachineRegNo = txtMachineRegNoValues.Text;
                    machine.ShopName = txtShopNameValues.Text;
                    machine.ShopAddress = txtShopAddressValues.Text;
                    machine.City = txtCityValues.Text;
                    machine.State = txtStateValues.Text;
                    machine.MacAddress = GetMacAddress();
                    await SaveMachineProfile(machine);
                }
            }
            catch (Exception)
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG!";
                MySnackbar.Show();
            }
            Loader.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Methods
        public async Task SaveMachineProfile(Domain.Entities.Machine machine)
        {
            ProfilePanel.Visibility = Visibility.Hidden;
            lblMachineProfile.Visibility = Visibility.Hidden;
            btnSubmit.Visibility = Visibility.Hidden;
            Loader.Visibility = Visibility.Visible;
            try
            {
                machine.ID = 0;
                string apiUrl = _apiURL + "Machine/SaveMachineProfile";
                string inputJson = (new JavaScriptSerializer()).Serialize(machine);
                using (var wc = new WebClient())
                {
                    wc.Headers["Content-type"] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                    ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                    if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                    {
                        if (!Directory.Exists(destinationFile))
                        {
                            Directory.CreateDirectory(destinationFile);
                        }

                        if (!File.Exists(machineDataFileName))
                        {
                            File.Create(machineDataFileName).Close();
                        }

                        machine.ID = responseObjectForAnything.ResultObjectID;
                        string jsonData = JsonConvert.SerializeObject(machine);
                        File.WriteAllText(machineDataFileName, jsonData);
                        PublicFile.machine = machine;
                        Loader.Visibility = Visibility.Hidden;

                        Container container = new Container();
                        container.Show();
                        this.Hide();
                    }
                    else
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "INCORRECT!";
                        MySnackbar.Message = "SOMETHING WENT WRONG!";
                        MySnackbar.Show();
                        Loader.Visibility = Visibility.Hidden;
                        ProfilePanel.Visibility = Visibility.Visible;
                        lblMachineProfile.Visibility = Visibility.Visible;
                        btnSubmit.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = ex.Message;
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

        private void txtMachineRegNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TextBox textBoxvalues = (TextBox)textBox.Template.FindName("txtMachineRegNoValues", textBox);
            TextBlock placeholder = (TextBlock)textBox.Template.FindName("txtMachineRegNoPlaceholder", textBox);

            if (placeholder != null)
            {
                if (!string.IsNullOrEmpty(textBoxvalues.Text))
                {
                    placeholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    placeholder.Visibility = Visibility.Visible;
                }
            }
        }

        private void txtShopName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TextBox textBoxvalues = (TextBox)textBox.Template.FindName("txtShopNameValues", textBox);
            TextBlock placeholder = (TextBlock)textBox.Template.FindName("txtShopNamePlaceholder", textBox);

            if (placeholder != null)
            {
                if (!string.IsNullOrEmpty(textBoxvalues.Text))
                {
                    placeholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    placeholder.Visibility = Visibility.Visible;
                }
            }
        }

        private void txtShopAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TextBox textBoxvalues = (TextBox)textBox.Template.FindName("txtShopAddressValues", textBox);
            TextBlock placeholder = (TextBlock)textBox.Template.FindName("txtShopAddressPlaceholder", textBox);

            if (placeholder != null)
            {
                if (!string.IsNullOrEmpty(textBoxvalues.Text))
                {
                    placeholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    placeholder.Visibility = Visibility.Visible;
                }
            }
        }

        private void txtCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TextBox textBoxvalues = (TextBox)textBox.Template.FindName("txtCityValues", textBox);
            TextBlock placeholder = (TextBlock)textBox.Template.FindName("txtCityPlaceholder", textBox);

            if (placeholder != null)
            {
                if (!string.IsNullOrEmpty(textBoxvalues.Text))
                {
                    placeholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    placeholder.Visibility = Visibility.Visible;
                }
            }
        }

        private void txtState_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TextBox textBoxvalues = (TextBox)textBox.Template.FindName("txtStateValues", textBox);
            TextBlock placeholder = (TextBlock)textBox.Template.FindName("txtStatePlaceholder", textBox);

            if (placeholder != null)
            {
                if (!string.IsNullOrEmpty(textBoxvalues.Text))
                {
                    placeholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    placeholder.Visibility = Visibility.Visible;
                }
            }
        }


    }
}
