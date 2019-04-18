using Dominio.Entidade;
using System;

namespace TestProject.Extentions
{
    public static class ProcessoExtention
    {
        public static Processo First(this Processo processo)
        {
            processo = new Processo
            {
                IdProcesso = 1,
                IdSite = 1,
                IdUsuarioIncluiu = 1,
                DataCadastro = DateTime.Now,
                DataAlteracao = DateTime.Now,
                Atividade = "IT",
                Nome = "Desenvolvimento",
                FlAtivo = true,
                FlQualidade = true,
            };
            return processo;
        }
    }
}
