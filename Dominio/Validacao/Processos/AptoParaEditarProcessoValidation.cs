using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Processos;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Processos
{
    public class AptoParaEditarProcessoValidation : Validator<Processo>
    {
        public AptoParaEditarProcessoValidation(IProcessoRepositorio repositorio)
        {
            var devePossuirNomeUnicoPorSite = new DevePossuirNomeUnicoPorSiteEdicao(repositorio);

            base.Add(Traducao.Resource.MsgProcessoDuplicado, new Rule<Processo>(devePossuirNomeUnicoPorSite, Traducao.Resource.MsgdevePossuirNomeUnicoPorSite));
        }
    }
}