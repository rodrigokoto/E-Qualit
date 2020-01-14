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
    public class RegistroQualificacaoServico : IRegistroQualificacaoServico
    {

        private readonly IRegistroQualificacaoRepositorio _registroQualificacao;
        public RegistroQualificacaoServico(IRegistroQualificacaoRepositorio registroQualificacao) {
            _registroQualificacao = registroQualificacao;
        }

        public void AtualizaEmail(RegistroQualificacao regQuali)
        {
            _registroQualificacao.Update(regQuali);
        }

        public void ExcluiEmail(RegistroQualificacao regQuali)
        {
            _registroQualificacao.Remove(regQuali);
        }

        public void InserirEmail(RegistroQualificacao regQuali)
        {
            _registroQualificacao.Add(regQuali);
        }

        public RegistroQualificacao RetornaRegistro(string guidAvaliacao)
        {
            return _registroQualificacao.Get(x => x.GuidAvaliacao == guidAvaliacao).FirstOrDefault();
           
        }
    }
}
