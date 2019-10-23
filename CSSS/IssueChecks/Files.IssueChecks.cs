//  CSSS - CyberSecurity Scoring System
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

namespace IssueChecks
{
    /// <summary>
    /// All of the available issue checks that can be called from
    /// the CSSSCheckerEngine CheckAPI are available to be called
    /// from this class, and the relevant processing needed to see
    /// if points have been gained or not is also completed
    /// </summary>
    public class Files : IssueChecks
    {
        /// <summary>
        /// Performs all of the known issue checks categorised under
        /// "issues.files"
        /// </summary>
        public void PerformAllFilesChecks()
        {
            CheckFilesContents();
            CheckFilesExistence();
        }

        /// <summary>
        /// Checks the file contents with the values in the issue file,
        /// to see if they do or don't match what is expected
        /// </summary>
        public void CheckFilesContents()
        {
            // This is the issue file category that is being looked
            // at for this check
            const string issueCategory = "issues.files.contents";

            // If the issue file is not available then any attempts
            // at checking it can be bypassed. This is indicated by
            // a boolean value being returned when trying to load
            // the relevant issue file
            dynamic issueFile = LoadIssueFile(issueCategory);
            if (!(issueFile is bool))
            {
                logger.Debug("Performing checks for the category: {0}", issueCategory);

                var fileCheck = new CheckAPI.Files.Contents();

                // Checking each issue in the file to see if the values
                // listed match with what is expected
                for (int issue = 0; issue < issueFile.Issues.Count; issue++)
                {
                    // Adding to the total number of issue to find, but
                    // only if the points available are more than 0
                    if ((int)issueFile.Issues[issue].Points > 0)
                    {
                        config.TotalIssues += 1;
                    }

                    var filePath = (string)issueFile.Issues[issue].Path;
                    // Calling `ToObject` is needed to convert from `Newtonsoft.Json.Linq.JArray`
                    // so that the contents of the file can be looped over
                    // See: https://stackoverflow.com/a/13565373
                    var fileContents = issueFile.Issues[issue].Contents.ToObject<List<String>>();

                    try
                    {
                        // Checking the current file contents match, according to
                        // the issue file
                        if (fileCheck.CheckFileContents(filePath, fileContents))
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

        /// <summary>
        /// Compares the files that are currently on the system against
        /// the ones expected in the issue file, to see if they exist or
        /// have been removed as needed
        /// </summary>
        public void CheckFilesExistence()
        {
            // This is the issue file category that is being looked
            // at for this check
            const string issueCategory = "issues.files.existence";

            // If the issue file is not available then any attempts
            // at checking it can be bypassed. This is indicated by
            // a boolean value being returned when trying to load
            // the relevant issue file
            dynamic issueFile = LoadIssueFile(issueCategory);
            if (!(issueFile is bool))
            {
                logger.Debug("Performing checks for the category: {0}", issueCategory);

                var fileCheck = new CheckAPI.Files.Existence();
                var isPenaltyIssue = new Boolean();

                // Checking each issue in the file to see if the values
                // listed match with what is expected
                for (int issue = 0; issue < issueFile.Issues.Count; issue++)
                {
                    // Resetting the `isPenaltyIssue` boolean ready for the next issue
                    isPenaltyIssue = true;

                    // Adding to the total number of issue to find, but
                    // only if the points available are more than 0
                    if ((int)issueFile.Issues[issue].Points > 0)
                    {
                        config.TotalIssues += 1;
                        isPenaltyIssue = false;
                    }

                    var filePath = (string)issueFile.Issues[issue].Path;
                    var fileShouldExist = (bool)issueFile.Issues[issue].FileShouldExist;

                    try
                    {
                        // Checking the current file path given has a file, and if
                        // it should exist or not, according to issue file
                        if (fileCheck.CheckFileExistence(filePath, fileShouldExist, isPenaltyIssue))
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
