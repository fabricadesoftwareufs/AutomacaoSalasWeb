#include <SoftwareSerial.h>
SoftwareSerial HM10(19, 18); // RX and TX
char appData;  
String inData = "";
void setup()
{
  Serial.begin(9600);
  Serial.println("HM10 serial started at 9600");
  HM10.begin(9600); // set HM10 serial at 9600 baud rate
}

void loop()
{
  HM10.listen();  // listen the HM10 port 
  if (Serial.available()) {           // Read user input if available.
    delay(10);
    HM10.write(Serial.read());
  }
}
