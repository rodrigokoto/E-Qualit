using Dominio.Entidade;

namespace TestProject.Extentions
{
    public static class FornecedorExtention
    {
        public static Fornecedor Criar(this Fornecedor fornecedor)
        {
            fornecedor = new Fornecedor
            {
                Nome = "Vans",
                Telefone = "11959390757",
                Email = "aciolecarmo@gmail.com",
                IdProcesso = 1,
                IdSite = 1,
            };

            return fornecedor;
        }

        public static Fornecedor First(this Fornecedor fornecedor)
        {
            fornecedor = new Fornecedor
            {
                IdFornecedor = 1,
                Nome = "Vans",
                Telefone = "11959390757",
                Email = "aciolecarmo@gmail.com",
                IdProcesso = 1,
                IdSite = 5,
            };

            return fornecedor;
        }

    }
}
