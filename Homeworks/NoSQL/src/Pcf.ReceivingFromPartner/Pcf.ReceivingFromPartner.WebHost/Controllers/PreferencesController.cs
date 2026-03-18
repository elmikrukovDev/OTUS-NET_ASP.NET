using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pcf.ReceivingFromPartner.Core.Abstractions.Cache;
using Pcf.ReceivingFromPartner.WebHost.Models;
namespace Pcf.ReceivingFromPartner.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения клиентов
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController
        : ControllerBase
    {
        private readonly IPreferenceCacheService _preferenceService;

        public PreferencesController(IPreferenceCacheService preferenceService)
        {
            _preferenceService = preferenceService;
        }
        
        /// <summary>
        /// Получить список предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
        {
            var preferences = await _preferenceService.GetPreferencesAsync();

            var response = preferences.Select(x => new PreferenceResponse()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Ok(response);
        }
    }
}