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

namespace CSSS
{
    /// <summary>
    /// The main config class for the whole CSSS project
    /// 
    /// <para>This is implemented as a singleton class, so that
    /// settings that are put in place by the Init class are
    /// preserved to be accessed by any other class. The fourth
    /// version from csharpindepth.com/Articles/General/Singleton.aspx
    /// has been chosen as the model</para>
    /// 
    /// <para>All public functions are created as getters and setters
    /// so that a correcponding private member does not need to be
    /// created to store the values</para>
    /// </summary>
    public sealed class Config
    {
        private static readonly Config configInstance = new Config();

        static Config()
        {
        }

        private Config()
        {
        }

        /// <summary>
        /// Gets the get current config
        /// </summary>
        /// <value>The current config instance for CSSS</value>
        public static Config GetCurrentConfig
        {
            get
            {
                return configInstance;
            }
        }

        /// <summary>
        /// The Operating Systems that CSSS supports to run checks on
        /// </summary>
        [Flags]
        public enum CurrentOperatingSystem
        {
            Unknown,
            Linux,
            WinNT
        }

        /// <summary>
        /// Sets or gets the current Operating System that CSSS is
        /// running on
        /// </summary>
        public CurrentOperatingSystem currentOperatingSystem { get; set; }
    }
}
