using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Web.UI.Helpers
{
    public static class Cultura
    {
        public const string NOME_COOKIE_IDIOMA = "__Equalit_Idioma__";
        public static void AlterarTradutor(string nomeCultura)
        {
            //grava idioma no cookie
            HttpCookie cookie = new HttpCookie(NOME_COOKIE_IDIOMA);
            cookie.Value = nomeCultura;

            //configura a data de expiração para 180 dias
            DateTime dtNow = DateTime.Now;
            TimeSpan tsMinute = new TimeSpan(180, 0, 0, 0);

            cookie.Expires = dtNow + tsMinute;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCultura()
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[NOME_COOKIE_IDIOMA];

            string nomeCultura = string.Empty;

            if (cookie != null && !String.IsNullOrEmpty(cookie.Value))
                nomeCultura = cookie.Value;
            else
            {
                nomeCultura = "pt-BR";
                AlterarTradutor(nomeCultura); //Grava o cookie com a cultura default
            }

            //Configura a thread corrente com a cultura
            Traducao.GlobalCulture.SetCulture(nomeCultura);

            return nomeCultura;
        }

        public static List<sIdioma> ListaIdiomas()
        {
            string configuracao = ConfigurationManager.AppSettings["Language"];
            if (String.IsNullOrEmpty(configuracao))
                return null;

            List<sIdioma> retorno = new List<sIdioma>();
            string[] idiomas = configuracao.Split(';');
            foreach (string idioma in idiomas)
            {
                sIdioma item = new sIdioma();
                item.Nome = idioma.Split('|')[0];
                item.Cultura = idioma.Split('|')[1];
                retorno.Add(item);
            }

            return retorno;
        }

        public struct sIdioma
        {
            private string _cultura;
            public string Cultura
            {
                get
                {
                    return _cultura;
                }
                set
                {
                    _cultura = value;
                }
            }

            private string _nome;
            public string Nome
            {
                get
                {
                    return _nome;
                }
                set
                {
                    _nome = value;
                }
            }
        }
    }
}