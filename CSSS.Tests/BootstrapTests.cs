﻿//  CSSSTests - CyberSecurity Scoring System Tests
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

using CSSS;
using CSSSConfig;
using NUnit.Framework;

namespace CSSSTests
{
    /// <summary>
    /// Runs all tests related to the bootstrap class
    /// </summary>
    [TestFixture()]
    public class BootstrapTests
    {
        private Bootstrap? bootstrapChecks;

        private static Config? config;

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
            // Removing any set CSSS Mode flags
            // There doesn't seem to be a tidy way to do this
            if (config == null)
            {
                return;
            }
            config.CSSSProgramMode &= ~Config.CSSSModes.Check;
            config.CSSSProgramMode &= ~Config.CSSSModes.Help;
            config.CSSSProgramMode &= ~Config.CSSSModes.MultipleInstances;
            config.CSSSProgramMode &= ~Config.CSSSModes.Observe;
            config.CSSSProgramMode &= ~Config.CSSSModes.Prepare;
            config.CSSSProgramMode &= ~Config.CSSSModes.Start;

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
            string[] emptyArguments = Array.Empty<string>();

            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(bootstrapChecks.CheckArguments(emptyArguments),
                        Is.False,
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(helpShortArgument),
                            Is.False,
                               "'-h' argument passed to CSSS should not continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                            Is.EqualTo(Config.CSSSModes.Help),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(helpLongArgument), Is.False,
                               "'--help' argument passed to CSSS should not continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                                Is.EqualTo(Config.CSSSModes.Help),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(checkShortArgument), Is.True,
                               "'-c' argument passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                                Is.EqualTo(Config.CSSSModes.Check),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(checkLongArgument), Is.True,
                               "'--check' argument passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                                Is.EqualTo(Config.CSSSModes.Check),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(observeShortArgument), Is.True,
                               "'-o' argument passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                            Is.EqualTo((Config.CSSSModes.Check | Config.CSSSModes.Observe)),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(observeLongArgument), Is.True,
                               "'--observe' argument passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                                Is.EqualTo((Config.CSSSModes.Check | Config.CSSSModes.Observe)),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(prepareShortArgument), Is.True,
                               "'-p' argument passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                            Is.EqualTo((Config.CSSSModes.Check | Config.CSSSModes.Prepare)),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(prepareLongArgument),
                            Is.True,
                               "'--prepare' argument passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                                Is.EqualTo((Config.CSSSModes.Check | Config.CSSSModes.Prepare)),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(startShortArgument), Is.True,
                               "'-s' argument passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode,
                                Is.EqualTo(Config.CSSSModes.Start),
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(startLongArgument),
                            Is.True,
                               "'--start' argument passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode, Is.EqualTo(Config.CSSSModes.Start),
                                "Mode for CSSS should be 'Start' if '--start' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '-m' argument is passed, the program should not
        /// continue execution and the MultipleInstances enum should be set
        /// </summary>
        [Test()]
        public void TestMultipleInstancesShortArgumentToProgram()
        {
            string[] multipleInstancesShortArgument = new string[1] { "-m" };
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(multipleInstancesShortArgument), Is.False,
                               "'-m' argument passed to CSSS should not continue execution"
                              );
                Assert.That(config.CSSSProgramMode, Is.EqualTo(Config.CSSSModes.MultipleInstances),
                                "Mode for CSSS should be 'MultipleInstances' if '-m' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '--multiple' argument is passed, the program should not
        /// continue execution and the MultipleInstances enum should be set
        /// </summary>
        [Test()]
        public void TestMultipleInstancesLongArgumentToProgram()
        {
            string[] multipleInstancesLongArgument = new string[1] { "--multiple" };
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(multipleInstancesLongArgument), Is.False,
                               "'--multiple' argument passed to CSSS should not continue execution"
                              );
                Assert.That(config.CSSSProgramMode, Is.EqualTo(Config.CSSSModes.MultipleInstances),
                                "Mode for CSSS should be 'MultipleInstances' if '--multiple' argument is passed"
                               );
            });
        }

