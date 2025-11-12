using Dapper;
using Microsoft.Extensions.Configuration;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using System.Data.SqlClient;
using MDFusionLabHaute.Domain.ViewModel;
using System.Data;
using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.ResponseObject;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }

        public async Task<IEnumerable<UsersViewModel>> GetUsers(GridSearch gridSearch)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_User_Select] Procedure===========");
            var sql = $"[dbo].[usp_User_Select]";
            var values = new { PageIndex = gridSearch.start, PageSize = gridSearch.length, SearchText = gridSearch.search, SortColumn = gridSearch.order, SortOrder = gridSearch.orderDir };
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<UsersViewModel>(sql, values, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<int> Insert(Users entity)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_User_Upsert] Procedure===========");
            var sql = $"[dbo].[usp_User_Upsert]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(sql, entity, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<string> Delete(int ID)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_User_Delete] Procedure===========");
            var sql = $"[dbo].[usp_User_Delete]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = ID, ModifiedBy = 1, ModifiedDate = DateTime.Now };
                var result = await connection.QuerySingleAsync<int>(sql, value, commandType: CommandType.StoredProcedure);
                return AlertMessages.Message[200040];
            }
        }

        public async Task<ResponseObjectForAnything> ManageUser(UsersViewModel users)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            var entity = new Users()
            {
                ID = users.ID,
                FirstName = users.FirstName,
                LastName = users.LastName,
                UserName = users.UserName,
                Email = users.Email,
                RoleID = users.RoleID,
                Password = users.Password,
                Phone = users.Phone,
                MDFusionLabNo = users.MDFusionLabNo,
                BachLotNo = users.BachLotNo,
                ConsultantID = users.ConsultantID,
                Country = users.Country,
                Shop = users.Shop,
                //ProfileImage = users.ProfileImage,
                IsActive = users.IsActive,
                CreatedBy = users.CreatedBy,
                CreatedDate = users.CreatedDate,
                ModifiedBy = users.ModifiedBy,
                ModifiedDate = users.ModifiedDate,
                //ResetCode = users.ResetCode,
                //ResetCodeExpiry = users.ResetCodeExpiry,
                MacAddress = ""
            };
            var result = await Insert(entity);
            responseObjectForAnything.ResultObjectID = result;
            responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
            if (result == -1)
                responseObjectForAnything.ResultMessage = AlertMessages.Message[100021];
            else if (result == -2)
                responseObjectForAnything.ResultMessage = AlertMessages.Message[100022];
            else if (result == -3)
                responseObjectForAnything.ResultMessage = AlertMessages.Message[100023];
            else if (result == users.ID)
                responseObjectForAnything.ResultMessage = AlertMessages.Message[200039];
            else
                responseObjectForAnything.ResultMessage = AlertMessages.Message[200038];
            return responseObjectForAnything;
        }

        public async Task<UsersViewModel> GetUserByID(int id)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_User_GetByID] Procedure===========");
            var sql = $"[dbo].[usp_User_GetByID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { UserID = id };
                var result = await connection.QueryFirstAsync<UsersViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<UsersViewModel>> BindUserDropdown()
        {
            _logger.LogInformation("===========Execute [dbo].[usp_User_Bind_Dropdown] Procedure===========");
            var sql = $"[dbo].[usp_User_Bind_Dropdown]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { UserID = 1 };
                var result = await connection.QueryAsync<UsersViewModel>(sql, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<Roles>> BindUsertypeDropdown()
        {
            _logger.LogInformation("===========Execute [dbo].[usp_Roles_Select] Procedure===========");
            var sql = $"[dbo].[usp_Roles_Select]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<Roles>(sql, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<Dropdown>> BindMDFusionLabDropdown(int id)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_MDFusionLab_Select] Procedure===========");
            var sql = $"[dbo].[usp_MDFusionLab_Select]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { UserID = id };
                var result = await connection.QueryAsync<Dropdown>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<ExcelUserViewModel>> GetUserExcelFile()
        {
            try
            {
                _logger.LogInformation("===========Execute [usp_Get_Excel_AllData_User] Procedure===========");
                var sql = "[dbo].[usp_Get_Excel_AllData_User]";
                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ExcelUserViewModel>(sql, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserExcelFile: {ex.Message}");
                throw; // Rethrow the exception to be handled at a higher level
            }
        }
    }
}