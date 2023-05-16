using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infastructure
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBaskedAsync(string baskedId)
        {
            return await _database.KeyDeleteAsync(baskedId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string baskedId)
        {
            var data = await _database.StringGetAsync(baskedId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBaskedAsync(CustomerBasket basked)
        {
            var created = await _database.StringSetAsync(
                basked.Id,
                JsonSerializer.Serialize(basked),
                TimeSpan.FromDays(30)
                );
            if (!created) return null;
            return await GetBasketAsync(basked.Id);
        }
    }
}