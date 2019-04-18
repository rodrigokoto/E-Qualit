using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class PlaiProcessoNormaServico : IPlaiProcessoNormaServico
    {
        private readonly IPlaiProcessoNormaRepositorio _plaiProcessoNormaRepositorio;

        public PlaiProcessoNormaServico(IPlaiProcessoNormaRepositorio plaiProcessoNormaRepositorio) 
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
