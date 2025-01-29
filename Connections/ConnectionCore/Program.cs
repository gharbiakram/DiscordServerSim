namespace ConnectionCore
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
          

            new ServerCore();

           

            new ClientCore();


           

            await Task.Delay(-1); // keep it alive


            

            
        }
    }
}
