using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Servico;
using Dominio.Validacao.RegistroConformidades.GestaoDeRiscos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestProject
{
    [TestClass]
    public class TemaTest
    {
        private AptoParaCadastroComGestaoDeRiscoValidacao validaComConformidade = new AptoParaCadastroComGestaoDeRiscoValidacao();
        private AnaliseCriticaTema _tema = new AnaliseCriticaTema
        {
            Descricao = "Teste de criação",
            //CorRisco = "Verde",
            PossuiGestaoRisco = true,
        };


        [TestMethod]
        [TestCategory("Tema")]
        public void Possui_Gestao_De_Risco_Validate_False()
        {
            _tema.GestaoDeRisco = null;
            var result = new AptoParaCadastroComGestaoDeRiscoValidacao().Validate(_tema.GestaoDeRisco);

            Assert.IsFalse(result.IsValid);
        }

        //[TestMethod]
        //[TestCategory("Tema")]
        //public void Possui_Gestao_De_Risco_Cor_Validate_True()
        //{
        //    var result = new AptoParaCadastroComGestaoDeRiscoValidacao().Validate(_tema.GestaoDeRisco);

        //    Assert.IsTrue(result.IsValid);
        //}

        [TestMethod]
        [TestCategory("Tema")]
        public void Nao_Possui_Gestao_De_Risco_Cor_Vermelha_Validate_False()
        {

            _tema.PossuiGestaoRisco = false;
            //_tema.CorRisco = "Vermelho";

            var result = GestaoDeRiscoServico.PossuiGestaoDeRisco(_tema.GestaoDeRisco.CriticidadeGestaoDeRisco, _tema.PossuiGestaoRisco);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Tema")]
        public void Nao_Possui_Gestao_De_Risco_Cor_Verde_Validate_False()
        {
            var docDocumentoRepositorio = new Mock<IAnaliseCriticaTemaRepositorio>();
            var logRepositorio = new Mock<ILogRepositorio>();
            var temaServico = new AnaliseCriticaTemaAppServico(docDocumentoRepositorio.Object, logRepositorio.Object);

            _tema.PossuiGestaoRisco = false;
            _tema.CorRisco = "Verde";
            var result = GestaoDeRiscoServico.PossuiGestaoDeRisco(_tema.GestaoDeRisco.CriticidadeGestaoDeRisco, _tema.PossuiGestaoRisco);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Tema")]
        public void Nao_Possui_Gestao_De_Risco_Cor_Amarelo_Validate_False()
        {
            var docDocumentoRepositorio = new Mock<IAnaliseCriticaTemaRepositorio>();
            var logRepositorio = new Mock<ILogRepositorio>();
            var temaServico = new AnaliseCriticaTemaAppServico(docDocumentoRepositorio.Object, logRepositorio.Object);

            _tema.PossuiGestaoRisco = false;
            _tema.CorRisco = "Amarelo";
            var result = GestaoDeRiscoServico.PossuiGestaoDeRisco(_tema.GestaoDeRisco.CriticidadeGestaoDeRisco, _tema.PossuiGestaoRisco);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Tema")]
        public void Possui_Gestao_De_Risco_Cor_Vermelho_Validate_True()
        {
            var docDocumentoRepositorio = new Mock<IAnaliseCriticaTemaRepositorio>();
            var logRepositorio = new Mock<ILogRepositorio>();
            var temaServico = new AnaliseCriticaTemaAppServico(docDocumentoRepositorio.Object, logRepositorio.Object);

            _tema.PossuiGestaoRisco = true;
            _tema.CorRisco = "Vermelho";
            var result = GestaoDeRiscoServico.PossuiGestaoDeRisco(_tema.CorRisco, _tema.PossuiGestaoRisco);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Tema")]
        public void Possui_Gestao_De_Risco_Cor_Amarelo_Validate_True()
        {
            var docDocumentoRepositorio = new Mock<IAnaliseCriticaTemaRepositorio>();
            var logRepositorio = new Mock<ILogRepositorio>();
            var temaServico = new AnaliseCriticaTemaAppServico(docDocumentoRepositorio.Object, logRepositorio.Object);

            _tema.PossuiGestaoRisco = true;
            _tema.CorRisco = "Amarelo";

            var result = GestaoDeRiscoServico.PossuiGestaoDeRisco(_tema.CorRisco, _tema.PossuiGestaoRisco);

            Assert.IsTrue(result);
        }

        //[TestMethod]
        //public void Teste()
        //{
        //    var temaRepositorio = new AnaliseCriticaTemaRepositorio();

        //    var _tema = new AnaliseCriticaTema
        //    {
        //        IdAnaliseCritica = 1,
        //        IdUsuario = 2,
        //        IdControladorCategoria = 4,
        //        Descricao = "Teste de criação",
        //        CorRisco = "Verde",
        //        PossuiGestaoRisco = false,
        //        Ativo = 1,
        //    };
        //    temaRepositorio.Add(_tema);
        //}

        //[TestMethod]
        //public void Teste2()
        //{
        //    var analiseCritivaRepositorio = new AnaliseCriticaRepositorio();

        //    var analiseCritica = new AnaliseCritica();

        //    analiseCritica.Ata = 123456;
        //    analiseCritica.DataCadastro = DateTime.Now;
        //    analiseCritica.DataCriacao = DateTime.Now;
        //    analiseCritica.DataProximaAnalise = DateTime.Now;
        //    analiseCritica.IdSite = 1;


        //    analiseCritica.Temas = new List<AnaliseCriticaTema>();

        //    analiseCritica.Temas.Add(new AnaliseCriticaTema
        //    {
        //        IdAnaliseCritica = 1,
        //        IdUsuario = 2,
        //        IdControladorCategoria = 4,
        //        Descricao = "Teste de criação",
        //        CorRisco = "Verde",
        //        PossuiGestaoRisco = false,
        //        Ativo = 1,
        //    });

        //    analiseCritivaRepositorio.Add(analiseCritica);
        //}
    }
}
