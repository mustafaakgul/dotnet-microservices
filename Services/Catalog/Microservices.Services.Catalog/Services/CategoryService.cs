using AutoMapper;
using Microservices.Services.Catalog.DTOs;
using Microservices.Services.Catalog.Models;
using Microservices.Services.Catalog.Settings;
using Microservices.Shared.DTOs;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);  //db baglantısı

            var database = client.GetDatabase(databaseSettings.DatabaseName); //db ver

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);  //db ismini ver
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)  //donus Task<Response<CategoryDto>>
        {
            var category = await _categoryCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return Response<CategoryDto>.Fail("Category not found", 404);
            }

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }
}
