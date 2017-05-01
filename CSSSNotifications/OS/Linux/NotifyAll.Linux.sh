#!/bin/bash
# Notifies all X sessions of points scored
# See: https://unix.stackexchange.com/q/2881/#comment3049_2883
# See: https://unix.stackexchange.com/a/307097
PATH=/usr/bin:/bin

# Parsing the title, message and icon sent from CSSS
TITLE=$1
MESSAGE=$2
ICON=$3

who | awk '/\(:[0-9]+\)/ {gsub("[:|(|)]","");print "DISPLAY=:"$5 " sudo -u " $1 " notify-send -t 5 \"'"$TITLE"'\" \"'"$MESSAGE"'\" --icon=\"'"$ICON"'\""}' | bash
