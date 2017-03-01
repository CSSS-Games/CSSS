//  CSSSCheckerEngine - CyberSecurity Scoring System Checker Engine
//  Copyright(C) 2017  Jonathan Hart (stuajnht) <stuajnht@users.noreply.github.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program. If not, see <http://www.gnu.org/licenses/>.

using Newtonsoft.Json;
using NLog;
using System;
using System.IO;

namespace CSSSCheckerEngine
{
    /// <summary>
    /// Performs the loading and linting of the issue files
    /// that are used to check the current image state against
    /// what is expected
    /// </summary>
    public class IssueFiles
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public IssueFiles()
        {
        }

        /// <summary>
        /// Attempts to lint all of the issue files in the issue directory,
        /// </summary>
        /// <returns><c>true</c>, if all files were successfully linted, <c>false</c> otherwise</returns>
        public bool LintAllFiles()
        {
            logger.Info("Preparing to lint all issue files");

            bool FilesLintedSuccessfully = true;

            string[] IssueFiles = GetAllIssueFiles();

            foreach (string IssueFilePath in IssueFiles)
            {
                logger.Debug("Preparing to lint issue file located at: {0}", IssueFilePath);

                if (!LintFile(IssueFilePath))
                {
                    logger.Error("The issue file at \"{0}\" could not be linted correctly", IssueFilePath);
                    FilesLintedSuccessfully = false;
                }
            }

            logger.Info("Finished linting all issue files");
            logger.Debug("Successful linting of all issue files: {0}", FilesLintedSuccessfully);
            return FilesLintedSuccessfully;
        }

        /// <summary>
        /// Lints the issue file passed to the function
        /// </summary>
        /// <returns><c>true</c>, if file was successfully linted, <c>false</c> otherwise</returns>
        /// <param name="IssueFilePath">The full path to the issue file</param>
        public bool LintFile(string IssueFilePath)
        {
            try
            {
                dynamic IssueFileJSON = JsonConvert.DeserializeObject(IssueFilePath);
            }
            catch
            {
                // There was a problem parsing the JSON file
                return false;
            }

            // The JSON format of this file is fine
            return true;
        }

        /// <summary>
        /// Gets all of the issue files in the issue directory
        /// </summary>
        /// <returns>A path array to all of the issue files</returns>
        public string[] GetAllIssueFiles()
        {
            return Directory.GetFiles(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Issues",
                                      "*.json",
                                      SearchOption.AllDirectories);
        }
    }
}
