using System;
using System.Collections.Generic;

namespace Kerv
{
    public class Request
    {
        public string Url { get; }
        public Dictionary<String, String> Values { get; }

        public Request(String url)
        {
            Url = url;
            Values = new Dictionary<string, string>();
        }

        public void AddValue(String key, String val) {
            Values.Add(key, val);
        }
    }
}
