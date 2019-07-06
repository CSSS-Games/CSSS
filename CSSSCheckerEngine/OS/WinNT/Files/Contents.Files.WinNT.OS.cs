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
using System.IO;

namespace OS.WinNT.Files
{
    internal class ContentsFilesWinNT : Checker.Files.Contents
    {
        /// <summary>
        /// Checking the contents of the file path given, and seeing if they
        /// match, according to issue file
        /// </summary>
        /// <param name="filePath">The path to the file to check</param>
        /// <param name="fileContents">The contents to find in the file</param>
        /// <returns><c>true</c>, if the file matches its contents value, <c>false</c> otherwise</returns>
        public override bool CheckFileContents(string filePath, List<string> fileContents)
        {
            // This is set to true here, so that it can be set to false should
            // just one part of the content be missing from the file. This is
            // easier than trying to keep track of how many parts of content
            // may be in the file, and how many are missing
            var allFileContentsFound = true;

            // If the file doesn't exist, then it's not possible to check the file
            // contents
            if (!File.Exists(filePath))
            {
                return false;
            }

            // Read all of the file content, check if the required content from
            // the issue file exists in it, and if not, set `allFileContentsFound`
            // to false so the calling function knows not everything required has
            // been added to the file
            string file = File.ReadAllText(filePath);

            foreach (var content in fileContents)
            {
                if (!file.Contains(content))
                {
                    allFileContentsFound = false;
                }
            }

            return allFileContentsFound;
        }
    }
}
