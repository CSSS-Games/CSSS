//  CSSSCheckerEngineTests - CyberSecurity Scoring System Checker Engine Tests
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

using System.IO;
using System.Reflection;
using CSSSCheckerEngine;
using CSSSConfig;
using NUnit.Framework;

namespace CSSSCheckerEngineTests
{
    [TestFixture()]
    public class IssueFilesTests
    {
        private IssueFiles issueFilesChecks;

        private static Config config;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSSSCheckerEngineTests.IssueFilesTests"/> class
        /// and copies the issue files to a backup folder, which
        /// is used after each test to restore the issue files
        /// to their original state
        /// </summary>
        public IssueFilesTests()
        {
            try
            {
                // The directory may exist when testing locally, but
                // not on the CI infrastructure
                Directory.Delete(GetIssueFilesDirectory("OriginalIssueFiles"), true);
            }
            catch
            {
                // Not catching anything, as the files didn't already
                // exist, but nothing needs to be done about it
            }

            DirectoryCopy(GetIssueFilesDirectory(),
                          GetIssueFilesDirectory("OriginalIssueFiles"));
        }

        /// <summary>
        /// Creates an instance of the issue files class and
        /// copies the "clean" JSON issue files from the checker
        /// enging class
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            issueFilesChecks = new IssueFiles();
            config = Config.GetCurrentConfig;

            DirectoryCopy(GetIssueFilesDirectory("OriginalIssueFiles"),
                          GetIssueFilesDirectory());
        }

        /// <summary>
        /// Removes any reference to the issue files class and
        /// deletes any changes to the issue check files (such
        /// as encrypting them)
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            issueFilesChecks = null;
            config = null;

            Directory.Delete(GetIssueFilesDirectory(), true);
        }

        /// <summary>
        /// All known issue files are listed here, to see if the
        /// JSON files returned from the issues directory match
        /// </summary>
        /// <returns>An array of paths to the issue files</returns>
        private string[] IssueFilesList()
        {
            // Only the directory under the Issues directory and
            // the name of the JSON file are needed tobe included
            // in this array, as other parent folders are added
            // later on in this function. To separate directories
            // and files, a plus ('+') symbol should be used, which
            // is replaced later on with the relevant Operating
            // System directory path separator
            string[] issueFilesList = {
                "Files+Contents.Files.Issues.json",
                "Files+Existence.Files.Issues.json",
                "System+Registry.System.Issues.json",
                "System+Version.System.Issues.json"
            };

            // Updating the issue files list with full path references
            // and Operating System specific directory separators for
            // each item
            string issueFilesDirectory = GetIssueFilesDirectory();

            for (int issueFile = 0; issueFile < issueFilesList.Length; issueFile++)
            {
                issueFilesList[issueFile] = issueFilesDirectory
                                          + issueFilesList[issueFile].Replace("+", Path.DirectorySeparatorChar.ToString());
            }

            return issueFilesList;
        }

        /// <summary>
        /// Gets the issue files directory, or a temporary location
        /// to store them to copy them back after each test run
        /// </summary>
        /// <param name="issuesDirectoryName">The name of the folder the issues files are stored in</param>
        /// <returns>The issue files directory path</returns>
        private string GetIssueFilesDirectory(string issuesDirectoryName = "Issues")
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName
                                + Path.DirectorySeparatorChar
                                + issuesDirectoryName
                                + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Copies all files from one directory to another
        /// 
        /// Note: Overwriting any issues files has been turned on
        ///       to prevent exceptions being thrown about the files
        ///       already existing, which is different from the 
        ///       original Microsoft code 
        /// 
        /// See: https://msdn.microsoft.com/en-us/library/bb762914(v=vs.110).aspx
        /// </summary>
        /// <param name="sourceDirName">The source directory</param>
        /// <param name="destDirName">The destination directory</param>
        /// <param name="copySubDirs">If set to <c>true</c> copy sub dirs.</param>
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// All of the issue files included in the checker engine
        /// project should be correctly formatted JSON files and can
        /// be linted without any problems. This is testing that
        /// a valid JSON object is in the file, not the contents of
        /// the JSON files e.g. false isn't spelt flase
        /// </summary>
        [Test()]
        public void TestAllIssueFilesCanBeLinted()
        {
            Assert.IsTrue(issueFilesChecks.LintAllIssueFiles(),
                          "All issue files included with CSSS should be valid JSON objects and should lint without errors");
        }

        /// <summary>
        /// Tests if each individual issue file can be linted properly
        /// and doesn't return false. This test can be used in conjunction
        /// with the TestAllIssueFilesCanBeLinted to see what issue JSON
        /// file may be at fault
        /// </summary>
        /// <see cref="TestAllIssueFilesCanBeLinted()"/>
        [Test()]
        public void TestIndividualIssueFileCanBeLinted()
        {
            string[] issueFiles = IssueFilesList();
            string currentIssueFile = "";

            for (int issueFile = 0; issueFile < issueFiles.Length; issueFile++)
            {
                currentIssueFile = issueFiles[issueFile];
                Assert.IsTrue(issueFilesChecks.LintIssueFile(currentIssueFile),
                              "The issue file located at \"" + currentIssueFile + "\" could not be linted properly");
            }
        }

        /// <summary>
        /// Any file in the issues directory with a JSON extension
        /// is assumed to be an issue file. All known issue files
        /// are compared to the array that is collected from the
        /// checker engine issue files class
        /// </summary>
        [Test()]
        public void TestAllIssueFilesAreCollected()
        {
            CollectionAssert.AreEquivalent(IssueFilesList(),
                            issueFilesChecks.GetAllIssueFiles(),
                            "All JSON files in the issues directory should be a known issue check file");
        }

        /// <summary>
        /// Tests if the JSON issue files are deleted from the
        /// issues directory when they are prepared
        /// </summary>
        [Test()]
        public void TestAllIssueFilesAreDeletedWhenPrepared()
        {
            issueFilesChecks.PrepareAllIssueFiles();

            Assert.AreNotEqual(IssueFilesList(),
                               issueFilesChecks.GetAllIssueFiles(),
                               "All JSON files in the issues directory should be removed once prepared");
        }

        /// <summary>
        /// Tests all prepared issue files can be decrypted
        /// </summary>
        [Test()]
        public void TestAllPreparedIssueFilesCanBeDecrypted()
        {
            issueFilesChecks.PrepareAllIssueFiles();

            // The issue files are only decrypted when CSSS
            // is in "start" mode
            config.CSSSProgramMode = config.CSSSProgramMode | Config.CSSSModes.Start;

            issueFilesChecks.LoadAllIssueFiles();

            // If the issue files were loaded, then it should
            // be possible to get the JSON from it
            Assert.IsInstanceOf<dynamic>(config.GetIssueFile("issues.system.version"),
                                         "Issue files should be able to be decrypted once they have been prepared");
        }
    }
}
