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

            

             TcpClient client = await _tcpListener.AcceptTcpClientAsync();

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


               

                 _= Task.Run( () => HandleClientAsync(client) ); // runs on a seperate
                                                                //thread

               




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

            while (true)
            {
                int readBytes = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (readBytes == 0)
                {
                    // Client disconnected
                    Console.WriteLine("(server): Client disconnected.");
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, readBytes);
                Console.WriteLine($"(server): Client said {message}");

                await GenerateResponseAsync(client);
            }

            client.Close(); // Clean up when the client disconnects
        }

        private  async Task GenerateResponseAsync(TcpClient client)
        {

            var stream = client.GetStream();

            string responseMsg = "This is the server's response";

            var responseByte = Encoding.UTF8.GetBytes(responseMsg);

           await stream.WriteAsync(responseByte,0,responseByte.Length);
           await stream.FlushAsync();



        }    




    }
}
