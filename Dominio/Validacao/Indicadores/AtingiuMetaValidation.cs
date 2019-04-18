using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Indicadores;

namespace Dominio.Validacao.Indicadores
{
    public class AtingiuMetaValidation : Validator<PeriodicidaDeAnalise>
    {
        public AtingiuMetaValidation()
        {
            var deveTerPlanoDeAcao = new SeNãoAtingiuAMetaDeveTerPlanoDeAcaoOuJustificativa();

            base.Add(Traducao.Resource.PlanoDeAcao, new Rule<PeriodicidaDeAnalise>(deveTerPlanoDeAcao, Traducao.Resource.MsgPlanoDeAcao));
        }
    }
}
