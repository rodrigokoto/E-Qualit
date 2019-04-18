using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Usuarios;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Usuarios
{
    public class AptoParaCadastroValidation : Validator<Usuario>
    {
        public AptoParaCadastroValidation(IUsuarioRepositorio usuarioRepositorio)
        {
            var devePossuirEmailUnicio = new DevePossuirEmailUnico(usuarioRepositorio);
            var devePossuirCPFUnicio = new DevePossuirCPFUnico(usuarioRepositorio);

            base.Add(Traducao.Resource.EmailDuplicado, new Rule<Usuario>(devePossuirEmailUnicio, Traducao.Resource.MsgErroEmailDuplicado));
            base.Add(Traducao.Resource.CpfDuplicado, new Rule<Usuario>(devePossuirCPFUnicio, Traducao.Resource.MsgErroCpfDuplicado));

        }
    }
}
