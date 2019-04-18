using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Processos;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Processos
{
    public class AptoParaCadastraProcessoValidation : Validator<Processo>
    {    
        public AptoParaCadastraProcessoValidation(IProcessoRepositorio repositorio)
        {
            var devePossuirNomeUnicoPorSite = new DevePossuirNomeUnicoPorSite(repositorio);

            base.Add(Traducao.Resource.MsgProcessoDuplicado, new Rule<Processo>(devePossuirNomeUnicoPorSite, Traducao.Resource.MsgdevePossuirNomeUnicoPorSite));
        }
    }
}
