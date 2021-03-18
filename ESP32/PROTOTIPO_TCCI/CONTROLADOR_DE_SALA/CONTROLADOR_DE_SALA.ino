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
#include "EmonLib.h"

using namespace std;

/*
 * Dados da rede para conectar o dispositivot
 */
//const char * ssid      = "Net-Fathinha";
//const char * password  = "alohomora0707";
const char* ssid     = "VIVOFIBRA-5F70";
const char* password = "F03C999054";

/*
 * Caminhos para gravacao dos dados em arquivo
 */
const char * path                 = "/horariosSala.txt";
const char * pathLogMonitoramento = "/logMonitoramento.txt";

/*
 * Codigo da sala em que o ESP32 opera
 */
const String id_sala              = "2";

/*
 * Codigo das operacoes que o ESP32 pode fazer
 */
const String operacao_ligar       = "1";
const String operacao_desligar    = "2";

/* 
 *  Variáveis para o sensoriamento de corrente
 */
EnergyMonitor SCT013;
int pinSCT  = 14; //Pino analógico conectado ao SCT-013
int tensao  = 127;
int potencia;

/* 
 * criando server ouvindo na porta 8088 
 */
WiFiServer server(8088);

/*
 * Guarda a aconexão com um cliente
 */
WiFiClient client;

/* 
 * IR 
 * ESP8266 GPIO pin para usar. Recomendado: 4 (D2).
 */
const uint16_t kIrLed = 12;

/* 
 * Seta o GPIO para enviar o código.
 */
IRsend irsend(kIrLed);

/*
 * Variáveis para controlar a periodicidade de quando verificar se os horários estão desatualizados
 */
unsigned long anteriorMillis = 0; // a ultima vez que foi verificado
const long intervalo = 17280; // intervalo de tempo para ser verificado (em Millis) (1 dia)  86400000/5000 -> (para compensar o delay de 5000 é o delay de 5 seg no loop) 17280*5 = 1 dia - 86400000

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
 * Estrutura usada para guardar dados do monitoramento da sala
 */
typedef struct Monitoramento {
  int id;
  bool luzes;
  bool arCondicionado;
  int salaId;
};

/*
 * Guarda as reservas do dia atual
 */
vector <struct Reserva> reservasDeHoje;

/* 
 * Configurações de relógio on-line 
 */
WiFiUDP udp;
NTPClient ntp(udp, "a.st1.ntp.br", -3 * 3600, 60000); //Cria um objeto "NTP" com as configurações.utilizada no Brasil

/* 
 * Guarda hora atual (horário de brasilia)
 */
String horaAtualSistema;


/* 
 * Horarios base para consultar reservas do dia em arquivo 
 */
String horaInicicioCarregarReservas  = "00:05:00";
String horaFimCarregarReservas       = "00:10:00";
bool foiCarregadoHoje = false;

/* 
 * Variaveis para manipular bluetooth do dispositivo 
 */
BLEServer * pServer = NULL;
BLECharacteristic * pCharacteristic = NULL;
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
#define SERVICE_UUID "4fafc201-1fb5-459e-8fcc-c5c9c331914b"
#define CHARACTERISTIC_UUID "beb5483e-36e1-4688-b7f5-ea07361b26a8"
#define LED 2
#define RELE 23

/*
 * classe usada para receber conexoes com outros dispositivos
 */
class MyServerCallbacks: public BLEServerCallbacks {
  void onConnect(BLEServer * pServer) {
    deviceConnected = true;
    BLEDevice::startAdvertising();
  };

  void onDisconnect(BLEServer * pServer) {
    deviceConnected = false;
  }
};

/*
 * classe usada para receber informações de outros dispositivos 
 */
class MyCallbacks: public BLECharacteristicCallbacks {
  void onWrite(BLECharacteristic * pCharacteristic) {
    // Read the value of the characteristic.
    sensoriamento = pCharacteristic -> getValue();
    receivedData = true;
  }
};

/*
 * Configura o dispositivo para receber conexoes bluetooth
 */
