#define D1 53
#define D2 51
#define D3 49
#define D4 47

#define CD 45

#define C1 43

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

#define S1 52
#define S2 50
#define S3 48
#define S4 46

#define COR1 44
#define COR2 42
#define COR3 40
#define COR4 38

#define CC1 36
#define CC2 34
#include <iso646.h>

int d1, d2, d3, d4, cd, c1, c2;
int e1, e2, e3, e4, e5, e6, e7, e8;
int dato[8]{ 0,0,0,0,0,0,0,0 };
int error[8]{ 0,0,0,0,0,0,0,0 };
int sindrome[4]{ 0,0,0,0 };

int s1, s2, s3, s4;

void ReadPins();
int SumaModuloDos(int a, int b, int c);
void TransmisionSerial(int codigo[8], int salida);
int NormalizaInversion(int dato);

int SumaModuloDos(int a, int b, int c, int d);
void Sindrome(int cod[8]);

void setup()
{
	ReadPins();
	Serial.begin(9600);
	Serial.println("Puerto abierto");
}

void CorreccionError(int datoError[8]);

void loop()
{
	delay(2000);
	cd = digitalRead(CD);

	while (cd == 0)
		cd = digitalRead(CD);

	d1 = digitalRead(D1);
	d2 = digitalRead(D2);
	d3 = digitalRead(D3);
	d4 = digitalRead(D4);

	dato[0] = SumaModuloDos(d2, d3, d4);
	dato[1] = SumaModuloDos(d1, d2, d3);
	dato[2] = SumaModuloDos(d1, d2, d4);
	dato[3] = SumaModuloDos(d1, d3, d4);
	dato[4] = d1;
	dato[5] = d2;
	dato[6] = d3;
	dato[7] = d4;

	delay(1000);
	TransmisionSerial(dato, C1);

	// Agregar error en el canal
	if (digitalRead(CE))
	{
		// Agregar error
		error[0] = e1 = digitalRead(E1);
		error[1] = e2 = digitalRead(E2);
		error[2] = e3 = digitalRead(E3);
		error[3] = e4 = digitalRead(E4);
		error[4] = e5 = digitalRead(E5);
		error[5] = e6 = digitalRead(E6);
		error[5] = e7 = digitalRead(E7);
		error[7] = e8 = digitalRead(E8);

		if (e1) dato[0] = NormalizaInversion(dato[0]);
		if (e2) dato[1] = NormalizaInversion(dato[1]);
		if (e3) dato[2] = NormalizaInversion(dato[2]);
		if (e4) dato[3] = NormalizaInversion(dato[3]);
		if (e5) dato[4] = NormalizaInversion(dato[4]);
		if (e6) dato[5] = NormalizaInversion(dato[5]);
		if (e7) dato[6] = NormalizaInversion(dato[6]);
		if (e8) dato[7] = NormalizaInversion(dato[7]);

		TransmisionSerial(dato, C2);

		// Sindrome
		Sindrome(error);

		// Correccion Error
		CorreccionError(dato);

	}

}

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
}

int SumaModuloDos(int a, int b, int c)
{
	return a ^ b^ c;
}

int SumaModuloDos(int a, int b, int c, int d)
{
	return a ^ b^ c^ d;
}

void TransmisionSerial(int codigo[8], int salida)
{
	Serial.println("");
	for (auto i = 0; i < 8; i++)
	{
		digitalWrite(salida, codigo[i]);
		Serial.print(codigo[i]);
		delay(1000);
	}

	Serial.println("");
	Serial.println("Transmision terminada");
	digitalWrite(salida, 0);
}

int NormalizaInversion(int dato)
{
	int inverso;
	if (dato)inverso = 0;
	else inverso = 1;
	return inverso;
}

void Sindrome(int cod[8])
{
	Serial.println("Sindrome");

	sindrome[0] = s1 = SumaModuloDos(cod[0], cod[5], cod[6], cod[7]);
	sindrome[1] = s2 = SumaModuloDos(cod[1], cod[4], cod[5], cod[6]);
	sindrome[2] = s3 = SumaModuloDos(cod[2], cod[4], cod[5], cod[7]);
	sindrome[3] = s4 = SumaModuloDos(cod[3], cod[4], cod[6], cod[7]);

	Serial.print(s1);
	Serial.print(s2);
	Serial.print(s3);
	Serial.print(s4);

	digitalWrite(S1, s1);
	digitalWrite(S2, s2);
	digitalWrite(S3, s3);
	digitalWrite(S4, s4);
	Serial.println("");
}

