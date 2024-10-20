using Persistence;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Service
{
    public class ClienteSocketService
    {
        private readonly SalasDBContext _context;
        private string Ip { get; set; }
        private const int PORTA = 8088;
        private const int NR_TENTATIVAS_CONEXAO = 5;
        public TcpClient Client { get; set; }
        public Logrequest LogRequest { get; set; }

        public ClienteSocketService(SalasDBContext context, string ip)
        {
            Ip = ip;
            Client = new TcpClient();
            _context = context;
        }

        public void AbrirConexao()
        {
            Client.ConnectAsync(Ip, PORTA).Wait(10000);
        }

        public void FecharConexao()
        {
            Client.Close();
        }


        public string EnviarComando(string comando)
        {
            CreateLog(comando);

            if (Client.Connected)
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
                    }
                    catch (Exception e)
                    {
                        tentativas++;
                        enviouComando = false;
                        Console.WriteLine("Conexão falhou, tentando novamente...");
                        Console.WriteLine("Tentativa nr: " + tentativas);
                        Console.WriteLine(e);
                    }
                } while (!enviouComando && tentativas < NR_TENTATIVAS_CONEXAO);

                SaveLog(resposta);

                return resposta;
            }
            else
            {
                LogRequest.Date = DateTime.Now;
                SaveLog(null);
                Console.WriteLine("Não foi possível estabelecer conexão");
                throw new ServiceException("Não foi possível estabelecer conexão");
            }
        }

        private void CreateLog(string comando)
        {
            LogRequest = new Logrequest()
            {
                Ip = Ip,
                Origin = "ESP32",
                Url = comando
            };
        }
        private void SaveLog(string resposta)
        {
            LogRequest.Date = DateTime.Now;
            LogRequest.Input = resposta;
            LogRequest.StatusCode = LogRequest.Input != null ? HttpStatusCode.OK.ToString() : HttpStatusCode.BadRequest.ToString();
            _context.Logrequests.Add(LogRequest);
            _context.SaveChanges();
        }
    }
}
