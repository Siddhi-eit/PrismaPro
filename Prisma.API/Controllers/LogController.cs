using Microsoft.AspNetCore.Mvc;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.ViewModel;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Concrete;
using MDFusionLabHaute.Domain.Entities;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        #region Declaration
        private readonly ILogger<LogController> _logger;
        private readonly ILogRepository _unitOfWork;
        #endregion

        public LogController(ILogger<LogController> logger, LogRepository unitOfWork)
        {
            _logger = (ILogger<LogController>?)logger;
            _unitOfWork = unitOfWork;
        }

        #region Actions
        [HttpPost]
        [Route("InsertLog")]
        public async Task<IActionResult> InsertLog()
        {
            MachineLog machineLog = new MachineLog();
            machineLog.UserID = Convert.ToInt32(Request.Form["id"]);
            machineLog.MachineID = Convert.ToInt32(Request.Form["id"]);
            machineLog.Operation = Request.Form["id"].ToString();
            machineLog.OperationDetails = Request.Form["id"].ToString();
            _logger.LogInformation("==========Insert Log==========");
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                var result = await _unitOfWork.Insert(machineLog);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                responseObjectForAnything.ResultObjectID = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //await _unitOfWork.ExceptionLog.InsertLog(ex);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = ex.Message;
            }
            return Ok(responseObjectForAnything);
        }
        #endregion
    }
}