using Entidade;
using Servico.Validacao.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dominio.Enumerado;

namespace Servico.Validacao
{
    public class UsuarioValidacao : IBaseValidacao<CtrlUsuario>
    {
        private long _administrador = (int)PerfisAcesso.Administrador;
        private long _suporte = (int)PerfisAcesso.Suporte;
        private long _coordenador = (int)PerfisAcesso.Coordenador;
        private long _colaborador = (int)PerfisAcesso.Colaborador;

        private SiteValidacao _validaSite = new SiteValidacao();
        private List<ValidationResult> erros = new List<ValidationResult>();

        public List<ValidationResult> UsuarioValido(CtrlUsuario usuario)
        {
            ObjetoValido(usuario);
            return erros;
        }

        public List<ValidationResult> UsuarioEstaLogado(CtrlUsuario usuario)
        {
            var erroOrigem = new List<string>();
                 

            if (!UsuarioLogadoInformado(usuario))
            {
                erroOrigem.Add("");
                erros.Add(new ValidationResult("Não foi possível identificar o usuário", erroOrigem));

                return erros;
            }
            return erros;
        }

        public List<ValidationResult> ValidaUsuarioLogado(CtrlUsuario usuario)
        {
            var erroOrigem = new List<string>();

            List<ValidationResult> _erros = new List<ValidationResult>();

            if (!UsuarioLogadoInformado(usuario))
            {
                erroOrigem.Add("");
                _erros.Add(new ValidationResult("Não foi possível identificar o usuário", erroOrigem));

                return _erros;
            }
            return _erros;
        }

        public bool SuporteEMesmoCliente(CtrlUsuario usuarioSolicitante,
                                 CtrlUsuario usuarioCadastrado)
        {
            if (ESuporte(usuarioSolicitante.IdPerfil))
            {
                if (!EAdministrador(usuarioCadastrado.IdPerfil) ||
                    !ESuporte(usuarioSolicitante.IdPerfil))
                {
                    if (EMesmoCliente(usuarioSolicitante.UsuarioClienteSite.FirstOrDefault().IdCliente, usuarioCadastrado.UsuarioClienteSite.FirstOrDefault().IdCliente))
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        public List<ValidationResult> AdministradorAptoCadastrar(CtrlUsuario usuarioSolicitante, CtrlUsuario usuario)
        {
            var retorno = new CtrlUsuario();

            if (EAdministrador(usuarioSolicitante.IdPerfil) ||
                ECoordenador(usuarioSolicitante.IdPerfil))
            {
                return retorno.ModelErros;
            }
            else if (SuporteEMesmoCliente(usuarioSolicitante, usuario))
            {
                if (!EAdministrador(usuario.IdPerfil) ||
                    !ESuporte(usuario.IdPerfil))
                {
                    return retorno.ModelErros;
                }
            }
            else if (ECoordenador(usuarioSolicitante.IdPerfil))
            {
                if (!ECoordenador(usuario.IdPerfil) || !EColaborador(usuario.IdPerfil))
                {
                    return retorno.ModelErros;
                }
            }

            var erroOrigem = new List<string>
            {
                ""
            };
            retorno.ModelErros.Add(new ValidationResult("Necessário o cadastro de pelo menos um site.", erroOrigem));
            return retorno.ModelErros;
        }

        public List<ValidationResult> ColaboradorAptoCadastrar(CtrlUsuario usuarioSolicitante, CtrlUsuario usuario)
        {
            var retorno = new CtrlUsuario();

            if (EAdministrador(usuarioSolicitante.IdPerfil) ||
                ECoordenador(usuarioSolicitante.IdPerfil))
            {
                return retorno.ModelErros;
            }
            else if (SuporteEMesmoCliente(usuarioSolicitante, usuario))
            {
                if (!EAdministrador(usuario.IdPerfil) ||
                    !ESuporte(usuario.IdPerfil))
                {
                    return retorno.ModelErros;
                }
            }
            else if (ECoordenador(usuarioSolicitante.IdPerfil))
            {
                if (!ECoordenador(usuario.IdPerfil) || !EColaborador(usuario.IdPerfil))
                {
                    return retorno.ModelErros;
                }
            }

            var erroOrigem = new List<string>
            {
                ""
            };
            retorno.ModelErros.Add(new ValidationResult("Necessário o cadastro de pelo menos um site.", erroOrigem));
            return retorno.ModelErros;
        }

        public List<ValidationResult> RegrasCoordenadorSuporte(CtrlUsuario usuarioSolicitante, CtrlUsuario usuario)
        {
            var retorno = new CtrlUsuario();
            var erroOrigem = new List<string>();


            if (usuario.IdPerfil == _coordenador)
            {
                if (_validaSite.ListaEstaVazia(usuario.LvSite.Where(x => x.FlSelecionado == true).Count()))
                {
                    erroOrigem.Add("");
                    retorno.ModelErros.Add(new ValidationResult("Necessário o cadastro de pelo menos um site.", erroOrigem));
                }
            }
            else
            {
                if (_validaSite.ListaEstaVazia(usuario.LvCliente.Where(x => x.FlSelecionado == true).Count()))
                {
                    erroOrigem.Add("");
                    retorno.ModelErros.Add(new ValidationResult("Necessário o cadastro de pelo menos um cliente.", erroOrigem));
                }
            }

            if (!EAdministrador(usuarioSolicitante.IdPerfil) &&
                !ECoordenador(usuarioSolicitante.IdPerfil))
            {
                erroOrigem.Add("");
                retorno.ModelErros.Add(new ValidationResult("Usuário sem permissão.", erroOrigem));
            }

            return retorno.ModelErros;
        }
        
        public bool EMesmoCliente(int idClienteSolicitante, int idClienteCadastro)
        {
            if (idClienteSolicitante == idClienteCadastro)
            {
                return true;
            }
            return false;
        }

        public bool EAdministrador(int idPerfil)
        {
            if (idPerfil != _administrador)
            {
                return false;
            }
            return true;
        }

        public bool ESuporte(int idPerfil)
        {
            if (idPerfil != _suporte)
            {
                return false;
            }
            return true;
        }

        public bool ECoordenador(int idPerfil)
        {
            if (idPerfil != _coordenador)
            {
                return false;
            }
            return true;
        }

        public bool EColaborador(int idPerfil)
        {
            if (idPerfil != _colaborador)
            {
                return false;
            }
            return true;
        }

        public bool UsuarioPreenchido(CtrlUsuario usuario)
        {
            if (usuario != null && usuario.IsValid())
            {
                return true;
            }
            return false;
        }

        private bool UsuarioLogadoInformado(CtrlUsuario usuario)
        {
            if (usuario == null || usuario.IdUsuario == 0)
            {
                return false;
            }
            return true;
        }

        public void ObjetoValido(CtrlUsuario usuario)
        {
            if (!usuario.IsValid())
            {
                erros.AddRange(usuario.ModelErros);
            }
        }
    }
}
