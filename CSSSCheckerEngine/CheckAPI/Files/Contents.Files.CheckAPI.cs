﻿//  CSSSCheckerEngine - CyberSecurity Scoring System Checker Engine
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
using System.Collections.Generic;
using CSSSConfig;
using NLog;
using OS.Linux.Files;
using OS.WinNT.Files;

namespace CheckAPI.Files
{
    /// <summary>
    /// Public API to check the version of the installed Operating
    /// System against the issue file to verify the correct version
    /// is installed and has been updated
    /// </summary>
    public class Contents
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Checking the contents of the file path given, and seeing if they
        /// match, according to issue file
        /// </summary>
        /// <param name="filePath">The path to the file to check</param>
        /// <param name="fileContents">The contents to find in the file</param>
        /// <returns><c>true</c>, if the file matches its contents value, <c>false</c> otherwise</returns>
        public bool CheckFileContents(string filePath, List<string> fileContents)
        {
            return ContentsFilesFactory.GetCurrentOperatingSystemClass().CheckFileContents(filePath, fileContents);
        }
    }
}

/// <summary>
/// Contents.Files factory to create the relevant Operating System
/// specific classes for file content checks to be carried out
/// </summary>
static class ContentsFilesFactory
{
    private static Config config = Config.GetCurrentConfig;

    public static Checker.Files.Contents GetCurrentOperatingSystemClass()
    {
        switch (config.operatingSystemType)
        {
            case Config.OperatingSystemType.Linux:
                return new ContentsFilesLinux();
            case Config.OperatingSystemType.WinNT:
                return new ContentsFilesWinNT();
            default:
                throw new NotSupportedException("This Operating System is not supported for issue checks to be performed");
        }
    }
}
