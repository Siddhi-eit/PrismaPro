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
    public class CanisterLookupRepository : ICanisterLookupRepository
    {
        public readonly IConfiguration _configuration;
        public readonly string connectionString;
        public readonly ILogger<CanisterLookupRepository> _logger;

        public CanisterLookupRepository(IConfiguration configuration, ILogger<CanisterLookupRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }

        public async Task<IEnumerable<CanistersLookupViewModel>> GetAllAsync(GridSearch gridSearch) 
        {
            _logger.LogInformation("============ Execute [dbo].[usp_CanisterLookup_Select] ===========");
            var sql = $"[dbo].[usp_CanisterLookup_Select]";
            var value = new { PageIndex = gridSearch.start, PageSize = gridSearch.length, SearchText = gridSearch.search, SortColumn = gridSearch.order, SortOrder = gridSearch.orderDir, UserID = gridSearch.UserID, MachineID = gridSearch.MachineID };
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<CanistersLookupViewModel>(sql, value, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<string> Delete(int id)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_CanistersLookup_Delete] Store Procedure");
            var sql = $"[dbo].[usp_CanistersLookup_Delete]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id, ModifiedBy = 1, ModifiedDate = DateTime.Now };
                var result = await connection.QuerySingleAsync<int>(sql, value, commandType: CommandType.StoredProcedure);
                    return result.ToString();
            }
        }
        public async Task<int> Insert(CanisterLookup entity)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_CanistersLookup_Upsert] Store Procedure");
            var sql = $"[dbo].[usp_CanistersLookup_Upsert]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(sql, entity, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<ResponseObjectForAnything> Manage(CanistersLookupViewModel model)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            var enity = new CanisterLookup()
            {
                ID = model.ID,
                Name = model.Name,
                CanisterCode = model.CanisterCode,
                SKU = model.SKU,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                ModifiedBy = model.ModifiedBy,
                ModifiedDate = model.ModifiedDate,
                IsActive = model.IsActive
            };
            var result = await Insert(enity);
            responseObjectForAnything.ResultObjectID = result;
            responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
            if (result == -1)
                responseObjectForAnything.ResultMessage = $"code {AlertMessages.Message[100020]}";
            else if (result == model.ID)
                responseObjectForAnything.ResultMessage = AlertMessages.Message[200039];
            else
                responseObjectForAnything.ResultMessage = AlertMessages.Message[200038];
            return responseObjectForAnything;
        }

        public async Task<CanistersLookupViewModel> GetByIdAsync(int id)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_CanistersLookup_GetByID] Store Procedure");
            var sql = $"[dbo].[usp_CanistersLookup_GetByID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id };
                var result = await connection.QueryFirstAsync<CanistersLookupViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<ExcelCanisterLookupViewModel>> GetCanisterLookupExcelFile()
        {
            try
            {
                _logger.LogInformation("===========Execute [usp_Get_Excel_AllData_CanistersLookup] Procedure===========");
                var sql = "[dbo].[usp_Get_Excel_AllData_CanistersLookup]";
                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ExcelCanisterLookupViewModel>(sql, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCanisterExcelFile: {ex.Message}");
                throw; // Rethrow the exception to be handled at a higher level
            }
        }
    }
}
