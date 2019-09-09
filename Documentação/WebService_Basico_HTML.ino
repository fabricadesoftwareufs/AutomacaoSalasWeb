#include <Ethernet.h>//Biblioteca EthernetShield 
#include "DHT.h"
#define DHTPIN A0 // pino que estamos conectado
#define DHTTYPE DHT11 // DHT 11

DHT dht(DHTPIN, DHTTYPE);

byte mac[]     = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED }; //Define o endereço físico da placa
byte ip[]      = { 192, 168, 88, 150 };  // Ip da DaPlaca
byte gateway[] = { 192, 168, 88, 1 };   // Gateway (opcional)
byte subnet[]  = { 255, 255, 255, 0 }; // Mascara de subrede

String readString;
int pinoLed = 6;
unsigned long tempoAnterior = 0;
const long intervalo = 20000;
float tem = 0;
float umi = 0;

EthernetServer server(8888); //Cria o objeto na porta 80

void setup()
{
  Ethernet.begin(mac, ip, gateway, subnet); //Inicializando a ethernet
  server.begin(); //Inicializando o servidor web
  
   Serial.println("DHTxx test!");//Sensor de temperatura
   dht.begin();//Sensor de temperatura

   pinMode(pinoLed, OUTPUT);
}



void loop()
{
  EthernetClient client = server.available(); //
  
  if (client) {
    
    // uma requisição http termina com uma linha em branco
    boolean current_line_is_blank = true;
    
    while (client.connected()) {
      
      if (client.available()) {
        
        char c = client.read(); // Recebe caractere do browser e guarda em "c"
        // Se chegamos ao fim da linha (recebeu uma nova linha
        // de caractere) e a linha está em branco, a requisição http termina
        // para que possamos enviar uma resposta!

         if (readString.length() < 100) {
          readString += c;             
        }
        
        if (c == '\n' && current_line_is_blank) {
          
          // Envia um cabeçalho padrão de resposta HTTP 
          client.println("HTTP/1.1 200 OK");
          client.println("Content-Type: text/html");
          client.println("Connection: close");
          client.println("Refresh: 2");
          client.println();
          client.println();

          client.println("<!DOCTYPE html>");
          client.println("<html>");
            client.println("<head>");
            
                client.println("<title> WebServices</title>");
                client.println("<meta charset= ""utf-8"">");
                
            client.println("</head>");
            client.println("<body>");
            
              client.println("<font color=#4279c7><center><h1>Servidor webService</h1></center></font>");
              client.println("<br>");
            
              client.println("<table align=center border=3 bgcolor=#FFF8DC > ");
                  client.println("<tr align=center>");
                      client.println(" <TH><font color=#00FF00>Temperatura</TH></font>");
                      client.println(" <TH><font color=#00FF00>Umidade</TH></font>");
                  client.println(" </tr>");
                  pegar_Temperatura(client);
                  pegar_Umidade(client);
            //       if(millis() - tempoAnterior >= intervalo)
                //      {
                 //       tempoAnterior = millis();
                        client.println(" <tr align=right>");
                            client.println(" <TR>");
                                client.println(" <TD><font color=”#848484”>");
                                pegar_Temperatura(client);
                                client.println("</font></TD>");
                                client.println(" <TD><font color=”#848484”>");
                                pegar_Umidade(client);
                                client.println("</font></TD>");
                   //   }
                        client.println(" </tr>");
                    client.println("</table>");
                 
              //Controlar luz
              client.println("<table align=center border=3>");
              client.println("<center><CAMPTION>Controlar luz</CAPTION></center>");
                  client.println("<tr align=center>");
                      client.println(" <TH>Ligado</TH>");
                      client.println(" <TH>desligado</TH>");
                  client.println(" </tr>");
                  client.println(" <tr align=right>");
                      client.println(" <TR>");
                          client.println(" <TD>");
                          client.println("<a href=\"/?ledon\">ligar o led</a><br />");
                          client.println("</TD>");
                          client.println(" <TD>");
                          client.println("<a href=\"/?ledoff\">Desligar o led</a><br />"); 
                          client.println("</TD>");
                       client.println(" </tr>");
                   client.println("</table>");
                   ligarLuz(client);
            client.println("</body>");
          client.println("</html>");
          
          break;
        }
        if (c == '\n') {
          // Se começamos uma nova linha...
          current_line_is_blank = true;
        } 
        else if (c != '\r') {
          // Se ainda temos um caractere na linha atual...
          current_line_is_blank = false;
        }
      }
    }
 
   } 
  // Damos ao navegador um tempo para receber os dados
  delay(1);
  client.stop();// Fecha a conexão 
}


void pegar_Temperatura(EthernetClient client_aux)
{
   //   float h = dht.readHumidity();
      float t = dht.readTemperature();
      if(isnan(t))
      {
          Serial.println("Failed to read from DHT");
      }
      else
      {
        tem = t;
          client_aux.println("<font color=#000000>");
          client_aux.println("<font color=#4279c7><center><h1>");
          client_aux.println(t);
          client_aux.println(" ºC </h1></center></font>");
      }
}

//////////////////
void pegar_Umidade(EthernetClient client_aux)
{
    tempoAnterior = millis();
    float h = dht.readHumidity();
    if( isnan(h))
    {
        Serial.println("Failed to read from DHT");
    }
    else
    {
       
        client_aux.println("<font color=#4279c7><center><h1>");
        client_aux.println(h);
        client_aux.println(" % </h1></center></font>");
        client_aux.println("</font>");
    }
}

    int pinoSensorLuz = A1;               
    int valorLuz = 0;
   
void sensor_luz(EthernetClient cliente)
{
    cliente.print("Sensor de Luz:");
                         
      valorLuz = analogRead(pinoSensorLuz);        
      if(valorLuz >= 750)
      {                
        cliente.println(" Ambiente iluminado ");
      }
      else
      {
        cliente.println(" Ambiente sem luz ");                    
      }
      delay(10);                   
}

void ligarLuz(EthernetClient dados)
{
   if(readString.indexOf("?ledon")>0)
 {
   digitalWrite(pinoLed, HIGH); 
   dados.println("<center> Ligado </center>");
 }else{
  if(readString.indexOf("?ledof")>0)
  {
    digitalWrite(pinoLed, LOW); 
    dados.println("<center> Desligado </center>");
  }
 }
readString = ""; 
}