        /// <summary>
        /// When the '-o -m' arguments are passed, the program should
        /// continue execution and the Check, Observe and MultipleInstances
        /// enums should be set
        /// </summary>
        [Test()]
        public void TestObserveMultipleInstancesShortArgumentsToProgram()
        {
            string[] observeMultipleInstancesShortArguments = new string[2] { "-o", "-m" };
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(observeMultipleInstancesShortArguments), Is.True, "'-o -m' arguments passed to CSSS should continue execution");
                Assert.That(config.CSSSProgramMode,
                            Is.EqualTo((Config.CSSSModes.Check | Config.CSSSModes.Observe | Config.CSSSModes.MultipleInstances)),
                                "Mode for CSSS should be 'Check, Observe, MultipleInstances' if '-o -m' arguments are passed"
                               );
            });
        }

        /// <summary>
        /// When the '--observe --multiple' arguments are passed, the program should
        /// continue execution and the Check, Observe and MultipleInstances
        /// enums should be set
        /// </summary>
        [Test()]
        public void TestObserveMultipleInstancesLongArgumentsToProgram()
        {
            string[] observeMultipleInstancesLongArguments = new string[2] { "--observe", "--multiple" };
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(observeMultipleInstancesLongArguments), Is.True, "'--observe --multiple' arguments passed to CSSS should continue execution");
                Assert.That(config.CSSSProgramMode, Is.EqualTo((Config.CSSSModes.Check | Config.CSSSModes.Observe | Config.CSSSModes.MultipleInstances)),
                                "Mode for CSSS should be 'Check, Observe, MultipleInstances' if '--observe --multiple' arguments are passed"
                               );
            });
        }

        /// <summary>
        /// When the '-p -m' arguments are passed, the program should
        /// continue execution and the Check, Prepare and MultipleInstances
        /// enums should be set
        /// </summary>
        [Test()]
        public void TestPrepareMultipleInstancesShortArgumentsToProgram()
        {
            string[] prepareShortArgument = new string[2] { "-p", "-m" };
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(prepareShortArgument), Is.True, "'-p -m' arguments passed to CSSS should continue execution");
                Assert.That(config.CSSSProgramMode, Is.EqualTo((Config.CSSSModes.Check | Config.CSSSModes.Prepare | Config.CSSSModes.MultipleInstances)),
                                "Mode for CSSS should be 'Check, Prepare, MultipleInstances' if '-p -m' arguments are passed"
                               );
            });
        }

        /// <summary>
        /// When the '--prepare --multiple' arguments are passed, the program should
        /// continue execution and the Check, Prepare and MultipleInstances
        /// enums should be set
        /// </summary>
        [Test()]
        public void TestPrepareMultipleInstancesLongArgumentsToProgram()
        {
            string[] prepareLongArgument = new string[2] { "--prepare", "--multiple" };
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(prepareLongArgument), Is.True,
                    "'--prepare --multiple' arguments passed to CSSS should continue execution");
                Assert.That(config.CSSSProgramMode, Is.EqualTo((Config.CSSSModes.Check | Config.CSSSModes.Prepare | Config.CSSSModes.MultipleInstances)),
                                "Mode for CSSS should be 'Check, Prepare, MultipleInstances' if '--prepare --multiple' arguments are passed"
                               );
            });
        }

        /// <summary>
        /// When the '-s -m' arguments are passed, the program should
        /// continue execution and the Start and MultipleInstances
        /// enums should be set
        /// </summary>
        [Test()]
        public void TestStartMultipleInstancesShortArgumentsToProgram()
        {
            string[] prepareShortArgument = new string[2] { "-s", "-m" };
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(prepareShortArgument), Is.True,
                               "'-s -m' arguments passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode, Is.EqualTo((Config.CSSSModes.Start | Config.CSSSModes.MultipleInstances)),
                                "Mode for CSSS should be 'Start, MultipleInstances' if '-s -m' arguments are passed"
                               );
            });
        }

        /// <summary>
        /// When the '--start --multiple' arguments are passed, the program should
        /// continue execution and the Start and MultipleInstances
        /// enums should be set
        /// </summary>
        [Test()]
        public void TestStartMultipleInstancesLongArgumentsToProgram()
        {
            string[] prepareLongArgument = new string[2] { "--start", "--multiple" };
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(prepareLongArgument), Is.True,
                               "'--start --multiple' arguments passed to CSSS should continue execution"
                              );
                Assert.That(config.CSSSProgramMode, Is.EqualTo((Config.CSSSModes.Start | Config.CSSSModes.MultipleInstances)),
                                "Mode for CSSS should be 'Start, MultipleInstances' if '--start --multiple' arguments are passed"
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
            Assert.That(bootstrapChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(bootstrapChecks.CheckArguments(unknownArgument), Is.False,
                               "Unknown argument passed to CSSS should not continue execution"
                              );
                Assert.That(config.CSSSProgramMode, Is.EqualTo(Config.CSSSModes.Help),
                                "Mode for CSSS should be 'Help' if an unknown argument is passed"
                               );
            });
        }
    }
}
