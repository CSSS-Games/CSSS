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
    /// Checks for any arguments passed to the program, and performs
    /// any relevant processing before starting the main part of the
    /// program
    /// </summary>
    public class Bootstrap
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The values that can be returned from this class, once the
        /// argument checks have been carried out
        /// </summary>
        [Flags]
        public enum BootstrapOptions
        {
            Help = 0x0,
            Check = 0x1,
            Prepare = 0x2,
            Observe = 0x4,
            Start = 0x8
        }

        /// <summary>
        /// The result of the argument checks, used to decide the next
        /// step CSSS should take
        /// </summary>
        public BootstrapOptions bootstrapResult;

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
        /// Valid arguments that CSSS accepts are:
        ///   * -c, --check:   Checks the config files for any problems
        ///   * -o, --observe: Observes CSSS running before preparing it (implies 'c')
        ///   * -p, --prepare: Prepares CSSS ready for image release (implies '-c')
        ///   * -s, --start:   Starts the scoring system
        ///   * -h, --help:    Shows the program usage
        /// 
        /// These arguments have been chosen to spell 'COPS', as competitors
        /// are 'policing' the security of the computer
        /// 
        /// Under normal debugging, the 'o' option should be passed, as
        /// this allows CSSS to run as it would for training, but without
        /// affecting the files used to list the 'issues'
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
                    case "c":
                    case "check":
                        bootstrapResult = BootstrapOptions.Check;
                        canStart = true;
                        break;

                    case "o":
                    case "observe":
                        bootstrapResult = BootstrapOptions.Check | BootstrapOptions.Observe;
                        canStart = true;
                        break;

                    case "p":
                    case "prepare":
                        bootstrapResult = BootstrapOptions.Check | BootstrapOptions.Prepare;
                        canStart = true;
                        break;

                    case "s":
                    case "start":
                        bootstrapResult = BootstrapOptions.Start;
                        canStart = true;
                        break;

                    case "h":
                    case "help":
                    default:
                        bootstrapResult = BootstrapOptions.Help;
                        ShowUsage();
                        break;
                }
            }

            logger.Debug("Actions to perform: {0}", bootstrapResult);
            logger.Debug("CSSS can continue: {0}", canStart);

            return canStart;
        }

        /// <summary>
        /// Shows the usage documentation on using CSSS
        /// </summary>
        private void ShowUsage()
        {
            int rightPadding = 18;

            Console.WriteLine("Cyber Security Scoring System (CSSS) Usage");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            // Valid parameter combinations to pass
            Console.WriteLine("Usage:");
            Console.WriteLine("  CSSS.exe -c | -o | -p | -s | -h");
            Console.WriteLine();
            Console.WriteLine("Options:");

            // Check config files
            Console.Write("  -c, --check:".PadRight(rightPadding));
            Console.WriteLine("Checks the config files for any problems");

            // Observe running
            Console.Write("  -o, --observe:".PadRight(rightPadding));
            Console.WriteLine("Observes CSSS running before preparing it (implies '-c')");

            // Prepare CSSS
            Console.Write("  -p, --prepare:".PadRight(rightPadding));
            Console.WriteLine("Prepares CSSS ready for image release (implies '-c')");

            // Start scoring system
            Console.Write("  -s, --start:".PadRight(rightPadding));
            Console.WriteLine("Starts the scoring system");

            // Help
            Console.Write("  -h, --help:".PadRight(rightPadding));
            Console.WriteLine("Shows this help message");
        }
    }
}
