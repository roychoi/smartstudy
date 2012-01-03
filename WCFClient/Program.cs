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

            RoomClient test = new RoomClient("BasicHttpBinding_IRoom");

            String result;

            try
            {

                String resultString = test.Test2("콜콜");

                ROOM_RESULT test2 = test.Test("UISEREWRWERWE");
                bool test3 = test.LoginUser("test_userno1", "roy1669@daum.net", "최진혁", new DateTime(), 1, "wefwefwefwe");
				bool test4 = test.LoginUser("test_userno2", "roy1669@daum.net", "두번째", new DateTime(), 1, "wefwefwefwe");

				JOIN_ROOM_DETAIL detail = test.GetLoginUser();

				ROOM_INFO_LIST room_info_list = test.MyRoomList(" TEST");

				ROOM_RESULT result111 = test.Push("f34d46277322e73b8e67b23c8339158a1621f1c49a05127fd839e4d0f7c4f6c6", "잇힝", 1);


            }
            catch (FaultException<RoomService.MyFaultException> ee)
            {
                result = "Exception : " + ee.Detail.Reason ;
                Console.WriteLine(result);
 
            }
                 

		}
	}
}
