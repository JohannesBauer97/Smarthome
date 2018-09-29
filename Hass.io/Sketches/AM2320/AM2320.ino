#include <Adafruit_MQTT.h>
#include <Adafruit_MQTT_Client.h>
#include "Adafruit_Sensor.h"
#include "Adafruit_AM2320.h"
#include "ESP8266WiFi.h"

/**
 * CONFIGURATION
 */
const char* ssid = ""; // WiFi SSID
const char* password = ""; // WiFi Password
const char* homeassistantAddress = ""; // HomeAssistant server address
const char* tempPath = "s1/temp"; // MQTT path for the temperature
const char* humPath = "s1/hum"; // MQTT path for the humidity
const char* mqttUser = "homeassistant"; // MQTT broker username
const char* mqttPass = ""; // MQTT broker password
const int mqttPort = 1883; // MQTT broker port at the HomeAssistant server
const int publishDelay = 5000; // Pause time (ms) between the MQTT publishes

/**
 * CONSTANTS
 */
Adafruit_AM2320 am2320 = Adafruit_AM2320();
WiFiClient client;
Adafruit_MQTT_Client mqtt(&client, homeassistantAddress, mqttPort, mqttUser, mqttPass);
Adafruit_MQTT_Publish mqtt_temperature = Adafruit_MQTT_Publish(&mqtt, tempPath, MQTT_QOS_1);
Adafruit_MQTT_Publish mqtt_humidity = Adafruit_MQTT_Publish(&mqtt, humPath, MQTT_QOS_1);

void setup() {
  Serial.begin(115200);
  Serial.println("Connecting to " + String(ssid));
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(100);
    Serial.print(".");
  }

  Serial.println(" connected");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());

  am2320.begin();
}

void loop() {
  delay(publishDelay);

  if (!mqtt.connected()) {
    if (mqtt.connect() != 0) {
      Serial.println("Lost connection to broker");
      return;
    }
  }

  Serial.print("Temp: "); Serial.println(am2320.readTemperature());
  Serial.print("Hum: "); Serial.println(am2320.readHumidity());

  // publish data
  mqtt_temperature.publish(am2320.readTemperature());
  mqtt_humidity.publish(am2320.readHumidity());
  mqtt.disconnect();
}
