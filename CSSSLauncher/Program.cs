//  CSSS - CyberSecurity Scoring System Launcher
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

namespace CSSSLauncher
{
    /// <summary>
    /// CSSSLauncher is a stub application that is intended to
    /// launch the main CSSS executible in the "background"
    /// 
    /// <para>The main CSSS console should be hidden from view when
    /// it will be run in "start" mode, to prevent competitors being
    /// able to see what is happening</para>
    /// 
    /// <para>As console applications cannot normally run hidden from
    /// view (http://stackoverflow.com/q/836427), this launcher calls
    /// the CSSS executible then exits. This has been chosen rather
    /// than the alternate option of converting CSSS to be a WinForms
    /// project, as support on Mono may not be supported</para>
    /// 
    /// <para>This launcher should be called on computer startup from
    /// the 'Startup' start menu folder on WinNT or '/etc/rc.local' on
    /// Linux, but can be called manually if needed</para>
    /// 
    /// <para>A service is not used for two main reasons:
    ///   * WinNT services aren't really supported on other Operating Systems
    ///   * Attempting to display desktop score notifications on Linux
    ///     becomes incedibly complex, requiring sending commands to the
    ///     X server: https://wiki.archlinux.org/index.php/Desktop_notifications#Usage_in_programming
    /// </para>
    /// </summary>
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Hiding the main CSSS console from displaying
            // See: http://stackoverflow.com/a/836436
            // See: http://stackoverflow.com/a/848343
            // See: http://stackoverflow.com/a/29535211
            var CSSS = new Process();
            var CSSSProcessInfo = new ProcessStartInfo
            {
                FileName = "CSSS.exe",
                Arguments = "--start",

                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            CSSS.StartInfo = CSSSProcessInfo;

            // Let's get things underway...
            CSSS.Start();
        }
    }
}
