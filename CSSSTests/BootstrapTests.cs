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
using CSSSConfig;
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

        private static Config config;

        /// <summary>
        /// Creates an instance of the bootstrap class
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            bootstrapChecks = new Bootstrap();
            config = Config.GetCurrentConfig;
        }

        /// <summary>
        /// Removes any reference to the bootstrap class
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            bootstrapChecks = null;
            config = null;
        }

        /// <summary>
        /// When there is no parameter passed, the program should not
        /// continue execution and the Help enum should be set
        /// </summary>
        [Test()]
        public void TestEmptyArgumentsToProgram()
        {
            string[] emptyArguments = new string[0];

            Assert.IsFalse(bootstrapChecks.CheckArguments(emptyArguments),
                           "Empty arguments to CSSS should not continue execution"
                          );
        }

        /// <summary>
        /// When the '-h' argument is passed, the program should not
        /// continue execution and the Help enum should be set
        /// </summary>
        [Test()]
        public void TestHelpShortArgumentToProgram()
        {
            string[] helpShortArgument = new string[1] { "-h" };

            Assert.Multiple(() =>
            {
                Assert.IsFalse(bootstrapChecks.CheckArguments(helpShortArgument),
                               "'-h' argument passed to CSSS should not continue execution"
                              );
                Assert.AreEqual(Config.CSSSModes.Help,
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Help' if '-h' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '--help' argument is passed, the program should not
        /// continue execution and the Help enum should be set
        /// </summary>
        [Test()]
        public void TestHelpLongArgumentToProgram()
        {
            string[] helpLongArgument = new string[1] { "--help" };

            Assert.Multiple(() =>
            {
                Assert.IsFalse(bootstrapChecks.CheckArguments(helpLongArgument),
                               "'--help' argument passed to CSSS should not continue execution"
                              );
                Assert.AreEqual(Config.CSSSModes.Help,
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Help' if '--help' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '-c' argument is passed, the program should
        /// continue execution and the Check enum should be set
        /// </summary>
        [Test()]
        public void TestCheckShortArgumentToProgram()
        {
            string[] checkShortArgument = new string[1] { "-c" };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(bootstrapChecks.CheckArguments(checkShortArgument),
                               "'-c' argument passed to CSSS should continue execution"
                              );
                Assert.AreEqual(Config.CSSSModes.Check,
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Check' if '-c' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '--check' argument is passed, the program should
        /// continue execution and the Check enum should be set
        /// </summary>
        [Test()]
        public void TestCheckLongArgumentToProgram()
        {
            string[] checkLongArgument = new string[1] { "--check" };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(bootstrapChecks.CheckArguments(checkLongArgument),
                               "'--check' argument passed to CSSS should continue execution"
                              );
                Assert.AreEqual(Config.CSSSModes.Check,
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Check' if '--check' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '-o' argument is passed, the program should
        /// continue execution and the Check and Observe enums should be set
        /// </summary>
        [Test()]
        public void TestObserveShortArgumentToProgram()
        {
            string[] observeShortArgument = new string[1] { "-o" };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(bootstrapChecks.CheckArguments(observeShortArgument),
                               "'-o' argument passed to CSSS should continue execution"
                              );
                Assert.AreEqual((Config.CSSSModes.Check | Config.CSSSModes.Observe),
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Check, Observe' if '-o' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '--observe' argument is passed, the program should
        /// continue execution and the Check and Observe enums should be set
        /// </summary>
        [Test()]
        public void TestObserveLongArgumentToProgram()
        {
            string[] observeLongArgument = new string[1] { "--observe" };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(bootstrapChecks.CheckArguments(observeLongArgument),
                               "'--observe' argument passed to CSSS should continue execution"
                              );
                Assert.AreEqual((Config.CSSSModes.Check | Config.CSSSModes.Observe),
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Check, Observe' if '--observe' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '-p' argument is passed, the program should
        /// continue execution and the Check and Prepare enums should be set
        /// </summary>
        [Test()]
        public void TestPrepareShortArgumentToProgram()
        {
            string[] prepareShortArgument = new string[1] { "-p" };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(bootstrapChecks.CheckArguments(prepareShortArgument),
                               "'-p' argument passed to CSSS should continue execution"
                              );
                Assert.AreEqual((Config.CSSSModes.Check | Config.CSSSModes.Prepare),
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Check, Prepare' if '-p' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '--prepare' argument is passed, the program should
        /// continue execution and the Check and Prepare enums should be set
        /// </summary>
        [Test()]
        public void TestPrepareLongArgumentToProgram()
        {
            string[] prepareLongArgument = new string[1] { "--prepare" };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(bootstrapChecks.CheckArguments(prepareLongArgument),
                               "'--prepare' argument passed to CSSS should continue execution"
                              );
                Assert.AreEqual((Config.CSSSModes.Check | Config.CSSSModes.Prepare),
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Check, Prepare' if '--prepare' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '-s' argument is passed, the program should
        /// continue execution and the Start enum should be set
        /// </summary>
        [Test()]
        public void TestStartShortArgumentToProgram()
        {
            string[] startShortArgument = new string[1] { "-s" };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(bootstrapChecks.CheckArguments(startShortArgument),
                               "'-s' argument passed to CSSS should continue execution"
                              );
                Assert.AreEqual(Config.CSSSModes.Start,
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Start' if '-s' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '--start' argument is passed, the program should
        /// continue execution and the Start enum should be set
        /// </summary>
        [Test()]
        public void TestStartLongArgumentToProgram()
        {
            string[] startLongArgument = new string[1] { "--start" };

            Assert.Multiple(() =>
            {
                Assert.IsTrue(bootstrapChecks.CheckArguments(startLongArgument),
                               "'--start' argument passed to CSSS should continue execution"
                              );
                Assert.AreEqual(Config.CSSSModes.Start,
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Start' if '--start' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When an unknown argument is passed, the program should not
        /// continue execution and the Help enum should be set
        /// 
        /// While it is not possible to test every unknown possible input,
        /// passing a value of 'unknown' should peform in the expected way
        /// </summary>
        [Test()]
        public void TestUnknownArgumentToProgram()
        {
            string[] unknownArgument = new string[1] { "unknown" };

            Assert.Multiple(() =>
            {
                Assert.IsFalse(bootstrapChecks.CheckArguments(unknownArgument),
                               "Unknown argument passed to CSSS should not continue execution"
                              );
                Assert.AreEqual(Config.CSSSModes.Help,
                                config.CSSSProgramMode,
                                "Mode for CSSS should be 'Help' if an unknown argument is passed"
                               );
            });
        }
    }
}
