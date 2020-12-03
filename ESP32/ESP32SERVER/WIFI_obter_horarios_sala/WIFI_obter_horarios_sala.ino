#include <WiFi.h>
#include <HTTPClient.h>
#include "ArduinoJson.h"
#include "FS.h"
#include "SPIFFS.h"

const char* ssid     = "VIVOFIBRA-5F70";
const char* password = "F03C999054";

const char* path     = "/horariosSala.txt";

typedef struct Reserva {
  int id;
  int date;
  const char * horarioInicio;
  const char * horarioFim;
  const char * situacao;
  const char * objetivo;
  int usuarioId;
  int salaId;
  int planejamento; 
};

struct Reserva reserva[50];
 
void setup() {
  
  if(!SPIFFS.begin(true)){
        Serial.println("SPIFFS Mount Failed");
        return;
  }

  Serial.begin(115200);
  delay(4000);
  
  conectarDispoitivoNaRede();

  /* 
   * O código carrega todas as reservas da sala para a semana, para verificar se o arquivo está desatualizado
   * é feita uma comparação entre a data de gravacao dos dados no arquivo com a data do ultimo domingo,
   * se a data da gravação dos dados for menor que a data do ultimo domingo, quer dizer que o arquivo está desatualizado,
   * logo, se a data da gravacao do arquivo for maior que a data do ultimo domingo, quer dizer que o arquivo foi atualizado
   * ainda na semana corrente.
   */
  String dataArquivo = obterDataArquivo(SPIFFS,path);
  String dataUltimoDomingo = obterDataServidor("GETDATEPREVIOUSSUNDAY");
  
  if (dataUltimoDomingo >= dataArquivo) {
     Serial.println("Arquivo desatualizado"); 
     obterHorariosDaSemana();
  }
  else
    Serial.println("Arquivo Atualizado"); 
}
 
void loop() {

  lerArquivo(SPIFFS,path);
  delay(10000);
}


void obterHorariosDaSemana() {
  
  if ((WiFi.status() == WL_CONNECTED)) { //Check the current connection status
 
    HTTPClient http;
 
    http.begin("http://igorbruno22-001-site1.ctempurl.com/api/reservasala/reservasdasemana/2"); //Specify the URL
    int httpCode = http.GET();                                        //Make the request
 
    if (httpCode == 200) { //Check for the returning code

        // Obtendo corpo da mensagem
        String payload = http.getString();

        // Excluindo arquivo com dados desatualizados
        excluirArquivo(SPIFFS,path);
        
        // Percorrendo lista de onjetos jsone gravando no arquivo
        percorreListaDeObjetos(payload);  
    } else
      Serial.println("Error on HTTP request");
 
    http.end(); //Free the resources
  }  
}


void gravarDataAtualDaRequisicao() {
  
  String dataRequisicao = obterDataServidor("GETDATE");
  
  if(dataRequisicao.length() == 0)
    dataRequisicao = "01/01/0001";
    
  gravarHorariosEmArquivo(SPIFFS,path,dataRequisicao);
}

String obterDataServidor(String identificador)
{
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
  
  return "";
}

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
          gravarHorariosEmArquivo(SPIFFS, path ,objetoJson);
          objetoJson = "";
          i++;
      }  
   }       
}

struct Reserva converteJson(String objetoJson){
   struct Reserva res;
   
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

void conectarDispoitivoNaRede() {
  
  WiFi.begin(ssid, password);
 
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi..");
  }
 
  Serial.println("Connected to the WiFi network");  
}


void gravarHorariosEmArquivo(fs::FS &fs, const char * path, String message){
    Serial.printf("Writing file: %s\n", path);

    message += '\n';
    File file = fs.open(path, FILE_APPEND);
    if(!file){
        Serial.println("Failed to open file for writing");
        return;
    }
    if(file.print(message)){
        Serial.println("File written");
    } else {
        Serial.println("Write failed");
    }
}


void lerArquivo(fs::FS &fs, const char * path){
    Serial.printf("Reading file: %s\n", path);

    File file = fs.open(path);
    if(!file || file.isDirectory()){
        Serial.println("Failed to open file for reading");
        return;
    }

    Serial.print("Read from file: ");
    while(file.available()){
        Serial.write(file.read());
    }
}

void excluirArquivo(fs::FS &fs, const char * path){
    Serial.printf("Deleting file: %s\n", path);
    if(fs.remove(path)){
        Serial.println("File deleted");
    } else {
        Serial.println("Delete failed");
    }
}

String obterDataArquivo(fs::FS &fs, const char * path){
  String dataAtual;
  Serial.printf("Reading file: %s\n", path);

  File file = fs.open(path);
  if(!file || file.isDirectory()){
        Serial.println("Failed to open file for reading");
        return "01/01/0001";
  }

  Serial.print("Read from file: ");
  char caractere;
  while(file.available() || caractere == '\n'){
      caractere = file.read();
      
      if(caractere != '#' && caractere != ' ' && caractere != '\n')
        dataAtual += caractere;
  }

  Serial.println(dataAtual);
  
  return dataAtual; 
}
