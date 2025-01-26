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





            var stream = client.GetStream();

            int ReadChar;

            while( ( ReadChar =  stream.Read(buffer,0,buffer.Length) ) != 0 )
            {
                    //If the server handles multiple clients at the same time,
                    //ReadAsync allows the I/O operations to be non-blocking,
                    //freeing up the thread to serve other clients or tasks.

                    string Message = Encoding.UTF8.GetString(buffer, 0, ReadChar);

                    

                    Console.WriteLine($"Client said {Message}");
            }




            }catch(Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");

            } 




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
