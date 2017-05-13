//  CSSS - CyberSecurity Scoring System
//  Copyright(C) 2017  Jonathan Hart (stuajnht) <stuajnht@users.noreply.github.com>
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

using CSSSCheckerEngine;
using CSSSConfig;
using NLog;
using System;
using System.IO;
using System.Reflection;

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
        /// can start automatically at computer startup / user login
        /// </summary>
        public void AddToStartup()
        {
            logger.Info("Setting CSSS to run automatically on computer startup / user login");

            switch (config.operatingSystemType)
            {
                case Config.OperatingSystemType.WinNT:
                    break;
                    
                case Config.OperatingSystemType.Linux:
                    AddToStartupLinux();
                    break;
                    
                default:
                    break;
            }

            logger.Info("CSSS has been set to run automatically on computer startup / user login");
        }

        /// <summary>
        /// Allows CSSS to start at user login to a Linux computer
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
    }
}
