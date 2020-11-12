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
  irsend.sendSony(0xC160E044,24);
  delay(1000);

  // Power ON / OFF
  irsend.sendSony(0x25E745EA,32);
  delay(2000);

  // 20º
  irsend.sendSony(0xC160C044,32);
  delay(1000);
  
}
