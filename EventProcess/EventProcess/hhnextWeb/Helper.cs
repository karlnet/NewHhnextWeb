using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static hhnextWeb.Controllers.AccountsController;

namespace hhnextWeb
{
    class Helper
    {
        private static readonly Task<object> NULLTASK = Task.FromResult<object>(null);

        private static string configFile = "bodyAsString";
        public static JObject ConfigAsJson = JObject.Parse(configFile);
        public static JObject EZJsonObject = (JObject)ConfigAsJson["EZ"];
        public static JObject FogJsonObject = (JObject)ConfigAsJson["Fog"];
        public static JObject QiniuJsonObject = (JObject)ConfigAsJson["Qiniu"];

        public static string FOGBASEURL = (string)FogJsonObject["FogBaseURL"];
        public static string FOGLOGINURL = FOGBASEURL + FogJsonObject["FogLoginURL"];
        public static string FOGREGISTERURL = FOGBASEURL + FogJsonObject["FogRegisterURL"];
        public static string FOGRESETPASSWORDURL = FOGBASEURL + FogJsonObject["FogResetPasswordURL"];

        public static string EZBASEURL = (string)FogJsonObject["EZBaseURL"]; 
        public static string EZAPPKEY = (string)FogJsonObject["EZAppKey"];
        public static string EZCLOUDVERSION = (string)FogJsonObject["EZCloudVersion"];
        public static string EZGETSERVERTIMEMETHOD = (string)FogJsonObject["EZGetServerTimeMethod"];
        public static string EZGETTOKENMETHOD = (string)FogJsonObject["EZGetTokenMethod"];

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            //return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisCacheName"] + ",abortConnect=false,ssl=true,password=" + ConfigurationManager.AppSettings["RedisCachePassword"]);
            return ConnectionMultiplexer.Connect("hhnext.redis.cache.windows.net" + ",abortConnect=false,ssl=true,password=" + "2a8N8qu0H86t4UmXhWTwCjdmGq0EUZHYBqsCgB23j5Q=");

        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        public static string GetUnixTimeStamp()
        {
            return ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
        }
        public static string MD5ForPHP(string t)
        {
            byte[] data = Encoding.ASCII.GetBytes(t);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
        //public static string MD5Encode(string strText)
        //{
        //    MD5 md5 = new MD5CryptoServiceProvider();
        //    byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));
        //    return BitConverter.ToString(result).Replace("-", "").ToLower();
        //}
        public static string getSign(string appSecrett)
        {
            string timeStamp = GetUnixTimeStamp();
            return MD5ForPHP(appSecrett + timeStamp) + ", " + timeStamp;

        }
        public static string getActiveToken(string mac, string productSecertKey)
        {
            return MD5ForPHP(mac + productSecertKey);
        }

        public static void setRequest(HttpClient client)
        {
            string appId = (string)FogJsonObject["AppID"];
            string appSecret = (string)FogJsonObject["AppSecret"];

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Application-Id", appId);
            client.DefaultRequestHeaders.Add("X-Request-Sign", getSign(appSecret));
        }
        public static void EZSetRequest(EZRequest request)
        {
            StringBuilder signStr = new StringBuilder();
            string eZAppSecret = (string)EZJsonObject["EZAppSecret"];

            foreach (var item in request._params)
            {
                signStr.Append(item.Key + ":" + item.Value + ",");
            }
            signStr.Append("method:" + request.method + ",");
            signStr.Append("time:" + request._system.time + ",");
            signStr.Append("secret:" + eZAppSecret);

            //string test = "accessToken:f88c4dbb354711c9bf6597a4987dce90,deviceId:123456789,phone:18899998888,userId:ghhc4dbb354711c9bf6597a4987dce90,method:getDevice,time:1404443389,secret:yuc4dbb354sdsdfj77d76lkd86";

            //string s1= MyUtils.MD5ForPHP(test);
            string ss = signStr.ToString();
            request._system.sign = MD5ForPHP(ss);
        }
    }
}