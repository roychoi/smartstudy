using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;

using JdSoft.Apple.Apns.Notifications;
using System.Runtime.Serialization;

namespace RoomService
{
    [DataContract]
	public struct RoomSearchKey
	{
        [DataMember]
		public int _category;
        [DataMember]
		public int _location_main;
        [DataMember]
		public int _location_sub;
	};

    [DataContract]
    public class MyFaultException
    {
        private string _reason;

        [DataMember]
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
    }

	[ServiceContract (Namespace= "http://www.studyheyo.co.kr" )]
	public interface IRoom
	{
        [OperationContract]
        [FaultContract(typeof(MyFaultException))]
        ROOM_RESULT Test(String user_no);
        
        [OperationContract]
        [FaultContract(typeof(MyFaultException))]
        String Test2(String user_no);
		
		[OperationContract]
		[FaultContract(typeof(MyFaultException))]
		JOIN_ROOM_DETAIL GetLoginUser();

		[OperationContract]
		[FaultContract(typeof(MyFaultException))]
		ROOM_RESULT Push(String deviceToken, String Message, int badge);

        [OperationContract]
        bool LoginUser(String user_no, String login_id, String user_name, DateTime birth, byte gender, String deviceToken);
      
        [OperationContract]
        UPDATE_DEVICE_INFO UpdateUserDevice(String user_guid, String deviceToken);

        [OperationContract]
        ROOM_RESULT CreateRoom(String user_no, RoomSearchKey key, String name, String comment, String duration, int maxuser);

        [OperationContract]
        ROOM_INFO_LIST MyRoomList(String user_no);

        [OperationContract]
        ROOM_SUMMARY_LIST AllRoomList(RoomSearchKey key, String user_no);

        [OperationContract]
        JOIN_ROOM_DETAIL JoinRoomDetail(UInt32 room_index, String user_no);

        [OperationContract]
        ROOM_RESULT JoinRoom(String user_no, UInt32 room_index);

        [OperationContract]
        ROOM_RESULT LeaveRoom(String user_no, UInt32 room_index);

        [OperationContract]
        ROOM_RESULT CommitRoom(String user_no, UInt32 room_index);

        [OperationContract]
        CHAT_LIST Chat(UInt32 room_index, String user_no, int local_index, int last_update, String content);

        [OperationContract]
        CHAT_LIST ChatUpdate(UInt32 room_index, String user_no, int last_update);

        [OperationContract]
        NOTICE_LIST CreateNotice(UInt32 room_index, String user_no, int group, String title, String content);

        [OperationContract]
		NOTICE_LIST DeleteNotice(UInt32 room_index, String user_no, int group, int notice_index);

        [OperationContract]
        NOTICE_LIST UpdateNotice(UInt32 room_index, String user_no, int group, int last_update);
	}

	public class RoomWCFService : IRoom
	{
        static public int nValue = 10;
		static public NLogic.NRoom.List _roomList = new NLogic.NRoom.List();
		static public Dictionary<RoomSearchKey, NLogic.NRoom.List> _roomTree = new Dictionary<RoomSearchKey, NLogic.NRoom.List>();

		static public NLogic.NUser.List _userList = new NLogic.NUser.List();
		static public NApns.Provider _apnsProvider = new NApns.Provider("iphone_dev.p12", "roy3513!", true);

        public ROOM_RESULT Test(String user_no)
        {
            ROOM_RESULT result = new ROOM_RESULT();
            result.crud = user_no;
            result.reason_sort = -1;
            return result;
        }

