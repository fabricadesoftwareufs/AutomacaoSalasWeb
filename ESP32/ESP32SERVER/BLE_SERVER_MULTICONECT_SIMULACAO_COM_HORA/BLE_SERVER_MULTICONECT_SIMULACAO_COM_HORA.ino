/*
    Video: https://www.youtube.com/watch?v=oCMOYS71NIU
    Based on Neil Kolban example for IDF: https://github.com/nkolban/esp32-snippets/blob/master/cpp_utils/tests/BLE%20Tests/SampleNotify.cpp
    Ported to Arduino ESP32 by Evandro Copercini
    updated by chegewara
   Create a BLE server that, once we receive a connection, will send periodic notifications.
   The service advertises itself as: 4fafc201-1fb5-459e-8fcc-c5c9c331914b
   And has a characteristic of: beb5483e-36e1-4688-b7f5-ea07361b26a8
   The design of creating the BLE server is:
   1. Create a BLE Server
   2. Create a BLE Service
   3. Create a BLE Characteristic on the Service
   4. Create a BLE Descriptor on the characteristic
   5. Start the service.
   6. Start advertising.
   A connect hander associated with the server starts a background task that performs notification
   every couple of seconds.
*/
/*Bibliotecas para configuração de Bluetooth*/
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>
/* ----- Biblioteca para capturar horas ----- */
#include <NTPClient.h> //https://github.com/arduino-libraries/NTPClient
#include <WiFi.h> //Biblioteca do WiFi.
/*Biblioteca para gravar dados em arquivo*/
#include "FileManager.h"

BLEServer* pServer = NULL;
BLECharacteristic* pCharacteristic = NULL;
bool deviceConnected = false;
bool oldDeviceConnected = false;
uint32_t value = 0;

/* ----- Configurações de Wi-fi ----- */  
const char* ssid = "Bezerrinhos"; // Substitua pelo nome da rede
const char* password = "Fazenda19";    // Substitua pela senha

/* ----- Configurações de relógio on-line ----- */
WiFiUDP udp;
NTPClient ntp(udp, "a.st1.ntp.br", -3 * 3600, 60000);//Cria um objeto "NTP" com as configurações.utilizada no Brasil 
int horaAtual = 0;
int minutoAtual = 0;
String stringHoraSistema;
const char* vetorHoraSistema;

/* ----- Horarios ficticios para LIGAR e desligar o AR */
int horaLigarAr = 20
int minutoLigarAr = 30
int horaDesligarAr = 20
int minutoDesligarAr = 35

/* ----- Variavel para indicar se o ar esta ligado ou desligado ----- */
bool arLigado;


// See the following for generating UUIDs:
// https://www.uuidgenerator.net/

#define SERVICE_UUID        "4fafc201-1fb5-459e-8fcc-c5c9c331914b"
#define CHARACTERISTIC_UUID "beb5483e-36e1-4688-b7f5-ea07361b26a8"

//------------------------------------------
//create new file

FileManager fileManager("/Log.txt");

class MyServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* pServer) {
      deviceConnected = true;
      BLEDevice::startAdvertising();
    };

    void onDisconnect(BLEServer* pServer) {
      deviceConnected = false;
    }
};


void splitHora(){
  int cont = 0;
  String unidade = "";

  stringHoraSistema = "";
  unidade = "";
  //Armazena na variável hora, o horário atual.
  stringHoraSistema = ntp.getFormattedTime();
  vetorHoraSistema = stringHoraSistema.c_str();
  
  for (int i = 0; i < 6 ;i++){
    if(vetorHoraSistema[i] == ':'){
      cont++;
      if (cont == 1){
        horaAtual = atoi(unidade.c_str());
      }else if (cont == 2){
        minutoAtual = atoi(unidade.c_str());
        cont = 0;
      }
      unidade = "";  
    }else{
      unidade = unidade + vetorHoraSistema[i];  
    }
  }
}

void setup() {
  Serial.begin(115200);

  /* ----- Configurndo acesso ao WIFI ----- */
  WiFi.begin(ssid, password);
  delay(3000);               // Espera a conexão.
  ntp.begin();               // Inicia o protocolo
  ntp.forceUpdate();         // Atualização .

  // Create the BLE Device
  BLEDevice::init("ESP32MASTER");

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

  // https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml
  // Create a BLE Descriptor
  pCharacteristic->addDescriptor(new BLE2902());

  // Start the service
  pService->start();

//  init file manager
  if (!fileManager.init()) {
    Serial.println("File system error");
    delay(1000);
    ESP.restart();
  }

  // Start advertising
  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->addServiceUUID(SERVICE_UUID);
  pAdvertising->setScanResponse(false);
  pAdvertising->setMinPreferred(0x0);  // set value to 0x00 to not advertise this parameter
  BLEDevice::startAdvertising();
  Serial.println("Waiting a client connection to notify...");
}

void loop() {
  // notify changed value
  
  if (deviceConnected) {
    // Read the value of the characteristic.
    std::string value = pCharacteristic->getValue();
    Serial.print("sensores: ");
    Serial.println(value.c_str());
    pCharacteristic->notify();
    delay(3000); // bluetooth stack will go into congestion, if too many packets are sent, in 6 hours test i was able to go as low as 3ms
    std::string val = value.erase(value.find_last_not_of(" \n\r\t")+1);
    if (val.length() > 0) {
      fileManager.writeInFile(val.c_str());
      Serial.println("Arq log: ");
      Serial.println(fileManager.readFile());
    }

    /* ----- Configurando ar condicionado de acordo com a hora -----*/
    
    splitHora();
    if (horaAtual >= horaLigarAr && horaAtual <= horaDesligarAr && minutoAtual >= minutoLigarAr && !arLigado){
      
      Serial.println("Ligando ar condicionado");
      Serial.println("Hora: ");
      Serial.println(stringHoraSistema);
      
      arLigado = true;
    
    } else if (horaAtual >= horaDesligarAr && minutoAtual >= minutoDesligarAr && arLigado){
      Serial.println("Desligando ar condicionado");
      Serial.println("Hora: ");
      Serial.println(stringHoraSistema);
      arLigado = false;
    }
    
  }
}
