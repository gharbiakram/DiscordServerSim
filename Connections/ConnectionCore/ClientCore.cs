using System;
using System.Collections.Generic;
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
        public  ClientCore()
        {


             ConnectToServer("127.0.0.1", 13000);



        }

        private async void ConnectToServer(string IpAdress,int Port)
        {

            try
            {

              _tcpClient = new();

               await _tcpClient.ConnectAsync(IpAdress, Port);

                Console.WriteLine("(client) : Client Connected SuccesFully");

                while (true)
                {
                   
                    Console.WriteLine("(client) : Enter The Message to send");

                    string message = Console.ReadLine();

                    if(message == "exit") { 
                        

                        
                        break; 
                    
                    }


                    SendMessage(message);

                }

                _tcpClient.Close();

                Console.WriteLine("(client) : Connection Terminated by the Client");


            }catch(Exception e)
            {
                Console.WriteLine(e.Message);

            } 
           






        }

        private void SendMessage(string message) {

            messageByte = Encoding.UTF8.GetBytes(message, 0, message.Length);

            _tcpClient.GetStream().Write(messageByte, 0, messageByte.Length); // write 
            
        
        
        
        }



    }
}
