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

        public void AlterarUsuariosDoDocumento(List<DocUsuarioVerificaAprova> lista)
        {            
            foreach (var usuarioVerificaAprova in lista)
            {
                _docUsuarioVerificaAprovaRepositorio.Update(usuarioVerificaAprova);
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
