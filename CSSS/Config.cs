﻿//  CSSS - CyberSecurity Scoring System
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




        // **********************************************************
        //   Init Checks
        // **********************************************************

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="T:CSSS.Init"/> init
        /// tasks have been completed, used by the <see cref="T:CSSS.Kernel"/> kernel
        /// to see if it should start
        /// 
        /// <para>It is advised to set this value only via the <see cref="T:CSSS.Init"/> init
        /// class to prevent any unexpected functionality</para>
        /// </summary>
        /// <value><c>true</c> if init tasks completed; otherwise, <c>false</c>.</value>
        public bool InitTasksCompleted { get; set; }




        // **********************************************************
        //   Operating System
        // **********************************************************

        /// <summary>
        /// The Operating Systems that CSSS supports to run checks on
        /// </summary>
        [Flags]
        public enum OperatingSystemType
        {
            Unknown,
            Linux,
            WinNT
        }

        /// <summary>
        /// Sets or gets the current Operating System that CSSS is
        /// running on
        /// </summary>
        /// <value>The type of the operating system</value>
        public OperatingSystemType operatingSystemType { get; set; }

        /// <summary>
        /// Gets or sets the name of the operating system
        /// </summary>
        /// <value>The name of the operating system</value>
        public string OperatingSystemName { get; set; }

        /// <summary>
        /// Gets or sets the version of the operating system
        /// </summary>
        /// <value>The name of the operating system</value>
        public string OperatingSystemVersion { get; set; }




        // **********************************************************
        //   Runtime Environment
        // **********************************************************

        /// <summary>
        /// The various runtime environments that CSSS can run on
        /// </summary>
        public enum RuntimeEnvironment
        {
            Unknown,
            DotNet,
            Mono
        }

        /// <summary>
        /// Gets or sets the runtime environment
        /// </summary>
        /// <value>The runtime environment</value>
        public RuntimeEnvironment runtimeEnvironment { get; set; }
    }
}
