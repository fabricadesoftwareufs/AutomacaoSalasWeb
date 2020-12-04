#include <Arduino.h>
#include <IRremoteESP8266.h>      
#include <IRsend.h>
#include <WiFi.h>
#include <Vector.h>
#include <Streaming.h>
#include <HTTPClient.h>
#include "ArduinoJson.h"
#include "FS.h"
#include "SPIFFS.h"
using namespace std;

/*
 * Dados da rede para conectar o dispositivo
 */
const char* ssid     = "VIVOFIBRA-5F70";
const char* password = "F03C999054";


/*
 * Caminho para gravacao dos dados em arquivo
 */
const char* path     = "/horariosSala.txt";


/*
 * Codigo da sala em que o ESP opera
 */
const int id_sala    = 2;

/* 
 * criando server ouvindo na porta 8088 
 */
WiFiServer server(8088);

/* 
 * IR 
 * ESP8266 GPIO pin para usar. Recomendado: 4 (D2).
 */
const uint16_t kIrLed = 13;  


/* 
 * Seta o GPIO para enviar o código.
 */
IRsend irsend(kIrLed);  


/*
 * Estrutura usada para guardar dados da reserva da sala
 */
typedef struct Reserva {
  int id;
  const char * date;
  const char * horarioInicio;
  const char * horarioFim;
  const char * situacao;
  const char * objetivo;
  int usuarioId;
  int salaId;
  int planejamento; 
};

/*
 * <descricao> Obtem nome do dispositivo ou os codigos IR neviados na requisicao do servidor  <descricao/>
 * <parametros> data: codigos IR recebidos na requisicao do servidor <parametros/>
 * <parametros> separator: caracter chave para realizar o 'split' <parametros/>
 * <parametros> index: identificar que diz se quem chama quer receber o nome do dispositivo ou os codigos IR <parametros/>
 * <retorno> string com nome do dispotivo recebido na requisicao ou os codigos IR <retorno/>
 */
String SplitGetIndex(String data, char separator, int index)
{
  int found = 0;
  int strIndex[] = {0, -1};
  int maxIndex = data.length()-1;

  for(int i=0; i<=maxIndex && found<=index; i++){
    if(data.charAt(i)==separator || i==maxIndex){
        found++;
        strIndex[0] = strIndex[1]+1;
        strIndex[1] = (i == maxIndex) ? i+1 : i;
    }
  }

  return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}


/*
 * <descricao> Esse metodo retorna o codigo IR e por referencia atribui o nome do dispositivo <descricao/>
 * <parametros> msg: codigos IR recebidos na requisicao do servidor <parametros/>
 * <retorno> Lista de inteiros com codigos ir <retorno/>
 */
Vector<int> tratarCodigoIRrecebido(String msg)
{
  String nomeDispositivo = SplitGetIndex(msg, ';', 0);
  String codigoString = SplitGetIndex(msg, ';', 1);
  
  // uso do vetor tem que declarar um valor max
  int storage_array[200];
  Vector<int> codigo;
  codigo.setStorage(storage_array);
  Serial.println(codigoString);
  String temp = "";
  for(int i = 0; i < codigoString.length(); i++){
    if(codigoString.charAt(i)== ',' || i == codigoString.length()-1){
      codigo.push_back(temp.toInt());
      temp ="";
    }
    else {
      if(codigoString.charAt(i) != ';' || codigoString.charAt(i) != ' ')
        temp += codigoString.charAt(i);
       
    }
  }
  int k = 0;
  uint16_t rawData[35];
  Serial.println(codigo.size());
  for (int el : codigo)
  {
       rawData[k] = (uint16_t)el;
             Serial.println(el); 
       k++;
 }
  irsend.sendRaw(rawData, codigo.size(), 38);  ///envio do comando ao equipamento    
    delay(1000);
    
   return codigo;
}


/*
 * <descricao> Obtem as reservas para a data de hoje armazenadas no arquivo <descricao/>
 * <parametros> fs: utilizada para manipular arquivos <parametros/>
 * <parametros> dataAtual: data do dia atual para carregar as reservas <parametros/>
 * <retorno> Lista com reservas do tipo struct Reserva <retorno/>
 */
