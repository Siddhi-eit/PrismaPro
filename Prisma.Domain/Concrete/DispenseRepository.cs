using Dapper;
using MDFusionLabHaute.Common;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using static Dapper.SqlMapper;
using static OfficeOpenXml.ExcelErrorValue;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class DispenseRepository : IDispenseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<DispenseRepository> _logger;
        public DispenseRepository(IConfiguration configuration, ILogger<DispenseRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }

        public async Task<ResponseObjectForAnything> CheckConsultantID(string consultantID, string userID)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Dispense_CheckConsultantID] Procedure===========");
            var sql = $"[dbo].[usp_Dispense_CheckConsultantID]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var values = new { ConsultantID = consultantID, UserID = userID };
                var result = await connection.QuerySingleAsync<string>(sql, values, commandType: CommandType.StoredProcedure);
                if (result != null)
                {
                    responseObjectForAnything.ResultObject = result;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                else
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                }
            }
            return responseObjectForAnything;
        }

        public async Task<ResponseObjectForAnything> CheckProductCode(string productCode)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Dispense_CheckProductCode] Procedure===========");
            var sql = $"[dbo].[usp_Dispense_CheckProductCode]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var values = new { ProductCode = productCode };
                var result = await connection.QuerySingleAsync<string>(sql, values, commandType: CommandType.StoredProcedure);
                if (result != null)
                {
                    responseObjectForAnything.ResultObject = result;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                else
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                }
            }
            return responseObjectForAnything;
        }

        public async Task<ResponseObjectForAnything> CheckTailoringCode(string tailoringCode, string dermaprofile)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Dispense_CheckTailoringCode] Procedure===========");

            try
            {

                var sql = $"[dbo].[usp_Dispense_CheckTailoringCode]";
                using (var connection = new SqlConnection(this.connectionString))
                {
                    var values = new { TailoringCode = tailoringCode, Dermaprofile = dermaprofile };
                    var result = await connection.QuerySingleAsync<string>(sql, values, commandType: CommandType.StoredProcedure);
                    if (result != null)
                    {
                        responseObjectForAnything.ResultObject = result;
                        responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    }
                    else
                    {
                        responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking tailoring code.");
                responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                responseObjectForAnything.ResultObject = ex.Message; // Or a custom error message
            }
            return responseObjectForAnything;
        }
        public async Task<ResponseObjectForAnything> GetSelectedEssence(string tailoringCode, string selectedEssence)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Dispense_GetSelectedEssence] Procedure===========");
            var sql = $"[dbo].[usp_Dispense_GetSelectedEssence]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var values = new { TailoringCode = selectedEssence, SelectedEssence = selectedEssence };
                var result = await connection.QuerySingleAsync<string>(sql, values, commandType: CommandType.StoredProcedure);
                if (result != null)
                {
                    responseObjectForAnything.ResultObject = result;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                else
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                }
            }
            return responseObjectForAnything;
        }

        public async Task<ResponseObjectForAnything> GetDispenseData(string tailoringCode, string? dermaprofile, int? essenceOption, string machineID)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                _logger.LogInformation("===========Execute [usp_Dispense_GetDispenseData] Procedure===========");
                var sql = $"[dbo].[usp_Dispense_GetDispenseData]";
                using (var connection = new SqlConnection(this.connectionString))
                {
                    var values = new { TailoringCode = tailoringCode, Dermaprofile = dermaprofile, EssenceOption = essenceOption, MachineID = machineID };
                    var reader = connection.QueryMultiple(sql, values, commandType: CommandType.StoredProcedure);
                    if (reader != null)
                    {
                        List<ProductWithFormulaViewModel> productWithFormulaViewModel = new List<ProductWithFormulaViewModel>();
                        productWithFormulaViewModel = reader.Read<ProductWithFormulaViewModel>().ToList();
                        responseObjectForAnything.ResultObject = productWithFormulaViewModel;
                        responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    }
                    else
                    {
                        responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return responseObjectForAnything;
        }

        public async Task<ResponseObjectForAnything> GetDispenseDataByProductCode(string ProductCode)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            try
            {
                _logger.LogInformation("===========Execute [usp_Dispense_GetDispenseDataByProductCode] Procedure===========");
                var sql = $"[dbo].[usp_Dispense_GetDispenseDataByProductCode]";
                using (var connection = new SqlConnection(this.connectionString))
                {
                    var values = new { ProductCode = ProductCode };
                    var reader = connection.QueryMultiple(sql, values, commandType: CommandType.StoredProcedure);
                    if (reader != null)
                    {
                        List<ProductWithFormulaViewModel> productWithFormulaViewModel = new List<ProductWithFormulaViewModel>();
                        productWithFormulaViewModel = reader.Read<ProductWithFormulaViewModel>().ToList();
                        responseObjectForAnything.ResultObject = productWithFormulaViewModel;
                        responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                    }
                    else
                    {
                        responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return responseObjectForAnything;
        }

        public async Task<IEnumerable<CanistersViewModel>> CheckCanisterIsEligibleForDispense(string CanisterCodeAndAmount, decimal? AmountToDispense)
        {
            _logger.LogInformation("===========Execute [usp_Check_Canister_Is_Eligible_For_Dispense] Procedure===========");
            var sql = $"[dbo].[usp_Check_Canister_Is_Eligible_For_Dispense]";
            var values = new { CanisterCodeAndAmount = CanisterCodeAndAmount, AmountToDispense = AmountToDispense };
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<CanistersViewModel>(sql, values, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DispenserSelectViewModel>> GetAll(GridSearch gridSearch)
        {
            _logger.LogInformation("===========Execute [usp_DispenseLog_Select] Procedure===========");
            var sql = $"[dbo].[usp_DispenseLog_Select]";
            var values = new { PageIndex = gridSearch.start, PageSize = gridSearch.length, SearchText = gridSearch.search, SortColumn = gridSearch.order, SortOrder = gridSearch.orderDir, MachineID = gridSearch.MachineID };
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<DispenserSelectViewModel>(sql, values, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public ResponseObjectForAnything GetAllDispenseDataAsync()
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Get_AllData_Dispense] Procedure===========");
            var sql = $"[dbo].[usp_Get_AllData_Dispense]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var reader = connection.QueryMultiple(sql, null, commandType: CommandType.StoredProcedure);
                if (reader != null)
                {
                    DispenseViewModel dispenseViewModel = new DispenseViewModel();
                    dispenseViewModel.productCodeList = reader.Read<ProductCodeList>().ToList();
                    dispenseViewModel.collectionList = reader.Read<CollectionList>().ToList();
                    dispenseViewModel.productNameList = reader.Read<ProductNameList>().ToList();
                    //dispenseViewModel.productFormulaList = reader.Read<ProductFormulaList>().ToList();
                    dispenseViewModel.canSize = reader.Read<CanSize>().ToList();
                    dispenseViewModel.unitList = reader.Read<Unit>().ToList();
                    responseObjectForAnything.ResultObject = dispenseViewModel;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                return responseObjectForAnything;
            }
        }

        public ResponseObjectForAnything GetDispenseFormulaData()
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Get_Dispense_FormulaData] Procedure===========");

            var sql = $"[dbo].[usp_Get_Dispense_FormulaData]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var reader = connection.QueryMultiple(sql, null, commandType: CommandType.StoredProcedure);
                if (reader != null)
                {
                    DispenseViewModel dispenseViewModel = new DispenseViewModel();
                    dispenseViewModel.productFormulaList = reader.Read<ProductFormulaList>().ToList();
                    dispenseViewModel.canSize = reader.Read<CanSize>().ToList();
                    responseObjectForAnything.ResultObject = dispenseViewModel;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                return responseObjectForAnything;
            }
        }

        public ResponseObjectForAnything GetProductionModeData()
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Get_Dispense_FormulaData] Procedure===========");

            var sql = $"[dbo].[usp_Get_Dispense_FormulaData]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var reader = connection.QueryMultiple(sql, null, commandType: CommandType.StoredProcedure);
                if (reader != null)
                {
                    DispenseViewModel dispenseViewModel = new DispenseViewModel();
                    dispenseViewModel.productFormulaList = reader.Read<ProductFormulaList>().ToList();
                    dispenseViewModel.canSize = reader.Read<CanSize>().ToList();
                    responseObjectForAnything.ResultObject = dispenseViewModel;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                return responseObjectForAnything;
            }
        }

        public ResponseObjectForAnything GetDispenseDataByFilter(string[] items)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Get_AllData_Dispense_ByFilter] Procedure===========");
            var sql = $"[dbo].[usp_Get_AllData_Dispense_ByFilter]";
            var productCode = "";
            var collection = "";
            var productName = ((String.IsNullOrEmpty(items[0])) ? null : items[0]);
            using (var connection = new SqlConnection(this.connectionString))
            {
                var values = new { ProductCode = productCode, Collection = collection, ProductName = productName };
                var reader = connection.QueryMultiple(sql, values, commandType: CommandType.StoredProcedure);
                if (reader != null)
                {
                    DispenseViewModel dispenseViewModel = new DispenseViewModel();
                    dispenseViewModel.productFormulaList = reader.Read<ProductFormulaList>().ToList();
                    responseObjectForAnything.ResultObject = dispenseViewModel;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                return responseObjectForAnything;
            }
        }

        public async Task<int> Insert(DispenseLog entity)
        {
            _logger.LogInformation("===========Execute [usp_DispenseLog_Insert] Procedure===========");
            var sql = $"[dbo].[usp_DispenseLog_Insert]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(sql, entity, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public string GenerateLotNumber(int year, int month, int day, int count)
        {
           
            string yearStr = year.ToString().Substring(3, 1);

            string monthStr = month.ToString("D2");
            string dayStr = day.ToString("D2");
            
            string lotNumber = $"{yearStr}{monthStr}{dayStr}{count}";

            return lotNumber;
        }

        public async Task<ResponseObjectForAnything> Manage(DispenseLogNew model)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("=========== Set DispenseLog Enity ===========");
            DateTime now = DateTime.Now;
            int year = now.Year;
            int month = now.Month;
            int day = now.Day;
            int count = 0;
            var sql = $"[dbo].[usp_IsRefilled_Select]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var IsRefillResult = Convert.ToInt32(await connection.QuerySingleAsync<string>(sql, commandType: CommandType.StoredProcedure));
                if(IsRefillResult > 0)
                {
                    count = IsRefillResult + 1;
                }
                var _BatchLOTNo = GenerateLotNumber(year, month, day, count);
                var result = await Insert(new DispenseLog()
                {
                    UserID = model.UserID,
                    MachineID = model.MachineID,
                    ProductCode = model.ProductCode,
                    AmountToDispense = model.AmountToDispense,
                    DispensationsNumber = (int?)model.DispensationsNumber,
                    AmountToDispenseUnitId = model.AmountToDispenseUnitId,
                    ComponentUnitId = model.ComponentUnitId,
                    ComponentNames = model.ComponentNames.Trim(),
                    ComponentAmounts = model.ComponentAmounts.Trim(),
                    IsDispense = model.IsDispense,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = String.IsNullOrEmpty(model.ModifiedBy.ToString()) ? model.CreatedBy : model.ModifiedBy,
                    ModifiedDate = DateTime.Now,
                    MACHINE_REG_NO = model.MACHINE_REG_NO,
                    COUNTRY = model.COUNTRY,
                    SHOP = model.SHOP,
                    DATE = model.DATE,
                    TIME_ID_ENTERED = model.TIME_ID_ENTERED,
                    CONSULTANT_ID = model.CONSULTANT_ID,
                    DERMAPROFILE = model.DERMAPROFILE,
                    TAILORING_CODE = model.TAILORING_CODE,
                    ESSENCE = model.ESSENCE,

                    PRICE = model.PRICE,
                    BACH_LOT_NO = _BatchLOTNo,
                    TIME_DISPENSED = model.TIME_DISPENSED,
                    DispanseQuantity = model.dispenseQuantity
                });
                responseObjectForAnything.ResultObjectID = result;
                DispenseResultObject dispenseResultObject = new DispenseResultObject();
                dispenseResultObject.LotNr = _BatchLOTNo;
                dispenseResultObject.Date = model.DATE;
                responseObjectForAnything.ResultObject = dispenseResultObject;

                if (result > 0)
                {
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[200035];
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                else
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                    responseObjectForAnything.ResultMessage = AlertMessages.Message[300001];
                }
            }

            return responseObjectForAnything;
        }

        public ResponseObjectForAnything CheckIsCanisterExists(string UserID, string ComponentNames)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Check_canisters] Procedure===========");
            var sql = $"[dbo].[usp_Check_canisters]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var values = new { UserID = UserID, ComponentNames = ComponentNames, CreatedDate = DateTime.Now };
                var reader = connection.ExecuteScalar(sql, values, commandType: CommandType.StoredProcedure);
                if (reader != null)
                {
                    object NotExistsComponentNames = reader;
                    responseObjectForAnything.ResultObject = NotExistsComponentNames;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                return responseObjectForAnything;
            }
        }
        public async Task<ResponseObjectForAnything> CheckDermaprofileData(string dermaprofile)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [usp_Dispense_CheckDermaprofileData] Procedure===========");
            var sql = $"[dbo].[usp_Dispense_CheckDermaprofileData]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var values = new { Dermaprofile = dermaprofile };
                var result = await connection.QueryAsync<Dermaprofile>(sql, values, commandType: CommandType.StoredProcedure);
                if (result != null && result.Count() > 0 && result.First().ID != 0)
                {
                    responseObjectForAnything.ResultObject = result;
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                }
                else
                {
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_ERROR;
                }
            }
            return responseObjectForAnything;
        }

        public async Task<IEnumerable<ExcelDispenseViewModel>> GetDispenseExcelFile(int machineID)
        {
            try
            {
                _logger.LogInformation("===========Execute [usp_Get_Excel_AllData_Dispense] Procedure===========");
                var sql = "[dbo].[usp_Get_Excel_AllData_Dispense]";
                var parameters = new { MachineID = machineID };

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ExcelDispenseViewModel>(sql, parameters, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDispenseExcelFile: {ex.Message}");
                throw; // Rethrow the exception to be handled at a higher level
            }
        }
    }
}