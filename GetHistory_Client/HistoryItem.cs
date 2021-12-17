using System;
using System.Collections.Generic;
using System.Text;

namespace GetHistory_Client
{
	public class HistoryItem
	{
		public string URL { get; set; }
		public string Title { get; set; }
		public DateTime VisitedTime { get; set; }
	}
}
