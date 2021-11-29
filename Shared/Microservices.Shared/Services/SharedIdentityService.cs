using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        //jwt ustundeki sub datasını okumka claim nesneleri bunlar kullanıcı ile ilgili data lar demektir bunu httpcontext nesnesine eklegnden alttaki ustunden erisecegiz
        private IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //claims key vlaue olarak tutuluyor amac tokendeki sub key ine ulasmak
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
