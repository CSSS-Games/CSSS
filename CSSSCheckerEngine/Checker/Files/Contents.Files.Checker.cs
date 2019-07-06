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

using System.Collections.Generic;
using CSSSConfig;
using NLog;

namespace Checker.Files
{
    /// <summary>
    /// All checks for different Operating Systems to detect file
    /// contents should use this class as their base, then select
    /// the relevant class from the factory that is created for this
    /// class from the CheckerAPI namespace
    /// </summary>
    abstract class Contents
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        protected static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Checking the contents of the file path given, and seeing if they
        /// match, according to issue file
        /// </summary>
        /// <param name="filePath">The path to the file to check</param>
        /// <param name="fileContents">The contents to find in the file</param>
        /// <returns><c>true</c>, if the file matches its contents value, <c>false</c> otherwise</returns>
        public abstract bool CheckFileContents(string filePath, List<string> fileContents);
    }
}
