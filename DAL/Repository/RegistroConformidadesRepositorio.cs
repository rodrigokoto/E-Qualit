using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL.Repository
{
    public class RegistroConformidadesRepositorio : BaseRepositorio<RegistroConformidade>, IRegistroConformidadesRepositorio
    {
        public RegistroConformidade GerarNumeroSequencialPorSite(RegistroConformidade registroConformidade)
        {
            using (var dbContext = new BaseContext())
            {
                dbContext.Configuration.LazyLoadingEnabled = false;
                try
                {
                    var listaMaxGestao = dbContext.RegistroConformidade
                    .Where(x => x.IdSite == registroConformidade.IdSite && x.TipoRegistro == registroConformidade.TipoRegistro).ToList();

                    if (listaMaxGestao.Count == 0)
                    {
                        registroConformidade.NuRegistro = 1;
                    }
                    else
                    {
                        var maxNumeroRegistro = listaMaxGestao.Max(x => x.NuRegistro);
                        registroConformidade.NuRegistro = maxNumeroRegistro + 1;
                    }


                    return registroConformidade;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            
        }

        public void DeletarRegistro(int idGestaoDeRisco)
        {
            try
            {
                var teste = Db.RegistroConformidade
                                    .Where(x => x.IdRegistroConformidade == idGestaoDeRisco).FirstOrDefault();

                Db.RegistroConformidade.Remove(teste);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RegistroConformidade GetByIdAsNoTracking(int id)
        {
            using (var dbContext = Db)
            {
                dbContext.Configuration.LazyLoadingEnabled = false;

                var registroSimple = dbContext.RegistroConformidade
                    .AsNoTracking()
                    .Where(x => x.IdRegistroConformidade == id)
                    .FirstOrDefault();

                registroSimple.Emissor = dbContext.Usuario
                    .Where(x => x.IdUsuario == registroSimple.IdEmissor)
                    .FirstOrDefault();

                return registroSimple;

            }
        }

        public DataTable RetornarDadosGraficoNcsMes(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("DtDe", dtDe.ToString("yyyy-MM-dd 00:00:00"));
            parametros.Add("DtAte", dtAte.ToString("yyyy-MM-dd 23:59:59"));

            if (idTipoNaoConformidade.HasValue)
                parametros.Add("IdTipoNaoConformidade", idTipoNaoConformidade.Value);

            parametros.Add("IdSite", idSite);

            var dtDados = base.GetDataTable("SP_GRAFICO_NCS_MES", CommandType.StoredProcedure, parametros);

            return dtDados;

        }


        public DataTable RetornarDadosGraficoNcsAbertasFechadas(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("DtDe", dtDe.ToString("yyyy-MM-dd 00:00:00"));
            parametros.Add("DtAte", dtAte.ToString("yyyy-MM-dd 23:59:59"));

            if (idTipoNaoConformidade.HasValue)
                parametros.Add("IdTipoNaoConformidade", idTipoNaoConformidade.Value);

            parametros.Add("IdSite", idSite);

            var dtDados = base.GetDataTable("SP_GRAFICO_NCS_GERADAS_ABERTAS_FECHADAS", CommandType.StoredProcedure, parametros);

            return dtDados;

        }

        public DataTable RetornarDadosGraficoNcsAcaoCorretiva(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("DtDe", dtDe.ToString("yyyy-MM-dd 00:00:00"));
            parametros.Add("DtAte", dtAte.ToString("yyyy-MM-dd 23:59:59"));

            if (idTipoNaoConformidade.HasValue)
                parametros.Add("IdTipoNaoConformidade", idTipoNaoConformidade.Value);

            parametros.Add("IdSite", idSite);

            var dtDados = base.GetDataTable("SP_GRAFICO_NCS_GERADAS_COMAACAOCORRETIVA", CommandType.StoredProcedure, parametros);

            return dtDados;

        }

        public DataTable RetornarDadosGraficoNcsTipo(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("DtDe", dtDe.ToString("yyyy-MM-dd 00:00:00"));
            parametros.Add("DtAte", dtAte.ToString("yyyy-MM-dd 23:59:59"));

            if (idTipoNaoConformidade.HasValue)
                parametros.Add("IdTipoNaoConformidade", idTipoNaoConformidade.Value);

            parametros.Add("IdSite", idSite);

            var dtDados = base.GetDataTable("SP_GRAFICO_NCS_TIPO", CommandType.StoredProcedure, parametros);

            return dtDados;

        }

        public DataTable RetornarDadosGraficoNcsProcesso(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("DtDe", dtDe.ToString("yyyy-MM-dd 00:00:00"));
            parametros.Add("DtAte", dtAte.ToString("yyyy-MM-dd 23:59:59"));

            if (idTipoNaoConformidade.HasValue)
                parametros.Add("IdTipoNaoConformidade", idTipoNaoConformidade.Value);

            parametros.Add("IdSite", idSite);

            var dtDados = base.GetDataTable("SP_GRAFICO_NCS_PROCESSO", CommandType.StoredProcedure, parametros);

            return dtDados;

        }

        public DataTable RetornarDadosGraficoNcsSite(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade)
        {
            var parametros = new Dictionary<string, object>();
            parametros.Add("DtDe", dtDe.ToString("yyyy-MM-dd 00:00:00"));
            parametros.Add("DtAte", dtAte.ToString("yyyy-MM-dd 23:59:59"));

            if (idTipoNaoConformidade.HasValue)
                parametros.Add("IdTipoNaoConformidade", idTipoNaoConformidade.Value);

            var dtDados = base.GetDataTable("SP_GRAFICO_NCS_SITE", CommandType.StoredProcedure, parametros);

            return dtDados;

        }


    }
}
