using System;
using System.Linq;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Configuration;
using Entidade;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;

namespace Servico
{
    public partial class Service
    {
        public CtrlUsuario CtrlUsuarioNew(int idCliente = 0)
        {
            CtrlUsuario _entidade = new CtrlUsuario();
            try
            {
                Sucesso = true;

                _entidade = dal.CtrlUsuarioNew(idCliente);
                ObterListaSiteCargoCliente(ref _entidade);
            }
            catch (Exception ex)
            {
                Erros("CtrlUsuarioServico.CtrlUsuarioNew", ex);
            }

            return _entidade;
        }

        public CtrlUsuario CtrlUsuarioGetByID(int id, int idPerfil = 0, int idCliente = 0)
        {
            CtrlUsuario _usuario = new CtrlUsuario();

            try
            {
                Sucesso = true;

                _usuario = dal.CtrlUsuarioGetByID(id);

                if (idPerfil > 0)
                {
                    _usuario.IdPerfil = idPerfil;
                }


                if (_usuario.DtExpiracao != DateTime.MaxValue.ToString())
                {
                    _usuario.DtExpiracao = string.Format("{0:" + Traducao.Resource.formato_data + "}", _usuario.DtExpiracao);
                }
                else
                {
                    _usuario.DtExpiracao = string.Empty;
                }


                ObterListaSiteCargoCliente(ref _usuario, idCliente);


                var _usuarioClientesSites = unitOfWork.UsuarioClienteSiteRepository.Get(x => x.IdUsuario == id);

                var usuarioCargos = unitOfWork.UsuarioCargoRepository.Get(x => x.IdUsuario == id);

                foreach (var site in _usuario.LvSite.Where(x => x.Cargos.Count() > 0))
                {
                    InformarCargosQuePossui(usuarioCargos, site.Cargos);
                }

                InformarSitesQuePossui(_usuarioClientesSites, _usuario);

                InformarClientesQuePossui(_usuarioClientesSites, _usuario);

                InformarProcessosQuePossui(usuarioCargos, _usuario);

                if (_usuario.IdUsuario > 0 && _usuario.IdPerfil <= 2)
                {
                    _usuario.LvPerfil = _usuario.LvPerfil.Where(w => w.IdPerfil <= 2).ToList();
                }
                if (_usuario.IdUsuario > 0 && _usuario.IdPerfil > 2)
                {
                    _usuario.LvPerfil = _usuario.LvPerfil.Where(w => w.IdPerfil > 2).ToList();
                }
            }

            catch (Exception ex)
            {
                Erros("CtrlUsuarioServico.CtrlUsuarioGetByID", ex);
            }

            return _usuario;
        }

        private void ObterListaSiteCargoCliente(ref CtrlUsuario usuario, int idCliente = 0)
        {
            try
            {
                var _cargosSite = new List<UsuarioCargos>();

                var _sites = SiteObterTodosAtivos();
                var clientesAtivos = ClienteObterTodosAtivos();


                if (idCliente > 0)
                {
                    _sites = _sites.Where(w => w.IdCliente == idCliente).ToList();
                    clientesAtivos = clientesAtivos.Where(w => w.IdCliente == idCliente).ToList();

                    usuario.LvCliente.Clear();
                    usuario.LvCliente.AddRange(clientesAtivos);
                }
                else
                {
                    int[] _idsCliente = new int[] { };
                    if (usuario.IdPerfil == 3 || usuario.IdPerfil == 4)
                    {
                        _idsCliente = dal.UsuarioClienteSiteGetByIdUsuario(usuario.IdUsuario).Select(s => s.IdCliente).Distinct().ToArray();
                        clientesAtivos = clientesAtivos.Where(w => _idsCliente.Contains(w.IdCliente)).ToList();
                    }
                    _sites = _sites.Where(w => _idsCliente.Contains(w.IdCliente)).ToList();

                    usuario.LvCliente.Clear();
                    usuario.LvCliente.AddRange(clientesAtivos);
                }


                foreach (var site in _sites)
                {
                    _cargosSite = new List<UsuarioCargos>();

                    var cargosCTX = unitOfWork.CargoRepository.Get(x => x.IdSite == site.IdSite).ToList();

                    foreach (var cargo in cargosCTX)
                    {
                        _cargosSite.Add(new UsuarioCargos
                        {
                            IdCargo = cargo.IdCargo,
                            IdSite = cargo.IdSite,
                            Nome = cargo.NmNome,
                        });
                    }

                    //if (_cargosSite.Count > 0)
                    //{
                    usuario.LvSite.Add(new UsuarioSite
                    {
                        IdSite = site.IdSite,
                        IdCliente = site.IdCliente,
                        Nome = site.NmFantasia,
                        Cargos = _cargosSite
                    });
                    // }
                }
            }
            catch (Exception ex)
            {
                Erros("CtrlUsuarioServico.ObterListaSiteCargoCliente", ex);
            }

        }

