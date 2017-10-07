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

//Stuff for Temp/Humidity Sensor----------------------------
#include "DHT.h"

#define DHTPIN A0     // what digital pin we're connected to

// Uncomment whatever type you're using!
#define DHTTYPE DHT11   // DHT 11
//#define DHTTYPE DHT22   // DHT 22  (AM2302), AM2321
//#define DHTTYPE DHT21   // DHT 21 (AM2301)

// Connect pin 1 (on the left) of the sensor to +5V
// NOTE: If using a board with 3.3V logic like an Arduino Due connect pin 1
// to 3.3V instead of 5V!
// Connect pin 2 of the sensor to whatever your DHTPIN is
// Connect pin 4 (on the right) of the sensor to GROUND
// Connect a 10K resistor from pin 2 (data) to pin 1 (power) of the sensor

// Initialize DHT sensor.
// Note that older versions of this library took an optional third parameter to
// tweak the timings for faster processors.  This parameter is no longer needed
// as the current DHT reading algorithm adjusts itself to work on faster procs.
DHT dht(DHTPIN, DHTTYPE);

//Stuff for RFID/NFC Reader ---------------------------------
//NFC Setup Variables
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

void setup() {
Serial.begin(115200);

//Show RFID Connected & Sending Info
Serial.println("Hello NFC");
nfc.begin();

//check if board is properly connected
uint32_t versiondata = nfc.getFirmwareVersion();
  if (! versiondata) {
    Serial.print("Didn't find PN53x board");
    while (1); // halt
  }

//Got some data from the board, print it out
  Serial.print("Found chip PN5"); Serial.println((versiondata>>24) & 0xFF, HEX); 
  Serial.print("Firmware ver. "); Serial.print((versiondata>>16) & 0xFF, DEC); 
  Serial.print('.'); Serial.println((versiondata>>8) & 0xFF, DEC);

// Set the max number of retry attempts to read from a card
  // This prevents us from waiting forever for a card, which is
  // the default behaviour of the PN532.
  nfc.setPassiveActivationRetries(0xFF);
  
  // configure board to read RFID tags
  nfc.SAMConfig();
    
  Serial.println("Waiting for an ISO14443A card");

//Show DHT Temp Sensor Connector
Serial.println("DHT11 test!");
  dht.begin();

}

void loop() {
  // put your main code here, to run repeatedly:
  readNFC();
  readDHT();
}

void readDHT() {
// Wait a few seconds between measurements.
  delay(500);

  // Reading temperature or humidity takes about 250 milliseconds!
  // Sensor readings may also be up to 2 seconds 'old' (its a very slow sensor)
  float h = dht.readHumidity();
  // Read temperature as Celsius (the default)
  float t = dht.readTemperature();
  // Read temperature as Fahrenheit (isFahrenheit = true)
  float f = dht.readTemperature(true);

  // Check if any reads failed and exit early (to try again).
  if (isnan(h) || isnan(t) || isnan(f)) {
    Serial.println("Failed to read from DHT sensor!");
    return;
  }

  // Compute heat index in Fahrenheit (the default)
  float hif = dht.computeHeatIndex(f, h);
  // Compute heat index in Celsius (isFahreheit = false)
  float hic = dht.computeHeatIndex(t, h, false);

  Serial.print("Humidity:");
  Serial.print(h);
  Serial.println("%\t");
  Serial.print("Temperature:");
  Serial.print(f);
  Serial.println("*F\t");
}

void readNFC() {
boolean success;
  uint8_t uid[] = { 0, 0, 0, 0, 0, 0, 0 };  // Buffer to store the returned UID
  uint8_t uidLength;                        // Length of the UID (4 or 7 bytes depending on ISO14443A card type)

  // Wait for an ISO14443A type cards (Mifare, etc.).  When one is found
  // 'uid' will be populated with the UID, and uidLength will indicate
  // if the uid is 4 bytes (Mifare Classic) or 7 bytes (Mifare Ultralight)
  success = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, &uid[0], &uidLength);
  
  if (success) {
//    Serial.println("Found a card!");
//    Serial.print("UID Length: ");Serial.print(uidLength, DEC);Serial.println(" bytes");
//    Serial.print("UID Value: ");
//    Serial.print(uid[1], DEC); 

    if (uid[1] == 117) {
      Serial.println("RFID:1");
    } else if (uid[1] == 1) {
      Serial.println("RFID:2");
    } else if (uid[1] == 175) {
      Serial.println("RFID:3");
    } else if (uid[1] == 13) {
      Serial.println("RFID:4");
    }

// Wait 1 second before continuing
    
    } else {
      // PN532 probably timed out waiting for a card
//    Serial.println("Timed out waiting for a card");
    } 
}

