﻿using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public class Criptography
    {
        
        private static readonly string key = "E546C8DF278CD5931069B522E695D4F2"; // Chave AES de 32 bytes
        
        /// <summary>
        /// Enum AlgorithmS - Representa os tipos de Algorithms de hash disponíveis para geração de senhas.
        /// </summary>
        private enum Algorithms { SHA1, MD5, SHA256 };

        /// <summary>
        /// Propriedade Algorithm - Representa o algoritmo de hash específico.
        /// </summary>
        private static HashAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Método AlgorithmFactory - Cria um algoritmo de hash específico conforme a opção escolhida.
        /// </summary>
        /// <param name="opcao"> Tipo do algoritmo. </param>
        private static void AlgorithmFactory(Algorithms opcao)
        {
            if (opcao == Algorithms.MD5)
                Algorithm = MD5.Create();
            else if (opcao == Algorithms.SHA1)
                Algorithm = SHA1.Create();
            else
                Algorithm = SHA256.Create();
        }

        /// <summary>
        /// Método GerarHashSenha - Gera um hash específico utilizado para gerar senhas.
        /// </summary>
        /// <param name="senha"> Senha em texto plano (sem aplicação de Hash ou Criptografia). </param>
        /// <returns> Hash da senha fornecida. </returns>
        public static string GeneratePasswordHash(string senha)
        {
            var senhaCifrada = Encoding.UTF8.GetBytes(senha);

            // Aplicação da sequência MD5, SHA256, MD5, SHA256.
            AlgorithmFactory(Algorithms.MD5);
            senhaCifrada = Algorithm.ComputeHash(senhaCifrada);
            AlgorithmFactory(Algorithms.SHA256);
            senhaCifrada = Algorithm.ComputeHash(senhaCifrada);
            AlgorithmFactory(Algorithms.MD5);
            senhaCifrada = Algorithm.ComputeHash(senhaCifrada);
            AlgorithmFactory(Algorithms.SHA256);
            senhaCifrada = Algorithm.ComputeHash(senhaCifrada);

            var stringSenha = new StringBuilder();
            foreach (var caractere in senhaCifrada)
                stringSenha.Append(caractere.ToString("X2"));

            return stringSenha.ToString();
        }

        /// <summary>
        /// Método Encrypt - Gera uma criptografia simétrica AES 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Vetor de Inicialização (IV)

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Vetor de Inicialização (IV)

                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] cipherBytes = Convert.FromBase64String(encryptedText);
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}

