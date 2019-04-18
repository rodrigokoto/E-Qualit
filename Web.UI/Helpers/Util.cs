using ApplicationService.Entidade;
using Dominio.Servico;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml.Serialization;

namespace Web.UI.Helpers
{
    public static class Util
    {
        private static string NOME_COOKIE_IDIOMA = ConfigurationManager.AppSettings["CookieIdioma"];

        public static int ObterPerfilUsuarioLogado()
        {
            var meuPerfil = HttpContext.Current.Request.Cookies["meuPerfil"];
            var idPerfil = 0;

            if (meuPerfil != null)
            {
                int.TryParse(UtilsServico.DesCriptografarString(meuPerfil.Value), out idPerfil);
            }

            return idPerfil;
        }

        public static int ObterProcessoSelecionado()
        {
            var idProcesso = 0;

            var processoSelecionado = HttpContext.Current.Request.Cookies["processoSelecionadoCodigo"];

            if (processoSelecionado != null)
            {
                int.TryParse(UtilsServico.DesCriptografarString(processoSelecionado.Value), out idProcesso);

            }


            return idProcesso;
        }

        public static int SetProcessoSelecionado(int IdProcesso, string nomeProcesso)
        {
            try
            {
                string idProcesso = UtilsServico.CriptografarString(IdProcesso.ToString());

                var cookieCodigo = new HttpCookie("processoSelecionadoCodigo", idProcesso)
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                var cookieNome = new HttpCookie("processoSelecionadoNome", nomeProcesso)
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                HttpContext.Current.Response.Cookies.Add(cookieCodigo);
                HttpContext.Current.Response.Cookies.Add(cookieNome);

            }
            catch (Exception ex)
            {
                
            }

            return IdProcesso;
        }

        public static int ObterSiteSelecionado()
        {
            int idSite = 0;
            var siteSelecionado = HttpContext.Current.Request.Cookies["siteSelecionado"];
            if (siteSelecionado.Value != string.Empty)
            {

                int.TryParse(siteSelecionado.Value, out idSite);
            }


            return idSite;
        }

        public static int ObterClienteSelecionado()
        {
            var clienteSelecionado = HttpContext.Current.Request.Cookies["clienteSelecionado"];

            return Convert.ToInt32(clienteSelecionado.Value);
        }



        public static int ObterCodigoUsuarioLogado()
        {
            var idUsuario = 0;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            var usuarioCookie = HttpContext.Current.Request.Cookies["usuario"];

            if (usuarioCookie != null)
            {
                var objUsuario = JsonConvert.DeserializeObject<UsuarioApp>(usuarioCookie.Value);
                objUsuario.IdUsuario = UtilsServico.DesCriptografarString(objUsuario.IdUsuario);
                return Convert.ToInt32(objUsuario.IdUsuario);
            }

            return idUsuario;
        }

        public static List<SiteModuloApp> ObterSiteModuloSelecionado()
        {
            var siteModulos = HttpContext.Current.Request.Cookies["siteModulos"];

            var objSiteModulos = JsonConvert.DeserializeObject<List<SiteModuloApp>>(siteModulos.Value);

            return objSiteModulos;
        }

        public static UsuarioApp ObterUsuario()
        {
            var usuarioCookie = HttpContext.Current.Request.Cookies["usuario"];
            var objUsuario = new UsuarioApp();
            if (usuarioCookie != null)
            {
                objUsuario = JsonConvert.DeserializeObject<UsuarioApp>(usuarioCookie.Value);
                objUsuario.IdUsuario = UtilsServico.DesCriptografarString(objUsuario.IdUsuario);
                try
                {
                    objUsuario.Nome = UtilsServico.Descriptografar(objUsuario.Nome);
                }
                catch
                {
                    objUsuario.Nome = objUsuario.Nome;
                }
                
            }

            return objUsuario;
        }
        public static List<PermissoesApp> ObterPermissoes()
        {

            List<PermissoesApp> objPermissoes = (List<PermissoesApp>)HttpContext.Current.Session["matrizPermissao"];

            if(objPermissoes == null)
            {
                objPermissoes = new List<PermissoesApp>();
            }

            return objPermissoes;
        }

        public static string MaskCPF(this string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            string s = input;
            s = input.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);

            while (s.Length < 11)
                s = "0" + s;

            StringBuilder sb = new StringBuilder();
            sb.Append(s.Substring(0, 3));
            sb.Append('.');
            sb.Append(s.Substring(3, 3));
            sb.Append('.');
            sb.Append(s.Substring(6, 3));
            sb.Append('-');
            sb.Append(s.Substring(9, 2));
            return sb.ToString();
        }

