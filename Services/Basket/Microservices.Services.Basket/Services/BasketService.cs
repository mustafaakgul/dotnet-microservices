using Microservices.Services.Basket.DTOs;
using Microservices.Shared.DTOs;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservices.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        //redisle baglantı lazım ayrıa contructorlada iceri al
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);  //getdb ile db yi al
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket not found", 404);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId);  //belirlemis oldugumuz dat avarmu userId  id si ile

            if (String.IsNullOrEmpty(existBasket))
            {
                return Response<BasketDto>.Fail("Basket not found", 404);
            }

            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200); //stringi json serialize newtonsoft olmadan
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));  //jsona serialize ederek tutmak

            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket could not update or save", 500);  //true geliyorsa statusden 204 degilse  500
        }
    }
}
