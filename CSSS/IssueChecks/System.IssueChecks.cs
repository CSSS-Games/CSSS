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
using Newtonsoft.Json;
using System;

namespace IssueChecks
{
    /// <summary>
    /// All of the available issue checks that can be called from
    /// the CSSSCheckerEngine CheckAPI are available to be called
    /// from this class, and the relevant processing needed to see
    /// if points have been gained or not is also completed
    /// </summary>
    public class System : IssueChecks
    {
        /// <summary>
        /// Performs all of the known issue checks categorised under
        /// "issues.system"
        /// </summary>
        public void PerformAllSystemChecks()
        {
            CheckExpectedOperatingSystemVersion();
        }

        /// <summary>
        /// Compates the current running Operating System version
        /// against the one expected in the issue file, to see if the
        /// latest updated version is installed
        /// </summary>
        public void CheckExpectedOperatingSystemVersion()
        {
            // This is the issue file category that is being looked
            // at for this check
            const string issueCategory = "issues.system.version";

            // If the issue file is not available then any attempts
            // at checking it can be bypassed. This is indicated by
            // a boolean value being returned when trying to load
            // the relevant issue file
            if (!(LoadIssueFile(issueCategory) is bool))
            {
                // Todo: Perform checks
            }
        }
    }
}
