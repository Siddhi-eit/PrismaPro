using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MDFusionLabHaute.API.Helper;
using Microsoft.AspNetCore.Authorization;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        JWTHelper _jwtHelper = null;
        string _encryptedPassword = string.Empty;
        #endregion

        #region Constructor
        public AccountController(ILogger<AccountController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
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
        [Route("SignInWithEmailAndPassword")]
        public async Task<IActionResult> SignInWithEmailAndPassword()
        {
            Users users = new Users();
            users.Email = Request.Form["Email"].ToString();
            users.Password = Request.Form["Password"].ToString();
            users.Password = Helper.Security.Hash(users.Password);
            var IsAdmin = Request.Form["IsAdmin"].ToString();

            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("==========Sign In==========");
            try
            {
                var result = await _unitOfWork.Account.SignInWithEmailAndPassword(users);
                if (result.ID > 0)
                {
                    _logger.LogInformation("==========Sign In Successfully==========");
                    responseObjectForAnything.ResultObject = result;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    var tokenString = _jwtHelper.GenerateJSONWebToken(result.UserName, result.RoleName);

                    var filePath = "http://" + Request.Host + '/' + _configuration.GetSection("User");
                    result.ProfileImage = filePath + result.ProfileImage;
                    result.AccessToken = tokenString;

                    return Ok(result);
                }
                else if (result.ID == -1)
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

        [AllowAnonymous]
        [HttpPost]
        [Route("SignInWithEmailAndPasswordDesktop")]
        public async Task<IActionResult> SignInWithEmailAndPasswordDesktop(Users users)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            users.Password = Helper.Security.Hash(users.Password);
            _logger.LogInformation("==========Sign In Desktop==========");
            try
            {
                var result = await _unitOfWork.Account.SignInWithEmailAndPasswordDesktop(users);
                if (result.ID > 0)
                {
                    _logger.LogInformation("==========Sign In Successfully==========");
                    responseObjectForAnything.ResultObject = result;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    var tokenString = _jwtHelper.GenerateJSONWebToken(result.UserName, result.RoleName);

                    var filePath = "http://" + Request.Host.Value + '/' + "upload/user";
                    result.ProfileImage = filePath + result.ProfileImage;
                    result.AccessToken = tokenString;
                    responseObjectForAnything.ResultObject = result;
                    return Ok(responseObjectForAnything);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("GetDesktopDataByID")]
        public async Task<IActionResult> GetDesktopDataByID(Users users)
        {
            ResponseObjectForAnything responseObjectForAnything;
            _logger.LogInformation("==========Get Desktop Data By ID==========");
            try
            {
                responseObjectForAnything = await _unitOfWork.Account.GetDesktopDataByID(users.ID);
                if (responseObjectForAnything.ResultCode == Constants.RESPONSE_SUCCESS)
                {
                    _logger.LogInformation("==========Get Desktop Data Successfully==========");
                    return Ok(responseObjectForAnything);
                }
            }
            catch (Exception ex)
            {
                responseObjectForAnything = new ResponseObjectForAnything();
                _logger.LogError(ex.Message);
                await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }
        #endregion
    }
}
