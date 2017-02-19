# CyberSecurity Scoring System (CSSS)

CyberSecurity Scoring System (CSSS) is a program to assist with training for the CyberPatriot / CyberCenturion competitions.

While it is currently possible to create [your own practice images](https://www.uscyberpatriot.org/competition/training-materials/practice-images), there is only a scoring client for Windows computers. To assist with training competitors with Windows, Ubuntu and Debian Operating Systems, this program has been created to provide a scoring system for them.

## Requirements
To run CSSS, you will need to have an image running one of the supported operating systems:
  * Windows:
    * Windows Vista
    * Windows 7
    * Windows 8
    * Windows Server 2008
  * Linux:
    * Ubuntu 14.04
    * Ubuntu 16.04
    * Debian 7

Other operating systems should work, but they are not supported as the CyberPatriot / CyberCenturion competitions do not use them.

## Installing
Before CSSS will run, the following additional programs need to be installed onto your image:
  * Windows:
    * The .Net framework versions [4.0](https://www.microsoft.com/en-gb/download/details.aspx?id=17718) or [4.5](https://www.microsoft.com/en-gb/download/details.aspx?id=42642) (which one is required depends on if your image contains any of the needed Service Packs). Please follow the usual installation steps (Next, Next, ..., Finish)
  * Linux:
    * The Mono client should be installed. Please follow their instructions on the [Install Mono on Linux](http://www.mono-project.com/docs/getting-started/install/linux/) help article (summary: run the 3 lines of terminal commands from the top of the article, then run `sudo apt-get install -y mono-devel`)
    * __For Debian images__, the libnotify package also needs to be installed (`sudo apt-get install libnotify-bin`)

## Running
Double-clicking on CSSS will cause it to quickly show the usage window, then close itself. This is by design. To run CSSS properly, a number of arguments need to be passed to the program, which are:
```
-c, --check:   Checks the config files for any problems
-o, --observe: Observes CSSS running before preparing it (implies 'c')
-p, --prepare: Prepares CSSS ready for image release (implies '-c')
-s, --start:   Starts the scoring system
-h, --help:    Shows the program usage
```

These arguments have been chosen to spell 'COPS', as competitors are 'policing' the security of the computer.

Under normal image building, the 'o' option should be passed, as this allows CSSS to run as it would for training, but without affecting the files used to list the 'issues'.

## Copyright
CyberPatriot is copyright of US Air Force Association
CyberCentrurion is copyright of Cyber Security Challenge UK

## Disclaimer
This program is designed to score the state of a computer image against a pre-defined checklist of "issues". It is not designed to rate your computer security, provide security advice, or anything else that isn't related to the CyberPatriot / CyberCenturion competitions.
