//  CSSS - CyberSecurity Scoring System Notifications
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

using CSSSConfig;
using NLog;
using System;

namespace CSSSNotifications
{
    /// <summary>
    /// This is the abstract class for use with displaying notifications
    /// on the various Operating Systems that CSSS runs on
    /// </summary>
    abstract class Notification
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        protected static Config config = Config.GetCurrentConfig;

        // The notification message and title text to display
        protected const string NotificationTitleText = "CyberSecurity Scoring System";
        protected const string NotificationMessageTextPointsGained = "Points have been gained";
        protected const string NotificationMessageTextPointsChanged = "Points have been changed";
        protected const string NotificationMessageTextPointsLost = "Points have been lost";

        /// <summary>
        /// Displays a positive notification to the competitor to
        /// let them know points have been gained
        /// </summary>
        public abstract void PointsGained();

        /// <summary>
        /// Displays a nagative notification to the competitor to
        /// let them know points have been lost
        /// </summary>
        public abstract void PointsLost();

        /// <summary>
        /// Displays a neutral notification to the competitor to
        /// let them know points have been gained and lost
        /// </summary>
        public abstract void PointsChanged();
    }
}
