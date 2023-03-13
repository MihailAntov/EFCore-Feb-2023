using FastFood.Services.Web.ViewModels.Employees;

namespace FastFood.Services.Data;

public  interface IEmployeeService
{
    Task CreateAsync(RegisterEmployeeInputModel model);
    Task <IEnumerable<EmployeesAllViewModel>> GetAllAsync();
    Task<IEnumerable<RegisterEmployeeViewModel>> GetAllAvailablePositionsAsync();
}
