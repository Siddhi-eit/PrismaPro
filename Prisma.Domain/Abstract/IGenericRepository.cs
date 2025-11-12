
namespace MDFusionLabHaute.Domain.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> Insert(T entity);
        Task<string> Delete(int id);
    }
}
