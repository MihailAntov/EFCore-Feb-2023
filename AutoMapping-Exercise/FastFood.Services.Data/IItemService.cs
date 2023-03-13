using FastFood.Services.Web.ViewModels.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Services.Data
{
    public interface IItemService
    {
        Task CreateAsync(CreateItemInputModel model);
        Task<IEnumerable<ItemsAllViewModel>> GetAllAsync();
        Task<IEnumerable<CreateItemViewModel>> GetAllAvailableCategories();
    }
}
