using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomService.NDb.NData
{
	class ChatMessage
	{
		public int MsgId
		{
			get;
			set;
		}
				
		public string Contents
		{
			get;
			set;
		}
		
		public string NickName
		{
			get;
			set;
		}
		public string Email
		{
			get;
			set;
		}
	}
}
