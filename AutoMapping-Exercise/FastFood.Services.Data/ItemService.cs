using AutoMapper;
using FastFood.Data;
using FastFood.Services.Web.ViewModels.Items;
using FastFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data
{
    public class ItemService : IItemService
    {
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public ItemService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task CreateAsync(CreateItemInputModel model)
        {
            Item item = mapper.Map<Item>(model);
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();
        }

        

        public async Task<IEnumerable<ItemsAllViewModel>> GetAllAsync()
        {
            return await context.Items
                .ProjectTo<ItemsAllViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<CreateItemViewModel>> GetAllAvailableCategories()
        {
            return await context.Categories
                .ProjectTo<CreateItemViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}
