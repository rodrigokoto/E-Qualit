using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class FuncionalidadeAppServico : BaseServico<Funcionalidade>, IFuncionalidadeAppServico
    {
        private readonly IFuncionalidadeRepositorio _moduloRepositorio;

        public FuncionalidadeAppServico(IFuncionalidadeRepositorio moduloRepositorio) : base(moduloRepositorio)
        {
            _moduloRepositorio = moduloRepositorio;
        }

        public List<Funcionalidade> CriarFuncionalidadesPorSiteModulos(List<SiteFuncionalidade> sites)
        {
            var modulos = new List<Funcionalidade>();

            var modulosNaoDuplicados = sites.Select(x => new
            {
                x.Funcionalidade.IdFuncionalidade,
                x.Funcionalidade.Nome
            }).Distinct();

            foreach (var moduloNaoDuplicados in modulosNaoDuplicados)
            {
                modulos.Add(new Funcionalidade
                {
                    IdFuncionalidade = moduloNaoDuplicados.IdFuncionalidade,
                    Nome = moduloNaoDuplicados.Nome
                });
            }

            return  modulos;
        }
    }
}
