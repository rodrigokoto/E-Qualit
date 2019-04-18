using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Dominio.Servico
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


        const string senha = "EqualityHash";

        public static string Criptografar(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }

        public static string Descriptografar(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }

        public static string CriptografarString(string mensagem)
        {
            byte[] results;
            var senha = "equality#4862";

            var UTF8 = new UTF8Encoding();
            // Passo 1. Calculamos o hash da senha usando MD5
            // Usamos o gerador de hash MD5 como o resultado é um array de bytes de 128 bits
            // que é um comprimento válido para o codificador TripleDES usado abaixo
            var hashProvider = new MD5CryptoServiceProvider();
            byte[] tDESKey = hashProvider.ComputeHash(UTF8.GetBytes(senha));

            // Passo 2. Cria um objeto new TripleDESCryptoServiceProvider
            var tDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Passo 3. Configuração do codificador
            tDESAlgorithm.Key = tDESKey;
            tDESAlgorithm.Mode = CipherMode.ECB;
            tDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Passo 4. Converta a seqüência de entrada para um byte []
            byte[] dataToEncrypt = UTF8.GetBytes(mensagem);
            // Passo 5. Tentativa para criptografar a seqüência de caracteres
            try
            {
                ICryptoTransform encryptor = tDESAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                // Limpe as tripleDES e serviços hashProvider de qualquer informação sensível
                tDESAlgorithm.Clear();
                hashProvider.Clear();
            }
            // Passo 6. Volte a seqüência criptografada como uma string base64 codificada
            return Convert.ToBase64String(results);
        }

        public static string DesCriptografarString(string mensagem)
        {
            byte[] resultado;
            var senha = "equality#4862";
            var UTF8 = new UTF8Encoding();

            // Passo 1. Calculamos o hash da senha usando MD5
            // Usamos o gerador de hash MD5 como o resultado é um array de bytes de 128 bits
            // que é um comprimento válido para o codificador TripleDES usado abaixo
            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            byte[] tDESKey = hashProvider.ComputeHash(UTF8.GetBytes(senha));

            // Passo 2. Cria um objeto new TripleDESCryptoServiceProvider 
            var tDESAlgorithm = new TripleDESCryptoServiceProvider();
            // Passo 3. Configuração do codificador
            tDESAlgorithm.Key = tDESKey;
            tDESAlgorithm.Mode = CipherMode.ECB;
            tDESAlgorithm.Padding = PaddingMode.PKCS7;
            // Passo 4. Converta a seqüência de entrada para um byte []
            byte[] dataToDecrypt = Convert.FromBase64String(mensagem);
            // Passo 5. Tentativa para criptografar a seqüência de caracteres
            try
            {
                ICryptoTransform Decryptor = tDESAlgorithm.CreateDecryptor();
                resultado = Decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                // Limpe as tripleDES e serviços hashProvider de qualquer informação sensível
                tDESAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Passo 6. Volte a seqüência criptografada como uma string base64 codificada 
            return UTF8.GetString(resultado);
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
