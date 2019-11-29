#include <SoftwareSerial.h>
SoftwareSerial HMSlave(12, 11); // RX = 2, TX = 3
SoftwareSerial HMMaster(12, 11); // RX = 2, TX = 3
int BAUD = 9600;

void setup()
{
  Serial.begin(BAUD);
  Serial.println("HM10 serial started at 9600");
  setupMaster();
 }

void setupMaster(){
  HMMaster.begin(BAUD);
  HMMaster.println("AT1");
  HMMaster.println("AT+ROLE1");
  HMMaster.println("AT+RESET");
  HMMaster.println("AT+IMME0");
  HMMaster.println("AT+INQ");
  HMMaster.println("AT+CONN1");
}



void loop() {
  char c = ' ';
  HMMaster.begin(BAUD);
  while (c != 'X') {
    if (Serial.available()) {
      c = Serial.read();
      HMMaster.print(c);
    }
    if (HMMaster.available()) {
      c = HMMaster.read();
      Serial.print(c);
    }
  }
}
