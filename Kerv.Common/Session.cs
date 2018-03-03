using System;
using System.Collections.Generic;

namespace Kerv.Common
{
    public class Session
    {
        private IList<string> cookies;
        private static Session instance;

        private Session() {
            cookies = new List<string>();
        }

        public static Session Instance {
            get {
                if (instance == null) {
                    instance = new Session();
                }
                return instance;
            }
        }

        public IList<string> Cookies {
            get {
                return cookies;
            }
        }

    }
}
