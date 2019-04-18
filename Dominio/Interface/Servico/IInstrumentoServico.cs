using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IInstrumentoServico
    {
        void Valido(Instrumento instrumento, ref List<string> erros);
        bool DeletarInstrumentoEDependencias(int id);
        List<Instrumento> ObterPorIdSite(int idSite);
    }
}
