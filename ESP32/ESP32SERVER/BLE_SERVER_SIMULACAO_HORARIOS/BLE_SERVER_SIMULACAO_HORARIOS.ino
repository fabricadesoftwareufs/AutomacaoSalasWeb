/* ----- Bibliotecas para configuração do Bluetooth ----- */
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

/* ----- Biblioteca para capturar horas ----------------- */
#include <NTPClient.h> //https://github.com/arduino-libraries/NTPClient
#include <WiFi.h> //Biblioteca do WiFi.

/* ----- Biblioteca para gravar dados em arquivo -------- */
#include "FileManager.h"

/* ----- Variaveis para manipular bluetooth do dispositivo ----- */
BLEServer* pServer = NULL;
BLECharacteristic* pCharacteristic = NULL;
bool deviceConnected = false;
bool oldDeviceConnected = false;
bool receivedData = false;
uint32_t value = 0;

/* ----- Estrutura para guardar os horarios ficticios para LIGAR e desligar o AR ----- */
struct horario {
  int hora = 0;
  int minuto = 0;
}horaDesligarAr, horaLigarAr;

/* ----- Configurações de Wi-fi ----- */
const char* ssid = "DSI-Public"; // Substitua pelo nome da rede
const char* password = "sistem@s"; // Substitua pela senha

/* ----- Configurações de relógio on-line ----- */
WiFiUDP udp;
NTPClient ntp(udp, "a.st1.ntp.br", -3 * 3600, 60000);//Cria um objeto "NTP" com as configurações.utilizada no Brasil
int horaAtual = 0;
int minutoAtual = 0;
String stringHoraSistema;

/* ----- Variavel para indicar se o ar deve ser ligado ou desligado ----- */
bool arLigado = false;
bool temGente = false;

/* ----- Variaveis que armazenam dados recebidos de outros dispositivos ----- */
std::string sensoriamento = "";
std::string dadoSemEspaco = "";


// See the following for generating UUIDs:
// https://www.uuidgenerator.net/
#define SERVICE_UUID        "4fafc201-1fb5-459e-8fcc-c5c9c331914b"
#define CHARACTERISTIC_UUID "beb5483e-36e1-4688-b7f5-ea07361b26a8"
#define LED 2
#define RELE 13

/* ----- Criando um novo arquio ----- */
FileManager fileManager("/LogSimulacao.txt");

void splitHora() {
  int cont = 0;
  String unidade = "";
  stringHoraSistema = "";

  //Armazena na variável hora, o horário atual.
  stringHoraSistema = ntp.getFormattedTime();
  const char* vetorHoraSistema = stringHoraSistema.c_str();

  for (int i = 0; i < 6 ; i++) {
    if (vetorHoraSistema[i] == ':') {
      cont++;
      if (cont == 1) {
        horaAtual = atoi(unidade.c_str());
      } else if (cont == 2) {
        minutoAtual = atoi(unidade.c_str());
      }
      unidade = "";
    } else {
      unidade = unidade + vetorHoraSistema[i];
    }
  }
}

void ligarAr(){
  
   /* ----- Configurando ar condicionado de acordo com a hora ----- */
   if (horaAtual >= horaLigarAr.hora && horaAtual <= horaDesligarAr.hora && !arLigado && temGente) {      
        if ((horaAtual == horaLigarAr.hora && minutoAtual >= horaLigarAr.minuto && horaLigarAr.hora != horaDesligarAr.hora)
         || (horaAtual == horaLigarAr.hora && minutoAtual >= horaLigarAr.minuto && minutoAtual < horaDesligarAr.minuto && horaLigarAr.hora == horaDesligarAr.hora)
         || (horaAtual == horaDesligarAr.hora && minutoAtual < horaDesligarAr.minuto)
         || (horaAtual > horaLigarAr.hora && horaAtual < horaDesligarAr.hora)){

           
         
         Serial.println("Ligando ar condicionado");
         Serial.print("Hora: ");
         Serial.println(stringHoraSistema);
        
         fileManager.writeInFile("Ligando ar condicionado no horario: ");
         fileManager.writeInFile(stringHoraSistema);
        
         arLigado = true;
         digitalWrite(LED, HIGH);

          //Ligando ventilador
         digitalWrite(RELE, LOW);  
      }
   } 
   
}

