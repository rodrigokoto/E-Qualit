using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Context;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class PendenciaAppServico : IPendenciaAppServico
    {
        public List<Pendencia> PendeciaPorSite(int idSite, int idCliente)
        {
            return CarregaPendencia(idSite, idCliente);
        }

        public List<Pendencia> PendeciaPorUsuario(int idSite, int idCliente, int idUsuario)
        {
            return CarregaPendencia(idSite, idCliente).Where(x => x.IdResponsavel == idUsuario).ToList();
        }

        public List<Pendencia> ObterTodasPendencias()
        {
            throw new NotImplementedException();
        }

        private List<Pendencia> CarregaPendencia(int idSite, int idCliente)
        {

            try
            {
                using (var db = new BaseContext())
                {
                    DateTime LastDay = DateTime.Now.AddDays(-1);


                    var DtVencimento = DateTime.Now.AddDays(-1);


                    if (idSite != 0)
                    {
                        var licenca = (from lc in db.Licenca
                                       where lc.DataVencimento.Value < DateTime.Now && lc.Idcliente == idCliente

                                       select new Pendencia
                                       {
                                           Id = lc.IdLicenca,
                                           Titulo = lc.Titulo,
                                           IdResponsavel = lc.IdResponsavel,
                                           TipoPendencia = TipoPendencia.Pendencia,
                                           Modulo = "Licenca",
                                           Url = "Licenca/Editar/" + lc.IdLicenca


                                       });

                        var indicadores = (from ind in db.Indicador
                                           join per in db.PeriodicidaDeAnalise on ind.Id equals per.IdIndicador
                                           join meta in db.PlanoVoo on per.Id equals meta.IdPeriodicidadeAnalise
                                           where meta.DataReferencia < DateTime.Now && meta.Realizado == null && ind.IdSite == idSite

                                           select new Pendencia
                                           {
                                               Id = ind.Id,
                                               Titulo = ind.Objetivo,
                                               IdResponsavel = ind.IdResponsavel,
                                               TipoPendencia = TipoPendencia.Pendencia,
                                               Modulo = "Indicador",
                                               Url = "Indicador/Editar/" + ind.Id
                                           });
                        var docDocumentoAprov = (from doc in db.DocDocumento
                                                 where doc.FlStatus == (int)StatusDocumento.Aprovacao && doc.IdSite == idSite
                                                 select doc);

                        var docDocumentoVer = (from doc in db.DocDocumento
                                               where doc.FlStatus == (int)StatusDocumento.Verificacao && doc.IdSite == idSite
                                               select doc);

                        var doDocumentoRev = (from doc in db.DocDocumento
                                              where doc.FlRevisaoPeriodica == true && doc.DtNotificacao < DateTime.Now && doc.IdSite == idSite && doc.FlStatus == (int)StatusDocumento.Aprovado
                                              select doc);

                        var naoConformidade = (from nc in db.RegistroConformidade
                                               where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "nc"
                                               select new Pendencia
                                               {
                                                   Id = (int)nc.IdRegistroConformidade,
                                                   Titulo = nc.NuRegistro.ToString(),
                                                   IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                                   TipoPendencia = TipoPendencia.Notificacao,
                                                   Modulo = "NC",
                                                   Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                               }); ;

                        var naoConformidadePrazo = (from nc in db.RegistroConformidade
                                                    join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                                    where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "nc"
                                                    select new Pendencia
                                                    {
                                                        Id = (int)nc.IdRegistroConformidade,
                                                        Titulo = nc.NuRegistro.ToString(),
                                                        IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                        TipoPendencia = TipoPendencia.Pendencia,
                                                        Modulo = "NC",
                                                        Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                                    });

                        var naoConformidadeReverificacao = (from nc in db.RegistroConformidade
                                                            where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "nc" && nc.IdSite == idSite
                                                            select new Pendencia
                                                            {
                                                                Id = (int)nc.IdRegistroConformidade,
                                                                Titulo = nc.NuRegistro.ToString(),
                                                                TipoPendencia = TipoPendencia.Notificacao,
                                                                IdResponsavel = nc.ResponsavelReverificador.IdUsuario,
                                                                Modulo = "NC",
                                                                Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                                            });

                        var acaoCorretiva = (from ac in db.RegistroConformidade
                                             join ai in db.AcaoImediata on ac.IdRegistroConformidade equals ai.IdRegistroConformidade
                                             where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && ac.IdSite == idSite && ac.TipoRegistro == "ac"
                                             select new Pendencia
                                             {
                                                 Id = ac.IdRegistroConformidade,
                                                 Titulo = ac.NuRegistro.ToString(),
                                                 TipoPendencia = TipoPendencia.Pendencia,
                                                 IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                 Modulo = "AC",
                                                 Url = "AcaoCorretiva/Editar/" + ac.IdRegistroConformidade

                                             });

                        var acaoCorretivaRev = (from ac in db.RegistroConformidade
                                                where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && ac.TipoRegistro == "ac" && ac.DtPrazoImplementacao < DateTime.Now && ac.IdSite == idSite
                                                select new Pendencia
                                                {
                                                    Id = ac.IdRegistroConformidade,
                                                    Titulo = ac.NuRegistro.ToString(),
                                                    IdResponsavel = ac.ResponsavelImplementar.IdUsuario,
                                                    TipoPendencia = TipoPendencia.Notificacao,
                                                    Modulo = "AC",
                                                    Url = "AcaoCorretiva/Editar/" + ac.IdRegistroConformidade
                                                });
                        //No dia 15 do mês anterior P
                        var Auditoria = (from plai in db.Plai
                                         where plai.DataReuniaoAbertura.Day <= DateTime.Now.AddDays(-15).Day && plai.Pai.IdSite == idSite
                                         select new Pendencia
                                         {
                                             Id = plai.IdPlai,
                                             Titulo = "Auditoria",
                                             IdResponsavel = plai.IdElaborador,
                                             TipoPendencia = TipoPendencia.Notificacao,
                                             Modulo = "Auditoria",
                                             Url = "Auditoria/Editar/" + plai.IdPlai
                                         });

                        var AnaliseCritica = (from anc in db.AnaliseCritica
                                              where anc.IdSite == idSite && anc.DataProximaAnalise == DateTime.Now
                                              select new Pendencia
                                              {
                                                  Id = anc.IdAnaliseCritica,
                                                  Titulo = anc.Ata,
                                                  IdResponsavel = anc.IdResponsavel,
                                                  TipoPendencia = TipoPendencia.Notificacao,
                                                  Modulo = "AnaliseCritica",
                                                  Url = "AnaliseCritica/Editar/" + anc.IdAnaliseCritica
                                              }).ToList();


                        var AnaliseCriticaPendencia = (from anc in db.AnaliseCritica
                                                       where anc.IdSite == idSite && anc.DataProximaAnalise < DateTime.Now
                                                       select new Pendencia
                                                       {
                                                           Id = anc.IdAnaliseCritica,
                                                           Titulo = anc.Ata,
                                                           IdResponsavel = anc.IdResponsavel,
                                                           TipoPendencia = TipoPendencia.Pendencia,
                                                           Modulo = "AnaliseCritica",
                                                           Url = "AnaliseCritica/Editar/" + anc.IdAnaliseCritica
                                                       }).ToList();

                        //var InstrumentoQuery = (from ins in db.Instrumento
                        //                        join cal in db.Calibracao on ins.IdInstrumento equals cal.IdInstrumento
                        //                        where ins.IdSite == idSite && ins.Status == (byte)EquipamentoStatus.NaoCalibrado
                        //                        group ins by ins.IdInstrumento into idIsntrumentoOrder
                        //                        select idIsntrumentoOrder).ToList();


                        var InstrumentoQuery = (from ins in db.Instrumento
                                                where ins.IdSite == idSite && ins.Status == (byte)EquipamentoStatus.NaoCalibrado
                                                select ins).ToList();

                        var Instrumento = new List<Pendencia>();
                        foreach (var item in InstrumentoQuery)
                        {

                            var calibracao = (from cal in db.Calibracao
                                              where cal.IdInstrumento == item.IdInstrumento
                                              select cal).OrderByDescending(x => x.DataProximaCalibracao).FirstOrDefault();

                            if (calibracao != null)
                            {
                                if (calibracao.DataProximaCalibracao < DateTime.Now)
                                {
                                    Instrumento.Add(new Pendencia()
                                    {
                                        Id = item.IdInstrumento,
                                        Titulo = item.Equipamento,
                                        IdResponsavel = (int)item.IdResponsavel,
                                        TipoPendencia = TipoPendencia.Pendencia,
                                        Modulo = "Instrumento",
                                        Url = "Instrumento/Editar/" + item.IdInstrumento
                                    });
                                }
                            }
                        }

                        var Fornecedores = (from forn in db.Fornecedor
                                            join avaq in db.AvaliaCriterioQualificacao on forn.IdFornecedor equals avaq.IdFornecedor
                                            join prodf in db.ProdutoFornecedor on forn.IdFornecedor equals prodf.IdFornecedor
                                            where avaq.IdResponsavelPorControlarVencimento != null && avaq.DtVencimento != null && avaq.DtVencimento < DateTime.Now && forn.IdSite == idSite
                                            select new Pendencia
                                            {
                                                Id = forn.IdFornecedor,
                                                Titulo = forn.Nome,
                                                IdResponsavel = avaq.IdResponsavelPorQualificar,
                                                TipoPendencia = TipoPendencia.Pendencia,
                                                Modulo = "Fornecedor - CQ",
                                                Url = "fornecedor/acoesfornecedores/" + forn.IdFornecedor + "?idProduto=" + prodf.IdProduto + "&Ancora=Qualificar"
                                            });



                        var FornecedoresLembrete = (from forn in db.Fornecedor
                                                    join avaq in db.AvaliaCriterioQualificacao on forn.IdFornecedor equals avaq.IdFornecedor
                                                    join prodf in db.ProdutoFornecedor on forn.IdFornecedor equals prodf.IdFornecedor
                                                    where avaq.IdResponsavelPorControlarVencimento != null && avaq.DtVencimento != null && avaq.DtVencimento == DateTime.Now && forn.IdSite == idSite
                                                    select new Pendencia
                                                    {
                                                        Id = forn.IdFornecedor,
                                                        Titulo = forn.Nome,
                                                        IdResponsavel = avaq.IdResponsavelPorQualificar,
                                                        TipoPendencia = TipoPendencia.Notificacao,
                                                        Modulo = "Fornecedor - CQ",
                                                        Url = "fornecedor/acoesfornecedores/" + forn.IdFornecedor + "?idProduto=" + prodf.IdProduto + "&Ancora=Qualificar"
                                                    });



                        var FornecedoresQuery = (from forn in db.Fornecedor
                                                 where forn.IdSite == idSite
                                                 select forn);

                        var FornecedorP = new List<Pendencia>();

                        foreach (var fornecedor in FornecedoresQuery.ToList())
                        {
                            var avaliaCriterioAvaliacao = (from avaa in db.AvaliaCriterioAvaliacao
                                                           where avaa.IdUsuarioAvaliacao != null
                                                           select avaa).OrderByDescending(x => x.DtProximaAvaliacao).FirstOrDefault();

                            var Produto = (from prodF in db.ProdutoFornecedor
                                           where prodF.IdFornecedor == fornecedor.IdFornecedor
                                           select prodF).FirstOrDefault();
                            if (avaliaCriterioAvaliacao != null)
                            {
                                if (avaliaCriterioAvaliacao.DtProximaAvaliacao < DateTime.Now)
                                {

                                    FornecedorP.Add(new Pendencia()
                                    {
                                        Id = fornecedor.IdFornecedor,
                                        Titulo = fornecedor.Nome,
                                        IdResponsavel = (int)avaliaCriterioAvaliacao.IdUsuarioAvaliacao,
                                        TipoPendencia = TipoPendencia.Pendencia,
                                        Modulo = "Fornecedor - CA",
                                        Url = "fornecedor/acoesfornecedores/" + fornecedor.IdFornecedor + "?idProduto=" + Produto.IdProduto + "&Ancora=Avaliar"
                                    });
                                }
                                else if (avaliaCriterioAvaliacao.DtProximaAvaliacao == DateTime.Now)
                                {

                                    FornecedorP.Add(new Pendencia()
                                    {
                                        Id = fornecedor.IdFornecedor,
                                        Titulo = fornecedor.Nome,
                                        IdResponsavel = (int)avaliaCriterioAvaliacao.IdUsuarioAvaliacao,
                                        TipoPendencia = TipoPendencia.Notificacao,
                                        Modulo = "Fornecedor - CA",
                                        Url = "fornecedor/acoesfornecedores/" + fornecedor.IdFornecedor + "?idProduto=" + Produto.IdProduto + "&Ancora=Avaliar"
                                    });
                                }
                            }
                        }

                        var gestaoRisco = (from nc in db.RegistroConformidade
                                           where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "gr"
                                           select new Pendencia
                                           {
                                               Id = (int)nc.IdRegistroConformidade,
                                               Titulo = nc.NuRegistro.ToString(),
                                               IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                               TipoPendencia = TipoPendencia.Notificacao,
                                               Modulo = "GR",
                                               Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade
                                           });

                        var gestaoRiscoPrazo = (from nc in db.RegistroConformidade
                                                join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                                where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "gr"
                                                select new Pendencia
                                                {
                                                    Id = (int)nc.IdRegistroConformidade,
                                                    Titulo = nc.NuRegistro.ToString(),
                                                    IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                    TipoPendencia = TipoPendencia.Pendencia,
                                                    Modulo = "GR",
                                                    Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade
                                                });

                        var gestaoRiscoReverificacao = (from nc in db.RegistroConformidade
                                                        where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gr" && nc.IdSite == idSite
                                                        select new Pendencia
                                                        {
                                                            Id = (int)nc.IdRegistroConformidade,
                                                            Titulo = nc.NuRegistro.ToString(),
                                                            IdResponsavel = nc.ResponsavelReverificador.IdUsuario,
                                                            TipoPendencia = TipoPendencia.Notificacao,
                                                            Modulo = "GR",
                                                            Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade,
                                                        });



                        var gestaoMelhoria = (from nc in db.RegistroConformidade
                                              where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "gm"
                                              select new Pendencia
                                              {
                                                  Id = (int)nc.IdRegistroConformidade,
                                                  Titulo = nc.NuRegistro.ToString(),
                                                  IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                                  TipoPendencia = TipoPendencia.Notificacao,
                                                  Modulo = "GM",
                                                  Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                              });

                        var gestaoMelhoriaPrazo = (from nc in db.RegistroConformidade
                                                   join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                                   where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "gm"
                                                   select new Pendencia
                                                   {
                                                       Id = (int)nc.IdRegistroConformidade,
                                                       Titulo = nc.NuRegistro.ToString(),
                                                       TipoPendencia = TipoPendencia.Pendencia,
                                                       IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                       Modulo = "GM",
                                                       Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                                   });

                        var gestaoMelhoriaReverificacao = (from nc in db.RegistroConformidade
                                                           where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gm" && nc.IdSite == idSite
                                                           select new Pendencia
                                                           {
                                                               Id = (int)nc.IdRegistroConformidade,
                                                               Titulo = nc.NuRegistro.ToString(),
                                                               IdResponsavel = nc.ResponsavelImplementar.IdUsuario,
                                                               TipoPendencia = TipoPendencia.Notificacao,
                                                               Modulo = "GM",
                                                               Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                                           });


                        List<Pendencia> docPendencia = new List<Pendencia>();

                        foreach (var docap in docDocumentoAprov.ToList())
                        {
                            if (docap.DocUsuarioVerificaAprova != null)
                            {
                                var docResult = docap.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").OrderBy(x => x.Usuario.NmCompleto).ToList();

                                foreach (var res in docResult)
                                {
                                    Pendencia Pendencia = new Pendencia()
                                    {
                                        Id = docap.IdDocumento,
                                        Titulo = docap.Titulo,
                                        IdResponsavel = res.IdUsuario,
                                        TipoPendencia = TipoPendencia.Notificacao,
                                        Modulo = "ControlDoc",
                                        Url = "ControlDoc/Editar/" + docap.IdDocumento
                                    };

                                    docPendencia.Add(Pendencia);
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
                                    Pendencia Pendencia = new Pendencia()
                                    {
                                        Id = docver.IdDocumento,
                                        Titulo = docver.Titulo,
                                        IdResponsavel = res.IdUsuario,
                                        TipoPendencia = TipoPendencia.Notificacao,
                                        Modulo = "ControlDoc",
                                        Url = "ControlDoc/Editar/" + docver.IdDocumento
                                    };

                                    docPendencia.Add(Pendencia);
                                }
                            }
                        }

                        foreach (var docrev in doDocumentoRev.ToList())
                        {
                            Pendencia Pendencia = new Pendencia()
                            {
                                Id = docrev.IdDocumento,
                                Titulo = docrev.Titulo,
                                IdResponsavel = docrev.IdElaborador,
                                TipoPendencia = TipoPendencia.Pendencia,
                                Modulo = "ControlDoc",
                                Url = "ControlDoc/DocumentosRevisao"
                            };

                            docPendencia.Add(Pendencia);
                        }


                        Dictionary<int, bool> queryIndicadores = new Dictionary<int, bool>();

                        var indicadores1 = (from ind in db.Indicador
                                            where ind.IdSite == idSite
                                            select ind).ToList();
                        List<Pendencia> lstPendencia = new List<Pendencia>();
                        List<Pendencia> Indicadores = new List<Pendencia>();

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
                                                        Pendencia Pendencia = new Pendencia()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            TipoPendencia = TipoPendencia.Pendencia,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(Pendencia);
                                                    }
                                                }
                                                break;
                                            case 2:
                                                if ((mes % 2) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        Pendencia Pendencia = new Pendencia()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            TipoPendencia = TipoPendencia.Pendencia,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(Pendencia);
                                                    }
                                                }
                                                break;
                                            case 3:
                                                if ((mes % 3) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        Pendencia Pendencia = new Pendencia()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            TipoPendencia = TipoPendencia.Pendencia,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(Pendencia);
                                                    }
                                                }
                                                break;
                                            case 4:
                                                if ((mes % 6) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        Pendencia Pendencia = new Pendencia()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            TipoPendencia = TipoPendencia.Pendencia,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(Pendencia);
                                                    }
                                                }
                                                break;
                                            case 5:
                                                if ((mes % 12) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        Pendencia Pendencia = new Pendencia()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            TipoPendencia = TipoPendencia.Pendencia,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(Pendencia);
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
                        lstPendencia.AddRange(FornecedorP.ToList());
                        lstPendencia.AddRange(FornecedoresLembrete.ToList());
                        //lstPendencia.AddRange(AnaliseCritica.ToList());
                        //lstPendencia.AddRange(AnaliseCriticaPendencia.ToList());
                        lstPendencia.AddRange(gestaoRisco.ToList());
                        lstPendencia.AddRange(gestaoRiscoPrazo.ToList());
                        lstPendencia.AddRange(gestaoRiscoReverificacao.ToList());
                        lstPendencia.AddRange(gestaoMelhoria.ToList());
                        lstPendencia.AddRange(gestaoMelhoriaPrazo.ToList());
                        lstPendencia.AddRange(gestaoMelhoriaReverificacao.ToList());
                        lstPendencia.AddRange(docPendencia);

                        lstPendencia = lstPendencia.GroupBy(x => x.Id).Select(j => new Pendencia()
                        {
                            Id = j.First().Id,
                            IdResponsavel = j.First().IdResponsavel,
                            TipoPendencia = j.First().TipoPendencia,
                            Modulo = j.First().Modulo,
                            Titulo = j.First().Titulo,
                            Url = j.First().Url
                        }).ToList();


                        return lstPendencia;
                    }
                    else
                    {
                        return new List<Pendencia>();
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AlterarPendencia(int idUsuarioDestino, int idUsuarioOrigem, int idSite, int idCliente)
        {

            var Pendencias = CarregaPendencia(idSite, idCliente).Where(x => x.IdResponsavel == idUsuarioOrigem);


            using (var db = new BaseContext())
            {

                if (idSite != 0)
                {

                    DateTime LastDay = DateTime.Now.AddDays(-1);

                    var DtVencimento = DateTime.Now.AddDays(-1);

                    var licenca = (from lc in db.Licenca
                                   where lc.DataVencimento.Value < DtVencimento && lc.Idcliente == idCliente && lc.Idcliente == idCliente && lc.IdResponsavel == idUsuarioOrigem
                                   select lc).ToList();

                    licenca.ForEach(a => a.IdResponsavel = idUsuarioDestino);

                    var indicadores = (from ind in db.Indicador
                                       join per in db.PeriodicidaDeAnalise on ind.Id equals per.IdIndicador
                                       join meta in db.PlanoVoo on per.Id equals meta.IdPeriodicidadeAnalise
                                       where meta.DataReferencia < DateTime.Now && meta.Realizado == null && ind.IdSite == idSite && ind.IdResponsavel == idUsuarioOrigem
                                       select ind).ToList();

                    indicadores.ForEach(a => a.IdResponsavel = idUsuarioDestino);

                    //Docdocumento Resolvido abaixo
                    var docDocumentoAprov = (from doc in db.DocDocumento
                                             where doc.StatusRegistro == (int)StatusDocumento.Aprovacao && doc.IdSite == idSite
                                             select doc);

                    var docDocumentoVer = (from doc in db.DocDocumento
                                           where doc.StatusRegistro == (int)StatusDocumento.Verificacao && doc.IdSite == idSite
                                           select doc);

                    var doDocumentoRev = (from doc in db.DocDocumento
                                          where doc.FlRevisaoPeriodica == true && doc.DtNotificacao < DateTime.Now && doc.IdSite == idSite
                                          select doc);

                    var naoConformidade = (from nc in db.RegistroConformidade
                                           where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "nc" && nc.ResponsavelInicarAcaoImediata.IdUsuario == idUsuarioOrigem
                                           select nc).ToList();

                    naoConformidade.ForEach(a => a.IdResponsavelInicarAcaoImediata = idUsuarioDestino);

                    var naoConformidadePrazo = (from nc in db.RegistroConformidade
                                                join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                                where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "nc" && ai.ResponsavelImplementar.IdUsuario == idUsuarioOrigem
                                                select ai).ToList();

                    naoConformidadePrazo.ForEach(a => a.IdResponsavelImplementar = idUsuarioDestino);

                    var naoConformidadeReverificacao = (from nc in db.RegistroConformidade
                                                        where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "nc" && nc.IdSite == idSite && nc.ResponsavelReverificador.IdUsuario == idUsuarioOrigem
                                                        select nc).ToList();

                    naoConformidadeReverificacao.ForEach(a => a.IdResponsavelReverificador = idUsuarioDestino);

                    var acaoCorretiva = (from ac in db.RegistroConformidade
                                         join ai in db.AcaoImediata on ac.IdRegistroConformidade equals ai.IdRegistroConformidade
                                         where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && ac.IdSite == idSite && ac.TipoRegistro == "ac" && ai.ResponsavelImplementar.IdUsuario == idUsuarioOrigem
                                         select ac).ToList();

                    acaoCorretiva.ForEach(a => a.IdResponsavelImplementar = idUsuarioDestino);

                    var acaoCorretivaRev = (from ac in db.RegistroConformidade
                                            where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && ac.TipoRegistro == "ac" && ac.DtPrazoImplementacao < DateTime.Now && ac.IdSite == idSite && ac.ResponsavelImplementar.IdUsuario == idUsuarioOrigem
                                            select ac).ToList();

                    acaoCorretivaRev.ForEach(a => a.IdResponsavelImplementar = idUsuarioDestino);

                    var data15 = DateTime.Now.AddDays(-15).Day;

                    var Auditoria = (from plai in db.Plai
                                     where plai.DataReuniaoAbertura.Day <= data15 && plai.Pai.IdSite == idSite && plai.IdElaborador == idUsuarioOrigem
                                     select plai);

                    var AuditoriaResult = Auditoria.ToList();

                    AuditoriaResult.ForEach(a => a.IdElaborador = idUsuarioDestino);

                    var AnaliseCritica = (from anc in db.AnaliseCritica
                                          where anc.IdSite == idSite && anc.DataProximaAnalise == LastDay && anc.IdResponsavel == idUsuarioOrigem
                                          select anc).ToList();

                    AnaliseCritica.ForEach(a => a.IdResponsavel = idUsuarioDestino);

                    var InstrumentoQuery = (from ins in db.Instrumento
                                            join cal in db.Calibracao on ins.IdInstrumento equals cal.IdInstrumento
                                            where ins.IdSite == idSite && ins.Status == (byte)EquipamentoStatus.NaoCalibrado && ins.IdResponsavel == idUsuarioOrigem
                                            group ins by ins.IdInstrumento into idIsntrumentoOrder
                                            select idIsntrumentoOrder).ToList();


                    var Instrumento = new List<Pendencia>();
                    foreach (var item in InstrumentoQuery)
                    {
                        var a = item.FirstOrDefault();

                        a.IdResponsavel = idUsuarioDestino;
                    }

                    var Fornecedores = (from forn in db.Fornecedor
                                        join avaq in db.AvaliaCriterioQualificacao on forn.IdFornecedor equals avaq.IdFornecedor
                                        join prodf in db.ProdutoFornecedor on forn.IdFornecedor equals prodf.IdFornecedor
                                        where avaq.IdResponsavelPorControlarVencimento != null && avaq.DtVencimento != null && avaq.DtVencimento <= DateTime.Now && forn.IdSite == idSite && avaq.IdResponsavelPorQualificar == idUsuarioOrigem
                                        select avaq).ToList();

                    Fornecedores.ForEach(a => a.IdResponsavelPorQualificar = idUsuarioDestino);

                    var FornecedoresVal = (from forn in db.Fornecedor
                                           join avaa in db.AvaliaCriterioAvaliacao on forn.IdFornecedor equals avaa.IdFornecedor
                                           join prodf in db.ProdutoFornecedor on forn.IdFornecedor equals prodf.IdFornecedor
                                           where avaa.DtProximaAvaliacao == DateTime.Now && avaa.IdUsuarioAvaliacao != null && avaa.IdUsuarioAvaliacao == idUsuarioOrigem
                                           select avaa).ToList();

                    FornecedoresVal.ForEach(a => a.IdUsuarioAvaliacao = idUsuarioDestino);

                    var gestaoRisco = (from nc in db.RegistroConformidade
                                       where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "gr" && nc.ResponsavelInicarAcaoImediata.IdUsuario == idUsuarioOrigem
                                       select nc).ToList();

                    gestaoRisco.ForEach(a => a.IdResponsavelInicarAcaoImediata = idUsuarioDestino);

                    var gestaoRiscoPrazo = (from nc in db.RegistroConformidade
                                            join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                            where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "gr" && ai.ResponsavelImplementar.IdUsuario == idUsuarioOrigem
                                            select nc).ToList();

                    gestaoRiscoPrazo.ForEach(a => a.IdResponsavelImplementar = idUsuarioDestino);

                    var gestaoRiscoReverificacao = (from nc in db.RegistroConformidade
                                                    where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gr" && nc.IdSite == idSite && nc.ResponsavelReverificador.IdUsuario == idUsuarioOrigem
                                                    select nc).ToList();

                    gestaoRiscoReverificacao.ForEach(a => a.IdResponsavelReverificador = idUsuarioDestino);


                    var gestaoMelhoria = (from nc in db.RegistroConformidade
                                          where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "gm" && nc.ResponsavelInicarAcaoImediata.IdUsuario == idUsuarioOrigem
                                          select nc).ToList();

                    gestaoMelhoria.ForEach(a => a.IdResponsavelInicarAcaoImediata = idUsuarioDestino);


                    var gestaoMelhoriaPrazo = (from nc in db.RegistroConformidade
                                               join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                               where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "gm" && ai.ResponsavelImplementar.IdUsuario == idUsuarioOrigem
                                               select nc).ToList();

                    gestaoMelhoriaPrazo.ForEach(a => a.IdResponsavelImplementar = idUsuarioDestino);

                    var gestaoMelhoriaReverificacao = (from nc in db.RegistroConformidade
                                                       where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gm" && nc.IdSite == idSite && nc.ResponsavelImplementar.IdUsuario == idUsuarioOrigem
                                                       select nc).ToList();

                    gestaoMelhoriaReverificacao.ForEach(a => a.IdResponsavelImplementar = idUsuarioDestino);


                    List<Pendencia> docPendencia = new List<Pendencia>();

                    foreach (var docap in docDocumentoAprov.ToList())
                    {
                        if (docap.DocUsuarioVerificaAprova != null)
                        {
                            var docResult = docap.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A" && x.IdUsuario == idUsuarioOrigem).OrderBy(x => x.Usuario.NmCompleto).ToList();

                            foreach (var res in docResult)
                            {

                                res.IdUsuario = idUsuarioDestino;
                            }
                        }
                    }

                    foreach (var docver in docDocumentoVer.ToList())
                    {
                        if (docver.DocUsuarioVerificaAprova != null)
                        {
                            var docResult = docver.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "V" && x.IdUsuario == idUsuarioOrigem).OrderBy(x => x.Usuario.NmCompleto).ToList();

                            foreach (var res in docResult)
                            {
                                res.IdUsuario = idUsuarioDestino;
                            }
                        }
                    }

                    foreach (var docrev in doDocumentoRev.ToList().Where(x => x.IdElaborador == idUsuarioOrigem))
                    {
                        docrev.IdElaborador = idUsuarioDestino;
                    }


                    Dictionary<int, bool> queryIndicadores = new Dictionary<int, bool>();

                    var indicadores1 = (from ind in db.Indicador
                                        where ind.IdSite == idSite && ind.IdResponsavel == idUsuarioOrigem
                                        select ind).ToList();
                    List<Pendencia> lstPendencia = new List<Pendencia>();

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
                                                    indicador.IdResponsavel = idUsuarioDestino;

                                                }
                                            }
                                            break;
                                        case 2:
                                            if ((mes % 2) == 0)
                                            {
                                                if (mes != DateTime.Now.Month)
                                                {
                                                    indicador.IdResponsavel = idUsuarioDestino;
                                                }
                                            }
                                            break;
                                        case 3:
                                            if ((mes % 3) == 0)
                                            {
                                                if (mes != DateTime.Now.Month)
                                                {
                                                    indicador.IdResponsavel = idUsuarioDestino;
                                                }
                                            }
                                            break;
                                        case 4:
                                            if ((mes % 6) == 0)
                                            {
                                                if (mes != DateTime.Now.Month)
                                                {
                                                    indicador.IdResponsavel = idUsuarioDestino;
                                                }
                                            }
                                            break;
                                        case 5:
                                            if ((mes % 12) == 0)
                                            {
                                                if (mes != DateTime.Now.Month)
                                                {
                                                    indicador.IdResponsavel = idUsuarioDestino;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    db.SaveChanges();
                }
            }
        }


    }
}