        public String Test2(String user_no)
        {
            try
            {
                //NApns.Provider apnsProvider = new NApns.Provider("iphone_dev.p12", "roy3513!", true);

                return user_no + "TEST22222";
            }
            catch (Exception exp)
            {
                MyFaultException theFault = new MyFaultException();
                theFault.Reason = "Some Error " + exp.Message.ToString();
                throw new FaultException<MyFaultException>(theFault);
            }
        }
		public JOIN_ROOM_DETAIL GetLoginUser()
		{
			try
			{
				JOIN_ROOM_DETAIL join_room_detail = new JOIN_ROOM_DETAIL();

				join_room_detail.category = 0;
				join_room_detail.index = 0;
				join_room_detail.name = "";
				join_room_detail.comment = "";
				join_room_detail.location_main = 0;
				join_room_detail.location_sub = 0;
				join_room_detail.max_user = 0;
				join_room_detail.duration = "";

				Int32 current_member_count = _userList.GetCount();
				join_room_detail.MEMBER_LIST = new JOIN_ROOM_DETAILMEMBER_LIST();
				join_room_detail.MEMBER_LIST.count = (byte)current_member_count;
				join_room_detail.MEMBER_LIST.MEMBER = new JOIN_ROOM_DETAILMEMBER_LISTMEMBER[current_member_count];

				Int32 count = 0;
				foreach (KeyValuePair<String, NLogic.User> pair in _userList)
				{
					NLogic.User joined_user = pair.Value;

					join_room_detail.MEMBER_LIST.MEMBER[count] = new JOIN_ROOM_DETAILMEMBER_LISTMEMBER();
					DateTime birth = joined_user.GetBirth;
					DateTime current = DateTime.Today;

					if (current.Year < birth.Year)
					{
						join_room_detail.MEMBER_LIST.MEMBER[count].age = 0;
					}
					else
					{
						join_room_detail.MEMBER_LIST.MEMBER[count].age = (byte)(current.Year - birth.Year);
					}

					join_room_detail.MEMBER_LIST.MEMBER[count].gender = joined_user.Gender;
					join_room_detail.MEMBER_LIST.MEMBER[count].loginid = joined_user.LoginId;
					join_room_detail.MEMBER_LIST.MEMBER[count].user_name = joined_user.UserName;

					count++;
				}

				return join_room_detail;
			}

			catch (Exception exp)
			{
				MyFaultException theFault = new MyFaultException();
				theFault.Reason = "Some Error " + exp.Message.ToString();
				throw new FaultException<MyFaultException>(theFault);
			}
		}

		//public NotificationService Test(string p12File, string p12FilePassword, bool sandbox)
		//{
		//    string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);

		//    NotificationService _service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);

		//    _service.SendRetries = 5;		//5 retries before generating notificationfailed event
		//    _service.ReconnectDelay = 5000; //5 seconds

		//    _service.Error += new NotificationService.OnError(NApns.Provider.service_Error);
		//    _service.NotificationTooLong += new NotificationService.OnNotificationTooLong(NApns.Provider.service_NotificationTooLong);

		//    _service.BadDeviceToken += new NotificationService.OnBadDeviceToken(NApns.Provider.service_BadDeviceToken);
		//    _service.NotificationFailed += new NotificationService.OnNotificationFailed(NApns.Provider.service_NotificationFailed);
		//    _service.NotificationSuccess += new NotificationService.OnNotificationSuccess(NApns.Provider.service_NotificationSuccess);
		//    _service.Connecting += new NotificationService.OnConnecting(NApns.Provider.service_Connecting);
		//    _service.Connected += new NotificationService.OnConnected(NApns.Provider.service_Connected);
		//    _service.Disconnected += new NotificationService.OnDisconnected(NApns.Provider.service_Disconnected);

		//    return _service;
		//}

