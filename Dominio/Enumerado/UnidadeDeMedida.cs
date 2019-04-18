using System.ComponentModel;

namespace Dominio.Enumerado
{
    public enum UnidadeDeMedida
    {
        [Description("Porcentagem")]
        Porcentagem = 1,
        [Description("Comprimento")]
        Comprimento = 2,
        [Description("Massa")]
        Massa = 3,
        [Description("Area")]
        Area = 4,
        [Description("Volume")]
        Volume = 4,
    }
}
