using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomService.NDb.NData
{
	class JoinedRoom
	{
		public int Index
		{
			get;
			set;
		}

		public Guid MasterUserId
		{
			get;
			set;
		}

		public String Name
		{
			get;
			set;
		}

		public String Comment
		{
			get;
			set;
		}

		public byte Category
		{
			get;
			set;
		}

		public byte LocationMain
		{
			get;
			set;
		}

		public byte LocationSub
		{
			get;
			set;
		}

		public String Duration
		{
			get;
			set;
		}
		public byte CurrentUser
		{
			get;
			set;
		}
		public byte MaxUser
		{
			get;
			set;
		}

		public bool Commited
		{
			get;
			set;
		}

		public System.Nullable<System.DateTime> CreateDate
		{
			get;
			set;
		}

		public System.Nullable<System.DateTime> CommitedDate
		{
			get;
			set;
		}
	}
}