void inicializarConfiguracoesBluetooth() {

  /* 
   * Cria novo dispositivo BLE
   */
  BLEDevice::init("ESP32");

  /* 
   * Cria novo Servidor BLE
   */
  pServer = BLEDevice::createServer();
  pServer -> setCallbacks(new MyServerCallbacks());

  /* 
   * Criação dos Servicos BLE
   */
  BLEService * pService = pServer -> createService(SERVICE_UUID);

  /* 
   * Criação das caracteristicas BLE
   */
  pCharacteristic = pService -> createCharacteristic(
    CHARACTERISTIC_UUID,
    BLECharacteristic::PROPERTY_READ |
    BLECharacteristic::PROPERTY_WRITE |
    BLECharacteristic::PROPERTY_NOTIFY |
    BLECharacteristic::PROPERTY_INDICATE
  );

  pCharacteristic -> setCallbacks(new MyCallbacks());

  /* 
   * Cria o BLE descriptor 
   * https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml
   */
  pCharacteristic -> addDescriptor(new BLE2902());

  /* 
   *  Inicia o servico
   */
  pService -> start();

  /* 
   * Start advertising 
   */
  BLEAdvertising * pAdvertising = BLEDevice::getAdvertising();
  pAdvertising -> addServiceUUID(SERVICE_UUID);
  pAdvertising -> setScanResponse(false);
  pAdvertising -> setMinPreferred(0x0); // set value to 0x00 to not advertise this parameter
  BLEDevice::startAdvertising();
  Serial.println("Esperando os clientes iniciarem uma conexao...");
}

/*
 * <descricao> Realiza requisicao ao servidor para obter as reservas da semana para a sala deste dispositivo <descricao/>   
 */
void obterHorariosDaSemana() {

  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status

    HTTPClient http;

    http.begin("http://italabs-002-site2.ctempurl.com/api/horariosala/ReservasDaSemana/" + id_sala); //Specify the URL
    int httpCode = http.GET(); //Make the request

    //Serial.println(String(httpCode));

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
 * <descricao> Obtem do servidor os codigos IR para ligar/desligar o arcondicionado <descricao/>
 * <parametros> operacao: operacao que deve ser consultados os codigos IR (ligar/desligar) <parametros/>
 * <retorno> lista de inteiros com os codigos IR solicitados <retorno/>
 */
vector <int> obterComandosIrByIdSalaAndOperacao(String operacao) {

  String corpoRequisicao = "";
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status

    HTTPClient http;
    String url = "http://italabs-002-site2.ctempurl.com/api/infravermelho/CodigosPorSala/" + id_sala + "/" + operacao;
    http.begin(url); //Specify the URL
    int httpCode = http.GET();

    if (httpCode == 200)
      corpoRequisicao = http.getString();
    else
      Serial.println("Error on HTTP request");

    http.end(); //Free the resources
  }
  StaticJsonBuffer < 1024 > JSONBuffer;
  JsonObject & object = JSONBuffer.parseObject(corpoRequisicao);

  vector < int > listaCodigos;

  if (object.success()) {
    String codigos = object["codigo"];
    String codigo = "";

    for (int i = 0; i < codigos.length(); i++) {
      if (codigos.charAt(i) == ',' || i == codigos.length() - 1) {
        listaCodigos.push_back(codigo.toInt());
        codigo = "";
      } else {
        if (codigos.charAt(i) != ' ')
          codigo += codigos.charAt(i);
      }
    }
  }

  return listaCodigos;
}

/*
 * <descricao> Verifica se é para ligar os dispostivos (luzes e ar) de acordo com as 
 * infomacoes obtidas dos modulos de sensoriamento e dos dados das reservas da sala <descricao/>
 */
void ligarDispositivosGerenciaveis() {
  String horaInicio, horaFim, logMonitoramento;

  struct Reserva r;
  for (r: reservasDeHoje) {

    horaInicio = r.horarioInicio;
    horaFim = r.horarioFim;

    if (horaAtualSistema >= r.horarioInicio && horaAtualSistema < r.horarioFim && temGente) {

      if (!arLigado) {

        vector < int > listaCodigos = obterComandosIrByIdSalaAndOperacao(operacao_ligar);
        enviarComandosIr(listaCodigos);

        enviarMonitoramento(luzesLigadas, arLigado);

        arLigado = true;
        digitalWrite(LED, HIGH);

        Serial.println("Ligando ar condicionado");
        Serial.print("Hora: ");
        Serial.println(horaAtualSistema);

        logMonitoramento = "Ligando ar condicionado no horario: " + horaAtualSistema;
        gravarLinhaEmArquivo(SPIFFS, logMonitoramento, pathLogMonitoramento);

      }

      if (!luzesLigadas) {

        /*
         * Ligando luzes
         */
         ligarLuzes(true);
      }
    }
  }
}

