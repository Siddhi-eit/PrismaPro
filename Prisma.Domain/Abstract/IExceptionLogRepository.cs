using MDFusionLabHaute.Domain.Entities;
namespace MDFusionLabHaute.Domain.Abstract
{
    public interface IExceptionLogRepository : IGenericRepository<ExceptionLog>
    {
        Task<int> InsertLog(Exception ex);

        Task<int> InsertExceptionLog(string ExceptionMessage, string ExceptionSource);
    }
}
                                                                                                                                                                                                                                         