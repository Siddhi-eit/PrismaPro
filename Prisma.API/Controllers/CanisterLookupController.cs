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
    public class CanisterLookupController : ControllerBase
    {

        private readonly ILogger<CanisterLookupController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CanisterLookupController(ILogger<CanisterLookupController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this._unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetAllCanisterLookup")]

        public async Task<IActionResult> GetAllCanisterLookup()
        {
            GridSearch gridSearch = new GridSearch();
            gridSearch.start = Convert.ToInt32(Request.Form["start"]);
            gridSearch.length = Convert.ToInt32(Request.Form["length"]);
            gridSearch.draw = Convert.ToInt32(Request.Form["draw"]);
            gridSearch.search = Request.Form["Search"].ToString();
            gridSearch.order = Convert.ToInt32(Request.Form["order"]);
            gridSearch.orderDir = Request.Form["orderDir"].ToString();
            gridSearch.UserID = Convert.ToInt32(Request.Form["userId"]);
            gridSearch.MachineID = Convert.ToInt32(Request.Form["machinId"]);

            _logger.LogInformation("================= GetAllCanisterLookup ==================");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.CanisterLookup.GetAllAsync(gridSearch);
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
            CanistersLookupViewModel model = new CanistersLookupViewModel();
            model.ID = Convert.ToInt32(Request.Form["ID"]);
            model.Name = Request.Form["CanisterName"];
            model.CanisterCode = Request.Form["CanisterCode"];
            model.SKU = Request.Form["CanisterSKU"];
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

            _logger.LogInformation("==========Add Canisters Lookup==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = await _unitOfWork.CanisterLookup.Manage(model);
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
            _logger.LogInformation("==========Get CanistersLookup By ID==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.CanisterLookup.GetByIdAsync(id);
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
            _logger.LogInformation("==========Delete Canisters Lookup==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = Convert.ToInt32(await _unitOfWork.CanisterLookup.Delete(id));
                if(result == -1)
                {
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultObject = result;
                responseObjectForAnything.ResultObjectID = id;
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[4000153];
                }
                else
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    responseObjectForAnything.ResultObject = result;
                    responseObjectForAnything.ResultObjectID = id;
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[200040];
                }
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
        [Route("GetCanisterLookupExcelFile")]
        public async Task<IActionResult> GetCanisterLookupExcelFile()
        {
            _logger.LogInformation("========== GetCanisterLookupExcelFile ==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            try
            {
                // Set the ExcelPackage.LicenseContext property
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var result = await _unitOfWork.CanisterLookup.GetCanisterLookupExcelFile();
                List<ExcelCanisterLookupViewModel> excelCanisterLookupViewModelsList = result.ToList();

                if (excelCanisterLookupViewModelsList != null && excelCanisterLookupViewModelsList.Any())
                {
                    DataTable dataTable = CommonHelper.ToDataTable(excelCanisterLookupViewModelsList);

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
