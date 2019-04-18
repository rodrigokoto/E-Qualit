using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IProcessoServico
    {
        void Valido(Processo processo, ref List<string> erros);
        List<Processo> ListaProcessosPorSite(int site);
        List<Processo> ListaProcessosPorUsuario(int idUsuario);
        List<Processo> ListaProcessosPorSiteComTracking(int idSite);
        Processo GetProcessoById(int IdProcesso);
    }
}
