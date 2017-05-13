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
        /// Performs all of the preparation steps needed to get CSSS
        /// ready for image capture
        /// </summary>
        public void PerformAllPreparationSteps()
        {
            PrepareAllIssueFiles();
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
    }
}
