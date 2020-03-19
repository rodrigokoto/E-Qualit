using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    [SitePossuiModulo(6)]
    public class AuditoriaController : BaseController
    {
        private readonly IPaiAppServico _paiAppServico;
        private readonly IPlaiAppServico _plaiAppServico;
        private readonly IPlaiProcessoNormaAppServico _plaiProcessoNormaAppServico;
        private readonly INormaAppServico _NormaAppServico;
        private readonly IAnexoAppServico _AnexoAppServico;
        private readonly IProcessoAppServico _processoAuditoriaSerico;
        //private readonly IEmailPlaiAppServico _emailPlaiAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IFilaEnvioServico _filaEnvioServico;
        private readonly IRegistroAuditoriaServico _registroAuditoriaServico;

        public AuditoriaController(IPaiAppServico paiAppServico, IPlaiAppServico plaiAppServico, IPlaiProcessoNormaAppServico plaiProcessoNormaAppServico, INormaAppServico normaAppServico,
                            IAnexoAppServico anexoAppServico,
                            IProcessoAppServico processoAuditoriaSerico,/* IEmailPlaiAppServico emailPlaiAppServico,*/
                             ILogAppServico logAppServico,
                             IUsuarioAppServico usuarioAppServico,
                             IProcessoAppServico processoAppServico,
                             IFilaEnvioServico filaEnvio,
                             IRegistroAuditoriaServico registroAuditoria,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _paiAppServico = paiAppServico;
            _plaiAppServico = plaiAppServico;
            _plaiProcessoNormaAppServico = plaiProcessoNormaAppServico;
            _processoAuditoriaSerico = processoAuditoriaSerico;
            _NormaAppServico = normaAppServico;
            //_emailPlaiAppServico = emailPlaiAppServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _filaEnvioServico = filaEnvio;
            _AnexoAppServico = anexoAppServico;
            _registroAuditoriaServico = registroAuditoria;
        }

        public ActionResult Index(int? ano)
        {
            int idSite = Util.ObterSiteSelecionado();
            ViewBag.IdSite = idSite;
            ViewBag.ListaProcessos = _processoAuditoriaSerico.ListaProcessosPorSite(ViewBag.IdSite);

            List<Pai> pais = _paiAppServico.Get(x => x.IdSite == idSite).ToList();

            pais.ForEach(pai =>
            {
                pai.Plais = _plaiAppServico.Get(plai => plai.IdPai == pai.IdPai).ToList();

                pai.Plais.ForEach(plai =>
                {
                    plai.PlaiProcessoNorma = _plaiProcessoNormaAppServico.Get(plaiProcessoNorma => plaiProcessoNorma.IdPlai == plai.IdPlai).ToList();

                    foreach (var norma in plai.PlaiProcessoNorma)
                    {
                        if (norma.Processo != null)
                            norma.NomeProcesso = norma.Processo.Nome;
                    }

                });
            });

            return View(pais);
        }

        public void EnviarAlerta(List<Plai> plais)
        {
            //plais.ForEach(x => _emailPlaiAppServico.EnviarNotificacao(x));
        }

        public JsonResult ListarTodos()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var pais = _paiAppServico.GetAll();

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Pais = pais }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Excluir(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var pai = _paiAppServico.GetById(id);

            _paiAppServico.Remove(pai);

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Salvar(List<Pai> pais)
        {
            int IdSite = Util.ObterSiteSelecionado();

            var erros = new List<string>();

            try
            {

                if (erros.Count > 0)
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);


                foreach (var pai in pais)
                {
                    pai.IdSite = Util.ObterSiteSelecionado();

                    if (pai.IdPai == 0)
                    {
                        pai.IdSite = Util.ObterSiteSelecionado();
                        pai.DataCadastro = DateTime.Now;
                        _paiAppServico.Add(pai);
                    }
                    else
                    {
                        var paiUpdate = _paiAppServico.GetById(pai.IdPai);
                        paiUpdate.IdGestor = pai.IdGestor;
                        _paiAppServico.Update(paiUpdate);
                    }

                    pai.Plais.ForEach(plaiNovo =>
                    {

                        Plai plai = _plaiAppServico.Get(x => x.Mes == plaiNovo.Mes && x.IdPai == pai.IdPai).FirstOrDefault();

                        if (plai != null)
                        {
                            int[] processosPlaiAtual = plai.PlaiProcessoNorma.Select(x => x.IdProcesso).ToArray();
                            int[] processosPlaiNovo = plaiNovo.PlaiProcessoNorma != null ? plaiNovo.PlaiProcessoNorma.Select(x => RetornaProcessoPorNome(x.NomeProcesso.Trim())).ToArray() : null;

                            if (processosPlaiNovo != null)
                            {
                                foreach (int processo in processosPlaiNovo)
                                {
                                    if (!processosPlaiAtual.Contains(processo))
                                    {
                                        var normas = _NormaAppServico.Get(x => x.IdSite == IdSite).ToList();
                                        _NormaAppServico.Get(x => x.IdSite == IdSite && x.Ativo == true).ToList().ForEach(norma =>
                                        {
                                            PlaiProcessoNorma plaiProcessoNorma = new PlaiProcessoNorma();
                                            plaiProcessoNorma.IdPlai = plai.IdPlai;
                                            plaiProcessoNorma.Data = DateTime.Now;
                                            plaiProcessoNorma.IdNorma = norma.IdNorma;
                                            plaiProcessoNorma.IdProcesso = processo;

                                            _plaiProcessoNormaAppServico.Add(plaiProcessoNorma);
                                        });
                                    }
                                }

                                foreach (int processo in processosPlaiAtual)
                                {
                                    if (!processosPlaiNovo.Contains(processo))
                                    {
                                        List<PlaiProcessoNorma> plaiProcessoNormas = _plaiProcessoNormaAppServico.Get(x => x.IdProcesso == processo && x.IdPlai == plai.IdPlai).ToList();

                                        plaiProcessoNormas.ForEach(plaiProcessoNorma =>
                                        {
                                            _plaiProcessoNormaAppServico.Remove(plaiProcessoNorma);
                                        });


                                    }
                                }
                            }
                            else
                            {
                                foreach (int processo in processosPlaiAtual)
                                {

                                    List<PlaiProcessoNorma> plaiProcessoNormas = _plaiProcessoNormaAppServico.Get(x => x.IdProcesso == processo && x.IdPlai == plai.IdPlai && plai.Pai.Ano == pai.Ano).ToList();

                                    plaiProcessoNormas.ForEach(plaiProcessoNorma =>
                                    {
                                        _plaiProcessoNormaAppServico.Remove(plaiProcessoNorma);
                                    });
                                }
                            }
                        }
                        else
                        {
                            plaiNovo.IdPai = pai.IdPai;
                            plaiNovo.DataCadastro = DateTime.Now;
                            plaiNovo.IdElaborador = pai.IdGestor;
                            plaiNovo.IdRepresentanteDaDirecao = pai.IdGestor;
                            plaiNovo.DataReuniaoAbertura = DateTime.Now;
                            plaiNovo.DataReuniaoEncerramento = DateTime.Now;

                            if (plaiNovo.PlaiProcessoNorma != null)
                            {
                                List<PlaiProcessoNorma> novoPlaiProcessoNorma = new List<PlaiProcessoNorma>();

                                plaiNovo.PlaiProcessoNorma.ForEach(plaiProcessoNorma =>
                                {

                                    _NormaAppServico.Get(x => x.IdSite == IdSite && x.Ativo == true).ToList().ForEach(norma =>
                                    {
                                        plaiProcessoNorma.IdPlai = plaiNovo.IdPlai;
                                        plaiProcessoNorma.Data = DateTime.Now;
                                        plaiProcessoNorma.IdNorma = norma.IdNorma;
                                        plaiProcessoNorma.IdProcesso = RetornaProcessoPorNome(plaiProcessoNorma.NomeProcesso);


                                        novoPlaiProcessoNorma.Add(plaiProcessoNorma);
                                    });
                                });

                                plaiNovo.PlaiProcessoNorma = novoPlaiProcessoNorma;
                            }

                            plai = plaiNovo;

                        }

                        TratarAnexos(plaiNovo, plai);


                        //if (plai.ArquivoPlai == null)
                        //{


                        //	List<ArquivoPlaiAnexo> arqPlai = new List<ArquivoPlaiAnexo>();
                        //	ArquivoPlaiAnexo arquiAnexo = new ArquivoPlaiAnexo();
                        //	arquiAnexo.IdArquivoPlaiAnexo = 0;
                        //	arquiAnexo.IdPlai = plai.IdPlai;
                        //	arquiAnexo.IdAnexo = 0;
                        //	arqPlai.Add(arquiAnexo);
                        //	plai.ArquivoPlai = arqPlai;
                        //}

                        if (plai.IdPlai == 0)
                        {
                            _plaiAppServico.Add(plai);
                        }
                        else
                        {
                            _plaiAppServico.Update(plai);
                        }
                    });
                }

                EnfileirarEmailAuditoria(pais);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(
                new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Success = Traducao.Auditoria.ResourceAuditoria.RegistroSalvoComSucesso
                }, JsonRequestBehavior.AllowGet);

        }

        private void TratarAnexos(Plai plaiNovo, Plai plaiAtual)
        {
            if (plaiNovo.ArquivoPlai == null)
                return;
            foreach (var esteArquivo in plaiNovo.ArquivoPlai)
            {
                if (esteArquivo.ApagarAnexo == 1)
                {
                    //apagamos deirtamente do anexo
                    //ninguem mais pode estar usando esse anexo
                    _AnexoAppServico.Remove(_AnexoAppServico.GetById(esteArquivo.IdAnexo));
                    continue;
                }

                if (esteArquivo == null)
                    continue;
                if (esteArquivo.Anexo == null)
                    continue;
                if (string.IsNullOrEmpty(esteArquivo.Anexo.Extensao))
                    continue;
                if (string.IsNullOrEmpty(esteArquivo.Anexo.ArquivoB64))
                    continue;

                Anexo anexoAtual = _AnexoAppServico.GetById(esteArquivo.IdAnexo);
                if (anexoAtual == null)
                {
                    esteArquivo.Anexo.Tratar();
                    plaiAtual.ArquivoPlai.Add(esteArquivo);
                    continue;
                }

                //atualização, não pode tter atualização! se trocar, o usuário paga um e insere o outro!
            }
        }

        public ActionResult SalvaPDF(int id)
        {
            GeraArquivoZip(ControllerContext, "PDF", id);

            return View();
        }

        public int RetornaProcessoPorNome(string Nome)
        {
            int idSite = Util.ObterSiteSelecionado();
            var processo = _processoAppServico.Get(x => x.Nome == Nome && x.IdSite == idSite).FirstOrDefault();

            if (processo != null)
            {
                return processo.IdProcesso;
            }
            else
            {
                return 0;
            }

        }

        //public ActionResult PDFDownload(int id)
        //{
        //    var analiseCritica = _analiseCriticaAppServico.GetById(id);

        //    var pdf = new ViewAsPdf
        //    {
        //        ViewName = "PDF",
        //        Model = pai,
        //        PageOrientation = Orientation.Portrait,
        //        PageSize = Size.A4,
        //        PageMargins = new Margins(10, 15, 10, 15),
        //        FileName = "Pai.pdf"
        //    };

        //    return pdf;
        //}

        public ActionResult PDF(int id)
        {
            var pai = _paiAppServico.GetById(id);

            return View(pai);
        }

        public void EnfileirarEmailAuditoria(List<Pai> pais)
        {
            foreach (var pai in pais)
            {
                var plais = _plaiAppServico.GetAll().Where(x => x.IdPai == pai.IdPai);

                foreach (var plai in plais)
                {
                    var registro = _registroAuditoriaServico.RetornaRegistro(plai);

                    if (registro != null)
                    {
                        if (RemoverFilaEnvioAcoesEfetivadas(registro))
                            SalvarEmailAuditoria(plai, pai.Ano);
                    }
                    else
                    {
                        SalvarEmailAuditoria(plai, pai.Ano);
                    }
                }
            }
        }

        public void SalvarEmailAuditoria(Plai plai, int ano)
        {
            var dataDaPlai = new DateTime(ano, plai.Mes, 1);
            var mandarEste = false;

            if ((dataDaPlai - DateTime.Now).Days >= 30 && DateTime.Now.Day > 15)
                mandarEste = true;
            if ((dataDaPlai - DateTime.Now).Days >= 62)
                mandarEste = true;

            if (mandarEste)
            {

                RegistroAuditoria registro = new RegistroAuditoria();

                var pai = _paiAppServico.Get(x => x.IdPai == plai.IdPai).FirstOrDefault();
                var gestor = _usuarioAppServico.GetById(pai.IdGestor);
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Auditoria-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

                var destinatario = gestor.CdIdentificacao;

                string template = System.IO.File.ReadAllText(path);
                string conteudo = template;

                StringBuilder sb = new StringBuilder();

                if (plai.PlaiProcessoNorma != null)
                {
                    foreach (var norma in plai.PlaiProcessoNorma)
                    {
                        sb.AppendFormat("<tr><td>{0} - {1}</td></tr>", norma.Norma.Titulo, norma.Norma.Codigo);
                    }
                }

                conteudo = conteudo.Replace("#NomeGestor#", gestor.NmCompleto);
                conteudo = conteudo.Replace("#PlaiAgendada#", sb.ToString());

                var filaEnvio = new FilaEnvio();

                filaEnvio.Assunto = "Agendamento de Plai";
                filaEnvio.DataAgendado = new DateTime(ano, plai.Mes, 15).AddMonths(-1);
                filaEnvio.DataInclusao = DateTime.Now;
                filaEnvio.Destinatario = destinatario;
                filaEnvio.Enviado = false;
                filaEnvio.Mensagem = conteudo;

                registro.DtInclusao = filaEnvio.DataInclusao;
                if (plai.Pai != null)
                    registro.IdGestor = plai.Pai.IdGestor;
                else
                    registro.IdGestor = gestor.IdUsuario;
                registro.IdPlai = plai.IdPlai;
                registro.IdUsuarioInclusao = int.Parse(Util.ObterUsuario().IdUsuario);

                _filaEnvioServico.Enfileirar(filaEnvio);

                registro.IdFilaEnvio = filaEnvio.Id;

                _registroAuditoriaServico.InserirEmail(registro);
            }
        }
        private bool RemoverFilaEnvioAcoesEfetivadas(RegistroAuditoria registroAuditorias)
        {
            if (registroAuditorias.IdFilaEnvio != null)
            {
                var filaEnvio = _filaEnvioServico.ObterPorId(registroAuditorias.IdFilaEnvio.Value);
                if (filaEnvio != null && !filaEnvio.Enviado)
                {
                    _registroAuditoriaServico.ExcluiEmail(registroAuditorias);
                    _filaEnvioServico.Apagar(filaEnvio);
                    return true;
                }
            }
            return false;
        }

        private void AtualizarDatasAgendadas(RegistroConformidade naoConformidade)
        {
            var acoes = naoConformidade.AcoesImediatas.Where(x => x.DtEfetivaImplementacao == null && x.IdAcaoImediata > 0 && x.IdFilaEnvio != null).ToList();
            var acoesEnfileirar = new List<RegistroAcaoImediata>();

            foreach (var acao in acoes)
            {
                var filaEnvio = _filaEnvioServico.ObterPorId(acao.IdFilaEnvio.Value);

                if (filaEnvio != null)
                {
                    if (!filaEnvio.Enviado)
                    {
                        filaEnvio.DataAgendado = acao.DtPrazoImplementacao.Value.AddDays(1);
                    }
                    else
                    {
                        acoesEnfileirar.Add(acao);
                    }
                }

                _filaEnvioServico.Atualizar(filaEnvio);
            }

            //EnfileirarEmailsAcaoImediata(acoesEnfileirar, naoConformidade);

        }
    }
}
