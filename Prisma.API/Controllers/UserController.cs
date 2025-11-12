using Microsoft.AspNetCore.Mvc;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.ViewModel;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Common;
using OfficeOpenXml;
using System.Data;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Declaration
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        #region Actions
        [HttpPost]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            _logger.LogInformation("==========Get User By Id==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Users.GetUserByID(id);
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
        [Route("GetUserGridResult")]
        public async Task<IActionResult> GetAllUsers()
        {
            GridSearch gridSearch = new GridSearch();
            gridSearch.start = Convert.ToInt32(Request.Form["start"]);
            gridSearch.length = Convert.ToInt32(Request.Form["length"]);
            gridSearch.draw = Convert.ToInt32(Request.Form["draw"]);
            gridSearch.search = Request.Form["search"].ToString();
            gridSearch.order = Convert.ToInt32(Request.Form["order"]);
            gridSearch.orderDir = Request.Form["orderDir"].ToString();
            _logger.LogInformation("==========Get All Users==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Users.GetUsers(gridSearch);
                var totalRecord = result.Count() != 0 ? Convert.ToInt32(result.FirstOrDefault().TotalRecord) : 0;
                GridResultObject gridResultObject = new GridResultObject();
                gridResultObject.data = result;
                gridResultObject.currentPage = gridSearch.start;
                gridResultObject.pageSize = gridSearch.length;
                gridResultObject.status = true;
                gridResultObject.totalItem = totalRecord;
                gridResultObject.totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRecord) / Convert.ToDecimal(gridSearch.length)));

                _logger.LogInformation($"Total User:{result.Count()}");
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
        [Route("UpsertUser")]
        public async Task<IActionResult> Post()
        {
            UsersViewModel usersViewModel = new UsersViewModel();
            usersViewModel.ID = Convert.ToInt32(Request.Form["ID"]);
            usersViewModel.FirstName = Request.Form["FirstName"].ToString();
            usersViewModel.LastName = Request.Form["LastName"].ToString();
            usersViewModel.UserName = Request.Form["UserName"].ToString();
            usersViewModel.Email = Request.Form["Email"].ToString();
            usersViewModel.Password = Request.Form["Password"].ToString();
            usersViewModel.Phone = Request.Form["Phone"].ToString();
            usersViewModel.RoleID = Convert.ToInt32(Request.Form["RoleID"]);
            usersViewModel.MDFusionLabNo = Request.Form["MDFusionLabNo"].ToString();
            usersViewModel.BachLotNo = Request.Form["bachLotNo"].ToString();
            usersViewModel.ConsultantID = Request.Form["consultantID"].ToString();
            usersViewModel.Country = Request.Form["country"].ToString();
            usersViewModel.Shop = Request.Form["shop"].ToString();
            usersViewModel.IsActive = Request.Form["IsActive"].ToString() == "true" || Request.Form["IsActive"].ToString() == "1" ? true : false;
            //usersViewModel.Password = Helper.Security.Hash(usersViewModel.Password);
            if (usersViewModel.ID > 0)
            {
                usersViewModel.ModifiedDate = DateTime.Now;
                usersViewModel.ModifiedBy = Convert.ToInt32(Request.Form["UserID"]);
            }
            else
            {
                usersViewModel.Password = Helper.Security.Hash(usersViewModel.Password);
                usersViewModel.CreatedDate = DateTime.Now;
                usersViewModel.CreatedBy = Convert.ToInt32(Request.Form["UserID"]);
            }
            _logger.LogInformation("==========Manage Users==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                responseObjectForAnything = await _unitOfWork.Users.ManageUser(usersViewModel);
                _logger.LogInformation($"{responseObjectForAnything.ResultObjectID} - {responseObjectForAnything.ResultMessage}");
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
            _logger.LogInformation("==========Delete Users==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Users.Delete(id);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObjectID = id;
                responseObjectForAnything.ResultObject = result;
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
        [Route("BindDropdownRolewise")]
        public async Task<IActionResult> BindDropdownRolewise()
        {
            _logger.LogInformation("==========Get User By Id==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Users.BindUserDropdown();
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
        [Route("BindDropdownUsertype")]
        public async Task<IActionResult> BindDropdownUsertype()
        {
            _logger.LogInformation("==========Bind Dropdown User type==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Users.BindUsertypeDropdown();
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
        [Route("BindDropdownMDFusionLab")]
        public async Task<IActionResult> BindDropdownMDFusionLab()
        {
            _logger.LogInformation("==========Bind Dropdown MDFusionLabNo==========");
            int id = Convert.ToInt32(Request.Form["id"]);
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Users.BindMDFusionLabDropdown(id);
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
        [Route("GetUserExcelFile")]
        public async Task<IActionResult> GetUserExcelFile()
        {
            _logger.LogInformation("========== GetUserExcelFile ==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();

            try
            {
                // Set the ExcelPackage.LicenseContext property
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var result = await _unitOfWork.Users.GetUserExcelFile();
                List<ExcelUserViewModel> excelUserViewModelsList = result.ToList();

                if (excelUserViewModelsList != null && excelUserViewModelsList.Any())
                {
                    DataTable dataTable = CommonHelper.ToDataTable(excelUserViewModelsList);

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
