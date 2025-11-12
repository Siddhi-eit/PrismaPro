using MDFusionLabHaute.Common;
using MDFusionLabHaute.DesktopSurface.LanguageResources;
using MDFusionLabHaute.DesktopSurface.Processes;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Nancy;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using WPFLocalizeExtension.Providers;
using ZXing;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.IO;
using System.Reflection;
using static iTextSharp.text.pdf.AcroFields;
using MDFusionLabHaute.DesktopSurface.Helper;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using static System.Net.WebRequestMethods;
using System.Configuration;
using iTextSharp.text.pdf.parser;
using Caliburn.Micro;
using System.Windows.Threading;
using System.Drawing;
using System.Drawing.Printing;
using System.Management;
using Spire.Pdf;
using System.Timers;
using WPFUI.Controls;
using MDFusionLabHaute.DesktopSurface.Views.UserControls;
using System.Windows.Automation;
using iText.Kernel.Geom;

namespace MDFusionLabHaute.DesktopSurface.Views.Pages
{
    /// <summary>
    /// Interaction logic for CustomDispense.xaml
    /// </summary>
    public partial class CustomDispense : Page
    {

        #region Declaration

        public static string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];
        public static int _step = 0;
        public static int _consultantIdMaxLimit = 4;
        public static int _tailoringCodeMaxLimit = 6;
        public static int _selectedEssence = 6;
        public static bool _isEssenceSelected = false;
        public static bool _isLabelingLanguageSelected = false;
        public static bool _isPrintSelected = false;
        public static bool _isMenuVisible = false;
        public static bool _isDispenseSelected = false;
        public static string _selectedLanguageResource = "ENGLanguageResource";
        public static int _ENGRAVINGLINE1MaxLimit = 7;
        public static int _ENGRAVINGLINE2MaxLimit = 7;
        public static int _DERMAPROFILETMMaxLimit = 7;
        private const int WmSyscommand = 0x0112;
        private const int ScMonitorpower = 0xF170;
        private const int HwndBroadcast = 0xFFFF;
        private const int ShutOffDisplay = 2;
        public double _dispenseAmount = 0;
        public string _ProductCode = "";
        public string _TailoringCode = "";
        public string _ComponentNames = "";
        public string _Date = "";
        public string _BatchLOTNo = "";
        public string _DispensationsNumber = "";

        private int _numValue = 0;
        private int _dispenseQuantity = 0;
        private string _essence = "";
        private int _amountMode = 0;  // 0 = default , 1 = Manual

        private static CustomDispense _instance;
        public static ProductWithFormulaViewModel? _productWithFormulaViewModel;
        BackgroundWorker _backgroundWorker = new BackgroundWorker();

        public static CustomDispense Instance
        {
            get { return _instance ?? (_instance = new CustomDispense()); }
        }
        private DispatcherTimer timer;

        #endregion

        #region Constructor

