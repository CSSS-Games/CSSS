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
using System.Security;
using Microsoft.Win32;

namespace OS.WinNT.System
{
    internal class RegistrySystemWinNT : Checker.System.Registry
    {
        /// <summary>
        /// Compares the current registry settings with those in the
        /// issue file, to see if the issue has been "resolved"
        /// </summary>
        /// <param name="registryPath">The path to the registry hive containing the key</param>
        /// <param name="registryName">The name of the key to look up in the registry hive</param>
        /// <param name="registryValue">The expected value of the registry key</param>
        /// <param name="registryValueShouldMatch">Should the value in the registry match what is in the issue file</param>
        /// <returns><c>true</c>, if the registry key value matches what is expected, <c>false</c> otherwise</returns>
        public override bool CheckRegistryValue(string registryPath, string registryName, string registryValue, bool registryValueShouldMatch)
        {
            try
            {
                // Get the value of the registry key, or `null` if it does
                // not exist
                dynamic value = Registry.GetValue(registryPath, registryName, null);

                // See: https://stackoverflow.com/a/1797610
                if (value == null)
                {
                    if (registryValueShouldMatch)
                    {
                        return string.IsNullOrEmpty(registryValue);
                    }
                    else
                    {
                        return !string.IsNullOrEmpty(registryValue);
                    }
                }

                // Binary values stored in the registry (REG_BINARY) are returned
                // as a numerical byte array. Binary values exported using regedit
                // are a comma-separated hex string. `System.BitConverter` separates
                // the values with a dash '-'
                if (value is byte[])
                {
                    value = BitConverter.ToString(value).Replace('-', ',');
                }

                //Console.WriteLine(registryValueShouldMatch);
                //Console.WriteLine(value.ToString());
                //Console.WriteLine(registryValue);

                if (registryValueShouldMatch)
                {
                    return string.Equals(value.ToString(), registryValue, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    return !string.Equals(value.ToString(), registryValue, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (SecurityException e)
            {
                // `SecurityException` is thrown when attempting to access a
                // registry key that can only be accessed by the SYSTEM account
                logger.Warn("Unable to access the registry key: {0}\\{1}", registryPath, registryName);
                logger.Warn("The error message is: {0}", e.Message);
                logger.Warn("Is CSSS being run with the correct permissions?");
                return false;
            }
        }
    }
}
