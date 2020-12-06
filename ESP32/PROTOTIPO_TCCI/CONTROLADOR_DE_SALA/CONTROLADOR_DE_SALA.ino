#include <Arduino.h>
#include <IRremoteESP8266.h>      
#include <IRsend.h>
#include <WiFi.h>
#include <Vector.h>
#include <Streaming.h>
#include <HTTPClient.h>
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>
#include <NTPClient.h> 
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
 * Caminhos para gravacao dos dados em arquivo
 */
const char* path                  = "/horariosSala.txt";
const char* pathLogMonitoramento  = "/logMonitoramento.txt";


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

typedef struct Monitoramento {
  int id;
  bool luzes;
  bool arCondicionado;
  int salaId;
  bool salaParticular;
};
/*
 * Guarda as reservas do dia atual
 */
vector<struct Reserva> reservasDeHoje;

/* 
 * Configurações de relógio on-line 
 */
WiFiUDP udp;
NTPClient ntp(udp, "a.st1.ntp.br", -3 * 3600, 60000);//Cria um objeto "NTP" com as configurações.utilizada no Brasil
String horaAtualSistema;


/* 
 * Variaveis para manipular bluetooth do dispositivo 
 */
BLEServer* pServer = NULL;
BLECharacteristic* pCharacteristic = NULL;
bool deviceConnected = false;
bool oldDeviceConnected = false;
bool receivedData = false;
uint32_t value = 0;

/* 
 * Variavel para indicar se o ar deve ser ligado ou desligado 
 */
bool arLigado = false;
bool luzesLigadas = false;
bool temGente = false;

/* 
 * Variaveis que armazenam dados recebidos de outros dispositivos 
 */
std::string sensoriamento = "";
std::string dadoSemEspaco = "";

/*
 * Chave de conexao para os escraves possam se conectar ao controlador.  
 * gerar UUID em: https://www.uuidgenerator.net/
 */
#define SERVICE_UUID        "4fafc201-1fb5-459e-8fcc-c5c9c331914b"
#define CHARACTERISTIC_UUID "beb5483e-36e1-4688-b7f5-ea07361b26a8"
#define LED 2
#define RELE 13



void ligarDispositivosGerenciaveis(){
   String horaInicio, horaFim, logMonitoramento;

   struct Reserva r; 
   for(r : reservasDeHoje){
     
     horaInicio = r.horarioInicio;
     horaFim = r.horarioFim;
     
     if (horaAtualSistema >= r.horarioInicio && horaAtualSistema < r.horarioFim && temGente) {      

           if(!arLigado){
              enviarComandosIr();

              arLigado = true;
              digitalWrite(LED, HIGH); 
              
              Serial.println("Ligando ar condicionado");
              Serial.print("Hora: ");
              Serial.println(horaAtualSistema);

              logMonitoramento = "Ligando ar condicionado no horario: "+horaAtualSistema;
              gravarLinhaEmArquivo(SPIFFS,logMonitoramento,pathLogMonitoramento);

              enviarMonitoramento(luzesLigadas,arLigado);      
           }

           if(!luzesLigadas) { 
            
              /*
               * Ligando luzes
               */
              luzesLigadas = true;
              digitalWrite(RELE, LOW);
              
              logMonitoramento = "Ligando luzes no horario: "+horaAtualSistema;
              gravarLinhaEmArquivo(SPIFFS,logMonitoramento,pathLogMonitoramento);

              enviarMonitoramento(luzesLigadas,arLigado);      
           } 
     } 
  }
}

void desligarDispositivosGerenciaveis(){
   String horaInicio;
   String horaFim;
   String logMonitoramento;
   bool naoEstaEmAula = true;

   struct Reserva r;
   for( r : reservasDeHoje){
     
     horaInicio = r.horarioInicio;
     horaFim = r.horarioFim;
     
     if (horaAtualSistema >= r.horarioInicio && horaAtualSistema < r.horarioFim)
      naoEstaEmAula = false;
   }

   if(naoEstaEmAula){ 
       if(arLigado){
           Serial.println("Desligando ar condicionado");
           Serial.print("Hora: ");
           Serial.println(horaAtualSistema);
            
           arLigado = false;
           digitalWrite(LED, LOW);

           logMonitoramento = "Desligando ar condicionado no horario: "+horaAtualSistema;
           gravarLinhaEmArquivo(SPIFFS,logMonitoramento,pathLogMonitoramento);
       }


       if(luzesLigadas) {  
         /*
          * Desligando luzes
          */
         luzesLigadas = false; 
         digitalWrite(RELE, HIGH);
  
         logMonitoramento = "Desligando luzes no horario: "+horaAtualSistema;
         gravarLinhaEmArquivo(SPIFFS,logMonitoramento,pathLogMonitoramento);

         enviarMonitoramento(luzesLigadas,arLigado);
       }                      
   }
}

class MyServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* pServer) {
      deviceConnected = true;
      BLEDevice::startAdvertising();
    };

    void onDisconnect(BLEServer* pServer) {
      deviceConnected = false;
    }
};


