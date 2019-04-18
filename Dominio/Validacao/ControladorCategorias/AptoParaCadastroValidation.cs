using DomainValidation.Validation;

namespace Dominio.Validacao.ControladorCategorias
{
    public class AptoParaCadastroValidation : Validator<Entidade.ControladorCategoria>
    {
        public AptoParaCadastroValidation()
        {
            //Rule<Entidade.ControladorCategorias>(x => x.)


            //var obj = new Entidade.ControladorCategorias();

            // var possuiRisco = new PossuiGestaoDeRiscoEspecification();
  
           // var teste = (obj => obj.).NotEmpty();

            //base.Add("Erro na Gestão de Risco", new Rule<Entidade.ControladorCategorias>(possuiRisco, "E necessário o cadastro de uma Gestão de Risco."));

            //base.Add("Erro na Gestão de Risco", new Rule<Entidade.ControladorCategorias>(possuiRisco, "E necessário o cadastro de uma Gestão de Risco."));
        }
    }
}
