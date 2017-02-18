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
    /// Runs all tests related to the init class
    /// </summary>
    [TestFixture()]
    public class InitTests
    {
        private Init initTests;

        /// <summary>
        /// Creates an instance of the init class
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            initTests = new Init();
        }

        /// <summary>
        /// Removes any reference to the init class
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            initTests = null;
        }

        /// <summary>
        /// Sees if the Operating System type returned matches
        /// one of the supported ones in CSSS. This test is dependent
        /// on the current Operating System running on the computer
        /// running the tests
        /// </summary>
        [Test()]
        public void TestOperatingSystemDetected()
        {
            Assert.AreNotEqual(Config.OperatingSystemType.Unknown,
                               initTests.SetOperatingSystemType(),
                               "The operating system detected should not be an unknown type");
        }
    }
}
