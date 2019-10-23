#include <SoftwareSerial.h>
SoftwareSerial HMSlave(12, 11); // RX = 2, TX = 3
SoftwareSerial HMMaster(12, 11); // RX = 2, TX = 3
int BAUD = 9600;

void setup()
{
  Serial.begin(BAUD);
  Serial.println("HM10 serial started at 9600");
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
