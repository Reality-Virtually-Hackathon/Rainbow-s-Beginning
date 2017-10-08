/*! 
    This code pulls temperature/humidity readings and RFID readings
    for a connected IOT unit installed in public places. The information
    is sent via serial to an internet connected device and can be published as
    an API to internet connected devices. 

    For demo purposes, the serial connects diretcly to Unity to send the information
    for A/R IOT connection.
    
    This code is pulled from 2 Arduino examples. The first is a Temperature
    Humidity Sensor Demo from LadyAda at Adafruit that tests different
    DHT temperature sensors and prints results. 

    The second is the PN532 specification example code to test out the
    RFID/NFC PN532 Model.   
    This example will attempt to connect to an ISO14443A
    card or tag and retrieve some basic information about it
    that can be used to determine what type of card it is.   
    Note that you need the baud rate to be 115200 because we need to print
    out the data and read from the card at the same time!    

    Both Pieces of code are in the public domain.
*/

//Pin Definition
int speakerPin = 9;
int ledPin = 11;

//Stuff for Temp/Humidity Sensor----------------------------
#include "DHT.h"
#define DHTPIN A0     // what digital pin we're connected to
#define DHTTYPE DHT11   // DHT 11

//NFC STUFF
#if 0
  #include <SPI.h>
  #include <PN532_SPI.h>
  #include "PN532.h"

  PN532_SPI pn532spi(SPI, 10);
  PN532 nfc(pn532spi);
#elif 0
  #include <PN532_HSU.h>
  #include <PN532.h>
      
  PN532_HSU pn532hsu(Serial);
  PN532 nfc(pn532hsu);
#else 
  #include <Wire.h>
  #include <PN532_I2C.h>
  #include <PN532.h>
  #include <NfcAdapter.h>
  
  PN532_I2C pn532i2c(Wire);
  PN532 nfc(pn532i2c);
#endif

//Initiate DHT Device by type and pin
DHT dht(DHTPIN, DHTTYPE);

void setup() {
//Set Up Pins
pinMode(speakerPin, OUTPUT);
pinMode(ledPin, OUTPUT);
digitalWrite(ledPin, HIGH);  

Serial.begin(115200);
dht.begin();
nfc.begin();

// Set the max number of retry attempts to read from a card
  // This prevents us from waiting forever for a card, which is
  // the default behaviour of the PN532.
  nfc.setPassiveActivationRetries(0xFF);
  
  // configure board to read NFC tags
  nfc.SAMConfig();
}

void loop() {
//DHT variables
  float h = dht.readHumidity(); //get humidity in %
  float f = dht.readTemperature(true); //gets temp in F

//NFC Variables
  boolean success;
  uint8_t uid[] = { 0, 0, 0, 0, 0, 0, 0 };  // Buffer to store the returned UID
  uint8_t uidLength;                        // Length of the UID (4 or 7 bytes depending on ISO14443A card type)
//If an NFC Card is also Found...
  // Wait for an ISO14443A type cards (Mifare, etc.).  When one is found
  // 'uid' will be populated with the UID, and uidLength will indicate
  // if the uid is 4 bytes (Mifare Classic) or 7 bytes (Mifare Ultralight)
  success = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, &uid[0], &uidLength);

//If a card is successfully found
 if (success) {
 tone(speakerPin, 261, 300);    
 if (uid[1] == 117) {
      //Flush the serial then print "h,f" for parsing in Unity
        Serial.flush();
        Serial.print(h);
        Serial.print(",");
        Serial.print(f);
        Serial.print(",1");
        Serial.println();
    } else if (uid[1] == 1) {
        Serial.flush();
        Serial.print(h);
        Serial.print(",");
        Serial.print(f);
        Serial.print(",2");
        Serial.println();
    } else if (uid[1] == 175) {
        Serial.flush();
        Serial.print(h);
        Serial.print(",");
        Serial.print(f);
        Serial.print(",3");
        Serial.println();
    } else if (uid[1] == 13) {
        Serial.flush();
        Serial.print(h);
        Serial.print(",");
        Serial.print(f);
        Serial.print(",1");
        Serial.print(",4");
        Serial.println();
    }
    } else {
      // PN532 probably timed out waiting for a card
    }  
}
