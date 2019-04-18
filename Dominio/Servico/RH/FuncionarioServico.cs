using Dominio.Entidade.RH;
using Dominio.Interface.Repositorio.RH;
using Dominio.Interface.Servico.RH;
using System.Collections.Generic;
using Dominio.Validacao.Funcionarios;

namespace Dominio.Servico.RH
{
    public class FuncionarioServico : IFuncionarioServico
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public FuncionarioServico(IFuncionarioRepositorio funcionarioRepositorio) 
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public void Valido(Funcionario funcionario, ref List<string> erros)
        {
            funcionario.ValidationResult =  new AptoParaCadastroValidation().Validate(funcionario);

            if (!funcionario.ValidationResult.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(funcionario.ValidationResult));
            }
        }
    }
}
