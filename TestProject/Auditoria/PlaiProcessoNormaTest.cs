using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;

namespace TestProject.Auditoria
{
    [TestClass]
    public class PlaiProcessoNormaTest
    {

        private PlaiProcessoNorma _plaiProcessoNorma;

        public void Construtor()
        {
            _plaiProcessoNorma = new PlaiProcessoNorma();

            _plaiProcessoNorma.IdProcesso = 5;
            _plaiProcessoNorma.IdPlai = 8;
            _plaiProcessoNorma.IdNorma = 3;

        }

        //[TestMethod]
        //public void ObterTodos()
        //{
        //    Construtor();
        //    var plaiProcessoNormas = new List<PlaiProcessoNorma>();
        //    plaiProcessoNormas.Add(_plaiProcessoNorma);
 
        //    var respositorio = new Mock<IPlaiProcessoNormaRepositorio>();
            
        //    respositorio.Setup(x => x.Get(f => f.IdPlai == It.IsAny<int>(), null, null)).Returns(plaiProcessoNormas);

        //    var servico = new PlaiProcessoNormaServico(respositorio.Object);

        //    var plaiProcessosNormas = servico.ObterPorIdPlai(It.IsAny<int>());

        //    var totalCheck = plaiProcessosNormas.Where(x => x.Ativo == true);

        //    Assert.AreEqual(totalCheck, 1);

        //}
    }
}
