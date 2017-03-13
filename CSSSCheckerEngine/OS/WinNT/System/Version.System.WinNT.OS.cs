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
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program. If not, see <http://www.gnu.org/licenses/>.

using Checker.System;
using System;

namespace OS.WinNT.System
{
    internal class VersionSystemWinNT : Checker.System.Version
    {
        /// <summary>
        /// Gets a value indicating whether the OS version currently
        /// installed matches what is expected in the issue json file
        /// </summary>
        /// <param name="OperatingSystemVersion">The Operating System version to compare with the one running</param>
        /// <value><c>true</c> if expected OS Version; otherwise, <c>false</c></value>
        public override bool ExpectedOSVersion(string OperatingSystemVersion)
        {
            // The current Operating System version is stored in the
            // config class when the init class is run, so the value
            // from there can be checked against what is expected
            if (config.OperatingSystemVersion == OperatingSystemVersion)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
