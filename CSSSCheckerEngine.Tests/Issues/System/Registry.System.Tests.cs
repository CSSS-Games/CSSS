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

using System.Runtime.InteropServices;
using CheckAPI.System;
using CSSSConfig;

namespace CSSSCheckerEngineTests.Issues.System
{
    [TestFixture()]
    class RegistryTests
    {
        private Registry registrySystemChecks;

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
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }
            Config.GetCurrentConfig.operatingSystemType = Config.OperatingSystemType.WinNT;
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // `DeleteSubKeyTree` is used to save having to delete any subkeys
                // that are created, as `DeleteSubKey` doesn't do this
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(HiveRoot, false);
            }
        }

        [Test()]
        public void TestRegistryValueWithExpectedStringValueMatchingAndShouldMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\StringTests");
            rk.SetValue("StringValueMatching", "ExpectedValue");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\StringTests";
            var registryName = "StringValueMatching";
            var registryValue = "ExpectedValue";
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.True, "Registry values with string values matching and should match should return true");
        }

        [Test()]
        public void TestRegistryValueWithExpectedStringValueMatchingAndShouldNotMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\StringTests");
            rk.SetValue("StringValueMatching", "ExpectedValue");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\StringTests";
            var registryName = "StringValueMatching";
            var registryValue = "ExpectedValue";
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.False, "Registry values with string values matching and should not match should return false");
        }

        [Test()]
        public void TestRegistryValueWithExpectedStringValueNotMatchingAndShouldMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\StringTests");
            rk.SetValue("StringValueNotMatching", "ActualValue");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\StringTests";
            var registryName = "StringValueNotMatching";
            var registryValue = "ExpectedValue";
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.False, "Registry values with string values not matching and should match should return false");
        }

        [Test()]
        public void TestRegistryValueWithExpectedStringValueNotMatchingAndShouldNotMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\StringTests");
            rk.SetValue("StringValueNotMatching", "ActualValue");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\StringTests";
            var registryName = "StringValueNotMatching";
            var registryValue = "ExpectedValue";
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.True, "Registry values with string values not matching and should not match should return true");
        }

        [Test()]
        public void TestRegistryValueWithExpectedDWORDValueMatchingAndShouldMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\DWORDTests");
            rk.SetValue("DWORDValueMatching", 2);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\DWORDTests";
            var registryName = "DWORDValueMatching";
            var registryValue = 2.ToString();
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.True, "Registry values with DWORD values matching and should match should return true");
        }

        [Test()]
        public void TestRegistryValueWithExpectedDWORDValueMatchingAndShouldNotMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\DWORDTests");
            rk.SetValue("DWORDValueMatching", 2);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\DWORDTests";
            var registryName = "DWORDValueMatching";
            var registryValue = 2.ToString();
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.False, "Registry values with DWORD values matching and should not match should return false");
        }

        [Test()]
        public void TestRegistryValueWithExpectedDWORDValueNotMatchingAndShouldMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\DWORDTests");
            rk.SetValue("DWORDValueNotMatching", 3);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\DWORDTests";
            var registryName = "DWORDValueNotMatching";
            var registryValue = 2.ToString();
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.False, "Registry values with DWORD values not matching and should match should return false");
        }

        [Test()]
        public void TestRegistryValueWithExpectedDWORDValueNotMatchingAndShouldNotMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\DWORDTests");
            rk.SetValue("DWORDValueNotMatching", 3);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\DWORDTests";
            var registryName = "DWORDValueNotMatching";
            var registryValue = 2.ToString();
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.True, "Registry values with DWORD values not matching and should not match should return true");
        }

        [Test()]
        public void TestRegistryValueWithExpectedBINARYValueMatchingAndShouldMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            // `binaryValue` = "C\0S\0S\0S\0"
            byte[] binaryValue = { 67, 0, 83, 0, 83, 0, 83, 0 };
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\BINARYTests");
            rk.SetValue("BINARYValueMatching", binaryValue);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\BINARYTests";
            var registryName = "BINARYValueMatching";
            var registryValue = "43,00,53,00,53,00,53,00";
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.True, "Registry values with BINARY values matching and should match should return true");
        }

        [Test()]
        public void TestRegistryValueWithExpectedBINARYValueMatchingAndShouldNotMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            // `binaryValue` = "C\0S\0S\0S\0"
            byte[] binaryValue = { 67, 0, 83, 0, 83, 0, 83, 0 };
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\BINARYTests");
            rk.SetValue("BINARYValueMatching", binaryValue);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\BINARYTests";
            var registryName = "BINARYValueMatching";
            var registryValue = "43,00,53,00,53,00,53,00";
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.False, "Registry values with BINARY values matching and should not match should return false");
        }

        [Test()]
        public void TestRegistryValueWithExpectedBINARYValueNotMatchingAndShouldMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            // `binaryValue` = "404"
            byte[] binaryValue = { 52, 48, 52 };
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\BINARYTests");
            rk.SetValue("BINARYValueNotMatching", binaryValue);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\BINARYTests";
            var registryName = "BINARYValueNotMatching";
            var registryValue = "43,00,53,00,53,00,53,00";
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.False, "Registry values with BINARY values not matching and should match should return false");
        }

        [Test()]
        public void TestRegistryValueWithExpectedBINARYValueNotMatchingAndShouldNotMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            // `binaryValue` = "404"
            byte[] binaryValue = { 52, 48, 52 };
            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\BINARYTests");
            rk.SetValue("BINARYValueNotMatching", binaryValue);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\BINARYTests";
            var registryName = "BINARYValueNotMatching";
            var registryValue = "43,00,53,00,53,00,53,00";
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.True, "Registry values with BINARY values not matching and should not match should return true");
        }

        [Test()]
        public void TestRegistryValueMissingValueMatchingAndShouldMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\ValueTests");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests";
            var registryName = "404";
            var registryValue = (string?)null;
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.True, "Registry values not existing that should not exist and should match should return true");
        }

        [Test()]
        public void TestRegistryValueMissingValueMatchingAndShouldNotMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\ValueTests");

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests";
            var registryName = "404";
            var registryValue = (string?)null;
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch),
                        Is.False,
                        "Registry values not existing that should not exist and should not match should return false");
        }

        [Test()]
        public void TestRegistryValueMissingValueNotMatchingAndShouldMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\ValueTests");
            rk.SetValue("ShouldBeRemoved", 404);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests";
            var registryName = "ShouldBeRemoved";
            var registryValue = (string?)null;
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch),
                        Is.False,
                        "Registry values existing that should not exist and should match should return false");
        }

        [Test()]
        public void TestRegistryValueMissingValueNotMatchingAndShouldNotMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(HiveRoot + "\\ValueTests");
            rk.SetValue("ShouldBeRemoved", 404);

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests";
            var registryName = "ShouldBeRemoved";
            var registryValue = (string?)null;
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch),
                        Is.True,
                        "Registry values existing that should not exist and should not match should return true");
        }

        [Test()]
        public void TestRegistryKeyMissingValueMatchingAndShouldMatchReturnsTrue()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests\\404";
            var registryName = "";
            var registryValue = (string?)null;
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch),
                        Is.True,
                        "Registry keys not existing that should not exist and should match should return true");
        }

        [Test()]
        public void TestRegistryKeyMissingValueMatchingAndShouldNotMatchReturnsFalse()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Ignore("Running Tests on non-Windows platform");
                return;
            }

            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests\\404";
            var registryName = "";
            var registryValue = (string?)null;
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch),
                        Is.False,
                        "Registry keys not existing that should not exist and should not match should return false");
        }

        [Test()]
        public void TestRegistryKeyMissingValueNotMatchingAndShouldMatchReturnsFalse()
        {
            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests\\404";
            var registryName = "KeyShouldExist";
            var registryValue = "KeyShouldExist";
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.False, "Registry keys not existing that should exist and should match should return false");
        }

        [Test()]
        public void TestRegistryKeyMissingValueNotMatchingAndShouldNotMatchReturnsTrue()
        {
            var registryPath = "HKEY_CURRENT_USER\\SOFTWARE\\CSSS\\ValueTests\\404";
            var registryName = "KeyShouldExist";
            var registryValue = "KeyShouldExist";
            bool registryValueShouldMatch = false;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch), Is.True, "Registry keys not existing that should exist and should not match should return true");
        }

        [Test()]
        public void TestRegistryValueWithoutPermissionReturnsFalse()
        {
            var registryPath = "HKEY_LOCAL_MACHINE\\SAM\\SAM\\Domains\\Account";
            var registryName = "F";
            var registryValue = (string?)null;
            bool registryValueShouldMatch = true;
            Assert.That(registrySystemChecks.CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch),
                        Is.False,
                        "Registry values accessed without permission return false");
        }
    }
}
