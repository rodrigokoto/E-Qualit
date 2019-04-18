using TestProject.Extentions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Servico;
using Dominio.Validacao.CriteriosQualificacao;
using System.Linq;
using Moq;
using System;

namespace TestProject.CriteriosQualificacao
{
    [TestClass]
    public class CriterioQualificacaoTest
    {
        private AptoParaCadastroCriterioQualificacao _validaCamposCadastro = new AptoParaCadastroCriterioQualificacao();
        private AptoParaQualificarCriterioQualificacao _validaCamposQualificar = new AptoParaQualificarCriterioQualificacao();

        private CriterioQualificacao _criterioQualificacao;

        private Mock<ICriterioQualificacaoRepositorio> _criterioQualificacaoRepositorio = new Mock<ICriterioQualificacaoRepositorio>();
        private CriterioQualificacaoAppServico _criterioQualificacaoServico;

        [TestInitialize]
        public void Start()
        {
            _criterioQualificacaoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_criterioQualificacao.First());
            _criterioQualificacaoServico = new CriterioQualificacaoAppServico(_criterioQualificacaoRepositorio.Object);
        }

        [TestMethod]
        [TestCategory("CriterioQualificacao - Cadastro")]
        public void Cadastra_CriterioQualificacao_True()
        {
            _criterioQualificacao = _criterioQualificacao.Criar();

            var result = _validaCamposCadastro.Validate(_criterioQualificacao);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("CriterioQualificacao - Cadastro")]
        public void Cadastra_CriterioQualificacao_False()
        {
            _criterioQualificacao = _criterioQualificacao.Criar();
            _criterioQualificacao.Titulo = null;
            _criterioQualificacao.TemControleVencimento = true;
            _criterioQualificacao.DtVencimento = null;

            var result = _validaCamposCadastro.Validate(_criterioQualificacao);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("CriterioQualificacao - Update")]
        public void Update_CriterioQualificacao_True()
        {
            _criterioQualificacao = _criterioQualificacaoServico.GetById(1);

            var result = _validaCamposCadastro.Validate(_criterioQualificacao);

            if (result.IsValid)
            {
                _criterioQualificacaoServico.Add(_criterioQualificacao);
                Assert.IsTrue(result.IsValid);
            }
            else
            {
                Assert.IsTrue(false);

            }
        }

        [TestMethod]
        [TestCategory("CriterioQualificacao - Update")]
        public void Update_CriterioQualificacao_False()
        {
            _criterioQualificacao = _criterioQualificacaoServico.GetById(1);
            _criterioQualificacao.Titulo = null;
            _criterioQualificacao.TemControleVencimento = true;
            _criterioQualificacao.DtVencimento = null;

            var result = _validaCamposCadastro.Validate(_criterioQualificacao);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("CriterioQualificacao - Qualifica")]
        public void Qualifica_CriterioQualificacao_True()
        {
            _criterioQualificacao = _criterioQualificacaoServico.GetById(1);
            _criterioQualificacao.ArquivoEvidencia = "DocumentoComprovação.pdf";
            _criterioQualificacao.Aprovado = true;
            _criterioQualificacao.Observacoes = "Muito bem observado";
            _criterioQualificacao.IdResponsavelPorControlarVencimento = 1;
            _criterioQualificacao.IdResponsavelPorQualificar = 150;
            _criterioQualificacao.DtEmissao = DateTime.Now;
            _criterioQualificacao.DtAlteracaoEmissao = DateTime.Now;
            _criterioQualificacao.DtQualificacaoVencimento = DateTime.Now;
            _criterioQualificacao.NumeroDocumento = "Doc2017";
            _criterioQualificacao.OrgaoExpedidor = "SSP";
            _criterioQualificacao.ObservacoesDocumento = "Documento Muito Bem Elaborado";

            var result = _validaCamposQualificar.Validate(_criterioQualificacao);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("CriterioQualificacao - Qualifica")]
        public void Qualifica_CriterioQualificacao_False()
        {
            _criterioQualificacao = _criterioQualificacaoServico.GetById(1);
            _criterioQualificacao.ArquivoEvidencia = null;
            _criterioQualificacao.Aprovado = true;
            _criterioQualificacao.Observacoes = "Muito bem observado";
            _criterioQualificacao.IdResponsavelPorControlarVencimento = 1;
            _criterioQualificacao.IdResponsavelPorQualificar = 150;
            _criterioQualificacao.DtEmissao = DateTime.Now;
            _criterioQualificacao.DtAlteracaoEmissao = DateTime.Now;
            _criterioQualificacao.DtQualificacaoVencimento = DateTime.Now;
            _criterioQualificacao.NumeroDocumento = null;
            _criterioQualificacao.OrgaoExpedidor = "SSP";
            _criterioQualificacao.ObservacoesDocumento = "Documento Muito Bem Elaborado";

            var result = _validaCamposQualificar.Validate(_criterioQualificacao);

            Assert.IsFalse(result.IsValid);
        }
    }
}
