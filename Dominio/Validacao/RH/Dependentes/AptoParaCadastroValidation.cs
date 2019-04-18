using DomainValidation.Validation;
using Dominio.Entidade.RH;
using Dominio.Especificacao.RH.Dependentes;

namespace Dominio.Validacao.RH.Dependentes
{
    public class AptoParaCadastroValidation : Validator<Dependente>
    {
        public AptoParaCadastroValidation()
        {
            var devePossuirFuncionario = new DevePossuirFuncionarioEspecification();

            base.Add(Traducao.Resource.Funcionario, new Rule<Dependente>(devePossuirFuncionario, Traducao.Resource.MsgDevePossuirFuncionario));
        }
    }
}
