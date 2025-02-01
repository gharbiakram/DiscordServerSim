using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
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

        private ConcurrentBag<TcpClient> connectedClients = new();

        public ServerCore()
        {

               InitialzeServerAsync("127.0.0.1", 13000);

        }


        private async Task InitialzeServerAsync(string adrIP , int port)
        {

            try
            {
            IPAddress IP = IPAddress.Parse(adrIP);
        
            _tcpListener = new(IP, port);
            _tcpListener.Start();

            Console.WriteLine("(server) : Server Started");

            buffer = new byte[256];

                //below i only want one client to connect to the server , adding a while(true) loop
                //later to handle multiple ones

                while (true)
                {

                    using TcpClient client = await _tcpListener.AcceptTcpClientAsync();

                    connectedClients.Add(client);

                    await BroadCastMessageAsync(client, await HandleClientAsync(client));


                }
            


                // Explanation of why an asynchronous approach is vi:
                // The AcceptTcpClientAsync() method is asynchronous, meaning it does not block the main thread
                // while waiting for a client to connect. In contrast, the synchronous AcceptTcpClient() method
                // would block the thread indefinitely until a client connects.

                // By using await, the execution is paused at this point, allowing the thread to return to 
                // the caller to handle other tasks (like initializing the client). When a client sends 
                // a connection request, the method resumes execution on the same thread, successfully 
                // accepting the client connection. This approach improves responsiveness and avoids 
                // potential deadlocks in scenarios where the client and server are dependent on each other 
                // to proceed with their tasks.

                // In essence, AcceptTcpClientAsync() allows the server to handle the connection in a 
                // non-blocking way, ensuring that both the server and client can operate concurrently
                // without being stuck waiting for each other.


               

                

               




            }
            catch(Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");

            } 




        }

        private async Task<string?> HandleClientAsync(TcpClient client)
        {
            var stream = client.GetStream();
            byte[] buffer = new byte[256];

            
                int readBytes = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (readBytes == 0)
                {
                    // Client disconnected
                    Console.WriteLine("(server): Client disconnected.");

                    return null;
                    
                }

                string message = Encoding.UTF8.GetString(buffer, 0, readBytes);


               return message;
            

             
        }

        private async Task BroadCastMessageAsync(TcpClient client,string? Message)
        {

            try { 
            
                if(Message == null)
                {

                    throw new IOException("Can't BroadCast a NULL value");


                }
            
            
                var stream = client.GetStream();

               

                foreach( TcpClient c in connectedClients)
                {

                    if(c != client)
                    {


                        var responseByte = Encoding.UTF8.GetBytes(Message);

                        await stream.WriteAsync(responseByte,0,responseByte.Length);

                        await stream.FlushAsync();



                    }

                }

            } 
            catch (Exception ex) 
            {

                Console.WriteLine(ex.Message);
            
            }




        }    
        private void InsertIntoDB()
        {









        }



    }
}
