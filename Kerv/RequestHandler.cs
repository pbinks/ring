using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kerv
{
    public abstract class RequestHandler
    {
        protected Dictionary<String, String> values;

        public RequestHandler()
        {
            values = new Dictionary<string, string>();
        }

        public void AddValue(String key, String value)
        {
            values.Add(key, value);
        }

        public abstract void Post(String url);

        public virtual async Task<String> Get(String url) => await Task.FromResult("");
    }
}
