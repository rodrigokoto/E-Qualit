using DomainValidation.Validation;
using Dominio.Entidade.RH;
using Dominio.Especificacao.RH.Funcionarios;

namespace Dominio.Validacao.RH.Funcionarios
{
    public class AptoParaCadastroValidation : Validator<Funcionario>
    {
        public AptoParaCadastroValidation()
        {
            var deveTerValeTransporte = new DevePossuirValeTransporteInformadoEspecification();
            var deveTerCargo = new DevePossuirCargoEspecification();
            var vencimentoCNHobrigatorioSeInformado = new SeInformadoCNHDeveTerVencimentoCNH();
            var preencherTodosCampos = new DeveInformarCamposObrigatorios();
                        
            base.Add(Traducao.Resource.ValeTransporte, new Rule<Funcionario>(deveTerValeTransporte, Traducao.Resource.MsgDeveTerValeTransporte));
            base.Add(Traducao.Resource.Cargo, new Rule<Funcionario>(deveTerCargo, Traducao.Resource.MsgDeveTerCargo));
            base.Add(Traducao.Resource.CNH, new Rule<Funcionario>(vencimentoCNHobrigatorioSeInformado, Traducao.Resource.MsgDeveTerCNH));
            base.Add(Traducao.Resource.MsgErroEnvio, new Rule<Funcionario>(vencimentoCNHobrigatorioSeInformado, Traducao.Resource.MsgErroEnvio));
        }
    }
}
