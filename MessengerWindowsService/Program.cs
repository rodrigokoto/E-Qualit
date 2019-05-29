using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

        #if (DEBUG)
            new ServicoMensageiro().Processar();
            return;
        #else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServicoMensageiro()
            };
            ServiceBase.Run(ServicesToRun);
        #endif
        }
    }
}
