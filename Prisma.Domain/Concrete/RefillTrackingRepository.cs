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
using System.Globalization;
using System.Reflection.PortableExecutable;
using static Dapper.SqlMapper;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class RefillTrackingRepository : IRefillTrackingRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<RefillTrackingRepository> _logger;

        public RefillTrackingRepository(IConfiguration configuration, ILogger<RefillTrackingRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }

        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RefillTrackingViewModel>> GetAll(GridSearch gridSearch)
        {
            _logger.LogInformation("========== Execute [dbo].[usp_RefillTracking_Select] Store Procedure ==========");
            var sql = $"[dbo].[usp_RefillTracking_Select]";
            var values = new { PageIndex = gridSearch.start, PageSize = gridSearch.length, SearchText = gridSearch.search, SortColumn = gridSearch.order, SortOrder = gridSearch.orderDir, MachineID = gridSearch.MachineID };
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<RefillTrackingViewModel>(sql, values, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<RefillTrackingViewModel> GetByIdAsync(int id)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_RefillTracking_GetByID] Store Procedure");
            var sql = $"[dbo].[usp_RefillTracking_GetByID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id };
                var result = await connection.QueryFirstAsync<RefillTrackingViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<RefillTrackingViewModel> GetScannerDetailByIdAsync(int id)
        {
            _logger.LogInformation("==============Execute [dbo].[usp_RefillTracking_GetByRefillTrackingID] Store Procedure");
            var sql = $"[dbo].[usp_RefillTracking_GetByRefillTrackingID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { RefillTrackingID = id };
                var reader = await connection.QueryFirstAsync<RefillTrackingViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return reader;
            }
        }

        public async Task<IEnumerable<CanSize>> GetCanSize()
        {
            _logger.LogInformation("===========Execute [dbo].[usp_CanSize_Select] Procedure===========");
            var sql = $"[dbo].[usp_CanSize_Select]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<CanSize>(sql, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<int> Insert(RefillTracking entity)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_RefillTracking_Select] Store Procedure==========");
            var sql = $"[dbo].[usp_RefillTracking_Upsert]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(sql, entity, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<RefillTrackingViewModel> InsertRefill(RefillTracking entity)
        {

            _logger.LogInformation("===========Execute [usp_RefillTracking_Upsert] Procedure===========");
            var sql = $"[dbo].[usp_RefillTracking_Upsert]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var reader = await connection.QueryFirstAsync<RefillTrackingViewModel>(sql, entity, commandType: CommandType.StoredProcedure);
                return reader;
            }
        }

        public async Task<ResponseObjectForAnything> Manage(RefillTrackingViewModel viewModel)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            var result = await InsertRefill(new RefillTracking()
            {
                ID = viewModel.ID,
                MachineID = viewModel.MachineID,
                CanisterID = viewModel.CanisterID,
                FusionLabNo = viewModel.FusionLabNo,
                DateFilled = viewModel.DateFilled,
                Quantity = viewModel.RefillML,
                LotNr = viewModel.LotNr,
                UnitID = viewModel.UnitID,
                CreatedBy = viewModel.CreatedBy,
                CreatedDate = DateTime.Now,
                ModifiedBy = viewModel.ModifiedBy,
                ModifiedDate = DateTime.Now,
                IsActive = viewModel.IsActive,
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

        public async Task<string> RefillTrakingDelete(int id, int userId)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_RefillTracking_Delete] Store Procedure");
            var sql = $"[dbo].[usp_RefillTracking_Delete]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id, ModifiedBy = userId, ModifiedDate = DateTime.Now };
                var result = await connection.QuerySingleAsync<int>(sql, value, commandType: CommandType.StoredProcedure);
                return AlertMessages.Message[200040];
            }
        }

        public async Task<ResponseObjectForAnything> SetRefillDone(int refillID, int userID, DateTime refilledDate,int machineID)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("========== Execute [dbo].[usp_Refill_Done] Store Procedure ==========");
            var sql = $"[dbo].[usp_Refill_Done]";
            var values = new { ID = refillID, UserID = userID, RefilledDate = refilledDate, MachineID = machineID };
            using (var connection = new SqlConnection(this.connectionString))
            {
                int result = await connection.QuerySingleAsync<Int32>(sql, values, commandType: CommandType.StoredProcedure);
                if (result > 0)
                {
                    responseObjectForAnything.ResultObjectID = result;
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

        public async Task<IEnumerable<ExcelRefillTrackingViewModel>> GetRefillTrackingExcelFile()
        {
            try
            {
                _logger.LogInformation("===========Execute [usp_Get_Excel_AllData_RefillTracking] Procedure===========");
                var sql = "[dbo].[usp_Get_Excel_AllData_RefillTracking]";
                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ExcelRefillTrackingViewModel>(sql, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetRefillTrackingExcelFile: {ex.Message}");
                throw; // Rethrow the exception to be handled at a higher level
            }
        }
    }
}