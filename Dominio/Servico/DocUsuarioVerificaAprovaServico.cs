using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dominio.Servico
{
    public class DocUsuarioVerificaAprovaServico : IDocUsuarioVerificaAprovaServico
    {
        private readonly IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprovaRepositorio;

        public DocUsuarioVerificaAprovaServico(IDocUsuarioVerificaAprovaRepositorio docUsuarioVerificaAprovaRepositorio) 
        {
            _docUsuarioVerificaAprovaRepositorio = docUsuarioVerificaAprovaRepositorio;
        }

        public void Add(DocUsuarioVerificaAprova obj)
        {
            throw new NotImplementedException();
        }

        public void AlteraEstado(DocUsuarioVerificaAprova obj, EstadoObjetoEF estado)
        {
            throw new NotImplementedException();
        }

        public void AlterarUsuariosDoDocumento(int idDocumento, List<DocUsuarioVerificaAprova> lista)
        {
            var listaDoBanco = _docUsuarioVerificaAprovaRepositorio.Get(x => x.IdDocumento == idDocumento).ToList();

            var entradaFiltrado = lista.Select(x => new { x.IdDocUsuarioVerificaAprova, x.IdUsuario });

            var bancoFiltrado = listaDoBanco.Select(x => new { x.IdDocUsuarioVerificaAprova, x.IdUsuario }).ToList();

            // O q veio da tela q eu nao tenho no banco
            var paraIncluir = entradaFiltrado.Except(bancoFiltrado);

            // O q nao veio da tela mas eu tenho no banco
            var paraDeletar = bancoFiltrado.Except(entradaFiltrado);

            List<DocUsuarioVerificaAprova> listaDel = new List<DocUsuarioVerificaAprova>();

            foreach (var del in paraDeletar)
            {
                listaDel.Add(listaDoBanco.Where(x => x.IdUsuario == del.IdUsuario
                                && x.IdDocUsuarioVerificaAprova == del.IdDocUsuarioVerificaAprova).FirstOrDefault());
            }

            foreach (var usDeletar in listaDel)
            {
                _docUsuarioVerificaAprovaRepositorio.Remove(usDeletar);
                lista.Remove(usDeletar);
            }

            List<DocUsuarioVerificaAprova> listaIncluir = new List<DocUsuarioVerificaAprova>();

            foreach (var usIncluir in paraIncluir)
            {
                _docUsuarioVerificaAprovaRepositorio.Add(lista.Where(x => usIncluir.IdUsuario == x.IdUsuario
                                                        && x.IdDocUsuarioVerificaAprova == usIncluir.IdDocUsuarioVerificaAprova).FirstOrDefault());
            }


            //if (paraDeletar.Count() > 0)
            //{
            //    return listaDoBanco;
            //}

            //return lista;
        }

        public void AtualizarParaEstadoInicialDoDocumento(List<DocUsuarioVerificaAprova> lista)
        {
            lista.ForEach(x => { x.FlVerificou = false; x.FlAprovou = false; });

            foreach (var usuario in lista)
                _docUsuarioVerificaAprovaRepositorio.Update(usuario);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DocUsuarioVerificaAprova> Get(Expression<Func<DocUsuarioVerificaAprova, bool>> filter = null, Func<IQueryable<DocUsuarioVerificaAprova>, IOrderedQueryable<DocUsuarioVerificaAprova>> orderBy = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DocUsuarioVerificaAprova> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DocUsuarioVerificaAprova> GetAllAsNoTracking()
        {
            throw new NotImplementedException();
        }

        public DocUsuarioVerificaAprova GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(DocUsuarioVerificaAprova obj)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllById(int id)
        {
            _docUsuarioVerificaAprovaRepositorio.RemoveAllById(id);

        }

        public void Update(DocUsuarioVerificaAprova obj)
        {
            throw new NotImplementedException();
        }
    }
}
