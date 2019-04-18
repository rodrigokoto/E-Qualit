using Dominio.Entidade;
using Dominio.Interface.Repositorio;
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
    }
}
