namespace Dominio.Entidade
{
    public partial class Telefone
    {
        public int IdTelefone { get; set; }
        public int IdRelacionado { get; set; }
        public string FlTipoTelefone { get; set; }
        public string NuTelefone { get; set; }
        public string NuRamal { get; set; }
        public string DsObservacao { get; set; }
    }
}
