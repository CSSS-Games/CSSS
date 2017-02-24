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
    /// The main running and scheduling of tasks that CSSS needs to perform,
    /// from linting the check files to calling the checker engine are
    /// co-ordinated by this class
    /// 
    /// <para>This class should be invoked using the PerformTasks function,
    /// which returns a bool of true should all tasks be completed so that
    /// the calling function knows that CSSS can exit. The calling class should
    /// call the PerformTasks inside a loop, so that it can be exited
    /// when this function returns true</para>
    /// 
    /// <para>When this class is initalised it performs some basic sanity
    /// checking (mainly to make sure that the init class has been called)
    /// before attempting to continue</para>
    /// </summary>
    public class Kernel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSSS.Kernel"/> class
        /// if the <see cref="T:CSSS.Init"/> init checks have been completed,
        /// or throws and exception if not
        /// </summary>
        public Kernel()
        {
            if (!config.InitTasksCompleted)
            {
                throw new InvalidOperationException("The CSSS kernel can not run before init tasks have been completed");
            }
        }

        /// <summary>
        /// Performs and co-ordinates the main tasks for CSSS
        /// 
        /// <para>To clarify the return status of this function, it should
        /// be set to true when CSSS should exit, as all of the tasks that
        /// the kernel needs to do have been completed. This will be true
        /// when the CSSSProgramMode is '-c' or '-p', as it will either
        /// lint the check files to make sure everything is in place or
        /// encrypt the check files and set the guardian service to start.
        /// This function will return false if it has been passed the '-o'
        /// or '-s' arguments as checks need to be continually run, so the
        /// calling loop can sleep then recall this function to perform the
        /// next round of tasks</para>
        /// </summary>
        /// <returns><c>true</c>, if tasks have been completed, <c>false</c> otherwise</returns>
        public bool PerformTasks()
        {
            logger.Debug("Performing kernel tasks");
            bool shouldExit = false;

            switch (config.CSSSProgramMode)
            {
                case Config.CSSSModes.Check:
                    shouldExit = true;
                    break;
                    
                case Config.CSSSModes.Observe:
                    shouldExit = false;
                    break;
                    
                case Config.CSSSModes.Prepare:
                    shouldExit = true;
                    break;
                    
                case Config.CSSSModes.Start:
                    shouldExit = false;
                    break;
            }

            logger.Debug("CSSS should exit: {0}", shouldExit);
            return shouldExit;
        }
    }
}