vector<struct Reserva> carregarHorariosDeHojeDoArquivo(fs::FS &fs, String dataAtual){
    Serial.printf("Carregando horarios do arquivo: %s\n", path);
        
    vector<struct Reserva> listaObjetos;

    File file = fs.open(path);
    if(!file || file.isDirectory()){
          Serial.println("Failed to open file for reading");
          return listaObjetos;
    }
  
    Serial.print("Read from file: ");
    int nQuebraDeLinha = 0; // a primeira linha do arquivo guarda a data de gravacao do arquivo, então as informacoes estão depois do primeiro '\n'
    String linha;
    String dataReserva;
    struct Reserva r;
    while(file.available()){
      
        linha = file.readStringUntil('\n');
      
        if(nQuebraDeLinha > 0){

          r = converteJson(linha);
          dataReserva = r.date;
         
          if (dataReserva.substring(0,10) == dataAtual)
            listaObjetos.push_back(r);

          dataReserva = "";
        }  
        nQuebraDeLinha++;
    }
    
    return listaObjetos; 
}


/*
 * <descricao> Realiza requisicao ao servidor para obter as reservas da semana para a sala deste dispositivo <descricao/>   
 */
void obterHorariosDaSemana() {
  
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status
 
    HTTPClient http;
 
    http.begin("http://igorbruno22-001-site1.ctempurl.com/api/reservasala/reservasdasemana/"+id_sala); //Specify the URL
    int httpCode = http.GET();                                        //Make the request
 
    if (httpCode == 200) { //Check for the returning code

        // Obtendo corpo da mensagem
        String payload = http.getString();

        // Excluindo arquivo com dados desatualizados
        excluirArquivo(SPIFFS);
        
        // Percorrendo lista de onjetos jsone gravando no arquivo
        percorreListaDeObjetos(payload);  
    } else
      Serial.println("Error on HTTP request");
 
    http.end(); //Free the resources
  }  
}


/*
 * <descricao> Grava no cabeçalho do arquivo a data da requisica/gravacao <descricao/> 
 */
void gravarDataAtualDaRequisicao() {
  
  String dataRequisicao = obterDataServidor("GETDATE");
  
  if(dataRequisicao.length() == 0)
    dataRequisicao = "01/01/0001";
    
  gravarHorariosEmArquivo(SPIFFS,dataRequisicao);
}

/*
 * <descricao> Realiza requisicao ao servidor para obter uma data <descricao/>
 * <parametros> identificar: é o parametro que identifica qual tipo de data o usuário está pedindo ao servidor <parametros/>
 * <retorno> Não retorna nenhum objeto <retorno/>
 */
String obterDataServidor(String identificador){
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status
 
    HTTPClient http;
 
    http.begin("http://igorbruno22-001-site1.ctempurl.com/api/Time/"+identificador); //Specify the URL
    int httpCode = http.GET();                                                          //Make the request
 
    if (httpCode == 200) { 
        // Obtendo corpo da mensagem
       return http.getString();
       
    } else
      Serial.println("Error on HTTP request");
 
    http.end(); //Free the resources
  }
  
  return "0001-01-01";
}

/*
 * <descricao> Manipula string com objetos json e os armazena no arquivo  <descricao/>
 * <parametros> payload: lista de reservas em formato json <parametros/>
 */
void percorreListaDeObjetos(String payload) {
   
   String objetoJson; 
   payload.replace("[","");
   payload.replace("]","");
    
   // Grava data da requisicao no cabeçalho do arquivo 
   gravarDataAtualDaRequisicao();
     
   for(int i = 0; i < payload.length();i++){
      
      objetoJson += payload[i];
      
      if(payload[i] == '}'){
          Serial.println(objetoJson);
          gravarHorariosEmArquivo(SPIFFS ,objetoJson);
          objetoJson = "";
          i++;
      }  
   }       
}

/*
 * <descricao> Converte um objeto json para o Tipo struct Reserva <descricao/>
 * <parametros> objetoJson: registro de reserva em formato json <parametros/>
 * <retorno> retorna uma variavel struct Reserva com as informacoes do objeto json <retorno/>
 */
struct Reserva converteJson(String objetoJson){
   struct Reserva res;

   Serial.println(objetoJson); 
   
   StaticJsonBuffer<1024> JSONBuffer;              
   JsonObject& object = JSONBuffer.parseObject(objetoJson);

   if(object.success()){
      
       res.id = object["id"];
       res.date = object["data"];
       res.horarioInicio = object["horarioInicio"];
       res.horarioFim = object["horarioFim"];
       res.situacao = object["situacao"];
       res.objetivo = object["objetivo"];
       res.usuarioId = object["usuarioId"];
       res.salaId = object["salaId"];
       res.planejamento = object["planejamento"];
    
   }
   
   return res;    
}


/*
 * <descricao> Conecta dispositivo na rede <descricao/>
 */
void conectarDispoitivoNaRede() {
  
  WiFi.begin(ssid, password);
 
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi..");
  }
 
  Serial.println("WiFi conectado e o ip é: ");
  Serial.println(WiFi.localIP());
}


