using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.RegistroConformidades;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class AptoParaExclusaoNaoConformidadeValidation : Validator<RegistroConformidade>
    {
        private readonly IRegistroConformidadesRepositorio _registroConformidadeRepositorio;

        public AptoParaExclusaoNaoConformidadeValidation(IRegistroConformidadesRepositorio registroConformidadeRepositorio)
        {
            //_registroConformidadeRepositorio = registroConformidadeRepositorio;

            //var deveSerMaiorRegistro = new DeveSerORegistroMaisAtualPeloSiteEspecification(_registroConformidadeRepositorio);
            //base.Add("Não Conformidade", new Rule<RegistroConformidade>(deveSerMaiorRegistro, "Não Conformidade deve ter o registro mais atual para deleção."));
        }
    }
}
