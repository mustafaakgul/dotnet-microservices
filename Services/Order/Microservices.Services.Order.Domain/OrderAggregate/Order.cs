using Microservices.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Domain.OrderAggregate
{
    //EF Core features  BUNLAR ARASTIRILABLR BU TEKNOLIJLER KULLANILDI
    // -- Owned Types
    // -- Shadow Property
    // -- Backing Field
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }

        //Id fln yok burada owned yapısı ile insa ediyoruz
        public Address Address { get; private set; }  //https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities

        public string BuyerId { get; private set; }  //satın alan kisini id si

        private readonly List<OrderItem> _orderItems;   //okuma ve yazma işlemini kendnden ziiyade field ile gerceklestiiryorsak backing fields denir bu encapsuleyi arttırmayı yarar
        //https://docs.microsoft.com/en-us/ef/core/modeling/backing-field?tabs=data-annotations
        //kmse order itema data eklemesn sadece bnm metodum uzernden eklesn alttaki metod ile 
        // DDS de HER YERI KORURSUN KMSE KAFASINA GORE SET ETMESN HERSEY KORUNSUN SADECE OKUMA OLARAK AC

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {
        }

        public Order(string buyerId, Address address)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        //order item eklemesn icin entity icinde metdolar tanımlanır
        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);

            if (!existProduct)
            {
                //normalde bu tarz seyler contructor ustunden yapmıyoruz ama smdi bu designda boyle yapılır normalde bire bir esitleyerek id=id gibi uretlir
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);

                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
    }
}
