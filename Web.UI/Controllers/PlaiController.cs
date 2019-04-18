﻿using Dominio.Entidade;
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

            int ? IdPrimeiraNorma = plai.PlaiProcessoNorma.FirstOrDefault()?.IdNorma;

            plai.PlaiProcessoNorma = plai.PlaiProcessoNorma.Where(x => x.IdNorma == IdPrimeiraNorma).ToList();

            plai.Elaborador = _usuarioAppServico.GetById(plai.IdElaborador);
            ViewBag.Normas = _normaAppServico.Get(x => x.IdSite == IdSite).ToList();

            plai.PlaiGerentes.ForEach(x =>
            {
                x.Usuario = _usuarioAppServico.GetById(x.IdUsuario);
            });

            ViewBag.Gestores = _usuarioAppServico.ObterUsuariosPorPerfilESite(IdSite, 3, IdPerfil);

            return View("Criar", plai);
        }

        public ActionResult DownloadPdf(int idPlai)
        {
            int IdSite = Util.ObterSiteSelecionado();

            var plai = _plaiAppServico.Get(x => x.IdPlai == idPlai).FirstOrDefault();

            ViewBag.IdSite = IdSite;
            ViewBag.IdPai = plai.IdPlai;
            ViewBag.Mes = plai.Mes;            

            ViewBag.NormasSelecionadas = plai.PlaiProcessoNorma;

            int? IdPrimeiraNorma = plai.PlaiProcessoNorma.FirstOrDefault()?.IdNorma;

            plai.PlaiProcessoNorma = plai.PlaiProcessoNorma.Where(x => x.IdNorma == IdPrimeiraNorma).ToList();

            plai.Elaborador = _usuarioAppServico.GetById(plai.IdElaborador);
            ViewBag.Normas = _normaAppServico.Get(x => x.IdSite == IdSite).ToList();

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
                Plai plaiAtual = _plaiAppServico.GetById(plai.IdPlai);

                plaiAtual.DataReuniaoAbertura = plai.DataReuniaoAbertura;
                plaiAtual.DataReuniaoEncerramento = plai.DataReuniaoEncerramento;
                plaiAtual.IdElaborador = plai.IdElaborador;
                plaiAtual.PlaiGerentes = plai.PlaiGerentes;


                _plaiServico.Valido(plaiAtual, ref erros);

                if (erros.Count > 0)
                {
                    return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                
                plai.PlaiProcessoNorma.ForEach(plaiProcessoNorma => {

                    var plaiProcessoNormaAcao = _plaiProcessoNormaAppServico.Get(x=> x.IdPlai == plaiProcessoNorma.IdPlai && x.IdProcesso == plaiProcessoNorma.IdProcesso && x.IdNorma == plaiProcessoNorma.IdNorma).FirstOrDefault();

                    if(plaiProcessoNormaAcao == null)
                    {
                        _plaiProcessoNormaAppServico.Add(plaiProcessoNorma);
                    }
                    else
                    {
                        plaiProcessoNormaAcao.Data = plaiProcessoNorma.Data;

                        _plaiProcessoNormaAppServico.Update(plaiProcessoNormaAcao);
                    }
                });


                plai.PlaiProcessoNorma.Select(x=> x.IdProcesso).ToList().ForEach(IdProcesso =>
                {
                    int[] novasNormas = plai.PlaiProcessoNorma.Where(x=> x.IdProcesso == IdProcesso).Select(x => x.IdNorma.Value).ToArray();
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
                new {
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