using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IControleImpressaoServico
    {
        void Valido(ControleImpressao controleImpressao, ref List<string> erros);
    }
}
