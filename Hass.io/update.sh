#!/usr/bin/env bash

sudo -u homeassistant -H -s
source /srv/homeassistant/bin/activate
pip3 install --upgrade homeassistant