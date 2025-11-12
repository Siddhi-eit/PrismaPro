using MDFusionLabHaute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDFusionLabHaute.Domain.ViewModel;
using MDFusionLabHaute.Domain.ResponseObject;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface ICanisterLookupRepository : IGenericRepository<CanisterLookup>
    {
        Task<IEnumerable<CanistersLookupViewModel>> GetAllAsync(GridSearch gridSearch);
        Task<ResponseObjectForAnything> Manage(CanistersLookupViewModel model);
        Task<CanistersLookupViewModel> GetByIdAsync(int id);
        Task<IEnumerable<ExcelCanisterLookupViewModel>> GetCanisterLookupExcelFile();
    }
}
