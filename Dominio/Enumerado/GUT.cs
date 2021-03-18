using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Enumerado
{

    public enum Gravidade
    {
        [Description("Sem Gravidade")]
        SemGravidade = 1,
        [Description("Pouco Grave")]
        PoucoGrave = 2,
        [Description("Grave")]
        Grave = 3,
        [Description("Muito Grave")]
        MuitoGrave = 4,
        [Description("Extremamente Grave")]
        ExtremamenteGrave = 5
    }
    public enum Urgencia
    {
        [Description("Sem Urgência")]
        SemUrgência = 1,
        [Description("Pouco Urgente")]
        PoucoUrgente = 2,
        [Description("Urgente")]
        Urgente = 3,
        [Description("Muito Urgente")]
        MuitoUrgente = 4,
        [Description("Extremamente Urgente")]
        ExtremamenteUrgente = 5
    }
    public enum Tendencia
    {
        [Description("Permanece")]
        Permanece = 1,
        [Description("Piora longo prazo")]
        PioralongoPrazo = 2,
        [Description(" Piora médio prazo")]
        Pioramedioprazo = 3,
        [Description(" Piora curto prazo")]
        Pioracurtoprazo = 4,
        [Description("Piora rapidamente")]
        Piorarapidamente = 5

    }
}
