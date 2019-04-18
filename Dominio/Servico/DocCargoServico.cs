using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class DocCargoServico : IDocCargoServico
    {
        private readonly IDocCargoRepositorio _docCargoRepositorio;

        public DocCargoServico(IDocCargoRepositorio docCargoRepositorio) 
        {
            _docCargoRepositorio = docCargoRepositorio;
        }

        public void AlterarCargosDoDocumento(int idDocumento, List<DocumentoCargo> lista)
        {
            var listaDoBanco = _docCargoRepositorio.Get(x => x.IdDocumento == idDocumento).ToList();

            var entradaFiltrado = lista.Select(x => new { x.Id, x.IdCargo });

            var bancoFiltrado = listaDoBanco.Select(x => new { x.Id, x.IdCargo }).ToList();

            // O q veio da tela q eu nao tenho no banco
            var paraIncluir = entradaFiltrado.Except(bancoFiltrado);

            // O q nao veio da tela mas eu tenho no banco
            var paraDeletar = bancoFiltrado.Except(entradaFiltrado);

            List<DocumentoCargo> listaDel = new List<DocumentoCargo>();

            foreach (var del in paraDeletar)
            {
                listaDel.Add(listaDoBanco.Where(x => x.Id == del.Id
                                && x.IdCargo == del.IdCargo).FirstOrDefault());
            }

            foreach (var cargoDel in listaDel)
            {
                _docCargoRepositorio.Remove(cargoDel);
                lista.Remove(cargoDel);
            }

            foreach (var cargoIncluir in paraIncluir)
            {
                _docCargoRepositorio.Add(lista.Where(x => cargoIncluir.IdCargo == x.IdCargo
                                                        && x.Id == cargoIncluir.Id).FirstOrDefault());
            }
        }
    }
}
