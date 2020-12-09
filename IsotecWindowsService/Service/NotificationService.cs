using ApplicationService.Interface;
using DAL.Context;
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Servico;
using IsotecWindowsService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Helpers;
using Web.UI.Models;

namespace IsotecWindowsService.Service
{
    public class NotificationService : INotificationService
    {
     
        private readonly IFilaEnvioServico _filaEnvioServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        
        DateTime LastDay = DateTime.Now.AddDays(-1);
        public NotificationService(IFilaEnvioServico filaEnvioServico, IUsuarioAppServico usuarioAppServico)
        {
            _filaEnvioServico = filaEnvioServico;
            _usuarioAppServico = usuarioAppServico;
        }

        public void SendNotification()
        {
            using (var db = new BaseContext())
            {


                var DtVencimento = DateTime.Now.AddDays(-1);



                var licenca = (from lc in db.Licenca
                               where lc.DataVencimento.Value < DtVencimento

                               select new PendenciaViewModel
                               {
                                   Id = lc.IdLicenca,
                                   Titulo = lc.Titulo,
                                   IdResponsavel = lc.IdResponsavel,
                                   Modulo = "Licenca",
                                   Url = "Licenca/Editar/" + lc.IdLicenca


                               });

                var indicadores = (from ind in db.Indicador
                                   join per in db.PeriodicidaDeAnalise on ind.Id equals per.IdIndicador
                                   join meta in db.PlanoVoo on per.Id equals meta.IdPeriodicidadeAnalise
                                   where meta.DataReferencia < DateTime.Now && meta.Realizado == null

                                   select new PendenciaViewModel
                                   {
                                       Id = ind.Id,
                                       Titulo = ind.Objetivo,
                                       IdResponsavel = ind.IdResponsavel,
                                       Modulo = "Indicador",
                                       Url = "Indicador/Editar/" + ind.Id
                                   });
                var docDocumentoAprov = (from doc in db.DocDocumento
                                         where doc.StatusRegistro == (int)StatusDocumento.Aprovacao
                                         select doc);

                var docDocumentoVer = (from doc in db.DocDocumento
                                       where doc.StatusRegistro == (int)StatusDocumento.Verificacao
                                       select doc);

                var doDocumentoRev = (from doc in db.DocDocumento
                                      where doc.FlRevisaoPeriodica == true && doc.DtNotificacao < DateTime.Now
                                      select doc);




                var naoConformidade = (from nc in db.RegistroConformidade
                                       where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.TipoRegistro == "nc"
                                       select new PendenciaViewModel
                                       {
                                           Id = (int)nc.IdRegistroConformidade,
                                           Titulo = nc.NuRegistro.ToString(),
                                           IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                           Modulo = "NC",
                                           Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                       }); ;

                var naoConformidadePrazo = (from nc in db.RegistroConformidade
                                            join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                            where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && nc.TipoRegistro == "nc"
                                            select new PendenciaViewModel
                                            {
                                                Id = (int)nc.IdRegistroConformidade,
                                                Titulo = nc.NuRegistro.ToString(),
                                                IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                Modulo = "NC",
                                                Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                            });

                var naoConformidadeReverificacao = (from nc in db.RegistroConformidade
                                                    where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "nc"
                                                    select new PendenciaViewModel
                                                    {
                                                        Id = (int)nc.IdRegistroConformidade,
                                                        Titulo = nc.NuRegistro.ToString(),
                                                        IdResponsavel = nc.ResponsavelReverificador.IdUsuario,
                                                        Modulo = "NC",
                                                        Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                                    });

                var acaoCorretiva = (from ac in db.RegistroConformidade
                                     where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ac.TipoRegistro == "ac"
                                     select new PendenciaViewModel
                                     {
                                         Id = ac.IdRegistroConformidade,
                                         Titulo = ac.NuRegistro.ToString(),
                                         IdResponsavel = ac.ResponsavelInicarAcaoImediata.IdUsuario,
                                         Modulo = "AC",
                                         Url = "AcaoCorretiva/Editar/" + ac.IdRegistroConformidade

                                     });

                var acaoCorretivaRev = (from ac in db.RegistroConformidade
                                        where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && ac.TipoRegistro == "ac" && ac.DtPrazoImplementacao < DateTime.Now
                                        select new PendenciaViewModel
                                        {
                                            Id = ac.IdRegistroConformidade,
                                            Titulo = ac.NuRegistro.ToString(),
                                            IdResponsavel = ac.ResponsavelImplementar.IdUsuario,
                                            Modulo = "AC",
                                            Url = "AcaoCorretiva/Editar/" + ac.IdRegistroConformidade
                                        });

                var Auditoria = (from plai in db.Plai
                                 where plai.DataReuniaoAbertura.Day <= DateTime.Now.AddDays(-15).Day
                                 select new PendenciaViewModel
                                 {
                                     Id = plai.IdPlai,
                                     Titulo = "Auditoria",
                                     IdResponsavel = plai.IdElaborador,
                                     Modulo = "Auditoria",
                                     Url = "Auditoria/Editar/" + plai.IdPlai
                                 });

                var AnaliseCritica = (from anc in db.AnaliseCritica
                                      where anc.DataProximaAnalise == LastDay
                                      select new PendenciaViewModel
                                      {
                                          Id = anc.IdAnaliseCritica,
                                          Titulo = anc.Ata,
                                          IdResponsavel = anc.IdResponsavel,
                                          Modulo = "AnaliseCritica",
                                          Url = "AnaliseCritica/Editar/" + anc.IdAnaliseCritica
                                      }).ToList();

                var InstrumentoQuery = (from ins in db.Instrumento
                                        join cal in db.Calibracao on ins.IdInstrumento equals cal.IdInstrumento
                                        where cal.DataProximaCalibracao < DateTime.Now && ins.Status == (byte)EquipamentoStatus.NaoCalibrado
                                        group ins by ins.IdInstrumento into idIsntrumentoOrder
                                        select idIsntrumentoOrder).ToList();

                var Instrumento = new List<PendenciaViewModel>();
                foreach (var item in InstrumentoQuery)
                {
                    var a = item.FirstOrDefault();

                    Instrumento.Add(new PendenciaViewModel()
                    {
                        Id = a.IdInstrumento,
                        Titulo = a.Equipamento,
                        IdResponsavel = (int)a.IdResponsavel,
                        Modulo = "Instrumento",
                        Url = "Instrumento/Editar/" + a.IdInstrumento
                    });

                }

                var Fornecedores = (from forn in db.Fornecedor
                                    join avaq in db.AvaliaCriterioQualificacao on forn.IdFornecedor equals avaq.IdFornecedor
                                    join prodf in db.ProdutoFornecedor on forn.IdFornecedor equals prodf.IdFornecedor
                                    where avaq.IdResponsavelPorControlarVencimento != null && avaq.DtVencimento != null && avaq.DtVencimento <= DateTime.Now
                                    select new PendenciaViewModel
                                    {
                                        Id = forn.IdFornecedor,
                                        Titulo = forn.Nome,
                                        IdResponsavel = avaq.IdResponsavelPorQualificar,
                                        Modulo = "Fornecedor",
                                        Url = "fornecedor/acoesfornecedores/" + forn.IdFornecedor + "?idProduto=" + prodf.IdProduto + "&Ancora=Qualificar"
                                    });

                var FornecedoresVal = (from forn in db.Fornecedor
                                       join avaa in db.AvaliaCriterioAvaliacao on forn.IdFornecedor equals avaa.IdFornecedor
                                       join prodf in db.ProdutoFornecedor on forn.IdFornecedor equals prodf.IdFornecedor
                                       where avaa.DtProximaAvaliacao == DateTime.Now && avaa.IdUsuarioAvaliacao != null
                                       select new PendenciaViewModel
                                       {
                                           Id = forn.IdFornecedor,
                                           Titulo = forn.Nome,
                                           IdResponsavel = (int)avaa.IdUsuarioAvaliacao,
                                           Modulo = "Fornecedor",
                                           Url = "fornecedor/acoesfornecedores/" + forn.IdFornecedor + "?idProduto=" + prodf.IdProduto + "&Ancora=Avaliar"
                                       });

                var gestaoRisco = (from nc in db.RegistroConformidade
                                   where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.TipoRegistro == "gr"
                                   select new PendenciaViewModel
                                   {
                                       Id = (int)nc.IdRegistroConformidade,
                                       Titulo = nc.NuRegistro.ToString(),
                                       IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                       Modulo = "GR",
                                       Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade
                                   });

                var gestaoRiscoPrazo = (from nc in db.RegistroConformidade
                                        where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && nc.DtPrazoImplementacao < DateTime.Now && nc.TipoRegistro == "gr"
                                        select new PendenciaViewModel
                                        {
                                            Id = (int)nc.IdRegistroConformidade,
                                            Titulo = nc.NuRegistro.ToString(),
                                            IdResponsavel = nc.ResponsavelImplementar.IdUsuario,
                                            Modulo = "GR",
                                            Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade
                                        });

                var gestaoRiscoReverificacao = (from nc in db.RegistroConformidade
                                                where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gr"
                                                select new PendenciaViewModel
                                                {
                                                    Id = (int)nc.IdRegistroConformidade,
                                                    Titulo = nc.NuRegistro.ToString(),
                                                    IdResponsavel = nc.ResponsavelReverificador.IdUsuario,
                                                    Modulo = "GR",
                                                    Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade,
                                                });



                var gestaoMelhoria = (from nc in db.RegistroConformidade
                                      where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.TipoRegistro == "gm"
                                      select new PendenciaViewModel
                                      {
                                          Id = (int)nc.IdRegistroConformidade,
                                          Titulo = nc.NuRegistro.ToString(),
                                          IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                          Modulo = "GM",
                                          Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                      });

                var gestaoMelhoriaPrazo = (from nc in db.RegistroConformidade
                                           where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && nc.DtPrazoImplementacao < DateTime.Now && nc.TipoRegistro == "gm"
                                           select new PendenciaViewModel
                                           {
                                               Id = (int)nc.IdRegistroConformidade,
                                               Titulo = nc.NuRegistro.ToString(),
                                               IdResponsavel = nc.ResponsavelImplementar.IdUsuario,
                                               Modulo = "GM",
                                               Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                           });

                var gestaoMelhoriaReverificacao = (from nc in db.RegistroConformidade
                                                   where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gm"
                                                   select new PendenciaViewModel
                                                   {
                                                       Id = (int)nc.IdRegistroConformidade,
                                                       Titulo = nc.NuRegistro.ToString(),
                                                       IdResponsavel = nc.ResponsavelImplementar.IdUsuario,
                                                       Modulo = "GM",
                                                       Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                                   });


                List<PendenciaViewModel> docPendencia = new List<PendenciaViewModel>();

                foreach (var docap in docDocumentoAprov.ToList())
                {
                    if (docap.DocUsuarioVerificaAprova != null)
                    {
                        var docResult = docap.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").OrderBy(x => x.Usuario.NmCompleto).ToList();

                        foreach (var res in docResult)
                        {
                            PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                            {
                                Id = docap.IdDocumento,
                                Titulo = docap.Titulo,
                                IdResponsavel = res.IdUsuario,
                                Modulo = "DocDocumento",
                                Url = "DocDocumento/Editar" + docap.IdDocumento
                            };

                            docPendencia.Add(pendenciaViewModel);
                        }
                    }
                }

                foreach (var docver in docDocumentoVer.ToList())
                {
                    if (docver.DocUsuarioVerificaAprova != null)
                    {
                        var docResult = docver.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "V").OrderBy(x => x.Usuario.NmCompleto).ToList();

                        foreach (var res in docResult)
                        {
                            PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                            {
                                Id = docver.IdDocumento,
                                Titulo = docver.Titulo,
                                IdResponsavel = res.IdUsuario,
                                Modulo = "DocDocumento",
                                Url = "DocDocumento/Editar" + docver.IdDocumento
                            };

                            docPendencia.Add(pendenciaViewModel);
                        }
                    }
                }

                foreach (var docrev in doDocumentoRev.ToList())
                {
                    PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                    {
                        Id = docrev.IdDocumento,
                        Titulo = docrev.Titulo,
                        IdResponsavel = docrev.IdElaborador,
                        Modulo = "DocDocumento",
                        Url = "DocDocumento/Editar" + docrev.IdDocumento
                    };

                    docPendencia.Add(pendenciaViewModel);
                }


                Dictionary<int, bool> queryIndicadores = new Dictionary<int, bool>();

                var indicadores1 = (from ind in db.Indicador
                                    select ind).ToList();
                List<PendenciaViewModel> lstPendencia = new List<PendenciaViewModel>();

                foreach (var indicador in indicadores1)
                {
                    foreach (var periodo in indicador.PeriodicidadeDeAnalises)
                    {
                        var query = periodo.MetasRealizadas.Where(x => x.DataReferencia < DateTime.Now && x.Realizado == null).ToList();

                        if (query.Count > 0)
                        {
                            foreach (var plano in query)
                            {
                                var mes = plano.DataReferencia.Date.Month;

                                switch (indicador.PeriodicidadeMedicao)
                                {
                                    case 1:
                                        if ((mes % 1) == 0)
                                        {
                                            if (mes != DateTime.Now.Month)
                                            {
                                                PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                {
                                                    Id = indicador.Id,
                                                    IdResponsavel = indicador.IdResponsavel,
                                                    Titulo = indicador.Objetivo,
                                                    Modulo = "Indicador",
                                                    Url = "Indicador/Editar/" + indicador.Id
                                                };

                                                lstPendencia.Add(pendenciaViewModel);
                                            }
                                        }
                                        break;
                                    case 2:
                                        if ((mes % 2) == 0)
                                        {
                                            if (mes != DateTime.Now.Month)
                                            {
                                                PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                {
                                                    Id = indicador.Id,
                                                    IdResponsavel = indicador.IdResponsavel,
                                                    Titulo = indicador.Objetivo,
                                                    Modulo = "Indicador",
                                                    Url = "Indicador/Editar/" + indicador.Id
                                                };

                                                lstPendencia.Add(pendenciaViewModel);
                                            }
                                        }
                                        break;
                                    case 3:
                                        if ((mes % 3) == 0)
                                        {
                                            if (mes != DateTime.Now.Month)
                                            {
                                                PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                {
                                                    Id = indicador.Id,
                                                    IdResponsavel = indicador.IdResponsavel,
                                                    Titulo = indicador.Objetivo,
                                                    Modulo = "Indicador",
                                                    Url = "Indicador/Editar/" + indicador.Id
                                                };

                                                lstPendencia.Add(pendenciaViewModel);
                                            }
                                        }
                                        break;
                                    case 4:
                                        if ((mes % 6) == 0)
                                        {
                                            if (mes != DateTime.Now.Month)
                                            {
                                                PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                {
                                                    Id = indicador.Id,
                                                    IdResponsavel = indicador.IdResponsavel,
                                                    Titulo = indicador.Objetivo,
                                                    Modulo = "Indicador",
                                                    Url = "Indicador/Editar/" + indicador.Id
                                                };

                                                lstPendencia.Add(pendenciaViewModel);
                                            }
                                        }
                                        break;
                                    case 5:
                                        if ((mes % 12) == 0)
                                        {
                                            if (mes != DateTime.Now.Month)
                                            {
                                                PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                {
                                                    Id = indicador.Id,
                                                    IdResponsavel = indicador.IdResponsavel,
                                                    Titulo = indicador.Objetivo,
                                                    Modulo = "Indicador",
                                                    Url = "Indicador/Editar/" + indicador.Id
                                                };

                                                lstPendencia.Add(pendenciaViewModel);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                lstPendencia.AddRange(licenca.ToList());
                lstPendencia.AddRange(naoConformidade.ToList());
                lstPendencia.AddRange(naoConformidadePrazo.ToList());
                lstPendencia.AddRange(naoConformidadeReverificacao.ToList());
                lstPendencia.AddRange(acaoCorretiva.ToList());
                lstPendencia.AddRange(acaoCorretivaRev.ToList());
                lstPendencia.AddRange(Instrumento.ToList());
                lstPendencia.AddRange(Fornecedores.ToList());
                lstPendencia.AddRange(AnaliseCritica);
                lstPendencia.AddRange(gestaoRisco.ToList());
                lstPendencia.AddRange(gestaoRiscoPrazo.ToList());
                lstPendencia.AddRange(gestaoRiscoReverificacao.ToList());
                lstPendencia.AddRange(gestaoMelhoria.ToList());
                lstPendencia.AddRange(gestaoMelhoriaPrazo.ToList());
                lstPendencia.AddRange(gestaoMelhoriaReverificacao.ToList());
                lstPendencia.AddRange(docPendencia);

            }
        }


        public void AgendarEmail(List<PendenciaViewModel> lstPendencia)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Notificacao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);

            string conteudo = template;

            foreach (var pendencia in lstPendencia)
            {
                StringBuilder sb = new StringBuilder();

                var Responsavel = _usuarioAppServico.GetById((int)pendencia.IdResponsavel);

                conteudo = conteudo.Replace("#Modulo#", pendencia.Modulo);
                conteudo = conteudo.Replace("#Titulo#", pendencia.Titulo);

                FilaEnvio filaEnvio = new FilaEnvio();

                filaEnvio.Assunto = "Pendência Equalit";
                filaEnvio.DataAgendado = DateTime.Now;
                filaEnvio.DataInclusao = DateTime.Now;
                filaEnvio.Destinatario = Responsavel.CdIdentificacao;
                filaEnvio.Enviado = false;
                filaEnvio.Mensagem = conteudo;

                _filaEnvioServico.Enfileirar(filaEnvio);
            }
        }
    }
}
