namespace Dominio.Entidade
{
    public partial class Endereco 
    {
        public int IdEndereco { get; set; }
        public int IdRelacionado { get; set; }
        public string FlTipoEndereco { get; set; }
        public string DsLogradouro { get; set; }
        public string NuNumero { get; set; }
        public string DsComplemento { get; set; }
        public string NmBairro { get; set; }
        public string NmCidade { get; set; }
        public string CdEstado { get; set; }
        public string NuCep { get; set; }
        public string DsPais { get; set; }
    }
}
