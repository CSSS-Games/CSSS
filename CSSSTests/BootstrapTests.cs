//  CSSSTests - CyberSecurity Scoring System Tests
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

using CSSS;
using NUnit.Framework;
using System;

namespace CSSSTests
{
    /// <summary>
    /// Runs all tests related to the bootstrap class
    /// </summary>
    [TestFixture()]
    public class BootstrapTests
    {
        private Bootstrap bootstrapChecks;

        /// <summary>
        /// Creates an instance of the bootstrap class
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            bootstrapChecks = new Bootstrap();
        }

        /// <summary>
        /// Removes any reference to the bootstrap class
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            bootstrapChecks = null;
        }

        /// <summary>
        /// When there is no parameter passed, the program should not
        /// continue execution and the help enum should be set
        /// </summary>
        [Test()]
        public void TestEmptyArgumentsToProgram()
        {
            string[] emptyArguments = new string[0];
            bool canStart = bootstrapChecks.CheckArguments(emptyArguments);
            var CSSSMode = bootstrapChecks.bootstrapResult;

            Assert.Multiple(() =>
            {
                Assert.IsFalse(canStart,
                               "Empty arguments to CSSS should not continue execution"
                              );
                Assert.AreEqual(Bootstrap.BootstrapOptions.Help,
                                CSSSMode,
                                "Mode for CSSS should be 'help' if no arguments are passed"
                               );
            });
        }
    }
}