/*
 * <descricao> Verifica se é para desligar os dispostivos (luzes e ar) de acordo com as 
 * informacoes obtidas dos modulos de sensoriamento e dos dados das reservas da sala <descricao/>
 */
void desligarDispositivosGerenciaveis() {
  String horaInicio;
  String horaFim;
  String logMonitoramento;
  bool naoEstaEmAula = true;

  struct Reserva r;
  for (r: reservasDeHoje) {

    horaInicio = r.horarioInicio;
    horaFim = r.horarioFim;

    if (horaAtualSistema >= r.horarioInicio && horaAtualSistema < r.horarioFim)
      naoEstaEmAula = false;
  }

  if (naoEstaEmAula) {
    if (arLigado) {

      vector < int > listaCodigos = obterComandosIrByIdSalaAndOperacao(operacao_desligar);
      enviarComandosIr(listaCodigos);

      enviarMonitoramento(luzesLigadas, arLigado);

      Serial.println("Desligando ar condicionado");
      Serial.print("Hora: ");
      Serial.println(horaAtualSistema);

      arLigado = false;
      digitalWrite(LED, LOW);

      logMonitoramento = "Desligando ar condicionado no horario: " + horaAtualSistema;
      gravarLinhaEmArquivo(SPIFFS, logMonitoramento, pathLogMonitoramento);
    }

    if (luzesLigadas) {
      /*
       * Desligando luzes
       */
      desligarLuzes(true);

    }
  }
}

/*
 * <descricao> Executa o comando de ligar luzes e envia o status do monitoramento pra o servidor além de gravar a operação em log <descricao/>
 */
void ligarLuzes(bool enviarDadosMonitoramento){
  /*
   * Ligando luzes
   */
  Serial.println("LIGANDO");

  luzesLigadas = true;
  digitalWrite(RELE, HIGH);

  if(enviarDadosMonitoramento)
    enviarMonitoramento(luzesLigadas, arLigado);
  
  String logMonitoramento = "Ligando luzes no horario: " + horaAtualSistema;
  gravarLinhaEmArquivo(SPIFFS, logMonitoramento, pathLogMonitoramento);
}

/*
 * <descricao> Executa o comando de desligar luzes e envia o status do monitoramento pra o servidor além de gravar a operação em log <descricao/>
 */
void desligarLuzes(bool enviarDadosMonitoramento){
  /*
   * Desligando luzes
   */
  Serial.println("DESLIGANDO");

  luzesLigadas = false;
  digitalWrite(RELE, LOW);

  if(enviarDadosMonitoramento)
    enviarMonitoramento(luzesLigadas, arLigado);

  String logMonitoramento = "Desligando luzes no horario: " + horaAtualSistema;
  gravarLinhaEmArquivo(SPIFFS, logMonitoramento, pathLogMonitoramento);
}

/*
 * <descricao> Atualiza a tabela Monitoramento do banco de dados com as atualizacoes feitas nos equipamentos pelo ESP  <descricao/>
 * <parametros> luzes: indica o ultimo estado das luzes (ligado/desligado) <parametros/>
 * <parametros> condicionador: indica o ultimo estado do ar condicionado (ligado/desligado) <parametros/>
 * <retorno> string com nome do dispotivo recebido na requisicao ou os codigos IR <retorno/>
 */
