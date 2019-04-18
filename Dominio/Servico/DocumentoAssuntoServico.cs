using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class DocumentoAssuntoServico : IDocumentoAssuntoServico
    {

        private IDocumentoAssuntoRepositorio _documentoAssuntoRepositorio;

        public DocumentoAssuntoServico(IDocumentoAssuntoRepositorio documentoAssuntoRepositorio) 
        {
            _documentoAssuntoRepositorio = documentoAssuntoRepositorio;
        }

        public void AlterarAssuntosDocumento(int idDocumento, List<DocumentoAssunto> lista)
        {
            if (lista != null)
            {
                var listaDoBanco = _documentoAssuntoRepositorio.Get(x => x.IdDocumento == idDocumento).ToList();

                var entradaFiltrado = lista.Select(x => new { x.Id, x.IdDocumento });

                var bancoFiltrado = listaDoBanco.Select(x => new { x.Id, x.IdDocumento }).ToList();

                // O q veio da tela q eu nao tenho no banco
                var paraIncluir = entradaFiltrado.Except(bancoFiltrado);

                // O q nao veio da tela mas eu tenho no banco
                var paraDeletar = bancoFiltrado.Except(entradaFiltrado);

                List<DocumentoAssunto> listaDel = new List<DocumentoAssunto>();

                foreach (var del in paraDeletar)
                {
                    listaDel.Add(listaDoBanco.Where(x => x.Id == del.Id
                                    && x.IdDocumento == del.IdDocumento).FirstOrDefault());
                }

                foreach (var templateDel in listaDel)
                {
                    _documentoAssuntoRepositorio.Remove(templateDel);
                    lista.Remove(templateDel);
                }

                foreach (var templateIncluir in paraIncluir)
                {
                    _documentoAssuntoRepositorio.Add(lista.Where(x => templateIncluir.Id == x.Id
                                                            && x.IdDocumento == templateIncluir.IdDocumento).FirstOrDefault());
                } 
            }
        }
    }
}
