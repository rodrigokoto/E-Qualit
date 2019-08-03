using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;
using Dominio.Enumerado;
using System.Data.Entity;
using DAL.Context;


namespace DAL.Repository
{
    public class DocDocumentoRepositorio : BaseRepositorio<DocDocumento>, IDocDocumentoRepositorio
    {
        private readonly RegistroConformidadesRepositorio _registroConformidadesRepositorio = new RegistroConformidadesRepositorio();

        public IEnumerable<DocDocumento> ListaDocumentosAprovacaoColaborador(int idSite, int idUsuario, string tpEtapa)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                           && d.FlStatus == (int)StatusDocumento.Aprovacao
                                           && d.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == tpEtapa).Count() > 0
                                           && d.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == idUsuario).Count() > 0
                                           && d.Ativo == true);
        }

        public IEnumerable<DocDocumento> ListaDocumentosElaboracaoColaborador(int idSite, int idUsuario)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                              && d.FlStatus == (int)StatusDocumento.Elaboracao
                                              && d.IdElaborador == idUsuario
                                              && d.Ativo == true);
        }

        public IEnumerable<DocDocumento> ListaDocumentosVerificacaoColaborador(int idSite, int idUsuario, string tpEtapa)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                           && d.FlStatus == (int)StatusDocumento.Verificacao
                                           && d.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == tpEtapa).Count() > 0
                                           && d.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == idUsuario).Count() > 0
                                           && d.Ativo == true);
        }

        public IEnumerable<DocDocumento> ListaDocumentosEtapa(int idSite, int etapaDocumento)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                              && d.FlStatus == etapaDocumento
                                              && d.Ativo == true);
        }

        public IEnumerable<DocDocumento> ListaDocumentosStatusProcessoSite(int idSite, int flStatus, int idProcesso)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                              && (d.IdProcesso == idProcesso || d.IdProcesso == null)
                                              && d.FlStatus == flStatus);
        }

        public IEnumerable<DocDocumento> ListaDocumentosSite(int idSite)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite).ToList();
        }

        public DocDocumento DocumentoElaboracaoColaborador(int idSite, int idUsuario, int idDocumento)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                              && d.FlStatus == (int)StatusDocumento.Elaboracao
                                              && d.IdElaborador == idUsuario
                                              && d.IdDocumento == idDocumento
                                              && d.Ativo == true).FirstOrDefault();
        }

        public DocDocumento DocumentoVerificacaoColaborador(int idSite, int idUsuario, string tpEtapa, int idDocumento)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                           && d.FlStatus == (int)StatusDocumento.Verificacao
                                           && d.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == tpEtapa).Count() > 0
                                           && d.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == idUsuario).Count() > 0
                                           && d.IdDocumento == idDocumento
                                           && d.Ativo == true).FirstOrDefault();
        }

        public DocDocumento DocumentoAprovacaoColaborador(int idSite, int idUsuario, string tpEtapa, int idDocumento)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                           && d.FlStatus == (int)StatusDocumento.Aprovacao
                                           && d.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == tpEtapa).Count() > 0
                                           && d.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == idUsuario).Count() > 0
                                           && d.IdDocumento == idDocumento).FirstOrDefault();
        }

        public DocDocumento DocumentoEtapa(int idSite, int etapaDocumento, int idDocumento)
        {
            return Db.DocDocumento.Where(d => d.IdSite == idSite
                                              && d.FlStatus == etapaDocumento
                                              && d.IdDocumento == idDocumento).FirstOrDefault();
        }

        public DocDocumento DocumentoPorIdENuRevisao(int idDocumento, int numeroRevisao)
        {
            return Db.DocDocumento.Where(f => f.Ativo == true
                                           && f.IdDocumento == idDocumento
                                           && f.NuRevisao == numeroRevisao).FirstOrDefault();
        }

        public void RemoverGenerico(object obj)
        {
            Db.Entry(obj).State = EntityState.Deleted;
        }

        public void RemoverDocumento(DocDocumento obj)
        {
            obj.Ativo = false;

            Db.Entry(obj).State = EntityState.Modified;
            Db.SaveChanges();
        }

        public IEnumerable<DocDocumento> ListarAssuntosDoDocumentoERevisoes(int idDocumentoPai)
        {
            return Db.DocDocumento.Where(w => w.IdDocumentoPai == idDocumentoPai);
        }

        public void CriarDocumento(DocDocumento doc)
        {
            using (var context = new BaseContext())
            {
                if (doc.GestaoDeRisco != null)
                {
                    context.Set<RegistroConformidade>().Add(doc.GestaoDeRisco);
                            _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(doc.GestaoDeRisco);
                }

               
                context.Set<DocDocumento>().Add(doc);
                context.SaveChanges();
            }
        }
    }
}
