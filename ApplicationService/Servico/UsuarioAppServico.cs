using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using Dominio.Validacao.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Servico;
using System.Configuration;
using ApplicationService.Entidade;
using ApplicationService.Enum;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Linq.Expressions;
using DAL.Context;

namespace ApplicationService.Servico
{
    public class UsuarioAppServico : BaseServico<Usuario>, IUsuarioAppServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUsuarioCargoRepositorio _usuarioCargoRepositorio;
        private readonly IUsuarioClienteSiteRepositorio _usuarioClienteSiteRepositorio;
        private readonly ICargoProcessoRepositorio _cargoProcessoRepositorio;
        private readonly ISiteRepositorio _siteRepositorio;
        private long _administrador = (int)PerfisAcesso.Administrador;
        private long _suporte = (int)PerfisAcesso.Suporte;
        private long _coordenador = (int)PerfisAcesso.Coordenador;
        private long _colaborador = (int)PerfisAcesso.Colaborador;


        public UsuarioAppServico(IUsuarioRepositorio usuarioRepositorio,
                              IUsuarioCargoRepositorio usuarioCargoRepositorio,
                              ISiteRepositorio siteRepositorio,
                              IUsuarioClienteSiteRepositorio usuarioClienteSiteRepositorio,
                              ICargoProcessoRepositorio cargoProcessoRepositorio) : base(usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _usuarioCargoRepositorio = usuarioCargoRepositorio;
            _siteRepositorio = siteRepositorio;
            _usuarioClienteSiteRepositorio = usuarioClienteSiteRepositorio;
            _cargoProcessoRepositorio = cargoProcessoRepositorio;
        }


        public List<Funcionalidade> ObtemModulosPermitidos(int idUsuario)
        {
            var modulos = new List<Funcionalidade>();

            try
            {
                var sites = new List<SiteFuncionalidade>();
                var usuarioCargos = _usuarioCargoRepositorio.ObterPorIdUsuario(idUsuario);

                foreach (var usuarioCargo in usuarioCargos)
                {
                    foreach (var siteModulo in usuarioCargo.Cargo.Site.SiteFuncionalidades)
                    {
                        modulos.Add(new Funcionalidade
                        {
                            IdFuncionalidade = siteModulo.Funcionalidade.IdFuncionalidade,
                            Nome = siteModulo.Funcionalidade.Nome
                        });
                    }
                }
            }
            catch (Exception)
            {
                return modulos;
            }

            return modulos;
        }

