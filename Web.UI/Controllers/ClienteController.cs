using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Net;
using Dominio.Enumerado;
using Dominio.Entidade;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using Web.UI.Helpers;
using ApplicationService.Interface;
using Dominio.Interface.Servico;
using Dominio.Servico;
using System.Linq;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class ClienteController : BaseController
    {

        private readonly IClienteAppServico _clienteAppServico;
        private readonly IClienteServico _clienteServico;

        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IUsuarioServico _usuarioServico;
        private readonly IAnexoAppServico _anexoAppServico;
        private readonly ISiteModuloAppServico _siteModuloAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteSiteAppServico;
        private readonly ISiteAppServico _siteAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IClienteContratoAppServico _clienteContratoAppServico;
        private readonly IFuncionalidadeAppServico _funcionalidadeAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IPendenciaAppServico _pendenciaAppServico;

        public ClienteController(IClienteAppServico clienteAppServico,
                                 IUsuarioAppServico usuarioAppServico,
                                 IUsuarioServico usuarioServico,
                                 ISiteModuloAppServico siteModuloAppServico,
                                 IProcessoAppServico processoAppServico,
                                 IUsuarioClienteSiteAppServico usuarioClienteSiteAppServico,
                                 ISiteAppServico siteAppServico,
                                 ILogAppServico logAppServico,
                                 IAnexoAppServico anexoAppServico,
                                 IClienteContratoAppServico clienteContratoAppServico,
                                 IClienteServico clienteServico,
                                 IFuncionalidadeAppServico funcionalidadeAppServico,
                                 IPendenciaAppServico pendenciaAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico, pendenciaAppServico)
        {
            _clienteAppServico = clienteAppServico;
            _usuarioAppServico = usuarioAppServico;
            _usuarioServico = usuarioServico;
            _siteModuloAppServico = siteModuloAppServico;
            _processoAppServico = processoAppServico;
            _usuarioClienteSiteAppServico = usuarioClienteSiteAppServico;
            _siteAppServico = siteAppServico;
            _logAppServico = logAppServico;
            _clienteServico = clienteServico;
            _anexoAppServico = anexoAppServico;
            _clienteContratoAppServico = clienteContratoAppServico;
            _funcionalidadeAppServico = funcionalidadeAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        [HttpGet]
        [AcessoCliente]
        public ActionResult Index()
        {
            var clientes = new List<Cliente>();
            var idPerfil = Util.ObterPerfilUsuarioLogado();
            var idUsuarioLogado = Util.ObterCodigoUsuarioLogado();

            if (idPerfil == (int)PerfisAcesso.Administrador)
            {
                clientes = _clienteAppServico.GetAll().ToList();
            }
            else if (idPerfil == (int)PerfisAcesso.Suporte)
            {
                var logado = _usuarioAppServico.GetById(idUsuarioLogado);
                logado.UsuarioClienteSites.ToList().ForEach(x =>
                {
                    clientes = _usuarioClienteSiteAppServico.Get(y => y.IdCliente == x.IdCliente).Select(d => d.Cliente).Distinct().ToList();
                });
            }

            return View(clientes);
        }

        [HttpGet]
        public ActionResult ObterClientesUsuario()
        {
            var clientes = _clienteAppServico.ObterClientesPorUsuario(Util.ObterCodigoUsuarioLogado());
            var listaJson = new List<Cliente>();

            foreach (var cliente in clientes)
            {

                Anexo clienteLogoAux = new Anexo();

                if (cliente.ClienteLogo != null)
                {
                    clienteLogoAux = cliente.ClienteLogo.FirstOrDefault().Anexo;

                    clienteLogoAux.ArquivoB64 = Convert.ToBase64String(clienteLogoAux.Arquivo);
                    clienteLogoAux.Arquivo = null;
                    clienteLogoAux.ClientesContratos = new List<ClienteContrato>();
                    clienteLogoAux.ClientesLogo = new List<ClienteLogo>();
                }

                listaJson.Add(new Cliente()
                {
                    IdCliente = cliente.IdCliente,
                    NmFantasia = cliente.NmFantasia,
                    ClienteLogoAux = clienteLogoAux,
                });

            }

            var json = JsonConvert.SerializeObject(listaJson, Formatting.Indented);

            return new ContentResult
            {
                Content = json,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        }

        [HttpGet]
        [AcessoAdmin]
        public ActionResult Criar()
        {
            var cliente = new Cliente();
            ViewBag.Funcionalidades = _funcionalidadeAppServico.Get(x => x.Ativo == true);
            return View(cliente);
        }

        [HttpGet]
        [AcessoAdmin]
        public ActionResult Editar(int id)
        {
            var cliente = _clienteAppServico.GetById(id);
            ViewBag.Funcionalidades = _funcionalidadeAppServico.Get(x => x.Ativo == true);
            ViewBag.PossuiSite = _siteAppServico.Get(x => x.IdCliente == cliente.IdCliente).Count();
            cliente.ContratosAux.AddRange(cliente.Contratos.Select(x => x.Contrato));
            return View("Criar", cliente);
        }

        [HttpGet]
        public ActionResult ObterClientes()
        {
            try
            {
                var clientesCTX = _clienteAppServico.GetAll();
                var clientes = new List<Cliente>();

                foreach (var cliente in clientesCTX)
                {
                    clientes.Add(new Cliente
                    {
                        IdCliente = cliente.IdCliente,
                        NmFantasia = cliente.NmFantasia
                    });
                }

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Lista = clientes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new
                {
                    StatusCode = 500,
                    Erro = ex
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AcessoAdmin]
        public JsonResult AtivaInativa(int id)
        {

            var erros = new List<string>();

            var resposta = _clienteAppServico.AtivarInativar(id);

            if (!resposta)
            {
                return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Cliente.ResourceCliente.Cliente_msg_icone_ativo_valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AcessoAdmin]
        public JsonResult Criar(Cliente cliente)
        {
            var erros = new List<string>();

            try
            {
                TrataDadosCriacaoCliente(cliente);

                _clienteServico.ValidaCriacao(cliente, ref erros);

                if (erros.Count != 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TrataRelacionamentoAnexo(cliente);

                    if (cliente.Site.SiteFuncionalidades.Where(x => x.IdFuncionalidade == 2).Count() > 0)
                    {
                        SiteFuncionalidade workflow = new SiteFuncionalidade();
                        workflow.IdFuncionalidade = 13;
                        cliente.Site.SiteFuncionalidades.Add(workflow);
                    }

                    _clienteAppServico.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            cliente.Usuario.CdSenha = cliente.Usuario.SenhaAtual;

            try
            {

                _usuarioAppServico.EnviaEmailNovoUsuario(cliente.Usuario);

                return Json(new { StatusCode = 200, Success = Traducao.Cliente.ResourceCliente.Cliente_msg_criar_valid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { StatusCode = 200, Success = Traducao.Shared.ResourceMensagens.Mensagem_erro_AoEnviarEmailNovoUsuario }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [AcessoAdmin]
        public JsonResult Excluir(int id)
        {
            var erros = new List<string>();

            try
            {
                bool returno = _clienteServico.Excluir(id);

                if (!returno)
                {
                    erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_erro_ExcluirClienteVerifique);
                    return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_erro_ExcluirCliente);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Cliente.ResourceCliente.Cliente_msg_exluir }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AcessoAdmin]
        public JsonResult Editar(Cliente cliente)
        {
            var erros = new List<string>();

            try
            {
                TrataAnexosEdicao(cliente);

                _clienteServico.ValidaEdicao(cliente, ref erros);
                if (erros.Count > 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AtualizaAnexo(cliente);
                    AtualizaContratos(cliente);
                    _clienteAppServico.Update(cliente);
                }

            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Cliente.ResourceCliente.Cliente_msg_editar_valid }, JsonRequestBehavior.AllowGet);
        }

        private void TrataRelacionamentoAnexo(Cliente cliente)
        {
            if (cliente.Usuario.FotoPerfilAux != null)
            {
                cliente.Usuario.FotoPerfil.Add(new UsuarioAnexo
                {
                    Usuario = cliente.Usuario,
                    Anexo = cliente.Usuario.FotoPerfilAux
                });
            }
            cliente.ClienteLogo.Add(new ClienteLogo
            {
                Cliente = cliente,
                Anexo = cliente.ClienteLogoAux
            });

            cliente.Site.SiteAnexo.Add(new SiteAnexo
            {
                Site = cliente.Site,
                Anexo = cliente.Site.SiteLogoAux
            });
        }

        private void AtualizaContratos(Cliente cliente)
        {
            var listaCtx = _clienteContratoAppServico.Get(x => x.IdCliente == cliente.IdCliente).ToList();
            if (cliente.ContratosAux.Count > 0 || listaCtx.Count > 0)
            {
                var listaDosQueSeraoAdicionados = cliente.ContratosAux.Where(x => x.IdAnexo == 0).ToList();

                var listaQueSeraDeletados = listaCtx.Select(b => b.IdAnexo)
                                                .Except(cliente.ContratosAux
                                                .Select(x => x.IdAnexo)).ToList();

                if (listaDosQueSeraoAdicionados.Count > 0)
                {
                    listaDosQueSeraoAdicionados.ForEach(itemAdd =>
                    {
                        _anexoAppServico.Add(itemAdd);

                        _clienteContratoAppServico.Add(new ClienteContrato
                        {
                            IdAnexo = itemAdd.IdAnexo,
                            IdCliente = cliente.IdCliente
                        });
                    });
                }

                if (listaQueSeraDeletados.Count > 0)
                {
                    listaQueSeraDeletados.ForEach(itemRemove =>
                    {
                        var deletado = listaCtx.FirstOrDefault(x => x.IdAnexo == itemRemove).Contrato;
                        //_anexoAppServico.Remove(deletado);
                        _clienteContratoAppServico.Remove(deletado.ClientesContratos.FirstOrDefault(x => x.IdAnexo == deletado.IdAnexo));

                    });
                }


            }
        }

        private void AtualizaAnexo(Cliente cliente)
        {
            var anexoCtx = _anexoAppServico.GetById(cliente.ClienteLogoAux.IdAnexo);
            anexoCtx.Nome = cliente.ClienteLogoAux.Nome;
            anexoCtx.Extensao = cliente.ClienteLogoAux.Extensao;
            anexoCtx.Arquivo = cliente.ClienteLogoAux.Arquivo;
            _anexoAppServico.Update(anexoCtx);
        }

        public void EscolheCliente(int idCliente)
        {

            var clienteJSON = string.Empty;
            try
            {
                HttpCookie cookie = Request.Cookies["clienteSelecionado"];

                var clienteCTX = _clienteAppServico.GetById(idCliente);

                if (cookie != null)
                {
                    cookie.Value = clienteCTX.IdCliente.ToString();

                    cookie.Expires = DateTime.Now.AddDays(1);

                    Response.Cookies.Set(cookie);
                }
                else {
                    cookie = new HttpCookie("clienteSelecionado");
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Set(cookie);

                }





            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }

            //return Json(new { StatusCode=(int)HttpStatusCode.OK, Data = clienteJSON });
        }

        private void TrataDadosCriacaoCliente(Cliente cliente)
        {

            TrataAnexosCriacao(cliente);
            TrataNovoUsuario(cliente);
            RemoveMascara(cliente);
            ConstroiRelacionamentos(cliente);
        }

        private void RemoveMascara(Cliente cliente)
        {
            cliente.Site.NuCNPJ = RetornaApenasNumeros(cliente.Site.NuCNPJ);
            if (cliente.Usuario.NuCPF != null)
            {
                cliente.Usuario.NuCPF = RetornaApenasNumeros(cliente.Usuario.NuCPF);
            }
        }

        private void Retorna(Cliente cliente)
        {
            cliente.Site.NuCNPJ = RetornaApenasNumeros(cliente.Site.NuCNPJ);
            cliente.Usuario.NuCPF = RetornaApenasNumeros(cliente.Usuario.NuCPF);
        }

        private void TrataNovoUsuario(Cliente cliente)
        {
            var senha = Util.GeraNovaSenha();
            var senhaCriptografada = UtilsServico.Sha1Hash(senha);

            cliente.Usuario.IdPerfil = (int)PerfisAcesso.Coordenador;
            cliente.Usuario.CdSenha = senhaCriptografada;
            cliente.Usuario.DtExpiracao = Convert.ToDateTime(cliente.Usuario.DtExpiracao);
            cliente.Usuario.DtInclusao = DateTime.Now;
            cliente.Usuario.SenhaAtual = senha;
        }

        private void ConstroiRelacionamentos(Cliente cliente)
        {
            cliente.UsuarioClienteSites.Add(new UsuarioClienteSite
            {
                Cliente = cliente,
                Usuario = cliente.Usuario,
                Site = cliente.Site
            });
            if (cliente.ContratosAux.Count > 0)
            {
                cliente.ContratosAux.ForEach(contrato =>
                {
                    cliente.Contratos.Add(new ClienteContrato
                    {
                        Cliente = cliente,
                        Contrato = contrato
                    });


                });
            }
        }

        private void TrataAnexosCriacao(Cliente cliente)
        {
            cliente.ClienteLogoAux.Tratar();
            cliente.Site.SiteLogoAux.Tratar();
            cliente.Site.FlAtivo = true;

            if (cliente.ContratosAux.Count > 0)
            {
                cliente.ContratosAux.ForEach(contrato => contrato.Tratar());
            }

            if (cliente.Usuario.FotoPerfilAux.ArquivoB64 != null)
            {
                cliente.Usuario.FotoPerfilAux.Tratar();
            }
            else
            {
                cliente.Usuario.FotoPerfilAux = null;
            }
        }

        private void TrataAnexosEdicao(Cliente cliente)
        {
            cliente.ClienteLogoAux.Tratar();

            if (cliente.ContratosAux.Count > 0)
            {
                cliente.ContratosAux.ForEach(contrato => contrato.Tratar());
            }
        }
    }
}