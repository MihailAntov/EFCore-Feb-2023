namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Services.Data;
    using Microsoft.AspNetCore.Mvc;
    using Services.Web.ViewModels.Items;

    public class ItemsController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;
        private readonly IItemService _itemService;

        public ItemsController(FastFoodContext context, IMapper mapper, IItemService itemService)
        {
            _context = context;
            _mapper = mapper;
            _itemService = itemService;
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<CreateItemViewModel> availableCategories = 
                await _itemService.GetAllAvailableCategories();

            return View(availableCategories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            await this._itemService.CreateAsync(model);

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ItemsAllViewModel> items =
                await _itemService.GetAllAsync();

            return this.View(items.ToList());
        }
    }
}
