using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;

using System.Data.Linq;
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
		CHAT_LIST Chat(UInt32 room_index, String user_no, int local_index, int last_update, String content);
		[OperationContract]
		CHAT_LIST ChatDb(UInt32 room_index, String user_no, int local_index, int last_update, String content);

		[OperationContract]
		CHAT_LIST ChatUpdate(UInt32 room_index, String user_no, int last_update);
		[OperationContract]
		CHAT_LIST ChatUpdateDb(UInt32 room_index, String user_no, int last_update);

		[OperationContract]
		NOTICE_LIST CreateNotice(UInt32 room_index, String user_no, int group, String title, String content);

		[OperationContract]
		NOTICE_LIST DeleteNotice(UInt32 room_index, String user_no, int group, int notice_index);

		[OperationContract]
		NOTICE_LIST UpdateNotice(UInt32 room_index, String user_no, int group, int last_update);

		//[OperationContract]
		//void UpdateBadge(UInt32 room_index, String user_no, int last_update);
	}

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
					 ConcurrencyMode = ConcurrencyMode.Single)]

	public class RoomWCFService : IRoom, IDisposable
	{
		static public int nValue = 10;
		static public NLogic.NRoom.List _roomList = new NLogic.NRoom.List();
		static public Dictionary<RoomSearchKey, NLogic.NRoom.List> _roomTree = new Dictionary<RoomSearchKey, NLogic.NRoom.List>();

		static public NLogic.NUser.List _userList = new NLogic.NUser.List();
		public NApns.Provider _apnsProvider = null;

		public RoomWCFService()
		{
			_apnsProvider = new NApns.Provider("iphone_dev.p12", "roy3513!", true);
			NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "WCFRoomService() called!!!!!!!!!!!!!!!!!");
			NApns.Provider._source.Flush();
		}

		public void Dispose()
		{
			NApns.Provider._source.TraceEvent(TraceEventType.Critical, 3, "Dispose()!!!!!!!!!!!!!!!!!!");
			NApns.Provider._source.Flush();
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

		public UPDATE_DEVICE_INFO UpdateUserDeviceDb(String user_guid, String deviceToken)
		{
			UPDATE_DEVICE_INFO update_device_info = new UPDATE_DEVICE_INFO();

			try
			{

				Guid UserId = new Guid(user_guid);

				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();
				String LoginId = null;

				var match_info = (from Device in db.GetTable<NDb.UserDeviceInfo>()
								  where (Device.UserId == UserId)
								  select Device).SingleOrDefault();

				if (match_info != null)
				{
					match_info.DeviceToken = deviceToken;
					db.SubmitChanges();

					update_device_info.login_id = match_info.aspnet_User.UserName;

					return update_device_info;
				}

				NDb.UserDeviceInfo update_info = new NDb.UserDeviceInfo();
				update_info.DeviceToken = deviceToken;
				update_info.Type = 0;
				update_info.UserId = UserId;

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

			room_info_list.JOIN_INFO = new ROOM_INFO_LISTROOM1[room_count];

			int index = 0;

			foreach (KeyValuePair<UInt32, NLogic.Room> pair in user.JoinCommitedList)
			{
				NLogic.Room room = pair.Value;

				room_info_list.JOIN_INFO[index] = new ROOM_INFO_LISTROOM1();
				room_info_list.JOIN_INFO[index].index = room.Index;
				room_info_list.JOIN_INFO[index].name = room.Name;
				room_info_list.JOIN_INFO[index].commited = 1;
				room_info_list.JOIN_INFO[index].comment = room.Comment;
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
				room_info_list.JOIN_INFO[index].comment = room.Comment;
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
				room_info_list.CREATE_INFO[index].comment = room.Comment;
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
				room_info_list.CREATE_INFO[index].comment = room.Comment;
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

		public ROOM_INFO_LIST MyRoomListDb(String user_no)
		{
			ROOM_INFO_LIST room_info_list = new ROOM_INFO_LIST();

			try
			{
				Guid UserId = new Guid(user_no);
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				List<NDb.NData.JoinedRoom> room_list = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
														join room in db.GetTable<NDb.CreateRoom>()
														on RoomUser.RoomIndex equals room.RoomIndex
														where (RoomUser.UserId == UserId)
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
															MasterUserId = room.UserId.ToString()

														}).ToList<NDb.NData.JoinedRoom>();

				Console.WriteLine("MyRoomListDb count...{0}", room_list.Count);

				IEnumerable<NDb.NData.JoinedRoom> query_created = from create_room in room_list where create_room.MasterUserId.Equals( user_no ) select create_room;
				int create_count = query_created.Count<NDb.NData.JoinedRoom>();
				room_info_list.CREATE_INFO = new ROOM_INFO_LISTROOM[create_count];
				int index = 0;
				foreach (NDb.NData.JoinedRoom createdRoom in query_created)
				{
					Console.WriteLine("MyRoomListDb Create Room {0} Name {1} Date {2}", createdRoom.Index,
																				createdRoom.Name, createdRoom.CreateDate);
					room_info_list.CREATE_INFO[index] = new ROOM_INFO_LISTROOM();
					room_info_list.CREATE_INFO[index].index = (uint)createdRoom.Index;
					room_info_list.CREATE_INFO[index].name = createdRoom.Name;
					room_info_list.CREATE_INFO[index].commited = (byte)Convert.ChangeType(createdRoom.Commited, TypeCode.Byte);
					room_info_list.CREATE_INFO[index].comment = createdRoom.Comment;
					room_info_list.CREATE_INFO[index].category = createdRoom.Category;
					room_info_list.CREATE_INFO[index].location_main = createdRoom.LocationMain;
					room_info_list.CREATE_INFO[index].location_sub = createdRoom.LocationSub;
					room_info_list.CREATE_INFO[index].current_user = createdRoom.CurrentUser;
					room_info_list.CREATE_INFO[index].max_user = createdRoom.MaxUser;
					room_info_list.CREATE_INFO[index].duration = createdRoom.Duration;
					index++;
				}

				IEnumerable<NDb.NData.JoinedRoom> query_joined = from join_room in room_list 
																 where !(join_room.MasterUserId.Equals(user_no))
																 select join_room;

				int join_count = query_joined.Count<NDb.NData.JoinedRoom>();
				room_info_list.JOIN_INFO = new ROOM_INFO_LISTROOM1[join_count];
				index = 0;

				foreach (NDb.NData.JoinedRoom joinedRoom in query_joined)
				{
					Console.WriteLine("MyRoomListDb Joined Room {0} Name {1} Date {2}", joinedRoom.Index, joinedRoom.Name, joinedRoom.CreateDate);

					room_info_list.JOIN_INFO[index] = new ROOM_INFO_LISTROOM1();
					room_info_list.JOIN_INFO[index].index = (uint)joinedRoom.Index;
					room_info_list.JOIN_INFO[index].name = joinedRoom.Name;
					room_info_list.JOIN_INFO[index].commited = (byte)Convert.ChangeType(joinedRoom.Commited, TypeCode.Byte);
					room_info_list.JOIN_INFO[index].comment = joinedRoom.Comment;
					room_info_list.JOIN_INFO[index].category = joinedRoom.Category;
					room_info_list.JOIN_INFO[index].location_main = joinedRoom.LocationMain;
					room_info_list.JOIN_INFO[index].location_sub = joinedRoom.LocationSub;
					room_info_list.JOIN_INFO[index].current_user = joinedRoom.CurrentUser;
					room_info_list.JOIN_INFO[index].max_user = joinedRoom.MaxUser;
					room_info_list.JOIN_INFO[index].duration = joinedRoom.Duration;
					index++;
				}
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
															MasterUserId = room.UserId.ToString()

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
										MasterUserId = room.UserId.ToString()
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

		public CHAT_LIST ChatDb(UInt32 room_index, String user_no, int local_index, int last_update, String content)
		{
			CHAT_LIST chat_list = new CHAT_LIST();
			chat_list.count = 0;
			chat_list.room_index = room_index;

			try
			{
				Guid UserId = new Guid(user_no);
				NDb.RoomDataClassesDataContext db = new NDb.RoomDataClassesDataContext();

				var message = (from JoinedUser in db.GetTable<NDb.RoomJoinedUser>()
							   where (JoinedUser.RoomIndex == room_index && JoinedUser.UserId == UserId)
							   //join Profile in db.GetTable<NDb.aspnet_Profile>() on UserId equals Profile.UserId
							   select new NDb.NData.ChatMessage
							   {
								   MsgId = (from Message in db.GetTable<NDb.Message>()
											where Message.RoomIndex == room_index
											select Message.MsgId).Count(),

								   Contents = content,
								   //NickName = db.fn_GetProfileElement("NickName", JoinedUser.aspnet_User.aspnet_Profile.PropertyNames,
								   //     JoinedUser.aspnet_User.aspnet_Profile.PropertyValuesString),
								   NickName = JoinedUser.NickName,	// NickName 만 JoinedUser 에 저장할까나?... 아니면 위에 같이 조인시킬까?
								   Email = JoinedUser.LoginId,	
								   //Email = JoinedUser.aspnet_User.UserName,	// squence 가 하나 여야 성공한다.

							   }).SingleOrDefault<NDb.NData.ChatMessage>();

				if (message == null)
				{
					Console.WriteLine("Invalid ChatMsg...");
					chat_list.local_index = 0;
					return chat_list;
				}

				NDb.Message insert_message = new NDb.Message();

				insert_message.MsgId = ++ message.MsgId;
				insert_message.IptTime = DateTime.Now;
				insert_message.Contents = message.Contents;
				insert_message.RoomIndex = (int)room_index;
				insert_message.NickName = message.NickName;
				insert_message.Email = message.Email;

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

					// 클라이언트에서 판단하도록 수정요망
					//if (user.UserGuid.Equals(query_message.UserGuid))
					//{
					//    chat_list.CHAT[nIndex].ownerSpecified = true;
					//    chat_list.CHAT[nIndex].owner = 1;
					//}
					//else
					//{
					//    chat_list.CHAT[nIndex].ownerSpecified = false;
					//}

					nIndex++;
				}

				chat_list.local_index = local_index;

				// Backend push server 로 옮겨야 ..
				List<String> device_info_list = (from RoomUser in db.GetTable<NDb.RoomJoinedUser>()
												 where RoomUser.RoomIndex == room_index
												join DeviceInfo in db.GetTable<NDb.UserDeviceInfo>() 
																on RoomUser.UserId equals DeviceInfo.UserId
												 select DeviceInfo.DeviceToken ).ToList<String>();


				Console.WriteLine("Push Notification to room {0} ", room_index );

				foreach (String device_info in device_info_list)
				{
					Console.WriteLine("DeviceToken {0} ", device_info );
				}

				return chat_list;
			}
			catch (Exception e)
			{
				Console.WriteLine("======================== Chat Error ==========================");
				Console.WriteLine("{0}", e.Message);
				Console.WriteLine("==============================================================");

				chat_list.local_index = 0;
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

					// 클라이언트에서 판단하도록 수정요망

					//if (user.UserGuid.Equals(query_message.UserGuid))
					//{
					//    chat_list.CHAT[nIndex].ownerSpecified = true;
					//    chat_list.CHAT[nIndex].owner = 1;
					//}
					//else
					//{
					//    chat_list.CHAT[nIndex].ownerSpecified = false;
					//}

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


	}
}