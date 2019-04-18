using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;
using Dominio.Validacao.Pais;

namespace TestProject.Auditoria
{
    [TestClass]
    public class PaiTest
    {

        private Pai _pai;

        public void Construtor()
        {
            _pai = new Pai();

            _pai.Ano = 2017;
            _pai.IdGestor = 5;
            _pai.IdSite = 5;
        }
        [TestMethod]
        [TestCategory("Pai")]
        public void Plai_Validacao_True()
        {
            Construtor();

            var validacao = new AptoParaCadastroValidacao().Validate(_pai);

            Assert.IsTrue(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Pai")]
        public void Plai_Validacao_False()
        {
            _pai = new Pai();

            var validacao = new AptoParaCadastroValidacao().Validate(_pai);

            Assert.IsFalse(validacao.IsValid);
        }

        //[TestMethod]
        //[TestCategory("Pai")]
        //public void Insert()
        //{
        //    Construtor();
        //    var repositorio = new PaiRepositorio();

        //    repositorio.Add(_pai);
        //}

        //[TestMethod]
        //[TestCategory("Pai")]
        //public void Update()
        //{
        //    Construtor();
        //    _pai.IdPai = 1;
        //    _pai.IdGestor = 2;
        //    var repositorio = new PaiRepositorio();

        //    repositorio.Update(_pai);
        //}

        //[TestMethod]
        //[TestCategory("Pai")]
        //public void Delete()
        //{
        //    Construtor();
        //    _pai.IdPai = 1;
        //    var repositorio = new PaiRepositorio();

        //    repositorio.Remove(_pai);
        //}
    }
}
