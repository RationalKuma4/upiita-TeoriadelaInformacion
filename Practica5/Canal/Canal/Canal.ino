/*
 Name:		Canal.ino
 Created:	12/05/2019 19:13:08
 Author:	Anselmo-PC
*/

#define C 8
#define E1 9
#define E2 10
#define TX 11

int c, e1, e2, tx;
int codigo[8]{ 0,0,0,0,0,0,0,0 };

void TransmisionSerial(int code[8]);

void setup()
{
	pinMode(C, INPUT);
	pinMode(E1, INPUT);
	pinMode(E2, INPUT);
	pinMode(TX, OUTPUT);

	Serial.begin(9600);
	Serial.println("Puerto abierto");
}

void loop()
{
	for (auto i = 8; i > 0; i--)
	{
		delay(1000);
		codigo[i] = digitalRead(C);
	}

	delay(500);

	if (digitalRead(E1))
	{
		if (codigo[0] == 1)
			codigo[0] = 0;
		else
			codigo[0] = 1;

		if (codigo[2] == 1)
			codigo[2] = 0;
		else
			codigo[2] = 1;
	}
	else if (digitalRead(E2))
	{
		if (codigo[1] == 1)
			codigo[1] = 0;
		else
			codigo[1] = 1;

		if (codigo[3] == 1)
			codigo[3] = 0;
		else
			codigo[3] = 1;
	}

	delay(500);
}

void TransmisionSerial(int code[8])
{
	for (auto i = 0; i < 8; i++)
	{
		digitalWrite(TX, code[i]);
		Serial.print(code[i]);
		delay(1000);
	}

	Serial.print("");
	Serial.print("Transmision terminada");
	digitalWrite(TX, 0);
	Serial.print("");
	delay(2000);
}
