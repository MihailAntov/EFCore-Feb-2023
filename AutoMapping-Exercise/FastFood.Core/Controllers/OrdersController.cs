namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Services.Data;
    using Microsoft.AspNetCore.Mvc;
    using Services.Web.ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrdersController(FastFoodContext context, IMapper mapper,IOrderService orderService)
        {
            _context = context;
            _mapper = mapper;
            _orderService = orderService; 
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = _context.Items.Select(x => x.Id).ToList(),
                Employees = _context.Employees.Select(x => x.Id).ToList(),
            };

            return View(viewOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }
            
            await _orderService.CreateAsync(model);

            
            return RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var orders = _context.Orders
                .ProjectTo<OrderAllViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            return View(orders);
        }
    }
}
