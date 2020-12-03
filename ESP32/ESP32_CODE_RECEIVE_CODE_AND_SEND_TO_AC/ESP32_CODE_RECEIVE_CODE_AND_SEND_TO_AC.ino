#include <Arduino.h>
#include <IRremoteESP8266.h>      
#include <IRsend.h>
#include <WiFi.h>
#include <Vector.h>
#include <Streaming.h>
/// dados para a conexão 
const char* ssid     = "Net.Fatinha";
const char* password = "alohomora0707";

/* criando server ouvindo na porta 8088 */
WiFiServer server(8088);

// IR 
const uint16_t kIrLed = 13;  // ESP8266 GPIO pin para usar. Recomendado: 4 (D2).

IRsend irsend(kIrLed);  // Seta o GPIO para enviar o código.

//uint16_t rawData[35] = {440,282,162,280,160,606,164,442,164,278,160,606,164,280,160,278,160,280,160,610,160,610,160,444,160,610,160,282,156,308,132,776,158,284,156};  // RCMM 2480A60C

//Esse metodo retorna o codigo IR e por referencia atribui o nome do dispositivo;
Vector<int> tratarCodigoIRrecebido(String msg);

void setup()
{
    irsend.begin();
    Serial.begin(115200);
    Serial.print("Connecting to ");
    Serial.println(ssid);
    /* conectando ao WiFi */
    WiFi.begin(ssid, password);
    /* esperando até que o WiFi esteja conectado ao dispositivo */
    while (WiFi.status() != WL_CONNECTED) {
        delay(500);
        Serial.print(".");
    }
    Serial.println("");
    Serial.println("WiFi conectado e o ip é: ");
    Serial.println(WiFi.localIP());
    /* inicia o server */
    server.begin();
}
void loop()
{
    /* ouvindo o cliente */
    WiFiClient client = server.available(); 
    if (client) {                   
      Serial.println("Aplicação conectada");         
      /* checando se o cleinte está conectando ao server */           
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
