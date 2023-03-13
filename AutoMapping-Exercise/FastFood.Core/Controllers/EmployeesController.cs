namespace FastFood.Core.Controllers
{
    using System;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Services.Web.ViewModels.Employees;
    using FastFood.Services.Data;
    using FastFood.Models;
    using Microsoft.EntityFrameworkCore;

    public class EmployeesController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(FastFoodContext context, IMapper mapper, IEmployeeService employeeService)
        {
            _context = context;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Register()
        {
            IEnumerable<RegisterEmployeeViewModel> availablePositions =
                 await this._employeeService.GetAllAvailablePositionsAsync();

            return this.View(availablePositions);

            
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var employee = _mapper.Map<Employee>(model);
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("All", "Employees");
        }

        public async Task<IActionResult> All()
        {
            var employees = await _context.Employees
                .ProjectTo<EmployeesAllViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(employees);
        }
    }
}
