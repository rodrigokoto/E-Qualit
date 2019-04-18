using System;
using System.Linq;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using Servico.Validacao;
using Dominio.Interface.Repositorio;
using System.IO;

namespace Servico
{
    public partial class Service
    {
        public ClienteValidacao _validaCliente = new ClienteValidacao();
        private readonly ILogRepositorio _logRepositorio;

        public Service(ILogRepositorio logRepositorio)
        {
            _logRepositorio = logRepositorio;
        }

        public Entidade.Cliente ClienteNew()
        {
            Entidade.Cliente _entidade = new Entidade.Cliente();

            // Armazenar senha
            _entidade.LvArmazenarSenhas = GetLista("PWD_STORE");
            _entidade.LvBloquearAcesso = GetLista("BLOCK_ACES");
            _entidade.LvTrocarSenhas = GetLista("PWD_STORE");
            _entidade.DtValidadeContrato = string.Format("{0:" + Traducao.Resource.formato_data + "}", DateTime.Today);
            _entidade.Usuario = new Entidade.CtrlUsuario()
            {
                FlAtivo = true,
                FlBloqueado = false,
                FlCompartilhado = false,
                FlRecebeEmail = true
            };


            return _entidade;
        }

        public Entidade.Cliente ClienteGetByID(int id, int idUsuario = 0)
        {
            Entidade.Cliente _entidade = new Entidade.Cliente();

            try
            {
                Sucesso = true;
                _entidade = dal.ClienteGetByID(id);

                List<Entidade.ListaValor> _LvArmazenarSenhas = GetLista("PWD_STORE");
                List<Entidade.ListaValor> _LvBloquearAcesso = GetLista("BLOCK_ACES");
                List<Entidade.ListaValor> _LvTrocarSenhas = GetLista("PWD_STORE");

                _entidade.LvSite = SiteGetByPerfilUsuario(idUsuario, _entidade.IdCliente).ToList();
                _entidade.LvArmazenarSenhas = _LvArmazenarSenhas;
                _entidade.LvBloquearAcesso = _LvBloquearAcesso;
                _entidade.LvTrocarSenhas = _LvTrocarSenhas;
            }
            catch (Exception ex)
            {
                Erros("ClienteServico.ClienteGetByID", ex);
            }

            return _entidade;
        }

