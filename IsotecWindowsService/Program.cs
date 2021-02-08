﻿using ApplicationService.Interface;
using IsotecWindowsService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace IsotecWindowsService
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            new ServicoIsotec().Processar();
           

            //#if DEBUG

            //            ServicoIsotec servicoIsotec = new ServicoIsotec();

            //            servicoIsotec.IsotecService();
            //#else
            //                        ServiceBase[] ServicesToRun;
            //                        ServicesToRun = new ServiceBase[]
            //                        {
            //                            new ServicoIsotec()
            //                        };
            //                        ServiceBase.Run(ServicesToRun);
            //#endif
        }
    }
}
