//  CSSS - CyberSecurity Scoring System
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

using CSSSCheckerEngine;
using CSSSConfig;
using NLog;
using System;
using System.Collections.Generic;

namespace IssueChecks
{
    /// <summary>
    /// Any issue check that is to be performed should inherit from this
    /// base class, as it contains functions to load the issue JSON and
    /// update the points scored
    /// </summary>
    public class IssueChecks
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        protected static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Attempts to load the requested issue file from the config
        /// class and return the JSON content to the calling function
        /// </summary>
        /// <returns>The issue file JSON contents, or <c>false</c> otherwise</returns>
        /// <param name="IssueFileCategory">The category of the issue file</param>
        public dynamic LoadIssueFile(string IssueFileCategory)
        {
            try
            {
                return config.GetIssueFile(IssueFileCategory);
            }
            catch (KeyNotFoundException e)
            {
                logger.Debug("Not performing checks for category \"{0}\": {1}", IssueFileCategory, e.Message);
                return false;
            }
        }
    }
}
