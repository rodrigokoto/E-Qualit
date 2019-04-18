using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Residuo
    {
        public string Processo;
        public string Revisao;
        public DateTime DataCriacao;
        public List<AtividadeResiduos> Atividades;
    }
}
