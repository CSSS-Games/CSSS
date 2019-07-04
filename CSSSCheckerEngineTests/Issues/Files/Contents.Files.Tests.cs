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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CheckAPI.Files;
using CSSSConfig;
using NUnit.Framework;

namespace CSSSCheckerEngineTests.Issues.Files
{
    [TestFixture()]
    class ContentsTests
    {
        private Contents contentsFilesChecks;

        private static Config config;

        /// <summary>
        /// A string to a file that has valid contents
        /// </summary>
        private String validFilePath;

        /// <summary>
        /// A list string of the valid file contents
        /// </summary>
        private List<string> validFileContents;

        /// <summary>
        /// A string to a file that has invalid contents
        /// </summary>
        private String invalidFilePath;

        /// <summary>
        /// A list string of the invalid file contents
        /// </summary>
        private List<string> invalidFileContents;

        /// <summary>
        /// A list string to contain the output that is being expected
        /// </summary>
        private List<string> expectedFileContents;

        /// <summary>
        /// A string to a file that doesn't exist
        /// </summary>
        private String nonExistingFilePath;

        /// <summary>
        /// Creates the paths to the files that are to be checked
        /// </summary>
        [OneTimeSetUp]
        protected void OneTimeSetUp()
        {
            // Using the output directory as the base of the files, as it is
            // going to exist anywhere (due to this file being compiled to it)
            var outputDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName
                                + Path.DirectorySeparatorChar;

            validFilePath = outputDirectory + "Question 1.txt";
            invalidFilePath = outputDirectory + "Question 2.txt";
            nonExistingFilePath = outputDirectory + "404.txt";

            expectedFileContents = new List<string> { "Hello World!" };
            validFileContents = expectedFileContents;
            invalidFileContents = new List<string> { "Hello World" };

            File.WriteAllLines(validFilePath, validFileContents);
            File.WriteAllLines(invalidFilePath, invalidFileContents);

            // The operating system needs to be set due to the `Contents` class
            // using it get the relevant checking code
            config = Config.GetCurrentConfig;
            config.operatingSystemType = SetOperatingSystemType();
            contentsFilesChecks = new Contents();
        }

        /// <summary>
        /// Deletes the files that are to be checked (to keep things tidy)
        /// </summary>
        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            File.Delete(validFilePath);
            File.Delete(invalidFilePath);
        }

        /// <summary>
        /// Sets the current operating system type that CSSS is running on
        /// in the <see cref="T:CSSS.Config"/> class
        /// 
        /// <para>This is a stub of the function in `init.cs` in the CSSS project</para>
        /// </summary>
        /// <returns>The current operating system</returns>
        public Config.OperatingSystemType SetOperatingSystemType()
        {
            // Seeing if CSSSCheckerEngineTests is running on WinNT
            if (Path.DirectorySeparatorChar == '\\')
            {
                return Config.OperatingSystemType.WinNT;
            }

            // Attempting to identify the Unix-based OS based on the returned
            // string from the `uname -s` command
            // While currently only Linux is also supported by CSSS, a switch
            // function is used here should additional OS support be added in
            // the future
            switch (ReadProcessOutput("uname", "-s").ToLower())
            {
                case "linux":
                    return Config.OperatingSystemType.Linux;
                default:
                    break;
            }

            // We haven't been able to work out what Operating System CSSS
            // is running on, so throw an error. It is up to the calling
            // function to do something with this
            throw new NotImplementedException("CSSSCheckerEngineTests do not support running on your Operating System");
        }

        /// <summary>
        /// Reads system program process output to determine information about
        /// the current Operating System type
        /// 
        /// <para>For Unix-like Operating Systems various commands can be used
        /// to dertermine some information about the current computer, such as
        /// the kernel type and version. This function serves as a wrapper to
        /// those programs to return any data to CSSS</para>
        /// 
        /// <para>See: https://blez.wordpress.com/2012/09/17/determine-os-with-netmono/</para>
        /// 
        /// <para>This is a stub of the function in `init.cs` in the CSSS project</para>
        /// </summary>
        /// <returns>Any string returned from the program output</returns>
        /// <param name="name">The name of the program to get system information</param>
        /// <param name="args">Any arguments to pass to the program</param>
        private static string ReadProcessOutput(string name, string args)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                if (args != null && args != "") p.StartInfo.Arguments = " " + args;
                p.StartInfo.FileName = name;
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (output == null) output = "";
                output = output.Trim();
                return output;
            }
            catch
            {
                return "";
            }
        }

        [Test()]
        public void TestFileDoesExistAndDoesHaveValidContentReturnsTrue()
        {
            Assert.IsTrue(contentsFilesChecks.CheckFileContents(validFilePath, expectedFileContents),
                          "Files that exist and have valid contents should return True");
        }

        [Test()]
        public void TestFileDoesExistAndDoesNotHaveValidContentReturnsFalse()
        {
            Assert.IsFalse(contentsFilesChecks.CheckFileContents(invalidFilePath, expectedFileContents),
                          "Files that exist and have invalid contents should return False");
        }

        [Test()]
        public void TestFileDoesNotExistReturnsFalse()
        {
            Assert.IsFalse(contentsFilesChecks.CheckFileContents(nonExistingFilePath, expectedFileContents),
                          "Files that do not exist should return False");
        }
    }
}