bool enviarMonitoramento(bool luzes, bool condicionador) {

  bool atualizacaoMonitoramento = false;
  struct Monitoramento monitoramento = obterMonitoramentoByIdSala();
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status

    HTTPClient http;

    http.begin("http://italabs-002-site2.ctempurl.com/api/monitoramento"); //Specify the URL
    http.addHeader("Content-Type", "application/json");

    String id               = String(monitoramento.id);
    String luzesLiagadas    = String(luzes ? "true" : "false");
    String arCondicionado   = String(condicionador ? "true" : "false");
    String salaId           = String(monitoramento.salaId);

    String monitoramentoJson = "{ ";
          monitoramentoJson += "\"id\": "               + id             + ", ";
          monitoramentoJson += "\"luzes\": "            + luzesLiagadas  + ", ";
          monitoramentoJson += "\"arCondicionado\": "   + arCondicionado + ", ";
          monitoramentoJson += "\"salaId\": "           + salaId         + ", ";
          monitoramentoJson += " }";

    int httpResponseCode = http.PUT(monitoramentoJson);

    //Serial.println(monitoramentoJson);

    if (httpResponseCode == 200) {
      atualizacaoMonitoramento = true;
    } else
      atualizacaoMonitoramento = false;

    http.end(); //Free the resources
  }

  return atualizacaoMonitoramento;
}

/*
 * <descricao> Obtem o estado atual do monitoramento da sala  <descricao/>
 * <retorno> Struct Monitoramento com os dados do monitoramento de acordo com o banco <retorno/>
 */
struct Monitoramento obterMonitoramentoByIdSala() {

  struct Monitoramento monitoramento;
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status

    HTTPClient http;

    http.begin("http://italabs-002-site2.ctempurl.com/api/monitoramento/" + id_sala); //Specify the URL
    int httpCode = http.GET();

    if (httpCode == 200) { //Check for the returning code

      String payload = http.getString();

      StaticJsonBuffer < 1024 > JSONBuffer;
      JsonObject & object = JSONBuffer.parseObject(payload);

      if (object.success()) {
        monitoramento.id = object["id"];
        monitoramento.luzes = object["luzes"];
        monitoramento.arCondicionado = object["arCondicionado"];
        monitoramento.salaId = object["salaId"];
      }
    } else
      Serial.println("Error on HTTP request");

    http.end(); //Free the resources
  }

  return monitoramento;
}


/*
 * <descricao> Executa a operação de envia de comandos IR para o ar-condicionado <descricao/>
 * <parametros> listaCodigos: armazena os codigos infravermelho a serem enviados <parametros/>
 */
void enviarComandosIr(vector <int> listaCodigos) {

  Serial.println("convertido");
  int k = 0;
  uint16_t rawData[listaCodigos.size()];
  for (int cd: listaCodigos) {
    rawData[k] = (uint16_t) cd;
    Serial.println(cd);
    k++;
  }

  irsend.sendRaw(rawData, listaCodigos.size(), 38); ///envio do comando ao equipamento    
  delay(1000);
}

/*
 * <descricao> Obtem nome do dispositivo ou os codigos IR neviados na requisicao do servidor  <descricao/>
 * <parametros> data: codigos IR recebidos na requisicao do servidor <parametros/>
 * <parametros> separator: caracter chave para realizar o 'split' <parametros/>
 * <parametros> index: identificar que diz se quem chama quer receber o nome do dispositivo ou os codigos IR <parametros/>
 * <retorno> string com nome do dispotivo recebido na requisicao ou os codigos IR <retorno/>
 */
String SplitGetIndex(String data, char separator, int index) {
  int found = 0;
  int strIndex[] = {
    0,
    -1
  };
  int maxIndex = data.length() - 1;

  for (int i = 0; i <= maxIndex && found <= index; i++) {
    if (data.charAt(i) == separator || i == maxIndex) {
      found++;
      strIndex[0] = strIndex[1] + 1;
      strIndex[1] = (i == maxIndex) ? i + 1 : i;
    }
  }

  return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}

/*
 * <descricao> Esse metodo retorna o codigo IR e por referencia atribui o nome do dispositivo <descricao/>
 * <parametros> msg: codigos IR recebidos na requisicao do servidor <parametros/>
 * <retorno> Lista de inteiros com codigos ir <retorno/>
 */
