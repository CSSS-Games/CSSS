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
using System.Collections.Generic;

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

            config.ResetScoringData();
        }

        /// <summary>
        /// Removes any reference to the issuechecks class
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
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
                Assert.AreEqual(0,
                                config.PointsLostTotal,
                                "Points should not be added to the PointsLostTotal when positive");
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
                Assert.AreEqual(0,
                                config.PointsLostTotal,
                                "Points should not be added to the PointsLostTotal when positive");
                Assert.Contains(description + " - " + points + " points",
                                config.PointsGainedDescriptions,
                                "The description and number of points should be added to the PointsGainedDescriptions list");
                Assert.True(config.pointsStatus.HasFlag(Config.PointsStatus.Gained),
                            "The 'Gained' pointsStatus enum should be set if the issue has not been solved before");
            });
        }

        /// <summary>
        /// When the points passed to PointsScored are negative, and
        /// the issue has already been triggered, only the points and
        /// points lost description list in the config should be updated
        /// </summary>
        [Test]
        public void TestPointsScoredNegativePointsTriggered()
        {
            int points = -5;
            string description = "TestPointsScoredNegativePointsTriggered";
            bool triggered = true;

            IssueChecksChecks.PointsScored(points, description, triggered);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(points,
                                config.PointsLostTotal,
                                "The points lost should be added to the PointsLostTotal integer");
                Assert.AreEqual(0,
                                config.PointsGainedTotal,
                                "Points should not be added to the PointsGainedTotal when negative");
                Assert.Contains(description + " - " + points.ToString().Replace("-", "") + " points",
                                config.PointsLostDescriptions,
                                "The description and number of points should be added to the PointsLostDescriptions list");
            });
        }

        /// <summary>
        /// When the points passed to PointsScored are negative, and
        /// the issue has not been triggered, the points and points
        /// lost description list in the config should be updated,
        /// along with the pointsStatus enum being set to Lost
        /// </summary>
        [Test]
        public void TestPointsScoredNegativePointsNotTriggered()
        {
            int points = -5;
            string description = "TestPointsScoredNegativePointsNotTriggered";
            bool triggered = false;

            IssueChecksChecks.PointsScored(points, description, triggered);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(points,
                                config.PointsLostTotal,
                                "The points scored should be added to the PointsLostTotal integer");
                Assert.AreEqual(0,
                                config.PointsGainedTotal,
                                "Points should not be added to the PointsGainedTotal when negative");
                Assert.Contains(description + " - " + points.ToString().Replace("-", "") + " points",
                                config.PointsLostDescriptions,
                                "The description and number of points should be added to the PointsLostDescriptions list");
                Assert.True(config.pointsStatus.HasFlag(Config.PointsStatus.Lost),
                            "The 'Lost' pointsStatus enum should be set if the penalty has not been triggered before");
            });
        }

        /// <summary>
        /// When the points passed to PointsScored are positive, only the
        /// points scored description list in the config should be updated,
        /// along with the pointsStatus enum being set to Lost
        /// </summary>
        [Test]
        public void TestPointsLostPositivePoints()
        {
            int points = 5;
            string description = "TestPointsLostPositivePoints";

            IssueChecksChecks.PointsLost(points, description);

            Assert.True(config.pointsStatus.HasFlag(Config.PointsStatus.Lost),
                        "The 'Lost' pointsStatus enum should be set if the issue has been solved but broken again");
        }

        /// <summary>
        /// When the points passed to PointsScored are negative, only the
        /// points lost description list in the config should be updated,
        /// along with the pointsStatus enum being set to Gained
        /// </summary>
        [Test]
        public void TestPointsLostNegativePoints()
        {
            int points = -5;
            string description = "TestPointsLostNegativePoints";

            IssueChecksChecks.PointsLost(points, description);

            Assert.True(config.pointsStatus.HasFlag(Config.PointsStatus.Gained),
                        "The 'Gained' pointsStatus enum should be set if the penalty has been resolved");
        }

        /// <summary>
        /// When multiple positive points have been scored, the points and
        /// points scored description list in the config should be updated
        /// </summary>
        [Test]
        public void TestMultiplePointsScoredPositivePoints()
        {
            int pointsOne = 5;
            string descriptionOne = "TestMultiplePointsScoredPositivePointsIssueOne";
            bool triggeredOne = true;

            IssueChecksChecks.PointsScored(pointsOne, descriptionOne, triggeredOne);

            int pointsTwo = 10;
            string descriptionTwo = "TestMultiplePointsScoredPositivePointsIssueTwo";
            bool triggeredTwo = true;

            IssueChecksChecks.PointsScored(pointsTwo, descriptionTwo, triggeredTwo);

            List<string> descriptionList = new List<string>();
            descriptionList.Add(descriptionOne + " - " + pointsOne + " points");
            descriptionList.Add(descriptionTwo + " - " + pointsTwo + " points");

            Assert.Multiple(() =>
            {
                Assert.AreEqual(pointsOne + pointsTwo,
                                config.PointsGainedTotal,
                                "The points scored should be added to the PointsGainedTotal integer");
                Assert.AreEqual(0,
                                config.PointsLostTotal,
                                "Points should not be added to the PointsLostTotal when positive");
                Assert.AreEqual(descriptionList,
                                config.PointsGainedDescriptions,
                                "The description and number of points should be added to the PointsGainedDescriptions list");
            });
        }

        /// <summary>
        /// When multiple negative points have been scored, the points and
        /// points lost description list in the config should be updated
        /// </summary>
        [Test]
        public void TestMultiplePointsScoredNegativePoints()
        {
            int pointsOne = -5;
            string descriptionOne = "TestMultiplePointsScoredNegativePointsIssueOne";
            bool triggeredOne = true;

            IssueChecksChecks.PointsScored(pointsOne, descriptionOne, triggeredOne);

            int pointsTwo = -10;
            string descriptionTwo = "TestMultiplePointsScoredNegativePointsIssueTwo";
            bool triggeredTwo = true;

            IssueChecksChecks.PointsScored(pointsTwo, descriptionTwo, triggeredTwo);

            List<string> descriptionList = new List<string>();
            descriptionList.Add(descriptionOne + " - " + pointsOne.ToString().Replace("-", "") + " points");
            descriptionList.Add(descriptionTwo + " - " + pointsTwo.ToString().Replace("-", "") + " points");

            Assert.Multiple(() =>
            {
                Assert.AreEqual(pointsOne + pointsTwo,
                                config.PointsLostTotal,
                                "The points scored should be added to the PointsLostTotal integer");
                Assert.AreEqual(0,
                                config.PointsGainedTotal,
                                "Points should not be added to the PointsGainedTotal when negative");
                Assert.AreEqual(descriptionList,
                                config.PointsLostDescriptions,
                                "The description and number of points should be added to the PointsLostDescriptions list");
            });
        }

        /// <summary>
        /// When both positive and negative points have been scored, the points
        /// gained and points lost, along with the relevant description lists
        /// in the config should be updated
        /// </summary>
        [Test]
        public void TestMultiplePointsScoredPositiveNegativePoints()
        {
            int pointsPositive = 5;
            string descriptionPositive = "TestMultiplePointsScoredPositivePointsIssueOne";
            bool triggeredPositive = true;

            IssueChecksChecks.PointsScored(pointsPositive, descriptionPositive, triggeredPositive);

            int pointsNegative = -10;
            string descriptionNegative = "TestMultiplePointsScoredNegativePointsIssueTwo";
            bool triggeredNegative = true;

            IssueChecksChecks.PointsScored(pointsNegative, descriptionNegative, triggeredNegative);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(pointsPositive,
                                config.PointsGainedTotal,
                                "The points gained should be added to the PointsGainedTotal integer");
                Assert.AreEqual(pointsNegative,
                                config.PointsLostTotal,
                                "The points lost should be added to the PointsLostTotal integer");
                Assert.Contains(descriptionPositive + " - " + pointsPositive + " points",
                                config.PointsGainedDescriptions,
                                "The description and number of points should be added to the PointsGainedDescriptions list");
                Assert.Contains(descriptionNegative + " - " + pointsNegative.ToString().Replace("-", "") + " points",
                                config.PointsLostDescriptions,
                                "The description and number of points should be added to the PointsLostDescriptions list");
            });
        }
    }
}
