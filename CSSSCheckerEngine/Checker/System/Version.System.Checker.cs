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
    /// All checks for different Operating Systems to detect their
    /// versions should use this class as their base, then select
    /// the relevant class from the factory that is created for this
    /// class from the CheckerAPI namespace
    /// </summary>
    abstract class Version
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        protected static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Gets a value indicating whether the OS version currently
        /// installed matches what is expected in the issue json file
        /// </summary>
        /// <param name="operatingSystemVersion">The Operating System version to compare with the one running</param>
        /// <value><c>true</c> if expected OS Version; otherwise, <c>false</c></value>
        public abstract bool ExpectedOSVersion(string operatingSystemVersion);
    }
}
