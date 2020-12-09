using ApplicationService.Interface;
using Dominio.Entidade;
using IoC;
using IsotecWindowsService.Interface;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IsotecWindowsService
{
    public partial class ServicoIsotec : ServiceBase
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        private readonly INotificationService _notificationService;
        private readonly ICalibracaoService _calibracaoService;
        private readonly IQualificacaoService _qualificacaoService;
        private readonly ILicencaService _licencaService;
        private Thread IsotecServiceThread;
        private Thread IsotecFornecedorServiceThread;
        private Thread IsotecLicencaServiceThread;
        private Timer _timer = null;

        public ServicoIsotec()
        {
            var kernel = CreateKernel();

            _notificationService = kernel.Get<INotificationService>();
            _calibracaoService = kernel.Get<ICalibracaoService>();
            _qualificacaoService = kernel.Get<IQualificacaoService>();
            _licencaService = kernel.Get<ILicencaService>();

        }


        public void Debug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            //StartTimer(new TimeSpan(6, 0, 0), new TimeSpan(24, 0, 0));



            IsotecServiceThread = new Thread(() => IsotecService());
            IsotecServiceThread.Start();

            //IsotecFornecedorServiceThread = new Thread(() => QualificacaoService());
            //IsotecFornecedorServiceThread.Start();

            //IsotecLicencaServiceThread = new Thread(() => LicencaService());
            //IsotecLicencaServiceThread.Start();
        }

        protected void StartTimer(TimeSpan scheduledRunTime, TimeSpan timeBetweenEachRun)
        {
            // Initialize timer
            double current = DateTime.Now.TimeOfDay.TotalMilliseconds;
            double scheduledTime = scheduledRunTime.TotalMilliseconds;
            double intervalPeriod = timeBetweenEachRun.TotalMilliseconds;
            // calculates the first execution of the method, either its today at the scheduled time or tomorrow (if scheduled time has already occurred today)
            double firstExecution = current > scheduledTime ? intervalPeriod - (current - scheduledTime) : scheduledTime - current;

            // create callback - this is the method that is called on every interval
            TimerCallback callback = new TimerCallback(IsotecService);

            // create timer
            _timer = new Timer(callback, null, Convert.ToInt32(firstExecution), Convert.ToInt32(intervalPeriod));

        }

        protected override void OnStop()
        {

        }
        public void IsotecService()
        {
            // Code that runs every interval period
            _notificationService.SendNotification();
            //_calibracaoService.AtualizaCalibracao();
            //_qualificacaoService.EnfileirarEmail();
            //_licencaService.EnfileirarEmail();
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
