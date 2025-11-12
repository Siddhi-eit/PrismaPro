using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MDFusionLabHaute.Common;
using Dapper;
using System.Net.Mail;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class MachineRepository : IMachineRepository
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<MachineRepository> _logger;
        #endregion

        #region Constructor
        public MachineRepository(IConfiguration configuration, ILogger<MachineRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }
        #endregion

        #region Constructor
        public async Task<ResponseObjectForAnything> SaveMachineProfile(Machine machine)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("==========Execute [dbo].[ups_UpsertMachineProfile] Store Procedure==========");
            var sql = $"[dbo].[ups_UpsertMachineProfile]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = machine.ID, MachineRegNo = machine.MachineRegNo, ShopName = machine.ShopName, ShopAddress = machine.ShopAddress, MacAddress = machine.MacAddress, City = machine.City, State = machine.State };
                var result = await connection.ExecuteScalarAsync(sql, value, commandType: CommandType.StoredProcedure);
                if (result != null)
                {
                    responseObjectForAnything.ResultObjectID = Convert.ToInt32(result);
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
            }
            return responseObjectForAnything;
        }

        public async Task<IEnumerable<Machine>> BindMachineDropdown(int id)
        {
            _logger.LogInformation("===========Execute [dbo].[usp_Machine_Bind_Dropdown] Procedure===========");
            var sql = $"[dbo].[usp_Machine_Bind_Dropdown]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { UserID = id };
                var result = await connection.QueryAsync<Machine>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<string> Delete(int id)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Machine_Delete] Store Procedure");
            var sql = $"[dbo].[usp_Machine_Delete]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id, ModifiedDate = DateTime.Now };
                var result = await connection.QuerySingleAsync<int>(sql, value, commandType: CommandType.StoredProcedure);
                return AlertMessages.Message[200040];
            }
        }

        public async Task<IEnumerable<MachineViewModel>> GetAllAsync(GridSearch gridSearch)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Machine_Select] Store Procedure");
            var sql = $"[dbo].[usp_Machine_Select]";
            var values = new { PageIndex = gridSearch.start, PageSize = gridSearch.length, SearchText = gridSearch.search, SortColumn = gridSearch.order, SortOrder = gridSearch.orderDir, UserID = gridSearch.UserID };
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<MachineViewModel>(sql, values, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<MachineViewModel> GetByIdAsync(int id)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Machine_GetByID] Store Procedure");
            var sql = $"[dbo].[usp_Machine_GetByID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id };
                var result = await connection.QueryFirstAsync<MachineViewModel>(sql, value, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<int> Insert(Machine entity)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Machine_Upsert] Store Procedure");
            var sql = $"[dbo].[usp_Machine_Upsert]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(sql, entity, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<ResponseObjectForAnything> Manage(MachineViewModel model)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            var enity = new Machine()
            {
                ID = model.ID,
                MachineRegNo = model.MachineRegNo,
                ShopName = model.ShopName,
                ShopAddress = model.ShopAddress,
                City = model.City,
                State = model.State,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                MacAddress = model.MacAddress,
                IsActive = model.IsActive
            };
            var result = await Insert(enity);
            responseObjectForAnything.ResultObjectID = result;
            if (result == -1)
            {
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultMessage = $"Machine {AlertMessages.Message[100020]}";
            }
            else
            {
                responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                if (result == -1)
                    responseObjectForAnything.ResultMessage = $"code {AlertMessages.Message[100020]}";
                else if (result == model.ID)
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[200039];
                else
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[200038];
            }
            return responseObjectForAnything;
        }

        public async Task<IEnumerable<ExcelMachineViewModel>> GetMachineExcelFile()
        {
            try
            {
                _logger.LogInformation("===========Execute [usp_Get_Excel_AllData_Machine] Procedure===========");
                var sql = "[dbo].[usp_Get_Excel_AllData_Machine]";
                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ExcelMachineViewModel>(sql, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetMachineExcelFile: {ex.Message}");
                throw; // Rethrow the exception to be handled at a higher level
            }
        }
        #endregion
    }
}
