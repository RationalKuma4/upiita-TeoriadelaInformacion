/*
 Name:		Decoder.ino
 Created:	12/05/2019 19:13:53
 Author:	Anselmo-PC
*/

#define IN 53
#define S1 52
#define S2 50
#define S3 48
#define S4 46
#define E1 44
#define E2 42
#define E3 40
#define E4 38
#define E5 36
#define E6 34
#define E7 32
#define E8 30
#define D1 29
#define D2 27
#define D3 25
#define D4 23
#include <iso646.h>

int in, s1, s2, s3, s4, e1, e2, e3, e4,
e5, e6, e7, e8, d1, d2, d3, d4;
int r[8]{ 0,0,0,0,0,0,0,0 };

void RecepcionMensaje();
void Sindrome();
int SumaModuloDos(int a, int b);

void setup()
{
	pinMode(IN, INPUT);
	pinMode(S1, OUTPUT);
	pinMode(S2, OUTPUT);
	pinMode(S3, OUTPUT);
	pinMode(S4, OUTPUT);
	pinMode(E1, OUTPUT);
	pinMode(E2, OUTPUT);
	pinMode(E3, OUTPUT);
	pinMode(E4, OUTPUT);
	pinMode(E5, OUTPUT);
	pinMode(E6, OUTPUT);
	pinMode(E7, OUTPUT);
	pinMode(E8, OUTPUT);
	pinMode(D1, OUTPUT);
	pinMode(D2, OUTPUT);
	pinMode(D3, OUTPUT);
	pinMode(D4, OUTPUT);

	Serial.begin(9600);
	Serial.println("Puerto abierto");
}

void loop()
{
	RecepcionMensaje();
	delay(500);
	Sindrome();

	// Correccion de errores
	int c1c = ((not s1) and s2 and s3 and s4) xor palabraCe[0];
	int c2c = (s1 and s2 and s3 and (not s4)) xor palabraCe[1];
	int c3c = (s1 and s2 and (not s3) and s4) xor palabraCe[2];
	int c4c = (s1 and (not s2) and s3 and s4) xor palabraCe[3];

}

void RecepcionMensaje()
{
	for (auto i = 8; i > 0; i--)
	{
		delay(1000);
		r[i] = digitalRead(IN);
	}
}

void Sindrome()
{
	s1 = SumaModuloDos(SumaModuloDos(SumaModuloDos(r[0], r[5]), r[6]), r[7]);
	s2 = SumaModuloDos(SumaModuloDos(SumaModuloDos(r[1], r[4]), r[5]), r[6]);
	s3 = SumaModuloDos(SumaModuloDos(SumaModuloDos(r[2], r[4]), r[5]), r[7]);
	s4 = SumaModuloDos(SumaModuloDos(SumaModuloDos(r[3], r[4]), r[6]), r[7]);

	digitalWrite(S1, s1);
	digitalWrite(S2, s2);
	digitalWrite(S3, s3);
	digitalWrite(S4, s4);
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
