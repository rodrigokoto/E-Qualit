using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.UI.Models;
using Dominio.Entidade;
using System.Text.RegularExpressions;
using Dominio.Enumerado;
using System.Collections.Generic;
using Newtonsoft.Json;
using ApplicationService.Interface;
using Dominio.Servico;
using Dominio.Interface.Servico;
using Web.UI.Helpers;
using ApplicationService.Entidade;

namespace Web.UI.Controllers
{
    public class LoginController : BaseController
    {

        private readonly IClienteAppServico _clienteAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;

        private readonly ILoginAppServico _loginAppServico;
        private readonly ILoginServico _loginServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteSiteAppServico;

        private readonly ILogAppServico _logAppServico;

        private readonly ISiteModuloAppServico _siteModulo;
        private readonly IUsuarioCargoAppServico _usuarioCargoAppServico;
        
        public LoginController(IClienteAppServico clienteAppServico, IUsuarioAppServico usuarioAppServico,
                               ILogAppServico logAppServico,
                               ILoginServico loginServico,
                               ILoginAppServico loginAppServico,
                               ISiteModuloAppServico siteModulo,
                               IPendenciaAppServico pendenciaAppServico,
                               IUsuarioClienteSiteAppServico usuarioClienteSiteAppServico,
                               IUsuarioCargoAppServico usuarioCargoAppServico) : base(logAppServico, null, null, null, pendenciaAppServico)
        {
            _clienteAppServico = clienteAppServico;
            _usuarioAppServico = usuarioAppServico;
            _loginAppServico = loginAppServico;
            _loginServico = loginServico;
            _usuarioClienteSiteAppServico = usuarioClienteSiteAppServico;
            _logAppServico = logAppServico;
            _siteModulo = siteModulo;
            _usuarioCargoAppServico = usuarioCargoAppServico;
        }


        public ActionResult Index(string rotaDoCliente)
        {
            var clienteCtx = _clienteAppServico.Get(x => x.NmUrlAcesso == rotaDoCliente).FirstOrDefault();

            if (clienteCtx != null)
            {
                TrataImg(clienteCtx);

            }

            return View(clienteCtx);
        }

