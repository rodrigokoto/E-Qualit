using ApplicationService.Servico;
using Dominio.Interface.Servico;
using Dominio.Interface.Servico.RH;
using Dominio.Servico;
using Dominio.Servico.RH;
using Ninject.Modules;

namespace IoC
{
    public class ModuloDominio : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocDocumentoServico>().To<DocDocumentoServico>();
            Bind<IDocUsuarioVerificaAprovaServico>().To<DocUsuarioVerificaAprovaServico>();
            Bind<IDocTemplateServico>().To<DocTemplateServico>();
            Bind<ISiteModuloServico>().To<SiteModuloServico>();
            Bind<ISiteServico>().To<SiteServico>();
            Bind<IUsuarioClienteSiteServico>().To<UsuarioClienteSiteServico>();
            Bind<IUsuarioCargoServico>().To<UsuarioCargoServico>();
            Bind<ICargoServico>().To<CargoServico>();
            Bind<IControladorCategoriasServico>().To<ControladorCategoriasServico>();
            Bind<IRegistroConformidadesServico>().To<RegistroConformidadesServico>();
            Bind<IAnaliseCriticaServico>().To<AnaliseCriticaServico>();
            Bind<IProcessoServico>().To<ProcessoServico>();
            Bind<IUsuarioServico>().To<UsuarioServico>();
            Bind<INotificacaoServico>().To<NotificacaoServico>();
            Bind<INotificacaoMensagemServico>().To<NotificacaoMensagemServico>();
            Bind<INotificacaoSmtpServico>().To<NotificacaoSmtpServico>();
            Bind<IAnaliseCriticaFuncionarioServico>().To<AnaliseCriticaFuncionarioServico>();
            Bind<IAnaliseCriticaTemaServico>().To<AnaliseCriticaTemaServico>();
            Bind<IDocCargoServico>().To<DocCargoServico>();
            Bind<IClienteServico>().To<ClienteServico>();
            Bind<INormaServico>().To<NormaServico>();
            Bind<ICriterioAceitacaoServico>().To<CriterioAceitacaoServico>();
            Bind<IProdutoServico>().To<ProdutoServico>();
            Bind<IAvaliacaoCriticidadeServico>().To<AvaliacaoCriticidadeServico>();
            Bind<ICalibracaoServico>().To<CalibracaoServico>();
            Bind<IInstrumentoServico>().To<InstrumentoServico>();
            Bind<IPlaiServico>().To<PlaiServico>();
            Bind<IPlaiProcessoNormaServico>().To<PlaiProcessoNormaServico>();
            Bind<IPaiServico>().To<PaiServico>();
            Bind<ICriterioQualificacaoServico>().To<CriterioQualificacaoServico>();
            Bind<IFornecedorServico>().To<FornecedorServico>();
            Bind<IProdutoFornecedorServico>().To<ProdutoFornecedorServico>();
            Bind<ICriterioAvaliacaoServico>().To<CriterioAvaliacaoServico>();
            Bind<IAvaliaCriterioAvaliacaoServico>().To<AvaliaCriterioAvaliacaoServico>();
            Bind<IHistoricoCriterioAvaliacaoServico>().To<HistoricoCriterioAvaliacaoServico>();
            Bind<ILogServico>().To<LogServico>();
            Bind<IControleImpressaoServico>().To<ControleImpressaoServico>();
            Bind<IIndicadorServico>().To<IndicadorServico>();
            Bind<ILoginServico>().To<LoginServico>();
            Bind<IDocumentoAssuntoServico>().To<DocumentoAssuntoServico>();
            Bind<IAnexoServico>().To<AnexoServico>();
            Bind<IRegistroAuditoriaServico>().To<RegistroAuditoriaServico>();
            Bind<IRegistroQualificacaoServico>().To<RegistroQualificacaoServico>();
            //RH
            Bind<IFuncionarioServico>().To<FuncionarioServico>();
            Bind<IUsuarioSenhaServico>().To<UsuarioSenhaServico>();
            Bind<IFilaEnvioServico>().To<FilaEnvioServico>();
            Bind<IRegistroAcaoImediataServico>().To<RegistroAcaoImediataServico>();
        }
    }
}
