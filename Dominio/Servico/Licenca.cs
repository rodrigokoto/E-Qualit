using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Instrumentos;
using Dominio.Validacao.Instrumentos.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class LicencaServico : ILicencaServico
    {
        public LicencaServico()
        {
        }

        public void Valido(Licenca licenca, ref List<string> erros)
        {
            ValidaRegraTela(licenca, ref erros);
        }

        private void ValidaRegraTela(Licenca licenca, ref List<string> erros)
        {

            var camposObrigatrios = new CriarLicencaViewValidation().Validate(licenca);
                

            if (!camposObrigatrios.IsValid)
                erros.AddRange(UtilsServico.PopularErros(camposObrigatrios.Errors));
        }
    }
}
