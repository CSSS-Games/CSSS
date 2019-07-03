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

using System.IO;

namespace OS.Linux.Files
{
    internal class ExistenceFilesLinux : Checker.Files.Existence
    {
        /// <summary>
        /// Checking the current file path given has a file, and if
        /// it should exist or not, according to issue file
        /// </summary>
        /// <param name="filePath">The path to the file to check</param>
        /// <param name="fileShouldExist">Should the file exist at the path, or not</param>
        /// <param name="isPenaltyIssue">If this is a penalty issue, then the response is reversed</param>
        /// <returns><c>true</c>, if the file matches its existence value, <c>false</c> otherwise</returns>
        public override bool CheckFileExistence(string filePath, bool fileShouldExist, bool isPenaltyIssue)
        {
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
            if ((File.Exists(filePath) && fileShouldExist) ||
                (!File.Exists(filePath) && !fileShouldExist))
            {
                if (isPenaltyIssue)
                {
                    return false;
                }

                return true;
            }
            else
            {
                if (isPenaltyIssue)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
