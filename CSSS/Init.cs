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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using SupportLibrary.OSVersionInfo;

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
            // Setting the current Operating System type
            SetOperatingSystemType();

            // Setting the name of the Operating System, or
            // throw an error if not possible to be found
            if (!SetOperatingSystemName())
            {
                throw new NotImplementedException("CSSS was not able to identify the name of the Operating System");
            }

            // Setting the version of the Operating System, or
            // throw an error if not possible to be found
            if (!SetOperatingSystemVersion())
            {
                throw new NotImplementedException("CSSS was not able to identify the version of the Operating System");
            }

            // Setting the runtime environment
            SetRuntimeEnvironment();
        }

        /// <summary>
        /// Sets the current operating system type that CSSS is running on
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
        /// to catch the exception thrown</para>
        /// 
        /// <para>The resources used are:
        ///   * https://blez.wordpress.com/2012/09/17/determine-os-with-netmono/
        ///   * http://softwarerecs.stackexchange.com/a/13722
        ///   * http://stackoverflow.com/q/10138040
        ///   * http://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform
        /// </para>
        /// </summary>
        /// <returns>The current operating system</returns>
        public Config.OperatingSystemType SetOperatingSystemType()
        {
            Config.OperatingSystemType currentOS = Config.OperatingSystemType.Unknown;

            // Seeing if CSSS us running on WinNT (this seems to be the
            // easiest check to carry out
            if (Path.DirectorySeparatorChar == '\\')
            {
                currentOS = Config.OperatingSystemType.WinNT;
                logger.Info("Operating System type: {0}", currentOS);
                config.operatingSystemType = currentOS;
                return currentOS;
            }

            // Attempting to identify the Unix-based OS based on the returned
            // string from the `uname -s` command
            // While currently only Linux is also supported by CSSS, a switch
            // function is used here should additional OS support be added in
            // the future
            switch (ReadProcessOutput("uname", "-s").ToLower())
            {
                case "linux":
                    currentOS = Config.OperatingSystemType.Linux;
                    break;
                default:
                    break;
            }

            if (currentOS != Config.OperatingSystemType.Unknown)
            {
                logger.Info("Operating System type: {0}", currentOS);
                config.operatingSystemType = currentOS;
                return currentOS;
            }

            // We haven't been able to work out what Operating System CSSS
            // is running on, so return unknown. It is up to the calling
            // function to throw an error
            logger.Error("Unable to identify what Operating System is in use");
            throw new NotImplementedException("CSSS does not support running on your Operating System");
        }

        /// <summary>
        /// Sets the name of the operating system that CSSS is running
        /// on, such as Windown Seven or Ubuntu Trusty
        /// 
        /// <para>This function is used to let CSSS know what operating
        /// system version it is running on, so that should any specific
        /// changes need to be made when checks are being carried out
        /// they can be</para>
        /// 
        /// <para>This function will always return false if an operating
        /// system is unknown once the `SetOperatingSystemType` function
        /// has been called. If the operating system name can not be
        /// detected then an error is thrown</para>
        /// </summary>
        /// <returns><c>true</c>, if operating system name was set, <c>false</c> otherwise.</returns>
        public bool SetOperatingSystemName()
        {
            switch (config.operatingSystemType)
            {
                case Config.OperatingSystemType.WinNT:
                    // Using the OSVersionInfo class from the Support Library
                    config.OperatingSystemName = WinNTVersionInfo.Name;
                    logger.Info("Operating System name: {0}", config.OperatingSystemName);
                    return true;
                    
                case Config.OperatingSystemType.Linux:
                    // Using the `lsb_release -i -s` and `lsb_release -c -s`
                    // program and arguments to join the Operating System
                    // distributor and codename
                    config.OperatingSystemName = ReadProcessOutput("lsb_release", "-i -s")
                                               + " "
                                               + ReadProcessOutput("lsb_release", "-c -s");
                    logger.Info("Operating System name: {0}", config.OperatingSystemName);
                    return true;
                    
                default:
                    // The operating system type is not known, so it is not
                    // possible to set a name
                    return false;
            }
        }

        /// <summary>
        /// Sets the version of the operating system that CSSS is running
        /// on, such as 6.1.7600.0 (Windows) or 14.04.5 (Ubuntu)
        /// 
        /// <para>This function is used to let CSSS know what operating
        /// system version it is running on, so that should any specific
        /// changes need to be made when checks are being carried out
        /// they can be</para>
        /// 
        /// <para>This function will always return false if an operating
        /// system is unknown once the `SetOperatingSystemType` function
        /// has been called. If the operating system name can not be
        /// detected then an error is thrown</para>
        /// </summary>
        /// <returns><c>true</c>, if operating system version was set, <c>false</c> otherwise.</returns>
        public bool SetOperatingSystemVersion()
        {
            switch (config.operatingSystemType)
            {
                case Config.OperatingSystemType.WinNT:
                    // Using the OSVersionInfo class from the Support Library
                    config.OperatingSystemVersion = WinNTVersionInfo.VersionString;
                    logger.Info("Operating System ver.: {0}", config.OperatingSystemVersion);
                    return true;

                case Config.OperatingSystemType.Linux:
                    // Using the `lsb_release -d -s` to get the Operating
                    // System description string, then removing any characters
                    // apart from numbers and dots
                    string operatingSystemDescription = ReadProcessOutput("lsb_release", "-d -s");
                    config.OperatingSystemVersion = Regex.Replace(operatingSystemDescription,
                                                                  @"[^0-9\.]+",
                                                                  "");
                    logger.Info("Operating System ver.: {0}", config.OperatingSystemVersion);
                    return true;
                    
                default:
                    // The operating system type is not known, so it is not
                    // possible to set a version number
                    return false;
            }
        }

        /// <summary>
        /// Sets the runtime environment that CSSS is running on
        /// </summary>
        /// <returns>The runtime environment</returns>
        public Config.RuntimeEnvironment SetRuntimeEnvironment()
        {
            // This is taken almost directly from the Mono FAQs
            // See: http://www.mono-project.com/docs/faq/technical/#how-can-i-detect-if-am-running-in-mono
            config.runtimeEnvironment = Config.RuntimeEnvironment.Unknown;

            Type t = Type.GetType("Mono.Runtime");
            if (t != null)
            {
                config.runtimeEnvironment = Config.RuntimeEnvironment.Mono;
            }
            else
            {
                config.runtimeEnvironment = Config.RuntimeEnvironment.DotNet;
            }

            logger.Info("Runtime environment: {0}", config.runtimeEnvironment);
            return config.runtimeEnvironment;
        }

        /// <summary>
        /// Reads system program process output to determine information about
        /// the current Operating System type
        /// 
        /// <para>For Unix-like Operating Systems various commands can be used
        /// to dertermine some information about the current computer, such as
        /// the kernel type and version. This function serves as a wrapper to
        /// those programs to return any data to CSSS</para>
        /// 
        /// <para>See: https://blez.wordpress.com/2012/09/17/determine-os-with-netmono/</para>
        /// </summary>
        /// <returns>Any string returned from the program output</returns>
        /// <param name="name">The name of the program to get system information</param>
        /// <param name="args">Any arguments to pass to the program</param>
        private static string ReadProcessOutput(string name, string args)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                if (args != null && args != "") p.StartInfo.Arguments = " " + args;
                p.StartInfo.FileName = name;
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (output == null) output = "";
                output = output.Trim();
                return output;
            }
            catch
            {
                return "";
            }
        }
    }
}
