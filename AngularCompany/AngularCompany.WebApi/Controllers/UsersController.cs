
using AngularCompany.Core.DTOs.Account;
using AngularCompany.Core.Services.Interface;
using AngularCompany.Core.Utilities.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static AngularCompany.Core.DTOs.Account.LoginUserDTO;
using static AngularCompany.Core.DTOs.Account.RegisterUserDTO;

namespace AngularCompany.WebApi.Controllers
{
    public class UsersController : SiteBaseController
    {
        #region constractor
        private IUserService userService;

        public UsersController(IUserService Service)
        {
            this.userService = Service;
        }
        #endregion
        #region select
        [HttpGet(template: "Users")]
        public async Task<IActionResult> Users()
        {
            var resault = await userService.GetAllUser();
            return JsonResponseStatus.Success(resault);
        }
        #endregion

        #region delete
        //[HttpGet(template: "removeUser")]
        //public async Task<IActionResult> removeUser()
        //{
        //    var resault = await userService.GetAllUser();
        //    return JsonResponseStatus.Success(resault);
        //}
        [HttpGet("removeUser")]
        public async Task<IActionResult> removeUser(long id)
        {
            if (ModelState.IsValid)
            {
                await this.userService.RemoveUser(id);
               
            }
            return JsonResponseStatus.Success();
        }
        #endregion

        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register ([FromBody] RegisterUserDTO registerDtos)
        {
            if (ModelState.IsValid)
            {
                var res = await this.userService.RegisterUser(registerDtos);
                switch (res)
                {
                    case RegisterUserResult.EmailExists:
                        return JsonResponseStatus.Error(new { info = "این کاربر قبلا ثبت نام نموده." });
                }
            }
            return JsonResponseStatus.Success();
        }

        #endregion

        #region Login

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO login)
        {
            if (ModelState.IsValid)
            {
                var res = await userService.LoginUser(login);

                switch (res)
                {
                    case LoginUserResult.IncorrectData:
                        return JsonResponseStatus.NotFound();

                    case LoginUserResult.NotActivated:
                        return JsonResponseStatus.Error(new { message = "حساب کاربری شما فعال نشده است" });

                    case LoginUserResult.Success:
                        var user = await userService.GetUserByEmail(login.Email);
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularEshopJwtBearer"));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokenOptions = new JwtSecurityToken(
                            issuer: "https://localhost:7029",
                            claims: new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Email),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                            },
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: signinCredentials
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                        return JsonResponseStatus.Success(new { token = tokenString, expireTime = 30, firstName = user.FirstName, lastName = user.LastName, userId = user.Id });
                }
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region Sign Out

        [HttpGet("sing-out")]
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion
    }
}
