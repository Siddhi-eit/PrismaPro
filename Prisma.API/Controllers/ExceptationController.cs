using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MDFusionLabHaute.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptationController : ControllerBase
    {
        private readonly ILogger<ExceptationController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ExceptationController(ILogger<ExceptationController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ExceptationLogForDesktop")]
        public async Task<IActionResult> ExceptationLogForDesktop(String[] input)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("==========Error==========");
            try
            {
                ExceptionLog exception = new ExceptionLog();
                
                exception.ExceptionMessage = input[0];
                exception.ExceptionSource = input[1];
                exception.CreatedDate = DateTime.Now;
                exception.Createdby = exception.Createdby;
                await _unitOfWork.ExceptionLog.InsertExceptionLog(exception.ExceptionMessage, exception.ExceptionSource);
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
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
    }
}
