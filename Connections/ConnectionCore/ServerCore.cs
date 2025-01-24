using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCore
{
    public class ServerCore
    {


        private TcpListener _tcpListener;

        private byte[] buffer;

        public ServerCore()
        {

            ServerStart();

        }


        private void ServerStart()
        {

            IPAddress adrIP = IPAddress.Parse("127.0.0.1");
            var Port = 13000;
            _tcpListener = new(adrIP, Port);
            _tcpListener.Start();

            Console.WriteLine("Server Started");

            buffer = new byte[256];

            using TcpClient _tcpClient = _tcpListener.AcceptTcpClient();

            Console.WriteLine("Client Connected");

            var clientStream = _tcpClient.GetStream();

            int totalReadCharFromStream;

            while (( totalReadCharFromStream = clientStream.Read(buffer,0,buffer.Length)) != 0 ){


                var IncomingMessage=Encoding.UTF8.GetString(buffer, 0, totalReadCharFromStream);

                Console.WriteLine(IncomingMessage);



            }

            







        }
    




    }
}
