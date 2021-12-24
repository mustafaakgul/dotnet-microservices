using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Models.Baskets
{
    //entity yerine burdaki agibi ara modellerle calısınca bu classdaki gibi logikleride yaablrz entitylerde buraya yazılmaz ddd dada class icine yazılır
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();
        }

        public string UserId { get; set; }

        public string DiscountCode { get; set; }

        public int? DiscountRate { get; set; }  //null olablr

        private List<BasketItemViewModel> _basketItems;

        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    //Örnek kurs fiyat 100 TL indirim %10
                    _basketItems.ForEach(x =>
                    {
                        var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                        x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));  //virgul snra max 2 karekter
                    });
                }
                return _basketItems;
            }
            set
            {
                _basketItems = value;
            }
        }

        public decimal TotalPrice
        {
            get => _basketItems.Sum(x => x.GetCurrentPrice);
        }

        public bool HasDiscount
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;  //code varsa true dncek yoksa false
        }

        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }

        public void ApplyDiscount(string code, int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
    }
}
