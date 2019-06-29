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

using CSSSConfig;
using CSSSNotifications;
using NLog;

namespace CSSS
{
    /// <summary>
    /// When a competitor gains or looses points, a desktop notification
    /// should be shown to them to let them know something they have
    /// recently done was one of the issues from the JSON files
    /// 
    /// <para>This class allows the notification API to be universal,
    /// but processes the notification relevant to the Operating System
    /// CSSS is currently running on</para>
    /// </summary>
    public class Notifications
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Creating an instance of the notifications class, so the
        /// relevant notification can be shown
        /// </summary>
        private static NotificationAPI notification = new NotificationAPI();

        /// <summary>
        /// Generates and shows a desktop notification if one
        /// should be displayed
        /// 
        /// <para>There are four possible states that CSSS can be in
        /// when this function is called:
        ///   * Nothing has changed between last issue check run
        ///   * Points have been gained (issue found or penalty removed)
        ///   * Points lost (penalty or a found issue broken)
        ///   * Points have changed (have been gained and lost)
        /// </para>
        /// </summary>
        public void ShowNotification()
        {
            // Points have been changed if the pointsStatus enum
            // doesn't have Gained or Lost flags set
            if ((!config.pointsStatus.HasFlag(Config.PointsStatus.Gained)) &&
                (!config.pointsStatus.HasFlag(Config.PointsStatus.Lost)))
            {
                // Return early as there's nothing left to do in this class
                logger.Debug("No points have been gained or lost, so not showing a notification");
                return;
            }

            // Points have been gained
            if ((config.pointsStatus.HasFlag(Config.PointsStatus.Gained)) &&
                (!config.pointsStatus.HasFlag(Config.PointsStatus.Lost)))
            {
                logger.Debug("Points have been gained on this check run, so showing a 'gained' notification");
                notification.PointsGained();
                return;
            }

            // Points have been lost
            if ((!config.pointsStatus.HasFlag(Config.PointsStatus.Gained)) &&
                (config.pointsStatus.HasFlag(Config.PointsStatus.Lost)))
            {
                logger.Debug("Points have been lost on this check run, so showing a 'lost' notification");
                notification.PointsLost();
                return;
            }

            // Points have been changed
            if ((config.pointsStatus.HasFlag(Config.PointsStatus.Gained)) &&
                (config.pointsStatus.HasFlag(Config.PointsStatus.Lost)))
            {
                logger.Debug("Points have been gained and lost on this check run, so showing a 'changed' notification");
                notification.PointsChanged();
                return;
            }
        }
    }
}
