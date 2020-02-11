using ApplicationService.Interface;
using ApplicationService.Servico;
using Dominio.Interface.Servico;
using Dominio.Servico;
using IsotecWindowsService.Interface;
using IsotecWindowsService.Service;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC
{
    public class ModuloServico : NinjectModule
    {
        public override void Load()
        {
            Bind<ICalibracaoService>().To<CalibracaoService>();
            Bind<IQualificacaoService>().To<QualificacaoService>();
            Bind<ILicencaService>().To<LicencaService>();


        }
    }
}
