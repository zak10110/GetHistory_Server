using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace GetHistory_Server
{
    public class Server
    {

        public int port { get; set; }
        public string IpAdr { get; set; }
        public int bytes { get; set; }
        public Socket socket { get; set; }
        public IPEndPoint ipPoint { get; set; }
        public List<Socket> clients { get; set; }
        public byte[] data { get; set; }
        public int ClientID { get; set; }
        public Server(int port, string IpAdr)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipPoint = new IPEndPoint(IPAddress.Parse(IpAdr), port);
            this.port = port;
            this.IpAdr = IpAdr;
            clients = new List<Socket>();
            ClientID = 0;
        }


        public void Start()
        {

            socket.Bind(ipPoint);
            socket.Listen(10);

        }

        public void Conection()
        {
            while (true)
            {


                this.clients.Add(socket.Accept());
                //Task.Factory.StartNew(() => { Console.WriteLine(GetMsg(clients.Last()));});
                ClientID++;


            }

        }
        public void SendMsg(List<byte> data, int index)
        {
            clients[index].Send(data.ToArray());
        }

        public static List<byte> StringToBytes(string str)
        {
            return Encoding.Unicode.GetBytes(str).ToList();
        }
        public void SendMsgToClien(Socket clientSoc, string msg)
        {

            clientSoc.Send(Encoding.Unicode.GetBytes(msg));


        }

        public void SendMsgToALL()
        {
            for (int i = 0; i < this.clients.Count(); i++)
            {

                SendMsgToClien(this.clients[i], "Welcome To Server");

            }


        }


        public string GetMsg(Socket clientSoc)
        {

            StringBuilder stringBuilder = new StringBuilder();
            this.data = new byte[2000000];
           
            do
            {
                this.bytes = clientSoc.Receive(data);
                stringBuilder.Append(Encoding.Unicode.GetString(this.data, 0, this.bytes));
            } while (socket.Available > 0);


            return stringBuilder.ToString();
        }


    }
}
