using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomService.NDb.NData
{
	class JoinedUser
	{
		public DateTime Birth
		{
			get;
			set;
		}

		public String LoginId
		{
			get;
			set;
		}
		public Guid UserId
		{
			get;
			set;
		}
		public String NickName
		{
			get;
			set;
		}
		public byte Gender
		{
			get;
			set;
		}
	}
}
