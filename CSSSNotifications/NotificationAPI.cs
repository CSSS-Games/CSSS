//  CSSS - CyberSecurity Scoring System Notifications
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
using CSSSConfig;
using OS.Linux;
using OS.WinNT;

/// <summary>
/// Notification factory to create the relevant Operating System
/// specific classes for desktop notifications to be displayed
/// </summary>
static class NotificationFactory
{
    private static Config config = Config.GetCurrentConfig;

    public static CSSSNotifications.Notification GetCurrentOperatingSystem()
    {
        switch (config.operatingSystemType)
        {
            case Config.OperatingSystemType.Linux:
                return new NotificationLinux();
            case Config.OperatingSystemType.WinNT:
                return new NotificationWinNT();
            default:
                throw new NotSupportedException("This Operating System is not supported for notifications to be displayed");
        }
    }
}

namespace CSSSNotifications
{
    /// <summary>
    /// Provides an API to display desktop notifications on supported
    /// Operating Systems
    /// </summary>
    public class NotificationAPI
    {
        /// <summary>
        /// Displays a positive notification to the competitor to
        /// let them know points have been gained
        /// </summary>
        public void PointsGained()
        {
            NotificationFactory.GetCurrentOperatingSystem().PointsGained();
        }

        /// <summary>
        /// Displays a nagative notification to the competitor to
        /// let them know points have been lost
        /// </summary>
        public void PointsLost()
        {
            NotificationFactory.GetCurrentOperatingSystem().PointsLost();
        }

        /// <summary>
        /// Displays a neutral notification to the competitor to
        /// let them know points have been gained and lost
        /// </summary>
        public void PointsChanged()
        {
            NotificationFactory.GetCurrentOperatingSystem().PointsChanged();
        }
    }
}
