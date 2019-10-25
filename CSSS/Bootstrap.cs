//  CSSS - CyberSecurity Scoring System
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

using System;
using CSSSConfig;
using NLog;

namespace CSSS
{
    /// <summary>
    /// Checks for any arguments passed to the program, and performs
    /// any relevant processing before starting the main part of the
    /// program
    /// </summary>
    public class Bootstrap
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// If the usage screen has been shown, the user does not
        /// need to see it again if a required argument hasn't been
        /// passed to CSSS
        /// </summary>
        private bool usageScreenShown = false;

        public Bootstrap()
        {
        }

        /// <summary>
        /// Checks the arguments passed to the program, and calls
        /// relevant functions to see if anything needs to be processed,
        /// or if the main part of the program can begin
        /// 
        /// Performing a check for any arguments being passed is a way
        /// to prevent any 'shooting yourself in the foot' problems by
        /// the user accidentally double-clicking the main executible,
        /// as the config files that are used are encrypted before the
        /// image is released, to prevent any cheating by competitors
        /// 
        /// Valid required arguments that CSSS accepts are:
        ///   * -c, --check:   Checks the config files for any problems
        ///   * -o, --observe: Observes CSSS running before preparing it (implies 'c')
        ///   * -p, --prepare: Prepares CSSS ready for image release (implies '-c')
        ///   * -s, --start:   Starts the scoring system
        ///   * -h, --help:    Shows the program usage
        /// 
        /// Additional arguments that can be passed to CSSS are:
        ///   * -m, --multiple: Allows multiple instances of CSSS to run concurently
        /// 
        /// These arguments have been chosen to spell 'COPS', as competitors
        /// are 'policing' the security of the computer
        /// 
        /// Under normal debugging, the 'o' option should be passed, as
        /// this allows CSSS to run as it would for training, but without
        /// affecting the files used to list the 'issues'. The 'm' option
        /// can also be used should an instance of CSSS be running in the
        /// background
        /// </summary>
        /// <returns><c>true</c>, if the main part of the program can start, <c>false</c> otherwise</returns>
        /// <param name="arguments">The arguments passed to the CSSS Main function</param>
        public bool CheckArguments(string[] arguments)
        {
            bool canStart = false;

            // If no arguments are passed, then show the help messages
            // and stop program execution
            if (arguments.Length == 0)
            {
                logger.Debug("No arguments passed to CSSS - showing program usage");
                ShowUsage();

                // No further checks are needed here, so we can return early
                return canStart;
            }

            // Seeing what has been passed and acting on it
            int argumentCount = 0;
            foreach (string argument in arguments)
            {
                argumentCount += 1;
                logger.Debug("Argument '{0}' passed to CSSS: {1}", argumentCount, argument);

                switch (argument.ToLower().Replace("-", ""))
                {
                    // ******************
                    // Required arguments
                    // ******************
                    case "c":
                    case "check":
                        config.CSSSProgramMode |= Config.CSSSModes.Check;
                        canStart = true;
                        break;

                    case "o":
                    case "observe":
                        config.CSSSProgramMode |= Config.CSSSModes.Check | Config.CSSSModes.Observe;
                        canStart = true;
                        break;

                    case "p":
                    case "prepare":
                        config.CSSSProgramMode |= Config.CSSSModes.Check | Config.CSSSModes.Prepare;
                        canStart = true;
                        break;

                    case "s":
                    case "start":
                        config.CSSSProgramMode |= Config.CSSSModes.Start;
                        canStart = true;
                        break;


                    // *******************
                    // Developer arguments
                    // *******************
                    case "m":
                    case "multiple":
                        config.CSSSProgramMode |= Config.CSSSModes.MultipleInstances;
                        break;


                    // *******************
                    // Optional arguments
                    // *******************
                    case "shutdown":
                        config.CSSSProgramMode |= Config.CSSSModes.Shutdown;
                        canStart = true;
                        break;

                    case "h":
                    case "help":
                    default:
                        config.CSSSProgramMode = Config.CSSSModes.Help;
                        ShowUsage();
                        break;
                }
            }

            logger.Debug("Actions to perform: {0}", config.CSSSProgramMode);
            logger.Debug("CSSS can continue: {0}", canStart);

            // Showing the usage screen if no required arguments have been passed
            if (!canStart)
            {
                logger.Warn("Required arguments have not been passed to CSSS... showing usage screen");
                ShowUsage();
            }

            return canStart;
        }

        /// <summary>
        /// Shows the usage documentation on using CSSS
        /// </summary>
        private void ShowUsage()
        {
            const int RIGHT_PADDING = 18;

            // Preventing the usage screen being shown multiple times if
            // the "-h" argument is passed more than once to CSSS, or a
            // required argument is missing and "-h" had been passed too
            if (usageScreenShown)
            {
                return;
            }
            usageScreenShown = true;

            Console.WriteLine("CyberSecurity Scoring System (CSSS) Usage");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            // Argument combinations to pass
            Console.WriteLine("Usage:");
            Console.WriteLine("  CSSS.exe -c | -o | -p [--shutdown] | -s | [-h] | [-m]");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  CSSS.exe -c");
            Console.WriteLine("  CSSS.exe -o -m");
            Console.WriteLine("  CSSS.exe -p");
            Console.WriteLine("  CSSS.exe -p --shutdown");
            Console.WriteLine("  CSSS.exe -s");
            Console.WriteLine("  CSSS.exe -h");


            // ******************
            // Required arguments
            // ******************
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Required arguments (at least one is needed):");

            // Check config files
            Console.Write("  -c, --check:".PadRight(RIGHT_PADDING));
            Console.WriteLine("Checks the config files for any problems");

            // Observe running
            Console.Write("  -o, --observe:".PadRight(RIGHT_PADDING));
            Console.WriteLine("Observes CSSS running before preparing it (implies '-c')");

            // Prepare CSSS
            Console.Write("  -p, --prepare:".PadRight(RIGHT_PADDING));
            Console.WriteLine("Prepares CSSS ready for image release (implies '-c')");

            // Start scoring system
            Console.Write("  -s, --start:".PadRight(RIGHT_PADDING));
            Console.WriteLine("Starts the scoring system");


            // ******************
            // Optional arguments
            // ******************
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Optional arguments:");

            // Help
            Console.Write("  -h, --help:".PadRight(RIGHT_PADDING));
            Console.WriteLine("Shows this help message");

            // Shutdown the computer
            Console.Write("  --shutdown".PadRight(RIGHT_PADDING));
            Console.WriteLine("Shuts down the computer");
            Console.Write("".PadRight(RIGHT_PADDING));
            Console.WriteLine("Can be used with -p / --prepare to aid image capture");

            // *******************
            // Developer arguments
            // *******************
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Developer arguments (all optional):");

            // Multipile instances allowed to concurrently run
            Console.Write("  -m, --multiple:".PadRight(RIGHT_PADDING));
            Console.WriteLine("Allows multiple instances of CSSS to run concurently");
        }
    }
}
