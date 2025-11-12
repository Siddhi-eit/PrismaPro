using MDFusionLabHaute.Domain.Entities;
using MDFusionLabHaute.Domain.ResponseObject;
using MDFusionLabHaute.Domain.ViewModel;

namespace MDFusionLabHaute.Domain.Abstract
{
    public interface IUserRepository : IGenericRepository<Users>
    {
        Task<IEnumerable<UsersViewModel>> GetUsers(GridSearch gridSearch);
        Task<ResponseObjectForAnything> ManageUser(UsersViewModel users);
        Task<UsersViewModel> GetUserByID(int id);
        Task<IEnumerable<UsersViewModel>> BindUserDropdown();
        Task<IEnumerable<Roles>> BindUsertypeDropdown();
        Task<IEnumerable<Dropdown>> BindMDFusionLabDropdown(int id);
        Task<IEnumerable<ExcelUserViewModel>> GetUserExcelFile();
    }
}
