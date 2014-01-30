using APLPromoter.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM = System.ServiceModel;


namespace APLPromoter.UI.ServiceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting up services...");
            Console.WriteLine("");

            SM.ServiceHost hostUserService = new SM.ServiceHost(typeof(UserService));
            StartService(hostUserService, "UserService");


            SM.ServiceHost hostAnalyticService = new SM.ServiceHost(typeof(AnalyticService));
            StartService(hostAnalyticService, "AnalyticService");


            Console.WriteLine("");
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
            Console.WriteLine("");


        }

        static void StartService(SM.ServiceHost host, string serviceDescription)
        {
            host.Open();
            Console.WriteLine("Service '{0}' started.", serviceDescription);

            foreach (var endpoint in host.Description.Endpoints)
            {
                Console.WriteLine(string.Format("Listening on endpoint:"));
                Console.WriteLine(string.Format("Address: {0}", endpoint.Address.Uri.ToString()));
                Console.WriteLine(string.Format("Binding: {0}", endpoint.Binding.Name));
                Console.WriteLine(string.Format("Contract: {0}", endpoint.Contract.ConfigurationName));
            }

            Console.WriteLine();
        }
    }
}
