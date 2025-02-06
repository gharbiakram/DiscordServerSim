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

        private ConcurrentDictionary<TcpClient,DateTime> connectedClients = new();

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

                     TcpClient client = await _tcpListener.AcceptTcpClientAsync();

                    Console.WriteLine("CLient Connected");

                    connectedClients[client] = DateTime.UtcNow;

                   _ =Task.Run( ()=>   HandleClientAsync(client) );


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

        private async Task HandleClientAsync(TcpClient client)
        {
            var stream = client.GetStream();
            byte[] buffer = new byte[256];
            try
            {






                while (client.Connected)
                {


                    int readBytes = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (readBytes == 0)
                    {


                        break;
                    
                    
                    }


                    string message = Encoding.UTF8.GetString(buffer, 0, readBytes);


                    Console.WriteLine(message);



                    await BroadCastMessageAsync(client, message);


                }
            }
            catch (Exception ex)
            {


                Console.WriteLine(ex.ToString());
            }
            finally { 
            
                if( client.Connected == false)
                {

                    connectedClients.Remove(client , out _);

                    client.Close();


                }


            
            
            }
        }












        private async Task BroadCastMessageAsync(TcpClient sender,string? Message)
        {

            try { 
            
                if(Message == null)
                {

                    throw new IOException("Can't BroadCast a NULL value");


                }
            
            
               
                var responseByte = Encoding.UTF8.GetBytes(Message);


                foreach ( TcpClient c in connectedClients.Keys)
                {

                    if(c != sender)
                    {


                        var stream = c.GetStream();

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




            //DB




        }



    }
}
