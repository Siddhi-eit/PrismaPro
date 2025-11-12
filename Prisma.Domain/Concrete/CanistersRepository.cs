using Dapper;
using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class CanistersRepository : ICanistersRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<CanistersRepository> _logger;

        public CanistersRepository(IConfiguration configuration, ILogger<CanistersRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }

        public async Task<IEnumerable<Unit>> BindDispenseUnitDropdown()
        {
            _logger.LogInformation("===========Execute [dbo].[usp_Unit_Select] Procedure===========");
            var sql = $"[dbo].[usp_Unit_Select]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<Unit>(sql, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<string> Delete(int id)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Canisters_Delete] Store Procedure");
            var sql = $"[dbo].[usp_Canisters_Delete]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id, ModifiedBy = 1, ModifiedDate = DateTime.Now };
                var result = await connection.QuerySingleAsync<int>(sql, value, commandType: CommandType.StoredProcedure);
                return AlertMessages.Message[200040];
            }
        }

        public async Task<IEnumerable<CanistersViewModel>> GetAllAsync(GridSearch gridSearch)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Canister_Select] Store Procedure");
            var sql = $"[dbo].[usp_Canister_Select]";
            var values = new { PageIndex = gridSearch.start, PageSize = gridSearch.length, SearchText = gridSearch.search, SortColumn = gridSearch.order, SortOrder = gridSearch.orderDir, UserID = gridSearch.UserID, MachineID = gridSearch.MachineID };
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<CanistersViewModel>(sql, values, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<CanistersViewModel> GetByIdAsync(int id)  
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Canisters_GetByID] Store Procedure");
            var sql = $"[dbo].[usp_Canisters_GetByID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id };
                var result = await connection.QueryFirstAsync<CanistersViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<CanistersViewModel>> getByUserIdAsync(int machineID)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_Canisters_GetByUserID] Procedure===========");
            var sql = $"[dbo].[usp_Canisters_GetByUserID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { MachineID = machineID };
                var result = await connection.QueryAsync<CanistersViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<Dropdown>> BindProductDropdown()
        {
            _logger.LogInformation("===========Execute [dbo].[usp_Dropdown_GetProduct] Procedure===========");
            var sql = $"[dbo].[usp_Dropdown_GetProduct]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new {};
                var result = await connection.QueryAsync<Dropdown>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<int> Insert(Canisters entity)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Canisters_Upsert] Store Procedure");
            var sql = $"[dbo].[usp_Canisters_Upsert]";
            using(var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(sql,entity,commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<ResponseObjectForAnything> Manage(CanistersViewModel model)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            var enity = new Canisters()
            {
                ID = model.ID,
                CanisterLookupId = model.CanisterLookupId,
                MaximumAmount = model.MaximumAmount,
                CurrentAmount = model.CurrentAmount,
                MinimumAmount = model.MinimumAmount,
                WarningAmount = model.WarningAmount,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                ModifiedBy = model.ModifiedBy,
                ModifiedDate = model.ModifiedDate,
                UserID = model.UserID,
                UnitID = model.UnitID,
                IsActive = model.IsActive,
                MachineID = model.MachineID
            };
            var result = await Insert(enity);
            responseObjectForAnything.ResultObjectID = result;
            responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
            if (result == -1)
                responseObjectForAnything.ResultMessage = $"code {AlertMessages.Message[100020]}";
            else if (result == -2)
                responseObjectForAnything.ResultMessage = $"Name {AlertMessages.Message[100020]}";
            else if (result == model.ID)
                responseObjectForAnything.ResultMessage = AlertMessages.Message[200039];
            else
                responseObjectForAnything.ResultMessage = AlertMessages.Message[200038];
            return responseObjectForAnything;
        }

        public async Task<IEnumerable<ExcelCanisterViewModel>> GetCanisterExcelFile()
        {
            try
            {
                _logger.LogInformation("===========Execute [usp_Get_Excel_AllData_Canisters] Procedure===========");
                var sql = "[dbo].[usp_Get_Excel_AllData_Canisters]";
                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ExcelCanisterViewModel>(sql, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCanisterExcelFile: {ex.Message}");
                throw; // Rethrow the exception to be handled at a higher level
            }
        }

        public async Task<IEnumerable<CanistersLookupViewModel>> GetCanisterLookup()
        {
            _logger.LogInformation("===========Execute [dbo].[usp_CanistersLookup_All_Select] Procedure===========");
            var sql = $"[dbo].[usp_CanistersLookup_All_Select]";
            using (var connection = new SqlConnection(this.connectionString))
                    {
                var result = await connection.QueryAsync<CanistersLookupViewModel>(sql, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
