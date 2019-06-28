using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Servico
{
    public class RegistroAuditoriaServico : IRegistroAuditoriaServico
    {
        private readonly IRegistroAuditoria _registroAuditoria;

        public RegistroAuditoriaServico(IRegistroAuditoria registroAuditoria) {
            _registroAuditoria = registroAuditoria;
        }

        public void AtualizaEmail(RegistroAuditoria regAudit)
        {
            _registroAuditoria.Update(regAudit);
        }

        public void ExcluiEmail(RegistroAuditoria regAudit)
        {
            _registroAuditoria.Remove(regAudit);
        }

        public void InserirEmail(RegistroAuditoria regAudit)
        {
            _registroAuditoria.Add(regAudit);
        }

        public RegistroAuditoria RetornaRegistro(Plai plai)
        {

            return _registroAuditoria.GetAll().Where(x => x.IdPlai == plai.IdPlai).FirstOrDefault();

        }
    }
}
