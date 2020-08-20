using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisClient.Models
{
    public class SetValueRequest
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