        private void InformarCargosQuePossui(IEnumerable<DAL.Entity.UsuarioCargo> usuarioCargos, List<UsuarioCargos> cargosSite)
        {
            if (usuarioCargos.Count() > 0)
            {
                foreach (var usuarioCargo in usuarioCargos)
                {
                    foreach (var cargoSite in cargosSite)
                    {
                        if (cargoSite.IdCargo == usuarioCargo.IdCargo)
                        {
                            cargoSite.FlSelecionado = true;
                        }
                    }
                }
            }
        }

        private void InformarSitesQuePossui(IEnumerable<DAL.Entity.UsuarioClienteSite> usuarioClientesSites, CtrlUsuario usuario)
        {
            var IdsUsuarioClientesSites = usuarioClientesSites.Select(x => x.IdSite.Value).ToList();
            var IdsSiteUsuarioSite = usuario.LvSite.Select(c => c.IdSite).Intersect(IdsUsuarioClientesSites).ToList();


            if (usuario.IdUsuario > 0)
            {
                foreach (var _site in usuario.LvSite.Where(site => IdsSiteUsuarioSite.Contains(site.IdSite)))
                {
                    _site.FlSelecionado = true;
                }
            }
        }

        private void InformarClientesQuePossui(IEnumerable<DAL.Entity.UsuarioClienteSite> usuarioClientesSites, CtrlUsuario usuario)
        {
            foreach (var usuarioClienteSite in usuarioClientesSites)
            {
                usuario.LvCliente.Where(x => x.IdCliente == usuarioClienteSite.IdCliente).FirstOrDefault().FlSelecionado = true;
            }
        }

        private void InformarProcessosQuePossui(IEnumerable<DAL.Entity.UsuarioCargo> usuarioCargos, CtrlUsuario usuario)
        {
            if (usuarioCargos.Count() > 0)
            {
                foreach (var cargo in usuarioCargos)
                {
                    var cargosProcessos = unitOfWork.CargoProcessoRepository.Get(x => x.IdCargo == cargo.IdCargo);

                    foreach (DAL.Entity.CargoProcesso _cargoProcesso in cargosProcessos)
                    {
                        var _processo = unitOfWork.ProcessoRepository.Get(x => x.IdProcesso == _cargoProcesso.IdProcesso).FirstOrDefault();

                        usuario.LvProcesso.Add(new Processo
                        {
                            IdProcesso = _processo.IdProcesso,
                            NmProcesso = _processo.NmProcesso
                        });
                    }
                }
            }
        }

