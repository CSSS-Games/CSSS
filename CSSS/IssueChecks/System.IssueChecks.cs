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
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program. If not, see <http://www.gnu.org/licenses/>.

using CheckAPI;
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
            dynamic issueFile = LoadIssueFile(issueCategory);
            if (!(issueFile is bool))
            {
                logger.Debug("Performing checks for the category: {0}", issueCategory);

                var versionCheck = new CheckAPI.System.Version();

                // Checking each issue in the file to see if the values
                // listed match with what is expected
                for (int issue = 0; issue < issueFile.Issues.Count; issue++)
                {
                    var operatingSystemVerion = (string)issueFile.Issues[issue].Expected;

                    try
                    {
                        // Checking the current version of the Operating System
                        // against what it is expected to be from the issue file
                        if (versionCheck.ExpectedOSVersion(operatingSystemVerion))
                        {
                            // The issue check matches with the current system state,
                            // so include the points in the total score
                            PointsScored((int)issueFile.Issues[issue].Points,
                                         (string)issueFile.Issues[issue].Description,
                                         (bool)issueFile.Issues[issue].Triggered);

                            // The check for this issue has been triggered,
                            // so update the JSON to reflect this
                            issueFile.Issues[issue].Triggered = true;
                        }
                        else
                        {
                            // Seeing if the issue was already resolved and has
                            // been broken again, as the check returned false
                            if (issueFile.Issues[issue].Triggered == true)
                            {
                                PointsLost((int)issueFile.Issues[issue].Points,
                                           (string)issueFile.Issues[issue].Description);

                                issueFile.Issues[issue].Triggered = false;
                            }
                        }
                    }
                    catch (NotImplementedException e)
                    {
                        // If the check is attempted on an Operating System that
                        // doesn't support it, it will throw an exception which
                        // will be caught here. This function is returned from here
                        // instead of looping through each possible issue on the basis
                        // of "if it can't be completed once, there's no point retrying"
                        logger.Warn("Unable to perform {0} check: {1}", issueCategory, e.Message);
                        return;
                    }
                }

                logger.Debug("Finished performing checks for the category: {0}", issueCategory);
            }
        }
    }
}
