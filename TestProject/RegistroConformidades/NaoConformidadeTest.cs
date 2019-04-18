using TestProject.Extentions;
using Dominio.Interface.Repositorio;
using ApplicationService.Servico;
using Dominio.Validacao.RegistroConformidades.NaoConformidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Dominio.Entidade;
using Dominio.Enumerado;
using System;
using Dominio.Validacao.Notificacoes;

namespace TestProject.RegistroConformidades
{
    [TestClass]
    public class NaoConformidadeTest
    {
        private RegistroConformidade _nc = null;

        private Mock<IRegistroConformidadesRepositorio> _registroConformidadesRepositorio;
        private Mock<IRegistroAcaoImediataRepositorio> _registroAcaoimediataRepositorio;
        private Mock<IUsuarioRepositorio> _usuarioRepositorio;

        private RegistroConformidadesAppServico _registroConformidadeServico;

        private CamposObrigatoriosNaoConformidadeIdentificacaoValidation _validaCamposIdentificacao;
        private AptoParaCadastroNotificacaoValidation _validaCamposNotificacao;
        private CamposObrigatoriosNaoConformidadeEProcedenteFalse _validaFluxoOndeEProcedenteEFalse;
        private CamposObrigatoriosNaoConformidadeEProcedenteTrue _validaFluxoOndeEProcedenteETrue;

        [TestInitialize]
        public void Start()
        {
            _validaCamposIdentificacao = new CamposObrigatoriosNaoConformidadeIdentificacaoValidation();
            _validaCamposNotificacao = new AptoParaCadastroNotificacaoValidation();
            _validaFluxoOndeEProcedenteEFalse = new CamposObrigatoriosNaoConformidadeEProcedenteFalse();
            _validaFluxoOndeEProcedenteETrue = new CamposObrigatoriosNaoConformidadeEProcedenteTrue();

            _registroConformidadesRepositorio = new Mock<IRegistroConformidadesRepositorio>();

            _registroConformidadesRepositorio.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(_nc.GetByIdNC());

            _registroAcaoimediataRepositorio = new Mock<IRegistroAcaoImediataRepositorio>();
            _usuarioRepositorio = new Mock<IUsuarioRepositorio>();
            _registroConformidadeServico = new RegistroConformidadesAppServico(_registroConformidadesRepositorio.Object,
                                                _registroAcaoimediataRepositorio.Object,
                                                null,
                                                _usuarioRepositorio.Object);
        }


