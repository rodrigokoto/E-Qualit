using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Usuarios;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Usuarios
{
    public class AlterarSenhaValidation : Validator<Usuario>
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public AlterarSenhaValidation(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;

            var devePossuirSenhasIguais = new DevePossuirSenhaAtualIgualDoBancoAlteracao(_usuarioRepositorio);

            base.Add(Traducao.Resource.MsgErroAlterarSenha, new Rule<Usuario>(devePossuirSenhasIguais, Traducao.Resource.MsgErroSenhaNaoCorresponde));
        }
    }
}
