using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using System.Linq;

namespace Dominio.Especificacao.Indicadores
{
    public class SeNãoAtingiuAMetaDeveTerPlanoDeAcaoOuJustificativa : ISpecification<PeriodicidaDeAnalise>
    {
        public bool IsSatisfiedBy(PeriodicidaDeAnalise periodicidaDeAnalise)
        {
            var valorMeta = periodicidaDeAnalise.PlanoDeVoo.FirstOrDefault().Valor;
            var valorRealizado = periodicidaDeAnalise.MetasRealizadas.FirstOrDefault().Realizado;
            
            if (BateuMeta(valorMeta, valorRealizado.Value) || PossuiJustificativa(periodicidaDeAnalise.Justificativa))
            {
                return true;
            }
            return false;
        }

        private bool BateuMeta(double valorMeta, double valorRealizado)
        {
            if (valorMeta > valorRealizado)
            {
                return false;
            }
            return true;
        }

        private bool PossuiJustificativa(string justificativa)
        {
            if (string.IsNullOrEmpty(justificativa))
            {
                return false;
            }
            return true;
        }
    }
}
