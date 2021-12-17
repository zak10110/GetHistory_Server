using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHistory_Server
{
	public class HistoryItem
	{
		public string URL { get; set; }
		public string Title { get; set; }
		public DateTime VisitedTime { get; set; }
	}
}
