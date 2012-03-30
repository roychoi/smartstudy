using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Diagnostics;

using System.Data.Linq;
//using JdSoft.Apple.Apns.Notifications;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Net;

namespace RoomService
{
	//public class HostFactory : ServiceHostFactoryBase
	//{
	//    public override ServiceHostBase CreateServiceHost(
	//      string constructorString, Uri[] baseAddresses)
	//    {
	//        Type service = Type.GetType(constructorString);
	//        ServiceHost host = new ServiceHost(service, baseAddresses);
	//        // hook up event handlers
	//        //host.Opening += ServiceHost.OnOpening;
	//        //host.Closing += OnClosing;

	//        return host;
	//    }
	//}


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

	[ServiceContract(Namespace = "http://www.studyheyo.co.kr")]
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
		UPDATE_DEVICE_INFO UpdateUserDeviceDb(String user_guid, String deviceToken);

		[OperationContract]
		ROOM_RESULT CreateRoom(String user_no, RoomSearchKey key, String name, String comment, String duration, int maxuser);
		[OperationContract]
		ROOM_RESULT CreateRoomDb(String user_no, RoomSearchKey key, String name, String comment, String duration, int maxuser);

		[OperationContract]
		ROOM_INFO_LIST MyRoomList(String user_no);
		[OperationContract]
		ROOM_INFO_LIST MyRoomListDb(String user_no);

		[OperationContract]
		ROOM_SUMMARY_LIST AllRoomList(RoomSearchKey key, String user_no);
		[OperationContract]
		ROOM_SUMMARY_LIST AllRoomListDb(RoomSearchKey key, String user_no, int Skip);

		[OperationContract]
		JOIN_ROOM_DETAIL JoinRoomDetail(UInt32 room_index, String user_no);
		[OperationContract]
		JOIN_ROOM_DETAIL JoinRoomDetailDb(UInt32 room_index, String user_no);

		[OperationContract]
		ROOM_RESULT JoinRoom(String user_no, UInt32 room_index);
		[OperationContract]
		ROOM_RESULT JoinRoomDb(String user_no, UInt32 room_index);

		[OperationContract]
		ROOM_RESULT LeaveRoom(String user_no, UInt32 room_index);
		[OperationContract]
		ROOM_RESULT LeaveRoomDb(String user_no, UInt32 room_index);

		[OperationContract]
		ROOM_RESULT CommitRoom(String user_no, UInt32 room_index);
		[OperationContract]
		ROOM_RESULT CommitRoomDb(String user_no, UInt32 room_index);

		[OperationContract]
		ROOM_RESULT RecruitMember(String user_no , int room_index );

		[OperationContract]
		CHAT_LIST Chat(UInt32 room_index, String user_no, int local_index, int last_update, String content);
		[OperationContract]
		CHAT_LIST ChatDb(UInt32 room_index, String user_no, int local_index, int last_update, String content, byte type );

		[OperationContract]
		CHAT_LIST ChatUpdate(UInt32 room_index, String user_no, int last_update);
		[OperationContract]
		CHAT_LIST ChatUpdateDb(UInt32 room_index, String user_no, int last_update);

		[OperationContract]
		NOTICE_LIST CreateNotice(UInt32 room_index, String user_no, int group, String title, String content);
		[OperationContract]
		NOTICE_LIST CreateNoticeDb(UInt32 room_index, String user_no, int category, String title, String content);

		[OperationContract]
		NOTICE_LIST DeleteNotice(UInt32 room_index, String user_no, int group, int notice_index);
		[OperationContract]
		NOTICE_LIST DeleteNoticeDb(UInt32 room_index, String user_no, int category, int notice_index);

		[OperationContract]
		NOTICE_LIST UpdateNotice(UInt32 room_index, String user_no, int group, int last_update);
		[OperationContract]
		NOTICE_LIST UpdateNoticeDb(UInt32 room_index, String user_no, int category, int last_update);

		[OperationContract]
		ROOM_RESULT UpdatePenaltyInfo(UInt32 room_index, String user_no, int deposit, int absenceA, int absenceB, int lateness, int homework);
		[OperationContract]
		ROOM_PENALTY GetPenaltyInfo(int room_index, String user_no );

		[OperationContract]
		MEMBER_DETAIL_INFO CheckUserPenalty(Int32 room_index, String user_no, String member_id, int penalty);
		[OperationContract]
		MEMBER_DETAIL_INFO MemberDetailInfo(Int32 room_index, String user_no);

		[OperationContract]
		ROOM_RESULT EntrustMaster(int room_index, String user_no, String dest_member_id );
		[OperationContract]
		ROOM_MAIN_INFO GetRoomMainInfo(int room_index, String user_no, int last_chat_index );
	
		[OperationContract]
		void UpdateRoomInfo(int room_index, String user_no, RoomSearchKey key);
		[OperationContract]
		MEMBER_PROFILE_INFO MemberProfileInfo(int room_index);

		[OperationContract]
		ROOM_RESULT InviteUser(int room_index, String user_no, String dest_member_id);
		[OperationContract]
		ROOM_INFO_LIST InviteRoomList(String user_no);
		[OperationContract]
		ROOM_RESULT DeleteInvitedRoom(int room_index, String user_no);

