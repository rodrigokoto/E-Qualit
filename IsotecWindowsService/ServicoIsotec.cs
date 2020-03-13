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
        private readonly ICalibracaoService _calibracaoService;
        private readonly IQualificacaoService _qualificacaoService;
        private readonly ILicencaService _licencaService;
        private Thread IsotecServiceThread;
        private Thread IsotecFornecedorServiceThread;
        private Thread IsotecLicencaServiceThread;

        public ServicoIsotec()
        {
            var kernel = CreateKernel();

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
            IsotecServiceThread = new Thread(() => CalibracaoService());
            IsotecServiceThread.Start();

            IsotecFornecedorServiceThread = new Thread(() => QualificacaoService());
            IsotecFornecedorServiceThread.Start();


            IsotecLicencaServiceThread = new Thread(() => LicencaService());
            IsotecLicencaServiceThread.Start();
        }

        protected override void OnStop()
        {

        }

        private void CalibracaoService()
        {
            while (true)
            {
                _calibracaoService.AtualizaCalibracao();
                Thread.Sleep(1000);
            }
        }

        private void QualificacaoService()

        {

            while (true)

            {

                //var SysHour = ConfigurationManager.AppSettings["StartService"].Split(':');

                //var confighour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(SysHour[0]), Convert.ToInt32(SysHour[1]), 0);

                //if (DateTime.Now.Date == confighour.Date)
                //{
                //    if (DateTime.Now.Hour == confighour.Hour)
                //    {
                _qualificacaoService.EnfileirarEmail();
                //    }
                //}
                Thread.Sleep(30000);
            }
        }
        private void LicencaService()
        {

            while (true)

            {

                //var SysHour = ConfigurationManager.AppSettings["StartService"].Split(':');

                //var confighour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(SysHour[0]), Convert.ToInt32(SysHour[1]), 0);

                //if (DateTime.Now.Date == confighour.Date)
                //{
                //    if (DateTime.Now.Hour == confighour.Hour)
                //    {
                _licencaService.EnfileirarEmail();
                //    }
                //}
                Thread.Sleep(30000);
            }
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
