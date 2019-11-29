#include <SoftwareSerial.h>
#include <SD.h>

SoftwareSerial HMMaster(7, 6); // RX = 2, TX = 3

const int ChipSelect = 4;
const int BAUD = 9600;
bool executarScript;
unsigned long tempoLeitura;
File DataFile;
int nCiclos = 0;

void setup(){
  Serial.begin(BAUD);
  Serial.println("HM10 serial started at 9600");
  setupMaster();

   //Inicia a Comunicacao Com O Modulo SD
  if (!SD.begin(ChipSelect)) {
    Serial.println("Falha Ao Acessar O Cartao !");
    Serial.println("Verifique O Cartao/Conexoes E Reinicie O Arduino...");
    executarScript = false;    
  } else {
    Serial.println("Cartao Iniciado Corretamente! ");
    executarScript = true;
  }
}

void loop() {
  if (executarScript)
    conectaDispositivio();    
}


void setupMaster() {
  HMMaster.begin(BAUD);

  // Verificando se comandos AT estão funcionado - verificar o retorno futuramente
  HMMaster.println("AT");
  delay(100);

  // Configurando componente em modo master
  HMMaster.println("AT+ROLE1");
  delay(100);
  HMMaster.println("AT+RESET");
  delay(5000);

  // Pesquisando dispositivos
  HMMaster.println("AT+INQ");
  delay(10000);
}

void conectaDispositivio(){
      //Abrindo arquivo de texto para gravacao de dados
      if (openArq()){
        // Conectando os dois primeiros dispositivos      
        HMMaster.println("AT+CONN1");
        delay(3000);
        HMMaster.println("AT+CONN2");
        delay(2000);
        
        leDado();
  
        // Desconectando dispositivos conectados anterioremnte
        HMMaster.println("AT+SLEEP");
        delay(10000);
    
        // Acordando sensor mestre
        HMMaster.println("AT");
        delay(1000);
          
        // Conectando mais dois dispositivos
        HMMaster.println("AT+CONN3");
        delay(3000);
        HMMaster.println("AT+CONN4");
        delay(2000);
  
        leDado();
  
        // Desconectando dispositivos conectados anterioremnte
        HMMaster.println("AT+SLEEP");
        delay(10000);
  
        // Acordando sensor mestre
        HMMaster.println("AT");
        delay(1000);
        
        // fechando arquivo
        DataFile.close();
      }else{
         Serial.print("Problema ao abrir arquivo, tentando novamento...");
      }
      nCiclos += 1;
}


void leDado() {
  char c;
  
  HMMaster.begin(BAUD);

  tempoLeitura = millis() + 10000;
  
  while (tempoLeitura > millis()){
    if (HMMaster.available()) {
      c = HMMaster.read();
      DataFile.print(c);
      Serial.print(c);
    }
  }
}

bool openArq() {
  // Abre O Arquivo Arquivo.Txt Do Cartao SD
  DataFile = SD.open("EXP_SALA.Txt", FILE_WRITE);
  
  // Verifica se o arquivo foi aberto
  if (DataFile){
    Serial.print("Arquivo aberto, nCiclo: ");
    Serial.println(nCiclos);
    DataFile.print("Arquivo aberto, nCiclos: ");
    DataFile.println(nCiclos);
    return true;
  }else{
    Serial.print("Erro ao abrir arquivo, nCiclo: ");
    Serial.println(nCiclos);
    return false;
  }
}
