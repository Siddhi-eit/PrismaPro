using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefillTrackingController : ControllerBase
    {
        private readonly ILogger<RefillTrackingController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RefillTrackingController(ILogger<RefillTrackingController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllRefillTracking()
        {
            GridSearch gridSearch = new GridSearch();
            gridSearch.start = Convert.ToInt32(Request.Form["start"]);
            gridSearch.length = Convert.ToInt32(Request.Form["length"]);
            gridSearch.draw = Convert.ToInt32(Request.Form["draw"]);
            gridSearch.search = Request.Form["search"].ToString();
            gridSearch.order = Convert.ToInt32(Request.Form["order"]);
            gridSearch.orderDir = Request.Form["orderDir"].ToString();
            gridSearch.MachineID = Convert.ToInt32(Request.Form["MachineID"]);
            _logger.LogInformation("==========Get All RefillTracking==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.RefillTracking.GetAll(gridSearch);
                var totalRecord = result.Count() != 0 ? Convert.ToInt32(result.FirstOrDefault().TotalRecord) : 0;
                GridResultObject gridResultObject = new GridResultObject();
                gridResultObject.data = result;
                gridResultObject.currentPage = gridSearch.start;
                gridResultObject.pageSize = gridSearch.length;
                gridResultObject.status = true;
                gridResultObject.totalItem = totalRecord;
                gridResultObject.totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRecord) / Convert.ToDecimal(gridSearch.length)));
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObject = gridResultObject;
                _logger.LogInformation($"Total Refill Tracking : {result.Count()}");
            }
            catch (Exception ex)
            {
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [Route("Save")]
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            RefillTrackingViewModel model = new RefillTrackingViewModel();
            model.MachineID = Convert.ToInt32(Request.Form["MachineID"]);
            model.UserID = Convert.ToInt32(Request.Form["UserID"]);
            model.ID = Convert.ToInt32(Request.Form["ID"]);
            model.FusionLabNo = Request.Form["FusionLabNo"].ToString();
            model.CanisterID = Convert.ToInt32(Request.Form["CanisterID"]);
            model.RefillML = Convert.ToInt32(Request.Form["refillML"]);
            model.UnitID = Convert.ToInt32(Request.Form["UnitID"]);
            model.LotNr = Convert.ToInt32(Request.Form["LotNr"]);
            model.IsActive = Request.Form["IsActive"].ToString() == "true" || Request.Form["IsActive"].ToString() == "1" ? true : false;
            if (model.ID > 0)
            {
                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = Convert.ToInt32(Request.Form["CreatedBy"]);
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = Convert.ToInt32(Request.Form["CreatedBy"]);
            }
            _logger.LogInformation("==========Manage RefillTracking==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = await _unitOfWork.RefillTracking.Manage(model);
                _logger.LogInformation($"{responseObjectForAnything.ResultObjectID} - {responseObjectForAnything.ResultMessage}");
            }
            catch (Exception ex)
            {
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
                responseObjectForAnything.ResultObjectID = 0;
            }
            return Ok(responseObjectForAnything);
        }

        [Route("SetRefillDone")]
        [HttpPost]
        public async Task<IActionResult> SetRefillDone(String[] input)
        {
            int refillID = Convert.ToInt32(input[0]);
            int userID = Convert.ToInt32(input[1]);
            int machineID = Convert.ToInt32(input[2]);
            DateTime RefilledDate = DateTime.Now;

            _logger.LogInformation("==========Refill Done==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = await _unitOfWork.RefillTracking.SetRefillDone(Convert.ToInt32(refillID), Convert.ToInt32(userID), RefilledDate, machineID);
                _logger.LogInformation($"{responseObjectForAnything.ResultObjectID} - {responseObjectForAnything.ResultMessage}");
            }
            catch (Exception ex)
            {
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("GetCanSize")]
        public async Task<IActionResult> GetCanSize()
        {
            _logger.LogInformation("==========Bind Dispense GetCanSize==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.RefillTracking.GetCanSize();
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObject = result;
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
        [Route("GetByID")]
        public async Task<IActionResult> GetByID()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            _logger.LogInformation("==========Get Canisters By ID==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.RefillTracking.GetByIdAsync(id);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObject = result;
                responseObjectForAnything.ResultObjectID = id;
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
        [Route("GetScannerDetailByID")]
        public async Task<IActionResult> GetScannerDetailByID()
        {
            int id = Convert.ToInt32(Request.Form["Id"]);
            _logger.LogInformation("==================== Get ScannerDetail By ID =====================");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try

            {
                var result = await _unitOfWork.RefillTracking.GetScannerDetailByIdAsync(id);
                responseObjectForAnything.ResultCode= Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObject = result;
                responseObjectForAnything.ResultObjectID = id;

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
        [Route("Delete")]
        public async Task<IActionResult> Delete()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            int userId = Convert.ToInt32(Request.Form["userId"]);
            _logger.LogInformation("==========Delete Sanitisting==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.RefillTracking.RefillTrakingDelete(id, userId);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObject = result;
                responseObjectForAnything.ResultObjectID = id;
                _logger.LogInformation($"{id} - {result}");
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
        [Route("GetRefillExcelFile")]
        public async Task<IActionResult> GetRefillTrackingExcelFile()
        {
            _logger.LogInformation("========== GetRefillTrackingExcelFile ==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            try
            {
                // Set the ExcelPackage.LicenseContext property
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var result = await _unitOfWork.RefillTracking.GetRefillTrackingExcelFile();
                List<ExcelRefillTrackingViewModel> excelRefillTrackingViewModelsList = result.ToList();

                if (excelRefillTrackingViewModelsList != null && excelRefillTrackingViewModelsList.Any())
                {
                    DataTable dataTable = CommonHelper.ToDataTable(excelRefillTrackingViewModelsList);

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
    }
}