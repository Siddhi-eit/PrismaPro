using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MDFusionLabHaute.API.Helper;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;
using System.Data;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        private readonly ILogger<MachineController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        JWTHelper _jwtHelper = null;
        string _encryptedPassword = string.Empty;
        #endregion

        #region Constructor
        public MachineController(ILogger<MachineController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _jwtHelper = new JWTHelper(configuration);
            _configuration = configuration;
        }
        #endregion

        #region Actions
        [AllowAnonymous]
        [HttpPost]
        [Route("SaveMachineProfile")]
        public async Task<IActionResult> SaveMachineProfile([FromBody] Machine machine)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("==========Sign In==========");
            try
            {
                var result = await _unitOfWork.MachineRepository.SaveMachineProfile(machine);
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
        [Route("BindDropdownRolewise")]
        public async Task<IActionResult> BindDropdownRolewise()
        {
            _logger.LogInformation("==========Get machine By Id==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                var result = await _unitOfWork.MachineRepository.BindMachineDropdown(id);
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
        [Route("GetAllMachine")]
        public async Task<IActionResult> GetAllMachine()
        {
            GridSearch gridSearch = new GridSearch();
            gridSearch.start = Convert.ToInt32(Request.Form["start"]);
            gridSearch.length = Convert.ToInt32(Request.Form["length"]);
            gridSearch.draw = Convert.ToInt32(Request.Form["draw"]);
            gridSearch.search = Request.Form["search"].ToString();
            gridSearch.order = Convert.ToInt32(Request.Form["order"]);
            gridSearch.orderDir = Request.Form["orderDir"].ToString();
            gridSearch.UserID = Convert.ToInt32(Request.Form["userID"]);

            _logger.LogInformation("==========GetAllMachine==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.MachineRepository.GetAllAsync(gridSearch);
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
        [Route("Save")]
        public async Task<IActionResult> Save()
        {
            MachineViewModel model = new MachineViewModel();
            model.ID = Convert.ToInt32(Request.Form["ID"]);
            model.MachineRegNo = Request.Form["MachineRegNo"].ToString();
            model.ShopName = Request.Form["ShopName"].ToString();
            model.ShopAddress = Request.Form["ShopAddress"].ToString();
            model.City = Request.Form["City"].ToString();
            model.State = Request.Form["State"];
            model.MacAddress = Request.Form["MacAddress"].ToString();
            model.IsActive = Request.Form["IsActive"].ToString() == "true" || Request.Form["IsActive"].ToString() == "1" ? 1 : 0;
            if (model.ID > 0)
            {
                model.ModifiedDate = DateTime.Now;
            }
            else
            {
                model.CreatedDate = DateTime.Now;
            }

            _logger.LogInformation("==========Add Machine==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = await _unitOfWork.MachineRepository.Manage(model);
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
            _logger.LogInformation("==========Get Machine By ID==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.MachineRepository.GetByIdAsync(id);
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
                var result = await _unitOfWork.MachineRepository.Delete(id);
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
        [Route("GetMachineExcelFile")]
        public async Task<IActionResult> GetMachineExcelFile()
        {
            _logger.LogInformation("========== GetMachineExcelFile ==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            try
            {
                // Set the ExcelPackage.LicenseContext property
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var result = await _unitOfWork.MachineRepository.GetMachineExcelFile();
                List<ExcelMachineViewModel> excelMachineViewModelsList = result.ToList();

                if (excelMachineViewModelsList != null && excelMachineViewModelsList.Any())
                {
                    DataTable dataTable = CommonHelper.ToDataTable(excelMachineViewModelsList);

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