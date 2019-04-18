using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class DocTemplateServico : IDocTemplateServico
    {
        private IDocTemplateRepositorio _docTemplateRepositorio;

        public DocTemplateServico(IDocTemplateRepositorio docTemplateRepositorio) 
        {
            _docTemplateRepositorio = docTemplateRepositorio;
        }

        public void AlterarTemplatesDocumento(int idDocumento, List<DocTemplate> lista)
        {
            var listaDoBanco = _docTemplateRepositorio.Get(x => x.IdDocumento == idDocumento).ToList();

            var entradaFiltrado = lista.Select(x => new { x.IdDocTemplate, x.IdDocumento });

            var bancoFiltrado = listaDoBanco.Select(x => new { x.IdDocTemplate, x.IdDocumento }).ToList();

            // O q veio da tela q eu nao tenho no banco
            var paraIncluir = entradaFiltrado.Except(bancoFiltrado);

            // O q nao veio da tela mas eu tenho no banco
            var paraDeletar = bancoFiltrado.Except(entradaFiltrado);

            List<DocTemplate> listaDel = new List<DocTemplate>();

            foreach (var del in paraDeletar)
            {
                listaDel.Add(listaDoBanco.Where(x => x.IdDocTemplate == del.IdDocTemplate
                                && x.IdDocumento == del.IdDocumento).FirstOrDefault());
            }

            foreach (var templateDel in listaDel)
            {
                _docTemplateRepositorio.Remove(templateDel);
                lista.Remove(templateDel);
            }

            foreach (var templateIncluir in paraIncluir)
            {
                _docTemplateRepositorio.Add(lista.Where(x => templateIncluir.IdDocTemplate == x.IdDocTemplate
                                                        && x.IdDocumento == templateIncluir.IdDocumento).FirstOrDefault());
            }
        }
    }
}
