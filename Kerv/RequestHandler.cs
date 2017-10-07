using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kerv
{
    public abstract class RequestHandler
    {
        public abstract Task<bool> Post(Request request);

        public virtual async Task<String> Get(Request request) => await Task.FromResult("");
    }
}
