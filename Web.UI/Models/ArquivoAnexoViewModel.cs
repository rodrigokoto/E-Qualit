using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class ArquivoAnexoViewModel
    {
        //para o botao e a caixa de dialogo
        public string IdentificadorInicial { get; set; }
        public string TextoLink { get; set; }
        public int NumeroArquivos { get; set; }

        //para a ciaxa de dialog
        public string ModalTittle { get; set; }
        public List<ArquivoAnexoItemViewModel> AnexosLista { get; set; }
        public bool PodeAnexar;

    }

    public class ArquivoAnexoItemViewModel 
    {
        public int IdChave1;
        public int IdChave2;
        public Anexo AnexoDados;
    }

}