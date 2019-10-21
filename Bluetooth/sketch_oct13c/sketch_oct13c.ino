#include <SoftwareSerial.h>
SoftwareSerial HMSlave(7, 8); // RX = 2, TX = 3
SoftwareSerial HMMaster(11, 12); // RX = 2, TX = 3
int BAUD = 9600;
String inData = "";
void setup()
{
  Serial.begin(BAUD);
  Serial.println("HM10 serial started at 9600");
  //setupSlave();
  //setupMaster();
  
}



//Passos para configurar o escravo
void setupSlave(){
  HMSlave.begin(BAUD);
  HMSlave.println("AT+ROLE0");
  HMSlave.println("AT+IMME0");
} 


//Passos para configurar o mestre
void setupMaster(){
  HMMaster.begin(BAUD);
  HMMaster.println("AT+ROLE1");
  HMMaster.println("AT+RESET");
  HMMaster.println("AT+IMME0");
  HMMaster.println("AT+CONN00:15:87:20:B1:C1");
}



void loop(){
  loopTestm();
  
//  for (int i = 0; i < 100; i++){
//    HMSlave.begin(BAUD);  
//    HMSlave.print("T: ");
//    HMSlave.print(i+27);
//    HMSlave.print(", U:");
//    HMSlave.print(i+25);  
//    HMMaster.begin(BAUD);
//    while (HMMaster.available() > 0)
//      Serial.print((char)HMMaster.read());
//    Serial.println();
//  }
  
}



boolean t = true;
//void loop(){
//  char c;
//  if (t){
//    Serial.println("Digite (M)estre ou (E)scravo");
//    t = false;
//  }
//  if (Serial.available()){
//    c = Serial.read();
//    if (c == 'M' )
//      loopTestm();
//    else
//      loopTest();
//    t = true;
//  }
//}




////  long int p [9]= {9600, 19200, 38400, 57600, 115200, 4800, 2400, 1200, 230400};
//


void loopTestm() {  
  char c;
  HMMaster.begin(BAUD);  
  while (c!= 'X'){
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







void loopTest() {  
  char c;
  HMSlave.begin(BAUD);
  while (c!= 'X'){
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







//void loop()
//{
//  HM10.listen();  // listen the HM10 port
//  while (HM10.available() > 0) {   // if HM10 sends something then read
////    Serial.println("Lendo do HM10");
//    appData = HM10.read();
//    inData = String(appData);  // save the data in string format
//    Serial.write(appData);
//  }
//
//  
//  if (Serial.available()) {           // Read user input if available.
//    delay(10);
////    Serial.println("Escrevendo para o HM10");
//    HM10.write(Serial.read());
//  }
//}
