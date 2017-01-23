using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Threading;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace hhnextWeb
{


    [HubName("DataTickerHub")]
    public class DataTickerHub : Hub
    {
        private  RedisDataTicker _redisDataTicker;

        public DataTickerHub() : this(RedisDataTicker.Instance) { }

        public DataTickerHub(RedisDataTicker redisDataTicker)
        {
            _redisDataTicker = redisDataTicker;
        }



        public void GetAllData()
        {
            string name = Context.User.Identity.Name;
            _redisDataTicker.GetAllData(name);
        }
        public void SendCommand(string data)
        {
            string name = Context.User.Identity.Name;
            _redisDataTicker.SendCommandToQueue(name,data);
        }














        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            _redisDataTicker.userConnections.AddOrUpdate(
                    name,
                    key =>
                    {
                        return new HashSet<string>() { Context.ConnectionId };
                    },
                    (key,oldValue) =>
                    {
                        oldValue.Add(Context.ConnectionId);
                        return oldValue;
                    }
            );

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _redisDataTicker.userConnections[name].Remove( Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            _redisDataTicker.userConnections.AddOrUpdate(
                   name,
                   key =>
                   {
                       return new HashSet<string>() { Context.ConnectionId };
                   },
                   (key, oldValue) =>
                   {
                       oldValue.Add(Context.ConnectionId);
                       return oldValue;
                   }
           );

           return base.OnReconnected();
        }

    }
}