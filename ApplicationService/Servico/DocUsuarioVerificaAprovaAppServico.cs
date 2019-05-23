using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class DocUsuarioVerificaAprovaAppServico : BaseServico<DocUsuarioVerificaAprova>, IDocUsuarioVerificaAprovaAppServico
    {
        private readonly IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprovaRepositorio;

        public DocUsuarioVerificaAprovaAppServico(IDocUsuarioVerificaAprovaRepositorio docUsuarioVerificaAprovaRepositorio) : base(docUsuarioVerificaAprovaRepositorio)
        {
            _docUsuarioVerificaAprovaRepositorio = docUsuarioVerificaAprovaRepositorio;
        }

        public void AlterarUsuariosDoDocumento(int idDocumento, List<DocUsuarioVerificaAprova> lista)
        {
            var listaDoBanco = _docUsuarioVerificaAprovaRepositorio.Get(x => x.IdDocumento == idDocumento).ToList();

            foreach (var usDeletar in listaDoBanco)
            {
                _docUsuarioVerificaAprovaRepositorio.Remove(usDeletar);
                lista.Remove(usDeletar);
            }

            
            List<DocUsuarioVerificaAprova> listaIncluir = new List<DocUsuarioVerificaAprova>();

            foreach (var usIncluir in lista)
            {
                _docUsuarioVerificaAprovaRepositorio.Add(lista.Where(x => usIncluir.IdUsuario == x.IdUsuario
                                                        && x.IdDocUsuarioVerificaAprova == usIncluir.IdDocUsuarioVerificaAprova).FirstOrDefault());
            }

        }

        public void AtualizarParaEstadoInicialDoDocumento(List<DocUsuarioVerificaAprova> lista)
        {
            lista.ForEach(x => { x.FlVerificou = false; x.FlAprovou = false; });

            foreach (var usuario in lista)
                _docUsuarioVerificaAprovaRepositorio.Update(usuario);
        }
    }
}
