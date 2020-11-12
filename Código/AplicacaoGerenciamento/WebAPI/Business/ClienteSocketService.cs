using System;
using System.Net.Sockets;
using System.Text;

namespace Service
{
    public class ClienteSocketService
    {
        private string Ip { get; set; }
        private const int PORTA = 8088;
        private const int NR_TENTATIVAS_CONEXAO = 5;

        public ClienteSocketService(string ip)
        {
            Ip = ip;
        }

        public bool EnviarComando(string comando)
        {
            int tentativas = 0;
            bool enviouComando;

            do
            {

                try
                {
                    enviouComando = true;
                    TcpClient client = new TcpClient(Ip, PORTA);

                    int byteCount = Encoding.ASCII.GetByteCount(comando + 1);
                    byte[] sendData = Encoding.ASCII.GetBytes(comando);

                    NetworkStream stream = client.GetStream();
                    stream.Write(sendData, 0, sendData.Length);
                    Console.WriteLine("Enviando dados para o servidor...");

                    /*
                     * StreamReader sr = new StreamReader(stream);
                     * string response = sr.ReadLine();
                     * Console.WriteLine(response);
                     */

                    stream.Close();
                    client.Close();
                }
                catch (Exception)
                {
                    tentativas++;
                    enviouComando = false;
                    Console.WriteLine("Conexão falhhou, tentando novamente...");
                    Console.WriteLine("Tentativa nr: " + tentativas);
                }
            } while (!enviouComando && tentativas < NR_TENTATIVAS_CONEXAO);

            return enviouComando;
        }
    }
}
