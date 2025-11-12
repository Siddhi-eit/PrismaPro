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
    public interface IFormulaRepository
    {
        Task<ResponseObjectForAnything> SaveFormulaProfile(Formula formula);
        Task<IEnumerable<FormulaViewModel>> GetAllFormula(GridSearch gridSearch);
        Task<FormulaViewModel> GetByID(int id);
        Task<string> Delete(int id);
        Task<IEnumerable<ExcelFormulaViewModel>> GetFormulaExcelFile();
    }
}
