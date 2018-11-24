#!/bin/bash

docker run -d \
    --name home-assistant \
    --restart=unless-stopped \
	-v /root/hassio:/config \
	-v /etc/localtime:/etc/localtime:ro \
	--net=host \
	homeassistant/raspberrypi3-homeassistant:latest
