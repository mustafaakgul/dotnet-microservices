using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }

        public IEnumerable<string> GetUserProps()  //bu metod properryleri doner bu metodu cagırdıgmız zaman
        {
            yield return UserName;
            yield return Email;
            yield return City;
        }
    }
}
