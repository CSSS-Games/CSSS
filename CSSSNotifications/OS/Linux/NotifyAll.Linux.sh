#!/bin/bash
# Notifies all X sessions of points scored
# See: https://unix.stackexchange.com/q/2881/#comment3049_2883
# See: https://unix.stackexchange.com/a/307097
PATH=/usr/bin:/bin

who | awk '/\(:[0-9]+\)/ {gsub("[:|(|)]","");print "DISPLAY=:"$5 " sudo -u " $1 " notify-send \"CyberSecurity Scoring System\" \"Points changed\""}' | bash
