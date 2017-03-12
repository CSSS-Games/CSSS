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

using CSSSNotifications;
using System;
using System.Drawing;

namespace OS.WinNT
{
    internal class NotificationWinNT : Notification
    {
        /// <summary>
        /// Displays a positive notification to the competitor to
        /// let them know points have been gained
        /// </summary>
        public override void PointsGained()
        {
            ShowNotification("Points Gained", SystemIcons.Information);
        }

        /// <summary>
        /// Displays a nagative notification to the competitor to
        /// let them know points have been lost
        /// </summary>
        public override void PointsLost()
        {
            ShowNotification("Points Lost", SystemIcons.Error);
        }

        /// <summary>
        /// Displays a neutral notification to the competitor to
        /// let them know points have been gained and lost
        /// </summary>
        public override void PointsChanged()
        {
            ShowNotification("Points Changed", SystemIcons.Warning);
        }

        /// <summary>
        /// Shows the notification
        /// 
        /// See: http://stackoverflow.com/a/34956412
        /// </summary>
        /// <param name="TitleText">The text to display in the notification title</param>
        /// <param name="NotificationIcon">The icon to show, see: https://msdn.microsoft.com/en-us/library/system.drawing.systemicons.aspx</param>
        private void ShowNotification(string TitleText, Icon NotificationIcon)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = NotificationIcon,
                BalloonTipText = TitleText
            };

            // Display for 5 seconds
            notification.ShowBalloonTip(5000);

            // The notification should be disposed when you don't need it anymore
            notification.Dispose();
        }
    }
}
