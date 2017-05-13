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
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program. If not, see <http://www.gnu.org/licenses/>.

using NLog;
using System;
using System.Threading;

namespace CSSS
{
    class MainClass
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static int Main(string[] args)
        {
            // Performing bootstrap checks to see if the program should load
            // normally, or perform a specific task
            var bootstrapChecks = new Bootstrap();
            bool canStart = bootstrapChecks.CheckArguments(args);

            if (!canStart)
            {
                // We can't go any further, so we can return early
                return 1;
            }

            // Setting up any environment options, such as the OS
            // CSSS is running on
            // Should any errors occur, an exception is thrown and
            // program execution will stop
            try
            {
                var init = new Init();
            }
            catch (NotImplementedException e)
            {
                int exitCode = 10;
                logger.Fatal("An error occurred trying to start CSSS: {0}", e.Message);
                logger.Fatal(GenerateExitCodeURLMessage(exitCode));
                return exitCode;
            }
            catch (System.Net.Sockets.SocketException)
            {
                int exitCode = 11;
                logger.Error("An instance of CSSS is probably already running... exiting");
                logger.Error(GenerateExitCodeURLMessage(exitCode));
                return exitCode;
            }
            catch (System.Security.SecurityException e)
            {
                int exitCode = 12;

                // CSSS is in "prepare" mode, but has not been run with administrative privileges
                logger.Error(e.Message);
                logger.Error(GenerateExitCodeURLMessage(exitCode));
                return exitCode;
            }

            // Creating an instance of the CSSS kernel so that tasks
            // can be co-ordinated. The constructor will throw an
            // error if the init class tasks have not been performed
            try
            {
                var kernel = new Kernel();

                // Starting the main run loop to keep the kernel performing
                // tasks, or if the response from the function is true,
                // break out of the loop. After performing the kernel tasks,
                // the thread sleeps for a minute to prevent the checks
                // being run constantly
                bool shouldExit = false;
                logger.Debug("Starting main run loop");
                while (!shouldExit)
                {
                    shouldExit = kernel.PerformTasks();

                    // This loop should sleep for a minute if the kernel
                    // tasks have not been finished
                    if (!shouldExit)
                    {
                        logger.Debug("Sleeping main run loop before re-running kernel tasks");
                        Thread.Sleep(60000);
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                int exitCode = 20;
                logger.Fatal("An error occurred trying to run CSSS: {0}", e.Message);
                logger.Fatal(GenerateExitCodeURLMessage(exitCode));
                return exitCode;
            }

            // Goodbye
            logger.Info("CSSS has finished performing any needed tasks");
            //Console.ReadLine();
            return 0;
        }

        /// <summary>
        /// Generates a URL message for the CSSS wiki for the exit code
        /// 
        /// Should CSSS need to return an exit code, this function
        /// will generate a URL message to direct users to the revevant
        /// wiki article and section
        /// 
        /// This is created as a function and not directly coded
        /// into the Main function so the URL can easilly be changed
        /// if needed in the future
        /// 
        /// The URLs are:
        ///   Full: https://github.com/stuajnht/CSSS/wiki/CSSS-Exit-Codes
        ///   Short: https://git.io/v9QbU
        /// 
        /// Note: If any additional exit codes are created, the
        ///       wiki pages will also need to be updated to reflect
        ///       the changes
        /// </summary>
        /// <returns>The exit code wiki URL message</returns>
        /// <param name="exitCode">The CSSS exit code</param>
        private static string GenerateExitCodeURLMessage(int exitCode)
        {
            return "See \"https://github.com/stuajnht/CSSS/wiki/CSSS-Exit-Codes#exit-code-" +
                   exitCode +
                   "\" for more information";
        }
    }
}
