﻿using System;
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


                    await SendMessageAsync(message);

                    await RecieveResponseAsync();




                }

                _tcpClient.Close();


                Console.WriteLine("(client) : Connection Terminated by the Client");


            }catch(Exception e)
            {
                Console.WriteLine(e.Message);

            } 
           






        }

        private async Task SendMessageAsync(string message) {

           

            messageByte = Encoding.UTF8.GetBytes(message, 0, message.Length);

            var stream = _tcpClient.GetStream();

            await stream.WriteAsync(messageByte, 0, messageByte.Length); // write 

            await stream.FlushAsync();
           
        }

        private async Task RecieveResponseAsync()
        {

            var stream = _tcpClient.GetStream();
            byte[] messageBytes = new byte[256];

            int readBytes = await stream.ReadAsync(messageBytes, 0, messageBytes.Length);

            if (readBytes > 0)
            {
                string response = Encoding.UTF8.GetString(messageBytes, 0, readBytes);
                Console.WriteLine($"(client): Server responded with: {response}");
            }

        }



    }
}
