using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Baskets
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; } = 1;

        public string CourseId { get; set; }
        public string CourseName { get; set; }

        public decimal Price { get; set; }

        private decimal? DiscountAppliedPrice;  //inidirmli fiyat icin null olablr

        public decimal GetCurrentPrice   //inidirm uygulamnmussa indirimli yoksa indirmsz fiyatı ver
        {
            get => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price;
        }

        public void AppliedDiscount(decimal discountPrice)
        {
            DiscountAppliedPrice = discountPrice;
        }
    }
}
