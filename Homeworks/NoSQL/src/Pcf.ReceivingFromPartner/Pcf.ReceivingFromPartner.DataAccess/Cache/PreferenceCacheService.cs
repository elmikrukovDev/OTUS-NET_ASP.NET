using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Abstractions.Cache;
using Pcf.ReceivingFromPartner.Core.Domain;
using System.Text.Json;
using Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using StackExchange.Redis;
namespace Pcf.ReceivingFromPartner.DataAccess.Cache;

public class PreferenceCacheService : IPreferenceCacheService
{
    private IRepository<Preference> _preferenceRepository;
    private IConnectionMultiplexer _redisConnection;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);
    
    public PreferenceCacheService(IRepository<Preference> repository, IConnectionMultiplexer redis)
    {
        _preferenceRepository = repository;
        _redisConnection = redis;
    }
   
    private IDatabase GetDatabase() => _redisConnection.GetDatabase();
    
    public async Task<IEnumerable<Preference>> GetPreferencesAsync()
    {
        var db = GetDatabase();
        string cacheKey = "preferences:all";
        var cached = await db.StringGetAsync(cacheKey);

        if (!cached.IsNull)
            return JsonSerializer
                .Deserialize<IEnumerable<Preference>>(cached);

        var preferences = await _preferenceRepository
            .GetAllAsync();
        
        await db.StringSetAsync(
            cacheKey,
            JsonSerializer.Serialize(preferences),
            _cacheExpiration
        );

        return preferences;
    }
}