using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Dominio.Util
{
    public static class UtilsServico
    {
        public static List<string> PopularErros(DomainValidation.Validation.ValidationResult resultado)
        {
            var erros = new List<string>();

            foreach (var erro in resultado.Erros)
            {
                erros.Add(erro.Message);
            }

            return erros;
        }

        public static List<string> PopularErros(IList<ValidationFailure> resultado)
        {
            var erros = new List<string>();

            foreach (var erro in resultado)
            {
                erros.Add(erro.ToString());
            }

            return erros;
        }

        public static bool ListaEstaPreenchido<T>(List<T> objeto) where T : class
        {
            if (objeto.Count() == 0)
            {
                return false;
            }
            return true;
        }

        public static bool EstaPreenchido<T>(T objeto) where T : class
        {
            if (objeto == null)
            {
                return false;
            }
            return true;
        }

        public static string Sha1Hash(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);

            var hash = string.Empty;

            foreach (var b in hashData)
                hash += b.ToString("X2");

            return hash;
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
    }
}