		public ROOM_RESULT Push(String deviceToken, String Message, int badge )
		{
			ROOM_RESULT res = new ROOM_RESULT();
			JdSoft.Apple.Apns.Notifications.Notification
			alertNotification = new JdSoft.Apple.Apns.Notifications.Notification(deviceToken);

			alertNotification.Payload.Alert.Body = Message;
			alertNotification.Payload.Sound = "default";
			alertNotification.Payload.Badge = badge;

            //string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iphone_dev.p12");

            //NotificationService _service = new NotificationService(true, p12Filename, "roy3513!", 1);

            //_service.SendRetries = 10;		//5 retries before generating notificationfailed event
            //_service.ReconnectDelay = 10000; //5 seconds

            //_service.Error += new NotificationService.OnError(NApns.Provider.service_Error);
            //_service.NotificationTooLong += new NotificationService.OnNotificationTooLong(NApns.Provider.service_NotificationTooLong);

            //_service.BadDeviceToken += new NotificationService.OnBadDeviceToken(NApns.Provider.service_BadDeviceToken);
            //_service.NotificationFailed += new NotificationService.OnNotificationFailed(NApns.Provider.service_NotificationFailed);
            //_service.NotificationSuccess += new NotificationService.OnNotificationSuccess(NApns.Provider.service_NotificationSuccess);
            //_service.Connecting += new NotificationService.OnConnecting(NApns.Provider.service_Connecting);
            //_service.Connected += new NotificationService.OnConnected(NApns.Provider.service_Connected);
            //_service.Disconnected += new NotificationService.OnDisconnected(NApns.Provider.service_Disconnected);

			//Queue the notification to be sent
            if (_apnsProvider.Service.QueueNotification(alertNotification))
			{
				res.crud = "Notification Queued!";

				NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "Notification Queued! by TraceEvent");
				NApns.Provider._source.Flush();
			}
			else
			{
				res.crud = "Notification Failed to be Queued!";
				Trace.WriteLine("Notification Failed to be Queued by Trace.WriteLine()");
				
                NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "Notification Failed to be Queued! by TraceEvent()");
				NApns.Provider._source.Flush();
			}

