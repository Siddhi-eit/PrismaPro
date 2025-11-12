using MDFusionLabHaute.API.Helper;
using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OfficeOpenXml;
using System.Data;
using System.Globalization;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispenseController : ControllerBase
    {
        private readonly ILogger<DispenseController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ConnectionHub> _connectionHub;
        //public readonly IConnectionMapping<string> _connections;

        public DispenseController(ILogger<DispenseController> logger, IUnitOfWork unitOfWork, IHubContext<ConnectionHub> hubContext/*, IConnectionMapping<string> connections*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _connectionHub = hubContext;
            //_connections = connections;
        }

        #region Actions


        [HttpPost]
        [Route("CheckConsultantID")]
        public async Task<IActionResult> CheckConsultantID(String[] input)
        {
            var consultantID = input[0].ToString();
            var userID = input[1].ToString();
            _logger.LogInformation("==========CheckConsultantID==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = _unitOfWork.Dispense.CheckConsultantID(consultantID, userID);
                result.Wait();
                responseObjectForAnything = result.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        //[HttpPost]
        //[Route("CheckConsultantPassword")]
        //public async Task<IActionResult> CheckConsultantPassword(String[] input)
        //{
        //    var consultantID = input[0].ToString();
        //    var userID = input[1].ToString();
        //    _logger.LogInformation("==========CheckConsultantPassword==========");
        //    ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
        //    try
        //    {
        //        var result = _unitOfWork.Dispense.CheckConsultantPassword(consultantID, userID);
        //        result.Wait();
        //        responseObjectForAnything = result.Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        await _unitOfWork.ExceptionLog.InsertLog(ex);
        //        responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
        //        responseObjectForAnything.ResultMessage = ex.Message;
        //    }
        //    return Ok(responseObjectForAnything);
        //}

        [HttpPost]
        [Route("CheckProductCode")]
        public async Task<IActionResult> CheckProductCode(String[] input)
        {
            var productCode = input[0].ToString();
            _logger.LogInformation("==========CheckProductCode==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = _unitOfWork.Dispense.CheckProductCode(productCode);
                result.Wait();
                responseObjectForAnything = result.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("CheckTailoringCode")]
        public async Task<IActionResult> CheckTailoringCode(String[] input)
        {
            var tailoringCode = input[0].ToString();
            var dermaprofile = input[1].ToString();
            _logger.LogInformation("==========CheckTailoringCode==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = _unitOfWork.Dispense.CheckTailoringCode(tailoringCode, dermaprofile);
                result.Wait();
                responseObjectForAnything = result.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetDispenseData")]
        public async Task<IActionResult> GetDispenseData(String[] input)
        {
            var tailoringCode = input[0].ToString();
            var dermaprofile = input[1].ToString();
            var essenceOption = Convert.ToInt32(input[2]);
            var userID = input[3].ToString();
            _logger.LogInformation("==========GetDispenseData==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Dispense.GetDispenseData(tailoringCode, dermaprofile, essenceOption, userID).ConfigureAwait(false);
                responseObjectForAnything = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetDispenseDataByProductCode")]
        public async Task<IActionResult> GetDispenseDataByProductCode(String[] input)
        {
            var productCode = input[0].ToString();
            _logger.LogInformation("==========GetDispenseDataByProductCode==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Dispense.GetDispenseDataByProductCode(productCode).ConfigureAwait(false);
                responseObjectForAnything = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetSelectedEssence")]
        public async Task<IActionResult> GetSelectedEssence(String[] input)
        {
            var tailoringCode = input[0].ToString();
            var selectedEssence = input[1].ToString();
            _logger.LogInformation("==========GetSelectedEssence==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = _unitOfWork.Dispense.GetSelectedEssence(tailoringCode, selectedEssence);
                responseObjectForAnything = result.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetAllDispense")]
        public async Task<IActionResult> GetAllDispense()
        {
            GridSearch gridSearch = new GridSearch();
            gridSearch.start = Convert.ToInt32(Request.Form["start"]);
            gridSearch.length = Convert.ToInt32(Request.Form["length"]);
            gridSearch.draw = Convert.ToInt32(Request.Form["draw"]);
            gridSearch.search = Request.Form["search"].ToString();
            gridSearch.order = Convert.ToInt32(Request.Form["order"]);
            gridSearch.orderDir = Request.Form["orderDir"].ToString();
            gridSearch.MachineID = Convert.ToInt32(Request.Form["machineID"]);
            _logger.LogInformation("==========Get_All_Data_For_Dispense==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Dispense.GetAll(gridSearch);
                var totalRecord = result.Count() != 0 ? Convert.ToInt32(result.FirstOrDefault().TotalRecord) : 0;
                GridResultObject gridResultObject = new GridResultObject();
                gridResultObject.data = result;
                gridResultObject.currentPage = gridSearch.start;
                gridResultObject.pageSize = gridSearch.length;
                gridResultObject.status = true;
                gridResultObject.totalItem = totalRecord;
                gridResultObject.totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRecord) / Convert.ToDecimal(gridSearch.length)));
                _logger.LogInformation($"Total User:{result.Count()}");
                // _logger.LogInformation($"Total Dispense: {result.Result.Count()}");
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObject = gridResultObject;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetDispenseFormulaData")]
        public async Task<IActionResult> GetDispenseFormulaData()
        {
            _logger.LogInformation("==========GetDispenseFormulaData==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = _unitOfWork.Dispense.GetDispenseFormulaData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }
            
        [HttpPost]
        [Route("BindProductionModeData")]
        public async Task<IActionResult> BindProductionModeData()
        {
            _logger.LogInformation("==========GetDispenseFormulaData==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                //responseObjectForAnything = _unitOfWork.Dispense.GetProductionModeData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        public async Task<IActionResult> Post(DispenseLogNew model)
        {
            _logger.LogInformation("==========Add Dispense==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = await _unitOfWork.Dispense.Manage(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetAllDispenseData")]
        public async Task<IActionResult> GetAllDispenseData()
        {
            _logger.LogInformation("==========GetAllDispense==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = _unitOfWork.Dispense.GetAllDispenseDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetDispenseDataByFilter")]
        public async Task<IActionResult> GetDispenseDataByFilter(string[] items)
        {
            _logger.LogInformation("==========GetAllDispense==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = _unitOfWork.Dispense.GetDispenseDataByFilter(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("DispenseNow")]
        public async Task<IActionResult> DispenseNow()
        {
            DispenseLog dispenserViewModel = new();
            _logger.LogInformation("==========DispenseNow==========");
            ResponseObjectForAnything responseObjectForAnything = new();

            try
            {
                dispenserViewModel.MachineID = Convert.ToInt32(Request.Form["machineID"]);
                dispenserViewModel.CreatedBy = Convert.ToInt32(Request.Form["CreatedBy"]);
                dispenserViewModel.AmountToDispense = Convert.ToDecimal(Request.Form["AmountToDispense"]);
                dispenserViewModel.AmountToDispenseUnitId = Convert.ToInt32(Request.Form["AmountToDispenseUnitID"]);
                dispenserViewModel.ComponentUnitId = Convert.ToInt32(Request.Form["AmountToDispenseUnitID"]);
                dispenserViewModel.ComponentNames = Request.Form["ComponentNames"];
                dispenserViewModel.ComponentAmounts = Request.Form["ComponentAmounts"];
                dispenserViewModel.FormulaDispenseAmount = Request.Form["FormulaDispenseAmount"];
                int CurrentUserID = Convert.ToInt32(Request.Form["CurrentUserID"]);

                string[] canisterCode = dispenserViewModel.ComponentNames.Split(',');//key
                string[] canisterAmount = dispenserViewModel.ComponentAmounts.Split('-');//value

                string CanisterCodeAndAmount = "";
                string dispenser = Newtonsoft.Json.JsonConvert.SerializeObject(dispenserViewModel);

                string productCode = Convert.ToString(Request.Form["ProductCode"]);
                dispenserViewModel.ProductCode = productCode;

                string collection = Convert.ToString(Request.Form["Collection"]);
                string productName = Convert.ToString(Request.Form["ProductName"]);
                byte IsdispenseFromDesktop = Convert.ToByte(Request.Form["IsDispenseFromDesktop"]);

                for (int i = 0; i < canisterCode.Length; i++)
                {
                    CanisterCodeAndAmount += canisterCode[i] + ":" + canisterAmount[i] + "-";
                }
                CanisterCodeAndAmount = CanisterCodeAndAmount.TrimEnd(',');
                CanisterCodeAndAmount = CanisterCodeAndAmount.Trim().Replace(" ", "");

                var result = await _unitOfWork.Dispense.CheckCanisterIsEligibleForDispense(CanisterCodeAndAmount, dispenserViewModel.AmountToDispense);
                if (result.Count() == 0)
                {

                    if (IsdispenseFromDesktop == 0)
                    {
                        var para = productCode + "/" + dispenserViewModel.AmountToDispense + "/" + dispenserViewModel.AmountToDispenseUnitId + "/" + dispenserViewModel.ComponentUnitId + "/" + dispenserViewModel.ComponentNames + "/" + dispenserViewModel.ComponentAmounts + "/" + dispenserViewModel.CreatedBy + "/" + dispenserViewModel.FormulaDispenseAmount + "/" + CurrentUserID;
                        if (PublicFile._connection != null)
                        {
                            var connectionId = PublicFile._connection.GetConnections("M-" + dispenserViewModel.MachineID.ToString());
                            if (connectionId.Count() > 0)
                            {
                                foreach (var connectionId1 in PublicFile._connection.GetConnections("M-" + dispenserViewModel.MachineID.ToString()))    
                                {
                                    responseObjectForAnything.ResultObjectID = 1;
                                    await _connectionHub.Clients.Client(connectionId1).SendAsync("DispenseNow", para);
                                }
                            }
                            else
                            {
                                responseObjectForAnything.ResultMessage = "Machine is unavailable !!";
                                responseObjectForAnything.ResultObjectID = 0;
                            }
                        }
                        else
                        {
                            responseObjectForAnything.ResultMessage = "Machine is unavailable !!";
                            responseObjectForAnything.ResultObjectID = 0;
                        }
                    }
                    else
                    {
                        responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                        responseObjectForAnything.ResultObjectID = 1;
                    }
                }
                else
                {
                    foreach (var item in result)
                    {
                        responseObjectForAnything.ResultMessage += $"Color {item.SKU} is not sufficient in {item.CanisterCode.Trim()} canister.\r\n ";
                    }
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                    responseObjectForAnything.ResultObjectID = -1;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
                responseObjectForAnything.ResultObjectID = 0;
            }
            return Ok(responseObjectForAnything);
        }


        [HttpPost]
        [Route("DispenseSuccess")]
        public async Task<IActionResult> DispenseSuccess()
        {
            List<ProductWithFormulaViewModel> productWithFormulaViewModel = new List<ProductWithFormulaViewModel>();
            DispenseLogNew dispenserViewModel = new();
            _logger.LogInformation("==========DispenseSuccess==========");
            ResponseObjectForAnything responseObjectForAnything = new();

            try
            {
                if (Request.Form["BACH_LOT_NO"].ToString() != null && Request.Form["CONSULTANT_ID"].ToString() != null)
                {
                    var tailoringCode = Convert.ToString(Request.Form["ProductCode"]);
                    string userID = Request.Form["UserID"].ToString();
                    var result = await _unitOfWork.Dispense.GetDispenseData(tailoringCode, string.Empty, 1, userID).ConfigureAwait(false);
                    productWithFormulaViewModel = (List<ProductWithFormulaViewModel>)result.ResultObject;
                }
                if (!int.TryParse(Request.Form["MachinID"], out int machineId))
                {
                    _logger.LogWarning("Invalid MachinID format: " + Request.Form["MachinID"]);
                    return BadRequest("Invalid MachinID");
                }
                dispenserViewModel.MachineID = machineId;

                //dispenserViewModel.MachineID = Convert.ToInt32(Request.Form["MachineID"]);
                dispenserViewModel.UserID = Convert.ToInt32(Request.Form["UserID"]);
                dispenserViewModel.ProductCode = string.IsNullOrEmpty(Request.Form["ProductCode"]) ? productWithFormulaViewModel[0].ProductCode : Convert.ToString(Request.Form["ProductCode"]);
                dispenserViewModel.CreatedBy = string.IsNullOrEmpty(Request.Form["CreatedBy"]) ? Convert.ToInt32(Request.Form["CreatedBy"]) : Convert.ToInt32(Request.Form["UserID"]);

                dispenserViewModel.DispensationsNumber = Convert.ToInt32(Request.Form["DispensationsNumber"]) == 0 ? 1
                                                            : Convert.ToInt32(Request.Form["DispensationsNumber"]);
                dispenserViewModel.AmountToDispense = Convert.ToDecimal(Request.Form["AmountToDispense"]);
                dispenserViewModel.ComponentNames = Request.Form["ComponentNames"];
                dispenserViewModel.ComponentAmounts = Request.Form["ComponentAmounts"];
                dispenserViewModel.AmountToDispenseUnitId = Convert.ToInt32(Request.Form["AmountToDispenseUnitID"]) == 0 ? 1
                                                            : Convert.ToInt32(Request.Form["AmountToDispenseUnitID"]);
                dispenserViewModel.ComponentUnitId = Convert.ToInt32(Request.Form["AmountToDispenseUnitID"]);
                dispenserViewModel.IsDispense = Request.Form["IsDispense"].ToString() == "True" || Request.Form["IsActive"].ToString() == "1" ? true : false;
                var machineRegNr = string.Empty;
                if (productWithFormulaViewModel != null && productWithFormulaViewModel.Count > 0)
                {
                    var machineRegNrFromProduct = productWithFormulaViewModel[0]?.MDFusionLabNo;
                    machineRegNr = string.IsNullOrEmpty(machineRegNrFromProduct) ? "" : machineRegNrFromProduct;
                }
                dispenserViewModel.MACHINE_REG_NO = string.IsNullOrEmpty(Request.Form["MACHINE_REG_NO"])
                    ? machineRegNr
                    : Convert.ToString(Request.Form["MACHINE_REG_NO"]);
                dispenserViewModel.COUNTRY = Convert.ToString(Request.Form["COUNTRY"]);
                dispenserViewModel.SHOP = Convert.ToString(Request.Form["SHOP"]);
                dispenserViewModel.CONSULTANT_ID = Request.Form["CONSULTANT_ID"];
                dispenserViewModel.DERMAPROFILE = Request.Form["DERMAPROFILE"];
                dispenserViewModel.TAILORING_CODE = string.IsNullOrEmpty(Request.Form["TAILORING_CODE"]) ? Convert.ToString(Request.Form["ProductCode"]) : Convert.ToString(Request.Form["TAILORING_CODE"]);
                dispenserViewModel.ESSENCE = Convert.ToString(Request.Form["ESSENCE"]);

                var bachLotNo = string.Empty;
                if (productWithFormulaViewModel != null && productWithFormulaViewModel.Count > 0)
                {
                    var bachLotNoFromProduct = productWithFormulaViewModel[0]?.BachLotNo;
                    bachLotNo = string.IsNullOrEmpty(bachLotNoFromProduct) ? "" : bachLotNoFromProduct;
                }

                dispenserViewModel.BACH_LOT_NO = string.IsNullOrEmpty(Request.Form["BACH_LOT_NO"])
                    ? bachLotNo
                    : Convert.ToString(Request.Form["BACH_LOT_NO"]);

                dispenserViewModel.PRICE = string.IsNullOrEmpty(Request.Form["PRICE"]) ? productWithFormulaViewModel[0].TotalFormulaPriceUSD : Convert.ToDecimal(Request.Form["PRICE"]);

                // Handle DateTime parsing with TryParseExact
                string[] dateFormats = { "dd-MM-yyyy HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd" };  // Add any other formats you might encounter

                if (string.IsNullOrEmpty(Request.Form["TIME_ID_ENTERED"]) || !DateTime.TryParseExact(Request.Form["TIME_ID_ENTERED"], dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeIdEntered))
                {
                    dispenserViewModel.TIME_ID_ENTERED = DateTime.Now;
                }
                else
                {
                    dispenserViewModel.TIME_ID_ENTERED = timeIdEntered;
                }

                if (string.IsNullOrEmpty(Request.Form["DATE"]) || !DateTime.TryParseExact(Request.Form["DATE"], dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    dispenserViewModel.DATE = DateTime.Now;
                }
                else
                {
                    dispenserViewModel.DATE = date;
                }

                if (string.IsNullOrEmpty(Request.Form["TIME_DISPENSED"]) || !DateTime.TryParseExact(Request.Form["TIME_DISPENSED"], dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeDispensed))
                {
                    dispenserViewModel.TIME_DISPENSED = DateTime.Now;
                }
                else
                {
                    dispenserViewModel.TIME_DISPENSED = timeDispensed;
                }

                dispenserViewModel.dispenseQuantity = Convert.ToInt32(Request.Form["dispenseQuantity"]);
                responseObjectForAnything = await _unitOfWork.Dispense.Manage(dispenserViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
                responseObjectForAnything.ResultObjectID = 0;
            }

            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("CheckIsCanisterExists")]
        public async Task<IActionResult> CheckIsCanisterExists()
        {
            string userid = Request.Form["userid"].ToString();
            string componentnames = Request.Form["componentnames"].ToString();
            _logger.LogInformation("==========Check if Canister Exists==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = _unitOfWork.Dispense.CheckIsCanisterExists(userid, componentnames);
            }
            catch (Exception ex)
            {   
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("CheckDermaprofileData")]
        public async Task<IActionResult> CheckDermaprofileData(String[] input)
        {
            var dermaprofile = input[0].ToString();
            var userID = input[1].ToString();
            _logger.LogInformation("==========CheckConsultantID==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = _unitOfWork.Dispense.CheckDermaprofileData(dermaprofile);
                result.Wait();
                responseObjectForAnything = result.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetDispenseExcelFile")]
        public async Task<IActionResult> GetDispenseExcelFile()
        {
            _logger.LogInformation("========== GetDispenseExcelFile ==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            try
            {
                // Set the ExcelPackage.LicenseContext property
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var machineID  = Convert.ToInt32(Request.Form["machineID"]);
                var result = await _unitOfWork.Dispense.GetDispenseExcelFile(machineID);
                List<ExcelDispenseViewModel> excelDispenseViewModelsList = result.ToList();

                if (excelDispenseViewModelsList != null && excelDispenseViewModelsList.Any())
                {
                    DataTable dataTable = CommonHelper.ToDataTable(excelDispenseViewModelsList);
                    dataTable.Columns["TailoringCode"].ColumnName = "Tailoring Code";
                    dataTable.Columns["AmountToDispensePerBottle"].ColumnName = "Amount To Dispense Per Bottle";
                    dataTable.Columns["DispensationsNumber"].ColumnName = "Total Number of Bottles";
                    dataTable.Columns["ComponentNames"].ColumnName = "Component Names";
                    dataTable.Columns["ComponentAmounts"].ColumnName = "Component Amounts";
                    dataTable.Columns["LotNr"].ColumnName = "Lot Nr";
                    dataTable.Columns["CreatedDate"].ColumnName = "Created Date";

                    using (MemoryStream stream = new MemoryStream())
                    using (ExcelPackage excelPackage = CommonHelper.CreateExcelPackage(dataTable, stream))
                    {
                        // Return the Excel file in the response with correct file extension ".xlsx"
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "output.xlsx");
                    }
                }
                else
                {
                    responseObjectForAnything.ResultObject = "No data found";
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                    return NotFound(responseObjectForAnything);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);

                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
                return StatusCode(500, responseObjectForAnything);
            }
        }

        #endregion
    }
}