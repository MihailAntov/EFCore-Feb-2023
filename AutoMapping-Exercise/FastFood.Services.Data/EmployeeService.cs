﻿

namespace FastFood.Services.Data
{

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using FastFood.Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Web.ViewModels.Employees;

    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public EmployeeService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task CreateAsync(RegisterEmployeeInputModel model)
        {
            Employee employee = this.mapper.Map<Employee>(model);

            await this.context.Employees.AddAsync(employee);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeesAllViewModel>> GetAllAsync()
            => await this.context.Employees
                .ProjectTo<EmployeesAllViewModel>(this.mapper.ConfigurationProvider)
                .ToArrayAsync();

        public async Task<IEnumerable<RegisterEmployeeViewModel>> GetAllAvailablePositionsAsync()
            => await this.context.Positions
                .ProjectTo<RegisterEmployeeViewModel>(this.mapper.ConfigurationProvider)
                .ToArrayAsync();

        
    }
}

