#include <Arduino.h>

#include <IRremoteESP8266.h>

#include <IRsend.h>

#include <WiFi.h>

#include <Vector.h>

#include <Streaming.h>

#include "EmonLib.h"

/* 
 *  Variáveis para o sensoriamento de corrente
 */
EnergyMonitor SCT013;

int pinSCT = 13; //Pino analógico conectado ao SCT-013

int tensao = 127;
int potencia;

/* 
 *  dados para a conexão
 */
const char * ssid = "Net.Fatinha"; // ita
const char * password = "alohomora0707";

/* 
 * criando server ouvindo na porta 8088 
 */
WiFiServer server(8088);

/* 
 * pino para usar na captação do sensor de corrente 
 */
const uint16_t kIrLed = 14; // ESP8266 GPIO pin para usar. Recomendado: 4 (D2).

/*
 * Seta o GPIO para enviar o código 
 */
IRsend irsend(kIrLed);

/* 
 * Esse método trata a msg que foi recebida e (retorna o vetor do codigo IR caso essa msg tenha sido um código IR e por referencia atribui o nome do dispositivo)
 */
Vector < int > tratarMsgRecebida(String &msg);

void setup() {
  irsend.begin();
  SCT013.current(pinSCT, 6.0606); // calibra o sensor de corrente

  Serial.begin(115200);
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, password); // conectando ao WiFi 

  while (WiFi.status() != WL_CONNECTED) { // esperando até que o WiFi esteja conectado ao dispositivo 
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi conectado e o ip é: ");
  Serial.println(WiFi.localIP()); // inicia o server 
  server.begin();
}
void loop() {
  WiFiClient client = server.available(); // ouvindo o cliente 
  if (client) {
    Serial.println("Aplicação conectada");
    while (client.connected()) { // checando se o cleinte está conectando ao server 

      if (client.available()) {
        String &&msg = client.readStringUntil('\n');
        Serial.print("cliente enviou: ");
        Serial.println(msg);
        Vector < int > codigo = tratarMsgRecebida(msg);
        delay(1000);
        double Irms = SCT013.calcIrms(1480); // Calcula o valor da Corrente
        potencia = Irms * tensao; // Calcula o valor da Potencia Instantanea   
        Serial.println("ps : ");
        Serial.println(codigo[0]);
        Serial.println(Irms);
        if (codigo[0] != -1) { // se algum código foi recebido
          if (Irms > 1) // se a corrente for maior que (valor de Ampere considerado ligado, é enviado a resposta para aplicação que o sensor está ligado
            client.println("AC-ON");
          else
            client.println("AC-OFF");
        }
      }
    }
  }
}

/*
 * Método para pegar uma posicão específica de uma string separada por um caractere
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
 * comentado na assinatura do método, lá no topo
 */
Vector < int > tratarMsgRecebida(String &msg) {
  //  Strings de comparação
  String condicionador = "CONDICIONADOR";
  String luzes = "LUZES";
  
  String tipoDeMsg = SplitGetIndex(msg, ';', 0); 
  
    int storage_array[200]; // uso do vetor tem que declarar um valor max
    Vector < int > codigo;
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
      Serial.println(el);
      k++;
    }

    irsend.sendRaw(rawData, codigo.size(), 38); // envia comando IR para o equipamento    
    delay(1000);

    msg = codigoString; // atribuição do tipo do dispostivo por referencia para a variável msg 
    return codigo;

  } else if (tipoDeMsg == luzes) { // caso o comando seja para ligar as luzes

  }
  codigo.push_back(-1);
  return codigo;
}
