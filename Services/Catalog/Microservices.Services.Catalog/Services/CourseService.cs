using AutoMapper;
using Microservices.Services.Catalog.DTOs;
using Microservices.Services.Catalog.Models;
using Microservices.Services.Catalog.Settings;
using Microservices.Shared.DTOs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Mass = MassTransit;

namespace Microservices.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        //private readonly Mass.IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings) //Mass.IPublishEndpoint publishEndpoint
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;

            //_publishEndpoint = publishEndpoint;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())  //kayıt varsa
            {
                foreach (var course in courses)  //tum kuralrda dolan
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync(); //kursların aktegorisin doldur bostu cnku db de bu kategori id si olanları getir
                }
            }
            else
            {
                courses = new List<Course>();  //bos data don yoksa
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return Response<CourseDto>.Fail("Course not found", 404);
            }
            course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();  //Category de arama yapıyorum

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();  //kategorsiin doldur 
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);

            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)  //update de geriye bisey donmeye grek yok
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);  //dto yu course nesnesine dnustur

            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);  //bulve degistir, courseupdatedto id si ile bul blursan updatecourse ile degisitr

            if (result == null)
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }

            //await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent { CourseId = updateCourse.Id, UpdatedName = courseUpdateDto.Name });

            return Response<NoContent>.Success(204);  //bdoy si olmayan bir durum kodu donmek
        }

        public async Task<Response<NoContent>> DeleteAsync(string id) //updat  delete geri dnus yok
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);  //metodlar mongo db den geliyor

            if (result.DeletedCount > 0) //silmisse 0 dan buyuk
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }
        }
    }
}
