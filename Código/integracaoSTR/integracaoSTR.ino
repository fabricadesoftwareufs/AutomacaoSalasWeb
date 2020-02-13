#include "dht.h" //INCLUSÃO DE BIBLIOTECA
// ------------------------- sensor temperatura/umidade ------------------------ //
const int pinoDHT11 = A0; //PINO ANALÓGICO UTILIZADO PELO DHT11
dht DHT; //VARIÁVEL DO TIPO DHT
// --------------------------------------------------------------------------- //

// ----------------------------- sensor presenca ---------------------------- //
const int pinoLED = 7;
const int pinoPIR = 6;
const int qtdConsideravel = 3; // quantidade de vezes que é considerado ter alguem presente - valor a ser definido
int iPresenca;
bool temGente;
long contadorTempo;
// ------------------------------------------------------------------------- //

// -------------------------- sensor luminosidade ------------------------- //
int limite = 70;
const int pinoSL = A2;
// ---------------------------------------------------------------------- //

//--------------------------- módulo bluetooth ------------------------- //
const int pinoRX = 0;
const int pinoTX = 1;
char BT_input = ' '; // armazena o input recebido via blutooth. O nome do dvc bt é "BT05"
// ------------------------------------------------------------------- //

void setup()
{
    pinMode(pinoLED, OUTPUT);
    pinMode(pinoPIR, INPUT);
    pinMode(pinoSL, INPUT);
    pinMode(pinoDHT11, INPUT);
    pinMode(pinoRX, INPUT);
    pinMode(pinoTX, INPUT);
    iPresenca = 0;
    temGente = false;
    Serial.begin(9600);
}

void loop()
{
    long tempo = millis();
    int valor = digitalRead(pinoPIR);
    if (Serial.available()) {
        BT_input = Serial.read();
        Serial.println(BT_input); //imprime o comando passado pelo app arduino bluetooth control
    }
    DHT.read11(pinoDHT11); //captura as informações do sensor
    Serial.print("Umidade: ");
    Serial.print(DHT.humidity);
    Serial.print("%");
    Serial.print(" / Temperatura: ");
    Serial.print(DHT.temperature, 0);
    Serial.println("*C");

    int sensorValor = analogRead(pinoSL); //sensor luz
    if (sensorValor < limite)
        Serial.println("Apagado");
    else
        Serial.println("Aceso");

    if (valor == HIGH) {
        iPresenca++;
        if (iPresenca == qtdConsideravel) {
            temGente = true;
            Serial.println("Tem gente!");
            temGente = false;
            iPresenca = 0;
        }

        Serial.println("detectado");
        digitalWrite(pinoLED, HIGH);
    }
    else {
        iPresenca = 0;
        temGente = false;
        Serial.println("não detectado");
        digitalWrite(pinoLED, LOW);
    }

    delay(1500); // intervalo para verificar presenca
}