        public JsonResult Acesso(Usuario usuario)
        {
            var erros = new List<string>();

            try
            {
                _loginServico.ValidoParaLogar(usuario, ref erros);

                if (erros.Count == 0)
                {
                    var usuarioCTX = _usuarioAppServico.ObterUsuarioPorIdeSenha(usuario.CdIdentificacao , usuario.CdSenha);

                    _loginServico.ValidoParaAcessar(usuarioCTX, ref erros);
                    _usuarioClienteSiteAppServico.EmpresaValidaLogin(usuarioCTX, ref erros);



                    if (erros.Count == 0)
                    {
                        usuarioCTX.DtUltimoAcesso = DateTime.Now;
                        usuarioCTX.NuFalhaLNoLogin = 0;

                        _usuarioAppServico.Update(usuarioCTX);

                        if (usuarioCTX.Token != null)
                        {
                            erros.Add(Traducao.Login.ResourceLogin.AlterarSenha_msg_erro_token_ativo);
                            return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            ContrutorCookie(usuarioCTX);
                        }
                    }
                    else
                    {

                        if (erros.FirstOrDefault().Equals(Traducao.Resource.VerifiqueSenha))
                        {
                            if (usuarioCTX.IdPerfil != (int)PerfisAcesso.Administrador)
                            {
                                var cliente = _clienteAppServico.GetById(usuarioCTX.UsuarioClienteSites.FirstOrDefault().IdCliente);

                                if (cliente.NuTentativaBloqueioLogin.HasValue)
                                {
                                    int Bloqueio = cliente.NuTentativaBloqueioLogin.Value;

                                    if (usuarioCTX.NuFalhaLNoLogin >= Bloqueio)
                                    {
                                        usuarioCTX.FlBloqueado = true;
                                    }
                                    else
                                    {
                                        usuarioCTX.NuFalhaLNoLogin++;
                                    }

                                    _usuarioAppServico.Update(usuarioCTX);
                                }
                            }
                        }

                        return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var usuarioCTX = _usuarioAppServico.ObterUsuarioPorCdIdentificacao(usuario.CdIdentificacao);

                    if (erros.FirstOrDefault().Equals(Traducao.Resource.login_usuario_senha_invalido))
                    {
                        if (usuarioCTX.IdPerfil != (int)PerfisAcesso.Administrador)
                        {
                            var cliente = _clienteAppServico.GetById(usuarioCTX.UsuarioClienteSites.FirstOrDefault().IdCliente);

                            if (cliente.NuTentativaBloqueioLogin.HasValue)
                            {
                                int Bloqueio = cliente.NuTentativaBloqueioLogin.Value;

                                if (usuarioCTX.NuFalhaLNoLogin >= Bloqueio)
                                {
                                    usuarioCTX.FlBloqueado = true;
                                }
                                else
                                {
                                    usuarioCTX.NuFalhaLNoLogin++;
                                }

                                _usuarioAppServico.Update(usuarioCTX);
                            }
                        }
                        else
                        {
                            if (usuarioCTX.NuFalhaLNoLogin >= 3)
                            {
                                usuarioCTX.FlBloqueado = true;
                            }
                            else
                            {
                                usuarioCTX.NuFalhaLNoLogin++;
                            }

                            _usuarioAppServico.Update(usuarioCTX);

                        }
                    }

                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                erros.Add(Traducao.Resource.MsgErroContatoADM);

                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);

        }



        private void TrataImg(Cliente cliente)
        {
            if (cliente.ClienteLogo.Count > 0)
            {
                var anexo = cliente.ClienteLogo.FirstOrDefault().Anexo;
                cliente.ClienteLogoAux = new Dominio.Entidade.Anexo
                {
                    ArquivoB64 = String.Format("data:image/" + anexo.Extensao + ";base64," + Convert.ToBase64String(anexo.Arquivo))
                };
            }

        }


        [VerificaIntegridadeLogin]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            RemoveTodosCookies();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult RecuperarSenha()
        {
            return View();
        }

        public ActionResult AlterarSenha(Guid token)
        {
            ViewBag.IdUsuario = 0;
            ViewBag.UtilizaSenhaForte = "false";

            var user = _usuarioAppServico.Get(x => x.Token == token).FirstOrDefault();
            if (user != null)
            {
                ViewBag.IdUsuario = user.IdUsuario;
                ViewBag.UtilizaSenhaForte = (user.IdPerfil == (int)PerfisAcesso.Coordenador && user.IdPerfil == (int)PerfisAcesso.Colaborador) ? user.UsuarioClienteSites.Any(x => x.Cliente.FlExigeSenhaForte).ToString() : "true";
                user.Token = null;
                _usuarioAppServico.Update(user);

                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarSenha(Usuario usuario)
        {

            var erros = new List<string>();
            try
            {
                _loginServico.ValidoParaAlterarSenhaViaEmail(usuario, ref erros);

                if (erros.Count == 0)
                {
                    var user = _usuarioAppServico.GetById(usuario.IdUsuario);
                    user.CdSenha = UtilsServico.Sha1Hash(usuario.CdSenha);
                    user.DtAlteracaoSenha = DateTime.Now;

                    _usuarioAppServico.Update(user);
                }
                else
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);

                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
            RemoveTodosCookies();
            return Json(new { StatusCode = 200, Success = Traducao.Login.ResourceLogin.AlterarSenha_msg_Senha_valid_change }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarSenha(Usuario usuario)
        {

            var erros = new List<string>();

            try
            {
                _loginServico.ValidoParaEsqueciSenha(usuario, ref erros);

                if (erros.Count == 0)
                {
                    var usuarioCtx = _usuarioAppServico.Get(x => x.CdIdentificacao == usuario.CdIdentificacao).FirstOrDefault();
                    var novaSenha = UtilsServico.GeraNovaSenha();
                    usuarioCtx.Token = Guid.NewGuid();
                    usuarioCtx.DtAlteracaoSenha = DateTime.Now;
                    _usuarioAppServico.NovaSenhaRandomica(usuarioCtx, novaSenha);

                    usuarioCtx.CdSenha = novaSenha;
                    _usuarioAppServico.EnviaEmailEsqueciASenha(usuarioCtx, usuarioCtx.Token.Value);
                }
                else
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);

                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
            RemoveTodosCookies();
            return Json(new { StatusCode = 200, Success = Traducao.Login.ResourceLogin.RecuperaSenha_msg_Email_valid_recovery }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Lingua(string lingua)
        {
            Web.UI.Helpers.Cultura.AlterarTradutor(lingua);

            return RedirectToAction("Index");
        }

        public static bool ESenhaForte(string password)
        {
            int tamanhoMinimo = 8;
            int tamanhoMinusculo = 1;
            int tamanhoMaiusculo = 1;
            int tamanhoNumeros = 1;
            int tamanhoCaracteresEspeciais = 1;

            // Definição de letras minusculas
            Regex regTamanhoMinusculo = new Regex("[a-z]");

            // Definição de letras minusculas
            Regex regTamanhoMaiusculo = new Regex("[A-Z]");

            // Definição de letras minusculas
            Regex regTamanhoNumeros = new Regex("[0-9]");

            // Definição de letras minusculas
            Regex regCaracteresEspeciais = new Regex("[^a-zA-Z0-9]");

            // Verificando tamanho minimo
            if (password.Length < tamanhoMinimo) return false;

            // Verificando caracteres minusculos
            if (regTamanhoMinusculo.Matches(password).Count < tamanhoMinusculo) return false;

            // Verificando caracteres maiusculos
            if (regTamanhoMaiusculo.Matches(password).Count < tamanhoMaiusculo) return false;

            // Verificando numeros
            if (regTamanhoNumeros.Matches(password).Count < tamanhoNumeros) return false;

            // Verificando os diferentes
            if (regCaracteresEspeciais.Matches(password).Count < tamanhoCaracteresEspeciais) return false;

            return true;
        }

        public void ConstrutorMatrizDePermissao(Usuario usuario)
        {

            if (usuario.IdPerfil == (int)PerfisAcesso.Colaborador)
            {
                var usuarioCargo = _usuarioCargoAppServico
                                        .Get(x => x.IdUsuario == usuario.IdUsuario)
                                        .FirstOrDefault();

                var lista = usuarioCargo.Cargo.CargoProcessos.ToList();
                var listaCookie = new List<PermissoesApp>();

                lista.ForEach(cargoProcesso =>
                {

                    listaCookie.Add(new PermissoesApp
                    {
                        IdCargo = cargoProcesso.IdCargo,
                        IdFuncao = cargoProcesso.IdFuncao,
                        IdProcesso = cargoProcesso.IdProcesso

                    });
                });

                Session.Add("matrizPermissao", listaCookie);

                var json = JsonConvert.SerializeObject(listaCookie,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });

                var siteModulo = new HttpCookie("matrizPermissao", json)
                {
                    Expires = DateTime.Now.AddDays(1)
                };


                Response.Cookies.Add(siteModulo);
            }

        }

        public void ContrutorCookie(Usuario usuario)
        {
            ContrutorCookiePerfil(usuario);
            ContrutorCookieCliente(usuario);
            ContrutorCookieSite(usuario);
            ContrutorCookieSitesModulos();
            ContrutorProcessoCodigo();
            ContrutorProcessoNome();
            ConstrutorUsuario(usuario);
            ConstrutorMatrizDePermissao(usuario);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                                                                usuario.IdUsuario.ToString(),
                                                                                DateTime.Now,
                                                                                DateTime.Now.AddDays(1),
                                                                                false,
                                                                                "",
                                                                                FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);

            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

        private void ContrutorCookiePerfil(Usuario usuario)
        {
            var meuPerfil = new HttpCookie("meuPerfil")
            {
                Expires = DateTime.Now.AddDays(1)
            };

            meuPerfil.Value = UtilsServico.CriptografarString(usuario.IdPerfil.ToString());

            Response.Cookies.Add(meuPerfil);
        }

        private void ContrutorCookieCliente(Usuario usuario)
        {
            var clienteSelecionado = new HttpCookie("clienteSelecionado")
            {
                Expires = DateTime.Now.AddDays(1)
            };

            var cliente = new ClienteApp();


            if (usuario.IdPerfil == (int)PerfisAcesso.Coordenador || usuario.IdPerfil == (int)PerfisAcesso.Colaborador)
            {
                var clienteCTX = usuario.UsuarioClienteSites.FirstOrDefault().Cliente;

                cliente.IdCliente = clienteCTX.IdCliente;
                clienteSelecionado.Value = cliente.IdCliente.ToString();
            }
            else
            {
                clienteSelecionado.Value = 0.ToString();
            }

            Response.Cookies.Add(clienteSelecionado);

        }

        private void ContrutorCookieSite(Usuario usuario)
        {

            var siteSelecionado = new HttpCookie("siteSelecionado")
            {
                Expires = DateTime.Now.AddDays(1)
            };
            var usuarioClienteSite = usuario.UsuarioClienteSites.FirstOrDefault();

            siteSelecionado.Value = usuarioClienteSite != null ? usuarioClienteSite.IdSite.ToString() : null;
            Response.Cookies.Add(siteSelecionado);
        }

        private void ContrutorCookieSitesModulos()
        {
            var siteFuncionalidadesCTX = _siteModulo.GetAll();

            var siteFuncionalidades = _siteModulo.RetirarReferenciaCircularDaLista(siteFuncionalidadesCTX);

            string siteModulosJson = JsonConvert.SerializeObject(siteFuncionalidades, Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });

            var siteModulo = new HttpCookie("siteModulos", siteModulosJson)
            {
                Expires = DateTime.Now.AddDays(1)
            };

            Response.Cookies.Add(siteModulo);
            //Response.Cookies.Add(modulo);

        }

        private void ContrutorProcessoCodigo()
        {
            var processoSelecionadoCodigo = new HttpCookie("processoSelecionadoCodigo")
            {
                Expires = DateTime.Now.AddDays(1)
            };

            Response.Cookies.Add(processoSelecionadoCodigo);
        }

        private void ContrutorProcessoNome()
        {
            var processoSelecionadoNome = new HttpCookie("processoSelecionadoNome")
            {
                Expires = DateTime.Now.AddDays(1)
            };

            Response.Cookies.Add(processoSelecionadoNome);
        }

        private void ConstrutorUsuario(Usuario usuario)
        {

            var usuarioCTX = new
            {
                IdUsuario = UtilsServico.CriptografarString(usuario.IdUsuario.ToString()),
                Nome = UtilsServico.Criptografar(usuario.NmCompleto),
                //Foto = usuario.NomeArquivo
            };


            string usuarioJson = JsonConvert.SerializeObject(usuarioCTX, Formatting.None,
                 new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });

            var usuarioCookie = new HttpCookie("usuario", usuarioJson)
            {
                Expires = DateTime.Now.AddDays(1)
            };

            HttpContext.Response.Cookies.Add(usuarioCookie);
            if (usuario.FotoPerfil.Count > 0)
            {
                Session["usuarioImg"] = String.Format("data:image/" + usuario.FotoPerfil.FirstOrDefault().Anexo.Extensao + ";base64," + Convert.ToBase64String(usuario.FotoPerfil.FirstOrDefault().Anexo.Arquivo));

            }

        }


    }
}