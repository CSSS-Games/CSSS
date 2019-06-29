//  CSSS - CyberSecurity Scoring System
//  Copyright(C) 2017, 2019  Jonathan Hart (stuajnht) <stuajnht@users.noreply.github.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Reflection;
using CSSSCheckerEngine;
using CSSSConfig;
using Microsoft.Win32.TaskScheduler;
using NLog;

namespace CSSS
{
    /// <summary>
    /// Prepares CSSS for image capture
    /// 
    /// When the user has customised the image and CSSS to
    /// check various "issues", the "--prepare" argument is
    /// passed. This class looks after performing the relevant
    /// steps needed to get CSSS ready for deployment, such
    /// as encrypting the issue files and setting itself to
    /// run automatically when a user logs in to the computer
    /// </summary>
    public class Prepare
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// A reference to the <see cref="T:CSSSCheckerEngine.IssueFiles"/>
        /// IssueFiles class, used when preparing the issue files
        /// </summary>
        private IssueFiles issueFiles = new IssueFiles();

        /// <summary>
        /// Getting the location where CSSS is stored, so that it can
        /// be added to the startup files / settings
        /// </summary>
        private string CSSSDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

        /// <summary>
        /// Performs all of the preparation steps needed to get CSSS
        /// ready for image capture
        /// </summary>
        public void PerformAllPreparationSteps()
        {
            PrepareAllIssueFiles();

            AddToStartup();
        }

        /// <summary>
        /// Prepares all issue files by encrypting them, to prevent
        /// competitors from opening them to see the issues
        /// </summary>
        public void PrepareAllIssueFiles()
        {
            // Encrypting the issue files and removing any plaintext
            // files
            issueFiles.PrepareAllIssueFiles();
        }

        /// <summary>
        /// Adds CSSS to the various startup locations, so that it
        /// can start automatically at any user login
        /// </summary>
        public void AddToStartup()
        {
            logger.Info("Setting CSSS to run automatically on any user login");

            switch (config.operatingSystemType)
            {
                case Config.OperatingSystemType.WinNT:
                    AddToStartupWinNT();
                    break;

                case Config.OperatingSystemType.Linux:
                    AddToStartupLinux();
                    break;

                default:
                    break;
            }

            logger.Info("CSSS has been set to run automatically on any user login");
        }

        /// <summary>
        /// Allows CSSS to start on any user login to a Linux computer
        /// 
        /// A file is created in /etc/profile.d directory, which is
        /// called whenever a user logs onto the computer. This file
        /// changes the running directory to the location where CSSS
        /// is stored, then if successful, starts CSSSLauncher.exe
        /// 
        /// See: http://www.linuxfromscratch.org/blfs/view/6.3/postlfs/profile.html
        /// </summary>
        private void AddToStartupLinux()
        {
            string fileLocation = @"/etc/profile.d/CSSS.sh";
            string fileText = "cd " + CSSSDirectory + " && mono " + CSSSDirectory + "/CSSSLauncher.exe";

            logger.Debug("Contents of CSSS.sh file: {0}", fileText);
            logger.Debug("Attempting to create {0} file", fileLocation);

            if (!File.Exists(fileLocation))
            {
                // Create a file to write to it
                using (StreamWriter sw = File.CreateText(fileLocation))
                {
                    sw.WriteLine(fileText);
                }

                logger.Debug("The {0} file has been created", fileLocation);
            }
            else
            {
                logger.Error("Unable to create CSSS startup file {0} as it already exists, so it wasn't overwritten", fileLocation);
                logger.Error("Please create this manually before taking an image. File contents: " + fileText);
            }
        }

        /// <summary>
        /// Allows CSSS to start on any user login to a WinNT computer
        /// 
        /// A scheduled task is created which runs whenever any user
        /// logs onto the computer. The task starts the CSSSLauncher.exe
        /// program inside the CSSS running directory
        /// </summary>
        private void AddToStartupWinNT()
        {
            logger.Debug("Creating scheduled task to run CSSS on any user login");

            TaskDefinition task = TaskService.Instance.NewTask();
            task.RegistrationInfo.Description = "Runs CSSS at user login";
            task.Settings.Hidden = true;
            task.Settings.StartWhenAvailable = true;
            task.Settings.DisallowStartIfOnBatteries = false;
            task.Settings.StopIfGoingOnBatteries = false;
            task.Settings.RestartCount = 5;
            task.Settings.RestartInterval = TimeSpan.FromSeconds(60);
            task.Actions.Add(CSSSDirectory + Path.DirectorySeparatorChar + "CSSSLauncher.exe",
                             null,
                             CSSSDirectory);

            LogonTrigger trigger = new LogonTrigger();
            task.Triggers.Add(trigger);

            TaskService.Instance.RootFolder.RegisterTaskDefinition("CyberSecurity Scoring System",
                                                                   task,
                                                                   TaskCreation.CreateOrUpdate,
                                                                   null,
                                                                   null,
                                                                   TaskLogonType.InteractiveToken,
                                                                   null);

            logger.Debug("CSSS scheduled task has been created");
        }
    }
}
