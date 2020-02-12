using DAL.Repository;
using DAL.Repository.RH;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Repositorio.RH;
using Ninject.Modules;

namespace IoC
{
    public class ModuloRepositorio : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocDocumentoRepositorio>().To<DocDocumentoRepositorio>();
            Bind<IControladorCategoriasRepositorio>().To<ControladorCategoriasRepositorio>();
            Bind<ISiteModuloRepositorio>().To<SiteModuloRepositorio>();
            Bind<ISiteRepositorio>().To<SiteRepositorio>();
            Bind<IUsuarioClienteSiteRepositorio>().To<UsuarioClienteSiteRepositorio>();
            Bind<IUsuarioCargoRepositorio>().To<UsuarioCargoRepositorio>();
            Bind<ICargoRepositorio>().To<CargoRepositorio>();
            Bind<IRegistroConformidadesRepositorio>().To<RegistroConformidadesRepositorio>();
            Bind<IDocTemplateRepositorio>().To<DocTemplateRepositorio>();
            Bind<IAnaliseCriticaRepositorio>().To<AnaliseCriticaRepositorio>();
            Bind<IProcessoRepositorio>().To<ProcessoRepositorio>();
            Bind<IDocUsuarioVerificaAprovaRepositorio>().To<DocUsuarioVerificaAprovaRepositorio>();
            Bind<IUsuarioRepositorio>().To<UsuarioRepositorio>();
            Bind<ICargoProcessoRepositorio>().To<CargoProcessoRepositorio>();
            Bind<IAnaliseCriticaTemaRepositorio>().To<AnaliseCriticaTemaRepositorio>();
            Bind<IFuncionalidadeRepositorio>().To<FuncionalidadeRepositorio>();
            Bind<IRegistroAcaoImediataRepositorio>().To<RegistroAcaoImediataRepositorio>();
            Bind<INotificacaoRepositorio>().To<NotificacaoRepositorio>();
            Bind<INotificacaoMensagemRepositorio>().To<NotificacaoMensagemRepositorio>();
            Bind<INotificacaoSmtpRepositorio>().To<NotificacaoSmtpRepositorio>();
            Bind<IAnaliseCriticaFuncionarioRepositorio>().To<AnaliseCriticaFuncionarioRepositorio>();
            Bind<IDocCargoRepositorio>().To<DocCargoRepositorio>();
            Bind<IClienteRepositorio>().To<ClienteRepositorio>();
            Bind<INormaRepositorio>().To<NormaRepositorio>();
            Bind<ICriterioAceitacaoRepositorio>().To<CriterioAceitacaoRepositorio>();
            Bind<IInstrumentoRepositorio>().To<InstrumentoRepositorio>();
            Bind<ICalibracaoRepositorio>().To<CalibracaoRepositorio>();
            Bind<IPlaiRepositorio>().To<PlaiRepositorio>();
            Bind<IPlaiProcessoNormaRepositorio>().To<PlaiProcessoNormaRepositorio>();
            Bind<IPlaiGerentesRepositorio>().To<PlaiGerentesRepositorio>();
            Bind<IPaiRepositorio>().To<PaiRepositorio>();
            Bind<IProdutoRepositorio>().To<ProdutoRepositorio>();
            Bind<IAvaliacaoCriticidadeRepositorio>().To<AvaliacaoDeCriticidadeRepositorio>();
            Bind<ICriterioQualificacaoRepositorio>().To<CriterioQualificacaoRepositorio>();
            Bind<IAvaliaCriterioQualificacaoRepositorio>().To<AvaliaCriterioQualificacaoRepositorio>();
            Bind<IFornecedorRepositorio>().To<FornecedorRepositorio>();
            Bind<IProdutoFornecedorRepositorio>().To<ProdutoFornecedorRepositorio>();
            Bind<ICriterioAvaliacaoRepositorio>().To<CriterioAvaliacaoRepositorio>();
            Bind<IAvaliaCriterioAvaliacaoRepositorio>().To<AvaliaCriterioAvaliacaoRepositorio>();
            Bind<IHistoricoCriterioAvaliacaoRepositorio>().To<HistoricoCriterioAvaliacaoRepositorio>();
            Bind<ILogRepositorio>().To<LogRepositorio>();
            Bind<IControleImpressaoRepositorio>().To<ControleImpressaoRepositorio>();
            Bind<IIndicadorRepostorio>().To<IndicadorRepositorio>();
            Bind<ILoginRepositorio>().To<LoginRepositorio>();
            Bind<IDocumentoComentarioRepositorio>().To<DocumentoComentarioRepositorio>();
            Bind<IDocumentoAssuntoRepositorio>().To<DocumentoAssuntoRepositorio>();
            Bind<ISubModuloRepositorio>().To<SubModuloRepositorio>();
            Bind<IFuncaoRepositorio>().To<FuncaoRepositorio>();
            Bind<IAnexoRepositorio>().To<AnexoRepositorio>();
            Bind<IUsuarioAnexoRepositorio>().To<UsuarioAnexoRepositorio>();
            Bind<IClienteContratoRepositorio>().To<ClienteContratoRepositorio>();
            Bind<IArquivoDeEvidenciaAcaoImediataRepositorio>().To<ArquivoDeEvidenciaAcaoImediataRepositorio>();
            Bind<IArquivoDeEvidenciaRegistroConformidadeRepositorio>().To<ArquivoDeEvidenciaRegistroConformidadeRepositorio>();
            Bind<IArquivosEvidenciaCriterioQualificacaoRepositorio>().To<ArquivosEvidenciaCriterioQualificacaoRepositorio>();
            Bind<IRegistroAuditoria>().To<RegistroAuditoriaRepositorio>();
            Bind<IRegistroQualificacaoRepositorio>().To<RegistroQualificacaoRepositorio>();
            Bind<IArquivoLicencaAnexoRepositorio>().To<ArquivoLicencaAnexoRepositorio>();
            Bind<IRegistroLicencaRepositorio>().To<RegistroLicencaRepositorio>();


            Bind<IArquivoCertificadoAnexoRepositorio>().To<ArquivoCertificadoAnexoRepositorio>();
            Bind<IArquivoPlaiAnexoRepositorio>().To<ArquivoPlaiAnexoRepositorio>();
            Bind<IArquivoNaoConformidadeAnexoRepositorio>().To<ArquivoNaoConformidadeAnexoRepositorio>();
            Bind<IClienteLogoRepositorio>().To<ClienteLogoRepositorio>();
            Bind<ISiteAnexoRepositorio>().To<SiteAnexoRepositorio>();
            Bind<IFilaEnvioRepositorio>().To<FilaEnvioRepositorio>();
            Bind<ILicencaRepositorio>().To<LicencaRepositorio>();
            
            //RH
            Bind<IFuncionarioRepositorio>().To<FuncionarioRepositorio>();
            Bind<IUsuarioSenhaRepositorio>().To<UsuarioSenhaRepositorio>();
        }
    }
}

