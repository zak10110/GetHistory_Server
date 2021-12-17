using System;

namespace GetHistory_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 8000);
            client.CreateIPEndPoint();
            client.Conect();
            if (client.TakeMSGFromServ()== "gethistory")
            {
                client.GetAndSendHistoryToServ();
            }
            Console.ReadLine();


        }
    }
}
