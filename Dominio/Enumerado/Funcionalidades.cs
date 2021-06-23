using System.ComponentModel;

namespace Dominio.Enumerado
{
    public enum Funcionalidades
    {
        [Description("Administrativo")]
        Administrativo = 1,
        [Description("Control Doc")]
        ControlDoc = 2,
        [Description("Não Conformidade")]
        NaoConformidade = 3,
        [Description("Ação Corretiva")]
        AcaoCorretiva = 4,
        [Description("Indicadores")]
        Indicadores = 5,
        [Description("Auditoria")]
        Auditoria = 6,
        [Description("Analise Critica")]
        AnaliseCritica = 7,
        [Description("Licenças")]
        Licencas = 8,
        [Description("Intrumento")]
        Instrumentos = 9,
        [Description("Fornecedor")]
        Fornecedores = 10,
        [Description("Gestão de Risco")]
        GestaoDeRiscos = 11,
        [Description("Recursos Humanos")]
        RecursosHumanos = 12,
        [Description("Doc")]
        Docs = 13,
        [Description("Usuário")]
        Usuario = 14,
    }
}
