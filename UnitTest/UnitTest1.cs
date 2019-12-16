using Microsoft.VisualStudio.TestTools.UnitTesting;
using tencentyun;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TLSSigAPIv2 api = new TLSSigAPIv2(1400000000, "5bd2850fff3ecb11d7c805251c51ee463a25727bddc2385f3fa8bfee1bb93b5e");
            string sig = api.GenSig("xiaojun");
            System.Console.WriteLine(sig);
        }
        [TestMethod]
        public void TestMethod2()
        {
            TLSSigAPIv2 api = new TLSSigAPIv2(1400000000, "5bd2850fff3ecb11d7c805251c51ee463a25727bddc2385f3fa8bfee1bb93b5e");
            for (int i = 0; i < 100; i++)
            {
                string sig = api.GenSig("xiaojun");
                System.Console.WriteLine(sig);
            }
        }
        [TestMethod]
        public void TestMethod3()
        {
            TLSSigAPIv2 api = new TLSSigAPIv2(1400000000, "5bd2850fff3ecb11d7c805251c51ee463a25727bddc2385f3fa8bfee1bb93b5e");
            byte[] userbuf = api.GetUserBuf("xiaojun",10000,86400*180,255,0);
            System.Console.WriteLine(userbuf);
            string sig = api.GenSigWithUserBuf("xiaojun", 86400*180, userbuf);
            System.Console.WriteLine(sig);
        }
    }
}
