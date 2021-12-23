using Microservices.Shared.DTOs;
using Microservices.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Microservices.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninInput signinInput);

        Task<TokenResponse> GetAccessTokenByRefreshToken();   //token kutuphanesi

        Task RevokeRefreshToken();  //logout oldugunda tokeni ucurmak icin
    }
}
