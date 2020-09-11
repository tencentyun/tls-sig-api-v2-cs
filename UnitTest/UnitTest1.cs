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
            string sig = api.genUserSig("xiaojun");
            System.Console.WriteLine(sig);
        }
        [TestMethod]
        public void TestMethod2()
        {
            TLSSigAPIv2 api = new TLSSigAPIv2(1400000000, "5bd2850fff3ecb11d7c805251c51ee463a25727bddc2385f3fa8bfee1bb93b5e");
            for (int i = 0; i < 100; i++)
            {
                string sig = api.genUserSig("xiaojun");
                System.Console.WriteLine(sig);
            }
        }
        [TestMethod]
        public void TestMethod3()
        {
            TLSSigAPIv2 api = new TLSSigAPIv2(1400000000, "5bd2850fff3ecb11d7c805251c51ee463a25727bddc2385f3fa8bfee1bb93b5e");
            string sig = api.genPrivateMapKey("xiaojun", 86400 * 180, 10000, 255);
            System.Console.WriteLine(sig);
        }
        [TestMethod]
        public void TestMethod4()
        {
            TLSSigAPIv2 api = new TLSSigAPIv2(1400000000, "5bd2850fff3ecb11d7c805251c51ee463a25727bddc2385f3fa8bfee1bb93b5e");
            string sig = api.genPrivateMapKeyWithStringRoomID("xiaojun", 86400 * 180, "100086545679", 255);
            System.Console.WriteLine(sig);
        }
    }
}
