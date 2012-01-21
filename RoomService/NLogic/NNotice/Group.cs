using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomService.NLogic.NNotice
{
	class Group
	{
		private Dictionary<int, Notice> _noticeList;
		private int _notice_indexer;
		private int _index;

		public Group( int index )
		{
			_notice_indexer = 0;
			_index = index;
			_noticeList = new Dictionary<int, Notice>();
		}

		public void Dispose()
		{
			_noticeList.Clear();
		}

		public int AddNotice(String title, String contents, User user, Room room, ref NOTICE_LIST notice_list)
		{
			if (_noticeList.Count > 10)
			{
				return -5;
			}

			Notice msg = new Notice();

			msg.Title = title;
			msg.Content = contents;
			msg.Index = ++_notice_indexer;
			msg.IptTime = DateTime.Now;

			try
			{
				_noticeList.Add(msg.Index, msg);

				this.UpdateNotice(user, room, msg.Index - 1, ref notice_list);

				return 0;
			}
			catch
			{
				return -3;
			}
		}

		public int DeleteNotice(int notice_index, User user)
		{
			bool bfound = _noticeList.Remove(notice_index);
			if (bfound == false)
			{
				return -4;
			}

			return notice_index;
		}

		public void UpdateNotice(User user,Room room, int last_update, ref NOTICE_LIST notice_list)
		{
			IEnumerable<KeyValuePair<int, Notice>> query = from chat in _noticeList where chat.Key > last_update select chat;
			int nCount = query.Count<KeyValuePair<int, Notice>>();

			notice_list.count = nCount;
			notice_list.room_index = room.Index;
			notice_list.group = this._index;

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
	}
}