        [TestMethod]
        [TestCategory("NaoConformidade - Indentificacao")]
        public void Cadastra_NaoConformidade_Indentificacao_True()
        {
            var nc = _nc.CriarNC();

            var result = _validaCamposIdentificacao.Validate(nc);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Indentificacao")]
        public void Cadastra_NaoConformidade_Indentificacao_False()
        {
            _nc = _nc.CriarNC();

            _nc.IdEmissor = 0;
            _nc.IdProcesso = 0;

            var result = _validaCamposIdentificacao.Validate(_nc);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Indentificacao com Notificacao")]
        public void Cadastra_NaoConformidade_Indentificacao_Com_Notificacao_True()
        {

            var nc = _nc.CriarNC();

            var resultNC = _validaCamposIdentificacao.Validate(nc);

            var notificacao = new Notificacao($"Não Conformidade #{nc.NuRegistro}",
                                null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                nc.IdProcesso, 108,
                                nc.IdSite, 0,
                                ((char)TipoNotificacao.Leitura).ToString(),
                                1);

            var resultNotificacao = _validaCamposNotificacao.Validate(notificacao);

            Assert.IsTrue((resultNC.IsValid && resultNotificacao.IsValid));
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Indentificacao com Notificacao")]
        public void Cadastra_NaoConformidade_Indentificacao_Com_Notificacao_False()
        {
            _nc = _nc.CriarNC();

            var resultNC = _validaCamposIdentificacao.Validate(_nc);

            var notificacao = new Notificacao($"Não Conformidade #1",
                                null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                1, 108,
                                1, 0,
                                ((char)TipoNotificacao.Leitura).ToString(),
                                1);

            notificacao.IdProcesso = 0;
            notificacao.IdSite = 0;

            var resultNotificacao = _validaCamposNotificacao.Validate(notificacao);

            Assert.IsFalse((resultNC.IsValid && resultNotificacao.IsValid));
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.001]")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediataIgualZero_True()
        {
            _nc = _registroConformidadeServico.GetById(1);
            _nc.EProcedente = false;
            _nc.DescricaoAcao = "Justificativa teste";
            _nc.DtDescricaoAcao = DateTime.Now;
            _nc.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;

            var result = _validaFluxoOndeEProcedenteEFalse.Validate(_nc);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.001]")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediata_IgualZero_False()
        {
            _nc = _registroConformidadeServico.GetById(1);
            _nc.EProcedente = true;
            _nc.DescricaoAcao = "Ju";
            _nc.DtDescricaoAcao = DateTime.Now;
            _nc.StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata;

            var result = _validaFluxoOndeEProcedenteEFalse.Validate(_nc);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.001] com Notificacao")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediataIgualZero_Com_Notificacao_True()
        {
            _nc = _registroConformidadeServico.GetById(1);
            _nc.EProcedente = false;
            _nc.DescricaoAcao = "Justificativa teste";
            _nc.DtDescricaoAcao = DateTime.Now;
            _nc.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;

            var resultNC = _validaFluxoOndeEProcedenteEFalse.Validate(_nc);

            var notificacao = new Notificacao($"Não Conformidade #{_nc.NuRegistro}",
                                null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                _nc.IdProcesso, 108,
                                _nc.IdSite, 0,
                                ((char)TipoNotificacao.Leitura).ToString(),
                                1);

            var resultNotificacao = _validaCamposNotificacao.Validate(notificacao);

            Assert.IsTrue((resultNC.IsValid && resultNotificacao.IsValid));
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.001] com Notificacao")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediata_IgualZero_Com_Notificacao_False()
        {
            _nc = _registroConformidadeServico.GetById(1);
            _nc.EProcedente = true;
            _nc.DescricaoAcao = "Ju"; //erro
            _nc.DtDescricaoAcao = DateTime.Now;
            _nc.StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata;

            var resultNC = _validaFluxoOndeEProcedenteEFalse.Validate(_nc);

            var notificacao = new Notificacao($"Não Conformidade #{_nc.NuRegistro}",
                                null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                _nc.IdProcesso, 108,
                                _nc.IdSite, 0,
                                ((char)TipoNotificacao.Leitura).ToString(),
                                1);

            var resultNotificacao = _validaCamposNotificacao.Validate(notificacao);

            Assert.IsFalse((resultNC.IsValid && resultNotificacao.IsValid));
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.002]")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediata_MaiorQueZero_NecessitaAcaoCorretivaFalse_True()
        {
            var nc = _registroConformidadeServico.GetById(1);
            nc.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
            nc.DtDescricaoAcao = DateTime.Now;
            nc.EProcedente = true;
            nc.AcoesImediatas.Add(new RegistroAcaoImediata {
                Aprovado = null,
                Descricao = "Descricao Acao Imediata",
                DtInclusao = DateTime.Now,
                DtPrazoImplementacao = DateTime.Now.AddDays(10),
                Estado = EstadoObjetoEF.Added,
                IdRegistroConformidade = nc.IdRegistroConformidade,
                IdResponsavelImplementar = 1,
                IdUsuarioIncluiu = 1
            });

            nc.ECorrecao = false;
            nc.IdResponsavelReverificador = 0;

            nc.NecessitaAcaoCorretiva = false;
            nc.DescricaoAnaliseCausa = "";
            nc.IdResponsavelPorIniciarTratativaAcaoCorretiva = 0;

            var result = _validaFluxoOndeEProcedenteETrue.Validate(nc);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.002]")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediata_MaiorQueZero_NecessitaAcaoCorretivaFalse_False()
        {
            var nc = _registroConformidadeServico.GetById(1);
            nc.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
            nc.DtDescricaoAcao = DateTime.Now;
            nc.EProcedente = true;
            nc.AcoesImediatas.Add(new RegistroAcaoImediata
            {
                Aprovado = null,
                Descricao = "Descricao Acao Imediata",
                DtInclusao = DateTime.Now,
                DtPrazoImplementacao = DateTime.Now.AddDays(10),
                Estado = EstadoObjetoEF.Added,
                IdRegistroConformidade = nc.IdRegistroConformidade,
                IdResponsavelImplementar = 1,
                IdUsuarioIncluiu = 1
            });

            nc.ECorrecao = true; //Erro
            nc.IdResponsavelReverificador = 0;

            nc.NecessitaAcaoCorretiva = false;
            nc.DescricaoAnaliseCausa = "";
            nc.IdResponsavelPorIniciarTratativaAcaoCorretiva = 0;

            var result = _validaFluxoOndeEProcedenteETrue.Validate(nc);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.002] com Notificacao")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediata_MaiorQueZero_NecessitaAcaoCorretivaFalse_Com_Notificacao_True()
        {
            var nc = _registroConformidadeServico.GetById(1);
            nc.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
            nc.DtDescricaoAcao = DateTime.Now;
            nc.EProcedente = true;
            nc.AcoesImediatas.Add(new RegistroAcaoImediata
            {
                Aprovado = null,
                Descricao = "Descricao Acao Imediata",
                DtInclusao = DateTime.Now,
                DtPrazoImplementacao = DateTime.Now.AddDays(10),
                Estado = EstadoObjetoEF.Added,
                IdRegistroConformidade = nc.IdRegistroConformidade,
                IdResponsavelImplementar = 1,
                IdUsuarioIncluiu = 1
            });

            nc.ECorrecao = false;
            nc.IdResponsavelReverificador = 0;

            nc.NecessitaAcaoCorretiva = false;
            nc.DescricaoAnaliseCausa = "";
            nc.IdResponsavelPorIniciarTratativaAcaoCorretiva = 0;

            var resultNC = _validaFluxoOndeEProcedenteETrue.Validate(nc);

            var notificacao = new Notificacao($"Não Conformidade #{nc.NuRegistro}",
                                null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                nc.IdProcesso, 108,
                                nc.IdSite, 0,
                                ((char)TipoNotificacao.Leitura).ToString(),
                                1);

            var resultNotificacao = _validaCamposNotificacao.Validate(notificacao);

            Assert.IsTrue((resultNC.IsValid && resultNotificacao.IsValid));
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.002] com Notificacao")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediata_MaiorQueZero_NecessitaAcaoCorretivaFalse_Com_Notificacao_False()
        {
            var nc = _registroConformidadeServico.GetById(1);
            nc.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
            nc.DtDescricaoAcao = DateTime.Now;
            nc.EProcedente = true;
            nc.AcoesImediatas.Add(new RegistroAcaoImediata
            {
                Aprovado = null,
                Descricao = "Descricao Acao Imediata",
                DtInclusao = DateTime.Now,
                DtPrazoImplementacao = DateTime.Now.AddDays(10),
                Estado = EstadoObjetoEF.Added,
                IdRegistroConformidade = nc.IdRegistroConformidade,
                IdResponsavelImplementar = 1,
                IdUsuarioIncluiu = 1
            });

            nc.ECorrecao = true; //Erro
            nc.IdResponsavelReverificador = 0;

            nc.NecessitaAcaoCorretiva = false;
            nc.DescricaoAnaliseCausa = "";
            nc.IdResponsavelPorIniciarTratativaAcaoCorretiva = 0;

            var resultNC = _validaFluxoOndeEProcedenteETrue.Validate(nc);

            var notificacao = new Notificacao($"Não Conformidade #{nc.NuRegistro}",
                                null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                nc.IdProcesso, 108,
                                nc.IdSite, 0,
                                ((char)TipoNotificacao.Leitura).ToString(),
                                1);

            var resultNotificacao = _validaCamposNotificacao.Validate(notificacao);

            Assert.IsFalse((resultNC.IsValid && resultNotificacao.IsValid));
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.002]")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediata_MaiorQueZero_NecessitaAcaoCorretivaTrue_True()
        {
            var nc = _registroConformidadeServico.GetById(1);
            nc.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
            nc.DtDescricaoAcao = DateTime.Now;
            nc.EProcedente = true;
            nc.AcoesImediatas.Add(new RegistroAcaoImediata
            {
                Aprovado = null,
                Descricao = "Descricao Acao Imediata",
                DtInclusao = DateTime.Now,
                DtPrazoImplementacao = DateTime.Now.AddDays(10),
                Estado = EstadoObjetoEF.Added,
                IdRegistroConformidade = nc.IdRegistroConformidade,
                IdResponsavelImplementar = 1,
                IdUsuarioIncluiu = 1
            });

            nc.ECorrecao = false;
            nc.IdResponsavelReverificador = 0;

            nc.NecessitaAcaoCorretiva = true;
            nc.DescricaoAnaliseCausa = "Descrição para Acao Corretiva";
            nc.IdResponsavelPorIniciarTratativaAcaoCorretiva = 1;

            var result = _validaFluxoOndeEProcedenteETrue.Validate(nc);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("NaoConformidade - Implementacao [Fluxo.002]")]
        public void Atualiza_NaoConformidade_Implementacao_AcaoImediata_MaiorQueZero_NecessitaAcaoCorretivaTrue_False()
        {
            var nc = _registroConformidadeServico.GetById(1);
            nc.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
            nc.DtDescricaoAcao = DateTime.Now;
            nc.EProcedente = true;
            nc.AcoesImediatas.Add(new RegistroAcaoImediata
            {
                Aprovado = null,
                Descricao = "Descricao Acao Imediata",
                DtInclusao = DateTime.Now,
                DtPrazoImplementacao = DateTime.Now.AddDays(10),
                Estado = EstadoObjetoEF.Added,
                IdRegistroConformidade = nc.IdRegistroConformidade,
                IdResponsavelImplementar = 1,
                IdUsuarioIncluiu = 1
            });

            nc.ECorrecao = false;
            nc.IdResponsavelReverificador = 0;

            nc.NecessitaAcaoCorretiva = true;
            nc.DescricaoAnaliseCausa = "Descrição para Acao Corretiva";
            nc.IdResponsavelPorIniciarTratativaAcaoCorretiva = 0; //erro

            var result = _validaFluxoOndeEProcedenteETrue.Validate(nc);

            Assert.IsFalse(result.IsValid);
        }

    }

}
