// Libreria para operaciones logicas
#include <iso646.h>
// Pins para palabra dato, control de fuente y muestra
// de transmision
#define D1 53
#define D2 51
#define D3 49
#define D4 47

#define CD 45

#define C1 43
// Pins para erro, control de canal y muestra
// de transmision
#define E1 41
#define E2 39
#define E3 37
#define E4 35
#define E5 33
#define E6 31
#define E7 29
#define E8 27

#define CE 25

#define C2 23
// Pins para sindrome
#define S1 52
#define S2 50
#define S3 48
#define S4 46
// Pins para palabra corregida
#define COR1 44
#define COR2 42
#define COR3 40
#define COR4 38
// Pins para canal
#define CCOUT1 36
#define CCIN1 34
// Pins para canal
#define CCOUT2 32
#define CCIN2 30

// Variables globales para codificacion  y decodficacion
int d1, d2, d3, d4, cd, c1, c2;
int ce[8]{ 0,0,0,0,0,0,0,0 };
int error[8]{ 0,0,0,0,0,0,0,0 };
int r[8]{ 0,0,0,0,0,0,0,0 };
int sindrome[4]{ 0,0,0,0 };
int cr[8]{ 0,0,0,0,0,0,0,0 };

// Prototipo de funciones
void ReadPins();
void TransmisionCanalError(int codidgo[8]);
void PatronError();
void AgregaError();
void ObtieneSindrome(int error[8]);
void TransmisionErrorDecoder(int codidgo[8]);
void DecodificaPalabra();

// Funcion de inicio
void setup()
{
	// Asignar un modo a los pines a utilizar
	ReadPins();
	Serial.begin(9600);
	Serial.println("Puerto abierto");
}

// Funcion de proceso principal
void loop()
{
	// Condiciones iniciales
	delay(2000);
	digitalWrite(COR1, 0);
	digitalWrite(COR2, 0);
	digitalWrite(COR3, 0);
	digitalWrite(COR4, 0);
	digitalWrite(S1, 0);
	digitalWrite(S2, 0);
	digitalWrite(S3, 0);
	digitalWrite(S4, 0);

	// Control de fuente
	cd = digitalRead(CD);
	while (cd == 0)
		cd = digitalRead(CD); // Permitir transmision

	// Lectura de palabra dato
	int codigo[8]{ 0,0,0,0,0,0,0,0 };
	d1 = digitalRead(D1);
	d2 = digitalRead(D2);
	d3 = digitalRead(D3);
	d4 = digitalRead(D4);

	// Generacion de palabra codigo
	codigo[0] = d2 ^ d3 ^ d4;
	codigo[1] = d1 ^ d2 ^ d3;
	codigo[2] = d1 ^ d2 ^ d4;
	codigo[3] = d1 ^ d3 ^ d4;
	codigo[4] = d1;
	codigo[5] = d2;
	codigo[6] = d3;
	codigo[7] = d4;

	// Se inicia la transmision de la fuente 
	// al canal
	TransmisionCanalError(codigo);

	// Se prepara el error en el canal y se 
	// controla la transmision
	if (digitalRead(CE))
	{
		// Leer los errores a agregar
		PatronError();
		// Agrega el error a la palabra
		AgregaError();
		// Se transmite la palabra codigo con error al 
		// decoder
		TransmisionErrorDecoder(ce);
		// Se obtiene el sindrome a partir del error 
		// del canal
		ObtieneSindrome(error);
		// Se corrige la palabra recibida
		DecodificaPalabra();
	}
}

// Asignacion de modo en los pines
void ReadPins()
{
	pinMode(D1, INPUT);
	pinMode(D2, INPUT);
	pinMode(D3, INPUT);
	pinMode(D4, INPUT);

	pinMode(CD, INPUT);

	pinMode(C1, OUTPUT);

	pinMode(E1, INPUT);
	pinMode(E2, INPUT);
	pinMode(E3, INPUT);
	pinMode(E4, INPUT);
	pinMode(E5, INPUT);
	pinMode(E6, INPUT);
	pinMode(E7, INPUT);
	pinMode(E8, INPUT);

	pinMode(CE, INPUT);

	pinMode(C2, OUTPUT);

	pinMode(S1, OUTPUT);
	pinMode(S2, OUTPUT);
	pinMode(S3, OUTPUT);
	pinMode(S4, OUTPUT);

	pinMode(COR1, OUTPUT);
	pinMode(COR2, OUTPUT);
	pinMode(COR3, OUTPUT);
	pinMode(COR4, OUTPUT);

	pinMode(CCOUT1, OUTPUT);
	pinMode(CCOUT2, OUTPUT);

	pinMode(CCIN1, INPUT);
	pinMode(CCIN2, INPUT);
}

void TransmisionCanalError(int codidgo[8])
{
	Serial.println("Transmision fuente -> canal");
	// Se inicia la transmision serial
	for (auto i = 0; i < 8; ++i)
	{
		// Se transmite al canal
		digitalWrite(CCOUT1, codidgo[i]);
		// Se lee el dato transmitido al canal
		ce[i] = digitalRead(CCIN1);
		// Se muestra el dato recibido
		digitalWrite(C1, ce[i]);
		Serial.print(ce[i]);
		// Timer
		delay(1000);
	}
	Serial.println();
	Serial.println("Transmision terminada");
	// Se liberan recursos
	digitalWrite(C1, 0);
}

