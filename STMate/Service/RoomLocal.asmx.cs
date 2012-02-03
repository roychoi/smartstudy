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
	/// Summary description for RoomLocal
	/// </summary>
	[WebService(Namespace = "http://localhost")]
	[WebServiceBinding(ConformsTo = WsiProfiles.None)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class RoomLocal : System.Web.Services.WebService
	{
		private ChannelFactory<IRoom> factory = new ChannelFactory<IRoom>(new ServiceEndpoint(
			ContractDescription.GetContract(typeof(IRoom)),
		  new NetTcpBinding(),
			new EndpointAddress(new Uri("net.tcp://localhost/wcf/roomwcfservice"))));

    
		[WebMethod]
		public string HelloWorld()
		{
			return "Hello World";
		}

		[WebMethod(EnableSession = true)]
		public ROOM_RESULT Push(String deviceTOken, String message, int badge)
		{
			try
			{
				IRoom proxy = factory.CreateChannel();
				ROOM_RESULT res = proxy.Push(deviceTOken, message, badge);
				(proxy as IDisposable).Dispose();

				return res;

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		[WebMethod(EnableSession = true)]
		public ROOM_RESULT Test(String user_no)
		{
			try
			{
				IRoom proxy = factory.CreateChannel();
				ROOM_RESULT res = proxy.Test(user_no);
				(proxy as IDisposable).Dispose();

				return res;

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}


		[WebMethod(EnableSession = true)]
		public ROOM_RESULT CreateRoom(String user_no, Int32 category, Int32 location_main, Int32 location_sub, String name, String comment, String duration, int maxuser)
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
		public ROOM_INFO_LIST RoomListDb(String user_no)
		{
			IRoom proxy = factory.CreateChannel();
			ROOM_INFO_LIST result = proxy.MyRoomListDb(user_no);
			(proxy as IDisposable).Dispose();

			return result;
		}
	}
}
