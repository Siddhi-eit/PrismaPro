using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.API.Helper;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Common;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Data;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormulaController : Controller
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        private readonly ILogger<FormulaController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        JWTHelper _jwtHelper = null;
        string _encryptedPassword = string.Empty;
        #endregion

        #region Constructor
        public FormulaController(ILogger<FormulaController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _jwtHelper = new JWTHelper(configuration);
            _configuration = configuration;
        }
        #endregion
        [AllowAnonymous]
        [HttpPost]
        [Route("SaveFormulaProfile")]
        public async Task<IActionResult> SaveFormulaProfile([FromBody] Formula formula)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("==========Sign In==========");
            try
            {

                var result = await _unitOfWork.FormulaRepository.SaveFormulaProfile(formula);
                if (result.ResultObjectID > 0)
                {
                    _logger.LogInformation("==========Sign In Successfully==========");
                    responseObjectForAnything.ResultObject = result;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    return Ok(result);
                }
                else if (result.ResultObjectID == -1)
                {
                    return Ok(result);
                }
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
        [Route("GetAllFormula")]
        public async Task<IActionResult> GetAllFormula()
        {
            GridSearch gridSearch = new GridSearch();
            gridSearch.start = Convert.ToInt32(Request.Form["start"]);
            gridSearch.length = Convert.ToInt32(Request.Form["length"]);
            gridSearch.draw = Convert.ToInt32(Request.Form["draw"]);
            gridSearch.search = Request.Form["search"].ToString();
            gridSearch.order = Convert.ToInt32(Request.Form["order"]);
            gridSearch.orderDir = Request.Form["orderDir"].ToString();
            gridSearch.UserID = Convert.ToInt32(Request.Form["userID"]);

            _logger.LogInformation("==========GetAllFormula==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.FormulaRepository.GetAllFormula(gridSearch);
                var totalRecord = result.Count() != 0 ? Convert.ToInt32(result.FirstOrDefault().TotalRecord) : 0;
                GridResultObject gridResultObject = new GridResultObject();
                gridResultObject.data = result;
                gridResultObject.currentPage = gridSearch.start;
                gridResultObject.pageSize = gridSearch.length;
                gridResultObject.status = true;
                gridResultObject.totalItem = totalRecord;
                gridResultObject.totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRecord) / Convert.ToDecimal(gridSearch.length)));

                _logger.LogInformation($"Total Machine{result.Count()}");
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
        [Route("GetByID")]
        public async Task<IActionResult> GetByID()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            _logger.LogInformation("==========GetByID==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.FormulaRepository.GetByID(id);
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
            _logger.LogInformation("==========Delete Machine==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.FormulaRepository.Delete(id);
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
        [Route("GetFormulaExcelFile")]
        public async Task<IActionResult> GetFormulaExcelFile()
        {
            _logger.LogInformation("========== GetFormulaExcelFile ==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            try
            {
                // Set the ExcelPackage.LicenseContext property
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var result = await _unitOfWork.FormulaRepository.GetFormulaExcelFile();
                List<ExcelFormulaViewModel> excelFormulaViewModelsList = result.ToList();

                if (excelFormulaViewModelsList != null && excelFormulaViewModelsList.Any())
                {
                    DataTable dataTable = CommonHelper.ToDataTable(excelFormulaViewModelsList);

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
