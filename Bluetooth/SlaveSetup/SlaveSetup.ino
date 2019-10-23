#include <SoftwareSerial.h>
SoftwareSerial HMSlave(12, 11); // RX = 2, TX = 3
int BAUD = 9600;
String inData = "";
void setup()
{
  Serial.begin(BAUD);
  Serial.println("HM10 serial started at 9600");
}

//Passos para configurar o escravo
void setupSlave() {
  HMSlave.begin(BAUD);
  HMSlave.println("AT+ROLE0");
  HMSlave.println("AT+RESET");
  HMSlave.println("AT+IMME0");
}


void loop() {  
  char c = ' ';
  HMSlave.begin(BAUD);
  while (c != 'X') {
    if (Serial.available()) {
      c = Serial.read();
      HMSlave.print(c);
    }
    if (HMSlave.available()) {
      c = HMSlave.read();
      Serial.print(c);
    }
  }
}