		//[OperationContract]
		//void UpdateBadge(UInt32 room_index, String user_no, int last_update);


	}

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
					 ConcurrencyMode = ConcurrencyMode.Single)]

	public class RoomWCFService : IRoom, IDisposable
	{

		//private const string BaseAddress = "http://a4c818f43a6c4a56bf4f6a4adfd48f6e.cloudapp.net/push.svc";
		//private const string BaseAddress = "http://StudyheyoApns.cloudapp.net/push.svc";
		//private const string BaseAddressWP7Service = "http://a4c818f43a6c4a56bf4f6a4adfd48f6e.cloudapp.net/WP7Device.svc";

		//private const string BaseAddressIosService = "http://a4c818f43a6c4a56bf4f6a4adfd48f6e.cloudapp.net/IosDevice.svc";

		private const string BaseAddress = "http://07e69acb3cca45acaa2e527ea4c8b38b.cloudapp.net/push.svc";
		//private const string BaseAddress = "http://127.0.0.1:81/push.svc";

		static public int nValue = 10;
		static public NLogic.NRoom.List _roomList = new NLogic.NRoom.List();
		static public Dictionary<RoomSearchKey, NLogic.NRoom.List> _roomTree = new Dictionary<RoomSearchKey, NLogic.NRoom.List>();

		static public NLogic.NUser.List _userList = new NLogic.NUser.List();
		//public NApns.Provider _apnsProvider = null;
		public static TraceSource _source = new TraceSource("TraceSourceSTmate");

		public RoomWCFService()
		{
			//_apnsProvider = new NApns.Provider("iphone_dev.p12", "roy3513!", true);
		//    NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "WCFRoomService() called!!!!!!!!!!!!!!!!!");
		//    NApns.Provider._source.Flush();
		}

		public void Dispose()
		{
			//NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "Dispose()!!!!!!!!!!!!!!!!!!");
			//NApns.Provider._source.Flush();
		}

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

		public ROOM_RESULT Push(String deviceToken, String Message, int badge)
		{
			ROOM_RESULT res = new ROOM_RESULT();
			JdSoft.Apple.Apns.Notifications.Notification
			alertNotification = new JdSoft.Apple.Apns.Notifications.Notification(deviceToken);

			alertNotification.Payload.Alert.Body = Message;
			alertNotification.Payload.Sound = "default";
			alertNotification.Payload.Badge = badge;

			////Queue the notification to be sent
			//if (_apnsProvider.Service.QueueNotification(alertNotification))
			//{
			//    res.crud = "Notification Queued!";

			//    NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "Notification Queued! by TraceEvent");
			//    NApns.Provider._source.Flush();
			//}
			//else
			//{
			//    res.crud = "Notification Failed to be Queued!";
			//    Trace.WriteLine("Notification Failed to be Queued by Trace.WriteLine()");

			//    NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "Notification Failed to be Queued! by TraceEvent()");
			//    NApns.Provider._source.Flush();
			//}

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

			if (deviceToken.Length != JdSoft.Apple.Apns.Notifications.Notification.DEVICE_TOKEN_STRING_SIZE)
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

		public UPDATE_DEVICE_INFO UpdateUserDeviceDb(String user_guid, String deviceToken)
		{
			UPDATE_DEVICE_INFO update_device_info = new UPDATE_DEVICE_INFO();

			try
			{
				Guid UserId = new Guid(user_guid);
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				List<NDb.UserDeviceInfo> device_logined = (from Device in db.GetTable<NDb.UserDeviceInfo>()
														where (Device.DeviceToken == deviceToken)
														select Device).ToList();

				foreach (NDb.UserDeviceInfo userdevice in device_logined)
				{
					userdevice.DeviceToken = "";
					userdevice.Enable = false;
				}

				db.SubmitChanges();

				var match_info = (from Device in db.GetTable<NDb.UserDeviceInfo>()
								  where (Device.UserId == UserId)
								  select Device).SingleOrDefault();

				if (match_info != null)
				{
					match_info.DeviceToken = deviceToken;
					match_info.Enable = true;
					db.SubmitChanges();

					update_device_info.login_id = match_info.aspnet_User.UserName;

					return update_device_info;
				}

				NDb.UserDeviceInfo update_info = new NDb.UserDeviceInfo();
				update_info.DeviceToken = deviceToken;
				update_info.Type = 0;
				update_info.UserId = UserId;
				update_info.Enable = true;
				
				db.UserDeviceInfos.InsertOnSubmit(update_info);
				db.SubmitChanges();

				update_device_info.login_id = update_info.aspnet_User.UserName;
				update_device_info.result_code = 0;

				return update_device_info;
			}
			catch (Exception e)
			{
				Console.WriteLine("================== UpdateUserDeviceDb Error ==================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				update_device_info.result_code = -1;
				return update_device_info;
			}
		}
		public ROOM_RESULT CreateRoomDb(String user_no, RoomSearchKey key, String name, String comment, String duration, int maxuser)
		{
			ROOM_RESULT result = new ROOM_RESULT();
			result.crud = "CR";

			try
			{
				// TRANSACTION
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();
				NDb.CreateRoom room = new NDb.CreateRoom();

				room.Category = (byte)key._category;
				room.Location_Main = (byte)key._location_main;
				room.Location_Sub = (byte)key._location_sub;
				room.Comment = comment;
				room.Name = name;
				room.CreateDateTime = DateTime.Now;
				room.MaxUser = (byte)maxuser;
				room.UserId = new Guid(user_no);
				room.Duration = duration;

				db.CreateRooms.InsertOnSubmit(room);
				db.SubmitChanges();

				Console.WriteLine("CreateRoom InsertOnSubmit...RoomIndex {0}", room.RoomIndex);

				var joining_user = (from User in db.GetTable<NDb.aspnet_User>()
									where User.UserId == room.UserId
									select new NDb.NData.JoinedUser
									{
										UserId = User.UserId,
										LoginId = User.UserName,
										Gender = Byte.Parse(db.fn_GetProfileElement("Gender", User.aspnet_Profile.PropertyNames,
																							User.aspnet_Profile.PropertyValuesString)),
										NickName = db.fn_GetProfileElement("NickName", User.aspnet_Profile.PropertyNames,
																							User.aspnet_Profile.PropertyValuesString),
										//Birth = DateTime.Parse(db.fn_GetProfileElement("BirthYear", User.aspnet_Profile.PropertyNames,
										//                                                    User.aspnet_Profile.PropertyValuesString) )

									}).SingleOrDefault<NDb.NData.JoinedUser>();

				if (joining_user == null)
				{
					result.reason_sort = -2;
					return result;
				}

				Console.WriteLine("CreateRoom joining_user LoginId (UserName ) {0} NickName {1} ", joining_user.LoginId, joining_user.NickName );

				// LoginId NULL 제약 조건시 Transaction 확인하기..
				NDb.RoomJoinedUser create_user = new NDb.RoomJoinedUser();
				create_user.RoomIndex = room.RoomIndex;
				create_user.UserId = joining_user.UserId;
				create_user.JoinDateTime = DateTime.Now;
				create_user.LoginId = joining_user.LoginId;
				create_user.Gender = joining_user.Gender;
				create_user.NickName = joining_user.NickName;

				db.RoomJoinedUsers.InsertOnSubmit(create_user);
				db.RoomJoinedUsers.Context.SubmitChanges();

				Console.WriteLine("CreateRoom Success...{0}", room.RoomIndex);

			}
			catch (Exception e)
			{
				Console.WriteLine("CreateRoom findRoomList Insert failed...{0}", e.Message);

				result.reason_sort = -1;
				result.room_index = 0;
				return result;
			}

			result.reason_sort = 0;
			result.room_index = 1;

			return result;
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

			//room_info_list.JOIN_INFO = new ROOM_INFO_LISTROOM1[room_count];

			int index = 0;

			//foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.JoinCommitedList)
			//{
			//    NLogic.Room room = pair.Value;

			//    room_info_list.JOIN_INFO[index] = new ROOM_INFO_LISTROOM1();
			//    room_info_list.JOIN_INFO[index].index = room.Index;
			//    room_info_list.JOIN_INFO[index].name = room.Name;
			//    room_info_list.JOIN_INFO[index].commited = 1;
			//    room_info_list.JOIN_INFO[index].comment = room.Comment;
			//    room_info_list.JOIN_INFO[index].category = room.SearchKey._category;
			//    room_info_list.JOIN_INFO[index].location_main = room.SearchKey._location_main;
			//    room_info_list.JOIN_INFO[index].location_sub = room.SearchKey._location_sub;
			//    room_info_list.JOIN_INFO[index].current_user = (byte)room.UserList.GetCount();
			//    room_info_list.JOIN_INFO[index].max_user = room.MaxUser;
			//    room_info_list.JOIN_INFO[index].duration = room.Duration;

			//    Console.WriteLine("JoinCommitedList index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

			//    index++;
			//}

			//foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.JoinList)
			//{
			//    NLogic.Room room = pair.Value;

			//    room_info_list.JOIN_INFO[index] = new ROOM_INFO_LISTROOM1();
			//    room_info_list.JOIN_INFO[index].index = room.Index;
			//    room_info_list.JOIN_INFO[index].name = room.Name;
			//    room_info_list.JOIN_INFO[index].commited = 0;
			//    room_info_list.JOIN_INFO[index].comment = room.Comment;
			//    room_info_list.JOIN_INFO[index].category = room.SearchKey._category;
			//    room_info_list.JOIN_INFO[index].location_main = room.SearchKey._location_main;
			//    room_info_list.JOIN_INFO[index].location_sub = room.SearchKey._location_sub;
			//    room_info_list.JOIN_INFO[index].current_user = (byte)room.UserList.GetCount();
			//    room_info_list.JOIN_INFO[index].max_user = room.MaxUser;
			//    room_info_list.JOIN_INFO[index].duration = room.Duration;

			//    Console.WriteLine("JoinList index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

			//    index++;
			//}

			//room_count = user.CreateList.Count;
			//room_count += user.ConfirmList.Count;

			//room_info_list.CREATE_INFO = new ROOM_INFO_LISTROOM[room_count];

			//index = 0;

			//foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.ConfirmList)
			//{
			//    NLogic.Room room = pair.Value;

			//    room_info_list.CREATE_INFO[index] = new ROOM_INFO_LISTROOM();
			//    room_info_list.CREATE_INFO[index].index = room.Index;
			//    room_info_list.CREATE_INFO[index].name = room.Name;
			//    room_info_list.CREATE_INFO[index].commited = 1;
			//    room_info_list.CREATE_INFO[index].comment = room.Comment;
			//    room_info_list.CREATE_INFO[index].category = room.SearchKey._category;
			//    room_info_list.CREATE_INFO[index].location_main = room.SearchKey._location_main;
			//    room_info_list.CREATE_INFO[index].location_sub = room.SearchKey._location_sub;
			//    room_info_list.CREATE_INFO[index].current_user = (byte)room.UserList.GetCount();
			//    room_info_list.CREATE_INFO[index].max_user = room.MaxUser;
			//    room_info_list.CREATE_INFO[index].duration = room.Duration;

			//    Console.WriteLine("ConfirmList index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

			//    index++;
			//}
			//foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.CreateList)
			//{
			//    NLogic.Room room = pair.Value;

			//    room_info_list.CREATE_INFO[index] = new ROOM_INFO_LISTROOM();
			//    room_info_list.CREATE_INFO[index].index = room.Index;
			//    room_info_list.CREATE_INFO[index].name = room.Name;
			//    room_info_list.CREATE_INFO[index].commited = 0;
			//    room_info_list.CREATE_INFO[index].comment = room.Comment;
			//    room_info_list.CREATE_INFO[index].category = room.SearchKey._category;
			//    room_info_list.CREATE_INFO[index].location_main = room.SearchKey._location_main;
			//    room_info_list.CREATE_INFO[index].location_sub = room.SearchKey._location_sub;
			//    room_info_list.CREATE_INFO[index].current_user = (byte)room.UserList.GetCount();
			//    room_info_list.CREATE_INFO[index].max_user = room.MaxUser;
			//    room_info_list.CREATE_INFO[index].duration = room.Duration;

			//    Console.WriteLine("Create List index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

			//    index++;
			//}


			return room_info_list;
		}

		public ROOM_INFO_LIST MyRoomListDb(String user_no)
		{
			ROOM_INFO_LIST room_info_list = new ROOM_INFO_LIST();

			try
			{
				Guid UserId = new Guid(user_no);
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();


				int invited_count = (from InviteUser in db.GetTable<NDb.InvitedUser>()
									 where InviteUser.UserId == UserId
									 select InviteUser).Count();

				room_info_list.invited_count = invited_count;

				List<NDb.RoomJoinedUser> room_user = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
													  where RoomUser.UserId == UserId
													  select RoomUser).ToList();

				foreach (NDb.RoomJoinedUser RoomUser in room_user)
				{
					Console.WriteLine("RoomList count...and RoomIndex {0} UserCount {1} ", RoomUser.CreateRoom.RoomIndex,
						RoomUser.CreateRoom.RoomJoinedUsers.Count);
				}

				//List<NDb.NData.JoinedRoom> room_list = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
				//                                        join room in db.GetTable<NDb.CreateRoom>()
				//                                        on RoomUser.RoomIndex equals room.RoomIndex
				//                                        where (RoomUser.UserId == UserId)
				//                                        select new NDb.NData.JoinedRoom
				//                                        {
				//                                            Index = room.RoomIndex,
				//                                            Name = room.Name,
				//                                            Comment = room.Comment,
				//                                            Duration = room.Duration,
				//                                            MaxUser = room.MaxUser,
				//                                            Category = room.Category,
				//                                            LocationMain = room.Location_Main,
				//                                            LocationSub = room.Location_Sub,
				//                                            Commited = room.Commited,
				//                                            CreateDate = room.CreateDateTime,
				//                                            CurrentUser = (byte)(from RoomUserCount in db.GetTable<NDb.RoomJoinedUser>()
				//                                                                 where RoomUserCount.RoomIndex == room.RoomIndex
				//                                                                 select RoomUserCount).Count<NDb.RoomJoinedUser>(),
				//                                            MasterUserId = room.UserId,
				//                                            CommitedDate = room.CommitedDateTime

				//                                        }).ToList<NDb.NData.JoinedRoom>();

				//Console.WriteLine("MyRoomListDb count...{0} and Requester {1} ", room_list.Count, user_no );
				//NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "MyRoomListDb count...{0} and Requester {1} ", room_list.Count, user_no );

				//IEnumerable<NDb.NData.JoinedRoom> query_created = from create_room in room_list
				//                                                  where create_room.MasterUserId.Equals(UserId) 
				//                                                  select create_room;

				IEnumerable<NDb.CreateRoom> query_created = from JoinedUser in room_user
																	where JoinedUser.CreateRoom.UserId.Equals(UserId)
																	select JoinedUser.CreateRoom;
				
				//int create_count = query_created.Count<NDb.NData.JoinedRoom>();
				int create_count = query_created.Count();
				room_info_list.CREATE_INFO = new ROOM_INFO_LISTCREATE_INFO();
				room_info_list.CREATE_INFO.count = (byte)create_count;
				room_info_list.CREATE_INFO.ROOM = new ROOM_INFO_LISTCREATE_INFOROOM[create_count];
				int index = 0;

				//foreach (NDb.NData.JoinedRoom joinedRoom in query_created)
				foreach (NDb.CreateRoom joinedRoom in query_created)
				{
					Console.WriteLine("MyRoomListDb Create Room {0} Name {1} Date {2}", joinedRoom.RoomIndex, joinedRoom.Name, joinedRoom.CreateDateTime);
					//NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "MyRoomListDb Create Room {0} Name {1} Date {2} User :{3}", joinedRoom.RoomIndex, 
					//    joinedRoom.Name,
					//    joinedRoom.CreateDateTime,
					//    joinedRoom.UserId );

					room_info_list.CREATE_INFO.ROOM[index] = new ROOM_INFO_LISTCREATE_INFOROOM();
					room_info_list.CREATE_INFO.ROOM[index].index = joinedRoom.RoomIndex;
					room_info_list.CREATE_INFO.ROOM[index].name = joinedRoom.Name;
					room_info_list.CREATE_INFO.ROOM[index].commited = (byte)Convert.ChangeType(joinedRoom.Commited, TypeCode.Byte);
					room_info_list.CREATE_INFO.ROOM[index].cm_dateSpecified = false;
					if (joinedRoom.Commited == true)
					{
						room_info_list.CREATE_INFO.ROOM[index].cm_dateSpecified = true;
						room_info_list.CREATE_INFO.ROOM[index].cm_date = joinedRoom.CommitedDateTime.Value;
					}

					room_info_list.CREATE_INFO.ROOM[index].comment = joinedRoom.Comment;
					room_info_list.CREATE_INFO.ROOM[index].category = joinedRoom.Category;
					room_info_list.CREATE_INFO.ROOM[index].location_main = joinedRoom.Location_Main;
					room_info_list.CREATE_INFO.ROOM[index].location_sub = joinedRoom.Location_Sub;
					room_info_list.CREATE_INFO.ROOM[index].current_user = (byte)joinedRoom.RoomJoinedUsers.Count;
					room_info_list.CREATE_INFO.ROOM[index].max_user = joinedRoom.MaxUser;
					room_info_list.CREATE_INFO.ROOM[index].duration = joinedRoom.Duration;

					index++;
				}

				//IEnumerable<NDb.NData.JoinedRoom> query_joined = from join_room in room_list 
				//                                                 where !join_room.MasterUserId.Equals(UserId) 
				//                                                 select join_room;
				IEnumerable<NDb.CreateRoom> query_joined = from JoinedUser in room_user
															where !JoinedUser.CreateRoom.UserId.Equals(UserId)
															select JoinedUser.CreateRoom;
		
				//int join_count = query_joined.Count<NDb.NData.JoinedRoom>();
				int join_count = query_joined.Count();
				room_info_list.JOIN_INFO = new ROOM_INFO_LISTJOIN_INFO();
				room_info_list.JOIN_INFO.count = (byte)join_count;
				room_info_list.JOIN_INFO.ROOM = new ROOM_INFO_LISTJOIN_INFOROOM[join_count];
				
				index = 0;

				//foreach (NDb.NData.JoinedRoom joinedRoom in query_joined)
				foreach (NDb.CreateRoom joinedRoom in query_joined)
				{
					Console.WriteLine("MyRoomListDb Joined Room {0} Name {1} Date {2}", joinedRoom.RoomIndex, joinedRoom.Name, joinedRoom.CreateDateTime);
					//NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "MyRoomListDb Joined Room {0} Name {1} Date {2} User :{3}", joinedRoom.RoomIndex,
					//                    joinedRoom.Name,
					//                    joinedRoom.CreateDateTime,
					//                    joinedRoom.UserId);

					room_info_list.JOIN_INFO.ROOM[index] = new ROOM_INFO_LISTJOIN_INFOROOM();
					room_info_list.JOIN_INFO.ROOM[index].index = joinedRoom.RoomIndex;
					room_info_list.JOIN_INFO.ROOM[index].name = joinedRoom.Name;
					room_info_list.JOIN_INFO.ROOM[index].commited = (byte)Convert.ChangeType(joinedRoom.Commited, TypeCode.Byte);

					room_info_list.JOIN_INFO.ROOM[index].cm_dateSpecified = false;
					if (joinedRoom.Commited == true)
					{
						room_info_list.JOIN_INFO.ROOM[index].cm_dateSpecified = true;
						room_info_list.JOIN_INFO.ROOM[index].cm_date = joinedRoom.CommitedDateTime.Value;
					}

					room_info_list.JOIN_INFO.ROOM[index].comment = joinedRoom.Comment;
					room_info_list.JOIN_INFO.ROOM[index].category = joinedRoom.Category;
					room_info_list.JOIN_INFO.ROOM[index].location_main = joinedRoom.Location_Main;
					room_info_list.JOIN_INFO.ROOM[index].location_sub = joinedRoom.Location_Sub;
					// RoomJoinedUser 가 없을때 Null 인지 체크
					room_info_list.JOIN_INFO.ROOM[index].current_user = (byte)joinedRoom.RoomJoinedUsers.Count;
					room_info_list.JOIN_INFO.ROOM[index].max_user = joinedRoom.MaxUser;
					room_info_list.JOIN_INFO.ROOM[index].duration = joinedRoom.Duration;
					index++;
				}

				NApns.Provider._source.Flush();
			}
			catch (Exception e)
			{
				Console.WriteLine("CreateRoom findRoomList Insert failed...{0}", e.Message);
				return room_info_list;
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
				room_summary_list.ROOM_SUMMARY[index].comment = room.Comment;
				room_summary_list.ROOM_SUMMARY[index].current_user = (byte)room.UserList.GetCount();
				room_summary_list.ROOM_SUMMARY[index].max_user = room.MaxUser;

				Console.WriteLine(" SUMMARY index {0} name {1} Master : {2}", pair.Key, room.Name, room.GetMaster().UserGuid);

				index++;
			}

			return room_summary_list;
		}


		public ROOM_SUMMARY_LIST AllRoomListDb(RoomSearchKey key, String user_no, int Skip)
		{
			ROOM_SUMMARY_LIST room_summary_list = new ROOM_SUMMARY_LIST();

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				List<NDb.NData.JoinedRoom> room_list = (from room in db.GetTable<NDb.CreateRoom>()

														where (room.Category == key._category &&
																room.Location_Main == key._location_main &&
																room.Location_Sub == key._location_sub &&
																room.Commited == false)
														orderby room.CreateDateTime descending
														select new NDb.NData.JoinedRoom
														{
															Index = room.RoomIndex,
															Name = room.Name,
															Comment = room.Comment,
															Duration = room.Duration,
															MaxUser = room.MaxUser,
															Category = room.Category,
															LocationMain = room.Location_Main,
															LocationSub = room.Location_Sub,
															Commited = room.Commited,
															CreateDate = room.CreateDateTime,
															CurrentUser = (byte)(from RoomUserCount in db.GetTable<NDb.RoomJoinedUser>()
																				 where RoomUserCount.RoomIndex == room.RoomIndex
																				 select RoomUserCount).Count<NDb.RoomJoinedUser>(),
															MasterUserId = room.UserId

														}).Skip(Skip).Take(50).ToList<NDb.NData.JoinedRoom>();
				Console.WriteLine(" Skip Count {0} ", Skip);

				room_summary_list.category = key._category;
				room_summary_list.location_main = key._location_main;
				room_summary_list.location_sub = key._location_sub;

				int room_count = room_list.Count;
				room_summary_list.ROOM_SUMMARY = new ROOM_SUMMARY_LISTROOM_SUMMARY[room_count];

				int index = 0;

				foreach (NDb.NData.JoinedRoom joinedRoom in room_list)
				{
					room_summary_list.ROOM_SUMMARY[index] = new ROOM_SUMMARY_LISTROOM_SUMMARY();
					room_summary_list.ROOM_SUMMARY[index].index = (uint)joinedRoom.Index;
					room_summary_list.ROOM_SUMMARY[index].name = joinedRoom.Name;
					room_summary_list.ROOM_SUMMARY[index].duration = joinedRoom.Duration;
					room_summary_list.ROOM_SUMMARY[index].comment = joinedRoom.Comment;
					room_summary_list.ROOM_SUMMARY[index].current_user = joinedRoom.CurrentUser;
					room_summary_list.ROOM_SUMMARY[index].max_user = joinedRoom.MaxUser;

					Console.WriteLine(" SUMMARY index {0} name {1} CreateDate : {2}", joinedRoom.Index, joinedRoom.Name, joinedRoom.CreateDate);

					index++;
				}


			}
			catch
			{
				return room_summary_list;
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
			join_room_detail.comment = room.Comment;
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


		public JOIN_ROOM_DETAIL JoinRoomDetailDb(UInt32 room_index, String user_no)
		{
			JOIN_ROOM_DETAIL join_room_detail = new JOIN_ROOM_DETAIL();

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();
				var matched_room = (from room in db.GetTable<NDb.CreateRoom>()
									where (room.RoomIndex == room_index)
									select new NDb.NData.JoinedRoom
									{
										Index = room.RoomIndex,
										Name = room.Name,
										Comment = room.Comment,
										Duration = room.Duration,
										MaxUser = room.MaxUser,
										Category = room.Category,
										LocationMain = room.Location_Main,
										LocationSub = room.Location_Sub,
										Commited = room.Commited,
										CreateDate = room.CreateDateTime,
										CurrentUser = 0,
										MasterUserId = room.UserId
									}).SingleOrDefault();
				if (matched_room == null)
				{
					return join_room_detail;
				}

				// aspnet_User Join 대신에.. 그냥 이미지 링크를 프로파일에 저장하자... 수정요망
				//List<NDb.NData.JoinedUser> user_list = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
				//                                        join Profile in db.GetTable<NDb.aspnet_Profile>()
				//                                          on RoomUser.UserId equals Profile.UserId
				//                                        join User in db.GetTable<NDb.aspnet_User>() on Profile.UserId equals User.UserId
				//                                        where RoomUser.RoomIndex == matched_room.Index
				//                                        select new NDb.NData.JoinedUser
				//                                        {
				//                                            UserId = RoomUser.UserId,
				//                                            LoginId = User.UserName,
				//                                            Gender = Byte.Parse(db.fn_GetProfileElement("Gender", Profile.PropertyNames, Profile.PropertyValuesString)),
				//                                            NickName = db.fn_GetProfileElement("NickName", Profile.PropertyNames, Profile.PropertyValuesString),
				//                                            Birth = DateTime.Parse(db.fn_GetProfileElement("BirthYear", Profile.PropertyNames, Profile.PropertyValuesString))

				//                                        }).ToList<NDb.NData.JoinedUser>();


				List<NDb.NData.JoinedUser> user_list = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
														where RoomUser.RoomIndex == matched_room.Index
														select new NDb.NData.JoinedUser
														{
															UserId = RoomUser.UserId,
															//LoginId = RoomUser.aspnet_User.UserName,
															LoginId = RoomUser.LoginId,
															Gender = RoomUser.Gender,
															NickName = RoomUser.NickName,
															//Gender = Byte.Parse(db.fn_GetProfileElement("Gender", RoomUser.aspnet_User.aspnet_Profile.PropertyNames,
															//                                                    RoomUser.aspnet_User.aspnet_Profile.PropertyValuesString)),
															//NickName = db.fn_GetProfileElement("NickName", RoomUser.aspnet_User.aspnet_Profile.PropertyNames,
															//                                                    RoomUser.aspnet_User.aspnet_Profile.PropertyValuesString),
															Birth = DateTime.Parse(db.fn_GetProfileElement("BirthYear", RoomUser.aspnet_User.aspnet_Profile.PropertyNames,
																												RoomUser.aspnet_User.aspnet_Profile.PropertyValuesString))

														}).ToList<NDb.NData.JoinedUser>();

				Int32 current_member_count = user_list.Count;

				join_room_detail.category = matched_room.Category;
				join_room_detail.index = (uint)matched_room.Index;
				join_room_detail.name = matched_room.Name;
				join_room_detail.comment = matched_room.Comment;
				join_room_detail.location_main = matched_room.LocationMain;
				join_room_detail.location_sub = matched_room.LocationSub;
				join_room_detail.current_user = (byte)current_member_count;
				join_room_detail.max_user = matched_room.MaxUser;
				join_room_detail.duration = matched_room.Duration;

				join_room_detail.MEMBER_LIST = new JOIN_ROOM_DETAILMEMBER_LIST();
				join_room_detail.MEMBER_LIST.count = (byte)current_member_count;

				Guid existUserGuid = new Guid(user_no);
				// List 검색 수정요망
				int exist = (from exist_user in user_list where exist_user.UserId == existUserGuid select exist_user).Take(1).Count();

				if (current_member_count < matched_room.MaxUser &&
					exist == 0)
				{
					join_room_detail.MEMBER_LIST.joinable = 1;
				}
				else
				{
					join_room_detail.MEMBER_LIST.joinable = 0;
				}

				join_room_detail.MEMBER_LIST.MEMBER = new JOIN_ROOM_DETAILMEMBER_LISTMEMBER[current_member_count];

				Int32 count = 0;
				foreach (NDb.NData.JoinedUser joined_user in user_list)
				{
					join_room_detail.MEMBER_LIST.MEMBER[count] = new JOIN_ROOM_DETAILMEMBER_LISTMEMBER();
					DateTime birth = joined_user.Birth;
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
					join_room_detail.MEMBER_LIST.MEMBER[count].user_name = joined_user.NickName;

					Console.WriteLine("JoinedUser {0} {1} {2} ", joined_user.NickName, joined_user.LoginId, joined_user.Birth);

					count++;
				}

			}
			catch (Exception e)
			{
				Console.WriteLine("====================== JoinedUser Error ======================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				return join_room_detail;
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

		public ROOM_RESULT JoinRoomDb(String user_no, UInt32 room_index)
		{
			ROOM_RESULT room_result = new ROOM_RESULT();
			room_result.crud = "JN";

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();
				NDb.RoomJoinedUser create_user = new NDb.RoomJoinedUser();

				Guid UserId = new Guid(user_no);
				var joining_user = (from User in db.GetTable<NDb.aspnet_User>()
									where User.UserId == UserId
									//join Profile in db.GetTable<NDb.aspnet_Profile>() on UserId equals Profile.UserId
									select new NDb.NData.JoinedUser
									{
										UserId = User.UserId,
										LoginId = User.UserName,
										//Gender = Byte.Parse(db.fn_GetProfileElement("Gender", Profile.PropertyNames, Profile.PropertyValuesString)),
										//NickName = db.fn_GetProfileElement("NickName", Profile.PropertyNames, Profile.PropertyValuesString),
										//Birth = DateTime.Parse(db.fn_GetProfileElement("BirthYear", Profile.PropertyNames, Profile.PropertyValuesString))
										Gender = Byte.Parse( db.fn_GetProfileElement("Gender", User.aspnet_Profile.PropertyNames,
																							User.aspnet_Profile.PropertyValuesString) ),
										NickName = db.fn_GetProfileElement("NickName", User.aspnet_Profile.PropertyNames,
																							User.aspnet_Profile.PropertyValuesString),
										//Birth = DateTime.Parse(db.fn_GetProfileElement("BirthYear", User.aspnet_Profile.PropertyNames,
										//                                                    User.aspnet_Profile.PropertyValuesString) )

									}).SingleOrDefault<NDb.NData.JoinedUser>();

				if (joining_user == null)
				{
					room_result.reason_sort = -2;
					return room_result;
				}

				create_user.RoomIndex = (int)room_index;
				create_user.UserId = joining_user.UserId;
				create_user.JoinDateTime = DateTime.Now;
				create_user.LoginId = joining_user.LoginId;
				create_user.NickName = joining_user.NickName;
				create_user.Gender = joining_user.Gender;

				db.RoomJoinedUsers.InsertOnSubmit(create_user);
				db.SubmitChanges();

				room_result.reason_sort = 0;
				room_result.room_index = room_index;
			}
			catch (Exception e)
			{
				Console.WriteLine("======================== JoinRoom Error ==========================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("================================================================");


				room_result.reason_sort = -1;	// already joined.. maybe
				room_result.room_index = 0;

				return room_result;
			}

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
				Console.WriteLine("[-2] index {0}", room_index);
				room_result.reason_sort = -2;
				return room_result;
			}

			Console.WriteLine("[Leave Success] index {0} name {1} Master : {2}", joined_room.Index, joined_room.Name, joined_room.GetMaster().UserGuid);

			bool result = joined_room.Leave(user);
			Trace.Assert(result == true);

			room_result.room_index = joined_room.Index;
			room_result.reason_sort = 0;

			return room_result;

		}
		public ROOM_RESULT LeaveRoomDb(String user_no, UInt32 room_index)
		{
			ROOM_RESULT room_result = new ROOM_RESULT();
			room_result.crud = "DR";

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);
				var matched_user = (from JoinedUser in db.GetTable<NDb.RoomJoinedUser>()
									where (JoinedUser.UserId == UserId && JoinedUser.RoomIndex == room_index)
									select JoinedUser).SingleOrDefault();

				if (matched_user == null)
				{
					room_result.reason_sort = -1;   // not found user
					return room_result;
				}

				NDb.RoomJoinedUser create_user = new NDb.RoomJoinedUser();

				db.RoomJoinedUsers.DeleteOnSubmit(matched_user);
				db.SubmitChanges();

				room_result.reason_sort = 0;
				room_result.room_index = room_index;
			}
			catch (Exception e)
			{
				Console.WriteLine("LeaveRoomDb - error {0}", e.Message);

				room_result.reason_sort = -2;
				room_result.room_index = 0;

				return room_result;
			}

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

		public ROOM_RESULT CommitRoomDb(String user_no, UInt32 room_index)
		{
			ROOM_RESULT room_result = new ROOM_RESULT();
			room_result.crud = "CM";

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);
				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where (Master.UserId == UserId && Master.RoomIndex == room_index)
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					room_result.reason_sort = -1;   // not found user
					return room_result;
				}

				if (matched_room.Commited == true)
				{
					room_result.reason_sort = -2; // already commited
					return room_result;
				}

				matched_room.Commited = true;
				matched_room.CommitedDateTime = DateTime.Now;

				db.SubmitChanges();

				room_result.reason_sort = 0;
				room_result.room_index = room_index;
			}
			catch (Exception e)
			{
				Console.WriteLine("CommitRoomDb - error {0}", e.Message);

				room_result.reason_sort = -3;
				room_result.room_index = 0;

				return room_result;
			}

			return room_result;
		}


		public ROOM_RESULT RecruitMember(String user_no, int room_index)
		{
			ROOM_RESULT room_result = new ROOM_RESULT();
			room_result.crud = "RC";

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);
				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where Master.RoomIndex == room_index
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					room_result.reason_sort = -1;   // not found room
					return room_result;
				}

				if (matched_room.UserId.Equals(UserId) == false )
				{
					room_result.reason_sort = -2;	// not master user
					return room_result;
				}

				if (matched_room.Commited == false )
				{
					room_result.reason_sort = -3;	// already commited room
					return room_result;
				}

				matched_room.Commited = false;
				matched_room.CommitedDateTime = null;

				db.SubmitChanges();

				room_result.reason_sort = 0;
				room_result.room_index = (uint)room_index;
			}
			catch (Exception e)
			{
				Console.WriteLine("RecruitMember - error {0}", e.Message);

				room_result.reason_sort = -4;
				room_result.room_index = 0;

				return room_result;
			}

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

				//try
				//{
				//    //Create a new notification to send
				//    JdSoft.Apple.Apns.Notifications.Notification
				//    alertNotification = new JdSoft.Apple.Apns.Notifications.Notification(joined_user.DeviceToken);

				//    alertNotification.Payload.Alert.Body = content;
				//    alertNotification.Payload.Sound = "default";
				//    alertNotification.Payload.Badge = chat_list.count;

				//    //Queue the notification to be sent
				//    if (_apnsProvider.Service.QueueNotification(alertNotification))
				//        Console.WriteLine("Notification Queued!");
				//    else
				//        Console.WriteLine("Notification Failed to be Queued!");
				//}
				//catch
				//{
				//    continue;
				//}
			}

			chat_list.local_index = local_index;
			return chat_list;
		}

		private static void SendNotificationToAzure( string BaseAddress, PUSH_NOTIFICATION info )
		{
			MemoryStream xmlStream = new MemoryStream();
			XmlSerializer xmlPushNotification = new XmlSerializer(typeof(PUSH_NOTIFICATION));
			xmlPushNotification.Serialize(xmlStream, info);

			byte[] data = xmlStream.ToArray();

			// Prepare web request...
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseAddress + "/device/push");
			request.Credentials = new NetworkCredential("", "");
			request.PreAuthenticate = true;
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;
			Stream newStream = request.GetRequestStream();

			// Send the data.
			newStream.Write(data, 0, data.Length);
			try
			{
				IAsyncResult result =(IAsyncResult)request.BeginGetResponse(new AsyncCallback(NotificationCallback), request);
				
				_source.TraceEvent(TraceEventType.Critical, 3, "Start BeginGetResponse");
				_source.Flush();
			}
			catch (Exception est)
			{
				string error = "Exception: " + est.Message;
			}
		}

		private static void NotificationCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				HttpWebRequest myHttpWebRequest = (HttpWebRequest)asynchronousResult.AsyncState;
				
				using (HttpWebResponse response = myHttpWebRequest.EndGetResponse(asynchronousResult) as HttpWebResponse)
				{
					// Get the response stream  
					//StreamReader reader = new StreamReader(response.GetResponseStream());
					//string result = reader.ReadToEnd();

					_source.TraceEvent(TraceEventType.Critical, 3, "NotificationCallback Content Len {0} ", response.ContentLength);
					_source.Flush();


				}

				return;
			}
			catch (WebException e)
			{
				Console.WriteLine("\nRespCallback Exception raised!");
				Console.WriteLine("\nMessage:{0}", e.Message);
				Console.WriteLine("\nStatus:{0}", e.Status);
			}
		}

		public CHAT_LIST ChatDb(UInt32 room_index, String user_no, int local_index, int last_update, String content, byte type )
		{
			CHAT_LIST chat_list = new CHAT_LIST();
			chat_list.count = 0;
			chat_list.room_index = room_index;

			try
			{
				Guid UserId = new Guid(user_no);
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				//var message = (from JoinedUser in db.GetTable<NDb.RoomJoinedUser>()
				//               where (JoinedUser.RoomIndex == room_index && JoinedUser.UserId == UserId)
				//               //join Profile in db.GetTable<NDb.aspnet_Profile>() on UserId equals Profile.UserId
				//               select new NDb.NData.ChatMessage
				//               {
				//                   MsgId = (from Message in db.GetTable<NDb.Message>()
				//                            where Message.RoomIndex == room_index
				//                            select Message.MsgId).Count(),

				//                   Contents = content,
				//                   //NickName = db.fn_GetProfileElement("NickName", JoinedUser.aspnet_User.aspnet_Profile.PropertyNames,
				//                   //											    JoinedUser.aspnet_User.aspnet_Profile.PropertyValuesString),
				//                   UserId = JoinedUser.UserId.ToString(),
				//                   NickName = JoinedUser.NickName,	// NickName 만 JoinedUser 에 저장할까나?... 아니면 위에 같이 조인시킬까?
				//                   Email = JoinedUser.LoginId,
				//                   //Email = JoinedUser.aspnet_User.UserName,	// squence 가 하나 여야 성공한다.

				//               }).SingleOrDefault<NDb.NData.ChatMessage>();

				//if (message == null)
				//{
				//    Console.WriteLine("Invalid ChatMsg...");
				//    chat_list.local_index = local_index;
				//    return chat_list;
				//}

				var matched_user = (from JoinedUser in db.GetTable<NDb.RoomJoinedUser>()
									where (JoinedUser.RoomIndex == room_index && JoinedUser.UserId == UserId)
									select JoinedUser).SingleOrDefault();

				if (matched_user == null)
				{
					Console.WriteLine("Invalid User...");
					chat_list.local_index = local_index;
					return chat_list;
				}

				//int lastMessage = (from Message in db.GetTable<NDb.Message>()
				//                   where Message.RoomIndex == room_index
				//                   select Message.MsgId).Count();

				NDb.Message insert_message = new NDb.Message();

				//insert_message.MsgId = ++ message.MsgId;
				//insert_message.IptTime = DateTime.Now;
				//insert_message.Contents = message.Contents;
				//insert_message.RoomIndex = (int)room_index;
				//insert_message.NickName = message.NickName;
				//insert_message.Email = message.Email;

				insert_message.IptTime = DateTime.Now;
				insert_message.Contents = content;
				insert_message.RoomIndex = (int)room_index;
				insert_message.NickName = matched_user.NickName;
				insert_message.Email = matched_user.LoginId;
				insert_message.Type = type;

				db.Messages.InsertOnSubmit(insert_message);
				db.SubmitChanges();

				IEnumerable<NDb.Message> query_message = (from Message in db.GetTable<NDb.Message>()
														  where Message.RoomIndex == room_index && Message.MsgId > last_update
														  orderby Message.MsgId ascending
														  select Message).Take(50);

				int return_count = query_message.Count();
				chat_list.count = return_count;
				chat_list.room_index = room_index;
				chat_list.CHAT = new CHAT_LISTCHAT[return_count];

				int nIndex = 0;
				foreach (NDb.Message msg in query_message)
				{
					chat_list.CHAT[nIndex] = new CHAT_LISTCHAT();

					chat_list.CHAT[nIndex].chat_index = msg.MsgId;
					chat_list.CHAT[nIndex].nick_name = msg.NickName;
					chat_list.CHAT[nIndex].Value = msg.Contents;
					chat_list.CHAT[nIndex].login_id = msg.Email;
					chat_list.CHAT[nIndex].ownerSpecified = false;
					chat_list.CHAT[nIndex].date_time = msg.IptTime;
					if( msg.Type == 0 )
					{
						chat_list.CHAT[nIndex].typeSpecified = false;
					}
					else{
						chat_list.CHAT[nIndex].typeSpecified = true;
						chat_list.CHAT[nIndex].type = msg.Type;
					}

					nIndex++;
				}

				chat_list.local_index = local_index;

				// Backend push server 로 옮겨야 ..
				//List<String> device_info_list = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
				//                                 where RoomUser.RoomIndex == room_index
				//                                join DeviceInfo in db.GetTable<NDb.UserDeviceInfo>() 
				//                                                on RoomUser.UserId equals DeviceInfo.UserId
				//                                 select DeviceInfo.DeviceToken ).ToList<String>();

				List<NDb.UserDeviceInfo> device_info_list = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
															 where RoomUser.RoomIndex == room_index && RoomUser.UserId != UserId
												 join DeviceInfo in db.GetTable<NDb.UserDeviceInfo>()
													 on RoomUser.UserId equals DeviceInfo.UserId
															 where DeviceInfo.Enable == true
												 select DeviceInfo).ToList<NDb.UserDeviceInfo>();

				Console.WriteLine("Push Notification to room {0} ", room_index );

				PUSH_NOTIFICATION pushInfo = new PUSH_NOTIFICATION();
				pushInfo.room = (int)room_index;
				pushInfo.msg = content;
				pushInfo.tp = 1; //chat
				pushInfo.r_name = matched_user.CreateRoom.Name;
				pushInfo.INFO = new PUSH_NOTIFICATIONINFO[device_info_list.Count];

				int nPushCount = 0;
				foreach (NDb.UserDeviceInfo device_info in device_info_list)
				{
					//if (device_info.UserId.Equals(UserId) == true)
					//{
					//    pushInfo.INFO[nPushCount] = new PUSH_NOTIFICATIONINFO();
					//    pushInfo.INFO[nPushCount].badge = 1;
					//    pushInfo.INFO[nPushCount].DeviceId = "";
					//    pushInfo.INFO[nPushCount].type = "";
					//    pushInfo.INFO[nPushCount].sound = "";
					//    nPushCount++;

					//    continue;
					//}

					pushInfo.INFO[nPushCount] = new PUSH_NOTIFICATIONINFO();


					if (matched_user.CreateRoom.UserId.Equals(device_info.UserId))
					{
						pushInfo.INFO[nPushCount].owner = 1;
					}
					else
					{
						pushInfo.INFO[nPushCount].owner = 0;
					}

					pushInfo.INFO[nPushCount].badge = 1;
					pushInfo.INFO[nPushCount].DeviceId = device_info.DeviceToken;
					pushInfo.INFO[nPushCount].type = "iOS";
					pushInfo.INFO[nPushCount].sound = "default";
					nPushCount++;
				}

				RoomWCFService.SendNotificationToAzure(BaseAddress, pushInfo);
				

				//foreach (NDb.UserDeviceInfo device_info in device_info_list)
				//{
				//    Console.WriteLine("DeviceToken {0} ", device_info );

				//    if (device_info.UserId.Equals(UserId))
				//    {
				//        Console.WriteLine("[ChatRoom Skip user]  : {0}", device_info.UserId);
				//        continue;
				//    }

				//    if (device_info.DeviceToken.Equals(""))
				//    {
				//        Console.WriteLine("[ChatRoom Skip user Invalid DeviceToken ]  : {0}", device_info.UserId);
				//        continue;
				//    }

				//    try
				//    {
				//        //Create a new notification to send
				//        JdSoft.Apple.Apns.Notifications.Notification
				//        alertNotification = new JdSoft.Apple.Apns.Notifications.Notification(device_info.DeviceToken);

				//        alertNotification.Payload.Alert.Body = content;
				//        alertNotification.Payload.Sound = "default";
				//        alertNotification.Payload.Badge = 1;

				//        //Queue the notification to be sent
				//        if (_apnsProvider.Service.QueueNotification(alertNotification))
				//            Console.WriteLine("Notification Queued!");
				//        else
				//            Console.WriteLine("Notification Failed to be Queued!");
				//    }
				//    catch
				//    {
				//        continue;
				//    }
				//}

				return chat_list;
			}
			catch (Exception e)
			{
				Console.WriteLine("======================== Chat Error ==========================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				chat_list.local_index = local_index;
				return chat_list;
			}
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

		public CHAT_LIST ChatUpdateDb(UInt32 room_index, String user_no, int last_update)
		{
			CHAT_LIST chat_list = new CHAT_LIST();
			chat_list.count = 0;
			chat_list.room_index = room_index;

			try
			{
				Guid UserId = new Guid(user_no);
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();
				DateTime last_date = DateTime.Now;
				IEnumerable<NDb.Message> query_message = (from Message in db.GetTable<NDb.Message>()
														  where Message.RoomIndex == room_index && Message.MsgId > last_update
														  orderby Message.MsgId ascending
														  select Message).Take(50);

				int return_count = query_message.Count();
				chat_list.count = return_count;
				chat_list.room_index = room_index;
				chat_list.CHAT = new CHAT_LISTCHAT[return_count];

				int nIndex = 0;
				foreach (NDb.Message msg in query_message)
				{
					chat_list.CHAT[nIndex] = new CHAT_LISTCHAT();
					chat_list.CHAT[nIndex].chat_index = msg.MsgId;
					chat_list.CHAT[nIndex].nick_name = msg.NickName;
					chat_list.CHAT[nIndex].Value = msg.Contents;
					chat_list.CHAT[nIndex].login_id = msg.Email;
					chat_list.CHAT[nIndex].ownerSpecified = false;
					chat_list.CHAT[nIndex].date_time = msg.IptTime;
					if (msg.Type == 0)
					{
						chat_list.CHAT[nIndex].typeSpecified = false;
					}
					else
					{
						chat_list.CHAT[nIndex].typeSpecified = true;
						chat_list.CHAT[nIndex].type = msg.Type;
					}
					nIndex++;
				}

				chat_list.local_index = -1;
				return chat_list;

			}
			catch (Exception e)
			{
				Console.WriteLine("======================== Chat Error ==========================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				return chat_list;
			}
		}

		public NOTICE_LIST CreateNotice(UInt32 room_index,
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

					//try
					//{
					//    //Create a new notification to send
					//    JdSoft.Apple.Apns.Notifications.Notification
					//    alertNotification = new JdSoft.Apple.Apns.Notifications.Notification(joined_user.DeviceToken);

					//    alertNotification.Payload.Alert.Body = content;
					//    alertNotification.Payload.Sound = "default";
					//    alertNotification.Payload.Badge = notice_list.count;

					//    //Queue the notification to be sent
					//    if (_apnsProvider.Service.QueueNotification(alertNotification))
					//        Console.WriteLine("Notification Queued!");
					//    else
					//        Console.WriteLine("Notification Failed to be Queued!");
					//}
					//catch
					//{
					//    continue;
					//}
				}
			}

			return notice_list;

		}

		public NOTICE_LIST CreateNoticeDb(uint room_index,
											String user_no,
											int category,
											String title,
											String content)
		{
			NOTICE_LIST notice_list = new NOTICE_LIST();
			notice_list.count = 0;
			notice_list.crud = "CR";
			notice_list.room_index = (uint)room_index;

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();
				Guid UserId = new Guid(user_no);
				
				var created_room = (from CreateRoom in db.GetTable<NDb.CreateRoom>()
									where (CreateRoom.RoomIndex == room_index && CreateRoom.UserId == UserId)
									select CreateRoom).SingleOrDefault<NDb.CreateRoom>();

				if (created_room == null)
				{
					Console.WriteLine("Create Notice - not master user...");
					notice_list.result_code = -1;
					return notice_list;
				}

				NDb.Notice insert_notice = new NDb.Notice ();

				insert_notice.RoomIndex = (int)room_index;
				insert_notice.Category = (byte)category;
				insert_notice.Title = title;
				insert_notice.Contents = content;
				insert_notice.IptTime = DateTime.Now;

				db.Notices.InsertOnSubmit(insert_notice);
				db.SubmitChanges();

				notice_list.result_code = 0;

				return notice_list;

			}
			catch( Exception e )
			{
				Console.WriteLine("======================== Chat Error ==========================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");
		
				notice_list.result_code = -2;
				return notice_list;
			}
		}

		public NOTICE_LIST DeleteNotice(UInt32 room_index, String user_no, int group, int notice_index)
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

		public NOTICE_LIST DeleteNoticeDb(UInt32 room_index, String user_no, int category, int notice_index)
		{
			NOTICE_LIST notice_list = new NOTICE_LIST();
			notice_list.count = 0;
			notice_list.crud = "DR";
			notice_list.room_index = room_index;
			notice_list.group = 0;
			
			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();
				Guid UserId = new Guid(user_no);


				var created_room = (from CreateRoom in db.GetTable<NDb.CreateRoom>()
									where (CreateRoom.RoomIndex == room_index && CreateRoom.UserId == UserId)
									select CreateRoom).SingleOrDefault<NDb.CreateRoom>();

				if (created_room == null)
				{
					Console.WriteLine("Create Notice - not master user...");
					notice_list.result_code = -1;
					return notice_list;
				}

				var matched_notice = (from Notice in db.GetTable<NDb.Notice>()
									where (Notice.RoomIndex == room_index &&
											Notice.NoticeId == notice_index )
									select Notice).SingleOrDefault();

				if (matched_notice == null)
				{
					notice_list.result_code = -2;   // not found Notice
					return notice_list;
				}

				db.Notices.DeleteOnSubmit(matched_notice);
				db.SubmitChanges();

				notice_list.group = category;
				notice_list.result_code = notice_index;

				return notice_list;
			}
			catch (Exception e)
			{
				Console.WriteLine("DeleteNoticeDb - error {0}", e.Message);

				notice_list.result_code = -3;

				return notice_list;
			}
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

			room.UpdateNotice(user, group, last_update, ref notice_list);
			notice_list.result_code = 0;

			return notice_list;
		}

		public NOTICE_LIST UpdateNoticeDb(UInt32 room_index, String user_no, int category, int last_update)
		{
			NOTICE_LIST notice_list = new NOTICE_LIST();
			notice_list.crud = "UP";
			notice_list.room_index = room_index;
			notice_list.group = category;

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();
				Guid UserId = new Guid(user_no);

				IEnumerable<NDb.Notice> query_message = (from Notice in db.GetTable<NDb.Notice>()
														  where Notice.RoomIndex == room_index && Notice.Category == category
														  orderby Notice.NoticeId ascending
														  select Notice);

				int count = query_message.Count();
				notice_list.count = count;
				notice_list.NOTICE = new NOTICE_LISTNOTICE[count];

				int nIndex = 0;
				foreach (NDb.Notice notice in query_message)
				{
					notice_list.NOTICE[nIndex] = new NOTICE_LISTNOTICE();
					notice_list.NOTICE[nIndex].index = notice.NoticeId;
					notice_list.NOTICE[nIndex].title = notice.Title;
					notice_list.NOTICE[nIndex].Value = notice.Contents;
					notice_list.NOTICE[nIndex].date_time = notice.IptTime;

					nIndex++;
				}
				notice_list.result_code = 0;

				return notice_list;
			}
			catch (Exception e)
			{
				Console.WriteLine("UpdateNoticeDb - error {0}", e.Message);

				notice_list.result_code = -3;

				return notice_list;
			}
		}

		public void UpdateRoomInfo(int room_index, String user_no, RoomSearchKey key)
		{
			Console.WriteLine("Update Room Info {0}", key._category);
		}

		public ROOM_RESULT UpdatePenaltyInfo(UInt32 room_index, String user_no, int deposit, int absenceA, int absenceB, int lateness, int homework)
		{
			ROOM_RESULT room_result = new ROOM_RESULT();
			room_result.crud = "UP";
			room_result.room_index = room_index;

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);
				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where (Master.UserId == UserId && Master.RoomIndex == room_index)
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					room_result.reason_sort = -1;   // not found user
					return room_result;
				}

				if (matched_room.Commited == false )
				{
					room_result.reason_sort = -2; // not commited room
					return room_result;
				}

				matched_room.Deposit = deposit;
				matched_room.AbsenceA = absenceA;
				matched_room.AbsenceB = absenceB;
				matched_room.Lateness = lateness;
				matched_room.Homework = homework;

				db.SubmitChanges();

				room_result.reason_sort = 0;
				room_result.room_index = room_index;

				return room_result;
			}
			catch (Exception e)
			{
				Console.WriteLine("================== UpdatePenalty Error ==================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				room_result.reason_sort = -3;
				room_result.room_index = 0;

				return room_result;
			}
		}

		public ROOM_PENALTY GetPenaltyInfo(int room_index, String user_no)
		{
			ROOM_PENALTY room_penalty = new ROOM_PENALTY();
			room_penalty.room_index = room_index;

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);

				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where Master.RoomIndex == room_index
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					room_penalty.reason_sort = -1;   // not found user
					return room_penalty;
				}

				if (matched_room.Commited == false)
				{
					room_penalty.reason_sort = -2; // not commited room
					return room_penalty;
				}

				room_penalty.deposit = matched_room.Deposit;
				room_penalty.absenceA = matched_room.AbsenceA;
				room_penalty.absenceB = matched_room.AbsenceB;
				room_penalty.lateness = matched_room.Lateness;
				room_penalty.homework = matched_room.Homework;

				db.SubmitChanges();

				room_penalty.reason_sort = 0;
				room_penalty.room_index = room_index;

				return room_penalty;
			}
			catch (Exception e)
			{
				Console.WriteLine("================== GetPenaltyInfo Error ==================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				room_penalty.reason_sort = -3;
				room_penalty.room_index = 0;

				return room_penalty;
			}
		}


		public MEMBER_DETAIL_INFO CheckUserPenalty(Int32 room_index, String user_no, String member_LoginId, int penalty)
		{
			MEMBER_DETAIL_INFO result = new MEMBER_DETAIL_INFO();
			result.room_index = room_index;
			result.reason_sort = 0;
			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);
				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where (Master.UserId == UserId && Master.RoomIndex == room_index)
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					result.reason_sort = -1;   // not master user
					return result;
				}

				if (matched_room.Commited == false)
				{
					result.reason_sort = -2;	// not commited room
					return result;
				}

				//List<NDb.RoomJoinedUser> user_list = (	from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
				//                                        where RoomUser.RoomIndex == matched_room.RoomIndex && RoomUser.LoginId == member_LoginId
				//                                        select RoomUser).ToList<NDb.RoomJoinedUser>();

				NDb.RoomJoinedUser mached_user = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
												 where RoomUser.RoomIndex == matched_room.RoomIndex && 
													RoomUser.LoginId == member_LoginId
												 select RoomUser ).SingleOrDefault();
				if (mached_user == null)
				{
					result.reason_sort = -3;   // not found user
					return result;
				}

				int total_penalty = 0;
				if ((penalty & 0x01) > 0)
				{
					total_penalty += matched_room.AbsenceA;
				}
				if ((penalty & 0x02) > 0)
				{
					total_penalty += matched_room.AbsenceB;
				}
				if ((penalty & 0x04) > 0)
				{
					total_penalty += matched_room.Lateness;
				}
				if ((penalty & 0x08) > 0)
				{
					total_penalty += matched_room.Homework;
				}

				mached_user.Penalty += total_penalty;
				db.SubmitChanges();

				/////???????????
				DateTime Birth = DateTime.Parse(db.fn_GetProfileElement("BirthYear", mached_user.aspnet_User.aspnet_Profile.PropertyNames,
																					mached_user.aspnet_User.aspnet_Profile.PropertyValuesString));

				result.reason_sort = 0;
				result.cm_date = matched_room.CommitedDateTime.Value;

				result.penalty_total = matched_room.RoomJoinedUsers.Sum<NDb.RoomJoinedUser>( UserPenalty => UserPenalty.Penalty );
				// 전체 Deposit 를 저장할지 결정...
				result.deposit_total = matched_room.RoomJoinedUsers.Count * matched_room.Deposit;
				result.count = 1;

				result.MEMBER = new MEMBER_DETAIL_INFOMEMBER[1];
				result.MEMBER[0] = new MEMBER_DETAIL_INFOMEMBER();
				result.MEMBER[0].loginid = mached_user.LoginId;
				result.MEMBER[0].user_name = mached_user.NickName;
				result.MEMBER[0].gender = mached_user.Gender;
				result.MEMBER[0].panalty = mached_user.Penalty;
				result.MEMBER[0].rank_no = 0;
				result.MEMBER[0].age = (byte)(DateTime.Now.Year - Birth.Year);

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine("================== CheckUserPenalty Error ==================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				result.reason_sort = -4;
				result.room_index = 0;

				return result;
			}
		}

		public MEMBER_DETAIL_INFO MemberDetailInfo(Int32 room_index, String user_no )
		{
			MEMBER_DETAIL_INFO result = new MEMBER_DETAIL_INFO();
			result.room_index = room_index;
			result.reason_sort = 0;
			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);
			
				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where  Master.RoomIndex == room_index
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					result.reason_sort = -1;   // not found user
					return result;
				}

				if (matched_room.Commited == false)
				{
					result.reason_sort = -2;   // not commited room
					return result;
				}

				// JoinedRoomUser 가 0 일때 체크 하기
				List<NDb.RoomJoinedUser> user_list = matched_room.RoomJoinedUsers.ToList<NDb.RoomJoinedUser>();
				//List<NDb.RoomJoinedUser> user_list = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
				//                                      where RoomUser.RoomIndex == room_index
				//                                      select RoomUser).ToList<NDb.RoomJoinedUser>();


				result.reason_sort = 0;
				result.cm_date = matched_room.CommitedDateTime.Value;
				result.count = (byte)user_list.Count;
				result.penalty_total = user_list.Sum<NDb.RoomJoinedUser>(UserPenalty => UserPenalty.Penalty);

				//// 전체 Deposit 를 저장할지 결정...
				result.deposit_total = user_list.Count * matched_room.Deposit;

				result.MEMBER = new MEMBER_DETAIL_INFOMEMBER[user_list.Count];

				int index = 0;
				foreach (NDb.RoomJoinedUser JoinedUser in user_list)
				{
					result.MEMBER[index] = new MEMBER_DETAIL_INFOMEMBER();
					result.MEMBER[index].loginid = JoinedUser.LoginId;
					result.MEMBER[index].user_name = JoinedUser.NickName;
					result.MEMBER[index].gender = JoinedUser.Gender;
					result.MEMBER[index].panalty = JoinedUser.Penalty;
					result.MEMBER[index].rank_no = 0;
					DateTime Birth = DateTime.Parse(db.fn_GetProfileElement("BirthYear", JoinedUser.aspnet_User.aspnet_Profile.PropertyNames,
																				JoinedUser.aspnet_User.aspnet_Profile.PropertyValuesString));

					result.MEMBER[index].age = (byte)(DateTime.Now.Year - Birth.Year);
					index++;
				}

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine("================== MemberDetailInfo Error ====================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				result.reason_sort = -4;
				result.room_index = 0;

				return result;
			}
		}

		public ROOM_RESULT EntrustMaster(int room_index, String user_no, String dest_member_id)
		{
			ROOM_RESULT result = new ROOM_RESULT();
			result.crud = "ET";

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);
				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where Master.RoomIndex == room_index
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					result.reason_sort = -1;   // not found room
					return result;
				}

				if (matched_room.UserId.Equals(UserId) == false)
				{
					result.reason_sort = -2;   // not master user
					return result;
				}

				if (matched_room.Commited == false)
				{
					result.reason_sort = -3;	// not commited room
					return result;
				}

				NDb.RoomJoinedUser mached_user = (from RoomUser in matched_room.RoomJoinedUsers
												  where RoomUser.LoginId == dest_member_id
												  select RoomUser).SingleOrDefault();
				
				if (mached_user == null)
				{
					result.reason_sort = -4;   // not found dest user
					return result;
				}
				
				if (mached_user.UserId.Equals(UserId) == true)
				{
					result.reason_sort = -5;
					return result;				// can't enturst to same user
				}

				matched_room.UserId = mached_user.UserId;
				db.SubmitChanges();

				result.reason_sort = 0;

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine("================== CheckUserPenalty Error ==================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				result.reason_sort = -6;
				result.room_index = 0;

				return result;
			}
		}

		public ROOM_MAIN_INFO GetRoomMainInfo(int room_index, String user_no, int last_chat_index )
		{
			ROOM_MAIN_INFO result = new ROOM_MAIN_INFO();
			result.reason_sort = 0;
			result.room_index = room_index;

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);

				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where Master.RoomIndex == room_index
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					result.reason_sort = -1;   // not found user
					return result;
				}

				if (matched_room.Commited == false)
				{
					result.reason_sort = -2;
					return result;
				}

				result.cm_date = matched_room.CommitedDateTime.Value;

				TimeSpan checkOneDaySpan = new TimeSpan( 2,0,0,0 );
				DateTime checkOneDayTime = DateTime.Now - checkOneDaySpan;

				List<NDb.Notice> notice_list = matched_room.Notices.ToList<NDb.Notice>();
				foreach( NDb.Notice notice in notice_list )
				{
					if (notice.IptTime > checkOneDayTime)
					{
						Console.WriteLine(String.Format("New Notice : {0} - {1}", notice.NoticeId, notice.Contents));

						switch (notice.Category)
						{
							case 1:
								result.notice_a_cnt++;
								break;
							case 2:
								result.notice_b_cnt++;
								break;
							case 3:
								result.notice_c_cnt++;
								break;
						}
					}
				}

				if (matched_room.Messages.Count() == 0)
				{
					result.chat_last_index = 0;
					result.chat_unread_count = 0;
				}
				else
				{
					result.chat_last_index = matched_room.Messages.Max<NDb.Message>(Message => Message.MsgId);
					result.chat_unread_count = matched_room.Messages.Count<NDb.Message>(Message => Message.MsgId > last_chat_index);
				}

				// JoinedRoomUser 가 0 일때 체크 하기
				List<NDb.RoomJoinedUser> user_list = matched_room.RoomJoinedUsers.ToList<NDb.RoomJoinedUser>();

				foreach (NDb.RoomJoinedUser JoinedUser in user_list)
				{
					String loginId = JoinedUser.LoginId;
					String imageUrl = db.fn_GetProfileElement("ImageUrl", JoinedUser.aspnet_User.aspnet_Profile.PropertyNames,
																JoinedUser.aspnet_User.aspnet_Profile.PropertyValuesString);

					Console.WriteLine(String.Format("ImageUrl User : {0} - {1}", JoinedUser.LoginId, imageUrl));
					//index++;
				}

			}

			catch (Exception e)
			{
				result.reason_sort = -3;
				return result;
			}

			return result;
		}


		public MEMBER_PROFILE_INFO MemberProfileInfo(int room_index )
		{
			MEMBER_PROFILE_INFO result = new MEMBER_PROFILE_INFO();
			result.reason_sort = 0;
			result.room_index = room_index;

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where Master.RoomIndex == room_index
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					result.reason_sort = -1;   // not found user
					return result;
				}

				if (matched_room.Commited == false)
				{
					result.reason_sort = -2;
					return result;
				}

				result.cm_date = matched_room.CommitedDateTime.Value;

				// JoinedRoomUser 가 0 일때 체크 하기
				List<NDb.RoomJoinedUser> user_list = matched_room.RoomJoinedUsers.ToList<NDb.RoomJoinedUser>();
				
				int user_count = user_list.Count;
				result.count = user_count;
				result.MEMBER_PROFILE = new MEMBER_PROFILE_INFOMEMBER_PROFILE[user_count];

				int index = 0;
				foreach (NDb.RoomJoinedUser JoinedUser in user_list)
				{
					result.MEMBER_PROFILE[index] = new MEMBER_PROFILE_INFOMEMBER_PROFILE();

					result.MEMBER_PROFILE[index].login_id = JoinedUser.LoginId;
					result.MEMBER_PROFILE[index].imageUrl = db.fn_GetProfileElement("ImageUrl", JoinedUser.aspnet_User.aspnet_Profile.PropertyNames,
																JoinedUser.aspnet_User.aspnet_Profile.PropertyValuesString);

					index++;
				}
			}

			catch (Exception e)
			{
				result.reason_sort = -3;
				return result;
			}

			return result;
		}

		public ROOM_RESULT InviteUser(int room_index, String user_no, String dest_member_id)
		{
			ROOM_RESULT result = new ROOM_RESULT();
			result.crud = "IV";

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);
				var matched_room = (from Master in db.GetTable<NDb.CreateRoom>()
									where Master.RoomIndex == room_index
									select Master).SingleOrDefault();

				if (matched_room == null)
				{
					result.reason_sort = -1;   // not found room
					return result;
				}

				if (matched_room.UserId.Equals(UserId) == false)
				{
					result.reason_sort = -2;   // not master user
					return result;
				}

				if (matched_room.Commited == false)
				{
					result.reason_sort = -3;	// not commited room
					return result;
				}


				NDb.RoomJoinedUser mached_user = (from RoomUser in matched_room.RoomJoinedUsers
												  where RoomUser.LoginId == dest_member_id
												  select RoomUser).SingleOrDefault();
				if (mached_user != null)
				{
					result.reason_sort = -4;   // already joined user
					return result;
				}

				NDb.InvitedUser invite_user = new NDb.InvitedUser();

				var invited_user_find = (from User in db.GetTable<NDb.aspnet_User>()
									where User.UserName == dest_member_id
									select User ).SingleOrDefault();

				if (invited_user_find == null)
				{
					result.reason_sort = -5;	// not found dest user
					return result;
				}

				invite_user.RoomIndex = matched_room.RoomIndex;
				invite_user.UserId = invited_user_find.UserId;
				invite_user.InviteDateTime = DateTime.Now;

				db.InvitedUsers.InsertOnSubmit(invite_user);
				db.SubmitChanges();

				result.reason_sort = 0;
				result.room_index = (uint)matched_room.RoomIndex;

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine("================== InviteUser Error ==================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				result.reason_sort = -6;
				result.room_index = 0;

				return result;
			}

		}

		public ROOM_INFO_LIST InviteRoomList(String user_no)
		{
			ROOM_INFO_LIST room_info_list = new ROOM_INFO_LIST();

			try
			{
				Guid UserId = new Guid(user_no);
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				List<NDb.InvitedUser> invited_user = (from InviteUser in db.GetTable<NDb.InvitedUser>()
												   where InviteUser.UserId == UserId
												   select InviteUser).ToList();

				
				int join_count = invited_user.Count();

				room_info_list.invited_count = join_count;

				room_info_list.JOIN_INFO = new ROOM_INFO_LISTJOIN_INFO();
				room_info_list.JOIN_INFO.count = (byte)join_count;
				room_info_list.JOIN_INFO.ROOM = new ROOM_INFO_LISTJOIN_INFOROOM[join_count];

				int index = 0;

				foreach (NDb.InvitedUser invitedRoom in invited_user)
				{
					Console.WriteLine("InviteRoomList Room {0} Name {1} Date {2}", invitedRoom.CreateRoom.RoomIndex, invitedRoom.CreateRoom.Name, invitedRoom.CreateRoom.CreateDateTime);
					//NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "InviteRoomList Room {0} Name {1} Date {2} User :{3}", invitedRoom.CreateRoom.RoomIndex,
					//                    invitedRoom.CreateRoom.Name,
					//                    invitedRoom.CreateRoom.CreateDateTime,
					//                    invitedRoom.CreateRoom.UserId);

					room_info_list.JOIN_INFO.ROOM[index] = new ROOM_INFO_LISTJOIN_INFOROOM();
					room_info_list.JOIN_INFO.ROOM[index].index = invitedRoom.CreateRoom.RoomIndex;
					room_info_list.JOIN_INFO.ROOM[index].name = invitedRoom.CreateRoom.Name;
					room_info_list.JOIN_INFO.ROOM[index].commited = (byte)Convert.ChangeType(invitedRoom.CreateRoom.Commited, TypeCode.Byte);

					room_info_list.JOIN_INFO.ROOM[index].cm_dateSpecified = false;
					if (invitedRoom.CreateRoom.Commited == true)
					{
						room_info_list.JOIN_INFO.ROOM[index].cm_dateSpecified = true;
						room_info_list.JOIN_INFO.ROOM[index].cm_date = invitedRoom.CreateRoom.CommitedDateTime.Value;
					}

					room_info_list.JOIN_INFO.ROOM[index].comment = invitedRoom.CreateRoom.Comment;
					room_info_list.JOIN_INFO.ROOM[index].category = invitedRoom.CreateRoom.Category;
					room_info_list.JOIN_INFO.ROOM[index].location_main = invitedRoom.CreateRoom.Location_Main;
					room_info_list.JOIN_INFO.ROOM[index].location_sub = invitedRoom.CreateRoom.Location_Sub;

					// RoomJoinedUser 가 없을때 Null 인지 체크
					room_info_list.JOIN_INFO.ROOM[index].current_user = (byte)invitedRoom.CreateRoom.RoomJoinedUsers.Count;
					room_info_list.JOIN_INFO.ROOM[index].max_user = invitedRoom.CreateRoom.MaxUser;
					room_info_list.JOIN_INFO.ROOM[index].duration = invitedRoom.CreateRoom.Duration;
					index++;
				}

				NApns.Provider._source.Flush();
			}
			catch (Exception e)
			{
				Console.WriteLine("================== InviteRoomList Error ==================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				return room_info_list;
			}

			return room_info_list;
		}

		public ROOM_RESULT DeleteInvitedRoom(int room_index, String user_no)
		{
			ROOM_RESULT result = new ROOM_RESULT();
			result.crud = "DI";

			try
			{
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				Guid UserId = new Guid(user_no);

				var invited_user = (from InviteUser in db.GetTable<NDb.InvitedUser>()
								  where InviteUser.UserId == UserId && InviteUser.RoomIndex == room_index
								  select InviteUser).SingleOrDefault();

				if (invited_user == null)
				{
					result.reason_sort = -1;
					return result;
				}

				db.InvitedUsers.DeleteOnSubmit(invited_user);
				db.SubmitChanges();

				result.reason_sort = 0;
				result.room_index = (uint)room_index;

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine("================== InviteUser Error ==================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");
				
				result.reason_sort = -2;
				result.room_index = 0;

				return result;
			}
		}
	}
}