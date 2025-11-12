using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface IDispenseRepository : IGenericRepository<DispenseLog>
    {
        Task<ResponseObjectForAnything> CheckConsultantID(string consultantID, string userID);
        Task<ResponseObjectForAnything> CheckProductCode(string productCode);
        Task<ResponseObjectForAnything> CheckTailoringCode(string tailoringCode, string dermaprofile);
        Task<ResponseObjectForAnything> GetDispenseData(string tailoringCode, string? dermaprofile, int? essenceOption, string machineID);
        Task<ResponseObjectForAnything> GetDispenseDataByProductCode(string ProductCode);
        Task<ResponseObjectForAnything> GetSelectedEssence(string tailoringCode, string selectedEssence);
        Task<IEnumerable<DispenserSelectViewModel>> GetAll(GridSearch gridSearch);
        Task<ResponseObjectForAnything> Manage(DispenseLogNew model);
        ResponseObjectForAnything GetAllDispenseDataAsync();
        ResponseObjectForAnything GetDispenseFormulaData();
        ResponseObjectForAnything GetDispenseDataByFilter(string[] items);
        Task<IEnumerable<CanistersViewModel>> CheckCanisterIsEligibleForDispense(string CanisterCodeAndAmount, decimal? AmountToDispense);
        ResponseObjectForAnything CheckIsCanisterExists(string UserID, string ComponentNames);
        Task<ResponseObjectForAnything> CheckDermaprofileData(string dermaprofile);
        Task<IEnumerable<ExcelDispenseViewModel>> GetDispenseExcelFile(int machineID);
    }
}