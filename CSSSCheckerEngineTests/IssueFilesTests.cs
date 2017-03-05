//  CSSSCheckerEngineTests - CyberSecurity Scoring System Checker Engine Tests
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

using CSSSCheckerEngine;
using CSSSConfig;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

namespace CSSSCheckerEngineTests
{
    [TestFixture()]
    public class IssueFilesTests
    {

        private IssueFiles issueFilesChecks;

        private static Config config;

        /// <summary>
        /// Creates an instance of the issue files class
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            issueFilesChecks = new IssueFiles();
            config = Config.GetCurrentConfig;
        }

        /// <summary>
        /// Removes any reference to the issue files class
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            issueFilesChecks = null;
            config = null;
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
                "System+Version.System.Issues.json"
            };

            // Updating the issue files list with full path references
            // and Operating System specific directory separators for
            // each item
            string issueFilesDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName
                                       + Path.DirectorySeparatorChar
                                       + "Issues"
                                       + Path.DirectorySeparatorChar;

            for (int issueFile = 0; issueFile < issueFilesList.Length; issueFile++)
            {
                issueFilesList[issueFile] = issueFilesDirectory
                                          + issueFilesList[issueFile].Replace("+", Path.DirectorySeparatorChar.ToString());
            }

            return issueFilesList;
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
            Assert.AreEqual(IssueFilesList(),
                            issueFilesChecks.GetAllIssueFiles(),
                            "All JSON files in the issues directory should be a known issue check file");
        }
    }
}
