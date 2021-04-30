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
using System.Threading.Tasks;
using DAL.Context;
using System.Activities.Debugger;
using System.Data.Entity;
using System.IO;
using Microsoft.Office.Interop.Word;
using Web.UI.Backup;
using System.Text;
using System.Configuration;
using Ionic.Zip;

namespace Web.UI.Controllers
{
    public class BackupController : BaseController
    {

        private readonly IAnaliseCriticaAppServico _analiseCriticaAppServico;
        private readonly IDocDocumentoAppServico _documentoAppServico;
        private readonly IDocDocumentoServico _documentoServico;
        private readonly IProcessoServico _processoServico;
        private readonly IRegistroConformidadesAppServico _registroConformidadeAppServico;
        private readonly IRegistroConformidadesServico _registroConformidadeServico;
        private readonly IDocUsuarioVerificaAprovaServico _docUsuarioVerificaAprovaServico;
        private readonly ICargoAppServico _cargoAppServico;
        private readonly IDocTemplateAppServico _docTemplateAppServico;
        private readonly IDocUsuarioVerificaAprovaAppServico _docUsuarioVerificaAprovaAppServico;
        public IUsuarioAppServico _usuarioAppServico;
        private readonly IControleImpressaoAppServico _controleImpressaoAppServico;
        private readonly IControleImpressaoServico _controleImpressaoServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IDocCargoAppServico _docCargoAppServico;
        private readonly IDocumentoAssuntoAppServico _documentoAssuntoAppServico;
        private readonly IDocumentoComentarioAppServico _documentoComentarioAppServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IAnexoAppServico _AnexoAppServico;
        private readonly ISiteAppServico _siteAppServico;
        private readonly IIndicadorAppServico _indicadorAppServico;
        private readonly IBackupFactory _backupFactory;
        private readonly IClienteAppServico _clienteAppServico;
        private readonly IInstrumentoAppServico _instrumentoAppServico;
        private readonly IPendenciaAppServico _pendenciaAppServico;

        public BackupController(IDocDocumentoAppServico docDocumentoAppServico,
                                    IDocDocumentoServico documentoServico,
                                    IRegistroConformidadesAppServico registroConformidadeAppServico,
                                    ICargoAppServico cargoAppServico,
                                    IDocTemplateAppServico docTemplate,
                                    IDocUsuarioVerificaAprovaAppServico docUsuarioVerificaAprovaAppServico,
                                    IUsuarioAppServico usuarioAppServico,
                                    IControleImpressaoAppServico controleImpressaoAppServico,
                                    ILogAppServico logAppServico,
                                    IDocCargoAppServico docCargoAppServico,
                                    IDocumentoAssuntoAppServico documentoAssuntoAppServico,
                                    IDocumentoComentarioAppServico documentoComentarioAppServico,
                                    IControleImpressaoServico controleImpressaoServico,
                                    IProcessoServico processoServico,
                                    IRegistroConformidadesServico registroConformidadeServico,
                                    IUsuarioClienteSiteAppServico usuarioClienteAppServico,
                                    IProcessoAppServico processoAppServico,
                                    IDocUsuarioVerificaAprovaServico docUsuarioVerificaAprovaServico,
                                    IControladorCategoriasAppServico controladorCategoriasServico,
                                    IAnexoAppServico anexoAppServico,
                                    ISiteAppServico siteAppServico,
                                    IBackupFactory backupFactory,
                                    IClienteAppServico clienteAppServico,
                                    IAnaliseCriticaAppServico analiseCriticaAppServico,
                                    IIndicadorAppServico indicadorAppServico,
                                    IPendenciaAppServico pendenciaAppServico,
                                    IInstrumentoAppServico instrumentoAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico, pendenciaAppServico)
        {
            _AnexoAppServico = anexoAppServico;
            _documentoAppServico = docDocumentoAppServico;
            _registroConformidadeAppServico = registroConformidadeAppServico;
            _cargoAppServico = cargoAppServico;
            _docTemplateAppServico = docTemplate;
            _docUsuarioVerificaAprovaAppServico = docUsuarioVerificaAprovaAppServico;
            _usuarioAppServico = usuarioAppServico;
            _controleImpressaoAppServico = controleImpressaoAppServico;
            _logAppServico = logAppServico;
            _docCargoAppServico = docCargoAppServico;
            _documentoAssuntoAppServico = documentoAssuntoAppServico;
            _documentoComentarioAppServico = documentoComentarioAppServico;
            _controleImpressaoServico = controleImpressaoServico;
            _documentoServico = documentoServico;
            _processoServico = processoServico;
            _registroConformidadeServico = registroConformidadeServico;
            _usuarioClienteAppServico = usuarioClienteAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _docUsuarioVerificaAprovaServico = docUsuarioVerificaAprovaServico;
            _siteAppServico = siteAppServico;
            _backupFactory = backupFactory;
            _clienteAppServico = clienteAppServico;
            _analiseCriticaAppServico = analiseCriticaAppServico;
            _indicadorAppServico = indicadorAppServico;
            _instrumentoAppServico = instrumentoAppServico;
            _pendenciaAppServico = pendenciaAppServico;
        }


