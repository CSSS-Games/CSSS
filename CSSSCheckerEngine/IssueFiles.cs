//  CSSSCheckerEngine - CyberSecurity Scoring System Checker Engine
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

using System.Reflection;
using System.Security.Cryptography;
using CSSSConfig;
using Newtonsoft.Json;
using NLog;

namespace CSSSCheckerEngine
{
    /// <summary>
    /// Performs the loading and linting of the issue files
    /// that are used to check the current image state against
    /// what is expected
    /// </summary>
    public class IssueFiles
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static readonly Config config = Config.GetCurrentConfig;

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
                dynamic? issueFileJSON = JsonConvert.DeserializeObject(issueFileContent, new JsonSerializerSettings
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
        /// 
        /// <para>If there are any problems loading the issue files,
        /// depending on the severity of the problem, either this
        /// function will throw an exception to halt execution or
        /// just warn the user there is a problem</para>
        /// </summary>
        /// <param name="issueFilePath">The full path to the issue file</param>
        private void LoadIssueFile(string issueFilePath)
        {
            var issueFileContent = File.ReadAllText(issueFilePath);

            try
            {
                // If CSSS is in "start" mode, then the JSON file
                // needs to be decrypted before it can be used
                if (config.CSSSProgramMode.HasFlag(Config.CSSSModes.Start))
                {
                    issueFileContent = SupportLibrary.Encryption.String.Decrypt(issueFileContent);
                }

                dynamic? issueFileJSON = JsonConvert.DeserializeObject(issueFileContent, new JsonSerializerSettings
                {
                    Error = HandleDeserializationError
                });
                if (issueFileJSON != null)
                {
                    config.AddIssueFile((string)issueFileJSON.Category, issueFileJSON);
                    logger.Debug("Added issue category: {0}", issueFileJSON.Category);
                }
            }
            catch (CryptographicException e)
            {
                // There was a problem decrypting the issue file contents
                logger.Warn("Unable to decrypt the issue file \"{0}\": {1}", issueFilePath, e.Message);
            }
            catch (FormatException e)
            {
                // There was a problem decrypting the issue file contents
                logger.Warn("Unable to decrypt the issue file \"{0}\": {1}", issueFilePath, e.Message);
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
                logger.Warn("There was a problem adding the issue file: {0}", e.Message);
            }

            // The JSON format of this file is fine
            logger.Debug("The issue file at \"{0}\" has been loaded", issueFilePath);
        }

        /// <summary>
        /// Prepares all issue files by encrypting the contents
        /// of them and saving them under a new filename and
        /// location, so that competitors cannot discover what
        /// issue checks are being carried out
        /// 
        /// <para>If the issue files are all encrypted correctly,
        /// then the plaintext JSON files are deleted</para>
        /// </summary>
        /// <returns><c>true</c>, if all issue files were prepared, <c>false</c> if there were problems</returns>
        public static bool PrepareAllIssueFiles()
        {
            logger.Info("Preparing to encrypt all issue files");

            bool filesPreparedSuccessfully = true;

            string[] issueFiles = GetAllIssueFiles();

            foreach (string issueFilePath in issueFiles)
            {
                logger.Debug("Preparing to encrypt issue file located at: {0}", issueFilePath);

                if (!PrepareIssueFile(issueFilePath))
                {
                    filesPreparedSuccessfully = false;
                }
            }

            logger.Info("Finished encrypting all issue files");
            logger.Debug("Successful encryption of all issue files: {0}", filesPreparedSuccessfully);

            // If all of the issue files have been prepared
            // successfully, then the original plaintext JSON
            // files can be deleted along with any now empty
            // directories
            if (filesPreparedSuccessfully)
            {
                logger.Info("Preparing to delete all plaintext issue files");
                foreach (string issueFilePath in issueFiles)
                {
                    logger.Debug("Deleting plaintext file: {0}", issueFilePath);
                    File.Delete(issueFilePath);
                }

                logger.Info("Removing any empty issue file directories");
                RemoveEmptyIssueDirectories(GetIssueFilesDirectory());
            }

            return filesPreparedSuccessfully;
        }

        /// <summary>
        /// Prepares the issue file by encrypting the contents of it
        /// </summary>
        /// <returns><c>true</c>, if issue file was encrypted, <c>false</c> otherwise</returns>
        /// <param name="issueFilePath">The full path to the issue file</param>
        private static bool PrepareIssueFile(string issueFilePath)
        {
            var issueFilePrepared = true;

            var issueFileContent = File.ReadAllText(issueFilePath);
            issueFileContent = SupportLibrary.Encryption.String.Encrypt(issueFileContent);

            try
            {
                var preparedIssueFilename = GetIssueFilesDirectory() + Path.DirectorySeparatorChar + GenerateFileName() + ".issue";
                File.WriteAllText(preparedIssueFilename, issueFileContent);
                logger.Debug("The issue file at \"{0}\" has been encrypted and saved as: {1}", issueFilePath, preparedIssueFilename);
            }
            catch (Exception e)
            {
                // While not good to use a generic "exception", it should
                // catch any problems when writing the issue file to disk
                logger.Error("There was a problem saving the encrypted issue file: {0}", e.Message);
                issueFilePrepared = false;
            }

            return issueFilePrepared;
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
        private void HandleDeserializationError(object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
            throw new JsonSerializationException(currentError);
        }

        /// <summary>
        /// Gets the filesystem location of the issue file directory
        /// </summary>
        /// <returns>The issue file directory location</returns>
        public static string GetIssueFilesDirectory()
        {
            var CSSSDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            return CSSSDirectory + Path.DirectorySeparatorChar + "Issues";
        }

        /// <summary>
        /// Gets all of the issue files in the issue directory
        /// 
        /// <para>If CSSS is in "Start" mode, then the issue files
        /// have a ".issue" extension when they were prepared.
        /// Otherwise, they have a normal ".json" extension</para>
        /// 
        /// See: https://stackoverflow.com/a/18562036
        /// See: https://social.msdn.microsoft.com/Forums/en-US/7d8798db-32eb-4886-9531-31b3decba018/#25e02b75-16d1-44e6-a04c-a6ab5ad88403
        /// </summary>
        /// <returns>A path array to all of the issue files</returns>
        public static string[] GetAllIssueFiles()
        {
            string issueFileExtension;
            if (config.CSSSProgramMode.HasFlag(Config.CSSSModes.Start))
            {
                issueFileExtension = "*.issue";
            }
            else
            {
                issueFileExtension = "*.json";
            }

            return Directory.GetFiles(GetIssueFilesDirectory(),
                                      issueFileExtension,
                                      SearchOption.AllDirectories);
        }

        /// <summary>
        /// Generates a random name for the prepared issue file, so
        /// the competitors cannot guess what issue checks are taking
        /// place
        /// </summary>
        /// <returns>The file name for the new issue file</returns>
        private static string GenerateFileName()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        /// <summary>
        /// Removes any empty issue directories once the issue files
        /// have been prepared
        /// 
        /// See: http://stackoverflow.com/a/2811654
        /// </summary>
        /// <param name="startLocation">The location to search from</param>
        private static void RemoveEmptyIssueDirectories(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                RemoveEmptyIssueDirectories(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }
    }
}
