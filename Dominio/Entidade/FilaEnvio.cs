using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class FilaEnvio
    {
        public long Id { get; set; }
        public string Assunto { get; set; }
        public string Destinatario { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAgendado { get; set; }
        public DateTime? DataEnviado { get; set; }
        public bool Enviado { get; set; }
    }
}
