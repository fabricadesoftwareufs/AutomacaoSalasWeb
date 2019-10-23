#include <SoftwareSerial.h>
#include "dht.h" // LIB TEMP

// Portas
SoftwareSerial HMSlave(12, 11); // RX = 2, TX = 3
int BAUD = 9600;
const int pinoDHT11 = A0; //PINO ANALÓGICO UTILIZADO PELO DHT11

// Declaração global DHT
dht DHT;

void setup()
{
  Serial.begin(BAUD);
  pinMode(pinoDHT11, INPUT);
  Serial.println("HM10 serial started at 9600");
  //setupSlave();
}

void loop() {
  char c = ' ';
  HMSlave.begin(BAUD);
  DHT.read11(pinoDHT11);

  // Teste
  HMSlave.print(DHT.humidity);
  HMSlave.print(" / ");
  HMSlave.println(DHT.temperature);

  // Reenviando a temperatura e umidade a cada 3s
  delay(3000);
}

/*
  //Passos para configurar o escravo
  void setupSlave() {
  HMSlave.begin(BAUD);
  HMSlave.println("AT+ROLE0");
  HMSlave.println("AT+RESET");
  HMSlave.println("AT+IMME0");
  }
*/
