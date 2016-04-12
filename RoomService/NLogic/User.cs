using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomService.NLogic
{
    public class User
    {
        private string _user_guid;
		private string _login_id;
		private string _user_name;
        private byte _gender;
        private DateTime _birth;
        private NRoom.List _joinList;
		private NRoom.List _joinCommitedList;
        private NRoom.List _createList;
		private NRoom.List _confirmList;
		
        public User(string user_guid, string login_id, string user_name, DateTime birth, byte gender, String deviceToken )
        {
            _user_guid = user_guid;
            _login_id = login_id;
            _birth = birth;
            _gender = gender;
			_user_name = user_name;
            _createList = new NRoom.List();
            _joinList = new NRoom.List();
			_joinCommitedList = new NRoom.List();
			_confirmList = new NRoom.List();
			DeviceToken = deviceToken;
        }

		public String DeviceToken
		{
			get;
			set;
		}

        public String UserGuid
        {
            get { return _user_guid; }
        }

        public String LoginId
        {
            get { return _login_id; }
        }

		public String UserName
		{
			get { return _user_name; }
		}

        public byte Gender
        {
            get { return _gender; }
        }

        public DateTime GetBirth
        {
            get { return _birth; } 
        }

        public NRoom.List JoinList
        {
            get { return _joinList; }
        }

        public NRoom.List CreateList
        {
            get { return _createList; }
        }

		public NRoom.List ConfirmList
		{
			get { return _confirmList; }
		}

		public NRoom.List JoinCommitedList
		{
			get { return _joinCommitedList; }
		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
