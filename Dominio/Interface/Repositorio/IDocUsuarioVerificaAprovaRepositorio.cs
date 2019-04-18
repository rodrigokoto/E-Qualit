using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IDocUsuarioVerificaAprovaRepositorio : IBaseRepositorio<DocUsuarioVerificaAprova>
    {
        //TODO Refatorar, pois está e a maneira errada
        List<DocUsuarioVerificaAprova> VerificaDuplicidadeAprovador(DocDocumento docDocumento);

        List<DocUsuarioVerificaAprova> VerificaDuplicidadeVerificador(DocDocumento docDocumento);
    }

}