        // GET: Backup
        public ActionResult Index(int idCliente)
        {

            var pathBackup = CriarEstruturaBackup(idCliente);
            var docTemplate = Server.MapPath("~/Templates/Backup/DocTemplate.html");
            var RegTemplate = Server.MapPath("~/Templates/Backup/IdentificacaoNaoConformidade.html");
            var AnaliseTemplate = Server.MapPath("~/Templates/Backup/AnaliseCritica.html");
            var LicencaTemplate = Server.MapPath("~/Templates/Backup/CadastroLicencas.html");
            var IndicadorTemplate = Server.MapPath("~/Templates/Backup/Indicador.html");
            var InstrumentoTemplate = Server.MapPath("~/Templates/Backup/Instrumento.html");


            /*Cria Backup dos registros*/
            GerarControlDoc(pathBackup, docTemplate, idCliente);
            GerarRegistros(pathBackup, RegTemplate, idCliente);
            GerarAnaliseTemplate(pathBackup, AnaliseTemplate, idCliente);
            GerarIndicadorTemplate(pathBackup, IndicadorTemplate, idCliente);
            GerarInstrumentoTemplate(pathBackup, InstrumentoTemplate, idCliente);
            _AnexoAppServico.BackupAnexo(idCliente, pathBackup);

            //var arquivo = CompactarDiretorio()
            //var url = GerarUrlDownload(arquivo);

            return View();
        }


