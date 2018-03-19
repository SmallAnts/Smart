using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Smart.Core.Utilites
{
    public static class IPAddressUtility
    {
        public static string GetLocalIPAddress()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            string result = string.Empty;
            foreach (var item in ips)
            {
                if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    result = item.ToString();
                    break;
                }
            }
            return result;
        }

    }
}
