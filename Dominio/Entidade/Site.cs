using DomainValidation.Interfaces.Validation;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;

namespace Dominio.Entidade
{
    public class Site : ISelfValidator
    {
        public Site()
        {
            ControladorCategorias = new List<ControladorCategoria>();
            Cargos = new List<Cargo>();
            Processos = new List<Processo>();
            Registros = new List<RegistroConformidade>();
            SiteFuncionalidades = new List<SiteFuncionalidade>();
            UsuarioClienteSites = new List<UsuarioClienteSite>();
            SiteAnexo = new List<SiteAnexo>();
        }

        public int IdSite { get; set; }
        public int IdCliente { get; set; }
        public string NmFantasia { get; set; }
        public string NmRazaoSocial { get; set; }
        public string NuCNPJ { get; set; }
        public string DsObservacoes { get; set; }
        public string DsFrase { get; set; }
        public bool FlAtivo { get; set; }
        public int? IdUsuarioIncluiu { get; set; }
        public DateTime? DtInclusao { get; set; }
        public Anexo SiteLogoAux { get; set; }

        public virtual ICollection<SiteAnexo> SiteAnexo { get; set; }
        public virtual ICollection<ControladorCategoria> ControladorCategorias { get; set; }
        public virtual ICollection<Cargo> Cargos { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<Processo> Processos { get; set; }
        public virtual ICollection<RegistroConformidade> Registros { get; set; }
        public virtual ICollection<SiteFuncionalidade> SiteFuncionalidades { get; set; }
        public virtual ICollection<UsuarioClienteSite> UsuarioClienteSites { get; set; }
        public virtual ICollection<NotificacaoMensagem> NotificacaoMensagens { get; set; }
        public virtual ICollection<Notificacao> Notificacoes { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; }
        public virtual ICollection<Fornecedor> Fornecedores { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
