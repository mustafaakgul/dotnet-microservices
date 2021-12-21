using Microservices.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Domain.OrderAggregate
{
    public class Address : ValueObject  //bu clas bir value objectir onn ustunden ilerleyecek gibi dsnmeliyiz
    {
        public string Province { get; private set; }  //dısarıdan set edemesn  set etme bnm kontrolmde olsun

        public string District { get; private set; }

        public string Street { get; private set; }

        public string ZipCode { get; private set; }

        public string Line { get; private set; }

        //set kapatıldıgı icin kendi contructor kendimi olusturup bunn ustunde obje uretcez yoksa uretemyix
        public Address(string province, string district, string street, string zipCode, string line)
        {
            Province = province;
            District = district;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }

        //set kapatıldıgı icin set edemeyiz bazı seyleri sey etmek istiyorsan kendin yazıcaksın metodunu, boyle businessde kullncagımız kuralları burada tanımlarız set etmek mesela veya deger dgstrmek gibi
        /*
         public void setzipcode string zipcode
        ziocode = zipcode 
        medodu ile kendimiz st ettik
         */

        protected override IEnumerable<object> GetEqualityComponents()  //bu method ile yukardaki alanları donecegiz
        {
            yield return Province;
            yield return District;
            yield return Street;
            yield return ZipCode;
            yield return Line;
        }
    }
}
