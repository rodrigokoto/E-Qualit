using IoC;
using IsotecWindowsService.Interface;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Dominio.Interface.Repositorio;
using System.Threading;
using System.Timers;

namespace IsotecWindowsService
{
    public partial class ServicoIsotec : ServiceBase
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        private readonly INotificationService _notificationService;
        private readonly ICalibracaoService _calibracaoService;
        private readonly IQualificacaoService _qualificacaoService;
        private readonly ILicencaService _licencaService;
        System.Timers.Timer _timer;
        DateTime _scheduleTime;

        public Thread ServicoMensageiroThread { get; private set; }

        public ServicoIsotec()
        {
            try
            {
                FileLogger.Log("Inicializando serviço");
                InitializeComponent();

                var kernel = CreateKernel();

                //_notificationService = kernel.Get<INotificationService>();
                _calibracaoService = kernel.Get<ICalibracaoService>();
                //_qualificacaoService = kernel.Get<IQualificacaoService>();
                //_licencaService = kernel.Get<ILicencaService>();

            }
            catch (Exception ex)
            {
                FileLogger.Log("Erro durante o processamento ", ex);
            }
        }

        public void Processar()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            try
            {

                ServicoMensageiroThread = new Thread(() => IsotecService());
                ServicoMensageiroThread.Start();

            }
            catch (Exception ex)
            {
                //Displays and Logs Message
                FileLogger.Log("Erro durante o processamento ", ex);

            }
        }



        protected override void OnStop()
        {

        }
        public void IsotecService()
        {
            while (true)
            {
                FileLogger.Log("Executando Isotec Service");

                if (DateTime.Now.Hour == 1)
                    _notificationService.SendNotification();
                _calibracaoService.AtualizaCalibracao();
                _qualificacaoService.EnfileirarEmail();
                _licencaService.EnfileirarEmail();

                TimeSpan ts = TimeSpan.FromMilliseconds(3600 * 1000);
                Thread.Sleep(ts);
            }

            // Code that runs every interval period

        }

        public void IsotecService(object state)
        {
            // Code that runs every interval period
            _notificationService.SendNotification();
            //_calibracaoService.AtualizaCalibracao();
            //_qualificacaoService.EnfileirarEmail();
            //_licencaService.EnfileirarEmail();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            var modules = new List<INinjectModule>
            {
                new ModuloAplicacao(),
                new ModuloDominio(),
                new ModuloRepositorio(),
                new ModuloServico()
            };
            kernel.Load(modules);
        }
    }
}
