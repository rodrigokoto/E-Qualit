using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ILicencaServico
    {
        void Valido(Licenca licenca, ref List<string> erros);
        //bool DeletarInstrumentoEDependencias(int id);
        //List<Instrumento> ObterPorIdSite(int idSite);
        //decimal GeraProximoNumeroRegistro(int idSite, int? idProcesso = null, int? idSigla = null);
    }
}
