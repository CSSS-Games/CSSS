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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CheckAPI.Files;
using CSSSConfig;
using NUnit.Framework;

namespace CSSSCheckerEngineTests.Issues.Files
{
    [TestFixture()]
    class ExistenceTests
    {
        private Existence existenceFilesChecks;

        private static Config config;

        /// <summary>
        /// A string to a file that exists
        /// </summary>
        private String existingFilePath;

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

            existingFilePath = outputDirectory + "CSSSCheckerEngine.dll";
            nonExistingFilePath = outputDirectory + "404.exe";

            // The operating system needs to be set due to the `Existence` class
            // using it get the relevant checking code
            config = Config.GetCurrentConfig;
            config.operatingSystemType = SetOperatingSystemType();
            existenceFilesChecks = new Existence();
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

        // There are four options that can happen when checking to see if
        // the file exists. These are:
        //   * 1: File exists, it should exist
        //   * 2: File exists, it shouldn't exist
        //   * 3: File doesn't exist, it should exist
        //   * 4: File doesn't exist, it shouldn't exist
        //
        // For each of the above, the following should happen:
        //   * 1: Gain points
        //   * 2: Loose points
        //   * 3: Loose points
        //   * 4: Gain points
        //
        // However, should this be a check that issues a penalty, then the
        // reverse of the above should happen:
        //   * 1: Loose points
        //   * 2: Gain points
        //   * 3: Gain points
        //   * 4: Loose points
        //
        // A "gain points" should return `True` - "loose points" returns `False`

        [Test()]
        public void TestFileExistsAndShouldExistAndIsNotPenaltyIssue()
        {
            Assert.IsTrue(existenceFilesChecks.CheckFileExistence(existingFilePath, true, false),
                          "Files that exist and should exist and aren't a penalty should return True");
        }
    }
}
