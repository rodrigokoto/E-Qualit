using Entidade;
using Servico.Validacao.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Servico.Validacao
{

    public class ClienteValidacao : IBaseValidacao<Cliente>
    {
        private UsuarioValidacao _validaUsuario = new UsuarioValidacao();
        private SiteValidacao _validaSite = new SiteValidacao();
        private List<ValidationResult> erros = new List<ValidationResult>();

        public List<ValidationResult> ClienteValido(Cliente cliente, CtrlUsuario usuario)
        {
            var erroOrigem = new List<string>();

            if (!_validaUsuario.EAdministrador(usuario.IdPerfil))
            {
                erroOrigem.Add("Cliente");
                erros.Add(new ValidationResult("Usuário informado não possui permissão para executar esta operação.", erroOrigem));
            }

            ObjetoValido(cliente);

            return erros;
        }

        public bool ClientePossuiId(int idCliente)
        {
            if (idCliente == 0)
            {
                return true;
            }
            return false;
        }

        public void ObjetoValido(Cliente cliente)
        {
            if (!cliente.IsValid())
            {
                erros.AddRange(cliente.ModelErros);
            }
        }
    }
}
