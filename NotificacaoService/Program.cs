using Dominio.Servico;
using System.ServiceProcess;

namespace NotificacaoWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if (DEBUG)
            SendNotificacao.Enviar();
            return;
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new NotificacaoServico()
			};
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
