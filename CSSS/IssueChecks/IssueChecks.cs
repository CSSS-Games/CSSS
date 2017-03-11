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

        /// <summary>
        /// Adds the points from the current issue to the total score
        /// 
        /// <para>When an issue check returns a 'true' result, this function
        /// is called to update the current total score. The triggered parameter
        /// is passed as this function will always be called when an issue check
        /// is valid, but we don't want to continously inform the user that
        /// points have been scored if it has been scored in a previous
        /// run check from the Kernel</para>
        /// 
        /// <para>Double negatives apply here. If the points scored are a
        /// positive value, then the total score increases. If the points
        /// are a negative, then points have been lost from the total. The
        /// reasoning for this is that if a issue check returns true for
        /// an issue that should cause a penalty, it will still call this
        /// function to add to the score, but it should be removed instead</para>
        /// 
        /// <para>There doesn't seem to be a 'nice' way to perform this function
        /// without performing one of the checks twice (triggered or points).
        /// Either we see if the user should be notified of a gain or loss, and
        /// then update the scores in the config file of gains or losses, or the
        /// score is updated of a gain or loss, but a notification check needs
        /// to be done for each option:
        /// <code>
        /// if (!triggered)
        ///     notify if gain or loss
        /// end if
        /// score points for gain or loss
        /// 
        /// --- or ---
        /// if (gain)
        ///     notify user and score points
        /// else
        ///     notify user and lose points
        /// end if
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="Points">The points to add to the current total score</param>
        /// <param name="Description">The description of the issue</param>
        /// <param name="Triggered">If set to <c>true</c> then the user doesn't need to be notified</param>
        public void PointsScored(int Points, string Description, bool Triggered)
        {
            // Seeing if the user should be notified about the points
            // being scored. They should be if the issue has not been
            // triggered before
            if (!Triggered)
            {
                var pointsStatus = new Config.PointsStatus();

                // If the points are less that 0 then points have actually
                // been lost (a penalty), otherwise it's a gain
                if (Points < 0)
                {
                    pointsStatus = Config.PointsStatus.Lost;
                }
                else
                {
                    pointsStatus = Config.PointsStatus.Gained;
                }

                config.pointsStatus = config.pointsStatus | pointsStatus;

                // The '-' is replaced from the points so negative scores
                // look correct
                // The pointsStatus is set to lowercase as it's capitalised
                // in the enum, so looks out of place in the comment
                // e.g. "5 points lost: <description>" vs "-5 points Lost: <description>"
                logger.Info("{0} points have been {1}: {2}",
                            Points.ToString().Replace("-", ""),
                            pointsStatus.ToString().ToLower(),
                            Description);
            }

            // Update the gained or lost points total in the config
            // class, along with the description for it
            if (Points < 0)
            {
                config.UpdatePointsLost(Points, Description);
            }
            else
            {
                config.UpdatePointsGained(Points, Description);
            }
        }

        /// <summary>
        /// Sets variables to inform the user that points have been lost,
        /// from breaking an issue they fixed, or fixing a penalty
        /// 
        /// <para>As with the <see cref="PointsScored"/> function, this
        /// involves some double negatives. If the penalty has been fixed,
        /// then points have been gained, while breaking an issue that was
        /// fixed actually looses points</para>
        /// 
        /// <para>This function does not actually update any values in
        /// the Config class, as by not calling the <see cref="PointsScored"/>
        /// when the issue check takes place, there are no points needing
        /// to be removed</para>
        /// </summary>
        /// <param name="Points">The amount of points that have been lost</param>
        /// <param name="Description">The description of the issue</param>
        public void PointsLost(int Points, string Description)
        {
            var pointsStatus = new Config.PointsStatus();

            // If the points are less that 0 then points have actually
            // been gained (fixed a penalty), otherwise it's a loss
            if (Points < 0)
            {
                pointsStatus = Config.PointsStatus.Gained;
            }
            else
            {
                pointsStatus = Config.PointsStatus.Lost;
            }

            config.pointsStatus = config.pointsStatus | pointsStatus;

            // The '-' is replaced from the points so negative scores
            // look correct
            // The pointsStatus is set to lowercase as it's capitalised
            // in the enum, so looks out of place in the comment
            // e.g. "5 points lost: <description>" vs "-5 points Lost: <description>"
            logger.Info("{0} points have been {1}: {2}",
                        Points.ToString().Replace("-", ""),
                        pointsStatus.ToString().ToLower(),
                        Description);
        }
    }
}
