# Contributing
> Please note that this project is released with a [Contributor Code of Conduct](#CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

Thanks for your interest in contributing to this project. You can contribute or report issues in the following ways:

## Pull Requests
If you would like to create a pull request, please make sure that you are on the [develop branch](https://github.com/stuajnht/CSSS/tree/develop) before opening one. Once you have cloned or forked this repo, open the `CSSS.sln` file to begin development. This project uses [git-flow](https://github.com/nvie/gitflow) as its branching model.

The current development environment is with Visual Studio Community 2019 and .Net Framework 4.0. Please make sure you are using these versions before submitting any pull requests.

## Coding Conventions
This project uses [*EditorConfig*](https://editorconfig.org/) to maintain a consistent coding style, using the [default EditorConfig file](https://docs.microsoft.com/en-gb/visualstudio/ide/editorconfig-code-style-settings-reference#example-editorconfig-file) that comes with Visual Studio (with [a few additional tweaks](.editorconfig)).

If you are planning to contribute code, please make sure that you have [performed a code cleanup](https://docs.microsoft.com/en-us/visualstudio/ide/code-styles-and-code-cleanup?#apply-code-styles). Alternatively, install an extension to do this for you:

* [Format document on Save](https://marketplace.visualstudio.com/items?itemName=mynkow.FormatdocumentonSave)
* [Code Cleanup On Save](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.CodeCleanupOnSave)

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
