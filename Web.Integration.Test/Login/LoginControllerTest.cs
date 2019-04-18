using ApplicationService.Interface;
using ApplicationService.Servico;
using DAL.Repository;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Servico;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;
using Web.UI.Controllers;
using Microsoft.CSharp;
using System.Reflection;
using Newtonsoft.Json;
using Moq;

namespace Web.Integration.Test.Login
{
    [TestClass]
    public class LoginControllerTest
    {
        #region Interfaces Services
        private IClienteAppServico _clienteAppServico;
        private IUsuarioAppServico _usuarioAppServico;
        private ILogAppServico _logAppServico;
        private ILoginAppServico _loginAppServico;
        private ILoginServico _loginServico;
        private ISiteModuloAppServico _siteModulo;

        #endregion

        #region Interfaces Repositories
        private readonly IClienteRepositorio _clienteRepositorio = new ClienteRepositorio();
        private readonly IUsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio();
        private readonly IUsuarioCargoRepositorio _usuarioCargoRepositorio = new UsuarioCargoRepositorio();
        private readonly IUsuarioClienteSiteRepositorio _usuarioClienteSiteRepositorio =  new UsuarioClienteSiteRepositorio();
        private readonly ICargoProcessoRepositorio _cargoProcessoRepositorio = new CargoProcessoRepositorio();

        private readonly ILogRepositorio _logRepositorio = new LogRepositorio();

        private readonly ISiteModuloRepositorio _siteModuloRepositorio = new SiteModuloRepositorio();


        #endregion
      


        private LoginController _loginController;

        [TestInitialize]
        public void Init()
        {

            

            _clienteAppServico = new ClienteAppServico(_clienteRepositorio, _usuarioRepositorio);

            _usuarioAppServico = new UsuarioAppServico(_usuarioRepositorio, _usuarioCargoRepositorio,
                _usuarioClienteSiteRepositorio, _cargoProcessoRepositorio);

            _logAppServico = new LogAppServico(_logRepositorio);

            _loginServico = new LoginServico(_usuarioRepositorio);

            _loginAppServico = new LoginAppServico(_usuarioRepositorio);

            _siteModulo = new SiteModuloAppServico(_siteModuloRepositorio);

            _loginController = new LoginController(_clienteAppServico, _usuarioAppServico,
                    _logAppServico, _loginServico, _loginAppServico, _siteModulo);


        }

        [TestMethod]
        public void Get_View_Login()
        {

            //var action = _loginController.Action(x => x.Index());
            //// act
            //var result = action.Execute();
            //// assert
            //Assert.IsInstanceOfType(result, typeof(ActionResult));

            //var teste = action.Controller;

        }

        [TestMethod]
        public void Post_Login_True()
        {
            // arrange
            //var user = new Usuario
            //{
            //    CdIdentificacao = "administrador@g2it.com.br",
            //    CdSenha = "kafe2##9"
            //};


            //var result = _loginController.Acesso(user);

            //var jsonExpected = new JsonResult();
            //jsonExpected.Data = new { StatusCode = 200 };
            //jsonExpected.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //Assert.IsInstanceOfType(result, typeof(JsonResult));
            //Assert.IsTrue(jsonExpected.Data.ToString().Equals(result.Data.ToString()));
        }

        [TestMethod]
        public void Post_Login_False()
        {
            var user = new Usuario
            {
                CdIdentificacao = "administrador@g2it.com.br",
                CdSenha = "kafe2##9"
            };

            var jsonExpected = JsonConvert.SerializeObject(new { StatusCode = 200 });
          
            _loginController.WithCallTo(c => c.Acesso(user)).ShouldReturnJson(data =>
            {
                var jsonString = JsonConvert.SerializeObject(data);
                Assert.AreEqual(jsonExpected, jsonString);
            });

        }

       
    }
}
