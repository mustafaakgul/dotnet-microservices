using Microservices.Services.Basket.DTOs;
using Microservices.Shared.DTOs;
using System.Threading.Tasks;

namespace Microservices.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);

        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);

        Task<Response<bool>> Delete(string userId);
    }
}
