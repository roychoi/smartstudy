using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

using RoomService;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {

			DateTime test = DateTime.Parse("1981-01-01");

            ServiceHost host = new ServiceHost(typeof(RoomWCFService),
                   new Uri("net.tcp://localhost/wcf/roomwcfservice"));

			ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();

			// if not found - add behavior with setting turned on  
			if (debug == null)
			{
				host.Description.Behaviors.Add(
					 new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
			}
			else
			{
				// make sure setting is turned ON 
				if (!debug.IncludeExceptionDetailInFaults)
				{
					debug.IncludeExceptionDetailInFaults = true;
				}
			}
			
			//ServiceMetadataBehavior metaDataBeh = host.Description.Behaviors.Find<ServiceMetadataBehavior>();

			//host.Description.Behaviors.Add(
			//         new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

			NetTcpBinding bd = new NetTcpBinding();
			
            host.AddServiceEndpoint(
                typeof(IRoom),        // service contract
				bd,      // service binding
                "");
			

            host.Open();

            Console.WriteLine("Press Any key to stop the service");
            Console.ReadKey(true);

            host.Close();
        }
    }
}
