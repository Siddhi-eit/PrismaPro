using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Data;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanitisingTrakingController : ControllerBase
    {
        private readonly ILogger<SanitisingTrakingController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SanitisingTrakingController(ILogger<SanitisingTrakingController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllSanitisingTraking()
        {
            GridSearch gridSearch = new GridSearch();
            gridSearch.start = Convert.ToInt32(Request.Form["start"]);
            gridSearch.length = Convert.ToInt32(Request.Form["length"]);
            gridSearch.draw = Convert.ToInt32(Request.Form["draw"]);
            gridSearch.search = Request.Form["search"].ToString();
            gridSearch.order = Convert.ToInt32(Request.Form["order"]);
            gridSearch.orderDir = Request.Form["orderDir"].ToString();
            //gridSearch.UserID = Convert.ToInt32(Request.Form["userID"]);
            gridSearch.MachineID = Convert.ToInt32(Request.Form["MachineID"]);

            _logger.LogInformation("==========Get All SanitisingTraking==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.SanitisingTraking.GetAllSanitisingTraking(gridSearch);
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
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }

        [HttpPost]
        [Route("UpsertSanitisingTraking")]
        public async Task<IActionResult> Post()
        {
            SanitisingTrakingViewModel model = new SanitisingTrakingViewModel();
            DateTime now = DateTime.Now;
            model.ID = Convert.ToInt32(Request.Form["ID"]);
            model.UserID = Convert.ToInt32(Request.Form["UserID"]);
            model.MachineID = Convert.ToInt32(Request.Form["MachineID"]);
            //model.RefillingPeriod = Convert.ToInt32(Request.Form["RefillingPeriod"]);
            model.SetReminder = DateTime.Now.AddMonths(model.RefillingPeriod);
            model.FusionLabNo = Request.Form["FusionLabNo"].ToString();
            model.CanisterID = Convert.ToInt32(Request.Form["CanisterID"].ToString());
            //model.ProductID = Convert.ToInt32(Request.Form["ProductID"]);
            //model.CanisterID = Convert.ToInt32(Request.Form["ProductID"]);
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
            _logger.LogInformation("==========Manage SanitisingTraking==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = await _unitOfWork.SanitisingTraking.Manage(model);
                _logger.LogInformation($"{responseObjectForAnything.ResultObjectID} - {responseObjectForAnything.ResultMessage}");
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
        [Route("GetByID")]
        public async Task<IActionResult> GetByID()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            _logger.LogInformation("==========Get Canisters By ID==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.SanitisingTraking.GetByIdAsync(id);
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
        [Route("Delete")]
        public async Task<IActionResult> Delete()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            int userId = Convert.ToInt32(Request.Form["userId"]);
            _logger.LogInformation("==========Delete Sanitisting==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.SanitisingTraking.SanitisingTrakingDelete(id, userId);
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
        [Route("SetSanitizationDone")]
        public async Task<IActionResult> SetSanitizationDone()
        {
            int userId = Convert.ToInt32(Request.Form["UserId"]);
            DateTime RefilledDate = DateTime.Now;
            _logger.LogInformation("==========Sanitisting Done==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.SanitisingTraking.SetSanitizationDone(userId, RefilledDate);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObject = result;
                responseObjectForAnything.ResultObjectID = userId;
                _logger.LogInformation($"{userId} - {result}");
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
        [Route("SetSanitizationSuccess")]
        public async Task<IActionResult> SetSanitizationSuccess(String[] input)
        {
            int ID = Convert.ToInt32(input[0]);
            int userID = Convert.ToInt32(input[1]);
            int machineID = Convert.ToInt32(input[2]);
            DateTime RefilledDate = DateTime.Now;
            _logger.LogInformation("==========Sanitisting success==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.SanitisingTraking.SetSanitizationSuccess(ID, userID, RefilledDate, machineID);
                responseObjectForAnything = result;
                _logger.LogInformation($"{ID} - {result}");
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
        [Route("GetSanitisingExcelFile")]
        public async Task<IActionResult> GetSanitisingTrakingExcelFile()
        {
            _logger.LogInformation("========== GetSanitisingTrakingExcelFile ==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            try
            {
                // Set the ExcelPackage.LicenseContext property
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var result = await _unitOfWork.SanitisingTraking.GetSanitisingTrakingExcelFile();
                List<ExcelSanitisingTrakingViewModel> excelSanitisingTrakingViewModelsList = result.ToList();

                if (excelSanitisingTrakingViewModelsList != null && excelSanitisingTrakingViewModelsList.Any())
                {
                    DataTable dataTable = CommonHelper.ToDataTable(excelSanitisingTrakingViewModelsList);

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