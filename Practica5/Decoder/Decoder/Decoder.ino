#define CC 2
#define IN 3

#define S1 4
#define S2 5
#define S3 6
#define S4 7

#define D1 8
#define D2 9
#define D3 10
#define D4 11

int cc, in;
int s1, s2, s3, s4;
int d1, d2, d3, d4;
int codigo[8] = { 0,0,0,0,0,0,0,0 };

void ReadPins();
int SumaModuloDos(int a, int b, int c, int d);
void Sindrome(int cod[8]);

void setup()
{
	ReadPins();
	Serial.begin(9600);
	Serial.println("Puerto abierto");
}

void loop()
{
	// Se revisa control de canal
	delay(2000);
	cc = digitalRead(CC);

	while (cc == 0)
		cc = digitalRead(CC);

	// Lectura de canal
	for (auto i = 0; i < 8; ++i)
	{
		codigo[i] = digitalRead(IN);
		Serial.print(codigo[i]);
		delay(1000);
	}

	Serial.println("Recepcion terminada");

	// Sindrome
	Sindrome(codigo);


}

void ReadPins()
{
	pinMode(CC, INPUT);
	pinMode(IN, INPUT);

	pinMode(S1, OUTPUT);
	pinMode(S2, OUTPUT);
	pinMode(S3, OUTPUT);
	pinMode(S4, OUTPUT);

	pinMode(D1, OUTPUT);
	pinMode(D2, OUTPUT);
	pinMode(D3, OUTPUT);
	pinMode(D4, OUTPUT);
}

int SumaModuloDos(int a, int b, int c, int d)
{
	return a ^ b^ c^ d;
}

void Sindrome(int cod[8])
{
	s1 = SumaModuloDos(codigo[0], codigo[5], codigo[6], codigo[7]);
	s2 = SumaModuloDos(codigo[1], codigo[4], codigo[5], codigo[6]);
	s3 = SumaModuloDos(codigo[2], codigo[4], codigo[5], codigo[7]);
	s4 = SumaModuloDos(codigo[3], codigo[4], codigo[6], codigo[7]);

	digitalWrite(S1, s1);
	digitalWrite(S2, s2);
	digitalWrite(S3, s3);
	digitalWrite(S4, s4);
}

/*
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
*/