        public static string UnMaskCPF(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            string s = input.Replace(".", string.Empty).Replace("-", string.Empty);
            while (s.Length < 11)
                s = "0" + s;
            return s.PadLeft(11, '0');
        }

        public static string MaskCNPJ(this string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            string s = input.PadLeft(14, '0');
            StringBuilder sb = new StringBuilder();
            sb.Append(s.Substring(0, 2));
            sb.Append('.');
            sb.Append(s.Substring(2, 3));
            sb.Append('.');
            sb.Append(s.Substring(5, 3));
            sb.Append('/');
            sb.Append(s.Substring(8, 4));
            sb.Append('-');
            sb.Append(s.Substring(12, 2));
            return sb.ToString();
        }

        public static string MaskCPFCNPJ(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            if (input.Length <= 11)
                return input.MaskCPF();
            else
                return input.MaskCNPJ();
        }

        public static string UnMaskCNPJ(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            string s = input.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            return s.PadLeft(14, '0');
        }

        public static string UnMask(this string input)
        {
            if (input == null) return null;

            string s = input.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);

            return s;
        }

        public static void VerificaDiretorio(string pDiretorio)
        {
            try
            {
                string[] estrutura = pDiretorio.Split('\\');

                string dir = string.Empty;
                for (int i = 0; i < estrutura.Length; i++)
                {
                    if (i == 0)
                    {
                        dir += estrutura[0];
                        continue;
                    }
                    else
                        dir += "\\" + estrutura[i];

                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeletaArquivo(string arquivo)
        {
            if (File.Exists(arquivo))
                File.Delete(arquivo);
        }

        public static string PegaCultura()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[NOME_COOKIE_IDIOMA];

            string nomeCultura;
            if (cookie != null && !String.IsNullOrEmpty(cookie.Value))
                nomeCultura = cookie.Value;
            else
            {
                nomeCultura = "pt-BR";
                DefineCultura(nomeCultura);
            }

            return nomeCultura;
        }

        public static void DefineCultura(string nomeCultura)
        {
            //grava idioma no cookie
            HttpCookie cookie = new HttpCookie(NOME_COOKIE_IDIOMA);
            cookie.Value = nomeCultura;

            //expira em 6 meses
            DateTime dtNow = DateTime.Now;
            TimeSpan tsMinute = new TimeSpan(180, 0, 0, 0);
            cookie.Expires = dtNow + tsMinute;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GeraNovaSenha()
        {
            Random randon = new Random();
            string senha = string.Empty;
            string[] elementos = { "0", "1", "2", "3", "4", "5", "6", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "w", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "W", "Y", "Z" };
            for (int i = 1; i <= 6; i++)
            {
                senha += elementos[randon.Next(0, elementos.Length)];
            }
            return senha;
        }

        public static bool ValidaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11) return false;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909") return false;
            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0) return false;
            }
            else
                if (numeros[9] != 11 - resultado)
                return false; soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0) return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        public static bool ValidaCNPJ(string vrCNPJ)
        {
            string CNPJ = vrCNPJ.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");
            int[] digitos, soma, resultado;
            int nrDig; string ftmt; bool[] CNPJOk;
            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11) soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));
                    if (nrDig <= 12) soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }
                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1)) CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                    else CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                }
                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }

        public static string GetIp(HttpContextBase context)
        {
            return context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? context.Request.UserHostAddress;
        }

        public static string GetBrowser(HttpContextBase context)
        {
            HttpBrowserCapabilitiesBase browser = context.Request.Browser;
            return browser.Type;
        }

        public static string EscreveXML(object objetoDecodificado)
        {
            string objetoFormatoXML = "";
            if (objetoDecodificado != null)
            {
                StringWriter objetoCodificado = new StringWriter();
                UTF8Encoding encoder = new UTF8Encoding();
                XmlSerializer serializadorXML = new XmlSerializer(objetoDecodificado.GetType());

                serializadorXML.Serialize(objetoCodificado, objetoDecodificado);
                objetoFormatoXML = encoder.GetString(encoder.GetBytes(objetoCodificado.ToString()));
                objetoCodificado.Close();
            }
            return objetoFormatoXML;
        }

        public static object LeXML(string objetoSerializado, Type tipoEstrutura)
        {
            object objetoDeserializado = null;
            string objetCodificadoUTF8 = null;

            UTF8Encoding encoder = new UTF8Encoding();
            objetCodificadoUTF8 = encoder.GetString(encoder.GetBytes(objetoSerializado));
            XmlSerializer serializadorXML = new XmlSerializer(tipoEstrutura);
            objetoDeserializado = serializadorXML.Deserialize(new StringReader(objetCodificadoUTF8));
            return objetoDeserializado;
        }
    }
}

