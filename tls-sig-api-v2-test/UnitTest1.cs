using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tencentyun;

namespace tls_sig_api_v2_test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TLSSigAPI api = new TLSSigAPI(1400000000, "5bd2850fff3ecb11d7c805251c51ee463a25727bddc2385f3fa8bfee1bb93b5e");
            System.Console.WriteLine(api.GenSig("xiaojun"));
        }
    }
}
