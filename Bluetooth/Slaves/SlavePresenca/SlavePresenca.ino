#include "dht.h" //INCLUSÃO DE BIBLIOTECA
#include <SoftwareSerial.h>

// ----------------------------- sensor presenca ---------------------------- //
const int pinoLED = 7;
const int pinoPIR = 6;
const int qtdConsideravel = 3; // quantidade de vezes que é considerado ter alguem presente - valor a ser definido
int iPresenca;
bool temGente;
long contadorTempo;
// ------------------------------------------------------------------------- //

//--------------------------- módulo bluetooth ------------------------- //
SoftwareSerial HMSlave(12, 11); // RX = 2, TX = 3
SoftwareSerial HMMaster(12, 11); // RX = 2, TX = 3
int BAUD = 9600;
String inData = "";
char c;
// ------------------------------------------------------------------- //


void setup()
{

    pinMode(pinoLED, OUTPUT);
    pinMode(pinoPIR, INPUT);
    iPresenca = 0;
    temGente = false;

   Serial.begin(BAUD);
   Serial.println("HM10 serial started at 9600");
      HMSlave.begin(BAUD);

}

void loop()
{
    if (Serial.available()) {
      c = Serial.read();
      HMSlave.print(c);
      
    }
    if (HMSlave.available()) {
      c = HMSlave.read();
      Serial.print(c);
    }
    long tempo = millis();
    int valor = digitalRead(pinoPIR);
    if (valor == HIGH) {
        iPresenca++;
        if (iPresenca == qtdConsideravel) {
            temGente = true;
           HMSlave.println("Tem gente!");
           Serial.println("Tem gente!");

            temGente = false;
            iPresenca = 0;
    	}
       //HMSlave.println("detectado");
    	Serial.println("detectado");
    	digitalWrite(pinoLED, HIGH);
    }
    else {
       iPresenca = 0;
       temGente = false;
       // HMSlave.println("não detectado");
       Serial.println("não detectado");
       digitalWrite(pinoLED, LOW);
    }
    delay(2000); // intervalo para verificar presenca
}
