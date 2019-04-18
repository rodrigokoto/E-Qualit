using Dominio.Entidade;
using Dominio.Validacao.AvaliacaoDeCriticidades;
using TestProject.Extentions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Interface.Repositorio;
using ApplicationService.Servico;
using System.Linq;
using Moq;

namespace TestProject.AvaliacoesCriticidade
{
    [TestClass]
    public class AvaliacaoCriticidadeTest
    {
        private AptoParaCadastrarAvaliacaoDeCriticidade _validaCampos = new AptoParaCadastrarAvaliacaoDeCriticidade();

        private AvaliacaoCriticidade _avaliacaoDeCriticidade;
        private Produto _produto;

        private Mock<IAvaliacaoCriticidadeRepositorio> _repositorio = new Mock<IAvaliacaoCriticidadeRepositorio>();
        private AvaliacaoCriticidadeAppServico _servico;

        private Mock<IProdutoRepositorio> _produtoRepositorio = new Mock<IProdutoRepositorio>();
        private ProdutoAppServico _produtoServico;

        [TestInitialize]
        public void Start()
        {

            var _produto = new Produto();
            _repositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_avaliacaoDeCriticidade.First());
            _produtoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_produto.First());

            _servico = new AvaliacaoCriticidadeAppServico(_repositorio.Object); ;
            _produtoServico = new ProdutoAppServico(_produtoRepositorio.Object);

        }


        [TestMethod]
        [TestCategory("AvaliacaoCriticidade - Cadastro")]
        public void Cadastra_AvaliacaoCriticidade_True()
        {
            _avaliacaoDeCriticidade = _avaliacaoDeCriticidade.Criar();
            _avaliacaoDeCriticidade.IdProduto = _produtoServico.GetById(1).IdProduto;

            var result = _validaCampos.Validate(_avaliacaoDeCriticidade);
         
            Assert.IsTrue(result.IsValid);

        }

        [TestMethod]
        [TestCategory("AvaliacaoCriticidade - Cadastro")]
        public void Cadastra_AvaliacaoCriticidade_False()
        {
            _avaliacaoDeCriticidade = _avaliacaoDeCriticidade.Criar();

            _avaliacaoDeCriticidade.Titulo = null;

            var result = _validaCampos.Validate(_avaliacaoDeCriticidade);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("AvaliacaoCriticidade - Update")]
        public void Update_AvaliacaoCriticidade_True()
        {
            _avaliacaoDeCriticidade = _servico.GetById(1);

            _avaliacaoDeCriticidade.Titulo = "Atualizado";
            

            var result = _validaCampos.Validate(_avaliacaoDeCriticidade);

            Assert.IsTrue(result.IsValid);

        }

        [TestMethod]
        [TestCategory("AvaliacaoCriticidade - Update")]
        public void Update_AvaliacaoCriticidade_False()
        {
            _avaliacaoDeCriticidade = _servico.GetById(It.IsAny<int>());

            _avaliacaoDeCriticidade.Titulo = null;

            var result = _validaCampos.Validate(_avaliacaoDeCriticidade);

            Assert.IsFalse(result.IsValid);
        }
    }
}
