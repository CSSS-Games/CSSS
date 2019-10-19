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
using CheckAPI.System;
using CSSSConfig;
using NUnit.Framework;

namespace CSSSCheckerEngineTests.Issues.System
{
    [TestFixture()]
    class RegistryTests
    {
        private Registry registrySystemChecks;

        private static Config config;

        /// <summary>
        /// The root hive to use for testing the registry class
        /// </summary>
        private string HiveRoot;

        /// <summary>
        /// Creates the paths to the files that are to be checked
        /// </summary>
        [OneTimeSetUp]
        protected void OneTimeSetUp()
        {
            // The operating system needs to be set due to the `Registry` class
            // using it get the relevant checking code
            config = Config.GetCurrentConfig;
            config.operatingSystemType = SetOperatingSystemType();

            if (config.operatingSystemType != Config.OperatingSystemType.WinNT)
            {
                Assert.Ignore("Registry tests have been ignored due to this Operating System not having a registry");
            }

            registrySystemChecks = new Registry();

            HiveRoot = "SOFTWARE\\CSSS";
            Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot);
        }

        /// <summary>
        /// Deletes the registry keys that are to be checked (to keep things tidy)
        /// </summary>
        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            // `DeleteSubKeyTree` is used to save having to delete any subkeys
            // that are created, as `DeleteSubKey` doesn't do this
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(HiveRoot, false);
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
        public void TestRegistryValueWithExpectedStringValueMatchingReturnsTrue()
        {
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\StringTests");
            rk.SetValue("StringValueMatching", "ExpectedValue");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\StringTests";
            var registryName = "StringValueMatching";
            var registryValue = "ExpectedValue";
            Assert.IsTrue(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values with string values matching should return true");
        }

        [Test()]
        public void TestRegistryValueWithExpectedStringValueNotMatchingReturnsFalse()
        {
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\StringTests");
            rk.SetValue("StringValueNotMatching", "ActualValue");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\StringTests";
            var registryName = "StringValueNotMatching";
            var registryValue = "ExpectedValue";
            Assert.IsFalse(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values with string values not matching should return false");
        }

        [Test()]
        public void TestRegistryValueWithExpectedDWORDValueMatchingReturnsTrue()
        {
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\DWORDTests");
            rk.SetValue("DWORDValueMatching", 2);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\DWORDTests";
            var registryName = "DWORDValueMatching";
            var registryValue = 2.ToString();
            Assert.IsTrue(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values with DWORD values matching should return true");
        }

        [Test()]
        public void TestRegistryValueWithExpectedDWORDValueNotMatchingReturnsFalse()
        {
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\DWORDTests");
            rk.SetValue("DWORDValueNotMatching", 3);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\DWORDTests";
            var registryName = "DWORDValueNotMatching";
            var registryValue = 2.ToString();
            Assert.IsFalse(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values with DWORD values not matching should return false");
        }

        [Test()]
        public void TestRegistryValueWithExpectedBINARYValueMatchingReturnsTrue()
        {
            // `binaryValue` = "C\0S\0S\0S\0"
            byte[] binaryValue = { 67, 0, 83, 0, 83, 0, 83, 0 };
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\BINARYTests");
            rk.SetValue("BINARYValueMatching", binaryValue);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\BINARYTests";
            var registryName = "BINARYValueMatching";
            var registryValue = "43,00,53,00,53,00,53,00";
            Assert.IsTrue(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values with BINARY values matching should return true");
        }

        [Test()]
        public void TestRegistryValueWithExpectedBINARYValueNotMatchingReturnsFalse()
        {
            // `binaryValue` = "404"
            byte[] binaryValue = { 52, 48, 52 };
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\BINARYTests");
            rk.SetValue("BINARYValueNotMatching", binaryValue);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\BINARYTests";
            var registryName = "BINARYValueNotMatching";
            var registryValue = "43,00,53,00,53,00,53,00";
            Assert.IsFalse(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values with BINARY values not matching should return false");
        }

        [Test()]
        public void TestRegistryValueMissingValueMatchingReturnsTrue()
        {
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\ValueTests");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests";
            var registryName = "404";
            var registryValue = (string)null;
            Assert.IsTrue(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values not existing that should not exist should return true");
        }

        [Test()]
        public void TestRegistryValueMissingValueNotMatchingReturnsFalse()
        {
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\ValueTests");
            rk.SetValue("ShouldBeRemoved", 404);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests";
            var registryName = "ShouldBeRemoved";
            var registryValue = (string)null;
            Assert.IsFalse(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values existing that should not exist should return false");
        }

        [Test()]
        public void TestRegistryKeyMissingValueMatchingReturnsTrue()
        {
            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests\\404";
            var registryName = "";
            var registryValue = (string)null;
            Assert.IsTrue(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry keys not existing that should not exist should return true");
        }

        [Test()]
        public void TestRegistryKeyMissingValueNotMatchingReturnsFalse()
        {
            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests\\404";
            var registryName = "KeyShouldExist";
            var registryValue = "KeyShouldExist";
            Assert.IsFalse(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry keys not existing that should exist should return false");
        }

        [Test()]
        public void TestRegistryValueWithoutPermissionReturnsFalse()
        {
            var registryPath = "HKEY_LOCAL_MACHINE\\SAM\\SAM\\Domains\\Account";
            var registryName = "F";
            var registryValue = (string)null;
            Assert.IsFalse(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue),
                          "Registry values accessed without permission return false");
        }
    }
}
