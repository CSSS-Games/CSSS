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

## To-Do
These are the tasks planned for each release version. Please note that these can change without warning, depending on how features are progressing.

### Version 0.2
* ~~Create checker system engine and initial check template files (to check the Operating System version, for example)~~
* Show notifications should points be gained or lost
* ~~Generate a scoring report~~

### Version 0.3
* Create a "guardian" service that can run CSSS when the computer starts (service should be set as disabled when installed, but when the '-p' option is passed it would be set up automatic)

### Version 0.4
* Allow check files to be encrypted when '-p' is passed, so that images can be released but competitors can not find out what "issues" need to be fixed

### Version 0.5
* Create scripts to install CSSS files into the correct locations (WinNT: root of `%SystemDrive%`, Linux: `/opt` directory)
* Install additional software when CSSS is being installed to make sure all items are available for it to use
* Create build scripts that install software that can be used when testing the checker system engine

### Versions 0.6 onward
* Create more check template files and relevant classes for the checker system engine to use (a new version for each check "category")
* Create some example checker files with instructions to create images based around them with pre-made questions and scenarios

## Copyright
CyberPatriot is copyright of US Air Force Association

CyberCentrurion is copyright of Cyber Security Challenge UK

## Disclaimer
This program is designed to score the state of a computer image against a pre-defined checklist of "issues". It is not designed to rate your computer security, provide security advice, or anything else that isn't related to the CyberPatriot / CyberCenturion competitions.
