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
        private Thread IsotecServiceThread;
        private Thread IsotecFornecedorServiceThread;

        public ServicoIsotec()
        {
            var kernel = CreateKernel();

            _calibracaoService = kernel.Get<ICalibracaoService>();
            _qualificacaoService = kernel.Get<IQualificacaoService>();
           
        }

        
        public void Debug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            //IsotecServiceThread = new Thread(() => CalibracaoService());
            //IsotecServiceThread.Start();

            IsotecFornecedorServiceThread = new Thread(() => QualificacaoService());
            IsotecFornecedorServiceThread.Start();
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

        private void QualificacaoService() {

            while (true) {
                _qualificacaoService.EnfileirarEmail();
                Thread.Sleep(1000);
                
                //Thread.Sleep(TimeSpan.FromDays(1));
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
