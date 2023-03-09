﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    public class Methods
    {
        public const string TOKEN_PADRAO = "594ac3eb82b5080393ad5c426f61c1ed5ac53f90e1abebc15316888cf1c8f5fe";

        private static List<string> InvalidCpfs = new List<string>{ "11111111111", "22222222222", "33333333333", "44444444444", "55555555555",
                                                                    "66666666666", "66666666666", "77777777777", "88888888888", "99999999999" };

        /// <summary>
        /// Recebe uma string e retorna a mesma sem caracteres especiais.
        /// </summary>
        /// <param stringPoluida="string">String a ser removida os caracteres especiais</param>
        /// <returns>String apenas com letras.</returns>
        public static string CleanString(string stringPoluida) => Regex.Replace(stringPoluida, "[^0-9a-zA-Z]+", "");

        public static bool ValidarCpf(string cpf)
        {
                cpf = CleanString(cpf);

            if (string.IsNullOrEmpty(cpf))
                return false;

            if(InvalidCpfs.Any(x => x.Equals(cpf)))
                return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static string GenerateUUID()
        {
            Guid uuid = Guid.NewGuid();
            return uuid.ToString();
        }

        public static string RandomStr(int length)
        {
            const string src = "abcdefghijklmnopqrstuvwxyz0123456789";
            var strRand = new StringBuilder();
            Random RNG = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[RNG.Next(0, src.Length)];
                strRand.Append(c);
            }
            return strRand.ToString();
        }

        public static string HashSHA256(string baseStr)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(baseStr));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