int tratarMsgRecebida(String msg) {
  //  Strings de comparação
  String condicionador = "CONDICIONADOR";
  String luzes = "LUZES";
  String atualizar = "atualizarHorarios;";
  
  String tipoDeMsg = SplitGetIndex(msg, ';', 0);
  int retorno = 0;
  
  int storage_array[200]; // uso do vetor tem que declarar um valor max
  Vector <int> codigo;
  codigo.setStorage(storage_array);
  
  if (tipoDeMsg == condicionador) { // se a msg for um comando para enviar para um equipamento de ar
    String codigoString = SplitGetIndex(msg, ';', 1);
    String temp = "";
    for (int i = 0; i < codigoString.length(); i++) {
      if (codigoString.charAt(i) == ',' || i == codigoString.length() - 1) {
        codigo.push_back(temp.toInt());
        temp = "";
      } else {
        if (codigoString.charAt(i) != ';' || codigoString.charAt(i) != ' ')
          temp += codigoString.charAt(i);

      }
    }

    int k = 0;
    uint16_t rawData[codigo.size()];
    for (int el: codigo) {
      rawData[k] = (uint16_t) el;
      k++;
    }

    irsend.sendRaw(rawData, codigo.size(), 38); // envia comando IR para o equipamento    
    delay(1000);

    double Irms = SCT013.calcIrms(1480); // Calcula o valor da Corrente
    potencia = Irms * tensao; // Calcula o valor da Potencia Instantanea  
    if (Irms > 2) // se a corrente for maior que (valor de Ampere considerado ligado, é enviado a resposta para aplicação que o sensor está ligado
       arLigado = true;
    else
       arLigado = false;       

    enviarMonitoramento(luzesLigadas, arLigado);
        
    String logMonitoramento = arLigado ? "Ligando luzes no horario: " + horaAtualSistema :  "Desligando luzes no horario: " + horaAtualSistema;
    gravarLinhaEmArquivo(SPIFFS, logMonitoramento, pathLogMonitoramento); 
    
    retorno = -1;    
    
  } else if (tipoDeMsg == luzes) { // caso o comando seja para ligar as luzes
    
    String operacaoLigarDesligar = SplitGetIndex(msg, ';', 1);
    if(operacaoLigarDesligar == "True;")
      ligarLuzes(false);
    else
      desligarLuzes(false);  
    
    retorno = -2;
    
  } else if (tipoDeMsg == atualizar) {
    obterHorariosDaSemana();

    retorno = -3;
  }

  return retorno;
}

/*
 * <descricao> Obtem as reservas para a data de hoje armazenadas no arquivo <descricao/>
 * <parametros> fs: utilizada para manipular arquivos <parametros/>
 * <parametros> dataAtual: data do dia atual para carregar as reservas <parametros/>
 * <retorno> Lista com reservas do tipo struct Reserva <retorno/>
 */
vector <struct Reserva> carregarHorariosDeHojeDoArquivo(fs::FS & fs, String dataAtual) {
  Serial.printf("Carregando horarios do arquivo: %s\n", path);

  vector <struct Reserva> listaObjetos;

  File file = fs.open(path);
  if (!file || file.isDirectory()) {
    Serial.println("Failed to open file for reading");
    return listaObjetos;
  }

  int nQuebraDeLinha = 0; // a primeira linha do arquivo guarda a data de gravacao do arquivo, então as informacoes estão depois do primeiro '\n'
  String linha;
  String dataReserva;
  struct Reserva r;
  while (file.available()) {

    linha = file.readStringUntil('\n');

    if (nQuebraDeLinha > 0) {

      r = converteJson(linha);
      dataReserva = r.date;

      if (dataReserva.substring(0, 10) == dataAtual)
        listaObjetos.push_back(r);

      dataReserva = "";
    }
    nQuebraDeLinha++;
  }

  file.close();

  foiCarregadoHoje = true;

  return listaObjetos;
}

/*
 * <descricao> Grava no cabeçalho do arquivo a data da requisica/gravacao <descricao/> 
 */
void gravarDataAtualDaRequisicao() {

  String dataRequisicao = obterDataServidor("GETDATE");

  if (dataRequisicao.length() == 0)
    dataRequisicao = "01/01/0001";

  gravarLinhaEmArquivo(SPIFFS, dataRequisicao, path);
}

/*
 * <descricao> Realiza requisicao ao servidor para obter uma data <descricao/>
 * <parametros> identificar: é o parametro que identifica qual tipo de data o usuário está pedindo ao servidor <parametros/>
 * <retorno> Não retorna nenhum objeto <retorno/>
 */
