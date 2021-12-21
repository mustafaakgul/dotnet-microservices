using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Discount.Models
{
    [Dapper.Contrib.Extensions.Table("discount")]  //kucuk harfle olusturlur mapleme
    public class Discount
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int Rate { get; set; }

        public string Code { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
