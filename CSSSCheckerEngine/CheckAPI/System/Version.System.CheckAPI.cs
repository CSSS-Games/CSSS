//  CSSSCheckerEngine - CyberSecurity Scoring System Checker Engine
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

using CSSSConfig;
using NLog;
using OS.Linux.System;
using OS.WinNT.System;
using System;

namespace CheckAPI.System
{
    /// <summary>
    /// Public API to check the version of the installed Operating
    /// System against the issue file to verify the correct version
    /// is installed and has been updated
    /// </summary>
    public class Version
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Compares the current running Operating System version against
        /// the expected number in the issue check file
        /// </summary>
        /// <returns><c>true</c>, if OS version matches what is expecteded, <c>false</c> otherwise</returns>
        public bool ExpectedOSVersion()
        {
            return VersionSystemFactory.GetCurrentOperatingSystemClass().ExpectedOSVersion;
        }
    }
}

/// <summary>
/// Version system factory to create the relevant Operating System
/// specific classes for version checks to be carried out
/// </summary>
static class VersionSystemFactory
{
    private static Config config = Config.GetCurrentConfig;

    public static Checker.System.Version GetCurrentOperatingSystemClass()
    {
        switch (config.operatingSystemType)
        {
            case Config.OperatingSystemType.Linux:
                return new VersionSystemLinux();
            case Config.OperatingSystemType.WinNT:
                return new VersionSystemWinNT();
            default:
                throw new NotSupportedException("This Operating System is not supported for issue checks to be performed");
        }
    }
}
