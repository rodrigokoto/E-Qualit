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

namespace Web.UI.Controllers
{
    public class BackupController : BaseController
    {

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
        private readonly IBackupFactory _backupFactory;
        private readonly IClienteAppServico _clienteAppServico;

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
                                    IClienteAppServico clienteAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
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
        }


        // GET: Backup
        public ActionResult Index(int idCliente)
        {

            var pathBackup = CriarEstruturaBackup(idCliente);
            var docTemplate = Server.MapPath("~/Templates/Backup/DocTemplate.html");
            var RegTemplate = Server.MapPath("~/Templates/Backup/IdentificacaoNaoConformidade.html");
            var AnaliseTemplate = Server.MapPath("~/Templates/Backup/AnaliseCritica.html");
            var LicencaTemplate = Server.MapPath("~/Templates/Backup/CadastroLicencas.html");

            //@"C:\projetos\Web.UI\Templates\Backup\DocTemplate.html";
            /*Cria Backup dos registros*/
            //GerarControlDoc(pathBackup, docTemplate, idCliente);
            GerarRegistros(pathBackup, docTemplate, idCliente);
            //_AnexoAppServico.BackupAnexo(9, pathBackup);

            return View();
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

                if (documento.TextoDoc != null)
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

                _backupFactory.GerarBackupArquivo(modelDoc);
            }
        }

        public void GerarRegistros(string path, string pathTemplate, int idCliente) {
            
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
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##TIPONAOCONFORMIDADE##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.TipoNaoConformidade.Descricao
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##RESPONSAVELANALISEDEFINICAOACAOIMEDIATA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.ResponsavelAnalisar.NmCompleto
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
                acaoImediataTbl = acaoImediataTbl + string.Format(@"<tr>
					          <td style='width: 20 %; '>{0}</td>
                              <td style ='width: 20%;'>{1}</td>
                              <td style ='width: 20%;'>{2}</td>
                              <td style ='width: 20%;'>{3}</td>
                              <td style ='width: 20%;'>{4}</td>
                            </tr>", acoes.Descricao, acoes.DtPrazoImplementacao, acoes.ResponsavelImplementar.NmCompleto, acoes.DtEfetivaImplementacao, acoes.Observacao);
            }

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##REGISTROTBL##",
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
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##RESPONSAVELREVERIFICACAO##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.ResponsavelReverificador.NmCompleto
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##FOIEFICAZ##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.FlEficaz == true ? "Sim" : "Não"
            });
            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##RESPONSAVELINICIARTRATATIVAACAOCORRETIVA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = registro.ResponsavelInicarAcaoImediata.NmCompleto
            });


            var nuAcaoCorretiva = _registroConformidadeAppServico.GetAll().FirstOrDefault(x => x.IdSite == siteSelecionado.IdSite && x.TipoRegistro == "ac" && x.NuRegistro == registro.IdNuRegistroFilho);

            modelDoc.Informacoes.Add(new InformacaoBackupModel
            {
                Tag = "##NUMEROACAOCORRETIVA##",
                Tipo = TipoInformacaoBackup.Texto,
                Valor = nuAcaoCorretiva.NuRegistro.ToString()
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
        }

        public void GerarAnaliseTemplate(string path, string pathTemplate, int idCliente)
        {
        
        }
    }
}