void desligarAr(){
         
   if (((horaAtual == horaDesligarAr.hora && minutoAtual >= horaDesligarAr.minuto) 
                    || horaAtual < horaLigarAr.hora || horaAtual > horaDesligarAr.hora) && arLigado) {

          Serial.println("Desligando ar condicionado");
          Serial.print("Hora: ");
          Serial.println(stringHoraSistema);
    
          fileManager.writeInFile("Desligando ar condicionado no horario: ");
          fileManager.writeInFile(stringHoraSistema);
    
          arLigado = false;
          digitalWrite(LED, LOW);

          //Dslgando Ventilador
          digitalWrite(RELE, HIGH);
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

void setup() {
  Serial.begin(115200);
  
  // Create the BLE Device
  BLEDevice::init("ESP32");

  // Create the BLE Server
  pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks());

  // Create the BLE Service
  BLEService *pService = pServer->createService(SERVICE_UUID);

  // Create a BLE Characteristic
  pCharacteristic = pService->createCharacteristic(
                      CHARACTERISTIC_UUID,
                      BLECharacteristic::PROPERTY_READ   |
                      BLECharacteristic::PROPERTY_WRITE  |
                      BLECharacteristic::PROPERTY_NOTIFY |
                      BLECharacteristic::PROPERTY_INDICATE
                    );

                    
  pCharacteristic->setCallbacks(new MyCallbacks());
  
  // https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml
  // Create a BLE Descriptor
  pCharacteristic->addDescriptor(new BLE2902());

  // Start the service
  pService->start();
  
  // Start advertising
  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->addServiceUUID(SERVICE_UUID);
  pAdvertising->setScanResponse(false);
  pAdvertising->setMinPreferred(0x0);  // set value to 0x00 to not advertise this parameter
  BLEDevice::startAdvertising();
  Serial.println("Waiting a client connection to notify...");

  //  init file manager
  if (!fileManager.init()) {
    Serial.println("File system error");
    delay(1000);
    ESP.restart();
  }

  pinMode(LED, OUTPUT);
  pinMode(RELE, OUTPUT);
  digitalWrite(RELE, HIGH);


  
  /* ----- Configurndo acesso ao WIFI ----- */
  WiFi.begin(ssid, password);
  delay(5000);               // Espera a conexão.
  ntp.begin();               // Inicia o protocolo
  ntp.forceUpdate();         // Atualização .


  /* ----- Configurandos hoarios ficticios ----- */
  horaLigarAr.hora = 15;
  horaLigarAr.minuto = 30;
  horaDesligarAr.hora = 16;
  horaDesligarAr.minuto = 00;
}

void loop() {
    if (deviceConnected && receivedData) {
      
        Serial.print("sensoriamento: ");
        Serial.println(sensoriamento.c_str());
        delay(1000);

                        
        dadoSemEspaco = sensoriamento.erase(sensoriamento.find_last_not_of(" \n\r\t") + 1);
        
        if (dadoSemEspaco.length() > 0) {
          fileManager.writeInFile(dadoSemEspaco.c_str());
          Serial.println("Arq log: ");
          Serial.println(fileManager.readFile());
        }
        
        delay(3000);

        if (sensoriamento.compare("Tem gente!") == 0)
          temGente = true;
          
        sensoriamento = "";
        dadoSemEspaco = "";
        receivedData = false;        
    }
    
    splitHora();
    Serial.println("Hora: ");
    Serial.println(stringHoraSistema);
    
    ligarAr();
    desligarAr();

    temGente = false;
    delay(3000);
}
