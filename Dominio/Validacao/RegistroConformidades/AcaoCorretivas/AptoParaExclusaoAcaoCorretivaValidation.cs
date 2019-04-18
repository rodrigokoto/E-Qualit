using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.RegistroConformidades;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.RegistroConformidades.AcaoCorretivas
{
    public class AptoParaExclusaoAcaoCorretivaValidation : Validator<RegistroConformidade>
    {
        private readonly IRegistroConformidadesRepositorio _registroConformidadeRepositorio;

        public AptoParaExclusaoAcaoCorretivaValidation(IRegistroConformidadesRepositorio registroConformidadeRepositorio)
        {
            _registroConformidadeRepositorio = registroConformidadeRepositorio;

            var deveSerMaiorRegistro = new DeveSerORegistroMaisAtualPeloSiteEspecification(_registroConformidadeRepositorio);
            base.Add(Traducao.Resource.MsgNaoConformidade, new Rule<RegistroConformidade>(deveSerMaiorRegistro, Traducao.Resource.MsgDeveSerMaiorRegistro));
        }
    }
}