void PatronError()
{
	// Se leen los errores a agregar a
	// la palabra codigo
	error[0] = digitalRead(E1);
	error[1] = digitalRead(E2);
	error[2] = digitalRead(E3);
	error[3] = digitalRead(E4);
	error[4] = digitalRead(E5);
	error[5] = digitalRead(E6);
	error[6] = digitalRead(E7);
	error[7] = digitalRead(E8);
	Serial.println("Patron error");
	/*for (int i = 0; i < 8; ++i)
		Serial.print(error[i]);
	Serial.println();*/
}

void AgregaError()
{
	// Se busca donde fue configurado el error
	// y se agrega a la palabra codigo
	if (error[0]) ce[0] = ce[0] xor 1;
	if (error[1]) ce[1] = ce[1] xor 1;
	if (error[2]) ce[2] = ce[2] xor 1;
	if (error[3]) ce[3] = ce[3] xor 1;
	if (error[4]) ce[4] = ce[4] xor 1;
	if (error[5]) ce[5] = ce[5] xor 1;
	if (error[6]) ce[6] = ce[6] xor 1;
	if (error[7]) ce[7] = ce[7] xor 1;
	/*Serial.println("Codigo con error");
	for (int i = 0; i < 8; ++i)
		Serial.print(ce[i]);
	Serial.println();*/
}

void ObtieneSindrome(int error[8])
{
	// Si el canal dejo de transmitir se cancela la 
	// operacion
	if (!cr[0] && !cr[1] && !cr[2] && !cr[3] && !cr[4] && !cr[5] &&
		!cr[6]) return;

	// Se calcula el sindrome con base en la ecuaciones 
	// obtenidas
	sindrome[0] = error[0] ^ error[5] ^ error[6] ^ error[7];
	sindrome[1] = error[1] ^ error[4] ^ error[5] ^ error[6];
	sindrome[2] = error[2] ^ error[4] ^ error[5] ^ error[7];
	sindrome[3] = error[3] ^ error[4] ^ error[6] ^ error[7];

	// Se muestra el sindrome en el arreglo de leds asignados
	// en el circuito
	digitalWrite(S1, sindrome[0]);
	digitalWrite(S2, sindrome[1]);
	digitalWrite(S3, sindrome[2]);
	digitalWrite(S4, sindrome[3]);

	/*Serial.println("Sindrome");
	for (int i = 0; i < 4; ++i)
		Serial.print(sindrome[i]);
	Serial.println();*/
}

void TransmisionErrorDecoder(int codidgo[8])
{
	// Se inicia la transmision serial
	Serial.println("Transmision error -> decoder");
	for (auto i = 0; i < 8; ++i)
	{
		// Se transmite al canal
		digitalWrite(CCOUT2, codidgo[i]);
		// Se lee el dato transmitido al canal
		Serial.print(digitalRead(CCIN2));
		// Se muestra el dato recibido
		cr[i] = digitalRead(CCIN2);
		digitalWrite(C2, cr[i]);
		// Timer
		delay(1000);
	}
	Serial.println();
	Serial.println("Transmision terminada");
	// Se liberan recursos
	digitalWrite(C2, 0);
}

void DecodificaPalabra()
{
	// Si el sindrome es 0000, significa se interrumpio la 
	// transmision o no se ha transmitido algo, por lo que se cancela 
	// la funcnion
	if (!sindrome[0] && !sindrome[1] && !sindrome[2] && !sindrome[3] && !digitalRead(CE))
		return;

	// Con base la matriz H transouesta se verifican la siguientes condiciones 
	// para corregir errores simples y dobles
	Serial.println("Corrigiendo errror");
	if (sindrome[0] && !sindrome[1] && !sindrome[2] && !sindrome[3]) //1000
	{
		cr[0] = cr[0] xor 1;
	}
	else if (!sindrome[0] && sindrome[1] && !sindrome[2] && !sindrome[3]) //0100
	{
		cr[1] = cr[1] xor 1;
	}
	else if (!sindrome[0] && !sindrome[1] && sindrome[2] && !sindrome[3]) //0010
	{
		cr[2] = cr[2] xor 1;
	}
	else if (!sindrome[0] && !sindrome[1] && !sindrome[2] && sindrome[3]) //0001
	{
		cr[3] = cr[3] xor 1;
	}
	else if (!sindrome[0] && sindrome[1] && sindrome[2] && sindrome[3]) //0111
	{
		cr[4] = cr[4] xor 1;
	}
	else if (sindrome[0] && sindrome[1] && sindrome[2] && !sindrome[3]) //1110
	{
		cr[5] = cr[5] xor 1;
	}
	else if (sindrome[0] && sindrome[1] && !sindrome[2] && sindrome[3]) //1101
	{
		cr[6] = cr[6] xor 1;
	}
	else if (sindrome[0] && !sindrome[1] && sindrome[2] && sindrome[3]) //1011
	{
		cr[7] = cr[7] xor 1;
	}
	else if (!sindrome[0] && sindrome[1] && !sindrome[2] && sindrome[3]) //0101 doble 2,4 
	{
		cr[1] = cr[1] xor 1;
		cr[3] = cr[3] xor 1;
	}
	else if (sindrome[0] && !sindrome[1] && sindrome[2] && !sindrome[3]) //1010 doble 1,3
	{
		cr[0] = cr[0] xor 1;
		cr[2] = cr[2] xor 1;
	}

	for (auto i = 4; i < 8; i++)
	{
		Serial.print(cr[i]);
	}

	// Se muestra la palabra corregida en el arrgle de 
	// leds asignados
	digitalWrite(COR1, cr[4]);
	digitalWrite(COR2, cr[5]);
	digitalWrite(COR3, cr[6]);
	digitalWrite(COR4, cr[7]);
	Serial.println("");
	// Timer
	delay(1000);
}

/*
 * for (int i = 0; i < 8; ++i)
			Serial.print(error[i]);
 */