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
                logger.Fatal("An error occurred trying to start CSSS: {0}", e.Message);
                return 10;
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
                logger.Fatal("An error occurred trying to run CSSS: {0}", e.Message);
                return 20;
            }

            // Goodbye
            logger.Info("CSSS has finished performing any needed tasks");
            //Console.ReadLine();
            return 0;
        }
    }
}
