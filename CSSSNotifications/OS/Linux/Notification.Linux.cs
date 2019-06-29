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

using System.Diagnostics;
using System.IO;
using System.Reflection;
using CSSSNotifications;

namespace OS.Linux
{
    internal class NotificationLinux : Notification
    {
        /// <summary>
        /// Displays a positive notification to the competitor to
        /// let them know points have been gained
        /// </summary>
        public override void PointsGained()
        {
            ShowNotification(notificationMessageTextPointsGained, "dialog-information");
        }

        /// <summary>
        /// Displays a nagative notification to the competitor to
        /// let them know points have been lost
        /// </summary>
        public override void PointsLost()
        {
            ShowNotification(notificationMessageTextPointsLost, "dialog-error");
        }

        /// <summary>
        /// Displays a neutral notification to the competitor to
        /// let them know points have been gained and lost
        /// </summary>
        public override void PointsChanged()
        {
            ShowNotification(notificationMessageTextPointsChanged, "dialog-warning");
        }

        /// <summary>
        /// Shows the notification
        /// 
        /// For Linux based Operating Systems, the "notify-send" program
        /// (part of the libnotify-bin package) can be used to show desktop
        /// notifications of any changes in points
        /// 
        /// However, as it's not possible to just call "notify-send" from
        /// a background program, we need to run a script that can perform
        /// a clever selection of calling programs and getting the right
        /// information to display the notifications
        /// 
        /// The script expects the parameters to be sent in this order:
        ///   NotifyAll.Linux.sh "[title]" "[message]" [icon]
        /// 
        /// See: https://wiki.archlinux.org/index.php/Desktop_notifications
        /// See: http://unix.stackexchange.com/questions/2881/show-a-notification-across-all-running-x-displays
        /// </summary>
        /// <param name="message">The message to show to the competitor</param>
        /// <param name="icon">The icon to show, see: https://specifications.freedesktop.org/icon-naming-spec/icon-naming-spec-latest.html</param>
        /// <param name="title">The text to display in the notification title</param>
        private void ShowNotification(string message, string icon, string title = notificationTitleText)
        {
            string CSSSDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + Path.DirectorySeparatorChar;
            var CSSSDirectoryLinux = "OS" + Path.DirectorySeparatorChar + "Linux" + Path.DirectorySeparatorChar;

            var notificationProgram = "/bin/bash";
            var notificationParameters = CSSSDirectoryLinux + "NotifyAll.Linux.sh \"" + title + "\" \"" + message + "\" " + icon;

            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.Arguments = " " + notificationParameters;
                p.StartInfo.FileName = notificationProgram;
                p.Start();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                logger.Warn("Unable to show desktop notification. Is the 'libnotify-bin' package installed?");
            }
        }
    }
}
