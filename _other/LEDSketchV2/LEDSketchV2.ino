#include <stdio.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h> 
#include <ESP8266WebServer.h>
#include <DNSServer.h>

#define LED 2
#define REDPIN 12
#define GREENPIN 14
#define BLUEPIN 4

// ----------
// Global Variables
// ----------

//LED Control
int RVal = 0;
int GVal = 0;
int BVal = 0;

//Setup
ESP8266WebServer webserver(80);
DNSServer dnsServer;
IPAddress serverIP; 
String Hostname;
const char *HTMLSETUP = "<!DOCTYPE html> <html> <head> <title>LED-Strip Configuration</title> <meta charset='utf-8' /> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <style> *{ box-sizing: border-box; } html,body{ margin:0; padding:0; width:100%; display:flex; flex-direction:column; justify-content: flex-start; height:100%; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:16px; } #top{ background:#00a1ff; width:100%; height:50px; display:flex; } #bot{ background:#00a1ff; width:100%; height:50px; margin-top:auto; display: flex; flex-direction: column; justify-content: center; } #mid{ width:100%; display:flex; justify-content: center; margin-top:20px; } #centered{ border:solid rgba(0, 0, 0, 0.274) 1px; box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.75); padding:15px; border-radius: 5px; } a{ text-decoration: none; color:inherit; } a:hover{ text-decoration: underline; } table tr td:first-child{ text-align: right; font-weight: 450; } </style> </head> <body> <div id='top'><span style='text-align: center; color:white; font-weight:bold; width:100%; font-size: 31px; text-shadow: 0px 0px 3px rgb(86, 86, 86);'>LED-Strip Configuration</span></div> <div id='mid'> <div id='centered'> <form> <table> <tr> <td>Room:</td> <td><input type='text' name='room' placeholder='Lounge'></td> </tr> <tr> <td>WiFi SSID:</td> <td><input type='text' name='ssid' placeholder='Homenetwork'></td> </tr> <tr> <td>WiFi Key:</td> <td><input type='password' name='key' placeholder='1234567890'></td> </tr> <tr> <td>Server IP:</td> <td> <input type='text' name='ip1' placeholder='192' style='width:28px'> <span>.</span> <input type='text' name='ip2' placeholder='168' style='width:28px'> <span>.</span> <input type='text' name='ip3' placeholder='178' style='width:28px'> <span>.</span> <input type='text' name='ip4' placeholder='123' style='width:28px'> </td> </tr> <tr> <td colspan='2'> <button type='submit'>Save</button> </td> </tr> </table> </form> </div> </div> <div id='bot'> <span style='color:white; width:100%; padding-left:10px;'> Made with <span style='color:red'>&#10084;</span> by <a href='mailto:jo.bauer97@gmail.com'>Johannes Bauer</a> </span> </div> </body> </html>";
const char *HTMLERROR = "<!DOCTYPE html> <html> <head> <title>LED-Strip Configuration</title> <meta charset='utf-8' /> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <style> *{ box-sizing: border-box; } html,body{ margin:0; padding:0; width:100%; display:flex; flex-direction:column; justify-content: flex-start; height:100%; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:16px; } #top{ background:#00a1ff; width:100%; height:50px; display:flex; } #bot{ background:#00a1ff; width:100%; height:50px; margin-top:auto; display: flex; flex-direction: column; justify-content: center; } #mid{ width:100%; display:flex; justify-content: center; margin-top:20px; } #centered{ border:solid rgba(0, 0, 0, 0.274) 1px; box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.75); padding:15px; border-radius: 5px; background: red; margin:5px; } a{ text-decoration: none; color:inherit; } a:hover{ text-decoration: underline; } table tr td:first-child{ text-align: right; font-weight: 450; } </style> </head> <body> <div id='top'><span style='text-align: center; color:white; font-weight:bold; width:100%; font-size: 31px; text-shadow: 0px 0px 3px rgb(86, 86, 86);'>LED-Strip Configuration</span></div> <div id='mid'> <div id='centered'> <span style='font-size:30px;'>Oops!</span> <br> <span>Something went wrong!<br> Please restart the product by disconnecting the power supply!</span> </div> </div> <div id='bot'> <span style='color:white; width:100%; padding-left:10px;'> Made with <span style='color:red'>&#10084;</span> by <a href='mailto:jo.bauer97@gmail.com'>Johannes Bauer</a> </span> </div> </body> </html>";
const char *HTMLSUCCESS = "<!DOCTYPE html> <html> <head> <title>LED-Strip Configuration</title> <meta charset='utf-8' /> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <style> *{ box-sizing: border-box; } html,body{ margin:0; padding:0; width:100%; display:flex; flex-direction:column; justify-content: flex-start; height:100%; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:16px; } #top{ background:#00a1ff; width:100%; height:50px; display:flex; } #bot{ background:#00a1ff; width:100%; height:50px; margin-top:auto; display: flex; flex-direction: column; justify-content: center; } #mid{ width:100%; display:flex; justify-content: center; margin-top:20px; } #centered{ border:solid rgba(0, 0, 0, 0.274) 1px; box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.75); padding:15px; border-radius: 5px; background: #2ad200; } a{ text-decoration: none; color:inherit; } a:hover{ text-decoration: underline; } table tr td:first-child{ text-align: right; font-weight: 450; } </style> </head> <body> <div id='top'><span style='text-align: center; color:white; font-weight:bold; width:100%; font-size: 31px; text-shadow: 0px 0px 3px rgb(86, 86, 86);'>LED-Strip Configuration</span></div> <div id='mid'> <div id='centered'> <span style='font-size:30px;'>Success!</span> </div> </div> <div id='bot'> <span style='color:white; width:100%; padding-left:10px;'> Made with <span style='color:red'>&#10084;</span> by <a href='mailto:jo.bauer97@gmail.com'>Johannes Bauer</a> </span> </div> </body> </html>";
bool setupCompleted = false;

