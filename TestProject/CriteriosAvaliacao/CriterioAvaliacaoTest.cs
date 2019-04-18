using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApplicationService.Servico;
using Dominio.Interface.Repositorio;
using TestProject.Extentions;
using Dominio.Entidade;
using System.Linq;
using Moq;
using Dominio.Validacao.CriteriosAvaliacao;

namespace TestProject.CriteriosAvaliacao
{
    [TestClass]
    public class CriterioAvaliacaoTest
    {
        private AptoParaCadastraCriterioAvaliacao _validaCampos;

        private Produto _produto;

        private Mock<ICriterioAvaliacaoRepositorio > _qualificaAvaliacaoCriticidadeRepositorio = new Mock<ICriterioAvaliacaoRepositorio>();
        private Mock<IProdutoRepositorio> _produtoRepositorio= new Mock<IProdutoRepositorio>();


        private CriterioAvaliacaoAppServico _qualificaAvaliacaiCriticidadeServico;
        private ProdutoAppServico _produtoServico;
        private CriterioAvaliacao _qualificaAvaliacaoCriticidade;

        [TestInitialize]
        public void Start()
        {
            _validaCampos = new AptoParaCadastraCriterioAvaliacao();
            _qualificaAvaliacaoCriticidadeRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_qualificaAvaliacaoCriticidade.First());
            _produtoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_produto.First());

            _produtoServico = new ProdutoAppServico(_produtoRepositorio.Object);
            _qualificaAvaliacaiCriticidadeServico = new CriterioAvaliacaoAppServico(_qualificaAvaliacaoCriticidadeRepositorio.Object);
        }

        [TestMethod]
        [TestCategory("CriterioAvaliacao - Cadastra")]
        public void Cadastra_QualificaAvaliacaoCriticidade_true()
        {
            var qualificaAvaliacaiCriticidade = new CriterioAvaliacao
            {
                Titulo = "Disponibilidade",
                IdProduto = _produtoServico.GetById(1).IdProduto
            };

            var result = _validaCampos.Validate(qualificaAvaliacaiCriticidade);

            Assert.IsTrue(result.IsValid);

        }

        [TestMethod]
        [TestCategory("CriterioAvaliacao - Cadastra")]
        public void Cadastra_QualificaAvaliacaoCriticidade_False()
        {
            var qualificaAvaliacaiCriticidade = new CriterioAvaliacao
            {
                Titulo = null,
                IdProduto = _produtoServico.GetById(1).IdProduto
            };

            var result = _validaCampos.Validate(qualificaAvaliacaiCriticidade);

            Assert.IsFalse(result.IsValid);

        }

        [TestMethod]
        [TestCategory("CriterioAvaliacao - Update")]
        public void Update_QualificaAvaliacaoCriticidade_true()
        {
            var qualificaAvaliacaiCriticidade = new CriterioAvaliacao
            {
                Titulo = "Agilidade",
                IdProduto = _produtoServico.GetById(1).IdProduto
            };

            var result = _validaCampos.Validate(qualificaAvaliacaiCriticidade);

            Assert.IsTrue(result.IsValid);

        }

        [TestMethod]
        [TestCategory("CriterioAvaliacao - Update")]
        public void Update_QualificaAvaliacaoCriticidade_False()
        {
            var qualificaAvaliacaiCriticidade = new CriterioAvaliacao
            {
                Titulo = null,
                IdProduto = _produtoServico.GetById(1).IdProduto
            };

            var result = _validaCampos.Validate(qualificaAvaliacaiCriticidade);

            Assert.IsFalse(result.IsValid);

        }
    }
}
