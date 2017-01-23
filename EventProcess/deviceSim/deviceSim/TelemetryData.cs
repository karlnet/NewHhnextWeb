using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deviceSim
{
    public enum TelemetryTypes
    {
        A = 0, B = 1, C = 2
    }
    public enum DCAC
    {
        DC = 0, AC = 1
    }

    public class TelemetryData
    {
        static Random random = new System.Random();
        public object Data { get; set; }
        public string Timestamp { get; set; }
        //public TelemetryTypes Type { get; set; }
        public string DeviceId { get; set; }
        //public string UID { get; set; }
        //public DCAC DCorAC { get; set; }
        //public string ADSLor3G { get; set; }
        //public string Message { get; set; }
        public static TelemetryData Random(string seqNo, string msg)
        {
            var ret = new TelemetryData()
            {
                Data = 
                new {p1= random.Next(0, 2), p2= random.Next(20, 40), p3= random.Next(0, 2), p4= random.Next(50, 60) },
                Timestamp = string.Format("{0:G}", DateTime.Now),
                //Type = (TelemetryTypes)random.Next(0, 2),
                DeviceId = "TYPE000012345678"
                //UID = "UID-" + Guid.NewGuid().ToString(),
                //DCorAC = (DCAC)random.Next(0, 1),
                //ADSLor3G = random.Next(100) >= 50 ? "ADSL" : "3G",
                //Message = msg
            };
            return ret;
        }
    }

}
