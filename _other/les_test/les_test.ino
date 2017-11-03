#include <stdio.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h> 
#include <ESP8266WebServer.h>

#define LED D4
#define REDPIN D6
#define GREENPIN D5
#define BLUEPIN D2

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
    delay(500);
    webserver.stop();
    delay(500);
    webserverStopped = true;
    WiFi.softAPdisconnect(true);
    delay(5000);

    char wifissidchar[wifissid.length() + 1];
    char wifipasschar[wifipassword.length() + 1];
    wifissid.toCharArray(wifissidchar,wifissid.length() + 1);
    wifipassword.toCharArray(wifipasschar,wifipassword.length() + 1);

    WiFi.begin(wifissidchar, wifipasschar);
    Serial.print("Connection to Wifi");
    uint8_t i = 0;
    while (WiFi.status() != WL_CONNECTED) {
      if(i >= 60){
        Serial.println("Wifi Connect Timed Out. Hardreset.");
        ESP.reset();
      }
      
      digitalWrite(LED, HIGH);
      delay(500);
      digitalWrite(LED, LOW);
      Serial.print(WiFi.status());
      Serial.print(".");
      i++;
    }
    Serial.println("");
    digitalWrite(LED, HIGH);
    delay(1000);
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
  char r[3], g[3], b[3];

  if(RVal <= 15){
      sprintf(r, "0%x", RVal);
  }else{
    sprintf(r, "%x", RVal);
  }

  if(GVal <= 15){
      sprintf(g, "0%x", GVal);
  }else{
    sprintf(g, "%x", GVal);
  }
  
  if(BVal <= 15){
      sprintf(b, "0%x", BVal);
  }else{
    sprintf(b, "%x", BVal);
  }
  sprintf(valInfo,"#%s%s%s",r,g,b);
}

void setup() {
  delay(1000);
  Serial.begin(115200);
  WiFi.hostname("ESP_" + ID);
  
  pinMode(LED, OUTPUT);
  pinMode(REDPIN, OUTPUT);
  pinMode(GREENPIN, OUTPUT);
  pinMode(BLUEPIN, OUTPUT);

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
  }
  
  Serial.println("Closing TCP connection.");
  client.stop();
  
  Serial.println("Loop ended. Waiting 5 seconds.");
  delay(5000);
}