        public void IniciaCriacaoBackup(string path, string pathTemplate, int idCliente)
        {

        }
        public string CriarEstruturaBackup(int idCliente)
        {

            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();
            var nameBackup = "Backup-" + siteSelecionado.NmRazaoSocial;

            Util.VerificaDiretorio(Server.MapPath("~/" + nameBackup.TrimEnd() + "/" + idCliente));


            return Server.MapPath("~/" + nameBackup.TrimEnd() + "/" + idCliente).ToString();
        }
        public void GerarControlDoc(string path, string pathTemplate, int idCliente)
        {

            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();
            var clienteLogo = _clienteAppServico.GetById(idCliente);
            var Documentos = _documentoAppServico.Get(x => x.IdSite == siteSelecionado.IdSite).ToList();


            foreach (var documento in Documentos)
            {
                var modelDoc = new BackupModel
                {
                    CaminhoTemplate = pathTemplate,
                    CaminhoBackup = path + "\\ControlDoc",
                    Informacoes = new List<InformacaoBackupModel>()
                };


                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##NOMEEMPRESA##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = clienteLogo.NmFantasia
                });

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##REVISAO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = documento.NuRevisao.ToString()
                });

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##TITULO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = documento.Titulo.ToString()
                });

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##SIGLA##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = documento.Sigla.Descricao.ToString()
                });

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##NUMERO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = documento.NumeroDocumento.ToString()
                });

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##PROCESSO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = documento.Processo.Nome.ToString()
                });

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##Categoria##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = documento.Categoria.Descricao.ToString()
                });

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##Elaborador##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = documento.Elaborador.NmCompleto.ToString()
                });

                string textoDoc = string.Empty;

                if (documento.TextoDoc != null)
                {
                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    Encoding utf8 = Encoding.UTF8;
                    byte[] bytes = Encoding.Default.GetBytes(documento.TextoDoc);
                    byte[] isoBytes = Encoding.Convert(utf8, iso, bytes);
                    textoDoc = iso.GetString(isoBytes);
                }

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##TEXTO##",
                    Tipo = TipoInformacaoBackup.CkEditor,
                    Valor = textoDoc
                });

                string fluxoDoc = documento.FluxoDoc != null ? documento.FluxoDoc : string.Empty;

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##FLUXO##",
                    Tipo = TipoInformacaoBackup.CkEditor,
                    Valor = fluxoDoc.ToString()
                });

                string registrotbl = string.Empty;

                foreach (var registro in documento.Registros)
                {
                    registrotbl = registrotbl + string.Format(@"<tr>
                    <td style='width: 16.666 %;'>{0}</td>
                      <td style ='width: 16.6667%;'> {1}</td>
                      <td style ='width: 16.6667%;'> {2} </td>
                      <td style ='width: 16.6667%;'> {3}</td>
                      <td style ='width: 16.6667%;'> {4}</td>
                      <td style ='width: 16.6667%;'> {5}</td>
                       </tr>", registro.Identificar, registro.Armazenar, registro.Proteger, registro.Recuperar, registro.Retencao, registro.Disposicao);
                }

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##REGISTROTBL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = registrotbl
                });

                string rotinatbl = string.Empty;

                foreach (var rotina in documento.Rotinas)
                {
                    registrotbl = registrotbl + string.Format(@"<tr>
                    <td style='width: 20%;'>{0}</td>
                      <td style ='width: 20%;'> {1}</td>
                      <td style ='width: 20%;'> {2} </td>
                      <td style ='width: 20%;'> {3}</td>
                      <td style ='width: 20%;'> {4}</td>
                       </tr>", rotina.Item, rotina.OQue, rotina.Quem, rotina.Registro, rotina.Como);
                }

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##ROTINATBL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = rotinatbl
                });

                string recursoDoc = string.Empty;

                if (documento.RecursoDoc != null)
                {
                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    Encoding utf8 = Encoding.UTF8;
                    byte[] bytes = Encoding.Default.GetBytes(documento.RecursoDoc);
                    byte[] isoBytes = Encoding.Convert(utf8, iso, bytes);
                    recursoDoc = iso.GetString(isoBytes);
                }

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##RECURSO##",
                    Tipo = TipoInformacaoBackup.CkEditor,
                    Valor = recursoDoc
                });

                string riscotbl = string.Empty;

                foreach (var risco in documento.DocRisco)
                {
                    registrotbl = registrotbl + string.Format(@"<tr>
                    <td style='width: 16.6667%;'>{0}</td>
                      <td style ='width: 16.6667%;'> {1}</td>
                      <td style ='width: 16.6667%;'> {2} </td>
                      <td style ='width: 16.6667%;'> {3}</td>
                      <td style ='width: 16.6667%;'> {4}</td>
                      <td style ='width: 16.6667%;'> {5}</td>
                       </tr>", risco.DescricaoRegistro, risco.CriticidadeGestaoDeRisco, risco.PossuiGestaoRisco ? "Sim" : "Não", risco.ResponsavelInicarAcaoImediataNomeCompleto, risco.Causa, risco.DsJustificativa);
                }

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##RISCOTBL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = riscotbl
                });

                string nmArquivo = documento.Sigla.Descricao + " " + documento.NumeroDocumento + "- rev" + documento.NuRevisao;

                _backupFactory.GerarBackupArquivo(modelDoc, nmArquivo);
            }
        }
        public void GerarRegistros(string path, string pathTemplate, int idCliente)
        {

            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();
            var Registros = _registroConformidadeAppServico.Get(x => x.IdSite == siteSelecionado.IdSite).ToList();
            var clienteLogo = _clienteAppServico.GetById(idCliente);

            foreach (var registro in Registros)
            {
                switch (registro.TipoRegistro.ToLower().ToString())
                {
                    case "ac":

                        var modelRegAC = new BackupModel
                        {
                            CaminhoTemplate = pathTemplate,
                            CaminhoBackup = path + "\\AcaoCorretiva",
                            Informacoes = new List<InformacaoBackupModel>()
                        };
                        ExtrairRegistro(siteSelecionado, clienteLogo, registro, modelRegAC);

                        break;
                    case "nc":
                        var modelRegNC = new BackupModel
                        {
                            CaminhoTemplate = pathTemplate,
                            CaminhoBackup = path + "\\NaoConformidade",
                            Informacoes = new List<InformacaoBackupModel>()
                        };
                        ExtrairRegistro(siteSelecionado, clienteLogo, registro, modelRegNC);
                        break;
                    case "gr":
                        var modelRegGR = new BackupModel
                        {
                            CaminhoTemplate = pathTemplate,
                            CaminhoBackup = path + "\\GestaoRisco",
                            Informacoes = new List<InformacaoBackupModel>()
                        };
                        ExtrairRegistro(siteSelecionado, clienteLogo, registro, modelRegGR);
                        break;
                    case "gm":
                        var modelRegGM = new BackupModel
                        {
                            CaminhoTemplate = pathTemplate,
                            CaminhoBackup = path + "\\GestaoMelhoria",
                            Informacoes = new List<InformacaoBackupModel>()
                        };
                        ExtrairRegistro(siteSelecionado, clienteLogo, registro, modelRegGM);
                        break;
                    default:
                        break;
                }
            }
        }
        private void ExtrairRegistro(Site siteSelecionado, Cliente clienteLogo, RegistroConformidade registro, BackupModel modelDoc)
        {
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##NOMEEMPRESA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = clienteLogo.NmFantasia
            });

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##NUMERO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.NuRegistro.ToString()
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##DATAEMISSAO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.DtEmissao.ToString()
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##PROCESSO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.Processo.Nome
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##EMISSORNAOCONFORMIDADE##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.Emissor.NmCompleto
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##NAOCONFORMIDADEAUDITORIA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.ENaoConformidadeAuditoria == true ? "Sim" : "Não"
            });
            string TpNaoConformidade = "";
            if (registro.TipoNaoConformidade != null)
            {
                TpNaoConformidade = registro.TipoNaoConformidade.Descricao;
            }
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##TIPONAOCONFORMIDADE##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = TpNaoConformidade
            }) ;

            var ResponsavelAnalise = string.Empty;

            if (registro.ResponsavelAnalisar != null)
            {
                ResponsavelAnalise = registro.ResponsavelAnalisar.NmCompleto;
            }

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##RESPONSAVELANALISEDEFINICAOACAOIMEDIATA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = ResponsavelAnalise
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##TAGS##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.Tags
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##DESCRICAOREGISTRO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.DescricaoRegistro
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##NAOCONFORMIDADEPROCEDENTE##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.EProcedente == true ? "Sim" : "Não"
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##DATADESCRICAOACAO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.DtDescricaoAcao.ToString()
            });

            var statusReg = string.Empty;
            switch (registro.StatusRegistro)
            {
                case 1:
                    statusReg = "Ação Imediata";
                    break;
                case 2:
                    statusReg = "Implementação";
                    break;
                case 3:
                    statusReg = "Reverificação";
                    break;
                case 4:
                    statusReg = "Encerrada";
                    break;
                default:
                    break;
            }

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##STATUS##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = statusReg
            });

            string acaoImediataTbl = string.Empty;


            foreach (var acoes in registro.AcoesImediatas)
            {
                var nmImplementar = string.Empty;
                if (acoes.ResponsavelImplementar != null)
                {
                    nmImplementar = acoes.ResponsavelImplementar.NmCompleto;
                }
                acaoImediataTbl = acaoImediataTbl + string.Format(@"<tr><td style='width: 20 %; '>{0}</td><td style ='width: 20%;'>{1}</td><td style ='width: 20%;'>{2}</td><td style ='width: 20%;'>{3}</td><td style ='width: 20%;'>{4}</td></tr>", acoes.Descricao, acoes.DtPrazoImplementacao, nmImplementar, acoes.DtEfetivaImplementacao, acoes.Observacao);
            }

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##TBLACAOIMEDIATA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = acaoImediataTbl
            });

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##ACAOIMEDIATAECORRECAO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.ECorrecao == true ? "Sim" : "Não"
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##NECESSITACORRECAO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.NecessitaAcaoCorretiva == true ? "Sim" : "Não"
            });

            string respRV = registro.ResponsavelReverificador == null ? "" : registro.ResponsavelReverificador.NmCompleto;

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##RESPONSAVELREVERIFICACAO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = respRV
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##FOIEFICAZ##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.FlEficaz == true ? "Sim" : "Não"
            });

            string respIA = "";

            respIA = registro.ResponsavelInicarAcaoImediata == null ? "" : registro.ResponsavelInicarAcaoImediata.NmCompleto;

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##RESPONSAVELINICIARTRATATIVAACAOCORRETIVA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = respIA
            });

            var nuAcaoCorretiva = _registroConformidadeAppServico.GetAll().FirstOrDefault(x => x.IdSite == siteSelecionado.IdSite && x.TipoRegistro == "ac" && x.NuRegistro == registro.IdNuRegistroFilho);

            var nuAcao = 0;

            if (nuAcaoCorretiva != null)
            {

                nuAcao = nuAcaoCorretiva.NuRegistro;
            }

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##NUMEROACAOCORRETIVA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = nuAcao.ToString()
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##ANALISECAUSA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.DescricaoAnaliseCausa
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##PARECER##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.Parecer
            });

            var nmArquivo = registro.Processo.Nome + " " + registro.NuRegistro;

            _backupFactory.GerarBackupArquivo(modelDoc , nmArquivo);
        }
        public void GerarAnaliseTemplate(string path, string pathTemplate, int idCliente)
        {
            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();
            var AnaliseCritica = _analiseCriticaAppServico.Get(x => x.IdSite == siteSelecionado.IdSite).ToList();
            var clienteLogo = _clienteAppServico.GetById(idCliente);


            foreach (var analise in AnaliseCritica)
            {
                var modelDoc = new BackupModel
                {
                    CaminhoTemplate = pathTemplate,
                    CaminhoBackup = path + "\\AnaliseCritica",
                    Informacoes = new List<InformacaoBackupModel>()
                };
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##RESPONSAVEL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = analise.Responsavel.NmCompleto
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##ATAANALISECRITICASG##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = analise.Ata
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##DATACRIACAO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = analise.DataCriacao.ToString()
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##DATAPROXIMAANALISE##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = analise.DataProximaAnalise.ToString()
                });

                string FuncionarioHeader = @"<table style='border-collapse: collapse;width: 100%;height: 50px; margin-bottom: 20px;'>
					<tr style='height: 18px;'>
						<td style='width: 50%;height: 18px;'>Nome</td>
						<td style='width: 50%; height: 18px;'>Funcao</td>
					</tr>";
                string FuncionarioBody = "";
                string FuncionarioFooter = @"</table>";

                foreach (var funcionario in analise.Funcionarios)
                {
                    FuncionarioBody = FuncionarioBody + string.Format(@"<tr style='height: 18px;'>
						<td style='width: 50%;height: 18px;'>{0}</td>
						<td style='width: 50%; height: 18px;'>{1}</td>
					</tr>", funcionario.Funcionario.NmCompleto, funcionario.Funcionario.NmFuncao);
                }

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##TBLUSUARIOS##",
                    Tipo = TipoInformacaoBackup.CkEditor,
                    Valor = FuncionarioHeader + FuncionarioBody + FuncionarioFooter
                });

                string Tema = "";

                foreach (var tema in analise.Temas)
                {
                    Tema = Tema + string.Format(@"<p><h4>{0}</h4></p>
                        <p>{1}</p>", tema.ControladorCategoria.Descricao, tema.Descricao);
                }

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##TBLTEMAS##",
                    Tipo = TipoInformacaoBackup.CkEditor,
                    Valor = Tema
                });

                _backupFactory.GerarBackupArquivo(modelDoc , analise.Ata);
            }
        }
        public void GerarIndicadorTemplate(string path, string pathTemplate, int idCliente)
        {
            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();
            var Indicadores = _indicadorAppServico.Get(x => x.IdSite == siteSelecionado.IdSite).ToList();
            var clienteLogo = _clienteAppServico.GetById(idCliente);


            foreach (var indicador in Indicadores)
            {
                var modelDoc = new BackupModel
                {
                    CaminhoTemplate = pathTemplate,
                    CaminhoBackup = path + "\\Indicador",
                    Informacoes = new List<InformacaoBackupModel>()
                };

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##PROCESSO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.Processo.Nome.ToString()
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##RESPONSAVEL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.Responsavel.NmCompleto.ToString()
                });


                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##PERIODICIDADEMEDICAO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = Enum.GetName(typeof(PeriodicidadeMedicao), indicador.PeriodicidadeMedicao)
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##PERIODICIDADEANALISE##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = Enum.GetName(typeof(PeriodicidadeMedicao), indicador.Periodicidade)
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##SENTIDOMETA##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.Direcao == 1 ? "Melhor pra Cima" : "Melhor pra Baixo"
                });

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##OBJETIVO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.Objetivo.ToString()
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##INDICADOR##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.Descricao.ToString()
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##ANO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.Ano.ToString()
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##META##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.MetaAnual.ToString()
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##UNIDADE##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.Unidade.ToString()
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##TIPOMEDIDA##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = indicador.Unidade
                });

                var temPeriodicidadeAnalise = indicador.PeriodicidadeDeAnalises.Count > 0;
                var listaPeriodicidade = new List<PeriodicidaDeAnalise>();
                var metas = new List<Meta>();
                var realizados = new List<PlanoVoo>();
                if (temPeriodicidadeAnalise)
                {
                    listaPeriodicidade.AddRange(indicador.PeriodicidadeDeAnalises);
                    metas.AddRange(listaPeriodicidade[0].PlanoDeVoo);
                    realizados.AddRange(listaPeriodicidade[0].MetasRealizadas);
                }

                var janPV = metas[0].Valor.ToString();
                var fevPV = metas[1].Valor.ToString();
                var marPV = metas[2].Valor.ToString();
                var abrPV = metas[3].Valor.ToString();
                var maiPV = metas[4].Valor.ToString();
                var junPV = metas[5].Valor.ToString();
                var julPV = metas[6].Valor.ToString();
                var agoPV = metas[7].Valor.ToString();
                var outPV = metas[8].Valor.ToString();
                var setPV = metas[9].Valor.ToString();
                var novPV = metas[10].Valor.ToString();
                var dezPV = metas[11].Valor.ToString();

                var janRL = realizados[0].Realizado.ToString();
                var fevRL = realizados[1].Realizado.ToString();
                var marRL = realizados[2].Realizado.ToString();
                var abrRL = realizados[3].Realizado.ToString();
                var maiRL = realizados[4].Realizado.ToString();
                var junRL = realizados[5].Realizado.ToString();
                var julRL = realizados[6].Realizado.ToString();
                var agoRL = realizados[7].Realizado.ToString();
                var outRL = realizados[8].Realizado.ToString();
                var setRL = realizados[9].Realizado.ToString();
                var novRL = realizados[10].Realizado.ToString();
                var dezRL = realizados[11].Realizado.ToString();

                /*PLANO DE VOO*/
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##JANEIRO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = janPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##FAVEREIRO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = fevPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##MARCO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = marPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##ABRIL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = abrPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##MAIO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = maiPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##JUNHO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = junPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##JULHO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = julPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##AGOSTO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = agoPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##SETEMBRO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = setPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##OUTUBRO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = outPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##NOVEMBRO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = novPV
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##DEZEMBRO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = dezPV
                });


                /*REALIZADO*/
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##JANEIRORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = janRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##FAVEREIRORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = fevRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##MARCORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = marRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##ABRILRL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = abrRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##MAIORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = maiRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##JUNHORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = junRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##JULHORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = julRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##AGOSTORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = agoRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##SETEMBRORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = setRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##OUTUBRORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = outRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##NOVEMBRORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = novRL
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##DEZEMBRORL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = dezRL
                });

                string nmArquivo = "Indicador" + "-" + indicador.Id;

                _backupFactory.GerarBackupArquivo(modelDoc , nmArquivo);
            }
        }
        public void GerarInstrumentoTemplate(string path, string pathTemplate, int idCliente)
        {
            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();
            var Instrumentos = _instrumentoAppServico.Get(x => x.IdSite == siteSelecionado.IdSite).ToList();
            var clienteLogo = _clienteAppServico.GetById(idCliente);


            foreach (var instrumento in Instrumentos)
            {
                var modelDoc = new BackupModel
                {
                    CaminhoTemplate = pathTemplate,
                    CaminhoBackup = path + "\\Instrumento",
                    Informacoes = new List<InformacaoBackupModel>()
                };

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##INSTRUMENTO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.Equipamento
                });

                string statusInst = "";

                if (instrumento.Status == 0)
                {
                    statusInst = "Não Calibrado";
                }
                else if (instrumento.Status == 1)
                {
                    statusInst = "Calibrado";
                }
                else if (instrumento.Status == 2)
                {
                    statusInst = "Fora de Uso";
                }
                else if (instrumento.Status == 3)
                {
                    statusInst = "Em Calibração";
                }
                else
                {
                    statusInst = "Não Calibrado";
                }


                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##STATUSINSTRUMENTO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = statusInst
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##SIGLA##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.Sigla.Descricao
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##NUMERO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.Numero.ToString()
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##MARCA##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.Marca
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##MODELO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.Modelo
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##RESPONSAVEL##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = _usuarioAppServico.GetById((int)instrumento.IdResponsavel).NmCompleto
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##LOCALUSO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.LocalDeUso
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##ESCALA##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.Escala
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##MENORDIVISAO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.MenorDivisao
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##CRITERIOACEITACAO##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.valorAceitacao
                });
                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##OBS##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instrumento.DescricaoCriterio
                });

                string instheadertbl = @"<table style='border-collapse: collapse;width: 100%;height: 50px; margin-bottom: 20px;'>
				<tr style='height: 18px;'>
					<td style='width: 16.66; height: 18px;'>Nº Certificado</td>
					<td style='width: 16.66; height: 18px;'>Data de Calibração</td>
					<td style='width: 16.66; height: 18px;'>Próxima Calibração</td>
					<td style='width: 16.66; height: 18px;'>Orgão Calibrador</td>
					<td style='width: 16.66; height: 18px;'>Aprovado Por</td>
					<td style='width: 16.66; height: 18px;'>Aprovado</td>
				</tr>";
                string instbodytbl = "";
                string instFooter = @"</table>";
                foreach (var calibracao in instrumento.Calibracao)
                {
                    instbodytbl = instbodytbl + string.Format(@"<tr style='height: 18px;'>
					<td style='width: 16.66; height: 18px;'>{0}</td>
					<td style='width: 16.66; height: 18px;'>{1}</td>
					<td style='width: 16.66; height: 18px;'>{2}</td>
					<td style='width: 16.66; height: 18px;'>{3}</td>
					<td style='width: 16.66; height: 18px;'>{4}</td>
					<td style='width: 16.66; height: 18px;'>{5}</td>
				</tr>", calibracao.Certificado, calibracao.DataCalibracao, calibracao.DataProximaCalibracao, calibracao.OrgaoCalibrador, calibracao.Aprovador, calibracao.Aprovado);
                }

                modelDoc.Informacoes.Add(new InformacaoBackupModel
                {
                    Tag = "##TBLCALIBRACOES##",
                    Tipo = TipoInformacaoBackup.Texto,
                    Valor = instheadertbl + instbodytbl + instFooter
                });

                string nmArquivo = instrumento.Equipamento;

                _backupFactory.GerarBackupArquivo(modelDoc , nmArquivo);
            }
        }
        public ActionResult Download(string f)
        {
            var diretorioBackups = ConfigurationManager.AppSettings["DiretorioBackups"];
            if (!diretorioBackups.EndsWith(@"\"))
                diretorioBackups += @"\";


            var caminhoArquivo = diretorioBackups + f + ".zip";

            if (!System.IO.File.Exists(caminhoArquivo))
                return new HttpStatusCodeResult(404, "Arquivo não encontrado");

            return File(System.IO.File.ReadAllBytes(caminhoArquivo), "application/octet-stream", $"Backup_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip");
        }

        private string GerarUrlDownload(string nomeArquivo)
        {
            var dominio = ConfigurationManager.AppSettings["Dominio"];

            if (!dominio.EndsWith(@"/"))
                dominio += @"/";

            var url = $"http://{dominio}Backup/Download?f={nomeArquivo}";

            return url;
        }
        private string CompactarDiretorio(string diretorioOrigem, string diretorioDestino)
        {
            var nomeZip = string.Empty;

            if (!diretorioDestino.EndsWith(@"\"))
                diretorioDestino += @"\";

            if (!Directory.Exists(diretorioDestino))
                Directory.CreateDirectory(diretorioDestino);

            if (Directory.Exists(diretorioOrigem) && Directory.GetFiles(diretorioOrigem).Length > 0)
            {

                nomeZip = Guid.NewGuid().ToString();
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(diretorioOrigem, string.Empty);

                    zip.Save(diretorioDestino + nomeZip + ".zip");
                }
            }

            return nomeZip;
        }
    }
}