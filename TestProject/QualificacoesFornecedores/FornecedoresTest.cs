using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;
using Dominio.Validacao.Fornecedores;
using Moq;
using Dominio.Interface.Repositorio;
using ApplicationService.Servico;
using ApplicationService.Interface;
using TestProject.Extentions;

namespace TestProject.QualificacoesFornecedores
{
    [TestClass]
    public class FornecedoresTest
    {

        private Fornecedor _fornecedor;
        private Processo _processo;
        private AptoParaCadastrarFornecedor _validaCampos;

        private Mock<IFornecedorRepositorio> _fornecedorRepositorio = new Mock<IFornecedorRepositorio>();
        private FornecedorAppServico _fornecedorServico;

        private Mock<IProcessoRepositorio> _processoRepositorio = new Mock<IProcessoRepositorio>();
        private ProcessoAppServico _processoServico;

        private Mock<IUsuarioCargoRepositorio> _usuarioCargoRepositorio = new Mock<IUsuarioCargoRepositorio>();
        private Mock<ICargoProcessoRepositorio> _cargoProcessoRepositorio = new Mock<ICargoProcessoRepositorio>();

        [TestInitialize]
        public void Start()
        {
            _validaCampos = new AptoParaCadastrarFornecedor();
            _processoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_processo.First());
            var log = new Mock<ILogAppServico>();

            _processoServico = new ProcessoAppServico(log.Object, _processoRepositorio.Object, _usuarioCargoRepositorio.Object, _cargoProcessoRepositorio.Object);

            _fornecedorRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_fornecedor.First());
            _fornecedorServico = new FornecedorAppServico(_fornecedorRepositorio.Object);


        }

        [TestMethod]
        [TestCategory("Fornecedor - Cadastro")]
        public void Cadastra_Fornecedor_True()
        {
            _fornecedor = _fornecedor.Criar();
            _fornecedor.IdProcesso = _processoServico.GetById(1).IdProcesso;

            var result = _validaCampos.Validate(_fornecedor);

            if (result.IsValid)
            {
                _fornecedorServico.Add(_fornecedor);
            }

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Fornecedor - Cadastro")]
        public void Cadastra_Fornecedor_False()
        {
            _fornecedor = _fornecedor.Criar();
            _fornecedor.Nome = null;
            _fornecedor.Telefone = null;

            var result = _validaCampos.Validate(_fornecedor);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Fornecedor - Update")]
        public void Update_Fornecedor_True()
        {

            _fornecedor = _fornecedorServico.GetById(1);

            _fornecedor.Nome = "Aciole";

            var result = _validaCampos.Validate(_fornecedor);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Fornecedor - Update")]
        public void Update_Fornecedor_False()
        {

            _fornecedor = _fornecedorServico.GetById(1);

            _fornecedor.Nome = "";

            var result = _validaCampos.Validate(_fornecedor);

            Assert.IsFalse(result.IsValid);
        }
    }
}
