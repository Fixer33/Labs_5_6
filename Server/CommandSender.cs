using Shared;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal static class CommandSender
    {
        private static SimpleTcpServer _server;

        public static void Initialize(ref SimpleTcpServer server)
        {
            _server = server;
        }

        public static void SendCheckIdReply(string toIp, bool exists)
        {
            _server.Send(toIp, new byte[] { (byte)ServerPacket.IdExistsResponse, Convert.ToByte(exists) });
        }
    }
}
