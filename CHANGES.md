# CyberSecurity Scoring System (CSSS) - Change Log

## Version 0.3
### Breaking Changes
* When running with the `--prepare` switch, CSSS needs to be run with administrative privileges
* Issue files are encrypted when the image is prepared, to prevent competitors discovering what checks are being carried out
> :warning: Make sure that you have a snapshot of your image before you run the `--prepare` command, otherwise you will need to set everything up again should there be any problems.

### New Stuff
* Prevented the start time (and total running time) from being changed in the scoring report
* Created a Launcher stub program to run CSSS, so the main program can run hidden from competitors
* CSSS adds itself to the relevant startup locations on supported operating systems, so it can run automatically

### Issue File Checks
#### All Operating Systems
* File contents - checks the file contents with the values in the issue file, to see if they do or don't match what is expected
* File existance - compares the files that are currently on the system against the ones expected in the issue file, to see if they exist or have been removed as needed

#### Windows
* Registry - compares the current registry settings with those in the issue file, to see if the issue has been resolved

## Version 0.2
### New Stuff
* Created CSSS checker engine to perform issue checks
* Generated a scoring report to let competitors know how they are doing
* Displayed desktop notifications when points are gained, lost or changed

## Version 0.1
### New Stuff
* Created CSSS project
* Designed config, init and bootstrap classes to look after the initial startup of CSSS
