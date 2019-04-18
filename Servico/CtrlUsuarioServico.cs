using Biblioteca;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Servico
{
    public class CtrlUsuarioServico : BaseService, IDisposable, IGenericService<Entidade.CtrlUsuario>
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    unitOfWork.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Entidade.CtrlUsuario New()
        {
            Entidade.CtrlUsuario _entidade = new Entidade.CtrlUsuario();
            List<DAL.Models.CtrlPerfil> _perfis = new List<DAL.Models.CtrlPerfil>();

            try
            {
                this.Sucesso = true;
                _perfis = unitOfWork.CtrlPerfilRepository.GetAll().ToList();

                _entidade.PerfiDisponveis = AutoMapper.Mapper.Map<List<Entidade.CtrlPerfil>>(_perfis);
                _entidade.PerfiSelecionados = new List<Entidade.CtrlPerfil>();
                _entidade.Perfis = AutoMapper.Mapper.Map<List<Entidade.CtrlPerfil>>(_perfis);
                _entidade.PerfisUsuario = new string[] { "0" };
                _entidade.FlAtivo = true;
                _entidade.FlSexo = "M";
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }

            return _entidade;
        }

        public Entidade.CtrlUsuario GetById(int id)
        {
            Entidade.CtrlUsuario _entidade = new Entidade.CtrlUsuario();
            List<DAL.Models.CtrlPerfil> _perfis = new List<DAL.Models.CtrlPerfil>();

            List<DAL.Models.CtrlPerfil> _perfilSelecionados = new List<DAL.Models.CtrlPerfil>();
            List<DAL.Models.CtrlPerfil> _perfilDisponivel = new List<DAL.Models.CtrlPerfil>();

            try
            {
                this.Sucesso = true;

                DAL.Models.CtrlUsuario _entity = unitOfWork.CtrlUsuarioRepository.Get(f => f.IdUsuario == id).FirstOrDefault();

                _entity.CtrlPerfilUsuarios = unitOfWork.CtrlPerfilUsuarioRepository.Get(f => f.IdUsuario == id).ToList();
                _perfis = unitOfWork.CtrlPerfilRepository.GetAll().ToList();

                foreach (DAL.Models.CtrlPerfil item in _perfis)
                {
                    DAL.Models.CtrlPerfilUsuario _achouPerfil = _entity.CtrlPerfilUsuarios.Where(w => w.IdPefil == item.IdPefil).FirstOrDefault();
                    if (_achouPerfil == null)
                        _perfilDisponivel.Add(item);
                    else
                        _perfilSelecionados.Add(item);
                }

                _entidade = AutoMapper.Mapper.Map<Entidade.CtrlUsuario>(_entity);

                _entidade.PerfiDisponveis = AutoMapper.Mapper.Map<List<Entidade.CtrlPerfil>>(_perfilDisponivel);
                _entidade.PerfiSelecionados = AutoMapper.Mapper.Map<List<Entidade.CtrlPerfil>>(_perfilSelecionados);
                _entidade.Perfis = AutoMapper.Mapper.Map<List<Entidade.CtrlPerfil>>(_perfis);

                return _entidade;
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }

            return _entidade;
        }

        public IQueryable<Entidade.CtrlUsuario> GetAll()
        {
            List<Entidade.CtrlUsuario> _entidade = new List<Entidade.CtrlUsuario>();

            try
            {
                this.Sucesso = true;

                List<DAL.Models.CtrlUsuario> _entity = unitOfWork.CtrlUsuarioRepository.GetAll().ToList();
                _entidade = AutoMapper.Mapper.Map<List<Entidade.CtrlUsuario>>(_entity);

                return _entidade.AsQueryable();
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }
            return _entidade.AsQueryable();
        }

        public Entidade.CtrlUsuario Add(Entidade.CtrlUsuario item)
        {
            Entidade.CtrlUsuario retorno = new Entidade.CtrlUsuario();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlUsuario _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlUsuario>(item);
                    _entity.CdSenha = Biblioteca.Util.GeraNovaSenha();

                    unitOfWork.CtrlUsuarioRepository.Insert(_entity);
                    unitOfWork.Save();

                    retorno.IdUsuario = _entity.IdUsuario;

                    foreach (string sPerfil in item.PerfisUsuario)
                    {
                        DAL.Models.CtrlPerfilUsuario _perfilUsario = new DAL.Models.CtrlPerfilUsuario() { IdPefil = Convert.ToInt32(sPerfil), IdUsuario = retorno.IdUsuario };

                        unitOfWork.CtrlPerfilUsuarioRepository.Insert(_perfilUsario);
                        unitOfWork.Save();
                    }

                    _transaction.Complete();

                    return retorno;
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    this.Sucesso = false;
                }
            }
            return retorno;
        }

        public void Update(Entidade.CtrlUsuario item)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlUsuario _entity = unitOfWork.CtrlUsuarioRepository.GetByID(item.IdUsuario);
                    if (_entity == null || _entity.IdUsuario == 0)
                    {
                        throw new Exception("Não é possível atualizar dados de um usuário excluído");
                    }

                    _entity.CdIdentificacao = item.CdIdentificacao;
                    _entity.FlAtivo = item.FlAtivo;
                    _entity.NmApelido = item.NmApelido;
                    _entity.FlSexo = item.FlSexo;

                    unitOfWork.CtrlUsuarioRepository.Update(_entity);
                    unitOfWork.Save();

                    _entity.CtrlPerfilUsuarios = unitOfWork.CtrlPerfilUsuarioRepository.Get(f => f.IdUsuario == _entity.IdUsuario).ToList();

                    //Exclui da base os perfils desmarcados
                    List<int> _ExcluiPerfis = new List<int>();
                    foreach (DAL.Models.CtrlPerfilUsuario sPerfil in _entity.CtrlPerfilUsuarios)
                    {
                        var pefil = item.PerfisUsuario.Where(w => w == sPerfil.IdPefil.ToString()).FirstOrDefault();
                        if (pefil == null)
                        {
                            _ExcluiPerfis.Add(sPerfil.IdCtrlPerfilUsuario);
                        }
                    }
                    foreach (int sPerfil in _ExcluiPerfis)
                    {
                        unitOfWork.CtrlPerfilUsuarioRepository.Delete(sPerfil);
                        unitOfWork.Save();
                    }

                    //Adiciona os novos perfils
                    foreach (string sPerfil in item.PerfisUsuario)
                    {
                        int _IdPerfilSelecionado = Convert.ToInt32(sPerfil);
                        DAL.Models.CtrlPerfilUsuario _perfilUsario = unitOfWork.CtrlPerfilUsuarioRepository
                                                                               .Get(x => x.IdUsuario == _entity.IdUsuario && x.IdPefil == _IdPerfilSelecionado)
                                                                               .FirstOrDefault();
                        if (_perfilUsario == null)
                        {
                            _perfilUsario = new DAL.Models.CtrlPerfilUsuario() { IdPefil = Convert.ToInt32(sPerfil), IdUsuario = _entity.IdUsuario };
                            unitOfWork.CtrlPerfilUsuarioRepository.Insert(_perfilUsario);
                            unitOfWork.Save();
                        }
                    }

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    this.Sucesso = false;
                }
            }
        }

        public void Remove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;
                    int[] idsFuncUsuario = unitOfWork.CtrlFuncionalidadeUsuarioRepository.Get(f => f.IdUsuario == id).Select(s => s.IdFuncionalidadeUsuario).ToArray();
                    int[] idsPerfilUsuario = unitOfWork.CtrlPerfilUsuarioRepository.Get(f => f.IdUsuario == id).Select(s => s.IdCtrlPerfilUsuario).ToArray();

                    foreach (int idtodelete in idsFuncUsuario)
                    {
                        unitOfWork.CtrlFuncionalidadeUsuarioRepository.Delete(idtodelete);
                        unitOfWork.Save();
                    }

                    foreach (int idtodelete in idsPerfilUsuario)
                    {
                        unitOfWork.CtrlPerfilUsuarioRepository.Delete(idtodelete);
                        unitOfWork.Save();
                    }

                    unitOfWork.CtrlUsuarioRepository.Delete(id);
                    unitOfWork.Save();

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    this.Sucesso = false;
                }
            }
        }

        public string RedefinirSenha(string Email)
        {
            Entidade.CtrlUsuario _entidade = new Entidade.CtrlUsuario();
            string senha = string.Empty;
            string email = string.Empty;

            try
            {
                this.Sucesso = true;

                DAL.Models.CtrlUsuario _entity = unitOfWork.CtrlUsuarioRepository.GetAll().Where(x => x.CdIdentificacao == Email).FirstOrDefault();
                if (_entity == null)
                {
                    this.Error = new Exception("Usuário não localizado.");
                    this.Sucesso = false;
                    return string.Empty;
                }

                _entidade = AutoMapper.Mapper.Map<Entidade.CtrlUsuario>(_entity);

                senha = Biblioteca.Util.GeraNovaSenha();
                _entity.CdSenha = Biblioteca.Util.Sha1Hash(senha);

                unitOfWork.CtrlUsuarioRepository.Update(_entity);
                unitOfWork.Save();

                email = _entity.CdIdentificacao;

                string template = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["DiretorioTemplates"] + @"\ReenvioSenha.html");

                string conteudo = template;
                conteudo = conteudo.Replace("#NOME#", _entity.NmApelido);
                conteudo = conteudo.Replace("#SENHA#", senha);
                conteudo = conteudo.Replace("#DOMINIO#", ConfigurationManager.AppSettings["Dominio"]);

                Email _email = new Email();
                _email.Assunto = "Reenvio de Senha Acesso ao Sistema FitClinic";
                _email.De = "FitClinic <" + ConfigurationManager.AppSettings["EmailDE"] + ">";
                _email.Para = _entity.NmApelido + "<" + _entity.CdIdentificacao + ">";
                _email.Conteudo = conteudo;
                _email.Enviar();

                return Email;
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }

            return Email;
        }

        public void TrocaSenha(string novaSenha, int idUser)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;
                    DAL.Models.CtrlUsuario _model = unitOfWork.CtrlUsuarioRepository.GetByID(idUser);

                    _model.CdSenha = Biblioteca.Util.Sha1Hash(novaSenha);

                    unitOfWork.CtrlUsuarioRepository.Update(_model);
                    unitOfWork.Save();

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    this.Sucesso = false;
                }
            }
        }

        public void AtivaInativa(int id, bool fl)
        {

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlUsuario _entity = unitOfWork.CtrlUsuarioRepository.Get(f => f.IdUsuario == id).FirstOrDefault();
                    _entity.FlAtivo = fl;

                    unitOfWork.CtrlUsuarioRepository.Update(_entity);
                    unitOfWork.Save();

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    this.Sucesso = false;
                }
            }
        }
    }
}
