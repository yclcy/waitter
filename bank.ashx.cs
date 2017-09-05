using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace waitter
{
    /// <summary>
    /// bank 的摘要说明
    /// </summary>
    public class bank : IHttpHandler
    {
        private const string PublicRsaKey = @"<RSAKeyValue><Modulus>MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCoFoljPoLGKSnkmpExqAp2+wgAduKt1Zy48R857OXY8ND/2MQXpzt4LZ/bL2cww9enrKB7CD544WqjKgG/64XO70/bj+a8gSBcCW4X7ldVgQsc3bYukR4zjGPdhIhXLGMyKQGOgvNLkGIGyn64yV43GV1429LUsqo/3qepdMPjnQIDAQAB</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public void ProcessRequest(HttpContext context)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicRsaKey);
            var source = "{\"realName\":\"刘长勇\",\"cardNo\":\"6222600110039716159\",\"idNumber\":\"320911197910073116\"}";
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(source), true);
            context.Response.Write(Convert.ToBase64String(cipherbytes));

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string input = "charest=utf-8&centerMerchantId=000000020000000&interfaceCode=chinaums.api.bankcard.RealNameVerifyCard&nonceStr=73e80a7b563d48bf91f475f4c5afedbb&orgCode=00000002&transNum=201604250000000224&verifyStr=L8frcgzoWy+5iwdvnw2itwx/lKPkffcqfh/QH4tA5xPUEldvt7ahffm9ESXik1mf0G0bYbbsFUM+dQX9CKPb7rGmf1uYA2IPtWIf2TbNPadAy3YghD/yel3bfuNHrjnLHZHMdydTKiKsHtvjVbMAnYsiawFqVRCpJS22nwbP+laDw4SltS2gHTmpG8EIfQD0kMtprT3lwM16ivXIsUIyrj0n&verifyType=verify3&f4fb795b7d164554a341977617f77bdf";
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = md5.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            context.Response.Write("<br />"+password);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}