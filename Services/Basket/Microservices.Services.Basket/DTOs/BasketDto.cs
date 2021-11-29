using System.Collections.Generic;
using System.Linq;

namespace Microservices.Services.Basket.DTOs
{
    public class BasketDto
    {
        public string UserId { get; set; }

        public string DiscountCode { get; set; }

        public int? DiscountRate { get; set; }

        public List<BasketItemDto> basketItems { get; set; }

        public decimal TotalPrice  //sadece get olsn
        {
            get => basketItems.Sum(x => x.Price * x.Quantity);
        }
    }
}