/*
 * <descricao> Grava um objeto json no arquivo <descricao/>
 * <parametros> fs: utilizada para manipulacao do arquivo <parametros/>
 * <retorno> retorno true se o objeto foi gravado com suceso ou false caso contrario <retorno/>
 */
bool gravarHorariosEmArquivo(fs::FS &fs, String objetoJson){
    Serial.printf("Writing file: %s\n");

    objetoJson += '\n';
    File file = fs.open(path, FILE_APPEND);
    
    if(!file){
        Serial.println("Failed to open file for writing");
        return false;
    }
    
    if(file.print(objetoJson))
        return true;
    else
        return false;
}


/*
 * <descricao> Percorre um arquivo e mostra as todas as linhas no Monitor Serial  <descricao/>
 * <parametros> fs: utilizada para manipulacao do arquivo <parametros/>
 */
void lerArquivo(fs::FS &fs){
    Serial.printf("Reading file: %s\n", path);

    File file = fs.open(path);
    if(!file || file.isDirectory()){
        Serial.println("Failed to open file for reading");
        return;
    }

    Serial.print("Read from file: ");
    while(file.available())
        Serial.println(file.readStringUntil('\n'));
}

/*
 * <descricao> Exclui um arquivo da memoria <descricao/>
 * <parametros> fs: utilizada para manipulacao do arquivo <parametros/>
 * <retorno> retorno true se o arquivo foi removido com suceso ou false caso contrario <parametros/>
 */
bool excluirArquivo(fs::FS &fs){
    Serial.printf("Deleting file: %s\n", path);
    
    if(fs.remove(path)){
        Serial.println("File deleted");
        return true;
    } else {
        Serial.println("Delete failed");
        return false;
    }
}


/*
 * <descricao> Verifica se o dispositivo já buscou os horarios dessa semana <descricao/>
 */
void verificarSeArquivoEstaAtualizado(){
  
  String dataArquivo = obterDataArquivo(SPIFFS);
  String dataUltimoDomingo = obterDataServidor("GETDATEPREVIOUSSUNDAY");
  
  /* 
   * O código carrega todas as reservas da sala para a semana, para verificar se o arquivo está desatualizado
   * é feita uma comparação entre a data de gravacao dos dados no arquivo com a data do ultimo domingo,
   * se a data da gravação dos dados for menor que a data do ultimo domingo, quer dizer que o arquivo está desatualizado,
   * logo, se a data da gravacao do arquivo for maior que a data do ultimo domingo, quer dizer que o arquivo foi atualizado
   * ainda na semana corraente.
   */
  if (dataUltimoDomingo >= dataArquivo) {
     
     Serial.println("Arquivo desatualizado"); 
     obterHorariosDaSemana();
     
  } else Serial.println("Arquivo Atualizado");   
  
}

/*
 * <descricao> Obtem a data de gravacao do arquivo presente no cabeçalho <descricao/>
 * <parametros> fs: utilizada para manipulacao do arquivo <parametros/>
 * <retorno> retorna uma string contendo a data de gravacao do arquivo <parametros/>
 */
String obterDataArquivo(fs::FS &fs){
  String dataAtual;
  Serial.printf("Reading file: %s\n", path);

  File file = fs.open(path);
  if(!file || file.isDirectory()){
        Serial.println("Failed to open file for reading");
        return "0001-01-01";
  }

  Serial.print("Read from file: ");

  dataAtual = file.readStringUntil('\n');
  Serial.println(dataAtual);
  
  return dataAtual; 
}

void setup()
{
    if(!SPIFFS.begin(true))
        Serial.println("SPIFFS falha ao montar objeto de manipulacao de arquivos");
    
    irsend.begin();
    
    Serial.begin(115200);
    delay(4000);

    /*
     * Realiza conexao com a rede WIFI
     */
    conectarDispoitivoNaRede();

    /*
     * Verifica se é necessário requisitar as reservas dessa semana ao servidor 
     */
    verificarSeArquivoEstaAtualizado();
    
    /* 
     * inicia o server 
     */
    server.begin();
}

void loop()
{
    /* 
     * ouvindo o cliente 
     */
    WiFiClient client = server.available(); 
        
    if (client) {                   
      
      Serial.println("Aplicação conectada");         
      
      /*
       * Checando se o cleinte está conectando ao server
       */           
      while (client.connected()) {   
               
          if (client.available()) {
              String equipamentoIR = client.readStringUntil('\n');    
              Serial.print("cliente enviou: ");            
              Serial.println(equipamentoIR); 
              Vector<int> codigo = tratarCodigoIRrecebido(equipamentoIR);
              delay(1000);
              client.println("Resposta");        ///envia resposta para aplicação

          }   
      }
    }
}
