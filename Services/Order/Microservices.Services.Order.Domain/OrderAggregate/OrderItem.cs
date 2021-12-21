using Microservices.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Domain.OrderAggregate
{
    public class OrderItem : Entity
    {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public Decimal Price { get; private set; }

        //Normal mimaride domain driven design olmadan burası order ile baglantılı olcaguncan
        //public int OrderId(get set) koyarız ama domain driven desidnden bunu tanmlamıyoruz tek basına eklemesn diye bu order uzernden agregate root ile ypaılcak
        //ama olmadanda onetomany nasıl kurulacak db de gene tanımlanacak ama burada karsılıgı olmayacak sdece bunlara shadow property deniyor
        //shadow entityde karsılıgı yok ancak db de karsılıgı olacak

        public OrderItem()
        {
        }

        public OrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

        public void UpdateOrderItem(string productName, string pictureUrl, decimal price)
        {
            ProductName = productName;
            Price = price;
            PictureUrl = pictureUrl;
        }
    }
}
