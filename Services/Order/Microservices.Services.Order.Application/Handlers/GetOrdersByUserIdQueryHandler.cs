using AutoMapper;
using MediatR;
using Microservices.Services.Order.Application.DTOs;
using Microservices.Services.Order.Application.Mapping;
using Microservices.Services.Order.Application.Queries;
using Microservices.Services.Order.Infrastructure;
using Microservices.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Services.Order.Application.Handlers
{
    internal class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any())
            {
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);  //burada lazy olayı cagırılır o zmn o kodalr calısır

            return Response<List<OrderDto>>.Success(ordersDto, 200);
        }
    }
}
