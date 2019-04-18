using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;
using Dominio.Validacao.Produtos;
using TestProject.Extentions;
using Dominio.Interface.Repositorio;
using ApplicationService.Servico;
using System.Linq;
using Dominio.Validacao.AvaliacaoDeCriticidades;
using Moq;

namespace TestProject.Produtos
{
    [TestClass]
    public class ProdutoTest
    {
        private AptoParaCadastrarProduto _validaCamposProduto;
        private AptoParaCadastrarAvaliacaoDeCriticidade _validaCamposAvaliacaoCriticidade;

        private Produto _prod;
        private AvaliacaoCriticidade _avaliacoesCriticidade;
        private CriterioQualificacao _criterioQualificacao;
        private Fornecedor _fornecedor;
        private CriterioAvaliacao _qualificaAvaliacaoCriticidade;

        private Mock<IProdutoRepositorio> _produtoRepositorio = new Mock<IProdutoRepositorio>();
        private Mock<IUsuarioRepositorio> _usuarioRepositorio = new Mock<IUsuarioRepositorio>();

        private ProdutoAppServico _produtoServico;
        private AvaliacaoCriticidadeAppServico _avaliacaoCriticidadeServico;
        private CriterioQualificacaoAppServico _criterioQualificacaoServico;
        private FornecedorAppServico _fornecedorServico;

        private SiteAppServico _siteServico;
        private CriterioAvaliacaoAppServico _qualificaAvaliacaoCriticidadeServico;

        private UsuarioAppServico _usuarioServico;

        [TestInitialize]
        public void Start()
        {
            _validaCamposProduto = new AptoParaCadastrarProduto();

            _produtoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_prod.First());
            _usuarioRepositorio.Setup(x => x.GetById(It.IsAny<int>()));
            _produtoServico = new ProdutoAppServico(_produtoRepositorio.Object);

        }

        [TestMethod]
        [TestCategory("Produto - Cadastro")]
        public void Cadastra_Produto_False()
        {
            _prod = _prod.CriaProdutoValido();
            _prod.Especificacao = null;
            _prod.MinReprovado = 0;

            var result = _validaCamposProduto.Validate(_prod);

            Assert.IsFalse(result.IsValid);
        }

        //[TestMethod]
        //[TestCategory("Produto - Update")]
        //public void Update_Produto_True()
        //{
        //    _prod = _produtoServico.GetById(1);

        //    var result = _validaCamposProduto.Validate(_prod);

        //    Assert.IsTrue(result.IsValid);
        //}

        [TestMethod]
        [TestCategory("Produto - Update")]
        public void Update_Produto_False()
        {
            _prod = _produtoServico.GetById(1);
            _prod.Especificacao = null;
            _prod.MinReprovado = 0;

            var result = _validaCamposProduto.Validate(_prod);

            Assert.IsFalse(result.IsValid);
        }

    }
}
