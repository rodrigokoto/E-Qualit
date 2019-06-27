using Dominio.Entidade;

namespace Dominio.Interface.Servico
{
    public interface IRegistroAuditoriaServico
    {
        void InserirEmail(RegistroAuditoria regAudit);
        void AtualizaEmail(RegistroAuditoria regAudit);
        void ExcluiEmail(RegistroAuditoria regAudit);
        RegistroAuditoria RetornaRegistro(Plai idplai);
        
    }
}
