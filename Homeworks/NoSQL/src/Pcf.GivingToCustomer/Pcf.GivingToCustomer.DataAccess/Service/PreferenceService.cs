using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Service;

namespace Pcf.GivingToCustomer.DataAccess.Service;

public class PreferenceService : IPreferenceService
{
    private readonly HttpClient _httpClient;

    public PreferenceService(HttpClient httpClient) =>    
        _httpClient = httpClient;

    public async Task<IEnumerable<Preference>> GetPreferencesAsync()
    {
        var response = await _httpClient.GetAsync("/api/v1/preferences");
        var content = await response.Content.ReadAsStringAsync();
        var preferences = JsonSerializer
            .Deserialize<List<Preference>>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        return preferences;
    }
}