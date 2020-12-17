 #include "EmonLib.h"

EnergyMonitor SCT013;

int pinSCT = 13;   //Pino analógico conectado ao SCT-013

int tensao = 127;
int potencia; 

void setup()
{
    SCT013.current(pinSCT, 6.0606);

    Serial.begin(115200);
}

void loop()
{
    double Irms = SCT013.calcIrms(1480);   // Calcula o valor da Corrente
    
    potencia = Irms * tensao;          // Calcula o valor da Potencia Instantanea    

    Serial.print("Corrente = ");
    Serial.print(Irms);
    Serial.println(" A");
    
    Serial.print("Potencia = ");
    Serial.print(potencia);
    Serial.println(" W");
   
    delay(500);

    Serial.print(".");
    delay(500);
    Serial.print(".");
    delay(500);
    Serial.println(".");
    delay(500);
}
