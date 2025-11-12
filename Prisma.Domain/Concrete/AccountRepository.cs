using Dapper;
using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class AccountRepository : IAccountRepository
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<AccountRepository> _logger;
        #endregion

        #region Constructor
        public AccountRepository(IConfiguration configuration, ILogger<AccountRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }
        #endregion

        #region Methods
        public async Task<UsersViewModel> SignInWithEmailAndPassword(Users users)
        {
            _logger.LogInformation("==========Execute [dbo].[ups_SignIn_With_Email_And_Password] Store Procedure==========");
            var sql = $"[dbo].[ups_SignIn_With_Email_And_Password]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { Email = users.Email, Password = users.Password };
                var result = await connection.QueryFirstAsync<UsersViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<UsersViewModel> SignInWithEmailAndPasswordDesktop(Users users)
        {
            _logger.LogInformation("==========Execute [dbo].[ups_SignIn_With_Email_And_Password_Desktop] Store Procedure==========");
            var sql = $"[dbo].[ups_SignIn_With_Email_And_Password_Desktop]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { Email = users.Email, Password = users.Password, MacAddress = users.MacAddress };
                var result = await connection.QueryFirstAsync<UsersViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<ResponseObjectForAnything> GetDesktopDataByID(int id)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("==========Execute [dbo].[ups_User_DesktopData_GetByID] Store Procedure==========");
            var sql = $"[dbo].[ups_User_DesktopData_GetByID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id };
                var reader = await connection.QueryMultipleAsync(sql, value, commandType: CommandType.StoredProcedure);
                if (reader != null)
                {
                    DesktopAlertViewModel desktopAlertViewModel = new DesktopAlertViewModel();
                    desktopAlertViewModel.refillAlertViewModelList = reader.Read<RefillAlertViewModel>().ToList();
                    desktopAlertViewModel.sanitizationAlertViewModelList = reader.Read<sanitizationAlertViewModel>().ToList();
                    responseObjectForAnything.ResultObject = desktopAlertViewModel;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                return responseObjectForAnything;
            }

        }
            #endregion
    }
}