using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.RegistroConformidades.GestaoDeRiscos;

namespace Dominio.Validacao.RegistroConformidades.GestaoDeRiscos
{
    public class AptoParaCadastroComGestaoDeRiscoValidacao : Validator<RegistroConformidade>
    {
        public AptoParaCadastroComGestaoDeRiscoValidacao()
        {
            var deveTerGestaoRisco = new DeveTerGestaoDeRiscoInformadoEspecification();

            base.Add(Traducao.Resource.DocDocumento_lbl_GestaoRisco, new Rule<RegistroConformidade>(deveTerGestaoRisco, Traducao.Resource.MsgGestaoNaoInformado));
        }
    }
}
