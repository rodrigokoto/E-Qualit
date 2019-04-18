using Dominio.Entidade.RH;
using Dominio.Interface.Repositorio.RH;
using ApplicationService.Interface.RH;
using System.Collections.Generic;
using Dominio.Validacao.Funcionarios;
using Dominio.Servico;

namespace ApplicationService.Servico.RH
{
    public class FuncionarioAppServico : BaseServico<Funcionario>, IFuncionarioAppServico
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public FuncionarioAppServico(IFuncionarioRepositorio funcionarioRepositorio) : base(funcionarioRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public void Valido(Funcionario funcionario, ref List<string> erros)
        {
            funcionario.ValidationResult = 
                new AptoParaCadastroValidation().Validate(funcionario);

            if (!funcionario.ValidationResult.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(funcionario.ValidationResult));
            }


        }
    }
}
