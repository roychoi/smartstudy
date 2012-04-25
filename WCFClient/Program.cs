using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Web;

namespace WCFClient
{
	class Program
	{
		
		static void Main(string[] args)
		{
			//Service1Client client = new Service1Client("BasicHttpBinding_IService1");

			//WCFATest.CompositeType value = new WCFATest.CompositeType();
			//value.BoolValue = true;
			//value.StringValue = "TEST!!!!";

			//String reString = client.GetData(333);
			//WCFATest.CompositeType  reComposite = client.GetDataUsingDataContract(value);
			ChannelFactory<IRoom> factory = new ChannelFactory<IRoom>(new ServiceEndpoint(
				ContractDescription.GetContract(typeof(IRoom)),
			  new NetTcpBinding(),
				new EndpointAddress(new Uri("net.tcp://localhost/wcf/roomwcfservice"))));


            String result;

            try
            {
				RoomService.RoomSearchKey key = new RoomService.RoomSearchKey();
				key._category = 1;
				key._location_main = 2;
				key._location_sub = 3;

				//IRoom proxy = factory.CreateChannel();
				//ROOM_RESULT rr2 = proxy.CreateRoomDb("9CA1A2F4-AA7C-462C-96D2-1F5FE26D691D", key, "첫방", "아무나오지말고", "1111111", 10);
				//(proxy as IDisposable).Dispose();

				RoomClient test = new RoomClient("BasicHttpBinding_IRoom");
				RoomClient testTcp = new RoomClient("NetTcpBinding_IRoomLocal");



				ROOM_RESULT rr = new ROOM_RESULT();
			
                String resultString = test.Test2("콜콜");

                ROOM_RESULT test2 = test.Test("UISEREWRWERWE");
                bool test3 = test.LoginUser("test_userno1", "roy1669@daum.net", "최진혁", new DateTime(), 1, "wefwefwefwe");
				bool test4 = test.LoginUser("test_userno2", "roy1669@daum.net", "두번째", new DateTime(), 1, "wefwefwefwe");

				JOIN_ROOM_DETAIL detail = test.GetLoginUser();

				ROOM_INFO_LIST room_info_list = test.MyRoomList(" TEST");




				ROOM_RESULT result12 = test.CreateRoomDb("410fba26-f787-421e-9bbe-1e13f9866da8", key, "내방은", "아무나와요", "1111111", 10);
				//ROOM_RESULT result12 = test.CreateRoomDb("9CA1A2F4-AA7C-462C-96D2-1F5FE26D691D", key, "웹에서", "아무나오지말고", "1111111", 10);

				//410fba26-f787-421e-9bbe-1e13f9866da8
				//ROOM_RESULT result111 = test.Push("f34d46277322e73b8e67b23c8339158a1621f1c49a05127fd839e4d0f7c4f6c6", "좋다", 1);


				IRoom proxy = factory.CreateChannel();
				
				//ROOM_RESULT rr2 = proxy.Push("f34d46277322e73b8e67b23c8339158a1621f1c49a05127fd839e4d0f7c4f6c6", "잇힝", 1);
				ROOM_RESULT rr2 = proxy.Push("a2df3ffc3a216b535907d042412c75694a704a82d18d61ab85d081d06073e857", "잇힝", 1);
				(proxy as IDisposable).Dispose();
            }
            catch (FaultException<RoomService.MyFaultException> ee)
            {
                result = "Exception : " + ee.Detail.Reason ;
                Console.WriteLine(result);
 
            }
                 

		}
	}
}
