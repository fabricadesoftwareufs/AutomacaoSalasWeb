using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    public class StringManipulation
    {
        /// <summary>
        /// Recebe uma string e retorna a mesma sem caracteres especiais.
        /// </summary>
        /// <param stringPoluida="string">String a ser removida os caracteres especiais</param>
        /// <returns>String apenas com letras.</returns>
        public static string CleanString(string stringPoluida) => Regex.Replace(stringPoluida, "[^0-9a-zA-Z]+", "");
    }
}
