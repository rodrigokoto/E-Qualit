using Dominio.Entidade;
using System;
using System.Data;

namespace Dominio.Interface.Repositorio
{
    public interface IRegistroConformidadesRepositorio: IBaseRepositorio<RegistroConformidade>
    {
        RegistroConformidade GerarNumeroSequencialPorSite(RegistroConformidade registroConformidade);
        RegistroConformidade GetByIdAsNoTracking(int id);
        DataTable RetornarDadosGraficoNcsMes(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoNcsTipo(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoNcsAbertasFechadas(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoNcsAcaoCorretiva(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoNcsProcesso(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoNcsSite(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idCliente);

        DataTable RetornarDadosGraficoMelhoriaMes(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoMelhoriaTipo(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoMelhoriaAbertasFechadas(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoMelhoriaProcesso(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite);
        DataTable RetornarDadosGraficoMelhoriasSite(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idCliente);

    }
}
