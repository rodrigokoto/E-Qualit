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

            //var entradaFiltrado = lista.Select(x => new { x.IdDocUsuarioVerificaAprova, x.IdUsuario });

            //var bancoFiltrado = listaDoBanco.Select(x => new { x.IdDocUsuarioVerificaAprova, x.IdUsuario }).ToList();

            //// O q veio da tela q eu nao tenho no banco
            //var paraIncluir = entradaFiltrado.Except(bancoFiltrado);

            //// O q nao veio da tela mas eu tenho no banco
            //var paraDeletar = bancoFiltrado.Except(entradaFiltrado);

            //List<DocUsuarioVerificaAprova> listaDel = new List<DocUsuarioVerificaAprova>();

            //foreach (var del in paraDeletar)
            //{
            //    listaDel.Add(listaDoBanco.Where(x => x.IdUsuario == del.IdUsuario
            //                    && x.IdDocUsuarioVerificaAprova == del.IdDocUsuarioVerificaAprova).FirstOrDefault());
            //}

            //foreach (var usDeletar in listaDel)
            //{
            //    _docUsuarioVerificaAprovaRepositorio.Remove(usDeletar);
            //    lista.Remove(usDeletar);
            //}

            foreach (var usDeletar in listaDoBanco)
            {
                _docUsuarioVerificaAprovaRepositorio.Remove(usDeletar);
                lista.Remove(usDeletar);
            }

            //[aquii]
            List<DocUsuarioVerificaAprova> listaIncluir = new List<DocUsuarioVerificaAprova>();

            //foreach (var usIncluir in paraIncluir)
            //{
            //    _docUsuarioVerificaAprovaRepositorio.Add(lista.Where(x => usIncluir.IdUsuario == x.IdUsuario
            //                                            && x.IdDocUsuarioVerificaAprova == usIncluir.IdDocUsuarioVerificaAprova).FirstOrDefault());
            //}

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
