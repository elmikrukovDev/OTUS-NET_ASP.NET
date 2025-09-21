using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferencesController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить предпочтения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PreferenceResponse>> GetPreferencesAsync()
        {
            try
            {
                var preferences = await _preferenceRepository.GetAllAsync();

                var preferenceModelList = preferences.Select(c =>
                    new PreferenceResponse()
                    {
                        Id = c.Id,
                        Description = c.Description
                    }).ToList();

                return preferenceModelList.Count == 0
                    ? NoContent()
                    : Ok(preferenceModelList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}