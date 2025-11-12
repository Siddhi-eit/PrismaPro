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
    public interface IAccountRepository
    {
        Task<UsersViewModel> SignInWithEmailAndPassword(Users users);
        Task<UsersViewModel> SignInWithEmailAndPasswordDesktop(Users users);
        Task<ResponseObjectForAnything> GetDesktopDataByID(int id);
    }
}
