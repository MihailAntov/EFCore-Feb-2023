using FastFood.Services.Web.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Services.Data
{
    public interface IOrderService
    {
        Task CreateAsync(CreateOrderInputModel model);
        Task<IEnumerable<OrderAllViewModel>> GetAllAsync();
        Task<IEnumerable<CreateOrderViewModel>> GetAllAvailableItemsAndEmployees();
    }
}
