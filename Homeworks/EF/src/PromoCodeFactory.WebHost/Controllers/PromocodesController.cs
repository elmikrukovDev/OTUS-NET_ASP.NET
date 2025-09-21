using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promocodeRepository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly ICustomerRepository _customerRepository;

        public PromocodesController(
            IRepository<PromoCode> promocodeRepository,
            IPreferenceRepository preferenceRepository,
            ICustomerRepository customerRepository)
        {
            _promocodeRepository = promocodeRepository;
            _preferenceRepository = preferenceRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            try
            {
                var promocodes = await _promocodeRepository.GetAllAsync();

                var promocodeModelList = promocodes.Select(p =>
                    new PromoCodeShortResponse()
                    {
                        Id = p.Id,
                        ServiceInfo = p.ServiceInfo,
                        Code = p.Code,
                        BeginDate = p.BeginDate.ToShortDateString(),
                        EndDate = p.EndDate.ToShortDateString(),
                        PartnerName = p.PartnerName
                    }).ToList();

                return promocodeModelList.Count == 0
                    ? NoContent()
                    : Ok(promocodeModelList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Получить промокод по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}", Name = "GetPromoCodeById")]
        public async Task<ActionResult<PromoCodeShortResponse>> GetPromocodeAsync(Guid id)
        {
            try
            {
                var promocode = await _promocodeRepository.GetByIdAsync(id);

                if (promocode == null)
                    return NotFound();

                var response = new PromoCodeShortResponse()
                {
                    Id = promocode.Id,
                    ServiceInfo = promocode.ServiceInfo,
                    Code = promocode.Code,
                    PartnerName = promocode.PartnerName,
                    BeginDate = promocode.BeginDate.ToShortDateString(),
                    EndDate = promocode.EndDate.ToShortDateString()
                };

                return response;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var preference = await _preferenceRepository.GetByDescription(request.Preference);
                if (preference == null)
                    return BadRequest("Preference not found");

                var customersWithPreference = await _customerRepository.GetWithPreference(preference.Id);
                if (!customersWithPreference.Any())
                    return StatusCode(500, "Customers with preference not found");

                var promocode = new PromoCode()
                {
                    Id = Guid.NewGuid(),
                    Code = request.PromoCode,
                    BeginDate = DateTime.Now.Date,
                    ServiceInfo = request.ServiceInfo,
                    PartnerName = request.PartnerName,
                    PreferenceId = preference.Id,
                    CustomerId = customersWithPreference.First().Id
                };
                promocode = await _promocodeRepository.CreateAsync(promocode);

                var response = new PromoCodeShortResponse()
                {
                    Id = promocode.Id,
                    ServiceInfo = promocode.ServiceInfo,
                    Code = promocode.Code,
                    PartnerName = promocode.PartnerName,
                    BeginDate = promocode.BeginDate.ToShortDateString(),
                    EndDate = promocode.EndDate.ToShortDateString()
                };

                return CreatedAtRoute("GetPromoCodeById", new { id = promocode.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}