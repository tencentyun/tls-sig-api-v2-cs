using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using ComponentAce.Compression.Libs.zlib;


namespace tencentyun
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

        private static byte[] CompressBytes(byte[] sourceByte)
        {
            MemoryStream inputStream = new MemoryStream(sourceByte);
            Stream outStream = CompressStream(inputStream);
            byte[] outPutByteArray = new byte[outStream.Length];
            outStream.Position = 0;
            outStream.Read(outPutByteArray, 0, outPutByteArray.Length);
            return outPutByteArray;
        }

        private static Stream CompressStream(Stream sourceStream)
        {
            MemoryStream streamOut = new MemoryStream();
            ZOutputStream streamZOut = new ZOutputStream(streamOut, zlibConst.Z_DEFAULT_COMPRESSION);
            CopyStream(sourceStream, streamZOut);
            streamZOut.finish();
            return streamOut;
        }

        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        /**
         * 对 identifier 进行签名
         * @param identifier 用户账号，utf-8 编码
         * @param expireTime 有效期，单位为秒，默认为 180 天
         * @return 签名内容
         */
        public string GenSig(string identifier, int expireTime=180*86400)
        {
            DateTime epoch = new DateTime(1970, 1, 1); // unix 时间戳
            Int64 currTime = (Int64)(DateTime.UtcNow - epoch).TotalMilliseconds / 1000;

            string rawContentToBeSigned = "TLS.identifier:" + identifier + "\n"
                 + "TLS.sdkappid:" + sdkappid + "\n"
                 + "TLS.time:" + currTime + "\n"
                 + "TLS.expire:" + expireTime + "\n";
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                using (MemoryStream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(rawContentToBeSigned)))
                {
                    byte[] rawSig = hmac.ComputeHash(inputStream);
                    string base64sig = Convert.ToBase64String(rawSig);
                    // 没有引入 json 库，所以这里手动进行组装
                    string jsonData = String.Format("{{"
                        + "\"TLS.ver\":" + "\"2.0\","
                        + "\"TLS.identifier\":" + "\"{0}\","
                        + "\"TLS.sdkappid\":" + "{1},"
                        + "\"TLS.expire\":" + "{2},"
                        + "\"TLS.time\":" + "{3},"
                        + "\"TLS.sig\":" + "\"{4}\""
                        + "}}", identifier, sdkappid, expireTime, currTime, base64sig);
                    byte[] buffer = Encoding.UTF8.GetBytes(jsonData);
                    return Convert.ToBase64String(CompressBytes(buffer))
                        .Replace('+', '*').Replace('/', '-').Replace('=', '_');
                }
            }
        }
    }
}
