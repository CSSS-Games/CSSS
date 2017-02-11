//  CSSS - Cyber Security Scoring System
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
//  along with this program. If not, see<http://www.gnu.org/licenses/>.

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

        public Bootstrap()
        {
        }

        /// <summary>
        /// Checks the arguments passed to the program, and calls
        /// relevant functions to see if anything needs to be processed,
        /// or if the main part of the program can begin
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
                logger.Debug("No arguments have been passed to CSSS, so showing program help messages");
                ShowHelp();

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
                    case "h":
                    case "help":
                    default:
                        ShowHelp();
                        break;
                }
            }

            return canStart;
        }

        /// <summary>
        /// Shows the help usage documentation on using CSSS
        /// </summary>
        private void ShowHelp()
        {
            Console.WriteLine("Cyber Security Scoring System (CSSS) Usage");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            // If there are no arguments passed, or the help argument is passed
            Console.Write("-h, --help:".PadRight(16));
            Console.WriteLine("Shows this help message");
        }
    }
}
