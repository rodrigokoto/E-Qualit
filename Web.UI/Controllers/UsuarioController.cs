using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio.Enumerado;
using System.Collections.Generic;
using Dominio.Entidade;
using Web.UI.Helpers;
using ApplicationService.Interface;
using ApplicationService.Servico;
using Dominio.Servico;
using Dominio.Interface.Servico;
using System.Web;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    [ValidaUsuario]
    public class UsuarioController : BaseController
    {
        private readonly ISiteAppServico _siteAppServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteSiteAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IUsuarioServico _usuarioServico;
        private readonly IUsuarioSenhaServico _usuarioSenhaServico;
        //private readonly IClienteAppServico _clienteAppServico;
        private readonly IUsuarioCargoAppServico _usuarioCargoAppServico;
        private readonly IAnexoAppServico _anexoAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAnexoAppServico _usuarioAnexoAppServico;
        private readonly IClienteServico _clienteServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public UsuarioController(ISiteAppServico siteAppServico,
                                IUsuarioClienteSiteAppServico usuarioClienteSiteAppServico,
                                IUsuarioAppServico usuarioAppServico,
                                IUsuarioCargoAppServico usuarioCargoAppServico,
                                ILogAppServico logAppServico,
                                IAnexoAppServico anexoAppServico,
                                IUsuarioServico usuarioServico,
                                IUsuarioSenhaServico usuarioSenhaServico,
                                IUsuarioAnexoAppServico usuarioAnexoAppServico,
                                IClienteServico clienteServico,
                                IProcessoAppServico processoAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            ViewBag.IdForm = "frmUsuario";
            _usuarioClienteSiteAppServico = usuarioClienteSiteAppServico;
            _usuarioAppServico = usuarioAppServico;
            _usuarioCargoAppServico = usuarioCargoAppServico;
            _logAppServico = logAppServico;
            _siteAppServico = siteAppServico;
            _anexoAppServico = anexoAppServico;
            _usuarioServico = usuarioServico;
            _usuarioSenhaServico = usuarioSenhaServico;
            _usuarioAnexoAppServico = usuarioAnexoAppServico;
            //_clienteAppServico = clienteAppServico;
            _clienteServico = clienteServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        [AcessoUsuarioSite]
        public ActionResult Index(int? id, int ? idSite)
        {
            var usuarios = new List<Usuario>();
            var idPerfil = Util.ObterPerfilUsuarioLogado();
            var idCliente = Util.ObterClienteSelecionado();
            
            var idUsuarioLogado = Util.ObterCodigoUsuarioLogado();


            ViewBag.IdPerfil = idPerfil;
            ViewBag.IdCliente = idCliente;
            ViewBag.IdSite = idSite == null ? Util.ObterSiteSelecionado() : idSite;

            if (idPerfil == (int)PerfisAcesso.Administrador)
            {
                if (id.HasValue)
                {
                    usuarios.AddRange(_usuarioClienteSiteAppServico.Get(y => y.IdCliente == id.Value && (idSite == null ||  idSite == 0 || y.IdSite == idSite)).Select(d => d.Usuario).Distinct().ToList());
                    SetCookieClienteSelecionado(id.Value);
                }
                else
                {
                    usuarios.AddRange(_usuarioAppServico.Get(x => x.IdPerfil == (int)PerfisAcesso.Administrador || x.IdPerfil == (int)PerfisAcesso.Suporte).ToList());

                }
            }
            else if (idPerfil == (int)PerfisAcesso.Suporte)
            {
                var listaUsuario = new List<Usuario>();
                var logado = _usuarioAppServico.GetById(idUsuarioLogado);
                logado.UsuarioClienteSites.ToList().ForEach(x =>
                {
                    listaUsuario.AddRange(_usuarioClienteSiteAppServico.Get(y => y.IdCliente == x.IdCliente && (idSite == 0 || y.IdSite == idSite)).Select(d => d.Usuario).Distinct().ToList());
                });
                usuarios.AddRange(listaUsuario);
            }
            else if (idPerfil == (int)PerfisAcesso.Coordenador)
            {
                var listaUsuario = new List<Usuario>();
                var logado = _usuarioAppServico.GetById(idUsuarioLogado);
                logado.UsuarioClienteSites.ToList().ForEach(x =>
                {
                    listaUsuario.AddRange(_usuarioClienteSiteAppServico.Get(y => y.IdCliente == x.IdCliente && (idSite == 0 || y.IdSite == idSite)).Select(d => d.Usuario).Where(u => u.IdPerfil != (int)PerfisAcesso.Suporte).Distinct().ToList());
                });
                usuarios.AddRange(listaUsuario);
            }

            return View(usuarios);
        }

        private void SetCookieClienteSelecionado(int idCliente)
        {
            HttpCookie cookie = Request.Cookies["clienteSelecionado"];


            cookie.Value = idCliente.ToString();

            cookie.Expires = DateTime.Now.AddDays(1);

            Response.Cookies.Set(cookie);
        }

        private bool SelecionouCliente(int idCliente)
        {
            if (idCliente == 0)
            {
                return false;
            }

            return true;
        }

        [AcessoUsuarioSite]
        public ActionResult Editar(int id, int IdSite = 0)
        {

            ViewBag.IdCliente = id != 0 ? id : Util.ObterClienteSelecionado();
            ViewBag.IdSite = IdSite != 0 ? IdSite : Util.ObterSiteSelecionado();
            var usuario = _usuarioAppServico.GetById(id);

            return View("Criar", usuario);
        }

        [AcessoUsuarioSite]
        public ActionResult Criar(int? id, int idCliente = 0, int IdSite = 0)
        {
            var usuario = new Usuario();

            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdCliente = idCliente != 0 ? idCliente : Util.ObterClienteSelecionado();
            ViewBag.IdSite = IdSite != 0 ? IdSite : Util.ObterSiteSelecionado();

            return View(usuario);
        }

        [HttpPost]
        [AcessoUsuarioSite]
        public JsonResult Criar(Usuario usuario)
        {
            usuario.FotoPerfilAux.Tratar();

            var erros = new List<string>();
            try
            {
                TrataUsuarioCriacao(usuario);

                _usuarioServico.Valido(usuario, ref erros);

                if (erros.Count != 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (usuario.FotoPerfilAux.ArquivoB64 != null)
                    {
                        usuario.FotoPerfil.Add(new UsuarioAnexo
                        {
                            Usuario = usuario,
                            Anexo = usuario.FotoPerfilAux
                        });
                    }


                    _usuarioAppServico.Add(usuario);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            usuario.CdSenha = usuario.SenhaAtual;

            try
            {
                _usuarioAppServico.EnviaEmailNovoUsuario(usuario);
                return Json(new { StatusCode = 200, Success = Traducao.Usuario.ResourceUsuario.Usuario_msg_criar_valid }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { StatusCode = 200, Success = Traducao.Usuario.ResourceUsuario.Usuario_msg_ErroEnviarEmail }, JsonRequestBehavior.AllowGet);
            }           
            
        }

        private void TrataUsuarioCriacao(Usuario usuario)
        {
            usuario.NuCPF = usuario.NuCPF != null ? RetornaApenasNumeros(usuario.NuCPF) : null;

            usuario.DtInclusao = DateTime.Now;
            usuario.DtAlteracaoSenha = DateTime.Now;
            usuario.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();

            //TrataAnexo(usuario);
            GerarNovaSenha(usuario);
            if (usuario.UsuarioClienteSites.Count > 0)
            {

                var listaCriacao = new List<UsuarioClienteSite>();
                if (usuario.IdPerfil == (int)PerfisAcesso.Suporte)
                {
                    usuario.UsuarioClienteSites.ToList().ForEach(x =>
                    {
                        x.Usuario = usuario;
                        var listaIdSite = _siteAppServico.Get(y => y.IdCliente == x.IdCliente).Select(v => v.IdSite)
                        .Distinct()
                        .ToList();

                        listaIdSite.ForEach(w =>
                        {
                            listaCriacao.Add(new UsuarioClienteSite
                            {
                                Usuario = usuario,
                                IdCliente = x.IdCliente,
                                IdSite = w
                            });
                        });
                    });
                }

                else if (usuario.IdPerfil == (int)PerfisAcesso.Coordenador)
                {
                    usuario.UsuarioClienteSites.ToList().ForEach(x =>
                    {
                        var site = _siteAppServico.GetById(x.IdSite.Value);
                        listaCriacao.Add(new UsuarioClienteSite
                        {
                            Usuario = usuario,
                            IdCliente = site.IdCliente,
                            IdSite = x.IdSite.Value
                        });
                    });
                }
                else if (usuario.IdPerfil == (int)PerfisAcesso.Colaborador)
                {
                    usuario.UsuarioClienteSites.ToList().ForEach(x =>
                    {
                        var site = _siteAppServico.GetById(x.IdSite.Value);
                        listaCriacao.Add(new UsuarioClienteSite
                        {
                            Usuario = usuario,
                            IdCliente = site.IdCliente,
                            IdSite = x.IdSite.Value
                        });
                    });
                }

                usuario.UsuarioClienteSites = listaCriacao;
            }
        }


        private void GerarNovaSenha(Usuario usuario)
        {
            var senha = Util.GeraNovaSenha();
            var senhaCriptografada = UtilsServico.Sha1Hash(senha);
            usuario.CdSenha = senhaCriptografada;
            usuario.SenhaAtual = senha;
        }

        private void TrataUsuarioEditar(Usuario usuario)
        {
            if (usuario.NuCPF != null)
            {
                usuario.NuCPF = RetornaApenasNumeros(usuario.NuCPF);
            }
            usuario.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
        }

        [HttpPost]
        [AcessoUsuarioSite]
        public JsonResult Editar(Usuario usuario)
        {
            usuario.FotoPerfilAux.Tratar();
            var erros = new List<string>();
            try
            {

                TrataUsuarioEditar(usuario);

                _usuarioServico.Valido(usuario, ref erros);

                if (erros.Count > 0)
                {
                    return Json(new { StatusCode = 505, Erros = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (usuario.FotoPerfilAux.IdAnexo == 0 && usuario.FotoPerfilAux.ArquivoB64 != null)
                    {

                            usuario.FotoPerfil.Add(new UsuarioAnexo
                            {
                                IdUsuario = usuario.IdPerfil,
                                Anexo = usuario.FotoPerfilAux
                            });
                            _usuarioAnexoAppServico.Add(new UsuarioAnexo
                            {
                                IdUsuario = usuario.IdUsuario,
                                Anexo = usuario.FotoPerfilAux
                            });
                            _anexoAppServico.Add(usuario.FotoPerfilAux);
                    }
                    else if (usuario.FotoPerfilAux.IdAnexo > 0)
                    {
                        var anexoCtx = _anexoAppServico.GetById(usuario.FotoPerfilAux.IdAnexo);
                        anexoCtx.Nome = usuario.FotoPerfilAux.Nome;
                        anexoCtx.Extensao = usuario.FotoPerfilAux.Extensao;
                        anexoCtx.Arquivo = usuario.FotoPerfilAux.Arquivo;
                        _anexoAppServico.Update(anexoCtx);

                    }
                    _usuarioAppServico.AtualizarCadastro(usuario);
                }

            }
            catch (Exception ex)
            {

                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Usuario.ResourceUsuario.Usuario_msg_editar_valid }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListaSiteCargo(int idPerfil = 0, int idUsuario = 0, int idCliente = 0)
        {
            var usuario = _usuarioAppServico.GetById(idUsuario);

            usuario.IdPerfil = idPerfil;

            return PartialView("_TipoUsuario", usuario);
        }

        [HttpGet]
        public JsonResult ObterFuncao(int idUsuario)
        {
            var funcoes = string.Empty;

            var usuario = _usuarioAppServico.GetById(idUsuario);

            if (usuario.IdPerfil == (int)PerfisAcesso.Coordenador)
            {
                funcoes = "Coordenador";
            }
            else
            {
                funcoes = _usuarioCargoAppServico.ListarFuncaoConcatenadas(idUsuario);
            }

            return Json(new { StatusCode = 200, Funcoes = funcoes }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObterFuncionalidadesPorUsuario(int idSite, int idFuncao, int idProcesso, int idUsuario)
        {
            //int idSite = 5;
            //int idFuncao = 5;
            //int idProcesso = 21;
            //int idUsuario = 22;
            var usuarios = new List<Usuario>();



            if (Util.ObterPerfilUsuarioLogado() == (byte)PerfisAcesso.Coordenador)
            {
                var usuarioCTX = _usuarioAppServico.GetById(Util.ObterCodigoUsuarioLogado());
                usuarios.Add(new Usuario
                {
                    IdUsuario = usuarioCTX.IdUsuario,
                    NmCompleto = usuarioCTX.NmCompleto
                });
            }
            else
            {
                usuarios = _usuarioAppServico.ObterFuncionalidadesPorUsuario(idSite, idFuncao, idProcesso, idUsuario);
            }

            return Json(new { Lista = usuarios }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObterUsuariosPorFuncionalidade(int idProcesso, int idSite, int idFuncionalidade)
        {
            //int idSite = 5;
            //int idFuncionalidade = 14;
            //int idProcesso = 21;
            var usuarios = new List<Usuario>();
            if (Util.ObterPerfilUsuarioLogado() == (byte)PerfisAcesso.Coordenador)
            {
                var usuarioCTX = _usuarioAppServico.GetById(Util.ObterCodigoUsuarioLogado());

                usuarios.Add(new Usuario
                {
                    IdUsuario = usuarioCTX.IdUsuario,
                    NmCompleto = usuarioCTX.NmCompleto
                });
            }
            else
            {
                usuarios.AddRange(_usuarioAppServico.ObterUsuariosPorFuncionalidade(idProcesso, idSite, idFuncionalidade));
            }

            return Json(new { Lista = usuarios }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObterUsuariosPorFuncaoSiteEProcesso(int ? idProcesso= null, int? idSite = null, int? idFuncao = null)
        {
            var usuarios = new List<Usuario>();

            usuarios.AddRange(_usuarioAppServico.ObterUsuariosPorFuncao(idProcesso, idSite, idFuncao));

            var usuariosLista = usuarios.Select(x => new { x.IdUsuario, x.NmCompleto }).ToList();

            return Json(new { StatusCode = HttpStatusCode.OK, Lista = usuariosLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObterUsuariosMigracao(int IdSite, int IdUsuario)
        {
            var usuarios = new List<Usuario>();

            Usuario usuario = _usuarioAppServico.GetById(IdUsuario);
            
            usuarios = _usuarioAppServico.Get(x => x.IdUsuario != IdUsuario && x.UsuarioClienteSites.Select(y=> y.IdSite).Contains(IdSite)).Where(x=> (VerificaCargos(x, usuario) || x.IdPerfil == (int)PerfisAcesso.Coordenador)).ToList();
            
            var usuariosLista = usuarios.Select(x => new { x.IdUsuario, x.NmCompleto }).ToList();

            return Json(new { StatusCode = HttpStatusCode.OK, Lista = usuariosLista }, JsonRequestBehavior.AllowGet);
        }

        private bool VerificaCargos(Usuario usuarioQuery, Usuario usuarioAtual)
        {
            bool retorno = true;

            usuarioAtual.UsuarioCargoes.Select(y => y.IdCargo).ToList().ForEach(cargo =>
            {
                if (!usuarioQuery.UsuarioCargoes.Select(x=> x.IdCargo).Contains(cargo))
                {
                    retorno = false;
                }                    
            });

            return retorno;
        }

        [HttpGet]
        public JsonResult ObterUsuariosPorFuncao(int ? idSite = null, int ? idFuncao = null)
        {
            var usuarios = new List<Usuario>();

            var perfil = Util.ObterPerfilUsuarioLogado();

            usuarios.AddRange(_usuarioAppServico.ObterUsuariosPorFuncao(null, idSite, idFuncao));

            var usuariosLista = usuarios.Select(x => new { x.IdUsuario, x.NmCompleto });

            return Json(new { StatusCode = HttpStatusCode.OK, Lista = usuariosLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObterUsuariosPorPerfilESite(int idSite, int idPerfil)
        {
            var usuarios = new List<Usuario>();

            var perfil = Util.ObterPerfilUsuarioLogado();

            usuarios.AddRange(_usuarioAppServico.ObterUsuariosPorPerfilESite(idSite, idPerfil, perfil));

            var usuariosLista = usuarios.Select(x => new { x.IdUsuario, x.NmCompleto });

            return Json(new { StatusCode = HttpStatusCode.OK, Lista = usuariosLista }, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult ObterUsuarioLogado()
        {
            var idUsuarioLogado = Util.ObterCodigoUsuarioLogado();
            var usuarios = new List<Usuario>();

            var usuario = _usuarioAppServico.GetById(idUsuarioLogado);

            usuarios.Add(new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                NmCompleto = usuario.NmCompleto
            });

            return Json(new { StatusCode = HttpStatusCode.OK, Lista = UsuarioAppServico.RetiraDuplicado(usuarios) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AlterarSenha()
        {
            var idUsuario = Util.ObterCodigoUsuarioLogado();

            var usuario = _usuarioAppServico.GetById(idUsuario);

            int perfil = Util.ObterPerfilUsuarioLogado();
            int idCliente = Util.ObterClienteSelecionado();

            Cliente cliente = _clienteServico.ObterClientesPorUsuario(idUsuario).FirstOrDefault();

            int? nuDiasTrocaSenha = (usuario.IdPerfil == (int)PerfisAcesso.Administrador || usuario.IdPerfil == (int)PerfisAcesso.Suporte) ? 30 : cliente.NuDiasTrocaSenha;

            if (DiasParaTrocaDeSenha(usuario.DtAlteracaoSenha.Value, DateTime.Now) >= nuDiasTrocaSenha)
            {
                ViewBag.SenhaExpirada = true;
            }

            ViewBag.UtilizaSenhaForte = (usuario.IdPerfil == (int)PerfisAcesso.Coordenador || usuario.IdPerfil == (int)PerfisAcesso.Colaborador) ? usuario.UsuarioClienteSites.Any(x => x.Cliente.FlExigeSenhaForte).ToString() : "true";

            if (perfil != (int)PerfisAcesso.Administrador)
            {
                ViewBag.UtilizaSenhaForte = usuario.UsuarioClienteSites.FirstOrDefault(x => x.IdCliente == idCliente).Cliente.FlExigeSenhaForte.ToString().ToLower();
            }

            return View(usuario);
        }

        [HttpPost]
        public JsonResult AlterarSenha(Usuario Usuario, string NovaSenha, string ConfirmaSenha)
        {
            List<string> erros = new List<string>();

            Usuario.CdSenha = UtilsServico.Sha1Hash(Usuario.CdSenha);

            _usuarioAppServico.PodeAlterarSenha(Usuario, ref erros);

            int SenhasUsadas = 0;
            int NuArmazenaSenha = 0;

            if (Usuario.IdPerfil == (int)PerfisAcesso.Coordenador || Usuario.IdPerfil == (int)PerfisAcesso.Colaborador)
            {
                int idCliente = Util.ObterClienteSelecionado();
                Cliente cliente = _clienteServico.ObjterClienteById(idCliente);
                NuArmazenaSenha = cliente.NuArmazenaSenha.Value;
                SenhasUsadas = _usuarioSenhaServico.ListaUltimasSenhas(Usuario.IdUsuario, cliente.NuArmazenaSenha.Value).Where(x => x.CdSenha == Usuario.CdSenha).Count();
            }
            else
            {
                NuArmazenaSenha = 6;
                SenhasUsadas = _usuarioSenhaServico.ListaUltimasSenhas(Usuario.IdUsuario, NuArmazenaSenha).Where(x => x.CdSenha == Usuario.CdSenha).Count();
            }

            if (SenhasUsadas > 0)
            {
                erros.Add(Traducao.Usuario.ResourceUsuario.Usuario_Senha_NovaSenhaDeveSerDiferente.Replace("@UltimasSenhas@", NuArmazenaSenha.ToString()));
            }

            if (erros.Count == 0)
            {
                _usuarioAppServico.AlterarSenha(Usuario.IdUsuario, NovaSenha);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Usuario.ResourceUsuario.AlterarSenha_msg_Senha_valid_change }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult MeusDados()
        {

            ViewBag.IdCliente = Util.ObterClienteSelecionado();
            var erros = new List<string>();

            try
            {
                var idUsuario = Util.ObterCodigoUsuarioLogado();

                var usuario = _usuarioAppServico.GetById(idUsuario);

                return View(usuario);
            }
            catch (Exception ex)
            {
                erros.Add(Traducao.Resource.MsgErroContatoADM);

                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AtualizarMeusDados(Usuario usuario)
        {
            usuario.FotoPerfilAux.Tratar();

            var erros = new List<string>();

            try
            {

                if (usuario.NuCPF != null)
                {
                    usuario.NuCPF = RetornaApenasNumeros(usuario.NuCPF);
                }

                usuario.IdPerfil = Util.ObterPerfilUsuarioLogado();

                _usuarioServico.ValidoAtualizarMeusDados(usuario, ref erros);
                
                if (erros.Count > 0)
                {
                    return Json(new { StatusCode = 505, Erros = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (usuario.FotoPerfilAux.IdAnexo == 0)
                    {
                        _anexoAppServico.Add(usuario.FotoPerfilAux);
                        _usuarioAnexoAppServico.Add(new UsuarioAnexo
                        {
                            IdUsuario = usuario.IdUsuario,
                            Anexo = usuario.FotoPerfilAux
                        });
                    }
                    else
                    {
                        var anexoCtx = _anexoAppServico.GetById(usuario.FotoPerfilAux.IdAnexo);
                        anexoCtx.Nome = usuario.FotoPerfilAux.Nome;
                        anexoCtx.Extensao = usuario.FotoPerfilAux.Extensao;
                        anexoCtx.Arquivo = usuario.FotoPerfilAux.Arquivo;
                        _anexoAppServico.Update(anexoCtx);

                    }
                    Session["usuarioImg"] = String.Format("data:image/" + usuario.FotoPerfilAux.Extensao + ";base64," + Convert.ToBase64String(usuario.FotoPerfilAux.Arquivo));
                    usuario.DtAlteracao = DateTime.Now;
                    _usuarioAppServico.AtualizarMeusDados(usuario);
                }

            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Usuario.ResourceUsuario.Usuario_msg_editar_valid }, JsonRequestBehavior.AllowGet);

        }

        [AcessoUsuarioSite]
        public ActionResult AtivaInativa(int idUsuario)
        {

            var erros = new List<string>();

            var resposta = _usuarioAppServico.AtivarInativar(idUsuario);

            if (!resposta)
            {
                return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Usuario.ResourceUsuario.Usuario_msg_icone_ativo_valid }, JsonRequestBehavior.AllowGet);
        }

        [AcessoUsuarioSite]
        public ActionResult BloqueiaDesbloqueia(int idUsuario)
        {

            var erros = new List<string>();

            var resposta = _usuarioAppServico.BloqueiaDesbloqueia(idUsuario);

            if (!resposta)
            {
                return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Usuario.ResourceUsuario.Usuario_msg_icone_bloqueia_valid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecebeNaoRecebeEmail(int idUsuario)
        {

            var erros = new List<string>();

            var resposta = _usuarioAppServico.RecebeNaoRecebeEmail(idUsuario);

            if (!resposta)
            {
                return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Usuario.ResourceUsuario.Usuario_msg_icone_email_valid }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[AcessoAdmin]
        public JsonResult Excluir(int id, int idUsuarioMigracao)
        {
            var erros = new List<string>();

            try
            {
                _usuarioServico.Excluir(id, idUsuarioMigracao);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_erro_ExcluirCargo);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Cargo.ResourceCargo.Cargo_msg_exluir }, JsonRequestBehavior.AllowGet);
        }

        public int DiasParaTrocaDeSenha(DateTime data1, DateTime data2)
        {
            return (data1 - data2).Days * -1;
        }

    }
}