// -----
// Setup
// -----
void setup() {
  Serial.begin(115200);
  
  Serial.println("Setting PinModes");
  pinMode(LED, OUTPUT);
  pinMode(REDPIN, OUTPUT);
  pinMode(GREENPIN, OUTPUT);
  pinMode(BLUEPIN, OUTPUT);
  digitalWrite(LED, LOW);
  
  Serial.println("Setting LED Pins to 0");
  analogWrite(REDPIN, RVal);
  analogWrite(GREENPIN, GVal);
  analogWrite(BLUEPIN, BVal);

  Serial.println("Starting AP");
  WiFi.mode(WIFI_AP);
  IPAddress apIP(192,168,1,1);
  
  WiFi.softAP("LED-Strip");
  delay(1000);
  WiFi.softAPConfig(apIP, apIP, IPAddress(255, 255, 255, 0));

  Serial.println("Starting DNS");
  dnsServer.start(53, "*", apIP);
  
  Serial.println("Configuring and starting Webserver");
  webserver.onNotFound(handleRoot);
  webserver.begin();

  Serial.println("Setup completed.\nFlushing Serial.");
  Serial.flush();
}

// ----
// Loop
// ----
void loop() {
  if(!setupCompleted){
    dnsServer.processNextRequest();
    webserver.handleClient();
    return;
  }

  Serial.print("WiFi.status(): ");
  Serial.println(WiFi.status());

  WiFiClient client;
  if (!client.connect(serverIP, 5555)) {
    Serial.println("Connection to TCP Server failed. Waiting 5 seconds.");
    delay(5000);
    return;
  }

  char response[8];
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
      client.printf("#%02x%02x%02x %s",RVal, GVal, BVal, Hostname.c_str()); 
      Serial.println(Hostname);
    }
    response[0] = 0;
    yield();
  }
  
  Serial.println("Closing TCP connection.");
  client.stop();
  
  Serial.println("Loop ended. Waiting 5 seconds.");
  delay(5000);
}

// --------------------
// Webserver root route
// --------------------
void handleRoot(){
  if(webserver.args() == 7){
    String wpaKey = webserver.arg("key");
    Serial.println("WPA KEY:" + wpaKey);
    if(wpaKey.length() <= 0){
      webserver.send(200,"text/html",HTMLERROR);
      Serial.println("WPA KEY LENGTH < 0");
      return;
    }
    String wifiSSID = webserver.arg("ssid");
    Serial.println("WIFI SSID:" + wifiSSID);
    if(wifiSSID.length() <= 0){
      webserver.send(200,"text/html",HTMLERROR);
      Serial.println("WIFI SSID LENGTH < 0");
      return;
    }
    String room = webserver.arg("room");
    Serial.println("ROOM:" + room);
    if(room.length() <= 0){
      webserver.send(200,"text/html",HTMLERROR);
      Serial.println("ROOM LENGTH < 0");
      return;
    }
    Hostname = room;
    
    uint8_t ip1 = webserver.arg("ip1").toInt();
    uint8_t ip2 = webserver.arg("ip2").toInt();
    uint8_t ip3 = webserver.arg("ip3").toInt();
    uint8_t ip4 = webserver.arg("ip4").toInt();
    serverIP = IPAddress(ip1, ip2, ip3, ip4);
    Serial.println("SERVERIP:" + serverIP);

    webserver.send(200,"text/html",HTMLSUCCESS);
    webserver.stop();
    WiFi.softAPdisconnect(true);
    WiFi.hostname(room);
    WiFi.begin(wifiSSID.c_str(), wpaKey.c_str());
    setupCompleted = true;
    digitalWrite(LED, HIGH);
  }else{
    webserver.send(200,"text/html",HTMLSETUP);
  }
  
}

