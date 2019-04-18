using Dominio.Entidade;
using Dominio.Validacao.Indicadores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.Indicadores
{
    [TestClass]
    public class PeriodicidadeAnaliseTest
    {
        private PeriodicidaDeAnalise _periodicidadeAnalise = null;

        private void Construtor(int valorMeta, int valorRealizado)
        {
            _periodicidadeAnalise = new PeriodicidaDeAnalise();

            _periodicidadeAnalise.Metas = new List<Meta>();
            _periodicidadeAnalise.Metas.FirstOrDefault().Valor = valorMeta;

            _periodicidadeAnalise.PlanosDeVoo = new List<PlanoVoo>();
            _periodicidadeAnalise.PlanosDeVoo.FirstOrDefault().Realizado = valorRealizado;
        }


        [TestMethod]
        [TestCategory("Indicador - Periodicidade Analise")]
        public void AtingiuMetaValidation_Valor_Meta_Menor_Realizado_True()
        {
            Construtor(100 , 150);

            var result = new AtingiuMetaValidation().Validate(_periodicidadeAnalise);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Indicador - Periodicidade Analise")]
        public void AtingiuMetaValidation_Valor_Meta_Maior_Realizado_False()
        {
            Construtor(100, 50);

            var result = new AtingiuMetaValidation().Validate(_periodicidadeAnalise);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Indicador - Periodicidade Analise")]
        public void AtingiuMetaValidation_Com_Justificativa_True()
        {
            Construtor(100, 50);
            _periodicidadeAnalise.Justificativa = "Teste";

            var result = new AtingiuMetaValidation().Validate(_periodicidadeAnalise);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Indicador - Periodicidade Analise")]
        public void AtingiuMetaValidation_Sem_Justificativa_False()
        {
            Construtor(100, 50);

            var result = new AtingiuMetaValidation().Validate(_periodicidadeAnalise);

            Assert.IsFalse(result.IsValid);
        }
    }
}