        public CustomDispense()
        {
            InitializeComponent();
           
            SetContents();
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _backgroundWorker.WorkerReportsProgress = true;

            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                Loader.Visibility = Visibility.Hidden;  
                BindCustomerModeData();
                
                var FirstName = PublicFile.users.FirstName;
                var LastName = PublicFile.users.LastName;
                //lblUserName.Content = FirstName + " " + LastName;

                imgFooterActions.Visibility = Visibility.Visible;
                btnENG.Visibility = Visibility.Visible;
                btnESP.Visibility = Visibility.Visible;
            }));

            // Initialize the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10); // Set the timeout duration (5 seconds in this example)
            timer.Tick += Timer_Tick; // Specify the event handler for when the timeout is reached

        }



        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_MINIMIZE = 6;

        #endregion 

        #region Events

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isMenuVisible)
                {
                    //Hide
                    btnLock.Visibility = Visibility.Hidden;
                    btnCardActionScanner.Visibility = Visibility.Hidden;
                    btnBack.Visibility = Visibility.Hidden;
                    btnForward.Visibility = Visibility.Hidden;
                    btnExit.Visibility = Visibility.Hidden;
                    _isMenuVisible = false;
                }
                else
                {
                    //Show
                    btnLock.Visibility = Visibility.Visible;
                    btnCardActionScanner.Visibility = Visibility.Visible;
                    btnBack.Visibility = Visibility.Visible;
                    btnForward.Visibility = Visibility.Visible;
                    btnExit.Visibility = Visibility.Visible;
                    _isMenuVisible = true;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> btnMenu_Click()");
            }
        }

        [DllImport("user32.dll")]
        private static extern void LockWorkStation();

        private void BtnLock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LockWorkStation();
                //Hide
                btnLock.Visibility = Visibility.Hidden;
                btnCardActionScanner.Visibility = Visibility.Hidden;
                btnBack.Visibility = Visibility.Hidden;
                btnForward.Visibility = Visibility.Hidden;
                btnExit.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> btnLock_Click()");
            }
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            // Parse the text to an integer
            if (int.TryParse(txtCanisterQuantity.Text, out int quantity))
            {
                // If the quantity is less than 1, set it to 1
                if (quantity <= 1)
                {
                    txtCanisterQuantity.Text = "1";
                }
                else
                {
                    NumValue--;
                }
            }
            else
            {
                // If parsing fails, set the text to "1"
                txtCanisterQuantity.Text = "1";
            }

        }

        private void BtnCardActionScanner_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Scanner scanner = new Scanner();

                scanner.Loader.Visibility = Visibility.Visible;
                this.NavigationService.Navigate(scanner);
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> btnCardActionScanner_Click()");
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _step = 0;

                //Hide
                btnLock.Visibility = Visibility.Hidden;
                btnCardActionScanner.Visibility = Visibility.Hidden;
                btnBack.Visibility = Visibility.Hidden;
                btnForward.Visibility = Visibility.Hidden;
                btnExit.Visibility = Visibility.Hidden;

                ClearControls();
                Login login = new Login();
                this.NavigationService.Navigate(login);
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> btnExit_Click()");
            }
        }

        private void DdlCanSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "INVALID!";
                MySnackbar.Show();
            }
        }

        private void DdlAmount_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Amount_Submit();
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "INVALID!";
                MySnackbar.Message = "ENTER " + lblAmount.Content + "!";
                MySnackbar.Show();
            }
        }

        private void TxtAmount_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtAmount.Text.ToString() != "")
            {
                if (e.Key == Key.Enter)
                {
                    Amount();
                }
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "INVALID!";
                MySnackbar.Message = "ENTER " + lblAmount.Content + "!";
                MySnackbar.Show();
            }
        }

        private void TxtCanisterQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                {
                    GetDispenseDataProductionMode();
                }
                else
                {
                    NoOfQuantity();
                }
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "INVALID!";
                MySnackbar.Message = "ENTER " + lblNoOfQuantity.Content + "!";
                MySnackbar.Show();
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true;
            }
        }

        private void BtnError_Click(object sender, RoutedEventArgs e)
        {
            ChangeSteps(_step);
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (ddlAmount.SelectedValue != null)
            {
                if (ddlAmount.SelectedValue == "Manual")
                {
                    TxtAmount_Submit();
                }
                else
                {
                    Amount_Submit();
                }
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "INVALID!";
                MySnackbar.Message = "ENTER " + lblAmount.Content + "!";
                MySnackbar.Show();
            }
        }

        private void BtnENG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _selectedLanguageResource = "ENGLanguageResource";
                imgPause.Visibility = Visibility.Visible;
                imgDispense.Visibility = Visibility.Visible;
                imgResume.Visibility = Visibility.Visible;
                
                imgPausa.Visibility = Visibility.Collapsed;
                imgDispensar.Visibility = Visibility.Collapsed;
                imgReiniciar.Visibility = Visibility.Collapsed;

                TextBlock txtModeProduction = FindVisualChild<TextBlock>(btnModeProduction);
                if (txtModeProduction != null)
                {
                    txtModeProduction.Text = "PRODUCTION";
                }
                TextBlock lblModeCustomer = FindVisualChild<TextBlock>(btnModeCustomer);
                if (lblModeCustomer != null)
                {
                    lblModeCustomer.Text = "CUSTOMER";
                }
                ChangeSteps(_step + 1);
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> btnENG_Click()");
            }
        }

        private void BtnESP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _selectedLanguageResource = "ESPLanguageResource";
                
                imgPause.Visibility = Visibility.Collapsed;
                imgDispense.Visibility = Visibility.Collapsed;
                imgResume.Visibility = Visibility.Collapsed;
               
                imgPausa.Visibility = Visibility.Visible; 
                imgDispensar.Visibility = Visibility.Visible;
                imgReiniciar.Visibility = Visibility.Visible;

                TextBlock txtModeProduction = FindVisualChild<TextBlock>(btnModeProduction);
                if (txtModeProduction != null)
                {
                    txtModeProduction.Text = "PRODUCCIÓN";
                }
                TextBlock lblModeCustomer = FindVisualChild<TextBlock>(btnModeCustomer);
                if (lblModeCustomer != null)
                {
                    lblModeCustomer.Text = "CLIENTE";
                }
                ChangeSteps(_step + 1);
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> btnESP_Click()");
            }
        }

        private void BtnModeProduction_Click(object sender, RoutedEventArgs e)
        {
            //BindProductionModeData();

            PublicFile._ApplicationMode = Common.Common.ApplicationMode.PRODUCTION.GetHashCode();
            ChangeSteps(_step + 1);
        }

        private void BtnModeCustomer_Click(object sender, RoutedEventArgs e)
        {
            BindCustomerModeData();

            PublicFile._ApplicationMode = Common.Common.ApplicationMode.CUSTOMER.GetHashCode();
            ChangeSteps(_step + 1);
        }

        private void TxtConsultantID_KeyUp(object sender, KeyEventArgs e)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                if (e.Key == Key.Enter)
                {
                    ConsultantID_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtConsultantID_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtConsultantPassword_KeyUp(object sender, KeyEventArgs e)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txtConsultantPassword.Text.Length == _consultantIdMaxLimit)
                    {
                        ConsultantPassword();
                    }
                    else
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "INVALID!";
                        MySnackbar.Message = "ENTER VALID " + lblConsultantPassword.Content + "!";
                        MySnackbar.Show();
                        txtConsultantPassword.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtConsultantID_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtDermaprofile_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    txtDermaprofile.Visibility = Visibility.Hidden;
                    Dermaprofile_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtDermaprofile_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtReDermaprofile_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    ReDermaprofile_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtDermaprofile_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtTailoringCode_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    TailoringCode_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtTailoringCode_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtReTailoringCode_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    ReTailoringCode_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtReTailoringCode_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    FirstName_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtFirstName_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtLastName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    LastName_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtLastName_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtEngravingLine1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    EngravingLine1_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtEngravingLine1_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TxtEngravingLine2_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Next)
                {
                    TxtEngravingLine2_Submit();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtEngravingLine2_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void DdlAmount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ddlAmount.SelectedValue != null)
            {
                if (ddlAmount.SelectedValue == "Manual")
                {
                    ddlAmount.Visibility = Visibility.Hidden;
                    txtAmount.Visibility = Visibility.Visible;
                    lblAmountml.Visibility = Visibility.Visible;
                    btnEnter.Visibility = Visibility.Visible;
                    _amountMode = 1;
                }
                else
                {
                    var SelectAmount = ddlAmount.SelectedValue;
                    _dispenseAmount = Convert.ToDouble(ddlAmount.SelectedValue);
                    _amountMode = 0;
                }
            }
        }

        private void TxtCanisterQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCanisterQuantity == null)
            {
                return;
            }

            if (!int.TryParse(txtCanisterQuantity.Text, out _numValue))
                txtCanisterQuantity.Text = _numValue.ToString();

            _dispenseQuantity = Convert.ToInt32(txtCanisterQuantity.Text);
        }

        private void BtnEssence1_Click(object sender, RoutedEventArgs e)
        {
            if (txtTailoringCode.Text == txtReTailoringCode.Text)
            {
                _essence = string.IsNullOrEmpty(_productWithFormulaViewModel.Essence1_number_of_drops) ? "" : _productWithFormulaViewModel.Essence1_number_of_drops;
                lblBottomEssence.Content = _essence;
                _selectedEssence = 1;
                _isEssenceSelected = true;
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "!";
                MySnackbar.Show();

            }
        }

        private void BtnEssence2_Click(object sender, RoutedEventArgs e)
        {
            if (txtTailoringCode.Text == txtReTailoringCode.Text)
            {
                _essence = string.IsNullOrEmpty(_productWithFormulaViewModel.Essence2_number_of_drops) ? "" : _productWithFormulaViewModel.Essence2_number_of_drops;

                lblBottomEssence.Content = _essence;
                _selectedEssence = 2;
                _isEssenceSelected = true;
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "!";
                MySnackbar.Show();
            }
        }

        private void BtnEssence3_Click(object sender, RoutedEventArgs e)
        {
            if (txtTailoringCode.Text == txtReTailoringCode.Text)
            {
                _essence = string.IsNullOrEmpty(_productWithFormulaViewModel.Essence3_number_of_drops) ? "" : _productWithFormulaViewModel.Essence3_number_of_drops;
                lblBottomEssence.Content = _essence;
                _selectedEssence = 3;
                _isEssenceSelected = true;
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "!";
                MySnackbar.Show();
            }
        }

        private void BtnEssence4_Click(object sender, RoutedEventArgs e)
        {
            if (txtTailoringCode.Text == txtReTailoringCode.Text)
            {
                _essence = string.IsNullOrEmpty(_productWithFormulaViewModel.Essence4_number_of_drops) ? "" : _productWithFormulaViewModel.Essence4_number_of_drops;
                lblBottomEssence.Content = _essence;
                _selectedEssence = 4;
                _isEssenceSelected = true;
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "!";
                MySnackbar.Show();
            }
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDispense_Click(object sender, RoutedEventArgs e)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
                {
                Loader.Visibility = Visibility.Visible;
                DispenseNow();
            }
            catch (Exception)
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            foreach (Process Proc in Process.GetProcesses())
            {
                if (Proc.ProcessName.Equals("PRISMA~1"))  //Process Excel?
                    Proc.Kill();
            }
            ClearControls();
            CustomDispense customDispense = new CustomDispense();
            this.NavigationService.Navigate(customDispense);
        }

        private void BtnENG1_Click(object sender, RoutedEventArgs e)
        {
            ChangeSteps(_step + 1);
        }

        private void BtnESP1_Click(object sender, RoutedEventArgs e)
        {
            ChangeSteps(_step + 1);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            lblPrinting.Visibility = Visibility.Hidden;
            btnPrint.Visibility = Visibility.Hidden;
            Loader.Visibility = Visibility.Visible;
            if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
            {
                CreateProductionPDF();
            } else
            {
                CreateIDCardFileAsync();
            }
                

            btnMenu.Visibility = Visibility.Hidden;
            btnLock.Visibility = Visibility.Hidden;
            btnCardActionScanner.Visibility = Visibility.Hidden;
            btnBack.Visibility = Visibility.Hidden;
            btnForward.Visibility = Visibility.Hidden;
            btnExit.Visibility = Visibility.Hidden;
            //ChangeSteps(_step + 1);
        }
   
        private void DdlProductCode_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void BtnProductCodeEnter_Click(object sender, RoutedEventArgs e)
        {
            ProductCode_Submit();
        }

        private void BtnQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
            {
                GetDispenseDataProductionMode();
            }
            else
            {
                NoOfQuantity();
            }
        }

        private async void BtnEssenceEnter_ClickAsync(object sender, RoutedEventArgs e)
        {
            var result = await GetDispenseData(txtTailoringCode.Text.ToString(), txtDermaprofile.Text);
            List<ProductWithFormulaViewModel> productWithFormulaViewModelList = new List<ProductWithFormulaViewModel>();
            productWithFormulaViewModelList = JsonConvert.DeserializeObject<List<ProductWithFormulaViewModel>>(result.ResultObject.ToString());
            _productWithFormulaViewModel = productWithFormulaViewModelList[0];
            //essence = string.IsNullOrEmpty(_productWithFormulaViewModel.Essence1_number_of_drops) ? "" : _productWithFormulaViewModel.Essence1_number_of_drops;
            btnEssence1.IsEnabled = false;
            btnEssence2.IsEnabled = false;
            btnEssence3.IsEnabled = false;
            btnEssence4.IsEnabled = false;

            ChangeSteps(_step + 1);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_step == 0)
                {

                }
                else if (_step == 14)
                {
                    _selectedEssence = 6;
                    ChangeSteps(_step - 1);
                }
                else if (_step == 12)
                {
                    if (_amountMode == 1)
                    {
                        lblNoOfQuantity.Visibility = Visibility.Hidden;
                        txtCanisterQuantity.Visibility = Visibility.Hidden;
                        btnUp.Visibility = Visibility.Hidden;
                        btnDown.Visibility = Visibility.Hidden;
                        btnQuantity.Visibility = Visibility.Hidden;

                        ddlAmount.Visibility = Visibility.Hidden;
                        txtAmount.Visibility = Visibility.Visible;
                        lblAmountml.Visibility = Visibility.Visible;
                        btnEnter.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ChangeSteps(_step - 1);
                    }
                }
                else
                {
                    ChangeSteps(_step - 1);
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> btnBack_Click()");
            }
        }

        private void BtnForward_Click(object sender, RoutedEventArgs e)
        {
            var result = CheckPreviousStepCompleted(_step);
            if (result == true)
            {
                if (btnEssence1.Visibility == Visibility.Visible)
                {
                    if (_selectedEssence != 6)
                    {
                        ChangeSteps(_step + 1);
                    }
                    else
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "ERROR!";
                        MySnackbar.Message = "SELECT ESSENCE FIRST";
                        MySnackbar.Show();
                    }
                }
                else if (_step < 2)
                {
                    ChangeSteps(_step + 1);
                }
            }
        }

        private void TxtCanisterQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            // Parse the text to an integer
            if (int.TryParse(txtCanisterQuantity.Text, out int quantity))
            {
                // If the quantity is less than 1, set it to 1
                if (quantity < 1)
                {
                    txtCanisterQuantity.Text = "1";
                }
            }
            else
            {
                // If parsing fails, set the text to "1"
                txtCanisterQuantity.Text = "1";
            }
        }

        #region backgroundworker

        private void BackgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> backgroundWorker_ProgressChanged()");
            }
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                Loader.Visibility = Visibility.Hidden;
                //Hide
                lblEssence.Visibility = Visibility.Hidden;
                btnEssence1.Visibility = Visibility.Hidden;
                btnEssence2.Visibility = Visibility.Hidden;
                btnEssence3.Visibility = Visibility.Hidden;
                btnEssence4.Visibility = Visibility.Hidden;
                lblBottomEssence.Visibility = Visibility.Hidden;
                btnEssenceEnter.Visibility = Visibility.Hidden;
                //Show
                lblDispensing.Visibility = Visibility.Visible;
                lblDispensingResult.Visibility = Visibility.Visible;
                btnPause.Visibility = Visibility.Visible;
                btnDispense.Visibility = Visibility.Visible;
                btnRestart.Visibility = Visibility.Visible;
                imgCenterTop.Visibility = Visibility.Visible;
                imgCenterBottom.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> backgroundWorker_RunWorkerCompleted()");
            }
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string[][] param = (string[][])e.Argument;
                int myResult = 0;
                int myIntegerResult;

                // comment
                if (_dispenseQuantity > 1)
                {
                    myResult = MultipleDispense(param);
                }
                else
                {
                    Dispenser.FmDispense myDispenser = new Dispenser.FmDispense();
                    Dispenser.FmDispensedFormula myDispenserFM = new Dispenser.FmDispensedFormula();

                    myIntegerResult = myDispenser.Init();
                    myResult = myDispenser.DefineTask(Convert.ToDouble(param[0][0]), Convert.ToInt32(param[0][1]), 0, param[1], param[2]);

                    IntPtr hWnd = FindWindow(null, "Tareas");
                    if (hWnd != IntPtr.Zero)
                    {
                        ShowWindow(hWnd, SW_HIDE);
                    }
                    myResult = myDispenser.Dispense(true);

                    myDispenser.Shutdown();
                    foreach (Process Proc in Process.GetProcesses())
                    {
                        if (Proc.ProcessName.Equals("PRISMA~1"))  //Process Excel?
                            Proc.Kill();
                    }
                }

                string amount = "";
                for (int i = 0; i < param[2].Length; i++)
                {
                    decimal v = Convert.ToDecimal(param[2][i]);
                    if (amount == "")
                    {
                        amount = Convert.ToString(v * 1000);
                    }
                    else
                    {
                        amount = amount + '-' + Convert.ToString(v * 1000);
                    }
                }

                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    DispenseSuccess(myResult, Convert.ToDecimal(param[0][0]), Convert.ToInt32(param[0][1]), param[0][2], amount, param[0][4], param[0][5], param[0][6], _dispenseQuantity);

                    ChangeSteps(_step + 1);
                }));

                
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "ERROR!";
                    MySnackbar.Message = "SOMETHING WENT WRONG!";
                    MySnackbar.Show();
                }));
            }
        }

        private int MultipleDispense(string[][] param)
        {
            try
            {
                int myResult = 0;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    for (int i = 1; i <= _dispenseQuantity; i++)
                    {
                        
                        Dispenser.FmDispense myDispenser = new Dispenser.FmDispense();
                        Dispenser.FmDispensedFormula myDispenserFM = new Dispenser.FmDispensedFormula();

                        int myIntegerResult = myDispenser.Init();
                        myResult = myDispenser.DefineTask(Convert.ToDouble(param[0][0]), Convert.ToInt32(param[0][1]), 0, param[1], param[2]);

                        if (i != 1)
                        {
                            // Display the popup
                            PopupWindow popup = new PopupWindow();
                            popup.ShowDialog(); // Show the popup modally

                            // Check if the popup button was clicked to continue or if the dialog was closed
                            if (popup.DialogResult == true)
                            {
                                myResult = myDispenser.Dispense(true);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            myResult = myDispenser.Dispense(true);
                        }

                        myDispenser.Shutdown();
                        foreach (Process Proc in Process.GetProcesses())
                        {
                            if (Proc.ProcessName.Equals("PRISMA~1"))  //Process Excel?
                                Proc.Kill();
                        }
                    }
                });

                return myResult;
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> MultipleDispense()");
                return 0;
            }
        }


        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> backgroundWorker_ProgressChanged()");
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                Loader.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> backgroundWorker_RunWorkerCompleted()");
            }
        }

        #endregion

        #endregion

        #region Methods

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtCanisterQuantity.Text = value.ToString();
            }
        }

        private static bool IsNumeric(string text)
        {
            return int.TryParse(text, out int result);
        }

        public void SetContents()
        {
            lblConsultantID.Content = LocalizedString.GetLocalizedValue<string>("CONSULTANT ID", _selectedLanguageResource);
            lblConsultantPassword.Content = LocalizedString.GetLocalizedValue<string>("CONSULTANT PASSWORD", _selectedLanguageResource);
            lblTailoringCode.Content = LocalizedString.GetLocalizedValue<string>("TAILORING CODE", _selectedLanguageResource);
            lblReTailoringCode.Content = LocalizedString.GetLocalizedValue<string>("REPEAT TAILORING CODE", _selectedLanguageResource);
            lblEssence.Content = LocalizedString.GetLocalizedValue<string>("SELECT ESSENCE", _selectedLanguageResource);
            lblFirstName.Content = LocalizedString.GetLocalizedValue<string>("FIRST NAME", _selectedLanguageResource);
            lblLastName.Content = LocalizedString.GetLocalizedValue<string>("LAST NAME", _selectedLanguageResource);
            lblEngravingLine1.Content = LocalizedString.GetLocalizedValue<string>("ENGRAVING LINE 1", _selectedLanguageResource);
            lblEngravingLine2.Content = LocalizedString.GetLocalizedValue<string>("ENGRAVING LINE 2", _selectedLanguageResource);
            lblDermaprofile.Content = LocalizedString.GetLocalizedValue<string>("DERMAPROFILE", _selectedLanguageResource);
            lblAmount.Content = LocalizedString.GetLocalizedValue<string>("SELECT CAN SIZE", _selectedLanguageResource);
            lblNoOfQuantity.Content = LocalizedString.GetLocalizedValue<string>("SELECT NO OF QUANTITY", _selectedLanguageResource);
            lblDispensing.Content = LocalizedString.GetLocalizedValue<string>("DISPENSING...", _selectedLanguageResource);
            lblLabelingLanguage.Content = LocalizedString.GetLocalizedValue<string>("LABELING LANGUAGE", _selectedLanguageResource);
            lblPrinting.Content = LocalizedString.GetLocalizedValue<string>("PRINTING...", _selectedLanguageResource);
            lblSuccess.Content = LocalizedString.GetLocalizedValue<string>("PERSONALIZED SERUM CONCENTRATE CREATED", _selectedLanguageResource);
            lblBottomEssence.Content = LocalizedString.GetLocalizedValue<string>("ADD X DROP/S OF ESSENCE X", _selectedLanguageResource);
            lblbtnLock.Text = LocalizedString.GetLocalizedValue<string>("LOCK", _selectedLanguageResource);
            lblbtnCardActionScanner.Text = LocalizedString.GetLocalizedValue<string>("SCANNER", _selectedLanguageResource);
            lblbtnBack.Text = LocalizedString.GetLocalizedValue<string>("BACK", _selectedLanguageResource);
            lblbtnForward.Text = LocalizedString.GetLocalizedValue<string>("FORWARD", _selectedLanguageResource);
            lblbtnExit.Text = LocalizedString.GetLocalizedValue<string>("EXIT", _selectedLanguageResource);
            lblModeSelection.Content = LocalizedString.GetLocalizedValue<string>("MODE", _selectedLanguageResource);
            btnModeProduction.Content = LocalizedString.GetLocalizedValue<string>("PRODUCTION", _selectedLanguageResource);
            btnModeCustomer.Content = LocalizedString.GetLocalizedValue<string>("CUSTOMER", _selectedLanguageResource);
            //txtModeProduction.tex = LocalizedString.GetLocalizedValue<string>("PRODUCTION", _selectedLanguageResource);
            //lblModeCustomer.Content = LocalizedString.GetLocalizedValue<string>("CUSTOMER", _selectedLanguageResource);
            lblReDermaprofile.Content = LocalizedString.GetLocalizedValue<string>("REPEAT DERMAPROFILE tm", _selectedLanguageResource);
            lblCaneSize.Content = LocalizedString.GetLocalizedValue<string>("SELECT CAN SIZE", _selectedLanguageResource);
            lblAmountml.Content = LocalizedString.GetLocalizedValue<string>(".00 ML", _selectedLanguageResource);
            lblProductCode.Content = LocalizedString.GetLocalizedValue<string>("SELECT PRODUCT", _selectedLanguageResource);

        }

        public async void CheckConsultantID(string ConsultantID)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            string apiUrl = _apiURL + "Dispense/CheckConsultantID";
            String[] input = new String[2];
            input[0] = ConsultantID;
            input[1] = PublicFile.users.ID.ToString();
            string inputJson = (new JavaScriptSerializer()).Serialize(input);

            txtConsultantID.Visibility = Visibility.Hidden;
            Loader.Visibility = Visibility.Visible;

            using (var wc = new WebClient())
            {
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);
                responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result.ToString());

                if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    btnError.Visibility = Visibility.Visible;

                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER VALID " + lblConsultantID.Content + " OR USER ID!";

                    MySnackbar.Show();
                    txtConsultantID.Text = string.Empty;
                }
            }
            Loader.Visibility = Visibility.Hidden;
        }

        public async void CheckConsultantPassword(string ConsultantPassword)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            string apiUrl = _apiURL + "Dispense/CheckConsultantPassword";
            String[] input = new String[2];
            input[0] = ConsultantPassword;
            input[1] = PublicFile.users.ID.ToString();
            string inputJson = (new JavaScriptSerializer()).Serialize(input);

            txtConsultantID.Visibility = Visibility.Hidden;
            Loader.Visibility = Visibility.Visible;

            using (var wc = new WebClient())
            {
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);
                responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result.ToString());

                if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    btnError.Visibility = Visibility.Visible;

                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER VALID " + lblConsultantID.Content + " OR USER ID!";

                    MySnackbar.Show();
                    txtConsultantID.Text = string.Empty;
                }
            }
            Loader.Visibility = Visibility.Hidden;
        }

        //public async void CheckProductCode(string productCode)
        //{
        //    ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
        //    string apiUrl = _apiURL + "Dispense/CheckProductCode";
        //    String[] input = new String[2];
        //    input[0] = productCode;
        //    string inputJson = (new JavaScriptSerializer()).Serialize(input);

        //    txtConsultantID.Visibility = Visibility.Hidden;
        //    Loader.Visibility = Visibility.Visible;

        //    using (var wc = new WebClient())
        //    {
        //        wc.Headers["Content-type"] = "application/json";
        //        wc.Encoding = Encoding.UTF8;
        //        var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);
        //        responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result.ToString());

        //        if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
        //        {
        //            ChangeSteps(_step + 1);
        //        }
        //        else
        //        {
        //            btnError.Visibility = Visibility.Visible;

        //            MySnackbar.Timeout = 4000;
        //            MySnackbar.Title = "INVALID!";
        //            MySnackbar.Message = "ENTER VALID " + lblProductCode.Content + "!";

        //            MySnackbar.Show();
        //            txtConsultantID.Text = string.Empty;
        //        }
        //    }
        //    Loader.Visibility = Visibility.Hidden;
        //}

        public async Task<ResponseObjectForAnything> CheckTailoringCode(string TailoringCode)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            string apiUrl = _apiURL + "Dispense/CheckTailoringCode";
            String[] input = new String[2];
            input[0] = TailoringCode;
            input[1] = txtDermaprofile.Text;
            string inputJson = (new JavaScriptSerializer()).Serialize(input);

            txtTailoringCode.Visibility = Visibility.Hidden;
            Loader.Visibility = Visibility.Visible;

            using (var wc = new WebClient())
            {
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);
                responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);
                if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    imgCenterTop.Visibility = Visibility.Visible;
                    imgCenterBottom.Visibility = Visibility.Visible;
                    btnError.Visibility = Visibility.Visible;

                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER VALID " + lblTailoringCode.Content + "!";
                    MySnackbar.Show();
                    txtTailoringCode.Text = string.Empty;
                }
            }
            Loader.Visibility = Visibility.Hidden;
            return responseObjectForAnything;
        }

        public async Task<ResponseObjectForAnything> GetDispenseData(string tailoringCode, string dermaprofile)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            string apiUrl = _apiURL + "Dispense/GetDispenseData";
            String[] input = new String[4];
            input[0] = tailoringCode;
            input[1] = dermaprofile;
            input[2] = _dispenseAmount == 38 ? "2" : "1";
            input[3] = PublicFile.users.ID.ToString();

            string inputJson = (new JavaScriptSerializer()).Serialize(input);

            using (var wc = new WebClient())
            {
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);
            }

            return responseObjectForAnything;
        }

        public async Task<ResponseObjectForAnything> GetDispenseDataByProductCode(string productCode)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            string apiUrl = _apiURL + "Dispense/GetDispenseDataByProductCode";
            String[] input = new String[1];
            input[0] = productCode;

            string inputJson = (new JavaScriptSerializer()).Serialize(input);

            using (var wc = new WebClient())
            {
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);
            }

            return responseObjectForAnything;
        }

        public async void CheckDermaprofileData(string dermaprofile)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            string apiUrl = _apiURL + "Dispense/CheckDermaprofileData";
            String[] input = new String[2];
            input[0] = dermaprofile;
            input[1] = PublicFile.users.ID.ToString();
            string inputJson = (new JavaScriptSerializer()).Serialize(input);


            Loader.Visibility = Visibility.Visible;

            using (var wc = new WebClient())
            {
                wc.Headers["Content-type"] = "application/json";
                wc.Encoding = Encoding.UTF8;
                var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);
                responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result.ToString());

                if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                {
                    List<Dermaprofile> dermaprofile1List = new List<Dermaprofile>();
                    dermaprofile1List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dermaprofile>>(responseObjectForAnything.ResultObject.ToString());

                    ChangeSteps(_step + 1);
                }
                else
                {
                    imgCenterTop.Visibility = Visibility.Visible;
                    imgCenterBottom.Visibility = Visibility.Visible;
                    btnError.Visibility = Visibility.Visible;

                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER VALID " + lblDermaprofile.Content + "!";
                    MySnackbar.Show();
                    txtDermaprofile.Text = string.Empty;
                }
            }
            Loader.Visibility = Visibility.Hidden;
        }

        public async Task<ResponseObjectForAnything> SaveDispenseDetails(int dispenseUnit, decimal dispenseAmount, string colorCode, string amount, string productCode, string collection, string productName, int dispenseQuantity)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                
                string apiUrl = _apiURL + "Dispense/DispenseSuccess";
                string inputJson = "AmountToDispense=" + dispenseAmount + "&"
                    + "AmountToDispenseUnitID=" + 2 + "&"
                    + "MachinID=" + (int)PublicFile.machine.ID + "&"
                    + "ComponentUnitId=" + 1 + "&"
                    + "ComponentNames=" + colorCode + "&"
                    + "ComponentAmounts=" + amount + "&"
                    + "CreatedBy=" + PublicFile.users.ID + "&"
                    + "ProductCode=" + productCode + "&"
                    + "Collection=" + collection + "&"
                    + "ProductName=" + productName + "&"    
                    + "UserID=" + PublicFile._user.ID.ToString() + "&"
                    + "IsDispenseFromDesktop=" + 1 + "&"
                    + "MACHINE_REG_NO=" + _productWithFormulaViewModel.MDFusionLabNo + "&"
                    + "COUNTRY=" + "" + "&"
                    + "SHOP=" + "" + "&"
                    + "DATE=" + DateTime.Now + "&"
                    + "TIME_ID_ENTERED=" + DateTime.Now + "&"
                    + "CONSULTANT_ID=" + txtConsultantID.Text + "&"
                    + "DERMAPROFILE=" + txtDermaprofile.Text + "&"
                    + "TAILORING_CODE=" + txtTailoringCode.Text + "&"
                    + "ESSENCE=" + _selectedEssence + "&"
                    + "PRICE=" + _productWithFormulaViewModel.TotalFormulaPriceUSD + "&"
                    + "BACH_LOT_NO=" + (string.IsNullOrEmpty(_productWithFormulaViewModel.BachLotNo) ? "" : _productWithFormulaViewModel.BachLotNo + " ") + "&"
                    + "TIME_DISPENSED=" + DateTime.Now + "&"
                    + "IsDispense=" + true + "&"
                    + "dispenseQuantity=" + dispenseQuantity;



                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Encoding = Encoding.UTF8;
                    var result = wc.UploadString(apiUrl, inputJson);

                    responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                    if (responseObjectForAnything.ResultCode == Constants.RESPONSE_ERROR)
                    {
                        TextBlock CanisterAler = new TextBlock();
                        CanisterAler.Margin = new Thickness(5, 5, 5, 5);
                        CanisterAler.FontSize = 14;
                        CanisterAler.Foreground = System.Windows.Media.Brushes.White;
                        CanisterAler.Text = responseObjectForAnything.ResultMessage;
                        CanisterAler.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                }
            }
            catch (Exception ex)
            {
                //Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG!";
                MySnackbar.Show();
            }
            return responseObjectForAnything;
        }

        private ResponseObjectForAnything CreateProductionPDF()
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                string directoryPath = System.IO.Path.GetPathRoot(Environment.SystemDirectory) + "\\MdFusionLabPDF";
                string filePath = System.IO.Path.Combine(directoryPath, " ProductionMode - " + DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString().Replace(":", "-") + ".pdf");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                Document document = new Document();
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    document.Open();
                    document.Add(new iTextSharp.text.Paragraph("Tailoring Code - " + _TailoringCode));
                    document.Add(new iTextSharp.text.Paragraph("Component Names " + _ComponentNames));
                    document.Add(new iTextSharp.text.Paragraph("Date - " + _Date));
                    document.Add(new iTextSharp.text.Paragraph("Dispensations Number - " + _DispensationsNumber));
                    document.Add(new iTextSharp.text.Paragraph("LOT Number - " + _BatchLOTNo)); 
                    document.Close();
                }

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
                Loader.Visibility = Visibility.Hidden;
                lblSuccess.Visibility = Visibility.Visible;
                timer.Start();
            }
            catch (Exception ex)
            {
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return responseObjectForAnything;
        }



        private async Task<ResponseObjectForAnything> CreateIDCardFileAsync()
        {
            //ExceptationLog.ExceptionLog("Before creating resource path", "=> CreateIDCardFile()");

            string sSrcFile = string.Format("{0}Resources\\PDF\\Custom_Dispension_temp_colors.pdf", System.AppDomain.CurrentDomain.BaseDirectory);

            //ExceptationLog.ExceptionLog("After creating resource path", "=> CreateIDCardFile()");

            string destinationFile = System.IO.Path.GetPathRoot(Environment.SystemDirectory) + "\\MdFusionLabPDF";
            string pdfFileName = string.Format("{0}Resources\\PDF\\PROSPECT HN01.pdf", System.AppDomain.CurrentDomain.BaseDirectory);


            if (!Directory.Exists(destinationFile))
            {
                Directory.CreateDirectory(destinationFile);
            }
            destinationFile = destinationFile + "\\DispenseLabel - " + DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString().Replace(":", "-") + ".pdf";

            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;

            
            try
            {
                // create a new PDF reader based on the PDF template document
                pdfReader = new PdfReader(sSrcFile);
                pdfStamper = new PdfStamper(pdfReader, new FileStream(destinationFile, FileMode.Create));

                //ExceptationLog.ExceptionLog("Before creating resource path", "=> CreateIDCardFile()");

                AcroFields pdfFormFields = pdfStamper.AcroFields;
                iTextSharp.text.Font font = FontFactory.GetFont("COURIER", 8.0f, BaseColor.BLACK);

                pdfFormFields.SetField("txtName", (string.IsNullOrEmpty(txtFirstName.Text) ? "" : txtFirstName.Text) + " " + (string.IsNullOrEmpty(txtLastName.Text) ? "" : txtLastName.Text));
                pdfFormFields.SetField("txtMdFusionLabNo", string.IsNullOrEmpty(_productWithFormulaViewModel.MDFusionLabNo) ? "" : _productWithFormulaViewModel.MDFusionLabNo);
                pdfFormFields.SetField("txtBach_Lot_No1", string.IsNullOrEmpty(_BatchLOTNo) ? "" : _BatchLOTNo);
                pdfFormFields.SetField("txtBach_Lot_No2", string.IsNullOrEmpty(_BatchLOTNo) ? "" : _BatchLOTNo);
                pdfFormFields.SetField("txtBach_Lot_No3", string.IsNullOrEmpty(_BatchLOTNo) ? "" : _BatchLOTNo);
                pdfFormFields.SetField("txtTailoringCode", string.IsNullOrEmpty(txtTailoringCode.Text) ? "" : txtTailoringCode.Text);
                pdfFormFields.SetField("txtAddOns1", string.IsNullOrEmpty(_productWithFormulaViewModel.Addon_A) ? "" : _productWithFormulaViewModel.Addon_A);
                pdfFormFields.SetField("txtAddOns2", string.IsNullOrEmpty(_productWithFormulaViewModel.Addon_B) ? "" : _productWithFormulaViewModel.Addon_B);
                pdfFormFields.SetField("txtAddOns3", string.IsNullOrEmpty(_productWithFormulaViewModel.Addon_C) ? "" : _productWithFormulaViewModel.Addon_C);
                pdfFormFields.SetField("txtAddOns4", string.IsNullOrEmpty(_productWithFormulaViewModel.Addon_D) ? "" : _productWithFormulaViewModel.Addon_D);
                pdfFormFields.SetField("txtFirstName", string.IsNullOrEmpty(txtFirstName.Text) ? "" : txtFirstName.Text);
                pdfFormFields.SetField("txtLastName", string.IsNullOrEmpty(txtLastName.Text) ? "" : txtLastName.Text);
                pdfFormFields.SetField("txtEngravingLine1", string.IsNullOrEmpty(txtEngravingLine1.Text) ? "" : txtEngravingLine1.Text);
                pdfFormFields.SetField("txtEngravingLine2", string.IsNullOrEmpty(txtEngravingLine2.Text) ? "" : txtEngravingLine2.Text);
                pdfFormFields.SetField("txtDermaprofile", string.IsNullOrEmpty(txtDermaprofile.Text) ? "" : txtDermaprofile.Text);
                pdfFormFields.SetField("txtDermaprofile1", string.IsNullOrEmpty(txtDermaprofile.Text) ? "" : txtDermaprofile.Text);
                pdfFormFields.SetField("txtDermaprofile2", string.IsNullOrEmpty(txtDermaprofile.Text) ? "" : txtDermaprofile.Text);
                pdfFormFields.SetField("txtIngredients", string.IsNullOrEmpty(_productWithFormulaViewModel.Ingredients) ? "" : _productWithFormulaViewModel.Ingredients);
                //pdfFormFields.SetField("txtIngredients", "abc");


                //pdfFormFields.SetField("txtEssence1", _essence);
                pdfFormFields.SetField("txtEssence1", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence1_number_of_drops) ? "" : _productWithFormulaViewModel.Essence1_number_of_drops);
                if (_selectedEssence == 1)
                {
                    pdfFormFields.SetField("txtEssence1", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence1_number_of_drops) ? "" : _productWithFormulaViewModel.Essence1_number_of_drops);
                }
                else if (_selectedEssence == 2)
                {
                    pdfFormFields.SetField("txtEssence1", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence2_number_of_drops) ? "" : _productWithFormulaViewModel.Essence2_number_of_drops);
                }
                else if (_selectedEssence == 3)
                {
                    pdfFormFields.SetField("txtEssence1", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence3_number_of_drops) ? "" : _productWithFormulaViewModel.Essence3_number_of_drops);
                }
                else if (_selectedEssence == 4)
                {
                    pdfFormFields.SetField("txtEssence1", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence4_number_of_drops) ? "" : _productWithFormulaViewModel.Essence4_number_of_drops);
                }


                pdfFormFields.SetField("txtEssence4", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence1_number_of_drops) ? "" : _productWithFormulaViewModel.Essence1_number_of_drops);
                if (_selectedEssence == 1)
                {
                    pdfFormFields.SetField("txtEssence4", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence1_number_of_drops) ? "" : _productWithFormulaViewModel.Essence1_number_of_drops);
                }
                else if (_selectedEssence == 2)
                {
                    pdfFormFields.SetField("txtEssence4", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence2_number_of_drops) ? "" : _productWithFormulaViewModel.Essence2_number_of_drops);
                }
                else if (_selectedEssence == 3)
                {
                    pdfFormFields.SetField("txtEssence4", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence3_number_of_drops) ? "" : _productWithFormulaViewModel.Essence3_number_of_drops);
                }
                else if (_selectedEssence == 4)
                {
                    pdfFormFields.SetField("txtEssence4", string.IsNullOrEmpty(_productWithFormulaViewModel.Essence4_number_of_drops) ? "" : _productWithFormulaViewModel.Essence4_number_of_drops);
                }

                var outputStream = new MemoryStream();
                pdfStamper.SetPageAction(iTextSharp.text.pdf.PdfWriter.PAGE_OPEN, new iTextSharp.text.pdf.PdfAction(iTextSharp.text.pdf.PdfAction.PRINTDIALOG), 1);
                var writer = pdfStamper.Writer;
                writer.AddJavaScript(GetAutoPrintJs("A4", 100));

                if (pdfFormFields.Fields.Keys.Count != 0)
                {
                    pdfStamper.AcroFields.GenerateAppearances = true;
                    pdfStamper.FormFlattening = true;
                }

                pdfStamper.Close();
                var content = outputStream.ToArray();
                outputStream.Close();
                outputStream.Dispose();

                //outputStream.Close();
                //outputStream.Dispose();
                Process.Start(System.IO.Path.GetPathRoot(Environment.SystemDirectory).Replace("\\\\", "\\") + "Program Files\\Internet Explorer\\iexplore.exe", destinationFile.Replace("\\\\", "\\"));
                //PrintPdf(destinationFile.Replace("\\\\", "\\"));

                await Task.Delay(2000);  

                string pdfFileName1 = string.Format("{0}Resources\\PDF\\PROSPECT HN01_perfect.pdf", System.AppDomain.CurrentDomain.BaseDirectory);
                PdfReader pdfReaderProspect = null;
                PdfStamper pdfStamperProspect = null;

                string destinationFileProspect = System.IO.Path.GetPathRoot(Environment.SystemDirectory) + "\\MdFusionLabPDF";
                if (!Directory.Exists(destinationFileProspect))
                {
                    Directory.CreateDirectory(destinationFileProspect);
                }
                destinationFileProspect = destinationFileProspect + "\\DispenseLabel - " + DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString().Replace(":", "-") + ".pdf";
                
                // create a new PDF reader based on the PDF template document
                pdfReaderProspect = new PdfReader(pdfFileName1);
                    pdfStamperProspect = new PdfStamper(pdfReaderProspect, new FileStream(destinationFileProspect, FileMode.Create));

                    AcroFields pdfFormFieldsProspect = pdfStamperProspect.AcroFields;
                    iTextSharp.text.Font fontProspect = FontFactory.GetFont("COURIER", 8.0f, BaseColor.BLACK);

                    pdfFormFieldsProspect.SetField("txtName", (string.IsNullOrEmpty(txtFirstName.Text) ? "" : txtFirstName.Text) + " " + (string.IsNullOrEmpty(txtLastName.Text) ? "" : txtLastName.Text));
                    pdfFormFieldsProspect.SetField("txtDermaprofile", string.IsNullOrEmpty(txtDermaprofile.Text) ? "" : txtDermaprofile.Text);

                    var outputStreamProspect = new MemoryStream();
                    pdfStamperProspect.SetPageAction(iTextSharp.text.pdf.PdfWriter.PAGE_OPEN, new iTextSharp.text.pdf.PdfAction(iTextSharp.text.pdf.PdfAction.PRINTDIALOG), 1);
                    var writerProspect = pdfStamperProspect.Writer;
                    writerProspect.AddJavaScript(GetAutoPrintJs("A4", 100));

                    if (pdfFormFieldsProspect.Fields.Keys.Count != 0)
                    {
                        pdfStamperProspect.AcroFields.GenerateAppearances = true;
                        pdfStamperProspect.FormFlattening = true;
                    }

                    pdfStamperProspect.Close();
                    var contentProspect = outputStreamProspect.ToArray();
                    outputStreamProspect.Close();
                    outputStreamProspect.Dispose();

                Process.Start(System.IO.Path.GetPathRoot(Environment.SystemDirectory).Replace("\\\\", "\\") + "Program Files\\Internet Explorer\\iexplore.exe", destinationFileProspect.Replace("\\\\", "\\"));
                //PrintPdf(pdfFileName1);

                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;

                Loader.Visibility = Visibility.Hidden;
                lblSuccess.Visibility = Visibility.Visible;

                timer.Start();
                /*  imgCenterTop.Visibility = Visibility.Visible;
                  imgCenterBottom.Visibility = Visibility.Visible;
                  lblSuccess.Visibility = Visibility.Visible;
                  btnENG.Visibility = Visibility.Visible;
                  btnESP.Visibility = Visibility.Visible;
                  imgFooterActions.Visibility = Visibility.Visible;*/

            }
            catch (Exception ex)
            {

                if (pdfStamper != null)
                {
                    pdfStamper.Close();
                }
                if (pdfReader != null)
                {
                    pdfReader.Close();
                }

                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }

                return responseObjectForAnything;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop(); // Stop the timer
            ClearControls();
            ChangeSteps(0);
        }

        protected static string GetAutoPrintJs(string pageSize, int scale)
        {
            var script = new StringBuilder();
            script.Append("var pp = getPrintParams();");
            script.Append("pp.interactive= pp.constants.interactionLevel.full;");
            script.Append("pp.NumCopies=eval(2);");
            script.Append("print(pp);");
            return string.Format(@" 
            var doc = this;
            var printParams = {{'pageSize':'{0}', 'scale': {1}, 'printDialog': true}};
            doc.print({{bUI: true, bSilent: false, bShrinkToFit: false}});", pageSize, scale);
        }

        private void ConsultantID()
        {
            try
            {
                CheckConsultantID(txtConsultantID.Text.ToString());
            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> ConsultantID()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void ConsultantPassword()
        {
            try
            {
                CheckConsultantPassword(txtConsultantPassword.Password.ToString());
            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> ConsultantPassword()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void ReDermaprofile()
        {
            try
            {
                if (txtDermaprofile.Text.ToString() == txtReDermaprofile.Text.ToString())
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    imgCenterTop.Visibility = Visibility.Visible;
                    imgCenterBottom.Visibility = Visibility.Visible;
                    btnError.Visibility = Visibility.Visible;

                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER VALID " + lblReDermaprofile.Content + " !";
                    MySnackbar.Show();
                    txtReDermaprofile.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> ReTailoringCode()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void TailoringCode()
        {
            try
            {
                var response = CheckTailoringCode(txtTailoringCode.Text.ToString());
            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> TailoringCode()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void ReTailoringCode()
        {
            try
            {
                if (txtTailoringCode.Text.ToString() == txtReTailoringCode.Text.ToString())
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    lblReTailoringCode.Visibility = Visibility.Hidden;
                    txtReTailoringCode.Visibility = Visibility.Hidden;
                    imgCenterTop.Visibility = Visibility.Visible;
                    imgCenterBottom.Visibility = Visibility.Visible;
                    btnError.Visibility = Visibility.Visible;

                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER VALID " + lblReTailoringCode.Content + " !";
                    MySnackbar.Show();
                    txtReTailoringCode.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> ReTailoringCode()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private async void CheckEssence()
        {
            if (txtTailoringCode.Text.ToString() == txtReTailoringCode.Text.ToString())
            {
                var result = await GetDispenseData(txtTailoringCode.Text.ToString(), txtDermaprofile.Text);

                List<ProductWithFormulaViewModel> productWithFormulaViewModelList = new List<ProductWithFormulaViewModel>();
                productWithFormulaViewModelList = JsonConvert.DeserializeObject<List<ProductWithFormulaViewModel>>(result.ResultObject.ToString());
                _productWithFormulaViewModel = productWithFormulaViewModelList[0];
            }

            btnEssence1.IsEnabled = true;
            btnEssence2.IsEnabled = true;
            btnEssence3.IsEnabled = true;
            btnEssence4.IsEnabled = true;


            if (String.IsNullOrWhiteSpace(_productWithFormulaViewModel.Essence1_number_of_drops) || _productWithFormulaViewModel.Essence1_number_of_drops == "null")
            {
                btnEssence1.IsEnabled = false;
            }

            if (String.IsNullOrWhiteSpace(_productWithFormulaViewModel.Essence2_number_of_drops) || _productWithFormulaViewModel.Essence2_number_of_drops == "null")
            {
                btnEssence2.IsEnabled = false;
            }

            if (String.IsNullOrWhiteSpace(_productWithFormulaViewModel.Essence3_number_of_drops) || _productWithFormulaViewModel.Essence3_number_of_drops == "null")
            {
                btnEssence3.IsEnabled = false;
            }

            if (String.IsNullOrWhiteSpace(_productWithFormulaViewModel.Essence4_number_of_drops) || _productWithFormulaViewModel.Essence4_number_of_drops == "null")
            {
                btnEssence4.IsEnabled = false;
            }
        }

        private void FirstName()
        {
            try
            {
                if (txtFirstName.Text.ToString() != "")
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "ENTER CLIENTS " + lblFirstName.Content + "!";
                    MySnackbar.Show();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> FirstName()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void LastName()
        {
            try
            {
                if (txtLastName.Text.ToString() != "")
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "ENTER CLIENTS " + lblLastName.Content + "!";
                    MySnackbar.Show();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> LastName()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void EngravingLine1()
        {
            try
            {
                if (txtEngravingLine1.Text.ToString() != "")
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "ENTER CLIENTS CHOSEN " + lblEngravingLine1.Content + " (8 CHARACTERS MAXIMUM)!";
                    MySnackbar.Show();
                    txtEngravingLine1.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> EngravingLine1()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void EngravingLine2()
        {
            try
            {
                if (txtEngravingLine2.Text.ToString() != "")
                {
                    ChangeSteps(_step + 1);
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED!";
                    MySnackbar.Message = "ENTER CLIENTS CHOSEN " + lblEngravingLine2.Content + " (8 CHARACTERS MAXIMUM)!";
                    MySnackbar.Show();
                    txtEngravingLine2.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> EngravingLine2()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void CheckDermaprofile()
        {
            try
            {

                CheckDermaprofileData(txtDermaprofile.Text.ToString());
            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> ConsultantID()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void Amount()
        {
            try
            {
                if (ddlAmount.SelectedValue != null)
                {
                    if (ddlAmount.SelectedValue == "Manual")
                    {
                        ddlAmount.Visibility = Visibility.Hidden;
                        txtAmount.Visibility = Visibility.Hidden;
                        lblAmountml.Visibility = Visibility.Hidden;
                        btnEnter.Visibility = Visibility.Hidden;
                        _amountMode = 1;
                        ChangeSteps(_step + 1);

                    }
                    else
                    {
                        var SelectAmount = ddlAmount.SelectedValue;
                        _dispenseAmount = Convert.ToDouble(ddlAmount.SelectedValue);
                        ChangeSteps(_step + 1);
                        _amountMode = 0;
                    }
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER " + lblAmount.Content + "!";
                    MySnackbar.Show();
                }

                //ChangeSteps(_step + 1);
                //CheckEssence();
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> Amount()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        private void NoOfQuantity()
        {
            try
            {
                CheckEssence();
                ChangeSteps(_step + 1);
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> Amount()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
            }
        }

        public bool CheckPreviousStepCompleted(int step)
        {
            bool result = true;
            try
            {
                if (step == 1)
                {
                    if (PublicFile._ApplicationMode == null)
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "ERROR!";
                        MySnackbar.Message = "PLEASE SELECT ONE OPTION";
                        MySnackbar.Show();

                        result = false;
                    }
                }
                else if (step == 2)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        result = ProductCode_Submit();
                    }
                    else
                    {
                        result = ConsultantID_Submit();
                    }
                }
                else if (step == 3)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        result = CanisterQuantity_Submit();
                    }
                    else
                    {
                        result = Dermaprofile_Submit();
                    }
                }
                else if (step == 4)
                {
                    result = ReDermaprofile_Submit();
                }
                else if (step == 5)
                {
                    result = TailoringCode_Submit();
                }
                else if (step == 6)
                {
                    result = ReTailoringCode_Submit();
                }
                else if (step == 7)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        if (_amountMode == 0)
                        {
                            result = Amount_Submit();
                        }
                        else
                        {
                            result = TxtAmount_Submit();
                        }   
                    }
                    else if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        result = FirstName_Submit();
                    }
                }
                else if (step == 8)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        result = CanisterQuantity_Submit();
                    }
                    else if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        result = LastName_Submit();
                    }
                }
                else if (step == 9)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        if (_selectedEssence == 6)
                        {
                            MySnackbar.Timeout = 4000;
                            MySnackbar.Title = "ERROR!";
                            MySnackbar.Message = "SELECT ESSENCE FIRST";
                            MySnackbar.Show();

                            result = false;
                        }
                    }
                    else if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        result = EngravingLine1_Submit();
                    }
                }
                else if (step == 10)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "ERROR!";
                        MySnackbar.Message = "PLEASE SELECT ONE OPTION";
                        MySnackbar.Show();

                        result = false;
                    }
                    else if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        result = TxtEngravingLine2_Submit();
                    }
                }
                else if (step == 11)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "ERROR!";
                        MySnackbar.Message = "PLEASE SELECT ONE OPTION";
                        MySnackbar.Show();

                        result = false;
                    }
                    else if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        result = Amount_Submit();
                    }
                }
                else if (step == 12)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "ERROR!";
                        MySnackbar.Message = "PLEASE SELECT ONE OPTION";
                        MySnackbar.Show();

                        result = false;
                    }
                    else if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        result = CanisterQuantity_Submit();
                    }
                }
                else if (step == 13)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        if (_selectedEssence == 6)
                        {
                            MySnackbar.Timeout = 4000;
                            MySnackbar.Title = "ERROR!";
                            MySnackbar.Message = "SELECT ESSENCE FIRST";
                            MySnackbar.Show();

                            result = false;
                        }
                    }
                }
                else if (step == 14)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "ERROR!";
                        MySnackbar.Message = "PLEASE SELECT ONE OPTION";
                        MySnackbar.Show();

                        result = false;
                    }
                }
                else if (step == 15)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "ERROR!";
                        MySnackbar.Message = "PLEASE SELECT ONE OPTION";
                        MySnackbar.Show();

                        result = false;
                    }
                }
                else if (step == 16)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "ERROR!";
                        MySnackbar.Message = "PLEASE SELECT ONE OPTION";
                        MySnackbar.Show();

                        result = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async void BindCustomerModeData()
        {
            try
            {

                string apiUrl = _apiURL + "Dispense/GetDispenseFormulaData";
                using (var wc = new WebClient())
                {
                    wc.Headers["Content-type"] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    var result = wc.UploadString(apiUrl, "");

                    ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                    if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                    {

                        DispenseViewModel dispenseViewModel = new DispenseViewModel();
                        dispenseViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DispenseViewModel>(responseObjectForAnything.ResultObject.ToString());
                            
                        ProductFormulaList productFormulaList = new ProductFormulaList();
                        ddlProductCode.SelectedValuePath = "ProductCode";
                        ddlProductCode.DisplayMemberPath = "ProductCode";
                        productFormulaList.ProductCode = "--SELECT PRODUCT--";  
                        dispenseViewModel.productFormulaList.Insert(0, productFormulaList);
                        ddlProductCode.ItemsSource = dispenseViewModel.productFormulaList;

                        //ProductFormulaList productFormulaList = new ProductFormulaList();
                        //ddlProductCode.SelectedValuePath = "ProductCode";
                        //ddlProductCode.DisplayMemberPath = "ProductCode";
                        //productFormulaList.ProductCode = "--SELECT PRODUCT--";
                        //dispenseViewModel.productFormulaList.Insert(0, productFormulaList);
                        //ddlProductCode.ItemsSource = dispenseViewModel.productFormulaList;


                        CanSize canSize = new CanSize();
                        ddlAmount.SelectedValuePath = "Size";
                        ddlAmount.DisplayMemberPath = "DisplayText";
                        if(_selectedLanguageResource == "ENGLanguageResource")
                        {
                            canSize.DisplayText = "--SELECT AMOUNT--";
                        } else
                        {
                            canSize.DisplayText = "--SELECCIONE--";
                        }
                        dispenseViewModel.canSize.Insert(0, canSize);
                        ddlAmount.ItemsSource = dispenseViewModel.canSize;

                        CanSize Manual = new CanSize();
                        Manual.DisplayText = "Manual";
                        Manual.Size = "Manual";
                        dispenseViewModel.canSize.Insert(dispenseViewModel.canSize.Count, Manual);
                        ddlAmount.ItemsSource = dispenseViewModel.canSize;
                        ddlAmount.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG!";
                MySnackbar.Show();
            }
            Loader.Visibility = Visibility.Hidden;
        }

        public async void GetDispenseDataProductionMode()
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            responseObjectForAnything = await GetDispenseDataByProductCode(_ProductCode);

            List<ProductWithFormulaViewModel> productWithFormulaViewModelList = new List<ProductWithFormulaViewModel>();
            productWithFormulaViewModelList = JsonConvert.DeserializeObject<List<ProductWithFormulaViewModel>>(responseObjectForAnything.ResultObject.ToString());
            _productWithFormulaViewModel = productWithFormulaViewModelList[0];
            ChangeSteps(_step + 1);
        }

        private void ListBoxItem_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            double scrollMultiplier = 9.0; // Adjust this value to increase/decrease scroll "bigness"

            if (e.DeltaManipulation.Translation.Y != 0)
            {
                var scrollViewer = FindVisualChild<ScrollViewer>(ddlProductCode);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.DeltaManipulation.Translation.Y * scrollMultiplier);
                }
            }
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        #region NavigationMethods

        public void ChangeSteps(int NewStep)
        {
            try
            {
                //Hide
                HideControls();

                if (NewStep == 0)
                {
                    ShowFooterLanguageSelection();
                    _step = NewStep;
                }
                else if (NewStep == 1)
                {
                    imgCenterTop.Margin = new Thickness(imgCenterTop.Margin.Left, imgCenterTop.Margin.Top, imgCenterTop.Margin.Right, 170);
                    imgCenterBottom.Margin = new Thickness(imgCenterBottom.Margin.Left, 170, imgCenterBottom.Margin.Right, imgCenterBottom.Margin.Bottom);
                    ShowModeSelection();
                    SetContents();
                    _step = NewStep;
                }
                else if (NewStep == 2)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        ShowProductCodeSelection();
                        _step = NewStep;
                    }
                    else
                    {
                        ShowConsultantIDSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 3)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        imgCenterTop.Margin = new Thickness(imgCenterTop.Margin.Left, imgCenterTop.Margin.Top, imgCenterTop.Margin.Right, 170);
                        imgCenterBottom.Margin = new Thickness(imgCenterBottom.Margin.Left, 170, imgCenterBottom.Margin.Right, imgCenterBottom.Margin.Bottom);
                        ShowQuantitySelection();
                        _step = NewStep;
                    }
                    else
                    {
                        ShowDermaprofileSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 4)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                        ShowDispenseSelection();
                        _step = NewStep;
                    }
                    else
                    {
                        ShowReDermaprofileSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 5)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                    {
                       ShowPrintingSelection();
                    }
                    else
                    {
                        ShowTailoringCodeSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 6)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowReTailoringCodeSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 7)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowFirstNameSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 8)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowLastNameSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 9)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowEngravingLine1Selection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 10)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowEngravingLine2Selection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 11)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowAmountSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 12)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowQuantitySelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 13)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowEssenceSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 14)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowDispenseSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 15)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowLabelingLanguageSelection();
                        _step = NewStep;
                    }
                }
                else if (NewStep == 16)
                {
                    if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                    {
                        ShowPrintingSelection();
                        _step = NewStep;
                    }
                }
            }
            catch
            {

            }
        }

        public void HideControls()
        {
            Loader.Visibility = Visibility.Hidden;

            imgCenterTop.Visibility = Visibility.Hidden;
            imgCenterBottom.Visibility = Visibility.Hidden;

            lblModeSelection.Visibility = Visibility.Hidden;
            btnModeProduction.Visibility = Visibility.Hidden;
            btnModeCustomer.Visibility = Visibility.Hidden;

            lblProductCode.Visibility = Visibility.Hidden;
            ddlProductCode.Visibility = Visibility.Hidden;
            btnProductCodeEnter.Visibility = Visibility.Hidden;

            lblConsultantID.Visibility = Visibility.Hidden;
            txtConsultantID.Visibility = Visibility.Hidden;

            lblDermaprofile.Visibility = Visibility.Hidden;
            txtDermaprofile.Visibility = Visibility.Hidden;

            lblReDermaprofile.Visibility = Visibility.Hidden;
            txtReDermaprofile.Visibility = Visibility.Hidden;

            lblTailoringCode.Visibility = Visibility.Hidden;
            txtTailoringCode.Visibility = Visibility.Hidden;

            lblReTailoringCode.Visibility = Visibility.Hidden;
            txtReTailoringCode.Visibility = Visibility.Hidden;

            _isEssenceSelected = false;
            lblEssence.Visibility = Visibility.Hidden;
            btnEssence1.Visibility = Visibility.Hidden;
            btnEssence2.Visibility = Visibility.Hidden;
            btnEssence3.Visibility = Visibility.Hidden;
            btnEssence4.Visibility = Visibility.Hidden;
            lblBottomEssence.Visibility = Visibility.Hidden;
            btnEssenceEnter.Visibility = Visibility.Hidden;

            lblFirstName.Visibility = Visibility.Hidden;
            txtFirstName.Visibility = Visibility.Hidden;

            lblLastName.Visibility = Visibility.Hidden;
            txtLastName.Visibility = Visibility.Hidden;

            lblEngravingLine1.Visibility = Visibility.Hidden;
            txtEngravingLine1.Visibility = Visibility.Hidden;

            lblEngravingLine2.Visibility = Visibility.Hidden;
            txtEngravingLine2.Visibility = Visibility.Hidden;

            lblCaneSize.Visibility = Visibility.Hidden;
            ddlCanSize.Visibility = Visibility.Hidden;

            lblAmount.Visibility = Visibility.Hidden;
            ddlAmount.Visibility = Visibility.Hidden;
            btnEnter.Visibility = Visibility.Hidden;
            txtAmount.Visibility = Visibility.Hidden;
            lblAmountml.Visibility = Visibility.Hidden;

            lblNoOfQuantity.Visibility = Visibility.Hidden;
            txtCanisterQuantity.Visibility = Visibility.Hidden;
            btnUp.Visibility = Visibility.Hidden;
            btnDown.Visibility = Visibility.Hidden;
            btnQuantity.Visibility = Visibility.Hidden;

            lblDispensing.Visibility = Visibility.Hidden;
            lblDispensingResult.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Hidden;
            btnDispense.Visibility = Visibility.Hidden;
            btnRestart.Visibility = Visibility.Hidden;
            lblDispensingResult.Visibility = Visibility.Hidden;

            lblLabelingLanguage.Visibility = Visibility.Hidden;
            btnENG1.Visibility = Visibility.Hidden;
            btnESP1.Visibility = Visibility.Hidden;

            lblPrinting.Visibility = Visibility.Hidden;
            btnPrint.Visibility = Visibility.Hidden;

            lblSuccess.Visibility = Visibility.Hidden;
            btnError.Visibility = Visibility.Hidden;

            btnENG.Visibility = Visibility.Hidden;
            btnESP.Visibility = Visibility.Hidden;
            imgFooterActions.Visibility = Visibility.Hidden;

        }

        public void ClearControls()
        {
            ddlProductCode.SelectedIndex = 0;
            txtConsultantID.Text = null;
            txtTailoringCode.Text = null;
            txtReTailoringCode.Text = null;
            txtFirstName.Text = null;
            txtLastName.Text = null;
            txtEngravingLine1.Text = null;
            txtEngravingLine2.Text = null;
            txtDermaprofile.Text = null;
            txtReDermaprofile.Text = null;

            ddlCanSize.SelectedIndex = 0;
            ddlAmount.SelectedIndex = 0;
            txtAmount.Text = null;
            txtCanisterQuantity.Text = null;

            _step = 0;
            _consultantIdMaxLimit = 4;
            _tailoringCodeMaxLimit = 6;
            _selectedEssence = 6;
            _ENGRAVINGLINE1MaxLimit = 7;
            _ENGRAVINGLINE2MaxLimit = 7;
            _DERMAPROFILETMMaxLimit = 7;
            _isEssenceSelected = false;
            _isLabelingLanguageSelected = false;
            _isPrintSelected = false;
            _isMenuVisible = false;
            _isDispenseSelected = false;
            _productWithFormulaViewModel = new ProductWithFormulaViewModel();
            _selectedLanguageResource = "ENGLanguageResource";
        }

        #endregion

        #region ShowControlsMethods

        public void ShowFooterLanguageSelection()
        {
            //Show
            btnENG.Visibility = Visibility.Visible;
            btnESP.Visibility = Visibility.Visible;
            imgFooterActions.Visibility = Visibility.Visible;
        }

        public void ShowModeSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblModeSelection.Visibility = Visibility.Visible;
            btnModeCustomer.Visibility = Visibility.Visible;
            btnModeProduction.Visibility = Visibility.Visible;
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowConsultantIDSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblConsultantID.Visibility = Visibility.Visible;
            txtConsultantID.Visibility = Visibility.Visible;
            txtConsultantID.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowProductCodeSelection()
        {
            //Show
            imgCenterTop.Margin = new Thickness(imgCenterTop.Margin.Left, imgCenterTop.Margin.Top, imgCenterTop.Margin.Right, 230);
            imgCenterBottom.Margin = new Thickness(imgCenterBottom.Margin.Left, 230, imgCenterBottom.Margin.Right, imgCenterBottom.Margin.Bottom);
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblProductCode.Visibility = Visibility.Visible;
            ddlProductCode.Visibility = Visibility.Visible;
            btnProductCodeEnter.Visibility = Visibility.Visible;
            ddlProductCode.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowConsultantPasswordSelection()
        {
            ////Show
            //imgCenterTop.Visibility = Visibility.Visible;
            //imgCenterBottom.Visibility = Visibility.Visible;
            //lblConsultantPassword.Visibility = Visibility.Visible;
            //txtConsultantPassword.Visibility = Visibility.Visible;
            //btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowDermaprofileSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblDermaprofile.Visibility = Visibility.Visible;
            txtDermaprofile.Visibility = Visibility.Visible;
            txtDermaprofile.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowReDermaprofileSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblReDermaprofile.Visibility = Visibility.Visible;
            txtReDermaprofile.Visibility = Visibility.Visible;
            txtReDermaprofile.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowTailoringCodeSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblTailoringCode.Visibility = Visibility.Visible;
            txtTailoringCode.Visibility = Visibility.Visible;
            txtTailoringCode.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowReTailoringCodeSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblReTailoringCode.Visibility = Visibility.Visible;
            txtReTailoringCode.Visibility = Visibility.Visible;
            txtReTailoringCode.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowAmountSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblAmount.Visibility = Visibility.Visible;
            ddlAmount.Visibility = Visibility.Visible;
            ddlAmount.Focus();
            btnEnter.Visibility = Visibility.Visible;
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowManualAmountSelection()
        {
            if (ddlAmount.SelectedValue == "Manual")
            {
                //Show
                imgCenterTop.Visibility = Visibility.Visible;
                imgCenterBottom.Visibility = Visibility.Visible;
                ddlAmount.Visibility = Visibility.Hidden;
                txtAmount.Visibility = Visibility.Visible;
                lblAmountml.Visibility = Visibility.Visible;
                btnEnter.Visibility = Visibility.Visible;
                btnMenu.Visibility = Visibility.Visible;
            }
        }

        public void ShowFirstNameSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblFirstName.Visibility = Visibility.Visible;
            txtFirstName.Visibility = Visibility.Visible;
            txtFirstName.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowLastNameSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblLastName.Visibility = Visibility.Visible;
            txtLastName.Visibility = Visibility.Visible;
            txtLastName.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowQuantitySelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblNoOfQuantity.Visibility = Visibility.Visible;
            txtCanisterQuantity.Visibility = Visibility.Visible;
            btnQuantity.Visibility = Visibility.Visible;
            txtCanisterQuantity.Focus();
            btnUp.Visibility = Visibility.Visible;
            btnDown.Visibility = Visibility.Visible;
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowEssenceSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblEssence.Visibility = Visibility.Visible;
            lblBottomEssence.Visibility = Visibility.Visible;
            btnEssenceEnter.Visibility = Visibility.Visible;
            btnEssence1.Visibility = Visibility.Visible;
            btnEssence2.Visibility = Visibility.Visible;
            btnEssence3.Visibility = Visibility.Visible;
            btnEssence4.Visibility = Visibility.Visible;
            CheckEssence();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowDispenseSelection()
        {
            if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
            {
                lblDispensingResult.Content = "(" + txtDermaprofile.Text + ", " + txtTailoringCode.Text + ", " + (_amountMode == 0 ? _dispenseAmount : txtAmount.Text) + "ml, " + _dispenseQuantity + "QTY)";
                lblBottomEssence.Visibility = Visibility.Visible;
            }
            else
            {
                lblDispensingResult.Content = "(" + ddlProductCode.SelectedValue + ", " + (Convert.ToInt32(_productWithFormulaViewModel.dispenseAmount) - 3).ToString() + "ml, " + _dispenseQuantity + "QTY)";
            }
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblDispensing.Visibility = Visibility.Visible;
            lblDispensingResult.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Visible;
            btnDispense.Visibility = Visibility.Visible;
            btnRestart.Visibility = Visibility.Visible;
            lblBottomdispense.Visibility = Visibility.Visible;
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowEngravingLine1Selection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblEngravingLine1.Visibility = Visibility.Visible;
            txtEngravingLine1.Visibility = Visibility.Visible;
            txtEngravingLine1.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowEngravingLine2Selection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblEngravingLine2.Visibility = Visibility.Visible;
            txtEngravingLine2.Visibility = Visibility.Visible;
            txtEngravingLine2.Focus();
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowLabelingLanguageSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblLabelingLanguage.Visibility = Visibility.Visible;
            btnENG1.Visibility = Visibility.Visible;
            btnESP1.Visibility = Visibility.Visible;
            btnMenu.Visibility = Visibility.Visible;
        }

        public void ShowPrintingSelection()
        {
            //Show
            imgCenterTop.Visibility = Visibility.Visible;
            imgCenterBottom.Visibility = Visibility.Visible;
            lblPrinting.Visibility = Visibility.Visible;
            btnPrint.Visibility = Visibility.Visible;
            btnMenu.Visibility = Visibility.Visible;
        }

        #endregion

        #region ControlsSubmitMethods
        private bool ProductCode_Submit()
        {
            try
            {
                if (ddlProductCode.SelectedValue != null && ddlProductCode.SelectedValue != "--SELECT PRODUCT--")
                {
                    dynamic selectedItem = ddlProductCode.SelectedItem;
                    _ProductCode = ddlProductCode.SelectedValue.ToString();
                    _TailoringCode = _ProductCode;
                    _ComponentNames = selectedItem.ColorCode;
                    _DispensationsNumber = selectedItem.DispenseAmount;
                    ChangeSteps(_step + 1);
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER " + lblProductCode.Content + "!";
                    MySnackbar.Show();
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> ProductCode()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();
                return false;
            }
            return true;
        }




        public bool ConsultantID_Submit()
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                if (txtConsultantID.Text.Length == _consultantIdMaxLimit)
                {
                    ConsultantID();
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER VALID " + lblConsultantID.Content + " OR USER ID!";
                    MySnackbar.Show();
                    txtConsultantID.Text = string.Empty;

                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtConsultantID_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool Dermaprofile_Submit()
        {
            try
            {
                if (txtDermaprofile.Text.Length <= _DERMAPROFILETMMaxLimit)
                {

                    CheckDermaprofile();
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER CLIENTS " + lblDermaprofile.Content + " (7 CHARACTERS MAXIMUM)";
                    MySnackbar.Show();
                    txtDermaprofile.Text = string.Empty;

                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtDermaprofile_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool ReDermaprofile_Submit()
        {
            try
            {
                if (txtReDermaprofile.Text.Length <= _DERMAPROFILETMMaxLimit)
                {
                    ReDermaprofile();
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER CLIENTS " + lblDermaprofile.Content + " (7 CHARACTERS MAXIMUM)";
                    MySnackbar.Show();
                    txtReDermaprofile.Text = string.Empty;

                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtDermaprofile_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool TailoringCode_Submit()
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                if (txtTailoringCode.Text.ToString() != "")
                {
                    if (txtTailoringCode.Text.Length <= _tailoringCodeMaxLimit)
                    {
                        TailoringCode();
                    }
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER " + lblTailoringCode.Content + "!";
                    MySnackbar.Show();

                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtTailoringCode_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool ReTailoringCode_Submit()
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                if (txtReTailoringCode.Text.Length <= _tailoringCodeMaxLimit)
                {
                    ReTailoringCode();
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtReTailoringCode_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool FirstName_Submit()
        {
            try
            {
                FirstName();
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtFirstName_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool LastName_Submit()
        {
            try
            {
                LastName();
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtLastName_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool EngravingLine1_Submit()
        {
            try
            {
                if (txtEngravingLine1.Text.Length <= _ENGRAVINGLINE1MaxLimit)
                {
                    EngravingLine1();
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER CLIENTS CHOSEN " + lblEngravingLine1.Content + " (" + _ENGRAVINGLINE1MaxLimit + " CHARACTERS MAXIMUM)!";
                    MySnackbar.Show();
                    txtEngravingLine1.Text = string.Empty;

                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtEngravingLine1_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool TxtEngravingLine2_Submit()
        {
            try
            {
                if (txtEngravingLine2.Text.Length <= _ENGRAVINGLINE2MaxLimit)
                {
                    EngravingLine2();
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "INVALID!";
                    MySnackbar.Message = "ENTER CLIENTS CHOSEN " + lblEngravingLine2.Content + " (" + _ENGRAVINGLINE2MaxLimit + " CHARACTERS MAXIMUM)!";
                    MySnackbar.Show();
                    txtEngravingLine2.Text = string.Empty;

                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptationLog.ExceptionLog(ex.Message, this.ToString() + "=> txtEngravingLine2_KeyUp()");
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "Something went wrong!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool Amount_Submit()
        {
            try
            {
                Amount();
            }
            catch (Exception)
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "INVALID!";
                MySnackbar.Message = "ENTER " + lblAmount.Content + "!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool TxtAmount_Submit()
        {
            if (txtAmount.Text.ToString() != "")
            {
                Amount();
            }
            else
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "INVALID!";
                MySnackbar.Message = "ENTER " + lblAmount.Content + "!";
                MySnackbar.Show();

                return false;
            }
            return true;
        }

        public bool CanisterQuantity_Submit()
        {
            try
            {
                if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.PRODUCTION.GetHashCode())
                {
                    GetDispenseDataProductionMode();
                }
                else
                {
                    NoOfQuantity();
                }
            }
            catch (Exception)
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "INVALID!";
                MySnackbar.Message = "ENTER " + lblNoOfQuantity.Content + "!";
                MySnackbar.Show();

                return true;
            }

            return false;
        }

        #endregion

        #region DispenseMethods

        public void DispenseNow()
        {
            try
            {
                string id = _productWithFormulaViewModel.ID.ToString();
                string productCode = _productWithFormulaViewModel.ProductCode.ToString();
                string collection = _productWithFormulaViewModel.ColorCode.ToString();
                string productName = _productWithFormulaViewModel.ProductCode.ToString();
                string colorCode = _productWithFormulaViewModel.ColorCode.ToString();
                string amount = _productWithFormulaViewModel.Amount.ToString();

                var oldDispenseAmount = _productWithFormulaViewModel.dispenseAmount;
                var newDispenseAmount = "";
                char separator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                if (PublicFile._ApplicationMode == Common.Common.ApplicationMode.CUSTOMER.GetHashCode())
                {
                    var manualAMount = txtAmount.Text.ToString();
                    if (ddlAmount.SelectedValue == null)
                    {
                        newDispenseAmount = ddlAmount.Text.ToString();
                    }
                    else
                    {
                        if (ddlAmount.SelectedValue == "Manual" && manualAMount.Length > 0)
                        {
                            newDispenseAmount = manualAMount.ToString();
                            newDispenseAmount = newDispenseAmount + separator.ToString() + "00 ml";
                        }
                        else if (ddlAmount.SelectedValue == null || ddlAmount.SelectedValue == "--SELECT AMOUNT--")
                        {
                            newDispenseAmount = oldDispenseAmount.ToString();
                        }
                        else
                        {
                            newDispenseAmount = ddlAmount.SelectedValue.ToString();
                        }
                    }
                }
                else
                {
                    newDispenseAmount = oldDispenseAmount.ToString();
                }

                if (newDispenseAmount.Contains('.'))
                {
                    newDispenseAmount = newDispenseAmount.Replace('.', separator);
                    newDispenseAmount = newDispenseAmount.Substring(0, newDispenseAmount.IndexOf(separator.ToString()) + 2);
                }

                

                var colorCodeArray = colorCode.Split(',');
                var amountArray = amount.Split(',');

                for (int m = 0; m < amountArray.Length; m++)
                {
                    decimal value = Convert.ToDecimal(amountArray[m].Replace('.', Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)));
                    decimal newAmount = Convert.ToDecimal(newDispenseAmount.Replace('.', Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)));
                    var finalColorAmount = ((value * newAmount) / oldDispenseAmount);
                    amountArray[m] = (finalColorAmount / 1000).ToString();
                }
                amount = string.Join("-", amountArray);

                List<ColorFormulas> colorFormulaList = new List<ColorFormulas>();
                {
                    for (int f = 0; f < colorCodeArray.Length; f++)
                    {
                        ColorFormulas colorFormulas = new ColorFormulas { ColorCode = colorCodeArray[f].ToString().Trim(), Amount = amountArray[f].ToString().Trim() };
                        colorFormulaList.Add(colorFormulas);
                    }
                }

                string[] myComps;
                string[] myAmounts;
                myComps = new String[colorCodeArray.Length];
                myAmounts = new String[colorCodeArray.Length];
                int dispenseUnit = 0;
                var i = 0;
                foreach (var item in colorFormulaList)
                {
                    myComps[i] = item.ColorCode;
                    myAmounts[i] = item.Amount;
                    i++;
                }

                var dispenseAmount = (Convert.ToDecimal(newDispenseAmount.Replace('.', Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)))).ToString();

                dispenseAmount = (Convert.ToDecimal(dispenseAmount) / 1000).ToString();

                string[] otherdetails = new string[] { (dispenseAmount).ToString(), dispenseUnit.ToString(), colorCode, amount, productCode, collection, productName };
                string[][] param = new string[][] { otherdetails, myComps, myAmounts };

                HideControls();
                Loader.Visibility = Visibility.Visible;
                btnPause.Visibility = Visibility.Hidden;
                btnRestart.Visibility = Visibility.Hidden;
                btnDispense.Visibility = Visibility.Hidden;

                _backgroundWorker.RunWorkerAsync(param);

            }
            catch (Exception ex)
            {
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG!";
                MySnackbar.Show();
                foreach (Process Proc in Process.GetProcesses())
                {
                    if (Proc.ProcessName.Equals("PRISMA~1"))  //Process Excel?
                        Proc.Kill();
                }
            }
        }

        public void DispenseSuccess(int myResult1, decimal dispenseAmount, int dispenseUnit, string colorCode, string amount, string productCode, string collection, string productName, int dispenseQuantity) 
        {
            if (myResult1 == 0)
            {
                var resultdispense = SaveDispenseDetails(dispenseUnit, dispenseAmount, colorCode, amount, productCode, collection, productName, dispenseQuantity);
                if (resultdispense.Result.ResultCode == Constants.RESPONSE_SUCCESS)
                {
                    //Show
                    lblLabelingLanguage.Visibility = Visibility.Visible;
                    btnESP1.Visibility = Visibility.Visible;
                    btnENG1.Visibility = Visibility.Visible;
                    imgCenterTop.Visibility = Visibility.Visible;
                    imgCenterBottom.Visibility = Visibility.Visible;
                    _dispenseQuantity = 0;
                    dynamic resultObject = resultdispense.Result.ResultObject;
                    var lotNr = resultObject?.lotNr?.ToString();
                    if (lotNr != null)
                    {
                        _BatchLOTNo = lotNr;
                    }
                    _Date = resultObject?.date;
                    _DispensationsNumber = dispenseAmount.ToString();
                }
                else
                {
                    _dispenseQuantity = 0;
                }

                //Show
                lblLabelingLanguage.Visibility = Visibility.Visible;
                btnESP1.Visibility = Visibility.Visible;
                btnENG1.Visibility = Visibility.Visible;
                imgCenterTop.Visibility = Visibility.Visible;
                imgCenterBottom.Visibility = Visibility.Visible;
            }
            else
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG!";
                MySnackbar.Show();
            }
        }

        public void PrintPdf(string filePath)
        {
            //Load a PDF document
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument(filePath);

            //Print the document with the default printer 
            doc.Print();
        }

        #endregion

        #endregion

    }
}