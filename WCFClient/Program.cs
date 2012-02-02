using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;

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



				ROOM_RESULT rr = new ROOM_RESULT();
			
                String resultString = test.Test2("콜콜");

                ROOM_RESULT test2 = test.Test("UISEREWRWERWE");
                bool test3 = test.LoginUser("test_userno1", "roy1669@daum.net", "최진혁", new DateTime(), 1, "wefwefwefwe");
				bool test4 = test.LoginUser("test_userno2", "roy1669@daum.net", "두번째", new DateTime(), 1, "wefwefwefwe");

				JOIN_ROOM_DETAIL detail = test.GetLoginUser();

				ROOM_INFO_LIST room_info_list = test.MyRoomList(" TEST");




				ROOM_RESULT result12 = test.CreateRoomDb("9CA1A2F4-AA7C-462C-96D2-1F5FE26D691D", key, "웹에서", "아무나오지말고", "1111111", 10);

				
				ROOM_RESULT result111 = test.Push("f34d46277322e73b8e67b23c8339158a1621f1c49a05127fd839e4d0f7c4f6c6", "좋다", 1);



            }
            catch (FaultException<RoomService.MyFaultException> ee)
            {
                result = "Exception : " + ee.Detail.Reason ;
                Console.WriteLine(result);
 
            }
                 

		}
	}
}