void CorreccionError(int datoError[8])
{
	Serial.println("Corrigiendo errror");
	if (sindrome[0] && !sindrome[1] && !sindrome[2] && !sindrome[3]) //1000
	{
		dato[0] xor 1;
	}
	else if (!sindrome[0] && sindrome[1] && !sindrome[2] && !sindrome[3]) //0100
	{
		dato[1] xor 1;
	}
	else if (!sindrome[0] && !sindrome[1] && sindrome[2] && !sindrome[3]) //0010
	{
		dato[2] xor 1;
	}
	else if (!sindrome[0] && !sindrome[1] && !sindrome[2] && sindrome[3]) //0001
	{
		dato[3] xor 1;
	}
	else if (!sindrome[0] && sindrome[1] && sindrome[2] && sindrome[3]) //0111
	{
		dato[4] xor 1;
	}
	else if (sindrome[0] && sindrome[1] && sindrome[2] && !sindrome[3]) //1110
	{
		dato[5] xor 1;
	}
	else if (sindrome[0] && sindrome[1] && !sindrome[2] && sindrome[3]) //1101
	{
		dato[6] xor 1;
	}
	else if (sindrome[0] && !sindrome[1] && sindrome[2] && sindrome[3]) //1011
	{
		dato[7] xor 1;
	}
	else if (!sindrome[0] && sindrome[1] && !sindrome[2] && sindrome[3]) //0101 doble 2,4 
	{
		dato[1] xor 1;
		dato[3] xor 1;
	}
	else if (sindrome[0] && !sindrome[1] && sindrome[2] && !sindrome[3]) //1010 doble 1,3
	{
		dato[0] xor 1;
		dato[2] xor 1;
	}

	for (auto i = 4; i < 8; i++)
	{
		Serial.print(dato[i]);
		delay(1000);
	}

	digitalWrite(COR1, dato[4]);
	digitalWrite(COR2, dato[5]);
	digitalWrite(COR3, dato[6]);
	digitalWrite(COR4, dato[7]);

	Serial.println("");
}

/*
#define C5 8
#define C6 9
#define C7 10
#define C8 11
#define CT 12
#define TX 13

int c5, c6, c7, c8, ct, tx;
int dato[8]{ 0,0,0,0,0,0,0,0 };

void TransmisionSerial(int dato[8]);
int SumaModuloDos(int a, int b);

void setup()
{
	pinMode(C5, INPUT);
	pinMode(C6, INPUT);
	pinMode(C7, INPUT);
	pinMode(C8, INPUT);
	pinMode(CT, INPUT);
	pinMode(TX, OUTPUT);

	Serial.begin(9600);
	Serial.println("Puerto abierto");
}

void loop()
{
	delay(2000);
	ct = digitalRead(CT);

	while (ct == 0)
	{
		ct = digitalRead(CT);
	}

	c5 = digitalRead(C5);
	c6 = digitalRead(C6);
	c7 = digitalRead(C7);
	c8 = digitalRead(C8);

	// Codigo
	dato[0] = SumaModuloDos(SumaModuloDos(c6, c7), c8);
	dato[1] = SumaModuloDos(SumaModuloDos(c5, c6), c7);
	dato[2] = SumaModuloDos(SumaModuloDos(c5, c6), c8);
	dato[3] = SumaModuloDos(SumaModuloDos(c5, c7), c8);
	dato[4] = c5;
	dato[5] = c6;
	dato[6] = c7;
	dato[7] = c8;

	for (auto i = 0; i < 8; i++)
	{
		Serial.print(dato[i]);
	}
	Serial.println("");
	delay(10000);

	if (ct == 1)
	{
		for (auto i = 0; i < 8; i++)
		{
			Serial.print(dato[i]);
		}
		Serial.println("");
		TransmisionSerial(dato);
	}
}

void TransmisionSerial(int dato[8])
{
	for (auto i = 0; i < 8; i++)
	{
		digitalWrite(TX, dato[i]);
		Serial.print(dato[i]);
		delay(1000);
	}

	Serial.print("");
	Serial.print("Transmision terminada");
	digitalWrite(TX, 0);
	Serial.print("");
	delay(2000);
}

int SumaModuloDos(int a, int b)
{
	int res;
	if (a == 0 && b == 0) res = 0;
	else if (a == 0 && b == 1) res = 1;
	else if (a == 1 && b == 0) res = 1;
	else res = 0;
	return res;
}
*/