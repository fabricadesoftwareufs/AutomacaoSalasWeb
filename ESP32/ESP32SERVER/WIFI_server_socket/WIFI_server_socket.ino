#include <WiFi.h>
const char* ssid     = "Net.Fatinha";
//const char* ssid     = "Net-Fathinha";

const char* password = "alohomora0707";
/* create a server and listen on port 8088 */
WiFiServer server(8088);
void setup()
{
    Serial.begin(115200);
    Serial.print("Connecting to ");
    Serial.println(ssid);
    /* connecting to WiFi */
    WiFi.begin(ssid, password);
    /*wait until ESP32 connect to WiFi*/
    while (WiFi.status() != WL_CONNECTED) {
        delay(500);
        Serial.print(".");
    }
    Serial.println("");
    Serial.println("WiFi connected with IP address: ");
    Serial.println(WiFi.localIP());
    /* start Server */
    server.begin();
}
void loop(){
    /* listen for client */
    WiFiClient client = server.available(); 
    uint8_t data[1024]; 
    if (client) {                   
      Serial.println("new client");         
      /* check client is connected */           
      while (client.connected()) {          
          if (client.available()) {
//              int len = client.read(data, 1024);
//              if(len < 1024){
//                  data[len] = '\0';  
//              }else {
//                  data[1024] = '\0';
//              }
              String line = client.readStringUntil('\n');    
              Serial.print("client sent: ");            
              Serial.println(line); 
              client.println("Opa");
          }
      } 
    }
}