        public List<Usuario> ObterFuncionalidadesPorUsuario(int idSite, int idFuncao, int idProcesso, int idUsuario)
        {

            var usuarios = new List<Usuario>();
            try
            {

                var cargoProcessos = _cargoProcessoRepositorio.GetAll().Where(x => x.IdProcesso == idProcesso && x.Cargo.Ativo == true &&
                                                                   x.Processo.FlAtivo == true).ToList();


                foreach (var cargoProcesso in cargoProcessos.Where(x => x.IdFuncao == idFuncao))
                {
                    foreach (var usuarioCargo in cargoProcesso.Cargo.UsuarioCargos.Where(x => x.IdUsuario == idUsuario))
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = usuarioCargo.Usuario.IdUsuario,
                            NmCompleto = usuarioCargo.Usuario.NmCompleto
                        });
                    }
                }

                usuarios = RetiraDuplicado(usuarios);

            }
            catch (Exception)
            {

            }

            return usuarios;
        }

        public List<Usuario> ObterUsuariosPorFuncionalidade(int idProcesso, int idSite, int idFuncionalidade)
        {

            var usuarios = new List<Usuario>();
            try
            {

                var cargoProcessos = _cargoProcessoRepositorio.GetAll().Where(x => x.IdProcesso == idProcesso && x.Cargo.Ativo == true &&
                                                                   x.Processo.FlAtivo == true).ToList();

                var funcoes = cargoProcessos.Where(x => x.Funcao.IdFuncionalidade == idFuncionalidade);

                foreach (var cargoProcesso in funcoes)
                {
                    foreach (var usuarioCargo in cargoProcesso.Cargo.UsuarioCargos.Where(x => x.Usuario.FlAtivo == true))
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = usuarioCargo.Usuario.IdUsuario,
                            NmCompleto = usuarioCargo.Usuario.NmCompleto
                        });
                    }


                }

                usuarios = RetiraDuplicado(usuarios);

            }
            catch (Exception)
            {

            }


            return usuarios;
        }

        public List<Usuario> ObterUsuariosPorFuncao(int? idProcesso = null, int? idSite = null, int? idFuncao = null)
        {
            try
            {
                var cargoProcessos = _cargoProcessoRepositorio.Get(x => (x.IdProcesso == idProcesso || idProcesso == null) &&
                                                                   (x.IdFuncao == idFuncao.Value || idFuncao == null) &&
                                                                   x.Cargo.IdSite == idSite.Value &&
                                                                   x.Cargo.Ativo == true &&
                                                                   x.Processo.FlAtivo == true);

                List<Usuario> usuarios = new List<Usuario>();
                usuarios = PopularCargoProcesso(cargoProcessos, idSite.Value);

                //var usuariosCornedador = _usuarioClienteSiteRepositorio.Get(x => x.IdSite == idSite && idSite != null && x.Usuario.IdPerfil == (int)PerfisAcesso.Coordenador && x.Usuario.FlAtivo == true).Select(x => x.Usuario).ToList();

                //usuariosCornedador.ForEach(x =>
                //{
                //    if(!usuarios.Select(y => y.IdUsuario).Contains(x.IdUsuario))
                //    {
                //        usuarios.Add(x);
                //    }
                //});

                return usuarios;

            }
            catch (Exception)
            {
                //TODO: Adicionar a tratativa para log
            }

            return new List<Usuario>();
        }

        public List<Usuario> ObterUsuariosPorFuncao(int idSite, int idFuncao)
        {
            var usuarios = new List<Usuario>();

            var funcoes = _cargoProcessoRepositorio.Get(x => x.Cargo.IdSite == idSite && x.IdFuncao == idFuncao && x.Cargo.Ativo == true &&
                                                                   x.Processo.FlAtivo == true).ToList();

            return PopularCargoProcesso(funcoes, idSite);
        }

        private List<Usuario> PopularCargoProcesso(IEnumerable<CargoProcesso> cargoProcessos, int idSite)
        {

            var usuarios = new List<Usuario>();

            var usuarios1 = cargoProcessos.SelectMany(x => x.Cargo.UsuarioCargos).ToList();

            var user = (from a in usuarios1
                        where a.Usuario.FlAtivo == true
                        select new Usuario
                        {
                            IdUsuario = a.Usuario.IdUsuario,
                            NmCompleto = a.Usuario.NmCompleto
                        }).ToList();

            usuarios.AddRange(user);

            //foreach (var cargoProcesso in cargoProcessos)
            //{
            //    foreach (var usuarioCargo in cargoProcesso.Cargo.UsuarioCargos.Where(x => x.Usuario.FlAtivo == true).ToList())
            //    {
            //        usuarios.Add(new Usuario
            //        {
            //            IdUsuario = usuarioCargo.Usuario.IdUsuario,
            //            NmCompleto = usuarioCargo.Usuario.NmCompleto
            //        });
            //    }
            //}

            var coordenadores = cargoProcessos.Select(
                                      x => x.Processo.Site.UsuarioClienteSites
                                         .Where(y => y.Usuario.IdPerfil == (byte)PerfisAcesso.Coordenador && y.IdSite == x.Processo.IdSite && y.Usuario.FlAtivo))
                                         .SelectMany(x => x.Select(b => b.Usuario)).ToList();

            if (coordenadores.Count() == 0)
            {
                coordenadores = _usuarioClienteSiteRepositorio.Get(x => x.IdSite == idSite)
                    .Select(y => y.Usuario).Where(b => b.IdPerfil == (byte)PerfisAcesso.Coordenador && b.FlAtivo == true).ToList();

            }

            coordenadores.ForEach(coordenador =>
            {
                usuarios.Add(new Usuario
                {
                    IdUsuario = coordenador.IdUsuario,
                    NmCompleto = coordenador.NmCompleto
                });
            });

            usuarios = RetiraDuplicado(usuarios);

            return RetiraDuplicado(usuarios);
        }

        public static List<Usuario> RetiraDuplicado(List<Usuario> usuarios)
        {
            return usuarios.GroupBy(x => x.IdUsuario)
                                 .Select(y => y.First())
                                 .ToList();
        }


        public List<Usuario> ObterUsuariosPorPerfilESite(int idSite, int idPerfil, int idPerfilLogado)
        {
            var usuarios = new List<Usuario>();

            try
            {

                usuarios = _usuarioRepositorio.Get(x => (x.IdPerfil == idPerfil || (x.IdPerfil == idPerfilLogado && idPerfilLogado == 1)) && x.UsuarioClienteSites.Select(y => y.IdSite).Contains(idSite) && x.FlAtivo == true).ToList();



                return usuarios;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool PossuiAcesso(int idUsuario, int idfuncionalidade, int idFuncao)
        {
            try
            {
                var usuarios = new List<Usuario>();

                var cargoProcessos = _cargoProcessoRepositorio.Get(x => x.Funcao.IdFuncionalidade == idfuncionalidade &&
                                                                        x.Funcao.IdFuncao == idFuncao && x.Cargo.Ativo == true &&
                                                                   x.Processo.FlAtivo == true).ToList();

                foreach (var cargoProcesso in cargoProcessos)
                {
                    foreach (var usuarioCargo in cargoProcesso.Cargo.UsuarioCargos.Where(x => x.Usuario.IdUsuario.Equals(idUsuario)))
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = usuarioCargo.Usuario.IdUsuario,
                            NmCompleto = usuarioCargo.Usuario.NmCompleto
                        });
                    }
                }

                if (usuarios.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                //TODO: Adicionar a tratativa para log
            }
            return false;
        }

        public List<Usuario> ObterUsuariosPorPerfil(int idPerfil)
        {
            var usuarios = new List<Usuario>();

            try
            {

                if (EAdministrador(idPerfil))
                {
                    usuarios = _usuarioRepositorio.Get(x => x.IdPerfil == _administrador ||
                                                       x.IdPerfil == _suporte).ToList();
                }
                else
                {
                    usuarios = _usuarioRepositorio.Get(x => x.IdPerfil == _colaborador ||
                                                       x.IdPerfil == _coordenador).ToList();
                }

                return usuarios;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AlterarSenha(int idUsuario, string novaSenha)
        {
            try
            {
                var usuarioAtualizar = _usuarioRepositorio.GetById(idUsuario);

                usuarioAtualizar.CdSenha = UtilsServico.Sha1Hash(novaSenha);
                _usuarioRepositorio.AtualizaSenha(usuarioAtualizar);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PodeAlterarSenha(Usuario entidade, ref List<string> erros)
        {
            var validation = new AlterarSenhaValidation(_usuarioRepositorio);

            var erro = validation.Validate(entidade);

            erros.AddRange(UtilsServico.PopularErros(erro));
        }

        public Usuario ObterUsuarioPorCdIdentificacao(string cdIdentificacao)
        {
            return _usuarioRepositorio.Get(x => x.CdIdentificacao == cdIdentificacao).FirstOrDefault();
        }

        public List<Usuario> ObterUsuariosPorCargo(int idCargo)
        {
            var cargos = _usuarioCargoRepositorio.Get(x => x.IdCargo == idCargo).ToList();
            var usuarios = new List<Usuario>();

            foreach (var cargo in cargos)
            {
                usuarios.Add(cargo.Usuario);
            }

            return usuarios;
        }

        public List<Funcionalidade> ObterFuncionalidadesPermitidas(int idUsuario)
        {
            var listaProcesso = new List<CargoProcesso>();
            var idCargosUsuarios = new List<int>();
            var cargosProcesso = new List<CargoProcesso>();
            var funcionalidades = new List<Funcionalidade>();

            var usuarioCargo = _usuarioCargoRepositorio.Get(x => x.IdUsuario == idUsuario).ToList();
             
            foreach (var usuario in usuarioCargo)
            {
                foreach (var processo in usuario.Cargo.CargoProcessos.Where(x => x.Cargo.Ativo == true && x.Processo != null &&
                                                                   x.Processo.FlAtivo == true).ToList())
                {
                    funcionalidades.Add(processo.Funcao.Funcionalidade);
                }
            }

            return funcionalidades.Where(x => x.IdFuncionalidade != 2 && x.Ativo == true).Distinct().ToList();
        }

        public List<Funcionalidade> ObterFuncionalidadesPermitidasPorSite(int idSite)
        {
            var listaProcesso = new List<CargoProcesso>();
            var idCargosUsuarios = new List<int>();
            var cargosProcesso = new List<CargoProcesso>();

            if (idSite != 0)
            {
                List<Funcionalidade> funcionalidades = _siteRepositorio.Get(x => x.IdSite == idSite).FirstOrDefault().SiteFuncionalidades.Select(y => y.Funcionalidade).Where(x => x.CdFormulario == "1").Distinct().ToList();
                return funcionalidades.Where(x => x.Ativo == true).ToList();
            }
            else
            {
                return new List<Funcionalidade>();
            }

        }

        public List<Funcionalidade> ObterRelatorioPorSite(int idSite){

            using (var db = new BaseContext()) {

                var query = (from funcionalidade in db.Funcionalidade
                            join relat in db.Relatorio on funcionalidade.IdFuncionalidade equals relat.IdFuncionalidade
                            select funcionalidade);
            }

                return new List<Funcionalidade>();
        }

        public bool EAdministrador(int idPerfil)
        {
            if (idPerfil != (int)PerfisAcesso.Administrador)
            {
                return false;
            }
            return true;
        }

        private void EnviaEmail(Usuario usuario, TempletesDisparoDeEmail templetesDisparoDeEmail, Guid? token = null)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\{templetesDisparoDeEmail}-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);
            string conteudo = template;

            conteudo = conteudo.Replace("#NOME#", usuario.NmCompleto);
            conteudo = conteudo.Replace("#SENHA#", usuario.CdSenha);
            conteudo = conteudo.Replace("#NOMESISTEMA#", "e-Qualit");

            switch (templetesDisparoDeEmail)
            {
                case TempletesDisparoDeEmail.NovoUsuario:
                    conteudo = conteudo.Replace("#DOMINIO#", ConfigurationManager.AppSettings["Dominio"]);
                    break;
                case TempletesDisparoDeEmail.ReenvioSenha:
                    conteudo = conteudo.Replace("#DOMINIO#", (ConfigurationManager.AppSettings["DominioAlterarSenha"] + token.Value));
                    break;
                default:
                    break;
            }


            Email _email = new Email();

            _email.Assunto = "Acesso ao Sistema e-Qualit";
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = usuario.CdIdentificacao;
            _email.Conteudo = conteudo;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();
        }

        public void EnviaEmailEsqueciASenha(Usuario usuario, Guid token)
        {
            EnviaEmail(usuario, TempletesDisparoDeEmail.ReenvioSenha, token);
        }

        public void EnviaEmailNovoUsuario(Usuario usuario)
        {
            EnviaEmail(usuario, TempletesDisparoDeEmail.NovoUsuario);
        }

        public void AtualizarCadastro(Usuario usuario)
        {

            var usuarioCtx = _usuarioRepositorio.GetById(usuario.IdUsuario);
            var listaCriacao = new List<UsuarioClienteSite>();
            if (usuario.IdPerfil == (int)PerfisAcesso.Suporte)
            {
                usuario.UsuarioClienteSites.ToList().ForEach(x =>
                {
                    x.Usuario = usuario;
                    var listaIdSite = _siteRepositorio.Get(y => y.IdCliente == x.IdCliente).Select(v => v.IdSite)
                    .Distinct()
                    .ToList();

                    listaIdSite.ForEach(w =>
                    {
                        listaCriacao.Add(new UsuarioClienteSite
                        {
                            IdUsuario = usuario.IdUsuario,
                            IdCliente = x.IdCliente,
                            IdSite = w
                        });
                    });
                });
            }

            else if (usuario.IdPerfil == (int)PerfisAcesso.Coordenador)
            {
                usuarioCtx.UsuarioClienteSites.ToList().ForEach(x =>
                {
                    var site = _siteRepositorio.GetById(x.IdSite.Value);
                    listaCriacao.Add(new UsuarioClienteSite
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdCliente = site.IdCliente,
                        IdSite = x.IdSite.Value
                    });
                });
                _usuarioCargoRepositorio.DeletaTodosCargoUsuario(usuarioCtx.IdUsuario);
            }
            else if (usuario.IdPerfil == (int)PerfisAcesso.Colaborador)
            {
                usuario.UsuarioClienteSites.ToList().ForEach(x =>
                {
                    var site = _siteRepositorio.GetById(x.IdSite.Value);
                    listaCriacao.Add(new UsuarioClienteSite
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdCliente = site.IdCliente,
                        IdSite = x.IdSite.Value
                    });
                });
                usuario.UsuarioCargoes.ToList().ForEach(x =>
                {

                    int usuariosCargo = _usuarioCargoRepositorio.Get(uc => uc.IdCargo == x.IdCargo && uc.IdUsuario == usuario.IdUsuario).Count();
                    if (usuariosCargo == 0)
                    {
                        x.IdUsuario = usuario.IdUsuario;
                        _usuarioCargoRepositorio.Add(x);
                    }
                });

                _usuarioCargoRepositorio.Get(uc => uc.IdUsuario == usuario.IdUsuario).ToList().ForEach(x =>
                {
                    if (!usuario.UsuarioCargoes.Select(y => y.IdCargo).Contains(x.IdCargo))
                    {
                        _usuarioCargoRepositorio.Remove(x);
                    }
                });
            }

            usuarioCtx.DtAlteracao = DateTime.Now;
            usuarioCtx.NmCompleto = usuario.NmCompleto;
            //usuarioCtx.FlSexo = usuario.FlSexo;
            usuarioCtx.CdIdentificacao = usuario.CdIdentificacao;
            usuarioCtx.NuCPF = usuario.NuCPF;
            usuarioCtx.IdPerfil = usuario.IdPerfil;
            usuarioCtx.DtExpiracao = usuario.DtExpiracao;
            usuarioCtx.FlRecebeEmail = usuario.FlRecebeEmail;
            usuarioCtx.FlCompartilhado = usuario.FlCompartilhado;
            usuarioCtx.FlAtivo = usuario.FlAtivo;
            usuarioCtx.FlBloqueado = usuario.FlBloqueado;

            _usuarioRepositorio.Update(usuarioCtx);

        }

        public void AtualizarMeusDados(Usuario usuario)
        {
            var usuarioCtx = _usuarioRepositorio.GetById(usuario.IdUsuario);

            usuarioCtx.DtAlteracao = DateTime.Now;
            usuarioCtx.NmCompleto = usuario.NmCompleto;
            //usuarioCtx.FlSexo = usuario.FlSexo;
            usuarioCtx.CdIdentificacao = usuario.CdIdentificacao;
            usuarioCtx.NuCPF = usuario.NuCPF;

            _usuarioRepositorio.Update(usuarioCtx);
        }

        public void NovaSenhaRandomica(Usuario usuario, string novaSenha)
        {
            try
            {
                usuario.CdSenha = UtilsServico.Sha1Hash(novaSenha);
                _usuarioRepositorio.Update(usuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AtivarInativar(int idUsuario)
        {
            try
            {
                var usuario = _usuarioRepositorio.GetById(idUsuario);

                usuario.FlAtivo = !usuario.FlAtivo;
                _usuarioRepositorio.Update(usuario);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool BloqueiaDesbloqueia(int idUsuario)
        {
            try
            {
                var usuario = _usuarioRepositorio.GetById(idUsuario);

                usuario.FlBloqueado = !usuario.FlBloqueado;
                _usuarioRepositorio.Update(usuario);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RecebeNaoRecebeEmail(int idUsuario)
        {
            try
            {
                var usuario = _usuarioRepositorio.GetById(idUsuario);

                usuario.FlRecebeEmail = !usuario.FlRecebeEmail;
                _usuarioRepositorio.Update(usuario);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}