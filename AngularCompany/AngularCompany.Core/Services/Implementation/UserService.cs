
using AngularCompany.Core.DTOs.Account;
using AngularCompany.Core.Security;
using AngularCompany.Core.Services.Interface;
using AngularCompany.DataLayer.Entitys.Account;
using AngularCompany.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using static AngularCompany.Core.DTOs.Account.LoginUserDTO;
using static AngularCompany.Core.DTOs.Account.RegisterUserDTO;

namespace AngularCompany.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        #region
        private IGenericRepository<User> userRepository;
        private IPasswordHelper passwordHelper;
        public UserService(IGenericRepository<User> uRepository , IPasswordHelper password)
        {
            this.userRepository = uRepository;
            this.passwordHelper = password;
        }
        #endregion

        public bool IsUserExistsByEmail(string email)
        {
            return this.userRepository.GetEntityQuerys().Any(t => t.Email == email.ToLower().Trim());
        }

        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO register)
        {
            if (IsUserExistsByEmail(register.Email))
                return RegisterUserResult.EmailExists;
            var user = new User {
                Email = register.Email.SanitizeText(),
                Address = register.Address.SanitizeText(),
                FirstName = register.FirstName.SanitizeText(),
                LastName = register.LastName.SanitizeText(),
                EmailActiveCode = Guid.NewGuid().ToString(),
                Password = passwordHelper.EncodePasswordMd5(register.Password)
            };
            await userRepository.AddEntity(user);
            await userRepository.SaveChenges();

            return RegisterUserResult.Success;
        }
        public async Task<LoginUserResult> LoginUser(LoginUserDTO login)
        {
            var password = passwordHelper.EncodePasswordMd5(login.Password);
            var userResault = await this.userRepository.GetEntityQuerys().SingleOrDefaultAsync(s => s.Email == login.Email.ToLower().Trim() && s.Password == password);
            
            if (userResault == null) return LoginUserResult.IncorrectData;

            if (!userResault.IsActivated) return LoginUserResult.NotActivated;

            return LoginUserResult.Success;
        }
        public Task<User> GetUserByEmail(string email)
        {
            return this.userRepository.GetEntityQuerys().SingleOrDefaultAsync(t => t.Email == email.ToLower().Trim());
        }
       
        public async Task<List<User>> GetAllUser()
        {
            return await this.userRepository.GetEntityQuerys().ToListAsync();
        }
        public async Task RemoveUser(long Id)
        {
            await this.userRepository.RemoveEntity(Id);
            await this.userRepository.SaveChenges();
        }
        public async Task<List<User>> GetActiveUser()
        {
            return await this.userRepository.GetEntityQuerys().Where(t=> !t.IsDeleted).ToListAsync();
        }
        public void Dispose()
        {
            this.userRepository?.Dispose();
        }
       
    }
}
