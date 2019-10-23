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

using CSSSConfig;
using NLog;

namespace Checker.System
{
    /// <summary>
    /// Public API to check the registry against the issue file,
    /// to see if the issue has been "resolved"
    /// </summary>
    abstract class Registry
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        protected static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Compares the current registry settings with those in the
        /// issue file, to see if the issue has been "resolved"
        /// </summary>
        /// <param name="registryPath">The path to the registry hive containing the key</param>
        /// <param name="registryName">The name of the key to look up in the registry hive</param>
        /// <param name="registryValue">The expected value of the registry key</param>
        /// <param name="registryValueShouldMatch">Should the value in the registry match what is in the issue file</param>
        /// <returns><c>true</c>, if the registry key value matches what is expected, <c>false</c> otherwise</returns>
        public abstract bool CheckRegistryValue(string registryPath, string registryName, string registryValue, bool registryValueShouldMatch);
    }
}
