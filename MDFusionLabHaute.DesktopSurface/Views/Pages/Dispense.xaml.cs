using Dispenser;
using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MDFusionLabHaute.DesktopSurface.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dispense.xaml
    /// </summary>
    public partial class Dispense : Page
    {

        #region Declaration
        public static string _apiURL = System.Configuration.ConfigurationSettings.AppSettings["APIURL"];
        public static int _stepNo = 0;
        BackgroundWorker _backgroundWorker = new BackgroundWorker();
        #endregion

        #region Constructor
        public Dispense()
        {
            InitializeComponent();

            _backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            _backgroundWorker.WorkerReportsProgress = true;
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                Loader.Visibility = Visibility.Hidden;
                BindData();
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
                    Dashboard dashboard = new Dashboard();
                    this.NavigationService.Navigate(dashboard);
                }
                else if (_stepNo == 1)
                {
                    imgCenter.Visibility = Visibility.Visible;
                    lblProductFormula.Visibility = Visibility.Visible;
                    //ddlProductCode.Visibility = Visibility.Visible;
                    //ddlProductCollection.Visibility = Visibility.Visible;
                    ddlProductName.Visibility = Visibility.Visible;

                    lblProductFormulaCollection.Visibility = Visibility.Hidden;
                    grdColorFormula.Visibility = Visibility.Hidden;
                    imgCenterBtootm.Visibility = Visibility.Hidden;
                    imgCenterTop.Visibility = Visibility.Hidden;
                    _stepNo = 0;
                }
                else if (_stepNo == 2)
                {
                    lblProductFormulaCollection.Visibility = Visibility.Visible;
                    grdColorFormula.Visibility = Visibility.Visible;
                    imgCenterBtootm.Visibility = Visibility.Visible;
                    imgCenterTop.Visibility = Visibility.Visible;

                    lblCaneSize.Visibility = Visibility.Hidden;
                    ddlCanSize.Visibility = Visibility.Hidden;
                    imgCenter.Visibility = Visibility.Hidden;
                    _stepNo = 1;
                }
                else if (_stepNo == 3)
                {
                    lblCaneSize.Visibility = Visibility.Visible;
                    ddlCanSize.Visibility = Visibility.Visible;
                    imgCenter.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Visible;

                    btnSubmit.Visibility = Visibility.Hidden;
                    lblDispenseUnit.Visibility = Visibility.Hidden;
                    ddlDispenseUnit.Visibility = Visibility.Hidden;
                    _stepNo = 2;
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

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ddlDispenseUnit.SelectedIndex != 0)
                {
                    Loader.Visibility = Visibility.Visible;
                    DispenseNow();
                }
                else
                {
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "REQUIRED";
                    MySnackbar.Message = "PLEASE SELECT DISPENSE UNIT";
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
                Loader.Visibility = Visibility.Hidden;
            }
            //Loader.Visibility = Visibility.Visible;
            //Loader.Visibility = Visibility.Hidden;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Loader.Visibility = Visibility.Visible;
            try
            {
                if (_stepNo == 0)
                {
                    if (/*ddlProductCode.SelectedIndex != 0 ||*/ /*ddlProductCollection.SelectedIndex != 0*/ /*||*/ ddlProductName.SelectedIndex != 0)
                    {
                        lblProductFormula.Visibility = Visibility.Hidden;
                        //ddlProductCode.Visibility = Visibility.Hidden;
                        //ddlProductCollection.Visibility = Visibility.Hidden;
                        ddlProductName.Visibility = Visibility.Hidden;
                        imgCenter.Visibility = Visibility.Hidden;

                        lblProductFormulaCollection.Visibility = Visibility.Visible;
                        grdColorFormula.Visibility = Visibility.Visible;
                        imgCenterBtootm.Visibility = Visibility.Visible;
                        imgCenterTop.Visibility = Visibility.Visible;
                        _stepNo = 1;
                    }
                    else
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "REQUIRED!";
                        MySnackbar.Message = "PLEASE SELECT PRODUCT CODE";
                        MySnackbar.Show();
                    }
                }
                else if (_stepNo == 1)
                {
                    if (grdColorFormula.SelectedItems.Count > 0)
                    {
                        lblProductFormulaCollection.Visibility = Visibility.Hidden;
                        grdColorFormula.Visibility = Visibility.Hidden;
                        imgCenterBtootm.Visibility = Visibility.Hidden;
                        imgCenterTop.Visibility = Visibility.Hidden;

                        lblCaneSize.Visibility = Visibility.Visible;
                        ddlCanSize.Visibility = Visibility.Visible;
                        imgCenter.Visibility = Visibility.Visible;
                        _stepNo = 2;
                    }
                    else
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "REQUIRED!";
                        MySnackbar.Message = "PLEASE SELECT ONE ROW FROM GRID";
                        MySnackbar.Show();
                    }
                }
                else if (_stepNo == 2)
                {
                    if (ddlCanSize.SelectedIndex != 0)
                    {
                        lblCaneSize.Visibility = Visibility.Hidden;
                        ddlCanSize.Visibility = Visibility.Hidden;
                        imgCenterBtootm.Visibility = Visibility.Hidden;
                        imgCenterTop.Visibility = Visibility.Hidden;
                        btnNext.Visibility = Visibility.Hidden;

                        btnSubmit.Visibility = Visibility.Visible;
                        imgCenter.Visibility = Visibility.Visible;
                        lblDispenseUnit.Visibility = Visibility.Visible;
                        ddlDispenseUnit.Visibility = Visibility.Visible;
                        _stepNo = 3;
                    }
                    else
                    {
                        MySnackbar.Timeout = 4000;
                        MySnackbar.Title = "REQUIRED!";
                        MySnackbar.Message = "PLEASE SELECT CAN SIZE";
                        MySnackbar.Show();
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

        //private void ddlProductCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Loader.Visibility = Visibility.Visible;
        //    try
        //    {
        //        DispenseViewModel dispenseViewModel = new DispenseViewModel();
        //        ProductCodeList productCode = (ProductCodeList)ddlProductCode.SelectedItem;
        //        CollectionList collection = (CollectionList)ddlProductCollection.SelectedItem;
        //        ProductNameList productName = (ProductNameList)ddlProductName.SelectedItem;
        //        string[] items = new string[] {
        //        (productCode == null || productCode.ID == 0) ? null : productCode.ProductCode.ToString().Trim(),
        //        (collection == null || collection.ID == 0) ? null : collection.Collection.ToString().Trim(),
        //        (productName == null || productName.ID == 0) ? null : productName.ProductName.ToString().Trim()
        //    };

        //        string apiUrl = _apiURL + "Dispense/GetDispenseDataByFilter";
        //        string inputJson = (new JavaScriptSerializer()).Serialize(items);
        //        using (var wc = new WebClient())
        //        {
        //            wc.Headers["Content-type"] = "application/json";
        //            wc.Encoding = Encoding.UTF8;
        //            var result = wc.UploadString(apiUrl, inputJson);

        //            ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

        //            if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
        //            {
        //                dispenseViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DispenseViewModel>(responseObjectForAnything.ResultObject.ToString());

        //                // Bind grid color formula
        //                grdColorFormula.ItemsSource = dispenseViewModel.productFormulaList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MySnackbar.Timeout = 4000;
        //        MySnackbar.Title = "ERROR!";
        //        MySnackbar.Message = "SOMETHING WENT WRONG!";
        //        MySnackbar.Show();
        //    }
        //    Loader.Visibility = Visibility.Hidden;
        //}

        //private void ddlProductCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Loader.Visibility = Visibility.Visible;
        //    try
        //    {
        //        DispenseViewModel dispenseViewModel = new DispenseViewModel();
        //        //ProductCodeList productCode = (ProductCodeList)ddlProductCode.SelectedItem;
        //        CollectionList collection = (CollectionList)ddlProductCollection.SelectedItem;
        //        //ProductNameList productName = (ProductNameList)ddlProductName.SelectedItem;
        //        string[] items = new string[] {
        //       //(productCode == null || productCode.ID == 0) ? null : productCode.ProductCode.ToString().Trim(),
        //        (collection == null || collection.ID == 0) ? null : collection.Collection.ToString().Trim(),
        //        //(productName == null || productName.ID == 0) ? null : productName.ProductName.ToString().Trim()
        //    };

        //        string apiUrl = _apiURL + "Dispense/GetDispenseDataByFilter";
        //        string inputJson = (new JavaScriptSerializer()).Serialize(items);
        //        using (var wc = new WebClient())
        //        {
        //            wc.Headers["Content-type"] = "application/json";
        //            wc.Encoding = Encoding.UTF8;
        //            var result = wc.UploadString(apiUrl, inputJson);

        //            ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

        //            if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
        //            {
        //                dispenseViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DispenseViewModel>(responseObjectForAnything.ResultObject.ToString());

        //                // Bind grid color formula
        //                grdColorFormula.ItemsSource = dispenseViewModel.productFormulaList;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MySnackbar.Timeout = 4000;
        //        MySnackbar.Title = "ERROR!";
        //        MySnackbar.Message = "SOMETHING WENT WRONG!";
        //        MySnackbar.Show();
        //    }
        //    Loader.Visibility = Visibility.Hidden;
        //}

        private void ddlProductName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Loader.Visibility = Visibility.Visible;
            try
            {
                DispenseViewModel dispenseViewModel = new DispenseViewModel();
                //ProductCodeList productCode = (ProductCodeList)ddlProductCode.SelectedItem;
                //CollectionList collection = (CollectionList)ddlProductCollection.SelectedItem;
                ProductNameList productName = (ProductNameList)ddlProductName.SelectedItem;
                string[] items = new string[] {
               //(productCode == null || productCode.ID == 0) ? null : productCode.ProductCode.ToString().Trim(),
                //(collection == null || collection.ID == 0) ? null : collection.Collection.ToString().Trim(),
                (productName == null || productName.ID == 0) ? null : productName.ProductName.ToString().Trim()
            };

                string apiUrl = _apiURL + "Dispense/GetDispenseDataByFilter";
                string inputJson = (new JavaScriptSerializer()).Serialize(items);
                using (var wc = new WebClient())
                {
                    wc.Headers["Content-type"] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    var result = wc.UploadString(apiUrl, inputJson);

                    ResponseObjectForAnything responseObjectForAnything = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObjectForAnything>(result);

                    if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                    {
                        dispenseViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DispenseViewModel>(responseObjectForAnything.ResultObject.ToString());

                        // Bind grid color formula
                        grdColorFormula.ItemsSource = dispenseViewModel.productFormulaList;
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

        private void CanisterAlertDialog_OnButtonLeftClick(object sender, RoutedEventArgs e)
        {
            Loader.Visibility = Visibility.Visible;
            Scanner scanner = new Scanner();
            this.NavigationService.Navigate(scanner);
            Loader.Visibility = Visibility.Hidden;
            CanisterAlertDialog.Hide();
        }

        private void CanisterAlertDialog_OnButtonRightClick(object sender, RoutedEventArgs e)
        {
            CanisterAlertDialog.Hide();
            Loader.Visibility = Visibility.Hidden;
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string[][] param = (string[][])e.Argument;

                int myResult = 0;
                int myIntegerResult;
                Dispenser.FmDispense myDispenser = new Dispenser.FmDispense();
                Dispenser.FmDispensedFormula myDispenserFM = new FmDispensedFormula();

                myIntegerResult = myDispenser.Init();
                //var ii = myDispenser.ShowMainWindow();
                myResult = myDispenser.DefineTask(Convert.ToDouble(param[0][0]), Convert.ToInt32(param[0][1]), 0, param[1], param[2]);
                var myResult1 = myDispenser.Dispense(true);

                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    DispenseSuccess(myResult, Convert.ToInt32(param[0][1]), param[0][2], param[0][3], param[0][4], param[0][5], param[0][6]);
                }));

                myDispenser.Shutdown();
                foreach (Process Proc in Process.GetProcesses())
                {
                    if (Proc.ProcessName.Equals("PRISMA~1"))  //Process Excel?
                        Proc.Kill();
                }
            }
            catch (Exception)
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

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

            loading.Progress = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Loader.Visibility = Visibility.Hidden;
            loading.Progress = 0;
        }
        #endregion

        #region Methods
        public void BindData()
        {
            try
            {
                string apiUrl = _apiURL + "Dispense/GetAllDispenseData";
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

                        // Bind product code
                        //ddlProductCode.SelectedValuePath = "ID";
                        //ddlProductCode.DisplayMemberPath = "ProductCode";
                        //ProductCodeList productCode = new ProductCodeList();
                        //productCode.ID = 0;
                        //productCode.ProductCode = "--SELECT PRODUCT CODE--";
                        //dispenseViewModel.productCodeList.Insert(0, productCode);
                        //ddlProductCode.ItemsSource = dispenseViewModel.productCodeList;

                        //// Bind collection
                        //ddlProductCollection.SelectedValuePath = "ID";
                        //ddlProductCollection.DisplayMemberPath = "Collection";
                        //CollectionList collection = new CollectionList();
                        //collection.ID = 0;
                        //collection.Collection = "--SELECT COLLECTION--";
                        //dispenseViewModel.collectionList.Insert(0, collection);
                        //ddlProductCollection.ItemsSource = dispenseViewModel.collectionList;

                        // Bind product name
                        ddlProductName.SelectedValuePath = "ID";
                        ddlProductName.DisplayMemberPath = "ProductName";
                        ProductNameList productName = new ProductNameList();
                        productName.ID = 0;
                        productName.ProductName = "--SELECT PRODUCT NAME--";
                        dispenseViewModel.productNameList.Insert(0, productName);
                        ddlProductName.ItemsSource = dispenseViewModel.productNameList;

                        //// Bind can size
                        ddlCanSize.SelectedValuePath = "ID";
                        ddlCanSize.DisplayMemberPath = "Size";
                        CanSize canSize = new CanSize();
                        canSize.ID = 0;
                        canSize.Size = "--SELECT CAN SIZE--";
                        dispenseViewModel.canSize.Insert(0, canSize);
                        ddlCanSize.ItemsSource = dispenseViewModel.canSize;

                        //// Bind dispense unit
                        ddlDispenseUnit.SelectedValuePath = "ID";
                        ddlDispenseUnit.DisplayMemberPath = "Name";
                        Unit unit = new Unit();
                        unit.ID = 0;
                        unit.Name = "--SELECT DISPENSE UNIT--";
                        dispenseViewModel.unitList.Insert(0, unit);
                        ddlDispenseUnit.ItemsSource = dispenseViewModel.unitList;

                        lblProductFormula.Visibility = Visibility.Visible;
                        //ddlProductCode.Visibility = Visibility.Visible;
                        //ddlProductCollection.Visibility = Visibility.Visible;
                        ddlProductName.Visibility = Visibility.Visible;
                        imgCenter.Visibility = Visibility.Visible;
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

        public void DispenseNow()
        {
            try
            {
                ProductFormulaList? productFormulaList = new ProductFormulaList();
                string id = string.Empty, productCode = string.Empty, collection = string.Empty, productName = string.Empty, colorCode = string.Empty, amount = string.Empty;

                grdColorFormula.Visibility = Visibility.Visible;
                foreach (var obj in grdColorFormula.SelectedItems)
                {
                    productFormulaList = obj as ProductFormulaList;
                    id = productFormulaList.ID.ToString();
                    productCode = productFormulaList.ProductCode.ToString();
                    collection = productFormulaList.Collection.ToString();
                    productName = productFormulaList.ProductName.ToString();
                    colorCode = productFormulaList.ColorCode.ToString().Trim();
                    amount = productFormulaList.Amount.ToString().Trim();
                }
                grdColorFormula.Visibility = Visibility.Hidden;

                var colorCodeArray = colorCode.Split(',');
                var amountArray = amount.Split(',');

                for (int k = 0; k < amountArray.Length; k++)
                {
                    char separator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    decimal value = Convert.ToDecimal(amountArray[k].Replace('.', separator));
                    amountArray[k] = (value / 1000).ToString();
                }

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
                if (ddlDispenseUnit.SelectedValue.ToString() == "1")
                {
                    dispenseUnit = 0;
                }
                var i = 0;
                foreach (var item in colorFormulaList)
                {
                    myComps[i] = item.ColorCode;
                    myAmounts[i] = item.Amount;
                    i++;
                }

                var result = CheckDispenseDetails(dispenseUnit, colorCode, amount, productCode, collection, productName);
                if (result.Result.ResultCode == Constants.RESPONSE_SUCCESS)
                {
                    char separator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    var dispenseAmount = Convert.ToDecimal(ddlCanSize.Text.ToString().Replace('.', separator));
                    //var dispenseAmount = (Convert.ToDecimal(ddlCanSize.Text) / 1000).ToString();

                    string[] otherdetails = new string[] { (dispenseAmount).ToString(), dispenseUnit.ToString(), colorCode, amount, productCode, collection, productName };
                    string[][] param = new string[][] { otherdetails, myComps, myAmounts };
                    _backgroundWorker.RunWorkerAsync(param);
                }
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

        public async Task<ResponseObjectForAnything> CheckDispenseDetails(int dispenseUnit, string colorCode, string amount, string productCode, string collection, string productName)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                string apiUrl = _apiURL + "Dispense/DispenseNow";
                string inputJson = "AmountToDispense=" + ddlCanSize.Text + "&"
                    + "AmountToDispenseUnitID=" + dispenseUnit + "&"
                    + "ComponentUnitId=" + 0 + "&"
                    + "ComponentNames=" + colorCode + "&"
                    + "ComponentAmounts=" + amount + "&"
                    + "CreatedBy=" + PublicFile.users.ID + "&"
                    + "ProductCode=" + productCode + "&"
                    + "Collection=" + collection + "&"
                    + "ProductName=" + productName + "&"
                    + "MachinID=" + (int)PublicFile.machine.ID + "&"
                    + "UserID=" + PublicFile._user.ID.ToString() + "&"
                    + "IsDispenseFromDesktop=" + 1;

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
                        CanisterAler.Foreground = Brushes.White;
                        CanisterAler.Text = responseObjectForAnything.ResultMessage;
                        CanisterAler.HorizontalAlignment = HorizontalAlignment.Center;
                        pnlCanisterAlertDialogText.Children.Add(CanisterAler);
                        CanisterAlertDialog.Visibility = Visibility.Visible;
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
            return responseObjectForAnything;
        }

        public async Task<ResponseObjectForAnything> SaveDispenseDetails(int dispenseUnit, string colorCode, string amount, string productCode, string collection, string productName)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                string apiUrl = _apiURL + "Dispense/DispenseSuccess";
                string inputJson = "AmountToDispense=" + ddlCanSize.Text + "&"
                    + "AmountToDispenseUnitID=" + dispenseUnit + "&"
                    + "ComponentUnitId=" + 0 + "&"
                    + "ComponentNames=" + colorCode + "&"
                    + "ComponentAmounts=" + amount + "&"
                    + "CreatedBy=" + PublicFile.users.ID + "&"
                    + "MachinID=" + (int)PublicFile.machine.ID + "&"
                    + "ProductCode=" + productCode + "&"
                    + "Collection=" + collection + "&"
                    + "ProductName=" + productName + "&"
                    + "UserID=" + PublicFile._user.ID.ToString() + "&"
                    + "IsDispenseFromDesktop=" + 1;

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
                        CanisterAler.Foreground = Brushes.White;
                        CanisterAler.Text = responseObjectForAnything.ResultMessage;
                        CanisterAler.HorizontalAlignment = HorizontalAlignment.Center;
                        pnlCanisterAlertDialogText.Children.Add(CanisterAler);
                        CanisterAlertDialog.Visibility = Visibility.Visible;
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

        public void DispenseSuccess(int myResult1, int dispenseUnit, string colorCode, string amount, string productCode, string collection, string productName)
        {
            if (myResult1 == 0)
            {
                var resultDispense = SaveDispenseDetails(dispenseUnit, colorCode, amount, productCode, collection, productName);
                if (resultDispense.Result.ResultCode == Constants.RESPONSE_SUCCESS)
                {
                    //ddlProductCode.SelectedIndex = 0;
                    //ddlProductCollection.SelectedIndex = 0;
                    ddlProductName.SelectedIndex = 0;
                    ddlCanSize.SelectedIndex = 0;
                    ddlDispenseUnit.SelectedIndex = 0;

                    btnSubmit.Visibility = Visibility.Hidden;
                    lblDispenseUnit.Visibility = Visibility.Hidden;
                    ddlDispenseUnit.Visibility = Visibility.Hidden;

                    imgCenter.Visibility = Visibility.Visible;
                    lblProductFormula.Visibility = Visibility.Visible;
                    //ddlProductCode.Visibility = Visibility.Visible;
                    //ddlProductCollection.Visibility = Visibility.Visible;
                    ddlProductName.Visibility = Visibility.Visible;
                    _stepNo = 0;

                    Loader.Visibility = Visibility.Hidden;
                    MySnackbar.Timeout = 4000;
                    MySnackbar.Title = "SUCCESS!";
                    MySnackbar.Message = "DISPENSE SUCCESSFULLY!";
                    MySnackbar.Show();
                }
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
        #endregion
    }
}