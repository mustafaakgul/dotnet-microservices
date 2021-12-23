using Microservices.Web.Models.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//BU FRONTENDDEN GELCEK DATAYI KARSILAMAK CIIN OLUSTURULMUS BIR MODE DJANGO FORM GIBI ARA MODEL SERVICEDE DTO GIBI GECEN
namespace Microservices.Web.Models.Catalogs
{
    public class CourseViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;
        }

        public decimal Price { get; set; }

        public string UserId { get; set; }
        public string Picture { get; set; }

        public string StockPictureUrl { get; set; }

        public DateTime CreatedTime { get; set; }

        public FeatureViewModel Feature { get; set; }

        public string CategoryId { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}
