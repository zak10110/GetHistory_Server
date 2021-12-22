using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHistory_Server
{
    public class URL
    {
        public string url { get; set; }
        public string Title { get; set; }
        public string BrowserName { get; set; }
        public URL(string url, string Title, string BrowserName)
        {
            this.url = url;
            this.Title = Title;
            this.BrowserName = BrowserName;
        }
    }
}