/* ----- classe usada para receber informações de outros dispositivos ----- */
class MyCallbacks: public BLECharacteristicCallbacks {
    void onWrite(BLECharacteristic *pCharacteristic) {
      // Read the value of the characteristic.
      sensoriamento = pCharacteristic->getValue();
      receivedData = true;
    }
};


void inicializarConfiguracoesBluetooth(){
  
  /* 
   * Cria novo dispositivo BLE
   */
  BLEDevice::init("ESP32");

  /* 
   * Cria novo Servidor BLE
   */
  pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks());

  /* 
   * Criação dos Servicos BLE
   */
  BLEService *pService = pServer->createService(SERVICE_UUID);

  /* 
   * Criação das caracteristicas BLE
   */
  pCharacteristic = pService->createCharacteristic(
                      CHARACTERISTIC_UUID,
                      BLECharacteristic::PROPERTY_READ   |
                      BLECharacteristic::PROPERTY_WRITE  |
                      BLECharacteristic::PROPERTY_NOTIFY |
                      BLECharacteristic::PROPERTY_INDICATE
                    );

                    
  pCharacteristic->setCallbacks(new MyCallbacks());
  
  /* 
   * Cria o BLE descriptor 
   * https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml
   */
  pCharacteristic->addDescriptor(new BLE2902());

  /* 
   *  Inicia o servico
   */
  pService->start();
  
  /* 
   * Start advertising 
   */
  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->addServiceUUID(SERVICE_UUID);
  pAdvertising->setScanResponse(false);
  pAdvertising->setMinPreferred(0x0);  // set value to 0x00 to not advertise this parameter
  BLEDevice::startAdvertising();
  Serial.println("Esperando os clientes iniciarem uma conexao...");  
}



/*
 * <descricao>  <descricao/>   
 */
bool enviarMonitoramento(bool luzes, bool condicionador) {
  
  bool atualizacaoMonitoramento = false;
  struct Monitoramento monitoramento = obterMonitoramentoByIdSala();
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status
 
    HTTPClient http;
 
    http.begin("http://igorbruno22-001-site1.ctempurl.com/api/monitoramento/"+id_sala); //Specify the URL

    String id             = String(monitoramento.id);
    String luzes          = String(luzes ? "true" : "false");
    String arCondicionado = String(condicionador ? "true" : "false");
    String salaId         = String(monitoramento.salaId);
    String salaParticular = String(monitoramento.salaParticular ? "true" : "false");

    String monitoramentoJson = "{ "; 
           monitoramentoJson += "id:             " + id             + ", "; 
           monitoramentoJson += "luzes:          " + luzes          + ", ";
           monitoramentoJson += "arCondicionado: " + arCondicionado + ", ";
           monitoramentoJson += "salaId:         " + salaId         + ", ";
           monitoramentoJson += "salaParticular: " + salaParticular + ", ";
           monitoramentoJson += " }";
    
    int httpResponseCode = http.PUT(monitoramentoJson);
 
    if (httpResponseCode == 200){
        atualizacaoMonitoramento = true;
    } else
        atualizacaoMonitoramento = false;
 
    http.end(); //Free the resources
  } 

   return atualizacaoMonitoramento;
}


/*
 * <descricao>  <descricao/>   
 */
struct Monitoramento obterMonitoramentoByIdSala() {

  struct Monitoramento monitoramento;
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status
 
    HTTPClient http;
 
    http.begin("http://igorbruno22-001-site1.ctempurl.com/api/monitoramento/"+id_sala); //Specify the URL
    int httpCode = http.GET();
 
    if (httpCode == 200) { //Check for the returning code
      
         String payload = http.getString();

         StaticJsonBuffer<1024> JSONBuffer;              
         JsonObject& object = JSONBuffer.parseObject(payload);

         if(object.success()){
             monitoramento.id = object["id"];
             monitoramento.luzes = object["luzes"];
             monitoramento.arCondicionado = object["arCondicionado"];
             monitoramento.salaId = object["salaId"];
             monitoramento.salaParticular = object["salaParticular"];
         }
   } else
      Serial.println("Error on HTTP request");
 
    http.end(); //Free the resources
  } 

   return monitoramento;
}


void enviarComandosIr() {
  
  
}

/*=======================================================================================*/

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
        
        // Percorrendo lista de onjetos json e gravando no arquivo
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
    
  gravarLinhaEmArquivo(SPIFFS,dataRequisicao,path);
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
          gravarLinhaEmArquivo(SPIFFS,objetoJson,path);
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
bool gravarLinhaEmArquivo(fs::FS &fs, String objetoJson, const char* path){
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
     * Obtendo as reservas da sala atual para o dia de hoje 
     */
    reservasDeHoje = carregarHorariosDeHojeDoArquivo(SPIFFS, obterDataServidor("GETDATE"));

    /*
     * Configurar ESP para trabalhar com protocolo Bluetooth
     */
    inicializarConfiguracoesBluetooth();

    /*
     * 
     */
    pinMode(LED, OUTPUT);

    /*
     * Inicia o protocolo de obtencao de horarios
     */
    ntp.begin();               
    ntp.forceUpdate();         
    
    /* 
     * inicia o server 
     */
    server.begin();
}

void loop()
{
    lerArquivo(SPIFFS);
    
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
    
   delay(1000);
}
