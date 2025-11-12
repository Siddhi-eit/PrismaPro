using Dapper;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using MDFusionLabHaute.Common;
using System.Reflection.PortableExecutable;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class SanitisingTrakingRepository : ISanitisingTrakingRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<SanitisingTrakingRepository> _logger;

        public SanitisingTrakingRepository(IConfiguration configuration, ILogger<SanitisingTrakingRepository> logger)
        {
            _configuration = configuration;
            this.connectionString = _configuration.GetConnectionString("ConnectionString"); ;
            _logger = logger;
        }

        public async Task<IEnumerable<SanitisingTrakingViewModel>> GetAllSanitisingTraking(GridSearch gridSearch)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_SanitisingTraking_Select] Procedure===========");
            var sql = $"[dbo].[usp_SanitisingTraking_Select]";
            var values = new { PageIndex = gridSearch.start, PageSize = gridSearch.length, SearchText = gridSearch.search, SortColumn = gridSearch.order, SortOrder = gridSearch.orderDir, MachineId = gridSearch.MachineID };
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<SanitisingTrakingViewModel>(sql, values, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(SanitisingTraking entity)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_SanitisingTraking_Upsert] Procedure===========");
            var sql = $"[dbo].[usp_SanitisingTraking_Upsert]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(sql, entity, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<ResponseObjectForAnything> Manage(SanitisingTrakingViewModel viewModel)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            var result = await InsertSanitization(new SanitisingTraking()
            {
                ID = viewModel.ID,
                UserID = viewModel.UserID,
                MachineID = viewModel.MachineID,
                CanisterID = viewModel.CanisterID,
                FusionLabNo = viewModel.FusionLabNo,
                DateSanitised = viewModel.DateSanitised,
                SetReminder = viewModel.SetReminder,
                CreatedBy = viewModel.CreatedBy,
                CreatedDate = DateTime.Now,
                ModifiedBy = viewModel.ModifiedBy,
                ModifiedDate = DateTime.Now,
                IsActive = viewModel.IsActive

            });
            responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
            if (result.ID == viewModel.ID)
            {
                responseObjectForAnything.ResultObjectID = result.ID;
                responseObjectForAnything.ResultMessage = AlertMessages.Message[200039];
                responseObjectForAnything.ResultObject = result;
            }
            else if (result.ID != viewModel.ID)
            {
                responseObjectForAnything.ResultObjectID = result.ID;
                responseObjectForAnything.ResultMessage = AlertMessages.Message[200038];
                responseObjectForAnything.ResultObject = result;
            }
            else
            {
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = AlertMessages.Message[300001];

            }
            return responseObjectForAnything;
        }

        public async Task<SanitisingTrakingViewModel> GetByIdAsync(int id)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_SanitisingTraking_GetByID] Procedure===========");
            var sql = $"[dbo].[usp_SanitisingTraking_GetByID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id };
                var result = await connection.QueryFirstAsync<SanitisingTrakingViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<string> SanitisingTrakingDelete(int id, int userId)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_SanitisingTraking_Delete] Store Procedure");
            var sql = $"[dbo].[usp_SanitisingTraking_Delete]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id, ModifiedBy = userId, ModifiedDate = DateTime.Now };
                var result = await connection.QuerySingleAsync<int>(sql, value, commandType: CommandType.StoredProcedure);
                return AlertMessages.Message[200040];
            }
        }

        public async Task<string> SetSanitizationDone(int userId, DateTime RefilledDate)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Sanitising_Done] Store Procedure");
            var sql = $"[dbo].[usp_Sanitising_Done]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = userId, RefilledDate = RefilledDate };
                var result = await connection.QuerySingleAsync<int>(sql, value, commandType: CommandType.StoredProcedure);
                return AlertMessages.Message[200038];
            }
        }

        public async Task<ResponseObjectForAnything> SetSanitizationSuccess(int ID, int userID, DateTime refilledDate, int machineID)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("==========Execute [dbo].[usp_Sanitising_Success] Store Procedure");
            var sql = $"[dbo].[usp_Sanitising_Success]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var values = new { ID = ID, UserID = userID, RefilledDate = refilledDate, MachineID = machineID };
                var result = await connection.QuerySingleAsync<int>(sql, values, commandType: CommandType.StoredProcedure);
                responseObjectForAnything.ResultObjectID = result;
                if (result > 0)
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[200038];
                }
                else if (result == -1)
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_EXISTS;
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[100020];
                }
                else if (result == 0)
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[4000152];
                }
                return responseObjectForAnything;
            }
        }

        public async Task<SanitisingTrakingViewModel> InsertSanitization(SanitisingTraking entity)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_SanitisingTraking_Upsert] Procedure===========");
            var sql = $"[dbo].[usp_SanitisingTraking_Upsert]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryFirstAsync<SanitisingTrakingViewModel>(sql, entity, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<ExcelSanitisingTrakingViewModel>> GetSanitisingTrakingExcelFile()
        {
            try
            {
                _logger.LogInformation("===========Execute [usp_Get_Excel_AllData_SanitisingTraking] Procedure===========");
                var sql = "[dbo].[usp_Get_Excel_AllData_SanitisingTraking]";
                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ExcelSanitisingTrakingViewModel>(sql, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetSanitisingTrakingExcelFile: {ex.Message}");
                throw; // Rethrow the exception to be handled at a higher level
            }
        }
    }
}