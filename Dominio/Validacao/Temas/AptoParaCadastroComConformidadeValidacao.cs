using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Temas;

namespace Dominio.Validacao.Temas
{
    public class AptoParaCadastroComConformidadeValidacao : Validator<AnaliseCriticaTema>
    {
        public AptoParaCadastroComConformidadeValidacao()
        {
            var deveTerGestaoRisco = new DeveTerGestaoDeRiscoInformadoEspecification();

            base.Add(Traducao.Resource.DocDocumento_lbl_GestaoRisco, new Rule<AnaliseCriticaTema>(deveTerGestaoRisco, Traducao.Resource.MsgGestaoNaoInformado));
        }
    }
}
