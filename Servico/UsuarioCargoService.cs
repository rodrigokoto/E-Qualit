using Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;

namespace Servico
{
    public partial class Service
    {
        private UsuarioCargo UsuarioCargoAdicionar(UsuarioCargo item)
        {
            Entidade.UsuarioCargo _entidade = new Entidade.UsuarioCargo();

            List<ValidationResult> _ModelErros = new List<ValidationResult>();//validacao.ValidaCargo(item);

            if (_ModelErros.Count > 0)
            {
                Sucesso = false;
                item.ModelErros = _ModelErros;
            }

            var usuarioCargoBase = new DAL.Entity.UsuarioCargo
            {
                IdCargo = item.IdCargo,
                IdUsuario = item.IdUsuario,
            };

            dal.UsuarioCargoSalvar(usuarioCargoBase);

            return _entidade;
        }

        public void UsuarioCargoRemoverPorUsuario(int idUsuario)
        {
            try
            {
                var _entity = unitOfWork.UsuarioCargoRepository.Get(x => x.IdUsuario == idUsuario);

                foreach (var entity in _entity)
                {
                    unitOfWork.UsuarioCargoRepository.Delete(entity);
                    unitOfWork.Save();
                }

            }
            catch (DbEntityValidationException e)
            {
                DBErros("UsuarioCargoService.UsuarioCargoremover", e);
            }
            catch (Exception ex)
            {
                Erros("UsuarioCargoService.UsuarioCargoremover", ex);
            }
        }

        public List<UsuarioCargo> UsuarioCargoObterPorUsuario(int idUsuario)
        {
            try
            {
                var _usuariosCargos = new List<Entidade.UsuarioCargo>();
                var _usuariosCargosCTX = unitOfWork.UsuarioCargoRepository.Get(x => x.IdUsuario == idUsuario);

                foreach (var usuarioCargos in _usuariosCargosCTX)
                {
                    _usuariosCargos.Add(new Entidade.UsuarioCargo
                    {
                        IdUsuarioProcesso = usuarioCargos.IdUsuarioProcesso,
                        IdUsuario = usuarioCargos.IdUsuario,
                        IdCargo = usuarioCargos.IdCargo
                    });
                }

                return _usuariosCargos;
            }
            catch (DbEntityValidationException e)
            {
                DBErros("UsuarioCargoService.UsuarioCargoObterPorUsuario", e);
            }
            catch (Exception ex)
            {
                Erros("UsuarioCargoService.UsuarioCargoObterPorUsuario", ex);
            }

            return null;
        }

        private void CadastrarCargosPorUsuario(List<UsuarioCargos> cargos, int idUsuario)
        {
            foreach (var cargo in cargos)
            {
                if (cargo.FlSelecionado)
                {
                    UsuarioCargo usuarioCargo = new UsuarioCargo
                    {
                        IdCargo = cargo.IdCargo,
                        IdUsuario = idUsuario
                    };

                    UsuarioCargoAdicionar(usuarioCargo);
                }
            }
        }
    }
}
