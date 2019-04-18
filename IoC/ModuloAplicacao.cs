using ApplicationService.Interface;
using ApplicationService.Interface.RH;
using ApplicationService.Servico;
using ApplicationService.Servico.RH;
using Ninject.Modules;

namespace IoC
{
    public class ModuloAplicacao : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocDocumentoAppServico>().To<DocDocumentoAppServico>();
            Bind<IDocUsuarioVerificaAprovaAppServico>().To<DocUsuarioVerificaAprovaAppServico>();
            Bind<IDocTemplateAppServico>().To<DocTemplateAppServico>();
            Bind<ISiteModuloAppServico>().To<SiteModuloAppServico>();
            Bind<ISiteAppServico>().To<SiteAppServico>();
            Bind<IUsuarioClienteSiteAppServico>().To<UsuarioClienteSiteAppServico>();
            Bind<IUsuarioCargoAppServico>().To<UsuarioCargoAppServico>();
            Bind<ICargoAppServico>().To<CargoAppServico>();
            Bind<IControladorCategoriasAppServico>().To<ControladorCategoriasAppServico>();
            Bind<IRegistroConformidadesAppServico>().To<RegistroConformidadesAppServico>();
            Bind<IAnaliseCriticaAppServico>().To<AnaliseCriticaAppServico>();
            Bind<IProcessoAppServico>().To<ProcessoAppServico>();
            Bind<IUsuarioAppServico>().To<UsuarioAppServico>();
            Bind<INotificacaoAppServico>().To<NotificacaoAppServico>();
            Bind<INotificacaoMensagemAppServico>().To<NotificacaoMensagemAppServico>();
            Bind<INotificacaoSmtpAppServico>().To<NotificacaoSmtpAppServico>();
            Bind<IAnaliseCriticaFuncionarioAppServico>().To<AnaliseCriticaFuncionarioAppServico>();
            Bind<IAnaliseCriticaTemaAppServico>().To<AnaliseCriticaTemaAppServico>();
            Bind<IDocCargoAppServico>().To<DocCargoAppServico>();
            Bind<IClienteAppServico>().To<ClienteAppServico>();
            Bind<INormaAppServico>().To<NormaAppServico>();
            Bind<ICriterioAceitacaoAppServico>().To<CriterioAceitacaoAppServico>();
            Bind<IProdutoAppServico>().To<ProdutoAppServico>();
            Bind<IAvaliacaoCriticidadeAppServico>().To<AvaliacaoCriticidadeAppServico>();
            Bind<ICalibracaoAppServico>().To<CalibracaoAppServico>();
            Bind<IInstrumentoAppServico>().To<InstrumentoAppServico>();
            Bind<IPlaiAppServico>().To<PlaiAppServico>();
            Bind<IPlaiProcessoNormaAppServico>().To<PlaiProcessoNormaAppServico>();
            Bind<IPlaiGerentesAppServico>().To<PlaiGerentesAppServico>();
            Bind<IPaiAppServico>().To<PaiAppServico>();
            Bind<ICriterioQualificacaoAppServico>().To<CriterioQualificacaoAppServico>();
            Bind<IAvaliaCriterioQualificacaoAppServico>().To<AvaliaCriterioQualificacaoAppServico>();
            Bind<IFornecedorAppServico>().To<FornecedorAppServico>();
            Bind<IProdutoFornecedorAppServico>().To<ProdutoFornecedorAppServico>();
            Bind<ICriterioAvaliacaoAppServico>().To<CriterioAvaliacaoAppServico>();
            Bind<IAvaliaCriterioAvaliacaoAppServico>().To<AvaliaCriterioAvaliacaoAppServico>();
            Bind<IHistoricoCriterioAvaliacaoAppServico>().To<HistoricoCriterioAvaliacaoAppServico>();
            Bind<ILogAppServico>().To<LogAppServico>();
            Bind<IControleImpressaoAppServico>().To<ControleImpressaoAppServico>();
            Bind<IIndicadorAppServico>().To<IndicadorAppServico>();
            Bind<ILoginAppServico>().To<LoginAppServico>();
            Bind<IDocumentoAssuntoAppServico>().To<DocumentoAssuntoAppServico>();
            Bind<ISubModuloAppServico>().To<SubModuloAppServico>();
            Bind<IFuncaoAppServico>().To<FuncaoAppServico>();
            Bind<ICargoProcessoAppServico>().To<CargoProcessoAppServico>();
            Bind<IAnexoAppServico>().To<AnexoAppServico>();
            Bind<IUsuarioAnexoAppServico>().To<UsuarioAnexoAppServico>();
            Bind<IClienteContratoAppServico>().To<ClienteContratoAppServico>();
            Bind<IArquivoCertificadoAnexoAppServico>().To<ArquivoCertificadoAnexoAppServico>();
            Bind<IArquivosEvidenciaCriterioQualificacaoAppServico>().To<ArquivosEvidenciaCriterioQualificacaoAppServico>();
            Bind<IDocumentoComentarioAppServico>().To<DocumentoComentarioAppServico>();


            //RH
            Bind<IFuncionarioAppServico>().To<FuncionarioAppServico>();
            Bind<IFuncionalidadeAppServico>().To<FuncionalidadeAppServico>();
        }
    }
}
