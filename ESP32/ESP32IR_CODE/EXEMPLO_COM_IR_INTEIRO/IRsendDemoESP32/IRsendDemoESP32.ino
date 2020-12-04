/* IRremoteESP8266: IRsendDemo - demonstrates sending IR codes with IRsend.
 *
 * Version 1.1 January, 2019
 * Based on Ken Shirriff's IrsendDemo Version 0.1 July, 2009,
 * Copyright 2009 Ken Shirriff, http://arcfn.com
 *
 * An IR LED circuit *MUST* be connected to the ESP8266 on a pin
 * as specified by kIrLed below.
 *
 * TL;DR: The IR LED needs to be driven by a transistor for a good result.
 *
 * Suggested circuit:
 *     https://github.com/crankyoldgit/IRremoteESP8266/wiki#ir-sending
 *
 * Common mistakes & tips:
 *   * Don't just connect the IR LED directly to the pin, it won't
 *     have enough current to drive the IR LED effectively.
 *   * Make sure you have the IR LED polarity correct.
 *     See: https://learn.sparkfun.com/tutorials/polarity/diode-and-led-polarity
 *   * Typical digital camera/phones can be used to see if the IR LED is flashed.
 *     Replace the IR LED with a normal LED if you don't have a digital camera
 *     when debugging.
 *   * Avoid using the following pins unless you really know what you are doing:
 *     * Pin 0/D3: Can interfere with the boot/program mode & support circuits.
 *     * Pin 1/TX/TXD0: Any serial transmissions from the ESP8266 will interfere.
 *     * Pin 3/RX/RXD0: Any serial transmissions to the ESP8266 will interfere.
 *   * ESP-01 modules are tricky. We suggest you use a module with more GPIOs
 *     for your first time. e.g. ESP-12 etc.
 */

#include <Arduino.h>
#include <IRremoteESP8266.h>
#include <IRsend.h>

const uint16_t kIrLed = 13;  // ESP8266 GPIO pin to use. Recommended: 4 (D2).

IRsend irsend(kIrLed);  // Set the GPIO to be used to sending the message.

// Example of data captured by IRrecvDumpV2.ino
//uint16_t rawData[63] = {2644, 890,  432, 454,  430, 454,  432, 892,  432, 894,  1318, 890,
//              432, 454,  430, 454,  430, 454,  430, 454,  430, 454,  430, 454, 
//              430, 454,  874, 452,  430, 454,  430, 894,  432, 456,  428, 480,  
//              404, 480,  872, 870,  898, 870,  454, 456,  872, 454,  428, 872,  
//              454, 456,  428, 456,  428, 456,  428, 456,  872, 456,  428, 872,  
//              454, 456,  428};
//uint16_t rawData[35] = {460, 282,  158, 280,  158, 586,  184, 420,  184, 282,  158, 612,  158, 282,  158, 280,  158, 282,  158, 610,  158, 588,  206, 424,  182, 562,  208, 258,  156, 284,  156, 768,  176, 274,  180};  // RCMM 2480A60C

uint16_t rawData0[71] = {9046, 4474,  578, 556,  576, 556,  576, 1676,  578, 556,  576, 558,  574, 558,  576, 556,  576, 556,  576, 1676,  578, 1674,  578, 556,  576, 1674,  576, 1676,  578, 1676,  578, 1676,  576, 1676,  578, 558,  576, 556,  576, 558,  574, 1676,  578, 558,  574, 580,  552, 580,  552, 558,  574, 1676,  576, 1676,  576, 1676,  578, 558,  574, 1676,  578, 1674,  578, 1676,  578, 1674,  578, 39918,  9018, 2236,  578};  // NEC 20DF10EF
uint16_t rawData1[71] = {9044, 4476,  580, 552,  582, 550,  578, 1674,  578, 554,  602, 532,  602, 530,  600, 532,  602, 530,  602, 1650,  602, 1650,  604, 530,  602, 1648,  604, 1648,  606, 1646,  604, 1648,  602, 1650,  604, 532,  602, 530,  602, 530,  600, 1650,  604, 530,  602, 530,  600, 554,  578, 532,  600, 1648,  602, 1650,  604, 1650,  602, 554,  580, 1648,  602, 1650,  602, 1650,  602, 1650,  602, 39886,  9042, 2212,  604};  // NEC 20DF10EF
void setup() {
  irsend.begin();
  Serial.begin(115200);

}

void loop() {
  Serial.println("a rawData capture from IRrecvDumpV2");
  irsend.sendRaw(rawData1, 71, 32);  // Send a raw data capture at 38kHz.

  delay(1000);
   // irsend.sendRaw(rawData0, 71, 38);  // Send a raw data capture at 38kHz.

}
