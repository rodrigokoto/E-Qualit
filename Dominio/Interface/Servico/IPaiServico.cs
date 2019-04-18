using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IPaiServico 
    {
        void Valido(Plai plai, ref List<string> erros);
        Pai ObterPorAno(int? ano);
    }
}
