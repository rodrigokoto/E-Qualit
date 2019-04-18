using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class PlaiProcessoNormaAppServico : BaseServico<PlaiProcessoNorma>, IPlaiProcessoNormaAppServico
    {
        private readonly IPlaiProcessoNormaRepositorio _plaiProcessoNormaRepositorio;

        public PlaiProcessoNormaAppServico(IPlaiProcessoNormaRepositorio plaiProcessoNormaRepositorio) : base(plaiProcessoNormaRepositorio)
        {
            _plaiProcessoNormaRepositorio = plaiProcessoNormaRepositorio;
        }

        public List<PlaiProcessoNorma> ObterPorIdPlai(int idPlai)
        {
            try
            {
                var processosNormas =  _plaiProcessoNormaRepositorio.Get(x => x.IdPlai == idPlai).ToList();

                CheckCampoComoAtivo(ref processosNormas);

                return processosNormas;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CheckCampoComoAtivo(ref List<PlaiProcessoNorma> plaiProcessosNormas)
        {
            plaiProcessosNormas.ForEach(x => x.Ativo = true);
        }
    }
}
