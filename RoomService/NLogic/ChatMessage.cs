using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomService.NLogic
{
	class ChatMessage
	{
		public DateTime IptTime
		{
			get;
			set;
		}
		public String Content
		{
			get;
			set;
		}
		public int Index
		{
			get;
			set;
		}
		public String UserNickName
		{
			get;
			set;
		}
	}
}
