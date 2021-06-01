#include<Servo.h>

int DNUM,FNUM,LNUM,RNUM;

Servo Fservo,Dservo,Lservo,Rservo;

void setup() {

  Serial.begin(9600);
  Serial.println("MeArm begin ready to go !!!");
  //Serial.println("input your consel :");
  Fservo.attach(10);
  Fservo.write(0);
  FNUM = 0;
  delay(100);
  Rservo.attach(9);
  Rservo.write(135);
  RNUM = 135;
  delay(100);
  Lservo.attach(6);
  Lservo.write(135);
  LNUM = 135;
  delay(100);
  Dservo.attach(3);
  Dservo.write(90);
  DNUM = 90;
  delay(100);
  
}

void loop() {
  // put your main code here, to run repeatedly:
  if (Serial.available() > 0 ){
    
    char arm = Serial.read();
    int pos = Serial.parseInt();
    
    switch(arm){
      case 'f':
      {
        if (pos > FNUM){
          for (int i = FNUM;i<= pos;i++){
            Fservo.write(i);
            delay(20);
          }
        }
        if (pos < FNUM){
          for (int i = FNUM;i>= pos;i--){
            Fservo.write(i);
            delay(20);
          }
        }
        FNUM = pos;
        Serial.print("Forward Servo turn :");
        Serial.println(pos);
        delay(10);
      }
        break;
      case 'd':
      {
        if (pos > DNUM){
          for (int i = DNUM;i<= pos;i++){
            Dservo.write(i);
            delay(20);
          }
        }
        if (pos < DNUM){
          for (int i = DNUM;i>= pos;i--){
            Dservo.write(i);
            delay(20);
          }
        }
        DNUM = pos;
        Serial.print("Down Servo turn :");
        Serial.println(pos);
        delay(10);
      }
        break;
      case 'l':
      {
        if (pos > LNUM){
          for (int i = LNUM;i<= pos;i++){
            Lservo.write(i);
            delay(20);
          }
        }
        if (pos < LNUM){
          for (int i = LNUM;i>= pos;i--){
            Lservo.write(i);
            delay(20);
          }
        }
        LNUM = pos;
        Serial.print("Left Servo turn :");
        Serial.println(pos);
        delay(10);
      }
        break;
      case 'r':
      {
        if (pos > RNUM){
          for (int i = RNUM;i<= pos;i++){
            Rservo.write(i);
            delay(20);
          }
        }
        if (pos < RNUM){
          for (int i = RNUM;i>= pos;i--){
            Rservo.write(i);
            delay(20);
          }
        }
        RNUM = pos;
        Serial.print("Right Servo turn :");
        Serial.println(pos);
        delay(10);
      }
        break;
    }
    delay(1000);
  }
}
