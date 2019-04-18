using System;
using System.Transactions;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using Servico.Validacao;
using Dominio.Enumerado;
using Dominio.Entidade;

namespace Servico
{
    public partial class Service
    {
        private CargoValidacao _validaCargo = new CargoValidacao();

        public Entidade.Cargo CargoNew(int idSite)
        {
            Entidade.Cargo _entidade = null;
            Sucesso = true;
            Mensagem = "OK";

            try
            {
                _entidade = dal.CargoNew(idSite);
            }
            catch (Exception ex)
            {
                Sucesso = false;
                Mensagem = ex.Message;
            }

            return _entidade;
        }

        public Entidade.Cargo CargoGetByID(int id, int IdUsuario)
        {
            Entidade.Cargo _entidade = null;
            try
            {
                Entidade.CtrlUsuario usuario = dal.CtrlUsuarioGetByID(IdUsuario);

                if (usuario.IdPerfil == (int)PerfisAcesso.Colaborador)
                {
                    Sucesso = false;
                    Mensagem = Traducao.Resource.Acesso_Negado;
                }
                else
                {
                    _entidade = dal.CargoGetByID(id);

                    Sucesso = true;
                    Mensagem = "OK";
                }
            }
            catch (Exception ex)
            {
                Sucesso = false;

                Mensagem = Error.Message;
            }
            return _entidade;
        }

        public List<Entidade.Cargo> CargoGetByIdSite(int idSite, int idUsuario)
        {
            List<Entidade.Cargo> _cargoList = new List<Entidade.Cargo>();
            try
            {
                Entidade.CtrlUsuario usuario = dal.CtrlUsuarioGetByID(idUsuario);
                if (usuario.IdPerfil == (int)PerfisAcesso.Colaborador)
                {
                    throw new Exception(Traducao.Resource.Acesso_Negado);
                }
                else
                {
                    List<DAL.Entity.Cargo> _lista = dal.CargoGetByIdSite(idSite);
                    foreach (DAL.Entity.Cargo cargo in _lista)
                    {
                        _cargoList.Add(new Entidade.Cargo()
                        {
                            IdCargo = cargo.IdCargo,
                            IdSite = cargo.IdSite,
                            NmNome = cargo.NmNome
                        });
                    }
                }

                Sucesso = true;
                Mensagem = "OK";
            }
            catch (Exception ex)
            {
                Erros("CargoServico.CargoGetByIdSite", ex);
            }

            return _cargoList;
        }

        public Entidade.Cargo CargoSave(Entidade.Cargo cargo, int idUsuarioLogado)
        {
            Entidade.Cargo _cargo = new Entidade.Cargo();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var idPerfil = unitOfWork.CtrlUsuarioRepository.GetByID(idUsuarioLogado).IdPerfil;

                    Sucesso = true;

                    if (_validaUsuario.ECoordenador(idPerfil) || _validaUsuario.EAdministrador(idPerfil))
                    {
                        cargo.ModelErros.AddRange(_validaCargo.CargoValido(cargo));

                        if (cargo.ModelErros.Count > 0)
                        {
                            Sucesso = false;
                            return cargo;
                        }

                        _cargo = dal.CargoSave(cargo);
                        if (cargo.IdCargo == 0)
                        {
                            Mensagem = Traducao.Resource.mensagem_registro_incluido;
                        }
                        else
                        {
                            Mensagem = Traducao.Resource.mensagem_registro_alterado;
                        }
                    }

                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    var log = new Log(Convert.ToInt32(Acao.AtualizarEmailEnviado), ex);
                    _logRepositorio.Add(log);
                }
            }
            return _cargo;
        }

        public void CargoRemove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    dal.CargoRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("AK_Cargo") > 0)
                    {
                        Erros("CargoServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cargo_AK_Cargo));
                    }
                    else if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Cargo") > 0)
                    {
                        Erros("CargoServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cargo_PK_Cargo));
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioCargo_Cargo") > 0)
                    {
                        Erros("CargoServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cargo_FK_UsuarioCargo_Cargo));
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_CargoProcesso_Cargo") > 0)
                    {
                        Erros("CargoServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cargo_FK_CargoProcesso_Cargo));
                    }
                    else
                    {
                        Erros("CargoServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                    }
                }
                catch (DbEntityValidationException e)
                {
                    DBErros("CargoServico.CargoRemove", e);
                }
                catch (Exception ex)
                {
                    Erros("CargoServico.CargoRemove", ex);
                }
            }
        }

        public Entidade.PermissaoProcesso CargoPermissaoNew(int idSite, string nomeProcesso)
        {
            Entidade.PermissaoProcesso _processo = null;
            try
            {
                _processo = dal.CargoProcessoNew(idSite, nomeProcesso);
                Sucesso = true;
                Mensagem = "OK";
            }
            catch
            {
                Sucesso = false;
                Mensagem = Error.Message;
            }
            return _processo;
        }
    }
}
