#include <IRremote.h>                       // Biblioteca IRemote
int RECV_PIN = 13;                          // Arduino pino D11 conectado no Receptor IR
IRrecv irrecv(RECV_PIN);                    // criando a instância
decode_results results;                     // declarando os resultados
 
void setup()
{
  Serial.begin(9600);                       // Monitor velocidade 9600 bps
  irrecv.enableIRIn();                      // Inicializa a recepção de códigos
}
 
void loop()
{
  if (irrecv.decode(&results))              // se algum código for recebido
  {
    Serial.println(results.value, HEX);     // imprime o HEX Code
    irrecv.resume();                        // reinicializa o receptor
    delay(10);                              // atraso de 10 ms
  }
}