			return res;
		}

        public bool LoginUser(String user_guid, String login_id, String user_name, DateTime birth, byte gender, String deviceToken)
        {
            NLogic.User user = _userList.FindUser(user_guid);
            if (user != null)
            {
                Console.WriteLine(String.Format("Already FindUser {0} : {1}", user.LoginId, user.UserGuid));
                return false;
            }

            user = new NLogic.User(user_guid, login_id, user_name, birth, gender, deviceToken);
            Console.WriteLine(String.Format("NewUser {0} : {1}", user.LoginId, user.UserGuid));
            return _userList.InsertUser(user);
        }

        public UPDATE_DEVICE_INFO UpdateUserDevice(String user_guid, String deviceToken)
        {
            UPDATE_DEVICE_INFO update_device_info = new UPDATE_DEVICE_INFO();

            NLogic.User user = _userList.FindUser(user_guid);
            if (user == null)
            {
                update_device_info.user_no = user_guid;
                update_device_info.result_code = -1;

                return update_device_info;
            }

            if (deviceToken.Length != Notification.DEVICE_TOKEN_STRING_SIZE)
            {
                update_device_info.user_no = user_guid;
                update_device_info.result_code = -2;

                return update_device_info;
            }

            user.DeviceToken = deviceToken;

            update_device_info.login_id = user.LoginId;
            update_device_info.result_code = 0;

            return update_device_info;
        }


        public ROOM_RESULT CreateRoom(String user_no, RoomSearchKey key, String name, String comment, String duration, int maxuser)
        {
            ROOM_RESULT result = new ROOM_RESULT();
            result.crud = "CR";

            NLogic.User masterUser = _userList.FindUser(user_no);
            if (masterUser == null)
            {
                result.reason_sort = -1;
                return result;
            }

            NLogic.NRoom.List findRoomList = null;

            try
            {
                if (_roomTree.TryGetValue(key, out findRoomList) == false)
                {
                    findRoomList = new NLogic.NRoom.List();
                    _roomTree.Add(key, findRoomList);
                }
            }
            catch (Exception)
            {
                result.reason_sort = -3;
                return result;
            }

            Trace.Assert(findRoomList != null);

            NLogic.Room room = new NLogic.Room(duration, key, name, comment, (byte)maxuser);
            bool bResult = findRoomList.Insert(room);

            if (bResult == false)
            {
                Console.WriteLine("CreateRoom findRoomList Insert failed...");

                result.reason_sort = -2;
                return result;
            }

            bResult = _roomList.Insert(room);

            if (bResult == false)
            {
                Console.WriteLine("CreateRoom _roomList Insert failed...");

                result.reason_sort = -2;
                return result;
            }

            Trace.Assert(room.UserList.InsertUser(masterUser) == true);
            Trace.Assert(masterUser.CreateList.Insert(room) == true);

            Trace.Assert(room.SelectMaster(masterUser) == true);

            result.reason_sort = 0;
			result.room_index = room.Index;

            Console.WriteLine("CreateRoom Success... Index {0} name {1} maxUser {2}", room.Index, room.Name, room.MaxUser);

            return result;
        }

        public ROOM_INFO_LIST MyRoomList(String user_no)
        {
            ROOM_INFO_LIST room_info_list = new ROOM_INFO_LIST();

            NLogic.User user = _userList.FindUser(user_no);
            if (user == null)
            {
                return room_info_list;
            }

            Console.WriteLine(String.Format("MyRoomList {0} : {1}", user.LoginId, user.UserGuid));

            int room_count = user.JoinList.Count;
            room_count += user.JoinCommitedList.Count;

            room_info_list.JOIN_INFO = new ROOM_INFO_LISTROOM1[room_count];

            int index = 0;

            foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.JoinCommitedList)
            {
                NLogic.Room room = pair.Value;

                room_info_list.JOIN_INFO[index] = new ROOM_INFO_LISTROOM1();
                room_info_list.JOIN_INFO[index].index = room.Index;
                room_info_list.JOIN_INFO[index].name = room.Name;
                room_info_list.JOIN_INFO[index].commited = 1;
                room_info_list.JOIN_INFO[index].comment = room.Commment;
                room_info_list.JOIN_INFO[index].category = room.SearchKey._category;
                room_info_list.JOIN_INFO[index].location_main = room.SearchKey._location_main;
                room_info_list.JOIN_INFO[index].location_sub = room.SearchKey._location_sub;
                room_info_list.JOIN_INFO[index].current_user = (byte)room.UserList.GetCount();
                room_info_list.JOIN_INFO[index].max_user = room.MaxUser;
                room_info_list.JOIN_INFO[index].duration = room.Duration;

                Console.WriteLine("JoinCommitedList index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

                index++;
            }

            foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.JoinList)
            {
                NLogic.Room room = pair.Value;

                room_info_list.JOIN_INFO[index] = new ROOM_INFO_LISTROOM1();
                room_info_list.JOIN_INFO[index].index = room.Index;
                room_info_list.JOIN_INFO[index].name = room.Name;
                room_info_list.JOIN_INFO[index].commited = 0;
                room_info_list.JOIN_INFO[index].comment = room.Commment;
                room_info_list.JOIN_INFO[index].category = room.SearchKey._category;
                room_info_list.JOIN_INFO[index].location_main = room.SearchKey._location_main;
                room_info_list.JOIN_INFO[index].location_sub = room.SearchKey._location_sub;
                room_info_list.JOIN_INFO[index].current_user = (byte)room.UserList.GetCount();
                room_info_list.JOIN_INFO[index].max_user = room.MaxUser;
                room_info_list.JOIN_INFO[index].duration = room.Duration;

                Console.WriteLine("JoinList index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

                index++;
            }

            room_count = user.CreateList.Count;
            room_count += user.ConfirmList.Count;

            room_info_list.CREATE_INFO = new ROOM_INFO_LISTROOM[room_count];

            index = 0;

            foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.ConfirmList)
            {
                NLogic.Room room = pair.Value;

                room_info_list.CREATE_INFO[index] = new ROOM_INFO_LISTROOM();
                room_info_list.CREATE_INFO[index].index = room.Index;
                room_info_list.CREATE_INFO[index].name = room.Name;
                room_info_list.CREATE_INFO[index].commited = 1;
                room_info_list.CREATE_INFO[index].comment = room.Commment;
                room_info_list.CREATE_INFO[index].category = room.SearchKey._category;
                room_info_list.CREATE_INFO[index].location_main = room.SearchKey._location_main;
                room_info_list.CREATE_INFO[index].location_sub = room.SearchKey._location_sub;
                room_info_list.CREATE_INFO[index].current_user = (byte)room.UserList.GetCount();
                room_info_list.CREATE_INFO[index].max_user = room.MaxUser;
                room_info_list.CREATE_INFO[index].duration = room.Duration;

                Console.WriteLine("ConfirmList index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

                index++;
            }
            foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.CreateList)
            {
                NLogic.Room room = pair.Value;

                room_info_list.CREATE_INFO[index] = new ROOM_INFO_LISTROOM();
                room_info_list.CREATE_INFO[index].index = room.Index;
                room_info_list.CREATE_INFO[index].name = room.Name;
                room_info_list.CREATE_INFO[index].commited = 0;
                room_info_list.CREATE_INFO[index].comment = room.Commment;
                room_info_list.CREATE_INFO[index].category = room.SearchKey._category;
                room_info_list.CREATE_INFO[index].location_main = room.SearchKey._location_main;
                room_info_list.CREATE_INFO[index].location_sub = room.SearchKey._location_sub;
                room_info_list.CREATE_INFO[index].current_user = (byte)room.UserList.GetCount();
                room_info_list.CREATE_INFO[index].max_user = room.MaxUser;
                room_info_list.CREATE_INFO[index].duration = room.Duration;

                Console.WriteLine("Create List index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

                index++;
            }


            return room_info_list;
        }

        public ROOM_SUMMARY_LIST AllRoomList(RoomSearchKey key, String user_no)
        {
            ROOM_SUMMARY_LIST room_summary_list = new ROOM_SUMMARY_LIST();

            NLogic.User user = _userList.FindUser(user_no);
            if (user == null)
            {
                return room_summary_list;
            }

            Console.WriteLine(String.Format("AllRoomList {0} : {1}", user.LoginId, user.UserGuid));

            NLogic.NRoom.List findRoomList = null;
            if (_roomTree.TryGetValue(key, out findRoomList) == false)
            {
                Console.WriteLine(String.Format("AllRoomList Not Found category {0} : {1} , {2}", key._category, key._location_main, key._location_sub));
                return room_summary_list;
            }

            int room_count = findRoomList.Count;
            room_summary_list.category = key._category;
            room_summary_list.location_main = key._location_main;
            room_summary_list.location_sub = key._location_sub;

            room_summary_list.ROOM_SUMMARY = new ROOM_SUMMARY_LISTROOM_SUMMARY[room_count];

            int index = 0;

            foreach (KeyValuePair<UInt32, NLogic.Room> pair in findRoomList)
            {
                NLogic.Room room = pair.Value;

                room_summary_list.ROOM_SUMMARY[index] = new ROOM_SUMMARY_LISTROOM_SUMMARY();
                room_summary_list.ROOM_SUMMARY[index].index = room.Index;
                room_summary_list.ROOM_SUMMARY[index].name = room.Name;
                room_summary_list.ROOM_SUMMARY[index].duration = room.Duration;
                room_summary_list.ROOM_SUMMARY[index].comment = room.Commment;
                room_summary_list.ROOM_SUMMARY[index].max_user = room.MaxUser;

                Console.WriteLine(" SUMMARY index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

                index++;
            }

            return room_summary_list;
        }

        public JOIN_ROOM_DETAIL JoinRoomDetail(UInt32 room_index, String user_no)
        {
            JOIN_ROOM_DETAIL join_room_detail = new JOIN_ROOM_DETAIL();

            NLogic.Room room = _roomList.Find(room_index);
            if (room == null)
            {
                return join_room_detail;
            }

            NLogic.User already_user = room.UserList.FindUser(user_no);

            join_room_detail.category = room.SearchKey._category;
            join_room_detail.index = room.Index;
            join_room_detail.name = room.Name;
            join_room_detail.comment = room.Commment;
            join_room_detail.location_main = room.SearchKey._location_main;
            join_room_detail.location_sub = room.SearchKey._location_sub;
			join_room_detail.current_user = (byte)room.UserList.GetCount();
			join_room_detail.max_user = room.MaxUser;
            join_room_detail.duration = room.Duration;

            Int32 current_member_count = room.UserList.GetCount();
            join_room_detail.MEMBER_LIST = new JOIN_ROOM_DETAILMEMBER_LIST();
            join_room_detail.MEMBER_LIST.count = (byte)current_member_count;

            if (already_user == null &&
                (current_member_count < (int)room.MaxUser))
            {
                join_room_detail.MEMBER_LIST.joinable = 1;
            }
            else
            {
                join_room_detail.MEMBER_LIST.joinable = 0;
            }

            join_room_detail.MEMBER_LIST.MEMBER = new JOIN_ROOM_DETAILMEMBER_LISTMEMBER[current_member_count];

            Int32 count = 0;
            foreach (KeyValuePair<String, NLogic.User> pair in room.UserList)
            {
                NLogic.User joined_user = pair.Value;

                join_room_detail.MEMBER_LIST.MEMBER[count] = new JOIN_ROOM_DETAILMEMBER_LISTMEMBER();
                DateTime birth = joined_user.GetBirth;
                DateTime current = DateTime.Today;

                if (current.Year < birth.Year)
                {
                    join_room_detail.MEMBER_LIST.MEMBER[count].age = 0;
                }
                else
                {
                    join_room_detail.MEMBER_LIST.MEMBER[count].age = (byte)(current.Year - birth.Year);
                }

                join_room_detail.MEMBER_LIST.MEMBER[count].gender = joined_user.Gender;
                join_room_detail.MEMBER_LIST.MEMBER[count].loginid = joined_user.LoginId;
                join_room_detail.MEMBER_LIST.MEMBER[count].user_name = joined_user.UserName;

                count++;
            }

            return join_room_detail;

        }

        public ROOM_RESULT JoinRoom(String user_no, UInt32 room_index)
        {
            ROOM_RESULT room_result = new ROOM_RESULT();
            room_result.crud = "JN";

            NLogic.User user = _userList.FindUser(user_no);
            if (user == null)
            {
                room_result.reason_sort = -1;	// not found user
                return room_result;
            }

            NLogic.Room room = _roomList.Find(room_index);
            if (room == null)
            {
                room_result.reason_sort = -2;	// not found room
                return room_result;
            }

            room_result.reason_sort = room.Join(user);
			room_result.room_index = room_index;
            return room_result;
        }

        public ROOM_RESULT LeaveRoom(String user_no, UInt32 room_index)
        {
            ROOM_RESULT room_result = new ROOM_RESULT();
            room_result.crud = "DR";

            NLogic.User user = _userList.FindUser(user_no);
            if (user == null)
            {
                room_result.reason_sort = -1;   // not found user
                return room_result;
            }

            NLogic.Room created_room = user.CreateList.Find(room_index);

            if (created_room != null)
            {
                NLogic.NRoom.List findRoomList = null;

                if (_roomTree.TryGetValue(created_room.SearchKey, out findRoomList) == false)
                {
                    Console.WriteLine(String.Format("LeaveRoom Not Found category {0} : {1} , {2}",
                        created_room.SearchKey._category,
                        created_room.SearchKey._location_main,
                        created_room.SearchKey._location_sub));

                    room_result.reason_sort = -2;
                    return room_result;
                }

                foreach (KeyValuePair<String, NLogic.User> pair in created_room.UserList)
                {
                    NLogic.User joined_user = pair.Value;
                    joined_user.JoinList.Remove(created_room);
                }

                user.CreateList.Remove(created_room);

                Console.WriteLine("[Leave on createRoom Success] index {0} name {1} Master : {2}", created_room.Index, created_room.Name, created_room.GetMaster().UserGuid);

                findRoomList.Remove(created_room);
                _roomList.Remove(created_room);

				room_result.room_index = created_room.Index;
				room_result.reason_sort = 0;

				created_room.Dispose();

                return room_result;
            }

            NLogic.Room joined_room = user.JoinList.Find(room_index);
            if (joined_room == null)
            {
                Console.WriteLine("[-3] index {0}", room_index);
                room_result.reason_sort = -3;
                return room_result;
            }

            Console.WriteLine("[Leave Success] index {0} name {1} Master : {2}", joined_room.Index, joined_room.Name, joined_room.GetMaster().UserGuid);

            bool result = joined_room.Leave(user);
            Trace.Assert(result == true);

			room_result.room_index = joined_room.Index;
			room_result.reason_sort = 0;

            return room_result;

        }

        public ROOM_RESULT CommitRoom(String user_no, UInt32 room_index)
        {
            ROOM_RESULT room_result = new ROOM_RESULT();
            room_result.crud = "CM";

            NLogic.User user = _userList.FindUser(user_no);
            if (user == null)
            {
                room_result.reason_sort = -1;   // not found user
                return room_result;
            }

            NLogic.Room created_room = user.CreateList.Find(room_index);
            if (created_room == null)
            {
                room_result.reason_sort = -2;   // not found Room
                return room_result;
            }

            NLogic.NRoom.List findRoomList = null;

            if (_roomTree.TryGetValue(created_room.SearchKey, out findRoomList) == false)
            {
                Console.WriteLine(String.Format("CommitRoom Delete... Not Found category {0} : {1} , {2}",
                    created_room.SearchKey._category,
                    created_room.SearchKey._location_main,
                    created_room.SearchKey._location_sub));

                room_result.reason_sort = -2;
                return room_result;
            }

            findRoomList.Remove(created_room);

            user.CreateList.Remove(created_room);
            user.ConfirmList.Insert(created_room);

            foreach (KeyValuePair<String, NLogic.User> pair in created_room.UserList)
            {
                NLogic.User joined_user = pair.Value;

                if (user.UserGuid.Equals(joined_user.UserGuid))
                {
                    Console.WriteLine("[Commit Room Skip master]  : {0}", user.UserGuid);

                    continue;
                }

                joined_user.JoinList.Remove(created_room);
                joined_user.JoinCommitedList.Insert(created_room);
            }

			room_result.room_index = created_room.Index;
            room_result.reason_sort = 0;

            return room_result;
        }

		public CHAT_LIST Chat(UInt32 room_index, String user_no, int local_index, int last_update, String content)
        {
            CHAT_LIST chat_list = new CHAT_LIST();
            chat_list.count = 0;
            chat_list.room_index = room_index;

            NLogic.Room room = _roomList.Find(room_index);
            if (room == null)
            {
                return chat_list;
            }

            NLogic.User user = room.UserList.FindUser(user_no);
            if (user == null)
            {
                return chat_list;
            }

            room.InsertMessage(content, user, last_update, ref chat_list);

            foreach (KeyValuePair<String, NLogic.User> pair in room.UserList)
            {
                NLogic.User joined_user = pair.Value;

                if (user.UserGuid.Equals(joined_user.UserGuid))
                {
                    Console.WriteLine("[ChatRoom Skip user]  : {0}", user.UserGuid);
                    continue;
                }

                if (joined_user.DeviceToken.Equals(""))
                {
                    Console.WriteLine("[ChatRoom Skip user Invalid DeviceToken ]  : {0}", user.UserGuid);
                    continue;
                }

				try
				{
					//Create a new notification to send
					JdSoft.Apple.Apns.Notifications.Notification
					alertNotification = new JdSoft.Apple.Apns.Notifications.Notification(joined_user.DeviceToken);

					alertNotification.Payload.Alert.Body = content;
					alertNotification.Payload.Sound = "default";
					alertNotification.Payload.Badge = chat_list.count;

					//Queue the notification to be sent
					if (_apnsProvider.Service.QueueNotification(alertNotification))
						Console.WriteLine("Notification Queued!");
					else
						Console.WriteLine("Notification Failed to be Queued!");
				}
				catch
				{
					continue;
				}
            }

			chat_list.local_index = local_index;

            return chat_list;

        }

        public CHAT_LIST ChatUpdate(UInt32 room_index, String user_no, int last_update)
        {
            CHAT_LIST chat_list = new CHAT_LIST();
            chat_list.count = 0;
            chat_list.room_index = room_index;

            NLogic.Room room = _roomList.Find(room_index);
            if (room == null)
            {
                return chat_list;
            }

            NLogic.User user = room.UserList.FindUser(user_no);
            if (user == null)
            {
                return chat_list;
            }

            room.UpdateMessage(user, last_update, ref chat_list);
			chat_list.local_index = -1;
            return chat_list;
        }

        public NOTICE_LIST CreateNotice(	UInt32 room_index, 
											String user_no,
											int group,
											String title,
											String content)
        {
            NOTICE_LIST notice_list = new NOTICE_LIST();
            notice_list.count = 0;
            notice_list.crud = "CR";
            notice_list.room_index = room_index;

            NLogic.Room room = _roomList.Find(room_index);
            if (room == null)
            {
                notice_list.result_code = -1;
                return notice_list;
            }

            NLogic.User user = room.UserList.FindUser(user_no);
            if (user == null)
            {
                notice_list.result_code = -2;
                return notice_list;
            }

			int result = room.AddNotice(group, title, content, user, ref notice_list);

            notice_list.result_code = result;

            if (result == 0)
            {
                foreach (KeyValuePair<String, NLogic.User> pair in room.UserList)
                {
                    NLogic.User joined_user = pair.Value;

                    if (user.UserGuid.Equals(joined_user.UserGuid))
                    {
                        Console.WriteLine("[Notice Skip user]  : {0}", user.UserGuid);
                        continue;
                    }

                    if (joined_user.DeviceToken.Equals(""))
                    {
                        Console.WriteLine("[Notice Skip user Invalid DeviceToken ]  : {0}", user.UserGuid);
                        continue;
                    }

					try
					{
						//Create a new notification to send
						JdSoft.Apple.Apns.Notifications.Notification
						alertNotification = new JdSoft.Apple.Apns.Notifications.Notification(joined_user.DeviceToken);

						alertNotification.Payload.Alert.Body = content;
						alertNotification.Payload.Sound = "default";
						alertNotification.Payload.Badge = notice_list.count;

						//Queue the notification to be sent
						if (_apnsProvider.Service.QueueNotification(alertNotification))
							Console.WriteLine("Notification Queued!");
						else
							Console.WriteLine("Notification Failed to be Queued!");
					}
					catch
					{
						continue;
					}
                }
            }

            return notice_list;

        }
        public NOTICE_LIST DeleteNotice(UInt32 room_index, String user_no,int group, int notice_index)
        {
            NOTICE_LIST notice_list = new NOTICE_LIST();
            notice_list.count = 0;
            notice_list.crud = "DR";
            notice_list.room_index = room_index;

            NLogic.Room room = _roomList.Find(room_index);
            if (room == null)
            {
                notice_list.result_code = -1;
                return notice_list;
            }

            NLogic.User user = room.UserList.FindUser(user_no);
            if (user == null)
            {
                notice_list.result_code = -2;
                return notice_list;
            }

			int result = room.DeleteNotice(group, notice_index, user);
            notice_list.result_code = result;

            return notice_list;
        }


        public NOTICE_LIST UpdateNotice(UInt32 room_index, String user_no, int group, int last_update)
        {
            NOTICE_LIST notice_list = new NOTICE_LIST();
            notice_list.count = 0;
            notice_list.crud = "UP";
            notice_list.room_index = room_index;
			notice_list.group = 0;

            NLogic.Room room = _roomList.Find(room_index);
            if (room == null)
            {
                notice_list.result_code = -1;
                return notice_list;
            }

            NLogic.User user = room.UserList.FindUser(user_no);
            if (user == null)
            {
                notice_list.result_code = -2;
                return notice_list;
            }

            room.UpdateNotice(user,group, last_update, ref notice_list);
            notice_list.result_code = 0;

            return notice_list;
        }


	}
}
