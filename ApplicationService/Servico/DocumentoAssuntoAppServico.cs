using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ApplicationService.Servico
{
    public class DocumentoAssuntoAppServico : BaseServico<DocumentoAssunto>, IDocumentoAssuntoAppServico
    {

        private IDocumentoAssuntoRepositorio _documentoAssuntoRepositorio;

        public DocumentoAssuntoAppServico(IDocumentoAssuntoRepositorio documentoAssuntoRepositorio) : base(documentoAssuntoRepositorio)
        {
            _documentoAssuntoRepositorio = documentoAssuntoRepositorio;
        }

        public void AlterarAssuntosDocumento(int idDocumento, List<DocumentoAssunto> lista)
        {
            if (lista != null)
            {
                var listaDoBanco = _documentoAssuntoRepositorio.Get(x => x.IdDocumento == idDocumento).ToList();

                var entradaFiltrado = lista.Select(x => x.Id);

                var bancoFiltrado = listaDoBanco.Select(x => x.Id)
                                    .ToList();

                // O q veio da tela q eu nao tenho no banco
                var paraIncluir = entradaFiltrado.Except(bancoFiltrado);

                // O q nao veio da tela mas eu tenho no banco
                var paraDeletar = bancoFiltrado.Except(entradaFiltrado);

                List<DocumentoAssunto> listaDel = new List<DocumentoAssunto>();

                foreach (var del in paraDeletar)
                {
                    listaDel.Add(listaDoBanco.Where(x => x.Id == del)
                        .FirstOrDefault());
                }

                foreach (var templateDel in listaDel)
                {
                    _documentoAssuntoRepositorio.Remove(templateDel);
                    lista.Remove(templateDel);
                }

                foreach (var templateIncluir in paraIncluir)
                {
                    var assuntoIncluir = lista.Where(x => templateIncluir == x.Id).FirstOrDefault();
                    assuntoIncluir.DataAssunto = DateTime.Now;      
                    
                    _documentoAssuntoRepositorio.Add(assuntoIncluir);                    
                }

                foreach (var templateAtualizar in lista)
                {
                    var objAtualizar = _documentoAssuntoRepositorio.GetById(templateAtualizar.Id);
                    objAtualizar.Descricao = templateAtualizar.Descricao;
                    _documentoAssuntoRepositorio.Update(objAtualizar);
                }

            }
        }
    }
}
