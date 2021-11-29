using IdentityModel;
using IdentityServer4.Validation;
using Microservices.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager; //user dogurlayacagmzdan usera alıyoruz ihriyac var

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = await _userManager.FindByEmailAsync(context.UserName);  //user varmı, burada usernamede email gonderiyor olacagz 

            //email kontrol
            if (existUser == null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifreniz yanlış" });
                context.Result.CustomResponse = errors;

                return;  //hic bisey dnmuyoruz bos olarak ckıyoruz
            }
            var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);

            //password kontrol
            if (passwordCheck == false)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifreniz yanlış" });
                context.Result.CustomResponse = errors;

                return;
            }

            //boyle kullanıcı var buraya geldiyse ve grdigi bilgilerde dogur
            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
