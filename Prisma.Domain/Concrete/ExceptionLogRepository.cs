using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;

namespace MDFusionLabHaute.Domain.Concrete
{
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly ILogger<ExceptionLogRepository> _logger;
        public ExceptionLogRepository(IConfiguration configuration,ILogger<ExceptionLogRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            this.connectionString = _configuration.GetConnectionString("ConnectionString");
        }

        public Task<string> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(ExceptionLog entity)
        {
            _logger.LogInformation("========== Add Log into ExceptionLog ==========");
            var sql = $"[usp_ExceptionLog_Insert]";
            using(var connection = new SqlConnection(this.connectionString))
            {
                var result = await connection.QuerySingleAsync<int>(sql, entity,commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public Task<int> InsertLog(Exception ex)
        {
            var entity = new ExceptionLog()
            {
                ID = 0,
                ExceptionMessage = ex.Message,
                ExceptionSource = ex.StackTrace,
                CreatedDate = DateTime.Now,
                Createdby = 1
            };
            return Insert(entity);
        }

        //public async Task<int> InsertLogForDesktop(ExceptionLog entity)
        //{
        //    _logger.LogInformation("========== Add Log into ExceptionLog ==========");
        //    var sql = $"[usp_ExceptionLog_Insert]";
        //    using (var connection = new SqlConnection(this.connectionString))
        //    {
        //        var result = await connection.QuerySingleAsync<int>(sql, entity, commandType: CommandType.StoredProcedure);
        //        return result;
        //    }
        //}
        public Task<int> InsertExceptionLog(string ExceptionMessage, string ExceptionSource)
        {
            var entity = new ExceptionLog()
            {
                ID = 0,
                ExceptionMessage = ExceptionMessage,
                ExceptionSource = ExceptionSource,
                CreatedDate = DateTime.Now,
                Createdby = 1
            };
            return Insert(entity);
        }
    }
}