        private CtrlUsuario _UsuarioSave(CtrlUsuario usuario)
        {
            string cdSenha = string.Empty;
            string Hash = string.Empty;
            bool bDisparaEmail = false;

            if (usuario.IdUsuario == 0)
            {
                cdSenha = GeraNovaSenha();
                Hash = Sha1Hash(cdSenha);

                usuario.CdSenha = Hash;
                bDisparaEmail = true;
            }

            #region Grava Usuário

            CtrlUsuario _usuarioSolicitante = new CtrlUsuario
            {
                IdPerfil = unitOfWork.CtrlUsuarioRepository.GetByID(usuario.IdUsuarioLogado).IdPerfil
            };

            ValidarUsuario(usuario);

            if (usuario.ModelErros.Count > 0)
            {
                usuario.LvPerfil = CtrlPerfilGetAll().ToList();
                Sucesso = false;
                return usuario;
            }

            RemoveClienteSiteCargoPorUsuario(usuario.IdUsuario);

            if (_validaUsuario.EColaborador(usuario.IdPerfil))
            {
                CadastraSalva(usuario);
                foreach (var site in usuario.LvSite)
                {
                    if (site.FlSelecionado)
                    {
                        usuario.UsuarioClienteSite.Add(new UsuarioClienteSite
                        {
                            IdUsuario = usuario.IdUsuario,
                            IdSite = site.IdSite,
                            IdCliente = site.IdCliente
                        });

                        CadastrarCargosPorUsuario(site.Cargos, usuario.IdUsuario);
                    }
                }
                UsuarioClienteSiteSave(usuario.UsuarioClienteSite);
            }
            else
            {
                CadastraSalva(usuario);

                if (_validaUsuario.ESuporte(usuario.IdPerfil))
                {
                    foreach (var cliente in usuario.LvCliente)
                    {
                        if (cliente.FlSelecionado)
                        {
                            usuario.UsuarioClienteSite.Add(new UsuarioClienteSite
                            {
                                IdUsuario = usuario.IdUsuario,
                                IdCliente = cliente.IdCliente
                            });
                        }

                    }
                }
                else if (_validaUsuario.ECoordenador(usuario.IdPerfil))
                {
                    foreach (var site in usuario.LvSite)
                    {
                        if (site.FlSelecionado)
                        {
                            var _site = unitOfWork.SiteRepository.GetByID(site.IdSite);
                            usuario.UsuarioClienteSite.Add(new UsuarioClienteSite
                            {
                                IdUsuario = usuario.IdUsuario,
                                IdCliente = _site.IdCliente,
                                IdSite = site.IdSite
                            });
                        }

                    }
                }

                UsuarioClienteSiteSave(usuario.UsuarioClienteSite);
            }
            #endregion

            if (bDisparaEmail && usuario.ModelErros.Count == 0)
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Templates\NovoUsuario-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
                string template = System.IO.File.ReadAllText(path);
                string conteudo = template;

                conteudo = conteudo.Replace("#NOME#", usuario.NmCompleto);
                conteudo = conteudo.Replace("#SENHA#", cdSenha);
                conteudo = conteudo.Replace("#DOMINIO#", ConfigurationManager.AppSettings["Dominio"]);
                conteudo = conteudo.Replace("#NOMESISTEMA#", "e-Qualit");

                Email _email = new Email();

                _email.Assunto = Traducao.Resource.ControleAcesso_assunto_email_novosusuario;
                _email.De = ConfigurationManager.AppSettings["EmailDE"];
                _email.Para = usuario.NmCompleto + "<" + usuario.CdIdentificacao + ">";
                _email.Conteudo = conteudo;

                _email.Enviar();
            }

            return usuario;
        }

        /// <summary>
        /// Retorna o usuário através do seu endereço de e-mail
        /// </summary>
        /// <param name="identificacao">E-mail de identificação</param>
        /// <returns>Entidade.Usuario</returns>
        public CtrlUsuario CtrlUsuarioGetByIdentificacao(string identificacao)
        {
            CtrlUsuario _entidade = new CtrlUsuario();

            try
            {
                Sucesso = true;
                _entidade = dal.CtrlUsuarioGetByIdentificacao(identificacao);
            }
            catch (Exception ex)
            {
                Erros("CtrlUsuarioServico.CtrlUsuarioGetByID", ex);
            }

            return _entidade;
        }

