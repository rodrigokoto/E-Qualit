﻿using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interface.Servico
{
    public interface IFilaEnvioServico
    {
        void Enfileirar(FilaEnvio filaEnvio);
    }
}
