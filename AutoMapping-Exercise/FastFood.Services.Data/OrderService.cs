using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Services.Web.ViewModels.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Services.Data
{
    public class OrderService : IOrderService
    {
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public OrderService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task CreateAsync(CreateOrderInputModel model)
        {
            Order order = mapper.Map<Order>(model);
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderAllViewModel>> GetAllAsync()
        {
            

            return await context.Orders
                .ProjectTo<OrderAllViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<CreateOrderViewModel>> GetAllAvailableItemsAndEmployees()
        {
            CreateOrderViewModel model = new CreateOrderViewModel();
            model.Employees = await context.Employees
                .Select(e => e.Id)
                .ToListAsync();
            model.Items = await context.Items
                .Select(i => i.Id)
                .ToListAsync();

            return (IEnumerable<CreateOrderViewModel>)model;
        }

        
    }
}
