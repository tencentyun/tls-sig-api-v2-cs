using System;
using System.IO;
using System.Security.Cryptography;


namespace tecentyun
{
    public class TLSSigAPI
    {
        private readonly int sdkappid;
        private readonly string key;

        public TLSSigAPI(int sdkappid, string key)
        {
            this.sdkappid = sdkappid;
            this.key = key;
        }

        public string GenSig(string identifier, int expireTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1); // unix 时间戳
            Int64 currTime = (Int64)(DateTime.UtcNow - epoch).TotalMilliseconds / 1000;

            string rawContentToBeSigned = "TLS.identifier:" + identifier + "\n"
                 + "TLS.sdkappid:" + sdkappid + "\n"
                 + "TLS.time:" + currTime + "\n"
                 + "TLS.expire:" + expireTime + "\n";
            using (HMACSHA256 hmac = new HMACSHA256())
            {
            }
            return "";
        }
    }
}
