using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using System.Reflection.PortableExecutable;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface IRefillTrackingRepository : IGenericRepository<RefillTracking>
    {
        Task<IEnumerable<RefillTrackingViewModel>> GetAll(GridSearch gridSearch);
        Task<ResponseObjectForAnything> Manage(RefillTrackingViewModel viewModel);
        Task<ResponseObjectForAnything> SetRefillDone(int refillID,int userID,DateTime refilledDate, int machineID);
        Task<IEnumerable<CanSize>> GetCanSize();

        Task<RefillTrackingViewModel> GetByIdAsync(int id);

        Task<RefillTrackingViewModel> GetScannerDetailByIdAsync(int id);
        Task<string> RefillTrakingDelete(int id, int userId);
        Task<RefillTrackingViewModel> InsertRefill(RefillTracking entity);
        Task<IEnumerable<ExcelRefillTrackingViewModel>> GetRefillTrackingExcelFile();
    }
}
