

using AngularCompany.Core.DTOs.Account;
using AngularCompany.DataLayer.Entitys.Account;
using static AngularCompany.Core.DTOs.Account.LoginUserDTO;
using static AngularCompany.Core.DTOs.Account.RegisterUserDTO;

namespace AngularCompany.Core.Services.Interface
{
    public interface IUserService : IDisposable
    {
        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);
        bool IsUserExistsByEmail(string email);        
        Task<LoginUserResult> LoginUser(LoginUserDTO login);
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetAllUser();
        Task<List<User>> GetActiveUser();
        Task RemoveUser(long Id);
    }
}