String obterDataServidor(String identificador) {
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status

    HTTPClient http;

    http.begin("http://italabs-002-site2.ctempurl.com/api/Time/" + identificador); //Specify the URL
    int httpCode = http.GET(); //Make the request

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
  payload.replace("[", "");
  payload.replace("]", "");

  // Grava data da requisicao no cabeçalho do arquivo 
  gravarDataAtualDaRequisicao();

  for (int i = 0; i < payload.length(); i++) {

    objetoJson += payload[i];

    if (payload[i] == '}') {
      Serial.println(objetoJson);
      gravarLinhaEmArquivo(SPIFFS, objetoJson, path);
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
struct Reserva converteJson(String objetoJson) {
  struct Reserva res;

  Serial.println(objetoJson);

  StaticJsonBuffer < 1024 > JSONBuffer;
  JsonObject & object = JSONBuffer.parseObject(objetoJson);

  if (object.success()) {

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
bool gravarLinhaEmArquivo(fs::FS & fs, String objetoJson, const char * path) {
  Serial.printf("Writing file: %s\n");

  objetoJson += '\n';
  File file = fs.open(path, FILE_APPEND);

  if (!file) {
    Serial.println("Failed to open file for writing");
    return false;
  }

  bool retorno = false;
  if (file.print(objetoJson))
    retorno = true;

  file.close();

  return retorno;
}

/*
 * <descricao> Percorre um arquivo e mostra as todas as linhas no Monitor Serial  <descricao/>
 * <parametros> fs: utilizada para manipulacao do arquivo <parametros/>
 */
void lerArquivo(fs::FS & fs) {
  Serial.printf("Reading file: %s\n", path);

  File file = fs.open(path);
  if (!file || file.isDirectory()) {
    Serial.println("Failed to open file for reading");
    return;
  }

  Serial.print("Read from file: ");
  while (file.available())
    Serial.println(file.readStringUntil('\n'));

  file.close();
}

/*
 * <descricao> Exclui um arquivo da memoria <descricao/>
 * <parametros> fs: utilizada para manipulacao do arquivo <parametros/>
 * <retorno> retorno true se o arquivo foi removido com suceso ou false caso contrario <parametros/>
 */
bool excluirArquivo(fs::FS & fs) {
  Serial.printf("Deleting file: %s\n", path);

  if (fs.remove(path)) {
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
void verificarSeArquivoEstaAtualizado() {

  String dataArquivo = obterDataArquivo(SPIFFS);
  String dataUltimoDomingo = obterDataServidor("GETDATEPREVIOUSSUNDAY");

  /* 
   * O código carrega todas as reservas da sala para a semana, para verificar se o arquivo está desatualizado
   * é feita uma comparação entre a data de gravacao dos dados no arquivo com a data do ultimo domingo,
   * se a data da gravação dos dados for menor que a data do ultimo domingo, quer dizer que o arquivo está desatualizado,
   * logo, se a data da gravacao do arquivo for maior que a data do ultimo domingo, quer dizer que o arquivo foi atualizado
   * ainda na semana corraente.
   */
  if (dataUltimoDomingo > dataArquivo) {
    Serial.println("Arquivo desatualizado");
    obterHorariosDaSemana();

  } else Serial.println("Arquivo Atualizado");

}

/*
 * <descricao> Obtem a data de gravacao do arquivo presente no cabeçalho <descricao/>
 * <parametros> fs: utilizada para manipulacao do arquivo <parametros/>
 * <retorno> retorna uma string contendo a data de gravacao do arquivo <parametros/>
 */
String obterDataArquivo(fs::FS & fs) {
  String dataAtual = "0001-01-01";
  Serial.printf("Reading file: %s\n", path);

  File file = fs.open(path);
  if (!file || file.isDirectory()) {
    Serial.println("Failed to open file for reading");
    file.close();
    return dataAtual;
  }

  Serial.print("Read from file: ");

  dataAtual = file.readStringUntil('\n');
  Serial.println(dataAtual);

  file.close();

  return dataAtual;
}

/*
 * <descricao> Verifica se a hora atual está no intervalo de horas definido no sistema para 
 * recarregar os horarios do dia atual para a memoria do ESP32 <descricao/>
 */
void verificaHorarioDeCarregarReservas(){
  if (horaAtualSistema >= horaInicicioCarregarReservas && horaAtualSistema <= horaFimCarregarReservas){
       Serial.println(foiCarregadoHoje);
       if(!foiCarregadoHoje){
          Serial.println("Recarregando horarios do dia Atual");
          reservasDeHoje = carregarHorariosDeHojeDoArquivo(SPIFFS, obterDataServidor("GETDATE"));

          if(!foiCarregadoHoje)
            reservasDeHoje.clear();
       }
  } else 
      foiCarregadoHoje = false; 
}



/*
 * <descricao> Ouve requisicoes do cliente conecta via socket <descricao/>
 */
void recebeComandosDoServidor() {
    
    /* 
     * ouvindo o cliente 
     */
     client = server.available();

    if (client) {

      /*
       * Checando se o cleinte está conectando ao server
       */
      while (client.connected()) {

        if (client.available()) {
          String && msg = client.readStringUntil('\n');
          Serial.print("cliente enviou: ");
          Serial.println(msg);
          int tipoMensagem = tratarMsgRecebida(msg);
          delay(1000);
          double Irms = SCT013.calcIrms(1480); // Calcula o valor da Corrente
          potencia = Irms * tensao; // Calcula o valor da Potencia Instantanea   
          //Serial.println("ps : ");
          //Serial.println(Irms);
          if (tipoMensagem == (-1)) { // se algum código foi recebido
                    
               if (Irms > 2) // se a corrente for maior que (valor de Ampere considerado ligado, é enviado a resposta para aplicação que o sensor está ligado
                   client.println("AC-ON");
               else
                   client.println("AC-OFF");
                    
          } else if(tipoMensagem == (-2)) {
                    
               if (luzesLigadas)              
                   client.println("L-ON");
               else
                   client.println("L-OFF");
                   
          }  else if(tipoMensagem == (-3)) {
                client.println("OK");
          }
        }  
        delay(100);
      }
    }
}

void setup() {
  if (!SPIFFS.begin(true))
    Serial.println("SPIFFS falha ao montar objeto de manipulacao de arquivos");

  irsend.begin();

  SCT013.current(pinSCT, 6.0606); // calibra o sensor de corrente
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
   * 
   */
  pinMode(RELE, OUTPUT);

  /*
   * Inicia o protocolo de obtencao de horarios
   */
  ntp.begin();
  ntp.forceUpdate();

  /* 
   * inicia o server 
   */

  server.begin();

  /*
   * 
   */
  struct Monitoramento monitoramento = obterMonitoramentoByIdSala();
  arLigado = monitoramento.arCondicionado;
  luzesLigadas = monitoramento.luzes;
}

void loop() {
  if (deviceConnected && receivedData) {

    dadoSemEspaco = sensoriamento.erase(sensoriamento.find_last_not_of(" \n\r\t") + 1);

    if (dadoSemEspaco.length() > 0)
      gravarLinhaEmArquivo(SPIFFS, dadoSemEspaco.c_str(), pathLogMonitoramento);

    if (sensoriamento.compare("Tem gente!") == 0)
      temGente = true;

    sensoriamento = "";
    dadoSemEspaco = "";
    receivedData = false;
  }

  unsigned long atualMillis = millis(); // verificar se está no tempo de ver se os horários estão desatualizados
  if (atualMillis - anteriorMillis >= intervalo) {
    verificarSeArquivoEstaAtualizado();
    anteriorMillis = atualMillis; // salvando quando foi verificado

  }
  
   horaAtualSistema = ntp.getFormattedTime();
   Serial.println("Hora: ");
   Serial.println(horaAtualSistema);

  /*
   * Socket ouvindo requisicoes do servidor 
   */
  recebeComandosDoServidor();
  
  /*
   * Monitoração continua do ambiente para verificar se é necessário ligar     
   * os equipamentos de acordo com os horários e outras variaveis do ambiente
   */
   ligarDispositivosGerenciaveis();
    
  /*
   * Monitoração continua do ambiente para verificar se é necessário desligar     
   * os equipamentos de acordo com os horários e outras variaveis do ambiente
   */
   desligarDispositivosGerenciaveis();

  /*
   * Verifica se chegou o horário de carregar as reservas do dia 
   * que estao no arquivo para a memoria do ESP32
   */
  verificaHorarioDeCarregarReservas();

  temGente = false;
  delay(5000);
}
