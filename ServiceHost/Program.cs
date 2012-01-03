using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using RoomService;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(RoomWCFService),
                   new Uri("net.tcp://localhost/wcf/roomwcfservice"));

            host.AddServiceEndpoint(
                typeof(IRoom),        // service contract
                new NetTcpBinding(),      // service binding
                "");

            host.Open();

            Console.WriteLine("Press Any key to stop the service");
            Console.ReadKey(true);

            host.Close();
        }
    }
}
