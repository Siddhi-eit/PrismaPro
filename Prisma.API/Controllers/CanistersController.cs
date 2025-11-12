using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanistersController : ControllerBase
    {
        private readonly ILogger<CanistersController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CanistersController(ILogger<CanistersController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetAllCanisters")]
        public async Task<IActionResult> GetAllCanisters()
        {
            GridSearch gridSearch = new GridSearch();
            gridSearch.start = Convert.ToInt32(Request.Form["start"]);
            gridSearch.length = Convert.ToInt32(Request.Form["length"]);
            gridSearch.draw = Convert.ToInt32(Request.Form["draw"]);
            gridSearch.search = Request.Form["search"].ToString();
            gridSearch.order = Convert.ToInt32(Request.Form["order"]);
            gridSearch.orderDir = Request.Form["orderDir"].ToString();
            gridSearch.UserID = Convert.ToInt32(Request.Form["userID"]);
            gridSearch.MachineID = Convert.ToInt32(Request.Form["machineID"]);

            _logger.LogInformation("==========GetAllCanisters==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Canisters.GetAllAsync(gridSearch);
                var totalRecord = result.Count() != 0 ? Convert.ToInt32(result.FirstOrDefault().TotalRecord) : 0;
                GridResultObject gridResultObject = new GridResultObject();
                gridResultObject.data = result;
                gridResultObject.currentPage = gridSearch.start;
                gridResultObject.pageSize = gridSearch.length;
                gridResultObject.status = true;
                gridResultObject.totalItem = totalRecord;
                gridResultObject.totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRecord) / Convert.ToDecimal(gridSearch.length)));

                _logger.LogInformation($"Total Canisters{result.Count()}");
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
            var json = System.Text.Json.JsonSerializer.Serialize(responseObjectForAnything);
            return Ok(json);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save()
        {
            CanistersViewModel model = new CanistersViewModel();
            model.UserID = Convert.ToInt32(Request.Form["UserID"]);
            model.ID = Convert.ToInt32(Request.Form["ID"]);
            model.CanisterLookupId = Convert.ToInt32(Request.Form["canisterLookupId"]);
            model.MaximumAmount = Convert.ToDecimal(Request.Form["MaximumAmount"]);
            model.MinimumAmount = Convert.ToDecimal(Request.Form["MinimumAmount"]);
            model.CurrentAmount = Convert.ToDecimal(Request.Form["CurrentAmount"]);
            model.WarningAmount = Convert.ToDecimal(Request.Form["WarningAmount"]);
            model.MachineID = Convert.ToInt32(Request.Form["MachineID"]);

            model.UnitID = Convert.ToInt32(Request.Form["UnitID"]);
            model.IsActive = Request.Form["IsActive"].ToString() == "true" || Request.Form["IsActive"].ToString() == "1" ? 1 : 0;
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

            _logger.LogInformation("==========Add Canisters==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = await _unitOfWork.Canisters.Manage(model);
                _logger.LogInformation($"{responseObjectForAnything.ResultObjectID} = {responseObjectForAnything.ResultMessage}");
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
                var result = await _unitOfWork.Canisters.GetByIdAsync(id);
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
            _logger.LogInformation("==========Delete Canisters==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Canisters.Delete(id);
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
        [Route("BindDispenseUnitDropdown")]
        public async Task<IActionResult> BindDispenseUnitDropdown()
        {
            _logger.LogInformation("==========Bind Dispense UnitDropdown==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Canisters.BindDispenseUnitDropdown();
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
        [Route("getByUserID")]
        public async Task<IActionResult> getByUserID()
        {
            int id = Convert.ToInt32(Request.Form["machineID"]);
            _logger.LogInformation("==========Bind Dispense UnitDropdown==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Canisters.getByUserIdAsync(id);
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
        [Route("BindProductDropdown")]
        public async Task<IActionResult> BindProductDropdown()
        {
            _logger.LogInformation("==========Bind Product UnitDropdown==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Canisters.BindProductDropdown();
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
        [Route("GetCanisterExcelFile")]
        public async Task<IActionResult> GetCanisterExcelFile()
        {
            _logger.LogInformation("========== GetCanisterExcelFile ==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            try
            {   
                // Set the ExcelPackage.LicenseContext property
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var result = await _unitOfWork.Canisters.GetCanisterExcelFile();
                List<ExcelCanisterViewModel> excelCanisterViewModelsList = result.ToList();

                if (excelCanisterViewModelsList != null && excelCanisterViewModelsList.Any())
                {
                    DataTable dataTable = CommonHelper.ToDataTable(excelCanisterViewModelsList);

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

        [HttpPost]
        [Route("GetCanisterLookup")]
        public async Task<IActionResult> GetCanisterLookup()
        {
            _logger.LogInformation("=========== GetCanisterLookupByCode =========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                //string canisterCode = Request.Form["canisterCode"];
                var result = await _unitOfWork.Canisters.GetCanisterLookup();
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObject = result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage= ex.Message;

            }
            return Ok(responseObjectForAnything);
        }
    }
}