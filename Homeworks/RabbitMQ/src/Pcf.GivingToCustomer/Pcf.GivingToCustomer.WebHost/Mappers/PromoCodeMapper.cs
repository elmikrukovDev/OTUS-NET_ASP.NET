using Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Collections.Generic;

namespace Pcf.GivingToCustomer.WebHost.Mappers
{
    public class PromoCodeMapper
    {
        public static PromoCode MapFromModel(Models.GivePromoCodeRequest request, Preference preference, IEnumerable<Customer> customers)
        {

            var promocode = new PromoCode
            {
                Id = request.PromoCodeId,
                PartnerId = request.PartnerId,
                Code = request.PromoCode,
                ServiceInfo = request.ServiceInfo,
                BeginDate = DateTime.Parse(request.BeginDate),
                EndDate = DateTime.Parse(request.EndDate),
                Preference = preference,
                PreferenceId = preference.Id,
                Customers = []
            };

            foreach (var item in customers)
            {
                promocode.Customers.Add(new PromoCodeCustomer()
                {

                    CustomerId = item.Id,
                    Customer = item,
                    PromoCodeId = promocode.Id,
                    PromoCode = promocode
                });
            }

            return promocode;
        }
    }
}
