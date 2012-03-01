using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Caching;
using System.ServiceModel;
using System.ServiceModel.Description;
using RoomService;

namespace STMate.Service
{
    /// <summary>
    /// Summary description for RoomManager
    /// </summary>
    [WebService(Namespace = "http://www.studyheyo.co.kr")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RoomManager : System.Web.Services.WebService
    {
        private ChannelFactory<IRoom> factory = new ChannelFactory<IRoom>(new ServiceEndpoint(
			ContractDescription.GetContract(typeof(IRoom)),
			//ContractDescription.GetContract(typeof(IRoomDb)),
          new BasicHttpBinding(),
            new EndpointAddress(new Uri("http://www.studyheyo.co.kr/Service/RoomService.RoomWCFService.svc"))));
			//new EndpointAddress(new Uri("http://www.studyheyo.co.kr/Service/RoomManager.svc"))));


        [WebMethod(EnableSession = true)]
        public ROOM_INFO_LIST MyRoomList(String user_no)
        {
            IRoom proxy = factory.CreateChannel();
            ROOM_INFO_LIST room_info_list = proxy.MyRoomListDb(user_no);

            (proxy as IDisposable).Dispose();

            return room_info_list;
        }

        [WebMethod(EnableSession = true)]
        public ROOM_SUMMARY_LIST AllRoomList(Int32 category, Int32 location_main, Int32 location_sub, String user_no)
        {
            IRoom proxy = factory.CreateChannel();
            RoomSearchKey search_key = new RoomSearchKey();
            search_key._category = category;
            search_key._location_main = location_main;
            search_key._location_sub = location_sub;

            ROOM_SUMMARY_LIST room_summary_list = proxy.AllRoomListDb(search_key, user_no, 0);

            (proxy as IDisposable).Dispose();

            return room_summary_list;
        }

        [WebMethod(EnableSession = true)]
        public JOIN_ROOM_DETAIL JoinRoomDetail(String user_no, UInt32 room_index)
        {
            IRoom proxy = factory.CreateChannel();

            JOIN_ROOM_DETAIL join_room_detail = proxy.JoinRoomDetailDb(room_index, user_no);

            (proxy as IDisposable).Dispose();

            return join_room_detail;
        }

        [WebMethod(EnableSession = true)]
        public ROOM_RESULT Create(String user_no, Int32 category, Int32 location_main, Int32 location_sub, String name, String comment, String duration, int maxuser)
        {
            IRoom proxy = factory.CreateChannel();
            RoomSearchKey search_key = new RoomSearchKey();
            search_key._category = category;
            search_key._location_main = location_main;
            search_key._location_sub = location_sub;

            ROOM_RESULT result = proxy.CreateRoomDb(user_no, search_key, name, comment, duration, maxuser);
            (proxy as IDisposable).Dispose();

            if (result != null)
            {
                return result;
            }

            ROOM_RESULT resultError = new ROOM_RESULT();
            resultError.crud = "CR";
            resultError.reason_sort = -1;
            return resultError;
        }

        [WebMethod(EnableSession = true)]
        public ROOM_RESULT Commit(String user_no, UInt32 room_index)
        {
            IRoom proxy = factory.CreateChannel();
            ROOM_RESULT commit_result = proxy.CommitRoomDb(user_no, room_index);
            (proxy as IDisposable).Dispose();

            return commit_result;
        }

        [WebMethod(EnableSession = true)]
        public ROOM_RESULT Join(String user_no, UInt32 room_index)
        {
            IRoom proxy = factory.CreateChannel();
            ROOM_RESULT join_result = proxy.JoinRoomDb(user_no, room_index);
            (proxy as IDisposable).Dispose();

            return join_result;
        }


        [WebMethod(EnableSession = true)]
        public ROOM_RESULT Leave(String user_no, UInt32 room_index)
        {
            IRoom proxy = factory.CreateChannel();
            ROOM_RESULT join_result = proxy.LeaveRoomDb(user_no, room_index);
            (proxy as IDisposable).Dispose();

            return join_result;
        }


        [WebMethod(EnableSession = true)]
        public CHAT_LIST Chat(String user_no, UInt32 room_index, int local_index, int last_update, String message)
        {
            IRoom proxy = factory.CreateChannel();
            CHAT_LIST chat_list = proxy.ChatDb(room_index, user_no,local_index, last_update, message);
            (proxy as IDisposable).Dispose();

            return chat_list;
        }

        [WebMethod(EnableSession = true)]
        public CHAT_LIST ChatUpdate(String user_no, UInt32 room_index, int last_update)
        {
            IRoom proxy = factory.CreateChannel();
            CHAT_LIST chat_list = proxy.ChatUpdateDb(room_index, user_no, last_update);
            (proxy as IDisposable).Dispose();

            return chat_list;
        }

        [WebMethod(EnableSession = true)]
        public NOTICE_LIST CreateNotice(String user_no, UInt32 room_index, int group, String title, String content)
        {
            IRoom proxy = factory.CreateChannel();
            NOTICE_LIST notice_list = proxy.CreateNoticeDb(room_index, user_no, group, title, content);
            (proxy as IDisposable).Dispose();

            return notice_list;
        }

        [WebMethod(EnableSession = true)]
		public NOTICE_LIST DeleteNotice(String user_no, UInt32 room_index, int group, int notice_index)
        {
            IRoom proxy = factory.CreateChannel();
			NOTICE_LIST notice_list = proxy.DeleteNoticeDb(room_index, user_no, group, notice_index);
            (proxy as IDisposable).Dispose();

            return notice_list;
        }


        [WebMethod(EnableSession = true)]
		public NOTICE_LIST UpdateNotice(String user_no, UInt32 room_index, int group, int last_update)
        {
            IRoom proxy = factory.CreateChannel();
			NOTICE_LIST notice_list = proxy.UpdateNoticeDb(room_index, user_no, group, last_update);
            (proxy as IDisposable).Dispose();

            return notice_list;
        }

		[WebMethod(EnableSession = true)]
		public ROOM_RESULT UpdatePenaltyInfo(UInt32 room_index, String user_no, int deposit, int absenceA, int absenceB, int lateness, int homework)
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_RESULT join_result = proxy.UpdatePenaltyInfo(room_index, user_no, deposit, absenceA, absenceB, lateness, homework);
			(proxy as IDisposable).Dispose();

			return join_result;
		}

		[WebMethod(EnableSession = true)]
		public MEMBER_DETAIL_INFO CheckUserPenalty(Int32 room_index, String user_no, String member_LoginId, int penalty)
		{
			IRoom proxy = factory.CreateChannel();
			MEMBER_DETAIL_INFO result = proxy.CheckUserPenalty(room_index, user_no, member_LoginId, penalty);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public MEMBER_DETAIL_INFO MemberDetailInfo(Int32 room_index, String user_no)
		{
			IRoom proxy = factory.CreateChannel();
			MEMBER_DETAIL_INFO result = proxy.MemberDetailInfo(room_index, user_no);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public ROOM_PENALTY GetPenaltyInfo(Int32 room_index, String user_no)
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_PENALTY result = proxy.GetPenaltyInfo(room_index, user_no);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public ROOM_RESULT RecruitMember(Int32 room_index, String user_no)
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_RESULT result = proxy.RecruitMember(user_no, room_index);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public ROOM_RESULT EntrustMaster(int room_index, String user_no, String dest_member_id)
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_RESULT result = proxy.EntrustMaster(room_index, user_no, dest_member_id);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public ROOM_MAIN_INFO GetRoomMainInfo(int room_index, String user_no, int last_chat_index)
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_MAIN_INFO result = proxy.GetRoomMainInfo(room_index, user_no, last_chat_index);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public MEMBER_PROFILE_INFO MemberProfileInfo(int room_index)
		{
			IRoom proxy = factory.CreateChannel();
			MEMBER_PROFILE_INFO result = proxy.MemberProfileInfo(room_index);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public ROOM_RESULT InviteUser(int room_index, String user_no, String dest_member_id)
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_RESULT result = proxy.InviteUser(room_index, user_no, dest_member_id);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public ROOM_INFO_LIST InviteRoomList(String user_no)
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_INFO_LIST result = proxy.InviteRoomList(user_no);
			(proxy as IDisposable).Dispose();

			return result;
		}

		[WebMethod(EnableSession = true)]
		public ROOM_RESULT DeleteInvitedRoom(String user_no, int room_index )
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_RESULT result = proxy.DeleteInvitedRoom(room_index, user_no);
			(proxy as IDisposable).Dispose();

			return result;
		}
		[WebMethod(EnableSession = true)]
		public JOIN_ROOM_DETAIL GetUser()
		{
			IRoom proxy = factory.CreateChannel();
			JOIN_ROOM_DETAIL notice_list = proxy.GetLoginUser();
			(proxy as IDisposable).Dispose();

			return notice_list;
		}


		[WebMethod(EnableSession = true)]
		public ROOM_RESULT Push( String deviceTOken, String message, int badge )
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_RESULT res = proxy.Push(deviceTOken, message, badge);
			(proxy as IDisposable).Dispose();

			return res;
		}


    }
}
