using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace RedisClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly ILogger<RedisController> _logger;
        private IRedisCacheClient _redisCacheClient;

        public RedisController(IRedisCacheClient redisCacheClient, ILogger<RedisController> logger)
        {
            _logger = logger;
            _redisCacheClient = redisCacheClient;
        }

        [HttpPost]
        [Route("get")]
        public async Task<string> GetKey([FromBody] string key)
        {
            string value = string.Empty;
            try
            {
                value = await this._redisCacheClient.Db0.GetAsync<string>(key);
            }
            catch (Exception ex)
            {
                value = ex.Message;
                
            }
            return value;
        }

        [HttpPost]
        [Route("set")]
        public async Task<bool> Set([FromBody] string key, string value)
        {
            bool added = false;
            try
            {
                added = await this._redisCacheClient.Db0.AddAsync(key, value, DateTimeOffset.Now.AddMinutes(10));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return added;
        }
        
    }
}
