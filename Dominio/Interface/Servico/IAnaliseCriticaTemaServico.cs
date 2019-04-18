using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IAnaliseCriticaTemaServico 
    {
        void Remove(List<AnaliseCriticaTema> temas);
    }
}
