using Dominio.Entidade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Dominio.Enumerado;
using Moq;
using Dominio.Interface.Repositorio;
using Dominio.Validacao.Usuarios;
using Dominio.Servico;

namespace TestProject
{

    [TestClass]
    public class UsuarioTest
    {
        private int _suporte = (int)PerfisAcesso.Suporte;
        private int _coordenador = (int)PerfisAcesso.Coordenador;

        private readonly Mock<IUsuarioRepositorio> _usuarioRepositorio = new Mock<IUsuarioRepositorio>();
        private readonly Mock<IUsuarioCargoRepositorio> _usuarioCargoRepositorio = new Mock<IUsuarioCargoRepositorio>();
        private readonly Mock<ICargoProcessoRepositorio> _cargoProcessoRepositorio = new Mock<ICargoProcessoRepositorio>();
        private readonly Mock<IUsuarioClienteSiteRepositorio> _usuarioClienteSiteRepositorio = new Mock<IUsuarioClienteSiteRepositorio>();

                       

        private List<UsuarioCargo> _usuarioCargo;

        private Usuario _usuario = new Usuario
        {
            NmCompleto = "Teste",
            NuCPF = "123456789",
            FlSexo = "M",
            FlRecebeEmail = true,
            FlAtivo = false,
            FlCompartilhado = true,
            FlBloqueado = false,
            DtAlteracaoSenha = DateTime.Now.AddYears(2),
            DtExpiracao = DateTime.Now,
            CdIdentificacao = UtilsServico.GeraNovaSenha(),
            CdSenha = "123mudar",
        };

        private List<UsuarioCargo> _cargos = new List<UsuarioCargo>();
        private List<UsuarioClienteSite> _usuarioClienteSite = new List<UsuarioClienteSite>();

        public void InicializadorUsuariosCargo()
        {
            var sites = new List<SiteFuncionalidade>();
            _usuarioCargo = new List<UsuarioCargo>();

            sites.Add(new SiteFuncionalidade
            {
                Funcionalidade = new Funcionalidade
                {
                    IdFuncionalidade = 1,
                    Nome = "Teste"
                }
            });

            _usuarioCargo.Add(new UsuarioCargo
            {
                IdUsuario = 1,
                Cargo = new Cargo
                {
                    Site = new Dominio.Entidade.Site
                    {
                        IdSite = 1,
                        SiteFuncionalidades = sites
                    }
                }
            });
        }

        public void InicializadorCargos()
        {
            _cargos.Add(new UsuarioCargo
            {
                IdCargo = 28,
                Cargo = new Cargo
                {
                    NmNome = "Teste",
                },
            });
            _cargos.Add(new UsuarioCargo
            {
                IdCargo = 29,
                Cargo = new Cargo
                {
                    NmNome = "Teste 2",
                },
            });
            _cargos.Add(new UsuarioCargo
            {
                IdCargo = 30,
                Cargo = new Cargo
                {
                    NmNome = "Teste 3",
                },
            });
            _cargos.Add(new UsuarioCargo
            {
                IdCargo = 31,
                Cargo = new Cargo
                {
                    NmNome = "Teste 4",
                },
            });
        }

    
        [TestMethod]
        [TestCategory("Usuario")]
        public void Usuario_Apto_Para_Acessar_True()
        {
            var _usuario = new Usuario
            {
                FlAtivo = true
            };

            var validacao = new AptoParaAcessarValidation().Validate(_usuario);

            Assert.IsTrue(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Usuario")]
        public void Usuario_Apto_Para_Acessar_False()
        {
            var _usuario = new Usuario();

            var validacao = new AptoParaAcessarValidation().Validate(_usuario);

            Assert.IsFalse(validacao.IsValid);
        }

    }
}
