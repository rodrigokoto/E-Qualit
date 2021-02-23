using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ApplicationService.Servico
{
    public class AnexoAppServico : BaseServico<Anexo>, IAnexoAppServico
    {
        private readonly IAnexoRepositorio _anexoRepositorio;
        private readonly IRegistroConformidadesRepositorio _registroConformidadesRepositorio;
        private readonly IRegistroAcaoImediataRepositorio _registroRegistroAcaoImediataRepositorio;
        private readonly IPaiAppServico _paiAppServico;
        private readonly IPlaiAppServico _plaiAppServico;
        private readonly IPlaiProcessoNormaAppServico _plaiProcessoNormaAppServico;
        private readonly INormaAppServico _NormaAppServico;
        private readonly IDocDocumentoRepositorio _docDocumentoRepositorio;
        private readonly ILicencaRepositorio _licencaAppRepositorio;
        private readonly IInstrumentoAppServico _instrumentoAppRepositorio;
        private readonly IInstrumentoRepositorio _instrumentoRepositorio;
        private readonly ICalibracaoRepositorio _calibracaoAppServico;
        private readonly ICriterioAceitacaoRepositorio _criterioAceitacaoAppServico;
        private readonly ISiteAppServico _siteAppServico;
        public int idSite;

        public AnexoAppServico(IAnexoRepositorio anexoRepositorio,
                               IRegistroConformidadesRepositorio registroConformidadesRepositorio,
                               IRegistroAcaoImediataRepositorio registroRegistroAcaoImediataRepositorio,
                               IPaiAppServico paiAppServico, IPlaiAppServico plaiAppServico,
                               IPlaiProcessoNormaAppServico plaiProcessoNormaAppServico,
                               INormaAppServico normaAppServico, IDocDocumentoRepositorio docDocumentoRepositorio,
                               ILicencaRepositorio licencaAppRepositorio,
                               IInstrumentoAppServico instrumentoAppRepositorio,
                               IInstrumentoRepositorio instrumentoRepositorio,
                               ICalibracaoRepositorio calibracaoAppServico,
                               ICriterioAceitacaoRepositorio criterioAceitacaoAppServico, ISiteAppServico siteAppServico) : base(anexoRepositorio)
        {
            _anexoRepositorio = anexoRepositorio;
            _registroConformidadesRepositorio = registroConformidadesRepositorio;
            _registroRegistroAcaoImediataRepositorio = registroRegistroAcaoImediataRepositorio;
            _paiAppServico = paiAppServico;
            _plaiAppServico = plaiAppServico;
            _plaiProcessoNormaAppServico = plaiProcessoNormaAppServico;
            _NormaAppServico = normaAppServico;
            _docDocumentoRepositorio = docDocumentoRepositorio;
            _licencaAppRepositorio = licencaAppRepositorio;
            _instrumentoAppRepositorio = instrumentoAppRepositorio;
            _instrumentoRepositorio = instrumentoRepositorio;
            _calibracaoAppServico = calibracaoAppServico;
            _criterioAceitacaoAppServico = criterioAceitacaoAppServico;
            _siteAppServico = siteAppServico;
        }

        public void BackupAnexo(int idCliente, string pathBackup)
        {


        }

        public void BackupRegistros(int idCliente , string pathBackup)
        {
            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();
            var Registros = _registroConformidadesRepositorio.Get(x => x.IdSite == siteSelecionado.IdSite).ToList();

            foreach (var registro in Registros)
            {
                var AcaoComAnexo = registro.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0).ToList();

                if (AcaoComAnexo.Count > 0)
                {
                    switch (registro.TipoRegistro.ToLower().ToString())
                    {
                        case "ac":
                            var relativePathac = pathBackup + "\\AcaoCorretiva";

                          
                            var AcaoImediataac = registro.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0).ToList();

                            foreach (var acao in AcaoImediataac)
                            {
                                relativePathac = relativePathac + "//" + registro.NuRegistro;

                                foreach (var anexo in acao.ArquivoEvidencia)
                                {
                                    VerificaDiretorio(relativePathac);
                                    Permissionamento(relativePathac);
                                    
                                    var archive = relativePathac + "//" + anexo.Anexo.Nome;
                                    File.WriteAllBytes(archive, anexo.Anexo.Arquivo);
                                }
                            }
                            break;
                        case "nc":
                            var relativePathnc = pathBackup + "\\NaoConformidade";

                           
                            var AcaoImediatanc = registro.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0).ToList();

                            foreach (var acao in AcaoImediatanc)
                            {
                                relativePathnc = relativePathnc + "//" + registro.NuRegistro;

                                foreach (var anexo in acao.ArquivoEvidencia)
                                {
                                    VerificaDiretorio(relativePathnc);
                                    Permissionamento(relativePathnc);
                                    
                                    var archive = relativePathnc + "//" + anexo.Anexo.Nome;
                                    File.WriteAllBytes(archive, anexo.Anexo.Arquivo);
                                }
                            }

                            break;
                        case "gr":
                            var relativePathgr = pathBackup + "\\GestaoRisco";

                            
                            var AcaoImediatagr = registro.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0).ToList();

                            foreach (var acao in AcaoImediatagr)
                            {
                                relativePathgr = relativePathgr + "//" + registro.NuRegistro;

                                foreach (var anexo in acao.ArquivoEvidencia)
                                {
                                    VerificaDiretorio(relativePathgr);
                                    Permissionamento(relativePathgr);

                                    var archive = relativePathgr + "//" + anexo.Anexo.Nome;
                                    File.WriteAllBytes(archive, anexo.Anexo.Arquivo);
                                }
                            }
                            break;
                        case "gm":
                            var relativePathgm = pathBackup + "\\GestaoMelhoria";

                            var AcaoImediatagm = registro.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0).ToList();

                            foreach (var acao in AcaoImediatagm)
                            {
                                relativePathgm = relativePathgm + "//" + registro.NuRegistro;

                                foreach (var anexo in acao.ArquivoEvidencia)
                                {
                                    

                                    VerificaDiretorio(relativePathgm);
                                    Permissionamento(relativePathgm);

                                    var archive = relativePathgm + "//" + anexo.Anexo.Nome;
                                    File.WriteAllBytes(archive, anexo.Anexo.Arquivo);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public void BackupDocdocumento(int idCliente, string pathBackup)
        {
            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();
            var Documentos = _docDocumentoRepositorio.Get(x => x.IdSite == siteSelecionado.IdSite).ToList() ;

            var AnexoDoc = Documentos.Where(x => x.ArquivoDocDocumentoAnexo.Count() > 0).ToList();
            
            foreach (var doc in AnexoDoc)
            {
                var relativePath = pathBackup + "\\ControlDoc";
                relativePath = relativePath + "//" + doc.NumeroDocumento;

                foreach (var anexo in doc.ArquivoDocDocumentoAnexo)
                {
                    VerificaDiretorio(relativePath);
                    Permissionamento(relativePath);

                    var archive = relativePath + "//" + anexo.Anexo.Nome;
                    File.WriteAllBytes(archive, anexo.Anexo.Arquivo);
                }
            }
        }
        public void BackuAuditoria(int idCliente, string pathBackup)
        {
            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();

            List<Pai> pais = _paiAppServico.Get(x => x.IdSite == siteSelecionado.IdSite).ToList();

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

                    var relativePath = pathBackup + "\\Auditoria";
                    relativePath = relativePath + "//" + pai.Ano + "//" + plai.Mes;

                    foreach (var anexo in plai.ArquivoPlai)
                    {
                        VerificaDiretorio(relativePath);
                        Permissionamento(relativePath);

                        var archive = relativePath + "//" + anexo.Anexo.Nome;
                        File.WriteAllBytes(archive, anexo.Anexo.Arquivo);
                    }
                });
            });
        }
        public void BackupLicencas(int idCliente, string pathBackup)
        {
            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();

            var licencas = _licencaAppRepositorio.GetAll().Where(x => x.Idcliente == idCliente).ToList();


            foreach (var licenca in licencas)
            {
                var relativePath = pathBackup + "\\Licenca";
                relativePath = relativePath + "//" + licenca.Titulo;

                foreach (var anexo in licenca.ArquivoLicenca)
                {
                    VerificaDiretorio(relativePath);
                    Permissionamento(relativePath);

                    var archive = relativePath + "//" + anexo.Anexo.Nome;
                    File.WriteAllBytes(archive, anexo.Anexo.Arquivo);
                }
            }
        }
        public void BackupInstrumento(int idCliente, string pathBackup)
        {
            var siteSelecionado = _siteAppServico.Get(x => x.IdCliente == idCliente).FirstOrDefault();

            var instrumentos = _instrumentoAppRepositorio.Get(x => x.IdSite == siteSelecionado.IdSite).OrderByDescending(x => x.IdInstrumento).ToList();

            foreach (var instrumento in instrumentos)
            {
                var relativePath = pathBackup + "\\Instrumento";
                relativePath = relativePath + "//" + instrumento.Modelo;

                foreach (var anexo in instrumento.ArquivoInstrumento)
                {
                    VerificaDiretorio(relativePath);
                    Permissionamento(relativePath);

                    var archive = relativePath + "//" + anexo.Anexo.Nome;
                    File.WriteAllBytes(archive, anexo.Anexo.Arquivo);
                }
            }
        }

        private static void VerificaDiretorio(string pDiretorio)
        {
            try
            {
                string[] estrutura = pDiretorio.Split('\\');

                string dir = string.Empty;
                for (int i = 0; i < estrutura.Length; i++)
                {
                    if (i == 0)
                    {
                        dir += estrutura[0];
                        continue;
                    }
                    else
                        dir += "\\" + estrutura[i];

                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Permissionamento(string file)
        {
            bool exists = System.IO.Directory.Exists(file);
            if (!exists)
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(file);
                Console.WriteLine("The Folder is created Sucessfully");
            }
            else
            {
                Console.WriteLine("The Folder already exists");
            }
            DirectoryInfo dInfo = new DirectoryInfo(file);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

        }
    }
}
