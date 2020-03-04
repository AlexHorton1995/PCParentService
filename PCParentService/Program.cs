using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PCParentServiceApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            PCParentService service = new PCParentService();

            if (Environment.UserInteractive)
            {
                service.RunAsConsole(args);
            }
            else
            {

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new PCParentService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
