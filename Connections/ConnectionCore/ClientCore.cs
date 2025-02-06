using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCore
{
    public class ClientCore
    {

      private TcpClient _tcpClient;

        private byte[] messageByte;

        string _name;
        public  ClientCore(string name)
        {

            _name = name;

             ConnectToServer("127.0.0.1", 13000);



        }
        
        private async void ConnectToServer(string IpAdress,int Port)
        {

            try
            {


                _tcpClient = new();

                await _tcpClient.ConnectAsync(IpAdress, Port);

                Console.WriteLine($"({_name}) : Client Connected SuccesFully");

                var stream = _tcpClient.GetStream();

                byte[] nameData = Encoding.UTF8.GetBytes(_name + "\n");
                await stream.WriteAsync(nameData, 0, nameData.Length);
                await stream.FlushAsync();




            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

            } 
           






        }

        public async Task<bool> SendMessageAsync(string message) {

            try {

                if (!_tcpClient.Connected) { return false; }

                Console
                    .WriteLine("Sending The Message To the server");

                string formattedMessage = $"{_name} : {message}";

                messageByte = Encoding.UTF8.GetBytes(formattedMessage, 0, formattedMessage.Length);

                var stream = _tcpClient.GetStream();

                await stream.WriteAsync(messageByte, 0, messageByte.Length); // write on stream

                await stream.FlushAsync();


                

                return true;
                

            }
            catch(Exception e)
            {

                Console.WriteLine(e.Message);

                return false; 
            }

            
           
        }

        public async Task<string> RecieveResponseAsync()
        {

            var stream = _tcpClient.GetStream();
            byte[] messageBytes = new byte[256];

            int readBytes = await stream.ReadAsync(messageBytes, 0, messageBytes.Length);

            if (readBytes > 0)
            {
                string response = Encoding.UTF8.GetString(messageBytes, 0, readBytes);
                Console.WriteLine($"({_name}): Server responded with: {response}");

                return response;
            }
            else
            {
                        
                
                throw new Exception();



            }


        }



    }
}
