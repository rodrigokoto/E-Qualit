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
    public class RegistroLicencaServico : IRegistroLicencaServico
    {

        private readonly IRegistroLicencaRepositorio _registroLicenca;
        public RegistroLicencaServico(IRegistroLicencaRepositorio registroLicenca)
        {
            _registroLicenca = registroLicenca;
        }

        public void AtualizaEmail(RegistroLicenca regLicenca)
        {
            _registroLicenca.Update(regLicenca);
        }

        public void ExcluiEmail(RegistroLicenca regLicenca)
        {
            _registroLicenca.Remove(regLicenca);
        }

        public void InserirEmail(RegistroLicenca regLicenca)
        {
            _registroLicenca.Add(regLicenca);
        }

        public RegistroLicenca RetornaRegistro(string guidLicenca)
        {
            return _registroLicenca.Get(x => x.GuidLicenca == guidLicenca).FirstOrDefault();

        }
    }
}
