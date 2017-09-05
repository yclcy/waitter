using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace waitter
{
    /// <summary>
    /// rec 的摘要说明
    /// </summary>
    public class rec : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string key = sysconf.rec_app_key;
            string secret = sysconf.rec_secret;
            string timestamp = WxPayAPI.WxPayApi.GenerateTimeStamp();
            string source = key + secret + timestamp;
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);
                var obj = new
                {
                    timestamp = timestamp,
                    api_key = key,
                    verify_type = "ocr_id_name_check_v2",
                    trans_id = GetGuid(),
                    trade_date = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    param = new
                    {
                        image = ""
                    },
                    sign = hash
                };
                
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));

            }
        }

        private static string GetGuid()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid.ToString();
        }

    public static string GetMd5Hash(MD5 md5Hash,String input)
        {

            //System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            //bs = md5.ComputeHash(bs);
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
                //sBuilder.Append(data[i].ToString());
            }
            return sBuilder.ToString();
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