using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaix.Helpers
{
    public static class GuidHelper
    {
        public static string Shorter(string guid)
        {
            return Shorter(Guid.Parse(guid));
        }

        public static string Shorter(Guid guid)
        {
            //return Convert.ToBase64String(guid.ToByteArray());
            return guid.ToString().Substring(guid.ToString().Length - 4, 4);
        }

    }
}
