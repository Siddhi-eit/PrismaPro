using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface ISanitisingTrakingRepository : IGenericRepository<SanitisingTraking>
    {
        Task<IEnumerable<SanitisingTrakingViewModel>> GetAllSanitisingTraking(GridSearch gridSearch);
        Task<ResponseObjectForAnything> Manage(SanitisingTrakingViewModel viewModel);
        Task<SanitisingTrakingViewModel> GetByIdAsync(int id);
        Task<string> SanitisingTrakingDelete(int id, int userId);
        Task<string> SetSanitizationDone(int userId, DateTime RefilledDate);
        Task<ResponseObjectForAnything> SetSanitizationSuccess(int ID, int userID, DateTime refilledDate,int machineID);
        Task<SanitisingTrakingViewModel> InsertSanitization(SanitisingTraking entity);
        Task<IEnumerable<ExcelSanitisingTrakingViewModel>> GetSanitisingTrakingExcelFile();
    }
}