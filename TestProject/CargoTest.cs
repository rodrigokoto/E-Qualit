using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Interface.Repositorio;
using Moq;
using Dominio.Entidade;
using System.Collections.Generic;
using Dominio.Servico;

namespace TestProject
{
    [TestClass]
    public class CargoTest
    {
        private Cargo _cargo;
        private List<SiteFuncionalidade> _siteModulos;

        private Mock<ICargoRepositorio> _cargoRepositorio;
        private Mock<ISiteModuloRepositorio> _siteRepositorio;
        private Mock<IFuncionalidadeRepositorio> _moduloRepositorio;
        private Mock<ICargoProcessoRepositorio> _cargoProcessoRepositorio;


        private void InicializadorCargo()
        {
            _cargo = new Cargo
            {
                IdCargo = 1
            };
        }

        private void InicializadorSite()
        {
            _siteModulos = new List<SiteFuncionalidade>();
            _siteModulos.Add(new SiteFuncionalidade
            {
                IdFuncionalidade = 1,
                Funcionalidade = new Funcionalidade
                {
                    IdFuncionalidade = 1,
                    Nome = "Teste"
                }
            });
        }

        [TestMethod]
        [TestCategory("Cargo")]
        public void Obtem_Modulos_Permitidos_Retorna_Um_Site_Modulo()
        {
            InicializadorCargo();
            InicializadorSite();

            _cargoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_cargo);
            _siteRepositorio.Setup(x => x.ListarSiteModuloPorSite(It.IsAny<int>())).Returns(_siteModulos);

            var docServico = new CargoServico(_cargoRepositorio.Object, 
                                              _siteRepositorio.Object, 
                                              _moduloRepositorio.Object, _cargoProcessoRepositorio.Object);

            var retorno = docServico.ObtemModulosPermitidos(It.IsAny<int>());

            Assert.AreEqual(retorno.Count, 1);
        }

        [TestMethod]
        [TestCategory("Cargo")]
        public void Obtem_Modulos_Permitidos_Cargo_Vazio()
        {
            _cargoRepositorio = new Mock<ICargoRepositorio>();
            _siteRepositorio = new Mock<ISiteModuloRepositorio>();

            _cargoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns((Cargo)null);

            var docServico = new CargoServico(_cargoRepositorio.Object, _siteRepositorio.Object, _moduloRepositorio.Object, _cargoProcessoRepositorio.Object);

            var retorno = docServico.ObtemModulosPermitidos(It.IsAny<int>());

            Assert.AreEqual(retorno.Count, 0);
        }

        [TestMethod]
        [TestCategory("Cargo")]
        public void Obtem_Modulos_Permitidos_Site_Vazio()
        {
            InicializadorCargo();

            _cargoRepositorio = new Mock<ICargoRepositorio>();
            _siteRepositorio = new Mock<ISiteModuloRepositorio>();

            _cargoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_cargo);
            _siteRepositorio.Setup(x => x.ListarSiteModuloPorSite(It.IsAny<int>())).Returns(new List<SiteFuncionalidade>());

            var docServico = new CargoServico(_cargoRepositorio.Object, _siteRepositorio.Object, _moduloRepositorio.Object, _cargoProcessoRepositorio.Object);

            var retorno = docServico.ObtemModulosPermitidos(It.IsAny<int>());

            Assert.AreEqual(retorno.Count, 0);
        }
    }
}
