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

namespace MDFusionLabHaute.Domain.Concrete
{
    public class LogRepository : ILogRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<LogRepository> _logger;
        public LogRepository(IConfiguration configuration, ILogger<LogRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }

        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(MachineLog log)
        {
            ResponseObjectForAnything responseObjectForAnything = new ResponseObjectForAnything();
            _logger.LogInformation("===========Execute [dbo].[i_MachineLog] Procedure===========");
            var sql = $"[dbo].[i_MachineLog]";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var value = new { UserID = log.UserID, @MachineID = log.MachineID, @Operation = log.Operation, @OperationDetails = log.OperationDetails };
                var result = await connection.QueryFirstAsync<int>(sql, value, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}