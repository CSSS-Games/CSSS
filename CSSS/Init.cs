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

using NLog;
using System;
using System.IO;

namespace CSSS
{
    /// <summary>
    /// Performs a number of initialization tasks for CSSS, such as
    /// setting the current OS and the location of various files
    /// </summary>
    public class Init
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSSS.Init"/> class
        /// </summary>
        public Init()
        {
            // Setting the current Operating System
            SetCurrentOperatingSystem();
        }

        /// <summary>
        /// Sets the current operating system that CSSS is running on
        /// in the <see cref="T:CSSS.Config"/> class
        /// 
        /// <para>CSSS can run on a variety of Operating Systems as long
        /// as there is a supported checker class for it. To assist with
        /// using the right checker, we need to find out what Operating
        /// System CSSS is currently running on</para>
        /// 
        /// <para>There doesn't seem to be an easy way to accomplish this
        /// using either .Net or Mono, so a selection of methods found
        /// online are used to perform these checks</para>
        /// 
        /// <para>If the Operating System cannot be identified, then an
        /// 'unknown' value is returned. It is up to the calling function
        /// to throw an error</para>
        /// 
        /// <para>The resources used are:
        ///   * https://blez.wordpress.com/2012/09/17/determine-os-with-netmono/
        ///   * http://softwarerecs.stackexchange.com/a/13722
        ///   * http://stackoverflow.com/q/10138040
        ///   * http://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform
        /// </para>
        /// </summary>
        /// <returns>The current operating system</returns>
        public Config.CurrentOperatingSystem SetCurrentOperatingSystem()
        {
            Config.CurrentOperatingSystem currentOS = Config.CurrentOperatingSystem.Unknown;

            // Seeing if CSSS us running on WinNT (this seems to be the
            // easiest check to carry out
            if (Path.DirectorySeparatorChar == '\\')
            {
                currentOS = Config.CurrentOperatingSystem.WinNT;
                logger.Info("CSSS is running on: {0}", currentOS);
                config.currentOperatingSystem = currentOS;
                return currentOS;
            }

            // We haven't been able to work out what Operating System CSSS
            // is running on, so return unknown. It is up to the calling
            // function to throw an error
            logger.Error("Unable to identify what operating system is in use");
            return currentOS;
        }
    }
}
