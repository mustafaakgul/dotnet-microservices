using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Microservices.Services.Catalog.Models
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public string UserId { get; set; }  //guvenlik acısından string veya random degerler tutmak cok daha guvenli

        public string Picture { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }

        public Feature Feature { get; set; }  //onetoone baglantı yontemi

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        [BsonIgnore]//bunu db ye yansıtma bunu kendi icimizde kullancaz
        public Category Category { get; set; }  //categoryıd donerken categoryileride donmek istersek, ayrıca mongodbde karsılıgı olmayacakignore diyecegiz
    }
}
