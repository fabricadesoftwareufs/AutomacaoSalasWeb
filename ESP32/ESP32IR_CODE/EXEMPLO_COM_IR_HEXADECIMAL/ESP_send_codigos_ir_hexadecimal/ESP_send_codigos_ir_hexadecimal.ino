#include <IRremote.h> // use the library

IRsend irsend;

void setup()
{
  Serial.begin(9600);
}

void loop() {

  /*
   * Lembrar de colocar 0x na frente do código hexadecimal pra poder enviar
   */
  

  // Power ON / OFF
  irsend.sendNEC(0x2480A60C,32);
  Serial.println("enviou");
  delay(2000);

  
}
