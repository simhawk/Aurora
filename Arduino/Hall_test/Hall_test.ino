int hallPin_0 = 0;
int hallPin_1 = 1;
int hallPin_2 = 2;

int hallVal_0 = 0;
int hallVal_1 = 0;
int hallVal_2 = 0;
#define ROLLING_AVG_COUNT 5
void setup()
{

  Serial.begin(9600);              //  setup serial
  pinMode(0, INPUT);
  pinMode(1, INPUT);
  pinMode(2, INPUT);
}

int G0[ROLLING_AVG_COUNT];
int G1[ROLLING_AVG_COUNT];
int G2[ROLLING_AVG_COUNT];

int count = 0;
bool full = false;
void loop()
{
  hallVal_0 = analogRead(hallPin_0);
  hallVal_1 = analogRead(hallPin_1);
  hallVal_2 = analogRead(hallPin_2);

  G0[count] = hallVal_0;
  G1[count] = hallVal_1;
  G2[count] = hallVal_2;
  count++;

  if (!full) {
    if (count == ROLLING_AVG_COUNT) {
      full = true;
    }
  }

  count = count % ROLLING_AVG_COUNT;

  if (full) {
    // smoothing
    int total0 = 0;
    int total1 = 0;
    int total2 = 0;
    
    for (int i = 0; i < ROLLING_AVG_COUNT; i++) {
      total0 += G0[i];
      total1 += G1[i];
      total2 += G2[i];
    }
    double average0 =  double(total0) / ROLLING_AVG_COUNT;
    double average1 =  double(total1) / ROLLING_AVG_COUNT;
    double average2 =  double(total2) / ROLLING_AVG_COUNT;
   
    Serial.print(average0);
    Serial.print(",");   
    Serial.print(average1);
    Serial.print(",");   
    Serial.println(average2);
    

  }
}
