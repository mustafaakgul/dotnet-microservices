using Microservices.Services.Catalog.DTOs;
using Microservices.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();

        Task<Response<CategoryDto>> CreateAsync(CategoryDto category);  //burasi category ismlendrmesi ynlıs

        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
