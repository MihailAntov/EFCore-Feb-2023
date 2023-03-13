namespace FastFood.Services.Mapping
{
    using AutoMapper;
    using FastFood.Models;
    using Services.Web.ViewModels.Categories;
    using Services.Web.ViewModels.Positions;
    using Services.Web.ViewModels.Employees;
    using Services.Web.ViewModels.Orders;
    using Services.Web.ViewModels.Items;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            //Categories
            CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(c => c.Name, c => c.MapFrom(c => c.CategoryName));

            CreateMap<Category, CategoryAllViewModel>();


            //Employees
            CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(d => d.PositionId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d=>d.PositionName, opt=> opt.MapFrom(s=>s.Name));

            CreateMap<RegisterEmployeeInputModel, Employee>()
                .ForMember(d => d.PositionId, opt => opt.MapFrom(s => s.PositionId));


            CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(d => d.Position, opt => opt.MapFrom(s => s.Position.Name));


            //Items
            CreateMap<Category, CreateItemViewModel>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.Id));

            CreateMap<CreateItemInputModel, Item>();

            CreateMap<Item, ItemsAllViewModel>()
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category.Name));

            //Orders

            CreateMap<Order, OrderAllViewModel>()
                .ForMember(d => d.Employee, opt => opt.MapFrom(s => s.Employee.Name))
                .ForMember(d => d.OrderId, opt => opt.MapFrom(s => s.Id));

            CreateMap<CreateOrderInputModel, Order>()
                .ForMember(d => d.DateTime, opt => opt.MapFrom(s => DateTime.Now));
                

            //CreateMap<Item, CreateOrderViewModel>()
            //    .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Id));
            //CreateMap<Employee, CreateOrderViewModel>()
            //    .ForMember(d => d.Employees, opt => opt.MapFrom(s => s.Id));
                

                


        }
    }
}
