using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RoomService.NLogic
{
    public class Room : IDisposable
    {
        static public UInt32 _indexer = 1000;

        private RoomSearchKey _search_key;

        private String _duration;
        private String _name;
        private String _comment;
        private byte _maxuser;
		private UInt32 _index;
		private Int32 _notice_indexer;
        private NUser.List _userList;
		private User _master_user;

		private List<ChatMessage> _instantChat;
		private Dictionary<int, Notice> _noticeList;

        public Room(	String duration,
						RoomSearchKey search_key,
						String name,
						String comment,
						byte maxuser )
        {
            _userList = new NUser.List();
            _duration = duration;
            _name = name;
            _comment = comment;
            _maxuser = maxuser;
            _index = _indexer++;
            _search_key = new RoomSearchKey();
            _search_key = search_key;
			_instantChat = new List<ChatMessage>();
			_notice_indexer = 0;
			_noticeList = new Dictionary<int, Notice>();
        }

        public void Dispose()
        {
			_userList.RemoveAll();
			_instantChat.Clear();
			_noticeList.Clear();

			_master_user = null;

			Console.WriteLine("Room Dispose() index {0} ",  _index );
			_index = 0;
        }

        public int Join(User user)
        {
			if (_userList.GetCount() >= _maxuser)
			{
				return -3;	// max count over
			}

            if (_userList.InsertUser(user) == false)
            {
                return -4;	// duplicate
            }

            bool bResult = user.JoinList.Insert(this);
			Trace.Assert(bResult == true);

			return 0;
        }

        public bool Leave(User user)
        {
            bool result = _userList.RemoveUser(user);
            if( result == false )
            {
                return false;
            }

            return user.JoinList.Remove(this);
        }

        public NUser.List UserList
        {
            get { return _userList; }
        }

        public UInt32 Index
        {
            get { return _index; }
        }

        public String Duration
        {
            get { return _duration; }
        }

        public String Name
        {
            get { return _name; }
        }

        public String Commment
        {
            get { return _comment; }
        }

        public byte MaxUser
        {
            get { return _maxuser; }
        }

        public RoomSearchKey SearchKey
        {
            get { return _search_key; }
        }

		public bool SelectMaster(User user)
		{
			if (UserList.FindUser(user.UserGuid) == null)
			{
				return false;
			}

			_master_user = user;

			return true;
		}

		public User GetMaster()
		{
			Trace.Assert(_master_user != null);
			return _master_user;
		}

		public int AddNotice(String title, String contents, User user, ref NOTICE_LIST notice_list)
		{
			if (!user.UserGuid.Equals(_master_user.UserGuid) )
			{
				return -1;
			}

			if ( _noticeList.Count > 10)
			{
				return -2;
			}

			Notice msg = new Notice();

			msg.Title = title;
			msg.Content = contents;
			msg.Index = ++_notice_indexer;
			msg.IptTime = DateTime.Now;

			try
			{
				_noticeList.Add(msg.Index, msg);

				this.UpdateNotice(user, msg.Index - 1, ref notice_list);

				return 0;
			}
			catch
			{
				return -3;
			}
		}

		public int DeleteNotice( int notice_index, User user )
		{
			if (!user.UserGuid.Equals(_master_user.UserGuid))
			{
				return -3;
			}

			bool bfound = _noticeList.Remove(notice_index);
			if (bfound == false)
			{
				return -4;
			}

			return notice_index;
		}

		public void UpdateNotice( User user, int last_update, ref NOTICE_LIST notice_list)
		{
			IEnumerable<KeyValuePair<int,Notice>> query = from chat in _noticeList where chat.Key > last_update select chat;
			int nCount = query.Count<KeyValuePair<int, Notice>>();

			notice_list.count = nCount;
			notice_list.room_index = this.Index;
			notice_list.NOTICE = new NOTICE_LISTNOTICE[nCount];

			int nIndex = 0;
			foreach (KeyValuePair<int, Notice> msg in query)
			{
				notice_list.NOTICE[nIndex] = new NOTICE_LISTNOTICE();

				notice_list.NOTICE[nIndex].index = msg.Value.Index;
				notice_list.NOTICE[nIndex].date_time = msg.Value.IptTime;
				notice_list.NOTICE[nIndex].title = msg.Value.Title;
				notice_list.NOTICE[nIndex].Value = msg.Value.Content;

				nIndex++;
			}
		}

		public void InsertMessage(String contents, User user, int last_update, ref CHAT_LIST chat_list)
		{
			ChatMessage msg = new ChatMessage();

			msg.Content = contents;
			msg.Index = _instantChat.Count + 1;
			msg.IptTime = DateTime.Now;
			msg.UserNickName = user.UserName;

			_instantChat.Add(msg);

			this.UpdateMessage(user, last_update, ref chat_list);
		}

		public void UpdateMessage(User user, int last_update, ref CHAT_LIST chat_list )
		{
			// 50 개만 셀렉트하기
			IEnumerable<ChatMessage> query = from chat in _instantChat where chat.Index > last_update select chat;
			int nCount = query.Count<ChatMessage>();

			chat_list.count = nCount;
			chat_list.room_index = this.Index;
			chat_list.CHAT = new CHAT_LISTCHAT[nCount];

			int nIndex = 0;
			foreach (ChatMessage msg in query)
			{
				chat_list.CHAT[nIndex] = new CHAT_LISTCHAT();

				chat_list.CHAT[nIndex].chat_index = msg.Index;
				chat_list.CHAT[nIndex].date_time = msg.IptTime;
				chat_list.CHAT[nIndex].nick_name = msg.UserNickName;
				chat_list.CHAT[nIndex].Value = msg.Content;

				nIndex++;
			}
		}
    }
}
