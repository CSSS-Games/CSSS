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
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program. If not, see <http://www.gnu.org/licenses/>.

using CSSS;
using CSSSConfig;
using NUnit.Framework;
using System;

namespace IssueChecks
{
    /// <summary>
    /// Runs all tests related to the issuechecks class
    /// </summary>
    [TestFixture]
    public class IssueChecksTests
    {
        private IssueChecks IssueChecksChecks;

        private static Config config;

        /// <summary>
        /// Creates an instance of the issuechecks class
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            IssueChecksChecks = new IssueChecks();
            config = Config.GetCurrentConfig;

            config.PointsGainedTotal = 0;
            config.PointsLostTotal = 0;
        }

        /// <summary>
        /// Removes any reference to the issuechecks class
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            config.pointsStatus = config.pointsStatus ^ Config.PointsStatus.Gained;
            config.pointsStatus = config.pointsStatus ^ Config.PointsStatus.Lost;

            IssueChecksChecks = null;
            config = null;
        }

        /// <summary>
        /// When the points passed to PointsScored are positive, and
        /// the issue has already been triggered, only the points and
        /// points scored description list in the config should be updated
        /// </summary>
        [Test]
        public void TestPointsScoredPositivePointsTriggered()
        {
            int points = 5;
            string description = "TestPointsScoredPositivePointsTriggered";
            bool triggered = true;

            IssueChecksChecks.PointsScored(points, description, triggered);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(points,
                                config.PointsGainedTotal,
                                "The points scored should be added to the PointsGainedTotal integer");
                Assert.Contains(description + " - " + points + " points",
                                config.PointsGainedDescriptions,
                                "The description and number of points should be added to the PointsGainedDescriptions list");
            });
        }

        /// <summary>
        /// When the points passed to PointsScored are positive, and
        /// the issue has not been triggered, the points and points
        /// scored description list in the config should be updated,
        /// along with the pointsStatus enum being set to Gained
        /// </summary>
        [Test]
        public void TestPointsScoredPositivePointsNotTriggered()
        {
            int points = 5;
            string description = "TestPointsScoredPositivePointsNotTriggered";
            bool triggered = false;

            IssueChecksChecks.PointsScored(points, description, triggered);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(points,
                                config.PointsGainedTotal,
                                "The points scored should be added to the PointsGainedTotal integer");
                Assert.Contains(description + " - " + points + " points",
                                config.PointsGainedDescriptions,
                                "The description and number of points should be added to the PointsGainedDescriptions list");
                Assert.AreEqual(Config.PointsStatus.Gained,
                                config.pointsStatus,
                                "The 'Gained' pointsStatus enum should be set if the issue has not been solved before");
            });
        }
    }
}
