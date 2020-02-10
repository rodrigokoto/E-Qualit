using Dominio.Entidade;

namespace Dominio.Interface.Servico
{
    public interface IRegistroLicencaServico
    {
        void InserirEmail(RegistroLicenca regLicenca);
        void AtualizaEmail(RegistroLicenca regLicenca);
        void ExcluiEmail(RegistroLicenca regLicenca);
        RegistroLicenca RetornaRegistro(int Licenca);

       
        
    }
}
