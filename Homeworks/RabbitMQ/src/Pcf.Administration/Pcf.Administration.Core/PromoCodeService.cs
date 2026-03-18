using System;
using System.Threading.Tasks;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Abstractions.Services;
using Pcf.Administration.Core.Domain.Administration;

namespace Pcf.Administration.Core;

public class PromoCodeService : IPromoCodeService
{
    private readonly IRepository<Employee> _employeeRepository;

    public PromoCodeService(IRepository<Employee> employeeRepository) =>
        _employeeRepository = employeeRepository;

    public async Task UpdateAppliedPromocodesAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id) 
            ?? throw new ApplicationException($"Сотрудник не найден не найден.");
        
        employee.AppliedPromocodesCount++;

        await _employeeRepository.UpdateAsync(employee);
    }
}