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
			IRoom proxy = factory.CreateChannel();
			ROOM_RESULT res = proxy.Push(deviceTOken, message, badge);
			(proxy as IDisposable).Dispose();

			return res;
		}
	}
}
