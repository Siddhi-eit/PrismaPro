using AForge.Video;
using AForge.Video.DirectShow;
using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.ResponseObject;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;
using ZXing.Windows.Compatibility;

namespace MDFusionLabHaute.DesktopSurface.Views.Pages
{
    /// <summary>
    /// Interaction logic for Scanner.xaml
    /// </summary>
    public partial class Scanner : Page
    {
        #region Declaration
        public static string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];
        FilterInfoCollection _filterInfoCollection;
        VideoCaptureDevice _videoCaptureDevice;
        System.Timers.Timer _timer;
        static bool _scanDone = false;
        public static int _stepNo = 0;
        public static BitmapImage _bitmapImage;
        #endregion

        #region Constructor
        public Scanner()
        {
            InitializeComponent();
            imgCenter.Visibility = Visibility.Visible;
            lblVideoDevices.Visibility = Visibility.Visible;
            ddlVideoDevices.Visibility = Visibility.Visible;

            _filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in _filterInfoCollection)
            {
                if (CustomDispense._selectedLanguageResource == "ESPLanguageResource")
                {
                    ddlVideoDevices.Items.Add("Cámara frontal de la tablet");
                    ddlVideoDevices.SelectedIndex = 0;
                }
                else 
                {
                    ddlVideoDevices.Items.Add(filterInfo.Name);
                    ddlVideoDevices.SelectedIndex = 0;
                }
            }
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                Loader.Visibility = Visibility.Hidden;
            }));
        }

        #endregion

        #region Events
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Loader.Visibility = Visibility.Visible;
            try
            {
                if (_stepNo == 0)
                {
                    CustomDispense customDispense = new CustomDispense();
                    this.NavigationService.Navigate(customDispense);
                }
                else if (_stepNo == 1)
                {
                    imgCenter.Visibility = Visibility.Visible;
                    lblVideoDevices.Visibility = Visibility.Visible;
                    ddlVideoDevices.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Visible;

                    imgCenterTop.Visibility = Visibility.Hidden;
                    imgCenterBtootm.Visibility = Visibility.Hidden;
                    lblScan.Visibility = Visibility.Hidden;
                    imgScanner.Visibility = Visibility.Hidden;
                    _stepNo = 0;
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
            Loader.Visibility = Visibility.Hidden;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Loader.Visibility = Visibility.Visible;
            try
            {
                if (_stepNo == 0)
                {
                    imgCenter.Visibility = Visibility.Hidden;
                    lblVideoDevices.Visibility = Visibility.Hidden;
                    ddlVideoDevices.Visibility = Visibility.Hidden;
                    btnNext.Visibility = Visibility.Hidden;

                    imgCenterTop.Visibility = Visibility.Visible;
                    imgCenterBtootm.Visibility = Visibility.Visible;
                    lblScan.Visibility = Visibility.Visible;
                    imgScanner.Visibility = Visibility.Visible;
                    _stepNo = 1;

                    //Start scanning
                    Loopy(1000);
                    startCaptureDevice(true);
                }
            }
            catch (Exception)
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "Error!";
                MySnackbar.Message = "WRONG QR CODE!";
                MySnackbar.Show();
            }
            Loader.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Methods
        public async void SaveScanData(string[] parameters)
        {
            try
            {
                var methodName = parameters[0];
                var id = parameters[1];
                String[] input = new String[3];
                input[0] = id;
                input[1] = PublicFile._user.ID.ToString();
                input[2] = PublicFile._machine.ID.ToString();

                if (methodName == "RefillTracking")
                {
                    string apiUrl = _apiURL + "RefillTracking/SetRefillDone";
                    string inputJson = (new JavaScriptSerializer()).Serialize(input);
                    using (var wc = new WebClient())
                    {
                        wc.Headers["Content-type"] = "application/json";
                        wc.Encoding = Encoding.UTF8;
                        var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                        ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                        if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS && responseObjectForAnything.ResultObjectID > 0)
                        {
                            Loader.Visibility = Visibility.Hidden;
                            MySnackbar.Timeout = 4000;
                            MySnackbar.Title = "SUCCEED!";
                            MySnackbar.Message = "DATA SUCCESSFULLY SAVED";
                            MySnackbar.Show();
                        }
                        else if (responseObjectForAnything.ResultCode == Constants.RESPONSE_EXISTS)
                        {
                            Loader.Visibility = Visibility.Hidden;
                            MySnackbar.Timeout = 4000;
                            MySnackbar.Title = "ALREADY USED!";
                            MySnackbar.Message = "QR CODE IS ALREADY USED";
                            MySnackbar.Show();
                        }
                        else if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS && responseObjectForAnything.ResultObjectID == 0)
                        {
                            Loader.Visibility = Visibility.Hidden;
                            MySnackbar.Timeout = 4000;
                            MySnackbar.Title = "ERROR!";
                            MySnackbar.Message = responseObjectForAnything.ResultMessage.ToUpper();
                            MySnackbar.Show();
                        }
                    }
                }
                else if (methodName == "SanitisationTracking")
                {
                    string apiUrl = _apiURL + "SanitisingTraking/SetSanitizationSuccess";
                    string inputJson = (new JavaScriptSerializer()).Serialize(input);
                    using (var wc = new WebClient())
                    {
                        wc.Headers["Content-type"] = "application/json";
                        wc.Encoding = Encoding.UTF8;
                        var result = await wc.UploadStringTaskAsync(apiUrl, inputJson);

                        ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                        if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS && responseObjectForAnything.ResultObjectID > 0)
                        {
                            Loader.Visibility = Visibility.Hidden;
                            MySnackbar.Timeout = 4000;
                            MySnackbar.Title = "SUCCEED!";
                            MySnackbar.Message = "DATA SUCCESSFULLY SAVED";
                            MySnackbar.Show();
                        }
                        else if (responseObjectForAnything.ResultCode == Constants.RESPONSE_EXISTS)
                        {
                            Loader.Visibility = Visibility.Hidden;
                            MySnackbar.Timeout = 4000;
                            MySnackbar.Title = "ALREADY USED!";
                            MySnackbar.Message = "QR CODE IS ALREADY USED";
                            MySnackbar.Show();
                        }
                        else if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS && responseObjectForAnything.ResultObjectID == 0)
                        {
                            Loader.Visibility = Visibility.Hidden;
                            MySnackbar.Timeout = 4000;
                            MySnackbar.Title = "ERROR!";
                            MySnackbar.Message = responseObjectForAnything.ResultMessage.ToUpper();
                            MySnackbar.Show();
                        }
                    }
                }
                else
                {
                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "Error!";
                    MySnackbar.Message = "WRONG QR CODE";
                    MySnackbar.Show();
                }
                _stepNo = 0;
            }
            catch (Exception ex)
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "Error!";
                MySnackbar.Message = "WRONG QR CODE";
                MySnackbar.Show();
                //_stepNo = 1;
            }
        }

        void Loopy(int times)
        {
            _timer = new System.Timers.Timer(times);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_bitmapImage != null)
                {
                    BarcodeReader barcodeReader = new BarcodeReader();
                    Result result = barcodeReader.Decode(_bitmapImage);
                    if (result != null)
                    {
                        Dispatcher.BeginInvoke(new ThreadStart(delegate
                        {
                            Loader.Visibility = Visibility.Visible;
                            _scanDone = true;
                            _timer.Stop();
                            _timer.Enabled = false;
                            startCaptureDevice(false);
                            var val = result.ToString().Split(',');
                            SaveScanData(val);
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

        void startCaptureDevice(bool capture)
        {
            try
            {
                if (capture)
                {
                    _videoCaptureDevice = new VideoCaptureDevice(_filterInfoCollection[ddlVideoDevices.SelectedIndex].MonikerString);
                    _videoCaptureDevice.NewFrame += CaputreDevice_NewFrame;
                    _videoCaptureDevice.Start();
                }
                else
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        _videoCaptureDevice.SignalToStop();
                        imgScanner.Source = null;
                        ButtonAutomationPeer peer = new ButtonAutomationPeer(btnBack);
                        IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                        invokeProv.Invoke();

                    }));
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

        private void CaputreDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                //BitmapImage bitmap = new BitmapImage((BitmapImage)eventArgs.Frame.Clone());
                //imgScanner.Source = bitmap;

                System.Drawing.Image imgforms = (Bitmap)eventArgs.Frame.Clone();

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();

                MemoryStream ms = new MemoryStream();
                imgforms.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                bi.StreamSource = ms;
                bi.EndInit();

                bi.Freeze();
                //Freezing the bitmapImage to avoid cross thread operation and then calling the UI thread using the Dispatcher to update the 'Image' WPF control called frameholder   

                Dispatcher.BeginInvoke(new ThreadStart(delegate { imgScanner.Source = bi; _bitmapImage = bi; }));
            }
            catch (Exception)
            {
                Loader.Visibility = Visibility.Hidden;
                MySnackbar.Timeout = 4000;
                MySnackbar.Title = "ERROR!";
                MySnackbar.Message = "SOMETHING WENT WRONG!";
                MySnackbar.Show();
            }
        }
        #endregion
    }
}