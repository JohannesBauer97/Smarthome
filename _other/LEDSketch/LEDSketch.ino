#include <stdio.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h> 
#include <ESP8266WebServer.h>

#define LED 2
#define REDPIN 12
#define GREENPIN 14
#define BLUEPIN 4

int RVal = 0;
int GVal = 0;
int BVal = 0;

char valInfo[16];

const char *ssid = "LEDStrip";
const int Port = 5555;
const String ID = "TV";
String wifipassword, wifissid;
IPAddress ip;
ESP8266WebServer webserver(80);
bool webserverStopped = false;

/* Just a little test message.  Go to http://192.168.4.1 in a web browser
 * connected to this access point to see it.
 */
void handleRoot() {

  if( webserver.args() > 0 ){
    wifipassword = webserver.arg("pass");
    wifissid = webserver.arg("ssid");
    uint8_t ip1 = atoi(webserver.arg("ip1").c_str());
    uint8_t ip2 = atoi(webserver.arg("ip2").c_str());
    uint8_t ip3 = atoi(webserver.arg("ip3").c_str());
    uint8_t ip4 = atoi(webserver.arg("ip4").c_str());
    ip = IPAddress(ip1, ip2, ip3, ip4);
    webserver.send(200,"text/html","Configuration successful.");
    Serial.println("Wifi SSID: " + wifissid);
    Serial.println("Wifi Password: " + wifipassword);
    Serial.println("Stopping webserver.");
    webserver.stop();
    Serial.println("Webserver stopped.");
    webserverStopped = true;
    Serial.println("Stopping softAP.");
    WiFi.softAPdisconnect(true);
    Serial.println("SoftAP stopped.");

    //Converting WiFi Credentials from String to Char[]
    char wifissidchar[wifissid.length() + 1];
    char wifipasschar[wifipassword.length() + 1];
    wifissid.toCharArray(wifissidchar,wifissid.length() + 1);
    wifipassword.toCharArray(wifipasschar,wifipassword.length() + 1);

    Serial.println("Trying to connect to " + wifissid + ".");
    WiFi.begin(wifissidchar, wifipasschar);
    return;
  }

  webserver.send(200, "text/html","<!DOCTYPE html>\
<html>\
  <head>\
    <title>LED Strip Configuration</title>\
  </head>\
  <body>\
    <h1>Setup LED Strip Controller</h1>\
    <form>\
      <input type='text' name='ssid' placeholder='SSID'><br>\
      <input type='text' name='pass' placeholder='Password'><br>\
      <input type='text' name='ip1' placeholder='192' style='width:25px'><input type='text' name='ip2' placeholder='168' style='width:25px'><input type='text' name='ip3' placeholder='188' style='width:25px'><input type='text' name='ip4' placeholder='00' style='width:25px'><br>\
      <input type='submit' value='Submit'>\
    </form>\
  </body>\
</html>");
}

void info(){
  sprintf(valInfo,"#%02x%02x%02x", RVal, GVal, BVal);
}

void setup() {
  Serial.begin(115200);
  Serial.println("Setting Hostname");
  WiFi.hostname("ESP_" + ID);

  Serial.println("Setting PinModes");
  pinMode(LED, OUTPUT);
  pinMode(REDPIN, OUTPUT);
  pinMode(GREENPIN, OUTPUT);
  pinMode(BLUEPIN, OUTPUT);

  Serial.println("Setting LED Pins to 0");
  analogWrite(REDPIN, RVal);
  analogWrite(GREENPIN, GVal);
  analogWrite(BLUEPIN, BVal);

  Serial.println("Starting AP");
  WiFi.softAP(ssid);
  webserver.on("/", handleRoot);
  Serial.println("Starting Webserver");
  webserver.begin();
  Serial.println("Setup completed");
  Serial.flush();
}

void loop() {
  digitalWrite(LED, LOW);
  if(!webserverStopped){
    webserver.handleClient();
    return;
  }

  Serial.print("WiFi.status(): ");
  Serial.println(WiFi.status());
  
  /*if(WiFi.status() != WL_CONNECTED){
    Serial.println("Not connected with WiFi");
    return;
  }else{
    Serial.println("Connected with WiFi.");
  }*/

  WiFiClient client;
  if (!client.connect(ip, Port)) {
        Serial.println("Connection to TCP Server failed. Waiting 5 seconds.");
        delay(5000);
        return;
    }

  char response[8]; //response außerhalb deklarieren und innerhalb auf 0 setzen?
  while(client.connected()){
  
    client.readBytes(response,7);
    if(response[0] == 35){
      char r[3] = {response[1], response[2]};
      char g[3] = {response[3], response[4]};
      char b[3] = {response[5], response[6]};

      RVal = strtol(r,NULL,16);
      GVal = strtol(g,NULL,16);
      BVal = strtol(b,NULL,16);
      
      Serial.print("Values: ");
      Serial.print(RVal);
      Serial.print(" ");
      Serial.print(GVal);
      Serial.print(" ");
      Serial.println(BVal);

      analogWrite(REDPIN, RVal);
      analogWrite(GREENPIN, GVal);
      analogWrite(BLUEPIN, BVal);
    }else if(response[0] == 73){
      info();
      client.print(valInfo); 
    }

    response[0] = 0;
    yield();
  }
  
  Serial.println("Closing TCP connection.");
  client.stop();
  
  Serial.println("Loop ended. Waiting 5 seconds.");
  delay(5000);
}
