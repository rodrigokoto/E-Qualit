using DomainValidation.Interfaces.Validation;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;

namespace Dominio.Entidade
{
    public class Cliente : ISelfValidator
    {
        public Cliente()
        {
            UsuarioClienteSites = new List<UsuarioClienteSite>();
            ClienteLogo = new List<ClienteLogo>();
            Contratos = new List<ClienteContrato>();
            ContratosAux = new List<Anexo>();
        }

        public int IdCliente { get; set; }

        public string NmFantasia { get; set; }
        
        public string NmUrlAcesso { get; set; }

        public DateTime? DtValidadeContrato { get; set; }

        public int? NuDiasTrocaSenha { get; set; }

        public int? NuTentativaBloqueioLogin { get; set; }

        public int? NuArmazenaSenha { get; set; }

        public bool FlExigeSenhaForte { get; set; }

        public bool FlTemCaptcha { get; set; }

        public bool? FlEstruturaAprovador { get; set; }

        public bool FlAtivo { get; set; }

        public int? IdUsuarioIncluiu { get; set; }

        public DateTime? DtInclusao { get; set; }

        public virtual Site Site { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<UsuarioClienteSite> UsuarioClienteSites { get; set; }


        public Anexo ClienteLogoAux { get; set; }

        public virtual ICollection<ClienteLogo> ClienteLogo { get; set; }

        public List<Anexo> ContratosAux { get; set; }

        public virtual ICollection<ClienteContrato> Contratos { get; set; }


        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
