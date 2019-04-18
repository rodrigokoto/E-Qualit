using Dominio.Entidade;
using Dominio.Validacao.Indicadores;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.Indicadores
{
    [TestClass]
    public class PlanoVooTest
    {
        private PlanoVoo _planoVoo = null;

        private void Construtor()
        {
            _planoVoo = new PlanoVoo();
            _planoVoo.Realizado = 10;

        }

        [TestMethod]
        [TestCategory("Indicador - Plano de Voo")]
        public void Plano_Voo__Validacao_True()
        {
            Construtor();

            var validacao = new PlanoDeVooAptoParaCadastroValidation().Validate(_planoVoo);

            Assert.IsTrue(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Indicador - Plano de Voo")]
        public void Plano_Voo__Validacao_False()
        {
            _planoVoo = new PlanoVoo();

            var validacao = new PlanoDeVooAptoParaCadastroValidation().Validate(_planoVoo);

            Assert.IsFalse(validacao.IsValid);
        }
    }
}
