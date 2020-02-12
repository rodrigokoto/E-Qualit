using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface ILicencaAppServico : IBaseServico<Licenca>
    {

        void SalvarArquivoLicenca(Licenca licenca);
    }
}
