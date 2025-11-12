using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface IMachineRepository: IGenericRepository<Machine>
    {
        Task<ResponseObjectForAnything> SaveMachineProfile(Machine machine);
        Task<IEnumerable<Machine>> BindMachineDropdown(int id);
        Task<IEnumerable<MachineViewModel>> GetAllAsync(GridSearch gridSearch);
        Task<MachineViewModel> GetByIdAsync(int id);
        Task<ResponseObjectForAnything> Manage(MachineViewModel model);
        Task<IEnumerable<ExcelMachineViewModel>> GetMachineExcelFile();
    }
}
