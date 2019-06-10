using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using ComponentAce.Compression.Libs.zlib;

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

        private string HMACSHA256(string identifier, long currTime, int expire)
        {
            string rawContentToBeSigned = "TLS.identifier:" + identifier + "\n"
                 + "TLS.sdkappid:" + sdkappid + "\n"
                 + "TLS.time:" + currTime + "\n"
                 + "TLS.expire:" + expire + "\n";
            using (HMACSHA256 hmac = new HMACSHA256())
            {
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] textBytes = encoding.GetBytes(rawContentToBeSigned);
                Byte[] keyBytes = encoding.GetBytes(key);
                Byte[] hashBytes;
                using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                    hashBytes = hash.ComputeHash(textBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public string GenSig(string identifier, int expire=180*86400)
        {
            DateTime epoch = new DateTime(1970, 1, 1); // unix 时间戳
            Int64 currTime = (Int64)(DateTime.UtcNow - epoch).TotalMilliseconds / 1000;

            string base64sig = HMACSHA256(identifier, currTime, expire);

            // 没有引入 json 库，所以这里手动进行组装
            string jsonData = String.Format("{{"
                + "\"TLS.ver\":" + "\"2.0\","
                + "\"TLS.identifier\":" + "\"{0}\","
                + "\"TLS.sdkappid\":" + "{1},"
                + "\"TLS.expire\":" + "{2},"
                + "\"TLS.time\":" + "{3},"
                + "\"TLS.sig\":" + "\"{4}\""
                + "}}", identifier, sdkappid, expire, currTime, base64sig);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonData);
            return Convert.ToBase64String(CompressBytes(buffer))
                .Replace('+', '*').Replace('/', '-').Replace('=', '_');
        }
    }
}
