using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class DocUsuarioVerificaAprovaRepositorio : BaseRepositorio<DocUsuarioVerificaAprova>, IDocUsuarioVerificaAprovaRepositorio
    {
       public List<DocUsuarioVerificaAprova> VerificaDuplicidadeAprovador(DocDocumento docDocumento)
        {
            return Db.DocUsuarioVerificaAprova .Where(x => x.IdUsuario == docDocumento.IdUsuarioIncluiu &&
                                                             x.IdDocumento == docDocumento.IdDocumento &&
                                                             x.TpEtapa == "A").ToList();
        }

        public List<DocUsuarioVerificaAprova> VerificaDuplicidadeVerificador(DocDocumento docDocumento)
        {
            return Db.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == docDocumento.IdUsuarioIncluiu &&
                                                            x.IdDocumento == docDocumento.IdDocumento &&
                                                            x.TpEtapa == "V").ToList();
        }

        public void RemoveAllById(int id)
        {
            try
            {
                var usuarioVerificaAprova = Db.DocUsuarioVerificaAprova
                                    .Where(x => x.IdDocumento == id).ToList();//.FirstOrDefault();
                foreach (var item in usuarioVerificaAprova)
                {
                    Db.DocUsuarioVerificaAprova.Remove(item);
                    Db.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        

    }
}
