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
Usage:
  CSSS.exe -c | -o | -p | -s | [-h] | [-m]

Examples:
  CSSS.exe -c
  CSSS.exe -o -m
  CSSS.exe -p
  CSSS.exe -s
  CSSS.exe -h


Required arguments (at least one is needed):
  -c, --check:    Checks the config files for any problems
  -o, --observe:  Observes CSSS running before preparing it (implies '-c')
  -p, --prepare:  Prepares CSSS ready for image release (implies '-c')
  -s, --start:    Starts the scoring system


Optional arguments:
  -h, --help:     Shows this help message


Developer arguments (all optional):
  -m, --multiple: Allows multiple instances of CSSS to run concurently
```

The required arguments have been chosen to spell 'COPS', as competitors are 'policing' the security of the computer.

Under normal image building, the `-o` option should be passed, as this allows CSSS to run as it would for training, but without affecting the files used to list the 'issues'.

When you are ready to release the image, __with administrative privileges__ (e.g. `sudo`, "Run as administrator") run CSSS with the `-p` option to prepare the necessary files and allow CSSS to start automatically on computer reboots.
> :warning: Running CSSS with the `-p` argument will encrypt the issue files. Make sure that you have a snapshot of your image before you run this command, otherwise you will need to set everything up again should there be any problems.

## Contributing
Thanks for your interest in contributing to this project. You can contribute or report issues in the following ways:

### Pull Requests
If you would like to create a pull request, please make sure that you are on the [develop branch](https://github.com/stuajnht/CSSS/tree/develop) before opening one. Once you have cloned or forked this repo, open the `CSSS.sln` file to begin development. This project uses [git-flow](https://github.com/nvie/gitflow) as its branching model.

The current development environment is with Visual Studio Community 2019 and .Net Framework 4.0. Please make sure you are using these versions before submitting any pull requests.

## License Terms
CSSS is publised under the GNU GPL v3 License, see the [LICENSE](LICENSE.md) file for more information.

### NuGet Packages
This project uses NuGet packages. Their project source code pages and licenses can be found below:
* [HtmlAgilityPack](https://htmlagilitypack.codeplex.com/)
* [Newtonsoft.Json](http://www.newtonsoft.com/json)
* [NLog](http://nlog-project.org/)
* [NUnit](https://www.nunit.org/)

### Other Projects
The following projects and source code are included in CSSS. Their licenses project pages can be found below:
* [OSVersionInfo](https://www.codeproject.com/Articles/73000/Getting-Operating-System-Version-Info-Even-for-Win)

## To-Do
These are the tasks planned for each release version. Please note that these can change without warning, depending on how features are progressing.

### Version 0.3
* Perform various tasks when the '-p' argument is passed:
  * ~~Set issue check files to be encrypted, so that images can be released but competitors can not find out what "issues" need to be fixed~~
  * ~~Add files to the relevant Operating System startup folders, so that CSSS will start automatically:~~
    * ~~[Windows shortcut](http://stackoverflow.com/a/19914018) in [Programdata startup folder](https://www.kiloroot.com/all-users-or-common-startup-folder-locations-launch-programs-at-window-login-windows-server-2008-r2-2012-2012-r2/)~~
    * ~~[Linux](http://raspberrypi.stackexchange.com/a/5159)~~
  * Email list of issues to be fixed when '--email' argument is passed
  * Shutdown the computer to allow an image to be taken
* Comment ~~and coding style~~ tidy-up

### Version 0.4
* Create scripts to install CSSS files into the correct locations (WinNT: root of `%SystemDrive%`, Linux: `/opt` directory)
* Install additional software when CSSS is being installed to make sure all items are available for it to use
* Create build scripts that install software that can be used when testing the checker system engine

### Versions 0.5 onward
* Create more check template files and relevant classes for the checker system engine to use (a new version for each check "category")
* Create some example checker files with instructions to create images based around them with pre-made questions and scenarios

## Copyright
CyberPatriot is copyright of US Air Force Association

CyberCentrurion is copyright of Cyber Security Challenge UK

## Disclaimer
This program is designed to score the state of a computer image against a pre-defined checklist of "issues". It is not designed to rate your computer security, provide security advice, or anything else that isn't related to the CyberPatriot / CyberCenturion competitions.
