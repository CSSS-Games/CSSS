//  CSSSTests - CyberSecurity Scoring System Tests
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

using System.Security.Principal;
using CSSS;
using CSSSConfig;
using NUnit.Framework;

namespace CSSSTests
{
    /// <summary>
    /// Runs all tests related to the Kernel class
    /// </summary>
    [TestFixture()]
    public class KernelTests
    {
        private Kernel kernel;

        private Init init;

        private static Config config;

        /// <summary>
        /// Creates an instance of the config class
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            config = Config.GetCurrentConfig;

            // Allowing multiple instances of tests to run at once,
            // as the first test locks port 55555 from the init tests
            config.CSSSProgramMode = Config.CSSSModes.MultipleInstances;

            init = new Init();
        }

        /// <summary>
        /// Removes any reference to the init, config  and kernel classes
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            // Removing any set CSSS Mode flags
            // There doesn't seem to be a tidy way to do this
            config.CSSSProgramMode &= ~Config.CSSSModes.Check;
            config.CSSSProgramMode &= ~Config.CSSSModes.Help;
            config.CSSSProgramMode &= ~Config.CSSSModes.MultipleInstances;
            config.CSSSProgramMode &= ~Config.CSSSModes.Observe;
            config.CSSSProgramMode &= ~Config.CSSSModes.Prepare;
            config.CSSSProgramMode &= ~Config.CSSSModes.Start;

            init = null;
            config = null;
            kernel = null;
        }

        /// <summary>
        /// The Kernel PerformTasks function when CSSS is called with the
        /// '-c' argument should return true
        /// </summary>
        [Test()]
        public void TestKernelPerformTasksCheckArgumentReturnsTrue()
        {
            config.CSSSProgramMode = Config.CSSSModes.Check;

            kernel = new Kernel();

            Assert.IsTrue(kernel.PerformTasks(),
                          "The 'Check' argument passed to CSSS should only perform one task and return true");
        }

        /// <summary>
        /// The Kernel PerformTasks function when CSSS is called with the
        /// '-o' argument should return false
        /// </summary>
        [Test()]
        public void TestKernelPerformTasksObserveArgumentReturnsFalse()
        {
            config.CSSSProgramMode = Config.CSSSModes.Observe;

            kernel = new Kernel();

            Assert.IsFalse(kernel.PerformTasks(),
                           "The 'Observe' argument passed to CSSS should continually perform tasks and return false");
        }

        /// <summary>
        /// The Kernel PerformTasks function when CSSS is called with the
        /// '-p' argument should return true
        /// </summary>
        [Test()]
        public void TestKernelPerformTasksPrepareArgumentReturnsTrue()
        {
            // The 'Prepare' mode requires administrative permissions due to it modifying
            // parts of the system that normal users shouldn't have access to (`/etc`,
            // task scheduler). If this permission is not currently granted, then ignore
            // the test rather than making it fail
            // See: https://stackoverflow.com/a/5953294
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    Assert.Ignore("The 'Prepare' argument test has been ignored due to administrative permissions not being available");
                }
            }

            config.CSSSProgramMode = Config.CSSSModes.Prepare;

            kernel = new Kernel();

            Assert.IsTrue(kernel.PerformTasks(),
                          "The 'Prepare' argument passed to CSSS should only perform one task and return true");
        }

        /// <summary>
        /// The Kernel PerformTasks function when CSSS is called with the
        /// '-s' argument should return false
        /// </summary>
        [Test()]
        public void TestKernelPerformTasksStartArgumentReturnsFalse()
        {
            config.CSSSProgramMode = Config.CSSSModes.Start;

            kernel = new Kernel();

            Assert.IsFalse(kernel.PerformTasks(),
                           "The 'Start' argument passed to CSSS should continually perform tasks and return false");
        }
    }
}
