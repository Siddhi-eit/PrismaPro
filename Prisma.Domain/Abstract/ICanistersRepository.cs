using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface ICanistersRepository : IGenericRepository<Canisters>
    {
        Task<IEnumerable<CanistersViewModel>> GetAllAsync(GridSearch gridSearch);
        Task<ResponseObjectForAnything> Manage(CanistersViewModel model);
        Task<CanistersViewModel> GetByIdAsync(int id);
        Task<IEnumerable<Unit>> BindDispenseUnitDropdown();
        Task<IEnumerable<CanistersViewModel>> getByUserIdAsync(int machineID);
        Task<IEnumerable<Dropdown>>BindProductDropdown();
        Task<IEnumerable<ExcelCanisterViewModel>> GetCanisterExcelFile();

        Task<IEnumerable<CanistersLookupViewModel>> GetCanisterLookup();
    }
}