        public IQueryable<CtrlUsuario> CtrlUsuarioGetAll()
        {
            IQueryable<CtrlUsuario> _entidade = null;

            try
            {
                Sucesso = true;

                _entidade = from lista in dal.CtrlUsuarioGetAll()
                            select new CtrlUsuario()
                            {
                                IdUsuario = lista.IdUsuario,
                                CdIdentificacao = lista.CdIdentificacao,
                                CdSenha = lista.CdSenha,
                                DtAlteracaoSenha = lista.DtAlteracaoSenha,
                                DtExpiracao = lista.DtExpiracao.ToString(),
                                DtUltimoAcesso = lista.DtUltimoAcesso,
                                FlAtivo = lista.FlAtivo,
                                FlBloqueado = lista.FlBloqueado,
                                FlCompartilhado = lista.FlCompartilhado,
                                FlRecebeEmail = lista.FlRecebeEmail,
                                FlSexo = lista.FlSexo,
                                IdPerfil = lista.IdPerfil,
                                //NmArquivoFoto = lista.NmArquivoFoto,
                                NmCompleto = lista.NmCompleto,
                                NmFuncao = lista.NmFuncao,
                                NuCPF = lista.NuCPF,
                                NuFalhaLNoLogin = lista.NuFalhaLNoLogin,
                                DtInclusao = lista.DtInclusao
                            };
            }
            catch (DbEntityValidationException e)
            {
                DBErros("CtrlUsuarioServico.CtrlUsuarioGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("CtrlUsuarioServico.CtrlUsuarioGetAll", ex);
            }

            return _entidade;
        }

        public void CtrlUsuarioRemove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    dal.CtrlUsuarioRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Usuario") > 0)
                    {
                        Error = new Exception(Traducao.Resource.CtrlUsuario_PK_Usuario);
                    }
                    if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioClienteSite_CtrlUsuario") > 0)
                    {
                        Error = new Exception(Traducao.Resource.CtrlUsuario_FK_UsuarioClienteSite_CtrlUsuario);
                    }
                    if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioProcessoAcao_CtrlUsuario") > 0)
                    {
                        Error = new Exception(Traducao.Resource.CtrlUsuario_FK_UsuarioProcessoAcao_CtrlUsuario);
                    }

                }
                catch (DbEntityValidationException e)
                {
                    DBErros("CtrlUsuarioServico.CtrlUsuarioRemove", e);
                }
                catch (Exception ex)
                {
                    Erros("CtrlUsuarioServico.CtrlUsuarioRemove", ex);
                }
            }
        }

        public void CtrlUsuarioTrocaSenha(string novaSenha, int idUser)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    DAL.Entity.CtrlUsuario _model = unitOfWork.CtrlUsuarioRepository.GetByID(idUser);

                    _model.CdSenha = Sha1Hash(novaSenha);
                    _model.DtAlteracaoSenha = DateTime.Now;

                    unitOfWork.CtrlUsuarioRepository.Update(_model);
                    unitOfWork.Save();

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    Error = ex;
                    Sucesso = false;
                }
            }
        }

        public string GeraNovaSenha()
        {
            Random randon = new Random();
            string senha = string.Empty;
            string[] elementos = { "0", "1", "2", "3", "4", "5", "6", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "w", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "W", "Y", "Z" };
            for (int i = 1; i <= 6; i++)
            {
                senha += elementos[randon.Next(0, elementos.Length)];
            }
            return senha;
        }

        public string Sha1Hash(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);

            var hash = string.Empty;

            foreach (var b in hashData)
                hash += b.ToString("X2");

            return hash;
        }

        public string CtrlUsuarioRedefinirSenha(string Email)
        {
            //Entidade.Usuario _entidade = new Entidade.Usuario();
            string senha = string.Empty;
            string email = string.Empty;

            try
            {
                Sucesso = true;

                DAL.Entity.CtrlUsuario _entity = unitOfWork.CtrlUsuarioRepository
                                                           .GetAll()
                                                           .Where(x => x.CdIdentificacao == Email)
                                                           .FirstOrDefault();
                if (_entity == null)
                {
                    Error = new Exception(Traducao.Resource.ControleAcesso_msg_erro_usuarionaolocalizado);
                    Sucesso = false;
                    return string.Empty;
                }

                senha = GeraNovaSenha();
                _entity.CdSenha = Sha1Hash(senha);
                _entity.DtAlteracaoSenha = DateTime.Now;
                _entity.NuFalhaLNoLogin = 0;

                unitOfWork.CtrlUsuarioRepository.Update(_entity);
                unitOfWork.Save();

                email = _entity.CdIdentificacao;

                string template = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["DiretorioTemplates"] + @"\ReenvioSenha-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html");

                string conteudo = template;
                conteudo = conteudo.Replace("#NOME#", _entity.NmCompleto);
                conteudo = conteudo.Replace("#SENHA#", senha);
                conteudo = conteudo.Replace("#DOMINIO#", ConfigurationManager.AppSettings["Dominio"]);

                Email _email = new Email();
                _email.Assunto = Traducao.Resource.ControleAcesso_assunto_email_reenviosenha;
                _email.De = ConfigurationManager.AppSettings["EmailDE"];
                _email.Para = _entity.NmCompleto + "<" + _entity.CdIdentificacao + ">";
                _email.Conteudo = conteudo;
                _email.Enviar();

                return Email;
            }
            catch (Exception ex)
            {
                Error = ex;
                Sucesso = false;
            }

            return Email;
        }

        public void CtrlUsuarioAtivaInativa(int id, bool fl)
        {

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;

                    DAL.Entity.CtrlUsuario _entity = unitOfWork.CtrlUsuarioRepository.Get(f => f.IdUsuario == id).FirstOrDefault();
                    _entity.FlAtivo = fl;

                    unitOfWork.CtrlUsuarioRepository.Update(_entity);
                    unitOfWork.Save();

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    Error = ex;
                    Sucesso = false;
                }
            }
        }

        public void CtrlUsuarioBloqueaDesbloquea(int id, bool fl)
        {

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;

                    DAL.Entity.CtrlUsuario _entity = unitOfWork.CtrlUsuarioRepository.Get(f => f.IdUsuario == id).FirstOrDefault();
                    _entity.FlBloqueado = fl;

                    unitOfWork.CtrlUsuarioRepository.Update(_entity);
                    unitOfWork.Save();

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    Error = ex;
                    Sucesso = false;
                }
            }
        }

        public void CtrlUsuarioRecebeNaoRecebeEmail(int id, bool fl)
        {

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;

                    DAL.Entity.CtrlUsuario _entity = unitOfWork.CtrlUsuarioRepository.Get(f => f.IdUsuario == id).FirstOrDefault();
                    _entity.FlRecebeEmail = fl;

                    unitOfWork.CtrlUsuarioRepository.Update(_entity);
                    unitOfWork.Save();

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    Error = ex;
                    Sucesso = false;
                }
            }
        }

        public CtrlUsuario ValidarUsuario(CtrlUsuario usuario)
        {
            try
            {
                if (!usuario.IsValid())
                {
                    Sucesso = false;
                    return usuario;
                }

                CtrlUsuario _usuarioSolicitante = new CtrlUsuario
                {
                    IdPerfil = unitOfWork.CtrlUsuarioRepository.GetByID(usuario.IdUsuarioLogado).IdPerfil
                };

                if (_validaUsuario.EColaborador(usuario.IdPerfil))
                {
                    var erros = _validaUsuario.ColaboradorAptoCadastrar(_usuarioSolicitante, usuario);
                    if (erros.Count > 0)
                    {
                        usuario.ModelErros.AddRange(erros);
                    }
                }
                else if (_validaUsuario.ESuporte(usuario.IdPerfil) || _validaUsuario.ECoordenador(usuario.IdPerfil))
                {
                    var erros = _validaUsuario.RegrasCoordenadorSuporte(_usuarioSolicitante, usuario);
                    if (erros.Count > 0)
                    {
                        usuario.ModelErros.AddRange(erros);
                    }
                }

                if (usuario.ModelErros.Count > 0)
                {
                    Sucesso = false;
                    return usuario;
                }

                Sucesso = true;
            }
            catch (Exception ex)
            {
                Erros("ClienteServico.ClienteSave", ex);
            }


            return usuario;
        }

        public CtrlUsuario UsuarioSave(CtrlUsuario usuario)
        {

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;

                    usuario = _UsuarioSave(usuario);

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Usuario") > 0)
                    {
                        Error = new Exception(Traducao.Resource.CtrlUsuario_PK_Usuario);
                    }
                    else if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("AK_CtrlUsuario") > 0)
                    {
                        Error = new Exception(Traducao.Resource.CtrlUsuario_AK_CtrlUsuario);
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioClienteSite_CtrlUsuario") > 0)
                    {
                        Error = new Exception(Traducao.Resource.CtrlUsuario_FK_UsuarioClienteSite_CtrlUsuario);
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioProcessoAcao_CtrlUsuario") > 0)
                    {
                        Error = new Exception(Traducao.Resource.CtrlUsuario_FK_UsuarioProcessoAcao_CtrlUsuario);
                    }
                    else
                    {
                        Erros("CtrlUsuarioServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                    }
                }
                catch (DbEntityValidationException e)
                {
                    DBErros("ClienteServico.ClienteSave", e);
                }
                catch (Exception ex)
                {
                    Erros("ClienteServico.ClienteSave", ex);
                }

            }

            return usuario;
        }

        private void CadastraSalva(CtrlUsuario usuario)
        {
            if (usuario.IdUsuario == 0)
            {
                usuario = dal.CtrlUsuarioAdd(usuario);
            }
            else
            {
                usuario = dal.CtrlUsuarioUpdate(usuario);
            }
        }

        public List<CtrlUsuario> ObterUsuariosPorPerfil(CtrlUsuario usuario)
        {
            var usuariosCTX = new List<CtrlUsuario>();
            var _usuariosCTX = new List<CtrlUsuario>();
            var usuariosClientesSites = unitOfWork.UsuarioClienteSiteRepository.Get(x => x.IdUsuario == usuario.IdUsuario);


            if (_validaUsuario.EColaborador(usuario.IdPerfil))
            {
                return usuariosCTX;
            }

            if (_validaUsuario.EAdministrador(usuario.IdPerfil))
            {
                usuariosCTX.AddRange(CtrlUsuarioGetAll().ToList());
            }
            else if (_validaUsuario.ESuporte(usuario.IdPerfil))
            {
                foreach (var usuarioCliente in usuariosClientesSites)
                {
                    foreach (var _itemUsu in ObterUsuariosPorClienteRelacionado(usuarioCliente.IdCliente))
                    {
                        //Para eliminar duplicidades, verifico antes se o usuário ja existe na lista
                        var _findUsu = _usuariosCTX.Where(w => w.IdUsuario == _itemUsu.IdUsuario).FirstOrDefault();
                        if (_findUsu == null)
                        {
                            _usuariosCTX.Add(_findUsu);
                        }
                    }

                    usuariosCTX.AddRange(_usuariosCTX);
                }
            }
            else if (_validaUsuario.ECoordenador(usuario.IdPerfil))
            {
                foreach (var usuarioSite in usuariosClientesSites.Where(x => x.IdSite != null))
                {
                    usuariosCTX.AddRange(ObterUsuariosPorSiteRelacionado(Convert.ToInt32(usuarioSite.IdSite)));
                }
            }

            if (_validaUsuario.EAdministrador(usuario.IdPerfil) || _validaUsuario.ESuporte(usuario.IdPerfil))
            {
                foreach (var _item in usuariosCTX)
                {
                    var _clientes = dal.UsuarioClienteSiteGetByIdUsuario(_item.IdUsuario);
                    string[] _nomesClientes = _clientes.Select(s => s.Cliente.NmFantasia).Distinct().ToArray();
                    _item.Clientes = string.Join(",", _nomesClientes);
                }
            }
            return usuariosCTX.ToList();
        }

        public List<CtrlUsuario> ObterUsuariosPorClienteRelacionado(int idCliente)
        {
            try
            {
                return dal.ObterUsuariosPorClienteRelacionado(idCliente);
            }
            catch (DbEntityValidationException e)
            {
                DBErros("CtrlUsuarioService.CtrlUsuariosObtemPorId", e);
            }
            catch (Exception ex)
            {
                Erros("CtrlUsuarioService.ClienteSave", ex);
            }

            return null;
        }

        public List<CtrlUsuario> ObterUsuariosPorSiteRelacionado(int idSite)
        {
            try
            {
                return dal.ObterUsuariosPorSiteRelacionado(idSite);
            }
            catch (DbEntityValidationException e)
            {
                DBErros("CtrlUsuarioService.CtrlUsuariosObtemPorId", e);
            }
            catch (Exception ex)
            {
                Erros("CtrlUsuarioService.ClienteSave", ex);
            }

            return null;
        }

        public void RemoveClienteSiteCargoPorUsuario(int idUsuario)
        {
            try
            {
                var _usuarioClientesSites = UsuarioClienteSiteObterPorUsuario(idUsuario);
                var _usuariosCargos = UsuarioCargoObterPorUsuario(idUsuario);

                foreach (var usuarioClienteSite in _usuarioClientesSites)
                {
                    dal.UsuarioClienteSiteRemove(usuarioClienteSite.IdUsuarioClienteSite);
                }

                UsuarioCargoRemoverPorUsuario(idUsuario);

            }
            catch (DbEntityValidationException e)
            {
                DBErros("CtrlUsuarioService.RemoverPermissoes", e);
            }
            catch (Exception ex)
            {
                Erros("CtrlUsuarioService.RemoverPermissoes", ex);
            }
        }

        public bool PossuiAcesso(int idUsuario, int idfuncionalidade, int idFuncao)
        {
            try
            {
                var usuarios = new List<CtrlUsuario>();
                var cargoProcessos = new List<DAL.Entity.CargoProcesso>();

                if (idFuncao != 0)
                {
                    cargoProcessos = unitOfWork.CargoProcessoRepository.Get(x => x.Funcao.IdFuncionalidade == idfuncionalidade &&
                                                                            x.Funcao.IdFuncao == idFuncao,
                                                                            null,
                                                                            "Funcao,Cargo").ToList();
                }
                else
                {
                    cargoProcessos = unitOfWork.CargoProcessoRepository.Get(x => x.Funcao.IdFuncionalidade == idfuncionalidade,
                                                                            null,
                                                                            "Funcao,Cargo").ToList();
                }

                foreach (var cargoProcesso in cargoProcessos)
                {
                    cargoProcesso.Cargo.UsuarioCargo = unitOfWork.UsuarioCargoRepository.Get(x => x.IdCargo == cargoProcesso.IdCargo).ToList();

                    foreach (var usuarioCargo in cargoProcesso.Cargo.UsuarioCargo.Where(x => x.IdUsuario == idUsuario))
                    {
                        var usuario = unitOfWork.CtrlUsuarioRepository.Get(x => x.IdUsuario == idUsuario).FirstOrDefault();

                        usuarios.Add(new CtrlUsuario
                        {
                            IdUsuario = usuario.IdUsuario,
                            NmCompleto = usuario.NmCompleto
                        });
                    }
                }

                if (usuarios.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (DbEntityValidationException e)
            {
                DBErros("CtrlUsuarioService.PossuiAcesso", e);
            }
            catch (Exception ex)
            {
                Erros("CtrlUsuarioService.PossuiAcesso", ex);
            }
            return false;
        }

        public class Email
        {
            public Email()
            {
                Servidor = ConfigurationManager.AppSettings["SMTPServer"];
                Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                Usuario = ConfigurationManager.AppSettings["SMTPUser"];
                Senha = ConfigurationManager.AppSettings["SMTPPassword"];
            }

            #region Propriedade Privadas

            private List<string> _anexos;

            #endregion

            #region Propriedades Publicas

            public string Servidor;
            public string De;
            public string Para;
            public string Copia;
            public string Assunto;
            public string Conteudo;
            public string Usuario;
            public string Senha;
            public int Porta;

            #endregion

            #region Metodos Publicos

            public void Enviar()
            {
                //configuracoes do email
                MailMessage mess = new MailMessage();
                mess.From = new MailAddress(De);

                if (Para != null && Para.Length > 0)
                {
                    foreach (string address in Para.Split(';'))
                    {
                        mess.To.Add(new MailAddress(address));
                    }
                }

                if (Copia != null && Copia.Length > 0)
                {
                    foreach (string address in Copia.Split(';'))
                    {
                        mess.CC.Add(new MailAddress(address));
                    }
                }

                mess.Subject = Assunto;

                //formato do email
                mess.IsBodyHtml = true;

                //corpo do email						
                mess.Body = Conteudo;

                //prioridade
                mess.Priority = MailPriority.High;

                //anexos
                if (_anexos != null)
                {
                    foreach (string path in _anexos)
                    {
                        mess.Attachments.Add(new Attachment(path));
                    }
                }

                //define servidor SMTP
                SmtpClient emailClient;
                if (Porta == 0)
                    emailClient = new SmtpClient(Servidor);
                else
                    emailClient = new SmtpClient(Servidor, Porta);

                //se for envio autenticado
                if (Usuario != null && Usuario.Trim() != "" &&
                    Senha != null && Senha.Trim() != "")
                {
                    emailClient.UseDefaultCredentials = false;
                    emailClient.Credentials = new System.Net.NetworkCredential(Usuario, Senha);
                }

                //envia email            	
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["EscreverEmailArquivo"]))
                    emailClient.Send(mess);
                else
                    EscreveArquivo(mess, ConfigurationManager.AppSettings["EscreverEmailArquivo"]);
            }

            public void AdicionarAnexo(string nomeArquivo_)
            {
                if (_anexos == null)
                    _anexos = new List<string>();

                _anexos.Add(nomeArquivo_);
            }

            private void EscreveArquivo(MailMessage mess, string pathArquivo)
            {
                System.IO.StreamWriter mWriter = new System.IO.StreamWriter(pathArquivo, true);
                mWriter.WriteLine("Assunto:" + mess.Subject);
                mWriter.WriteLine("De:" + mess.From.ToString());
                mWriter.WriteLine("Para:" + mess.To.ToString());
                mWriter.WriteLine("Copia:" + mess.CC.ToString());
                mWriter.WriteLine("Copia Oculta:" + mess.Bcc.ToString());
                mWriter.WriteLine("Data:" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                mWriter.WriteLine("----------------------------------------");
                mWriter.WriteLine(mess.Body);
                mWriter.WriteLine("----------------------------------------");
                mWriter.Close();
                mWriter.Dispose();
            }

            #endregion

        }

    }
}
