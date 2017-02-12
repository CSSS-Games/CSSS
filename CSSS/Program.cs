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

            // Goodbye
            Console.ReadLine();
            return 0;
        }
    }
}
