using Dominio.Entidade;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class DocumentoComentarioAppServico : BaseServico<DocumentoComentario>, IDocumentoComentarioAppServico
    {
        private IDocumentoComentarioRepositorio _documentoComentariorepositorio;

        public DocumentoComentarioAppServico(IDocumentoComentarioRepositorio documentoComentariorepositorio) : base(documentoComentariorepositorio)
        {
            _documentoComentariorepositorio = documentoComentariorepositorio;
        }

        public void AlterarCOmentariosDocumento(int idDocumento, List<DocumentoComentario> lista)
        {
            var listaDoBanco = _documentoComentariorepositorio.Get(x => x.IdDocumento == idDocumento).ToList();

            var entradaFiltrado = lista.Select(x => new { x.Id, x.IdDocumento });

            var bancoFiltrado = listaDoBanco.Select(x => new { x.Id, x.IdDocumento }).ToList();

            // O q veio da tela q eu nao tenho no banco
            var paraIncluir = entradaFiltrado.Except(bancoFiltrado);

            // O q nao veio da tela mas eu tenho no banco
            var paraDeletar = bancoFiltrado.Except(entradaFiltrado);

            List<DocumentoComentario> listaDel = new List<DocumentoComentario>();

            foreach (var del in paraDeletar)
            {
                listaDel.Add(listaDoBanco.Where(x => x.Id == del.Id
                                && x.IdDocumento == del.IdDocumento).FirstOrDefault());
            }

            foreach (var comentario in listaDel)
            {
                _documentoComentariorepositorio.Remove(comentario);
                lista.Remove(comentario);
            }

            foreach (var comentarioInclusao in paraIncluir)
            {
                _documentoComentariorepositorio.Add(lista.Where(x => comentarioInclusao.Id == x.Id
                                                        && x.IdDocumento == comentarioInclusao.IdDocumento).FirstOrDefault());
            }
        }
    }
}