        public List<Entidade.Cliente> ClienteGetAll()
        {
            List<Entidade.Cliente> _entidade = new List<Entidade.Cliente>();

            try
            {
                Sucesso = true;

                List<Entidade.ListaValor> _LvArmazenarSenhas = GetLista("PWD_STORE");
                List<Entidade.ListaValor> _LvBloquearAcesso = GetLista("BLOCK_ACES");
                List<Entidade.ListaValor> _LvTrocarSenhas = GetLista("PWD_STORE");

                foreach (DAL.Entity.Cliente lista in dal.ClienteGetAll())
                {
                    _entidade.Add(new Entidade.Cliente()
                    {
                        IdCliente = lista.IdCliente,
                        DtValidadeContrato = string.Format("{0:dd-MM-yyyy}", lista.DtValidadeContrato),
                        FlAtivo = lista.FlAtivo,
                        FlEstruturaAprovador = lista.FlEstruturaAprovador,
                        FlExigeSenhaForte = lista.FlExigeSenhaForte.Value,
                        FlTemCaptcha = lista.FlTemCaptcha,
                        NmAquivoContrato = lista.NmAquivoContrato,
                        NmFantasia = lista.NmFantasia,
                        NmLogo = lista.NmLogo,
                        NmUrlAcesso = lista.NmUrlAcesso,
                        NuArmazenaSenha = lista.NuArmazenaSenha,
                        NuDiasTrocaSenha = lista.NuDiasTrocaSenha,
                        NuTentativaBloqueioLogin = lista.NuTentativaBloqueioLogin,
                        LvArmazenarSenhas = _LvArmazenarSenhas,
                        LvBloquearAcesso = _LvBloquearAcesso,
                        LvTrocarSenhas = _LvTrocarSenhas

                    });
                }
            }
            catch (DbEntityValidationException e)
            {
                DBErros("ClienteServico.ClienteGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("ClienteServico.ClienteGetAll", ex);
            }

            return _entidade;
        }

        public IQueryable<Entidade.Cliente> ClienteActive()
        {
            IQueryable<Entidade.Cliente> _entidade = null;

            try
            {
                Sucesso = true;

                List<Entidade.ListaValor> _LvArmazenarSenhas = GetLista("PWD_STORE");
                List<Entidade.ListaValor> _LvBloquearAcesso = GetLista("BLOCK_ACES");
                List<Entidade.ListaValor> _LvTrocarSenhas = GetLista("PWD_STORE");


                _entidade = from lista in dal.ClienteGetAll()
                            where lista.FlAtivo == true
                            select new Entidade.Cliente()
                            {
                                IdCliente = lista.IdCliente,
                                DtValidadeContrato = string.Format("{0:dd-MM-yyyy}", lista.DtValidadeContrato),
                                FlAtivo = lista.FlAtivo,
                                FlEstruturaAprovador = lista.FlEstruturaAprovador,
                                FlExigeSenhaForte = lista.FlExigeSenhaForte.Value,
                                FlTemCaptcha = lista.FlTemCaptcha,
                                NmAquivoContrato = lista.NmAquivoContrato,
                                NmFantasia = lista.NmFantasia,
                                NmLogo = lista.NmLogo,
                                NmUrlAcesso = lista.NmUrlAcesso,
                                NuArmazenaSenha = lista.NuArmazenaSenha,
                                NuDiasTrocaSenha = lista.NuDiasTrocaSenha,
                                NuTentativaBloqueioLogin = lista.NuTentativaBloqueioLogin,
                                LvArmazenarSenhas = _LvArmazenarSenhas,
                                LvBloquearAcesso = _LvBloquearAcesso,
                                LvTrocarSenhas = _LvTrocarSenhas

                            };
            }
            catch (DbEntityValidationException e)
            {
                DBErros("ClienteServico.ClienteGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("ClienteServico.ClienteGetAll", ex);
            }

            return _entidade;

        }

        public void VerificaDiretorio(string pDiretorio)
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


        public string TiraNomeArquivoDuplicado(string nomeArquivo)
        {
            if (string.IsNullOrEmpty(nomeArquivo)) return string.Empty;

            if (nomeArquivo.IndexOf("|") == -1)
            {
                return nomeArquivo;
            }

            string nmArquivo = nomeArquivo;

            string[] list1 = nmArquivo.Split('|');

            List<string> sList = new List<string>();

            for (int i = 0; i < list1.Length; i++)
            {
                if (!string.IsNullOrEmpty(list1[i]))
                {
                    string _bExiste = sList.Where(w => w.ToLower() == list1[i].ToLower()).FirstOrDefault();
                    if (string.IsNullOrEmpty(_bExiste))
                    {
                        sList.Add(list1[i]);
                    }
                }
            }

            string separador = "|";
            string linhaNova = String.Join(separador, sList);

            return linhaNova;
        }

        public Entidade.Cliente ClienteSave(Entidade.Cliente item)
        {
            Entidade.Cliente entidade = new Entidade.Cliente();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                //try
                //{
                    Sucesso = true;

                    if (item.IdCliente == 0)
                    {
                        item.IsInsert = true;
                    }
                    else
                    {
                        item.IsUpdate = true;
                    }

                    item.NmAquivoContrato = TiraNomeArquivoDuplicado(item.NmAquivoContrato);

                    //Salva os dados do cliente
                    entidade = dal.ClienteSave(item);

                    if (item.IsInsert)
                    {
                        //Salva Site
                        item.IdCliente = entidade.IdCliente;
                        item.Site.IdCliente = item.IdCliente;

                        var sites = _SiteSave(item.Site, item.IdUsuarioLogado);

                        //Cria usuário coordenador
                        item.Usuario.IdPerfil = 3; //Perfil Coordenador
                        item.Usuario.IdUsuarioLogado = item.IdUsuarioLogado;
                        item.Usuario.LvSite.Add(new Entidade.UsuarioSite() { IdCliente = item.Site.IdCliente, FlSelecionado = true, IdSite = sites.IdSite });

                        var usuarios = _UsuarioSave(item.Usuario);
                    }

                    if (item.IsInsert)
                        Mensagem = Traducao.Resource.mensagem_registro_incluido;
                    else if (item.IsUpdate)
                        Mensagem = Traducao.Resource.mensagem_registro_alterado;

                    string basedir = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                    if (item.IsInsert)
                    {
                        foreach (string arquivo in item.NmAquivoContrato.Split('|'))
                        {
                            if (!String.IsNullOrWhiteSpace(arquivo))
                            {
                                string fullPath = basedir + string.Format("content\\cliente\\{0}\\contratos\\{1}", 0, arquivo);
                                string fullPathDestino = basedir + string.Format("content\\cliente\\{0}\\contratos\\{1}", entidade.IdCliente, arquivo);

                                VerificaDiretorio(basedir + string.Format("content\\cliente\\{0}\\contratos", entidade.IdCliente));

                                if (System.IO.File.Exists(fullPath))
                                    System.IO.File.Move(fullPath, fullPathDestino);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(item.NmLogo) && item.NmLogo.StartsWith("tmp_"))
                    {
                        string fullPath = basedir + string.Format("content\\cliente\\{0}",item.NmLogo);
                        string extensao = Path.GetExtension(item.NmLogo);

                        string fullPathDestino = basedir + string.Format("content\\cliente\\{0}\\Logo_{0}{1}", entidade.IdCliente, extensao);

                        if (System.IO.File.Exists(fullPath))
                            System.IO.File.Move(fullPath, fullPathDestino);

                        item.NmLogo = Path.GetFileName(fullPathDestino);
                        entidade = dal.ClienteSave(item);
                    }
                    _transaction.Complete();
                //}
                //catch (Exception ex)
                //{
                //    var log = new Log(Convert.ToInt32(Acao.AtualizarEmailEnviado), ex);
                //    _logRepositorio.Add(log);
                //}
            }
            return item;
        }

        public void ClienteRemove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    dal.ClienteRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("AK_Cliente") > 0)
                    {
                        Erros("ClienteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cliente_AK_Cliente));
                    }
                    else if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("AK_Cliente_Fantasia") > 0)
                    {
                        Erros("ClienteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cliente_AK_Cliente_Fantasia));
                    }
                    else if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Cliente") > 0)
                    {
                        Erros("ClienteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cliente_PK_Cliente));
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioClienteSite_Cliente") > 0)
                    {
                        Erros("ClienteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cliente_FK_UsuarioClienteSite_Cliente));
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_Site_Cliente") > 0)
                    {
                        Erros("ClienteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cliente_FK_Site_Cliente));
                    }
                    else
                    {
                        Erros("ClienteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                    }
                }
                catch (DbEntityValidationException e)
                {
                    DBErros("ClienteServico.ClienteRemove", e);
                }
                catch (Exception ex)
                {
                    Erros("ClienteServico.ClienteRemove", ex);
                }
            }
        }

        /// <summary>
        /// Verifica se existe algum cliente ativo para o usuário informado
        /// </summary>
        /// <param name="usuario">Usuário</param>
        /// <returns>bool</returns>
        public bool ClienteIsAtivo(Entidade.CtrlUsuario usuario)
        {
            if (usuario.IdPerfil == 1 || usuario.IdPerfil == 2) // Administrador / Suporte
            {
                return true;
            }
            else
            {
                List<DAL.Entity.UsuarioClienteSite> clienteSiteList = dal.UsuarioClienteSiteGetAll()
                                                                         .Where(x => x.IdUsuario == usuario.IdUsuario)
                                                                         .ToList();

                foreach (DAL.Entity.UsuarioClienteSite item in clienteSiteList)
                {
                    if (dal.ClienteGetByID(item.IdCliente).FlAtivo)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Verifica se existem clientes associados ao usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool PossuiCliente(Entidade.CtrlUsuario usuario)
        {
            if (usuario.IdPerfil == 1) // Administrador
            {
                return true;
            }
            else
            {
                if (dal.UsuarioClienteSiteGetAll().Where(x => x.IdUsuario == usuario.IdUsuario).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<Entidade.Cliente> ClienteObterTodosAtivos()
        {
            List<Entidade.Cliente> _entidade = new List<Entidade.Cliente>();

            try
            {
                Sucesso = true;

                List<Entidade.ListaValor> _LvArmazenarSenhas = GetLista("PWD_STORE");
                List<Entidade.ListaValor> _LvBloquearAcesso = GetLista("BLOCK_ACES");
                List<Entidade.ListaValor> _LvTrocarSenhas = GetLista("PWD_STORE");

                foreach (DAL.Entity.Cliente lista in dal.ClienteGetAll().Where(x => x.FlAtivo == true))
                {
                    _entidade.Add(new Entidade.Cliente()
                    {
                        IdCliente = lista.IdCliente,
                        DtValidadeContrato = string.Format("{0:dd-MM-yyyy}", lista.DtValidadeContrato),
                        FlAtivo = lista.FlAtivo,
                        FlEstruturaAprovador = lista.FlEstruturaAprovador,
                        FlExigeSenhaForte = lista.FlExigeSenhaForte.Value,
                        FlTemCaptcha = lista.FlTemCaptcha,
                        NmAquivoContrato = lista.NmAquivoContrato,
                        NmFantasia = lista.NmFantasia,
                        NmLogo = lista.NmLogo,
                        NmUrlAcesso = lista.NmUrlAcesso,
                        NuArmazenaSenha = lista.NuArmazenaSenha,
                        NuDiasTrocaSenha = lista.NuDiasTrocaSenha,
                        NuTentativaBloqueioLogin = lista.NuTentativaBloqueioLogin,
                        LvArmazenarSenhas = _LvArmazenarSenhas,
                        LvBloquearAcesso = _LvBloquearAcesso,
                        LvTrocarSenhas = _LvTrocarSenhas

                    });
                }
            }
            catch (DbEntityValidationException e)
            {
                DBErros("ClienteServico.ClienteGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("ClienteServico.ClienteGetAll", ex);
            }

            return _entidade;
        }
    }
}
