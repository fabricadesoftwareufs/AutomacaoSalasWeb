using System;
using System.Text.RegularExpressions;

namespace Utils
{
    public class Methods
    {
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
    }

    public static string GenerateUUID()
    {
        Guid uuid = Guid.NewGuid();
        return uuid.ToString();
    }
}
