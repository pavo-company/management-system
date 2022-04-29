using System;
using System.Security.Cryptography;

namespace management_system
{
    public class license
    {
        readonly RNGCryptoServiceProvider csp;
        protected string licenseKey;
        public license()
        {
            csp = new RNGCryptoServiceProvider();
        }
        public string newLicense()
        {
            byte[] buffer = new byte[64];
            csp.GetBytes(buffer);
            licenseKey = Convert.ToBase64String(buffer);
            return licenseKey;
        }

    }
}