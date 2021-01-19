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
        public TcpClient Client { get; set; }
        public ClienteSocketService(string ip)
        {
            Ip = ip;
            Client = new TcpClient();

        }

        public string EnviarComando(string comando)
        {
            if (!Client.ConnectAsync(Ip, PORTA).Wait(10000))
            {
                int tentativas = 0;
                bool enviouComando;
                string resposta = null;
                do
                {

                    try
                    {
                        enviouComando = true;
                        int byteCount = Encoding.ASCII.GetByteCount(comando + 1);
                        byte[] sendData = Encoding.ASCII.GetBytes(comando);

                        NetworkStream stream = Client.GetStream();
                        stream.Write(sendData, 0, sendData.Length);
                        Console.WriteLine("Enviando dados para o servidor...");

                        /*
                         * StreamReader sr = new StreamReader(stream);
                         * string response = sr.ReadLine();
                         * Console.WriteLine(response);
                         */
                        byte[] myReadBuffer = new byte[1024];
                        StringBuilder myCompleteMessage = new StringBuilder();
                        int numberOfBytesRead = 0;

                        do
                        {
                            numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

                            myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                        }
                        while (stream.DataAvailable);

                        resposta = myCompleteMessage.ToString();

                        stream.Close();
                        Client.Close();
                    }
                    catch (Exception)
                    {
                        tentativas++;
                        enviouComando = false;
                        Console.WriteLine("Conexão falhhou, tentando novamente...");
                        Console.WriteLine("Tentativa nr: " + tentativas);
                    }
                } while (!enviouComando && tentativas < NR_TENTATIVAS_CONEXAO);

                return resposta;
            }
            else
            {
                return null;
            }
        }
    }
}
