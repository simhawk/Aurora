int hallPin_0 = 0;
int hallPin_1 = 1;
int hallPin_2 = 2;

int hallVal_0 = 0;
int hallVal_1 = 0;
int hallVal_2 = 0;

void setup()
{

  Serial.begin(9600);              //  setup serial
  pinMode(0, INPUT);
  pinMode(1, INPUT);
  pinMode(2, INPUT);
}

int readings[10];
int count = 0;
bool full = false;
void loop()
{
  hallVal_0 = analogRead(hallPin_0);
  hallVal_1 = analogRead(hallPin_1);
  hallVal_2 = analogRead(hallPin_2);
  //  Serial.print("raw: ");
  //  Serial.print(hallVal_0);
  //  Serial.print(" , ");
  //  Serial.print(hallVal_1);
  //  Serial.print(" , ");
  //  Serial.println(hallVal_2);

  readings[count] = hallVal_1;
  count++;

  if (!full) {
    if (count == 10) {
      full = true;
    }
  }

  count = count % 10;

  if (full) {
    // smoothing
    int total = 0;
    for (int i = 0; i < 10; i++) {
      total += readings[i];
    }
    double average =  total / 10.0;
    Serial.print("Average: ");
    Serial.println(average);

  }
  delay(1);
}
