using MDFusionLabHaute.Domain.ResponseObject;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dapper;
using MDFusionLabHaute.Domain.ViewModel;
using MDFusionLabHaute.Common;



namespace MDFusionLabHaute.Domain.Concrete
{
    public class FormulaRepository: IFormulaRepository
    {
        #region Declarations
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<FormulaRepository> _logger;
        #endregion

        #region Constructor
        public FormulaRepository(IConfiguration configuration, ILogger<FormulaRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }
        #endregion
        public async Task<ResponseObjectForAnything> SaveFormulaProfile(Formula formula)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("==========Execute [dbo].[usp_ProductFormula_Upsert] Store Procedure==========");
            var sql = $"[dbo].[usp_ProductFormula_Upsert]";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var colorDetailTable = new DataTable();
                colorDetailTable.Columns.Add("ColorCode", typeof(string));
                colorDetailTable.Columns.Add("Amount", typeof(decimal));
                colorDetailTable.Columns.Add("UnitID", typeof(int)); // Add UnitID
                colorDetailTable.Columns.Add("CreatedBy", typeof(int)); // Add CreatedBy
                colorDetailTable.Columns.Add("ModifiedBy", typeof(int)); // Add ModifiedBy

                foreach (var color in formula.colorDetail)
                {
                    colorDetailTable.Rows.Add(color.ColorCode, color.DispenseAmount);
                }

                var parameters = new
                {
                    formulaID = formula.formulaID,
                    ProductCode = formula.ProductCode,
                    Amount = formula.Amount,
                    colorDetail = colorDetailTable.AsTableValuedParameter("dbo.ColorFormulaType"),
                    CreatedByID = 1,
                    ModifiedByID = 1,
                    UnitID = 2,
                };

                var result = await connection.ExecuteScalarAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                if (result != null)
                {
                    responseObjectForAnything.ResultObjectID = Convert.ToInt32(result);
                    responseObjectForAnything.ResultCode = Constants.RESPONSE_SUCCESS;
                   if(parameters.formulaID == -1)
                    {
                        responseObjectForAnything.ResultMessage = $"code {AlertMessages.Message[100020]}";
                    }
                    else if(formula.formulaID == 0)
                    {
                        responseObjectForAnything.ResultMessage = AlertMessages.Message[200038];
                    }
                    else if (parameters.formulaID == formula.formulaID)
                    {
                        responseObjectForAnything.ResultMessage = AlertMessages.Message[200039];
                    }
                }
                
            }
            return responseObjectForAnything;
        }



        public async Task<IEnumerable<FormulaViewModel>> GetAllFormula(GridSearch gridSearch)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Formula_Select] Store Procedure");
            var sql = $"[dbo].[usp_Formula_Select]";
            var values = new { PageIndex = gridSearch.start, PageSize = gridSearch.length, SearchText = gridSearch.search, SortColumn = gridSearch.order, SortOrder = gridSearch.orderDir};
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QueryAsync<FormulaViewModel>(sql, values, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<FormulaViewModel> GetByID(int id)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Formula_SelectByID] Store Procedure");
            var sql = $"[dbo].[usp_Formula_SelectByID]";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id };
                var result = await connection.QueryFirstAsync<FormulaViewModel>(sql, value, commandType: CommandType.StoredProcedure);

                // Initialize arrays
                result.ColorAmounts = new List<ColorAmountViewModel>();

                // Split the ColorCodes and Amounts if they are not null or empty
                if (!string.IsNullOrEmpty(result.ColorCodes) && !string.IsNullOrEmpty(result.Amounts))
                {
                    var colorCodesArray = result.ColorCodes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(code => code.Trim()).ToArray();
                    var amountsArray = result.Amounts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(amount => amount.Trim()).ToArray();

                    // Ensure both arrays have the same length
                    var length = Math.Min(colorCodesArray.Length, amountsArray.Length);
                    for (int i = 0; i < length; i++)
                    {
                        result.ColorAmounts.Add(new ColorAmountViewModel
                        {
                            ColorCode = colorCodesArray[i],
                            Amount = amountsArray[i]
                        });
                    }
                }

                // Convert DispenseAmount to a string
                if (result.DispenseAmount.HasValue)
                {
                    result.DispenseAmountString = result.DispenseAmount.Value.ToString("F2");
                }

                return result;
            }
        }

        public async Task<string> Delete(int id)
        {
            _logger.LogInformation("==========Execute [dbo].[usp_Formula_Delete] Store Procedure");
            var sql = $"[dbo].[usp_Formula_Delete]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { ID = id, ModifiedDate = DateTime.Now };
                var result = await connection.QuerySingleAsync<int>(sql, value, commandType: CommandType.StoredProcedure);
                return AlertMessages.Message[200040];
            }
        }

        public async Task<IEnumerable<ExcelFormulaViewModel>> GetFormulaExcelFile()
        {
            try
            {
                _logger.LogInformation("===========Execute [usp_Get_Excel_AllData_Formulas] Procedure===========");
                var sql = "[dbo].[usp_Get_Excel_AllData_Formulas]";
                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ExcelFormulaViewModel>(sql, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetFormulaExcelFile: {ex.Message}");
                throw; // Rethrow the exception to be handled at a higher level
            }
        }
    }
}
