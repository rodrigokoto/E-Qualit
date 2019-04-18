using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;
using System.Collections.Generic;
using Moq;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using ApplicationService.Servico;

namespace TestProject
{
    [TestClass]
    public class AnaliseCriticaTest
    {
        private AnaliseCritica _analiseCritica;
        private List<Usuario> _usuarios;

        public void Construtor()
        {
            _usuarios = new List<Usuario>();
            _analiseCritica = new AnaliseCritica
            {
                Funcionarios = new List<AnaliseCriticaFuncionario>()
            };

            _analiseCritica.Funcionarios.Add(new AnaliseCriticaFuncionario
            {
                Funcionario = new Usuario
                {
                    IdUsuario = 2,
                    NmCompleto = "Usuario2",
                    FlAtivo = false
                }
            });

            _analiseCritica.Funcionarios.Add(new AnaliseCriticaFuncionario
            {
                Funcionario = new Usuario
                {
                    IdUsuario = 3,
                    NmCompleto = "Usuario3",
                    FlAtivo = true
                }
            });

            _analiseCritica.Funcionarios.Add(new AnaliseCriticaFuncionario
            {
                Funcionario = new Usuario
                {
                    IdUsuario = 1,
                    NmCompleto = "Usuario1",
                    FlAtivo = true
                }
            });

            _usuarios.Add(new Usuario
            {
                IdUsuario = 1,
                NmCompleto = "Usuario1",
                FlAtivo = true
            });

            _usuarios.Add(new Usuario
            {
                IdUsuario = 3,
                NmCompleto = "Usuario3",
                FlAtivo = true
            });
        }
        
        [TestMethod]
        [TestCategory("Analise_Critica")]
        public void Usuarios_Por_Analise_Critica_Tres_Usuario_True_E_Um_Falso()
        {
            Construtor();

            var repositorio = new Mock<IAnaliseCriticaRepositorio>();
            var servico = new Mock<IUsuarioClienteSiteAppServico>();
            var analiseCriticaTemaRepositorio = new Mock<IAnaliseCriticaTemaRepositorio>();

            repositorio.Setup(x => x.GetById(It.IsAny<int>()))
                                    .Returns(_analiseCritica);




            servico.Setup(x => x.ListarPorEmpresa(It.IsAny<int>()))
                                .Returns(_usuarios);

            var analiseCriticaServico = new AnaliseCriticaAppServico(repositorio.Object, servico.Object, analiseCriticaTemaRepositorio.Object);

            var usuarios = analiseCriticaServico.ObterUsuariosPorAnaliseCritica(It.IsAny<int>(), It.IsAny<int>());

            Assert.AreEqual(usuarios.Count, 3);
        }

        [TestMethod]
        [TestCategory("Analise_Critica")]
        public void Usuarios_Por_Analise_Critica_Tres_Usuario_True_Analise_Critica_Null()
        {
            Construtor();

            var repositorio = new Mock<IAnaliseCriticaRepositorio>();
            var servico = new Mock<IUsuarioClienteSiteAppServico>();
            var analiseCriticaTemaRepositorio = new Mock<IAnaliseCriticaTemaRepositorio>();
            _analiseCritica = null;

            repositorio.Setup(x => x.GetById(It.IsAny<int>()))
                                    .Returns(_analiseCritica);

            servico.Setup(x => x.ListarPorEmpresa(It.IsAny<int>()))
                                .Returns(_usuarios);

            var analiseCriticaServico = new AnaliseCriticaAppServico(repositorio.Object, servico.Object, analiseCriticaTemaRepositorio.Object);

            var usuarios = analiseCriticaServico.ObterUsuariosPorAnaliseCritica(It.IsAny<int>(), It.IsAny<int>());

            Assert.AreEqual(usuarios.Count, 2);
        }

        [TestMethod]
        [TestCategory("Analise_Critica")]
        public void Usuarios_Por_Analise_Critica_Usuario_Null_Analise_Critica_Null()
        {
            var repositorio = new Mock<IAnaliseCriticaRepositorio>();
            var servico = new Mock<IUsuarioClienteSiteAppServico>();
            var analiseCriticaTemaRepositorio = new Mock<IAnaliseCriticaTemaRepositorio>();
            _analiseCritica = null;
            _usuarios = new List<Usuario>();

            repositorio.Setup(x => x.GetById(It.IsAny<int>()))
                                    .Returns(_analiseCritica);

            servico.Setup(x => x.ListarPorEmpresa(It.IsAny<int>()))
                                .Returns(_usuarios);

            var analiseCriticaServico = new AnaliseCriticaAppServico(repositorio.Object, servico.Object, analiseCriticaTemaRepositorio.Object);

            var usuarios = analiseCriticaServico.ObterUsuariosPorAnaliseCritica(It.IsAny<int>(), It.IsAny<int>());

            Assert.AreEqual(usuarios.Count, 0);
        }

        [TestMethod]
        [TestCategory("Analise_Critica")]
        public void Usuarios_Por_Analise_Critica_Usuario_Null_Analise_Critica_Tres_Usuario()
        {
            Construtor();

            var repositorio = new Mock<IAnaliseCriticaRepositorio>();
            var servico = new Mock<IUsuarioClienteSiteAppServico>();
            var analiseCriticaTemaRepositorio = new Mock<IAnaliseCriticaTemaRepositorio>();
            _usuarios = new List<Usuario>();

            repositorio.Setup(x => x.GetById(It.IsAny<int>()))
                                    .Returns(_analiseCritica);

            servico.Setup(x => x.ListarPorEmpresa(It.IsAny<int>()))
                                .Returns(_usuarios);

            var analiseCriticaServico = new AnaliseCriticaAppServico(repositorio.Object, servico.Object, analiseCriticaTemaRepositorio.Object);

            var usuarios = analiseCriticaServico.ObterUsuariosPorAnaliseCritica(It.IsAny<int>(), It.IsAny<int>());

            Assert.AreEqual(usuarios.Count, 3);
        }
    }
}

