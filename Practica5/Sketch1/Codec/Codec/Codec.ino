/*
 Name:		Codec.ino
 Created:	28/04/2019 17:12:56
 Author:	Anselmo-LT
*/

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
	dato[2] = SumaModuloDos(SumaModuloDos(c5, c7), c8);
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

	/*if (ct == 1)
	{
		for (auto i = 0; i < 8; i++)
		{
			Serial.print(dato[i]);
		}
		Serial.println("");
		TransmisionSerial(dato);
	}*/
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