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
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program. If not, see <http://www.gnu.org/licenses/>.

using CSSSConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.IO;
using System.Reflection;

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

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Sees if the issue files have been linted or are being
        /// loaded directly without any initil checks. This is set
        /// to true during the LintAllIssueFiles function, and will
        /// display a warning when LoadAllIssueFiles is called if it
        /// is still set to false
        /// </summary>
        /// <see cref="LintAllIssueFiles"/>
        /// <see cref="LoadAllIssueFiles"/>
        private bool issueFilesHaveBeenLinted = false;

        public IssueFiles()
        {
        }

        /// <summary>
        /// Attempts to lint all of the issue files in the issue directory,
        /// </summary>
        /// <returns><c>true</c>, if all files were successfully linted, <c>false</c> otherwise</returns>
        public bool LintAllIssueFiles()
        {
            logger.Info("Preparing to lint all issue files");

            bool filesLintedSuccessfully = true;

            string[] issueFiles = GetAllIssueFiles();

            foreach (string issueFilePath in issueFiles)
            {
                logger.Debug("Preparing to lint issue file located at: {0}", issueFilePath);

                if (!LintIssueFile(issueFilePath))
                {
                    filesLintedSuccessfully = false;
                }
            }

            logger.Info("Finished linting all issue files");
            logger.Debug("Successful linting of all issue files: {0}", filesLintedSuccessfully);
            issueFilesHaveBeenLinted = filesLintedSuccessfully;
            return filesLintedSuccessfully;
        }

        /// <summary>
        /// Lints the issue file passed to the function
        /// </summary>
        /// <returns><c>true</c>, if file was successfully linted, <c>false</c> otherwise</returns>
        /// <param name="issueFilePath">The full path to the issue file</param>
        public bool LintIssueFile(string issueFilePath)
        {
            var issueFileContent = File.ReadAllText(issueFilePath);

            try
            {
                dynamic issueFileJSON = JsonConvert.DeserializeObject(issueFileContent, new JsonSerializerSettings
                                        {
                                            Error = HandleDeserializationError
                                        });
            }
            catch (JsonSerializationException e)
            {
                // There was a problem parsing the JSON file
                logger.Error("The issue file at \"{0}\" could not be linted correctly", issueFilePath);
                logger.Error("The error message is: {0}", e.Message);
                return false;
            }

            // The JSON format of this file is fine
            logger.Debug("The issue file at \"{0}\" was linted correctly", issueFilePath);
            return true;
        }

        /// <summary>
        /// Loads all of the issue files to the config class for them
        /// to be used when performing the checks
        /// </summary>
        public void LoadAllIssueFiles()
        {
            logger.Info("Preparing to load all issue files");

            if (!issueFilesHaveBeenLinted)
            {
                logger.Warn("Issue files have not been linted, there may be problems loading them");
            }

            string[] issueFiles = GetAllIssueFiles();

            foreach (string issueFilePath in issueFiles)
            {
                logger.Debug("Preparing to load issue file located at: {0}", issueFilePath);
                LoadIssueFile(issueFilePath);
            }

            logger.Info("Finished loading all issue files");
        }

        /// <summary>
        /// Loads an issue file and saves the JSON structure of it
        /// in the CSSSConfig class
        /// </summary>
        /// <param name="issueFilePath">The full path to the issue file</param>
        private void LoadIssueFile(string issueFilePath)
        {
            var issueFileContent = File.ReadAllText(issueFilePath);

            try
            {
                dynamic issueFileJSON = JsonConvert.DeserializeObject(issueFileContent, new JsonSerializerSettings
                {
                    Error = HandleDeserializationError
                });

                logger.Debug("Issue file category: {0}", issueFileJSON.Category);
                config.AddIssueFile((string)issueFileJSON.Category, issueFileJSON);
            }
            catch (JsonSerializationException e)
            {
                // There was a problem loading the JSON file
                logger.Error("The issue file at \"{0}\" could not be loaded correctly", issueFilePath);
                throw new Exception("An unknown problem occurred when trying to load an issue file: " + e.Message);
            }
            catch (OperationCanceledException e)
            {
                // The issue category already exists
                logger.Error("There was a problem adding the issue file: {0}", e.Message);
            }

            // The JSON format of this file is fine
            logger.Debug("The issue file at \"{0}\" has been loaded", issueFilePath);
        }

        /// <summary>
        /// A handler for any deserialization errors that occur when
        /// linting the issue files, so that the error location can
        /// be shown to the user for them to fix the problem
        /// 
        /// <para>This function throws an exception if there was a
        /// problem deserialising the JSON file, and the error message
        /// is contained in the exception message. This should be
        /// caught by the calling function</para>
        /// 
        /// See: http://stackoverflow.com/a/26108527
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="errorArgs">Error arguments</param>
        private void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
            throw new JsonSerializationException(currentError);
        }

        /// <summary>
        /// Gets all of the issue files in the issue directory
        /// 
        /// See: https://stackoverflow.com/a/18562036
        /// See: https://social.msdn.microsoft.com/Forums/en-US/7d8798db-32eb-4886-9531-31b3decba018/#25e02b75-16d1-44e6-a04c-a6ab5ad88403
        /// </summary>
        /// <returns>A path array to all of the issue files</returns>
        public string[] GetAllIssueFiles()
        {
            string CSSSDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            return Directory.GetFiles(CSSSDirectory + Path.DirectorySeparatorChar + "Issues",
                                      "*.json",
                                      SearchOption.AllDirectories);
        }
    }
}
