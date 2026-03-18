using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Mappers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core;

public class PromoCodeService(
        IRepository<PromoCode> promoCodesRepository,
        IRepository<Preference> preferencesRepository, 
        IRepository<Customer> customersRepository) 
    : IPromoCodeService
{
    public async Task GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest req)
    {
        var preference = await preferencesRepository.GetByIdAsync(req.PreferenceId)
            ?? throw new ApplicationException($"Промокод не найден.");
        var customers = await customersRepository
            .GetWhere(d => d.Preferences.Any(x => x.Preference.Id == preference.Id));

        PromoCode promoCode = PromoCodeMapper.MapFromModel(req, preference, customers);
        await promoCodesRepository.AddAsync(promoCode);
    }
}