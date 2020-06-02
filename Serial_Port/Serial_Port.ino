#include <Wire.h> 
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd(0x27,16,2);  // set the LCD address to 0x27 for a 16 chars and 2 line display

char rx_byte;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  lcd.init();
  lcd.backlight();
  lcd.setCursor(0,0);
  lcd.print("CPU: ");
  lcd.setCursor(0,1);
  lcd.print("Memory: ");
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0){
    lcd.setCursor(5,0);
    for(int i = 0; i < 4; i++){
      rx_byte = Serial.read();
      lcd.print(rx_byte);  
    }
    lcd.print("%");
    
    lcd.setCursor(8,1);
    while(Serial.available()>0){
      rx_byte = Serial.read();
      lcd.print(rx_byte);  
    }
    lcd.print(" MB");
  }
}
