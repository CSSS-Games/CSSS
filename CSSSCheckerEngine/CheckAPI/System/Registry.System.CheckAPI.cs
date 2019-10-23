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

using System;
using CSSSConfig;
using NLog;
using OS.WinNT.System;

namespace CheckAPI.System
{
    /// <summary>
    /// Public API to check the registry against the issue file,
    /// to see if the issue has been "resolved"
    /// </summary>
    public class Registry
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Compares the current registry settings with those in the
        /// issue file, to see if the issue has been "resolved"
        /// </summary>
        /// <param name="registryPath">The path to the registry hive containing the key</param>
        /// <param name="registryName">The name of the key to look up in the registry hive</param>
        /// <param name="registryValue">The expected value of the registry key</param>
        /// <param name="registryValueShouldMatch">Should the value in the registry match what is in the issue file</param>
        /// <returns><c>true</c>, if the registry key value matches what is expected, <c>false</c> otherwise</returns>
        public bool CheckRegistryValue(string registryPath, string registryName, string registryValue, bool registryValueShouldMatch)
        {
            return RegistrySystemFactory.GetCurrentOperatingSystemClass().CheckRegistryValue(registryPath, registryName, registryValue, registryValueShouldMatch);
        }
    }
}

/// <summary>
/// Registry system factory to create the relevant Operating System
/// specific classes for registry checks to be carried out
/// </summary>
static class RegistrySystemFactory
{
    private static Config config = Config.GetCurrentConfig;

    public static Checker.System.Registry GetCurrentOperatingSystemClass()
    {
        // Only WinNT uses the concept of a registry, so other Operating
        // Systems cannot do anything with these checks
        switch (config.operatingSystemType)
        {
            case Config.OperatingSystemType.WinNT:
                return new RegistrySystemWinNT();
            default:
                throw new NotSupportedException("This Operating System is not supported for registry issue checks to be performed");
        }
    }
}
