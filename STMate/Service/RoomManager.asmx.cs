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
          new BasicHttpBinding(),
            new EndpointAddress(new Uri("http://www.studyheyo.co.kr/Service/RoomService.RoomWCFService.svc"))));


        [WebMethod(EnableSession = true)]
        public ROOM_INFO_LIST MyRoomList(String user_no)
        {
            IRoom proxy = factory.CreateChannel();
            ROOM_INFO_LIST room_info_list = proxy.MyRoomList(user_no);

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

            ROOM_SUMMARY_LIST room_summary_list = proxy.AllRoomList(search_key, user_no);

            (proxy as IDisposable).Dispose();

            return room_summary_list;
        }

        [WebMethod(EnableSession = true)]
        public JOIN_ROOM_DETAIL JoinRoomDetail(String user_no, UInt32 room_index)
        {
            IRoom proxy = factory.CreateChannel();

            JOIN_ROOM_DETAIL join_room_detail = proxy.JoinRoomDetail(room_index, user_no);

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

            ROOM_RESULT result = proxy.CreateRoom(user_no, search_key, name, comment, duration, maxuser);
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
            ROOM_RESULT commit_result = proxy.CommitRoom(user_no, room_index);
            (proxy as IDisposable).Dispose();

            return commit_result;
        }

        [WebMethod(EnableSession = true)]
        public ROOM_RESULT Join(String user_no, UInt32 room_index)
        {
            IRoom proxy = factory.CreateChannel();
            ROOM_RESULT join_result = proxy.JoinRoom(user_no, room_index);
            (proxy as IDisposable).Dispose();

            return join_result;
        }


        [WebMethod(EnableSession = true)]
        public ROOM_RESULT Leave(String user_no, UInt32 room_index)
        {
            IRoom proxy = factory.CreateChannel();
            ROOM_RESULT join_result = proxy.LeaveRoom(user_no, room_index);
            (proxy as IDisposable).Dispose();

            return join_result;
        }


        [WebMethod(EnableSession = true)]
        public CHAT_LIST Chat(String user_no, UInt32 room_index, int local_index, int last_update, String message)
        {
            IRoom proxy = factory.CreateChannel();
            CHAT_LIST chat_list = proxy.Chat(room_index, user_no,local_index, last_update, message);
            (proxy as IDisposable).Dispose();

            return chat_list;
        }

        [WebMethod(EnableSession = true)]
        public CHAT_LIST ChatUpdate(String user_no, UInt32 room_index, int last_update)
        {
            IRoom proxy = factory.CreateChannel();
            CHAT_LIST chat_list = proxy.ChatUpdate(room_index, user_no, last_update);
            (proxy as IDisposable).Dispose();

            return chat_list;
        }

        [WebMethod(EnableSession = true)]
        public NOTICE_LIST CreateNotice(String user_no, UInt32 room_index, int group, String title, String content)
        {
            IRoom proxy = factory.CreateChannel();
            NOTICE_LIST notice_list = proxy.CreateNotice(room_index, user_no, group, title, content);
            (proxy as IDisposable).Dispose();

            return notice_list;
        }

        [WebMethod(EnableSession = true)]
		public NOTICE_LIST DeleteNotice(String user_no, UInt32 room_index, int group, int notice_index)
        {
            IRoom proxy = factory.CreateChannel();
			NOTICE_LIST notice_list = proxy.DeleteNotice(room_index, user_no, group, notice_index);
            (proxy as IDisposable).Dispose();

            return notice_list;
        }


        [WebMethod(EnableSession = true)]
		public NOTICE_LIST UpdateNotice(String user_no, UInt32 room_index, int group, int last_update)
        {
            IRoom proxy = factory.CreateChannel();
			NOTICE_LIST notice_list = proxy.UpdateNotice(room_index, user_no, group, last_update);
            (proxy as IDisposable).Dispose();

            return notice_list;
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
