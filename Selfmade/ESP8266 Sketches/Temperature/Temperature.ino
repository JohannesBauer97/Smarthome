#include <ESP8266WiFi.h>
#include <DNSServer.h>
#include <ESP8266WebServer.h>
#include <WiFiManager.h> // https://github.com/tzapu/WiFiManager
#include <ESP8266HTTPClient.h>
#include <ESP8266httpUpdate.h>
#include <WiFiUdp.h>
#include <PubSubClient.h>
const int FW_VERSION = 1;
const char* FW_VERSION_URL = "http://esp.serverlein.de/temperature/version";
const char* FW_IMAGE_URL = "http://esp.serverlein.de/temperature/img.bin";
const unsigned int UDP_PORT = 7352;
char mqttBroker[50];
WiFiUDP udp;
WiFiClient espClient;
PubSubClient client(espClient);
String chipId;
int mqttReconnectCounter = 0;

void setup() {
  Serial.begin(115200);
  chipId = "ESP" + String(ESP.getChipId());
  WiFiManager wifiManager;
  wifiManager.autoConnect();
  checkForUpdates();
  udp.begin(UDP_PORT);
}

void loop() {
  scanMqttBroker();
  handleMqttBrokerConnection();

  String publishTemperatureDataTopic = "/iot/temperature/" + chipId;
  client.publish(publishTemperatureDataTopic.c_str(), "Hello World");
}

void checkForUpdates() {
  HTTPClient httpClient;
  httpClient.begin(FW_VERSION_URL);
  int httpCode = httpClient.GET();
  if(httpCode == 200) {
    String newFWVersion = httpClient.getString();
    Serial.print("Current firmware version: ");
    Serial.println(FW_VERSION);
    Serial.print("Available firmware version: ");
    Serial.println(newFWVersion);
    int newVersion = newFWVersion.toInt();
    if(newVersion > FW_VERSION){
      Serial.println("Preparing to update.");
      t_httpUpdate_return ret = ESPhttpUpdate.update(FW_IMAGE_URL);
      switch(ret) {
        case HTTP_UPDATE_FAILED:
          Serial.printf("HTTP_UPDATE_FAILED Error (%d): %s",  ESPhttpUpdate.getLastError(), ESPhttpUpdate.getLastErrorString().c_str());
          break;
        case HTTP_UPDATE_NO_UPDATES:
          Serial.println("HTTP_UPDATE_NO_UPDATES");
          break;
      }
    } else {
      Serial.println("Already on latest version");
    }
  } else {
    Serial.print("Firmware version check failed, got HTTP response code ");
    Serial.println(httpCode);
  }
  httpClient.end();
}

void scanMqttBroker(){
  if(strlen(mqttBroker) > 0){
    return;
  }
  if(udp.parsePacket() > 0){
    udp.read(mqttBroker, 50);
  } else {
    udp.beginPacket("255.255.255.255", UDP_PORT);
    String discoverMsg = "autodiscover:temperature:" + String(chipId);
    udp.write(discoverMsg.c_str());
    udp.endPacket();
    delay(1000);
  } 
}

void handleMqttBrokerConnection(){
  if(strlen(mqttBroker) == 0){
    return;
  }

  client.setServer(mqttBroker, 1883);
  while (!client.connected()) {
    Serial.print("Connecting to MQTT broker ");
    Serial.print(mqttBroker);
    Serial.print(" as ");
    Serial.println(chipId.c_str());
    if (!client.connect(chipId.c_str())) {
      if(mqttReconnectCounter >= 4){
        ESP.restart();
      }
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" retrying in 5 seconds");
      mqttReconnectCounter++;
      delay(5000);
    }
  }
  client.loop();
}
