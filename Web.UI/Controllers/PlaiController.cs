using Dominio.Entidade;
using Dominio.Enumerado;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.UI.Helpers;
using ApplicationService.Interface;
using Dominio.Interface.Servico;
using ApplicationService.Enum;
using Rotativa.Options;
using Web.UI.Models;
using System.Web.Routing;
using System.Threading;

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    public class PlaiController : BaseController
    {
        private readonly IPlaiAppServico _plaiAppServico;
        private readonly IPlaiProcessoNormaAppServico _plaiProcessoNormaAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IPlaiServico _plaiServico;
        private readonly IPlaiGerentesAppServico _plaiGerentesAppServico;
        private readonly INormaAppServico _normaAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public PlaiController(IPlaiAppServico plaiAppServico,
                              ILogAppServico logAppServico,
                              IPlaiProcessoNormaAppServico plaiProcessoNormaAppServico,
                              IPlaiGerentesAppServico plaiGerentesAppServico,
                              IPlaiServico plaiServico,
                              INormaAppServico normaAppServico,
                              IUsuarioAppServico usuarioAppServico,
                              IProcessoAppServico processoAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _plaiAppServico = plaiAppServico;
            _plaiProcessoNormaAppServico = plaiProcessoNormaAppServico;
            _normaAppServico = normaAppServico;
            _logAppServico = logAppServico;
            _plaiGerentesAppServico = plaiGerentesAppServico;
            _plaiServico = plaiServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        public ActionResult Criar(int idPai, int mes)
        {
            int IdSite = Util.ObterSiteSelecionado();
            int IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdSite = IdSite;
            ViewBag.IdPai = idPai;
            ViewBag.Mes = mes;

            var plai = _plaiAppServico.Get(x => x.IdPai == idPai && x.Mes == mes).FirstOrDefault();

            ViewBag.NormasSelecionadas = plai.PlaiProcessoNorma;

            int? IdPrimeiraNorma = plai.PlaiProcessoNorma.FirstOrDefault()?.IdNorma;

            var plaiTest = plai.PlaiProcessoNorma.ToList();

            plai.PlaiProcessoNorma = plai.PlaiProcessoNorma.Where(x => x.IdNorma == IdPrimeiraNorma).ToList();

            plai.Elaborador = _usuarioAppServico.GetById(plai.IdElaborador);
            ViewBag.Normas = _normaAppServico.Get(x => x.IdSite == IdSite).ToList();

            plai.PlaiGerentes.ForEach(x =>
            {
                x.Usuario = _usuarioAppServico.GetById(x.IdUsuario);
            });

            ViewBag.Gestores = _usuarioAppServico.ObterUsuariosPorPerfilESite(IdSite, 3, IdPerfil);
            ViewBag.Editar = (plai.DataReuniaoEncerramento >= DateTime.Now).ToString();

            return View("Criar", plai);
        }

        public ActionResult DownloadPdf(int idPlai)
        {
            int IdSite = Util.ObterSiteSelecionado();

            var plai = _plaiAppServico.Get(x => x.IdPlai == idPlai).FirstOrDefault();
            ///plai.PlaiProcessoNorma = _plaiProcessoNormaAppServico.GetAll().ToList();

            //var teste1 = _plaiProcessoNormaAppServico.GetAll().ToList();

            //var teste = plai.PlaiProcessoNorma.Where(x => x.IdPlai == plai.IdPlai).ToList();

            ViewBag.IdSite = IdSite;
            ViewBag.IdPai = plai.IdPlai;
            ViewBag.Mes = plai.Mes;

            ViewBag.NormasSelecionadas = plai.PlaiProcessoNorma;

            int? IdPrimeiraNorma = plai.PlaiProcessoNorma.FirstOrDefault()?.IdNorma;

            List<int> lstResult = new List<int>();

            foreach (var norma in plai.PlaiProcessoNorma)
            {
                lstResult.Add(norma.IdNorma.GetValueOrDefault());
            }

            plai.PlaiProcessoNorma = plai.PlaiProcessoNorma.Where(x => x.IdNorma == IdPrimeiraNorma).ToList();

            plai.Elaborador = _usuarioAppServico.GetById(plai.IdElaborador);
            var Normas = _normaAppServico.Get(x => x.IdSite == IdSite).ToList();

            var result = Normas.Where(x => lstResult.Contains(x.IdNorma)).ToList();

            ViewBag.Normas = result;

            plai.PlaiGerentes.ForEach(x =>
            {
                x.Usuario = _usuarioAppServico.GetById(x.IdUsuario);
            });

            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = plai,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "PLAI.pdf"
            };

            return pdf;
        }

        public ActionResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var plai = _plaiAppServico.GetById(id);

            return View("Criar", plai);
        }

        [HttpPost]
        public JsonResult Salvar(Plai plai)
        {
            var erros = new List<string>();

            try
            {
                if (plai.PlaiProcessoNorma != null)
                {
                    bool erroData = false;
                    foreach (var item in plai.PlaiProcessoNorma)
                    {
                        if (item.Data < plai.DataReuniaoAbertura || item.DataFinal > plai.DataReuniaoEncerramento)
                        {
                            erros.Add("A data inicial e final do processo deverão estar no intervalo de data e hora de abertura e encerramento do PLAI.");
                            break;
                        }

                        if (item.Data >= item.DataFinal)
                        {
                            erros.Add("A hora inicial não pode ser maior ou igual a hora final.");
                            break;
                        }

                        foreach (var processoVerificar in plai.PlaiProcessoNorma)
                        {
                            //     Data Item atual        Data Principal
                            if (processoVerificar.Data > item.Data && processoVerificar.Data < item.DataFinal ||
                                processoVerificar.DataFinal > item.Data && processoVerificar.DataFinal < item.DataFinal)
                            {
                                erros.Add("Data, Hora inicial e Hora final dos processos não podem se sobrepor.");
                                erroData = true;
                                break;
                            }
                        }
                        if (erroData == true)
                        {
                            break;
                        }
                    }
                }

                //bool temNormaAtiva = false;
                //int processoAtual = plai.PlaiProcessoNorma.FirstOrDefault().IdProcesso;
                //foreach (var item in plai.PlaiProcessoNorma)
                //{
                //	if (processoAtual == item.IdProcesso)
                //	{
                //		if (item.Ativo == true)
                //		{
                //			temNormaAtiva = true;

                //		}
                //	} else {
                //		if (temNormaAtiva == false)
                //		{
                //			break;
                //		}

                //		temNormaAtiva = false;
                //	}
                //	processoAtual = item.IdProcesso;

                //	// item.Ativo 
                //}
                //if (!temNormaAtiva)
                //{
                //	erros.Add("Seleciona uma norma por processo");
                //}

                Plai plaiAtual = _plaiAppServico.GetById(plai.IdPlai);


                bool temNormaAtiva = false;
                foreach (var item in plaiAtual.PlaiProcessoNorma.Select(x => x.IdProcesso).ToList())
                {
                    if (plai.PlaiProcessoNorma != null)
                    {
                        foreach (var itemNorma in plai.PlaiProcessoNorma)
                        {
                            if (itemNorma.IdProcesso == item)
                            {
                                if (itemNorma.Ativo == true)
                                {
                                    temNormaAtiva = true;
                                    break;
                                }
                            }
                        }
                        if (!temNormaAtiva)
                        {
                            erros.Add("Selecionar uma norma por processo");
                            break;
                        }
                        temNormaAtiva = false;
                    }
                    else
                    {
                        erros.Add("Selecionar uma norma por processo");
                        break;
                    }
                }



                plaiAtual.DataReuniaoAbertura = plai.DataReuniaoAbertura;
                plaiAtual.DataReuniaoEncerramento = plai.DataReuniaoEncerramento;
                plaiAtual.IdElaborador = plai.IdElaborador;
                plaiAtual.PlaiGerentes = plai.PlaiGerentes;

                _plaiServico.Valido(plaiAtual, ref erros);

                if (erros.Count > 0)
                {
                    return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                }

                plai.PlaiProcessoNorma.ForEach(plaiProcessoNorma =>
                {
                    var norma = _normaAppServico.Get(x => x.IdNorma == plaiProcessoNorma.IdNorma).FirstOrDefault();

                    plaiProcessoNorma.Ativo = norma.Ativo;

                    var plaiProcessoNormaAcao = _plaiProcessoNormaAppServico.Get(x => x.IdPlai == plaiProcessoNorma.IdPlai && x.IdProcesso == plaiProcessoNorma.IdProcesso && x.IdNorma == plaiProcessoNorma.IdNorma).FirstOrDefault();

                    if (plaiProcessoNormaAcao == null)
                    {
                        _plaiProcessoNormaAppServico.Add(plaiProcessoNorma);
                    }
                    else
                    {
                        plaiProcessoNormaAcao.Data = plaiProcessoNorma.Data;
                        plaiProcessoNormaAcao.DataFinal = plaiProcessoNorma.DataFinal;
                        _plaiProcessoNormaAppServico.Update(plaiProcessoNormaAcao);
                    }
                });

                var lstNorma = plai.PlaiProcessoNorma.Where(x => x.Ativo == false).ToList();

                if (lstNorma.Count > 0)
                {
                    foreach (var itemnorma in lstNorma)
                    {
                        var norma = _normaAppServico.Get(x => x.IdNorma == itemnorma.IdNorma).FirstOrDefault();
                        erros.Add(string.Format("A norma {0} está inativa e não pode ser utilizada na Plai", norma.Codigo));
                    }
                    return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                }

                plai.PlaiProcessoNorma.Select(x => x.IdProcesso).ToList().ForEach(IdProcesso =>
                 {

                     int[] novasNormas = plai.PlaiProcessoNorma.Where(x => x.IdProcesso == IdProcesso).Select(x => x.IdNorma.Value).ToArray();
                     int[] normasAtuais = plaiAtual.PlaiProcessoNorma.Where(x => x.IdProcesso == IdProcesso).Select(x => x.IdNorma.Value).ToArray();

                     if (novasNormas != null)
                     {
                         foreach (int norma in normasAtuais)
                         {
                             if (!novasNormas.Contains(norma))
                             {
                                 List<PlaiProcessoNorma> plaiProcessoNormas = _plaiProcessoNormaAppServico.Get(x => x.IdNorma == norma && x.IdPlai == plai.IdPlai && x.IdProcesso == IdProcesso).ToList();

                                 plaiProcessoNormas.ForEach(plaiProcessoNorma =>
                                 {
                                     _plaiProcessoNormaAppServico.Remove(plaiProcessoNorma);
                                 });
                             }
                         }
                     }
                 });

                _plaiGerentesAppServico.Get(x => x.IdPlai == plaiAtual.IdPlai).ToList().ForEach(y =>
                {
                    _plaiGerentesAppServico.Remove(y);
                });



                plaiAtual.DataAlteracao = DateTime.Now;
                _plaiAppServico.Atualizar(plaiAtual);

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

        public ActionResult Excluir(int id)
        {
            var plai = _plaiAppServico.GetById(id);

            _plaiAppServico.Remove(plai);

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

    }
}