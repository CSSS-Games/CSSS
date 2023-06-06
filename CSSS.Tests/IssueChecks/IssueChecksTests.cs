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

using CSSSConfig;

namespace IssueChecks
{
    /// <summary>
    /// Runs all tests related to the issuechecks class
    /// </summary>
    [TestFixture]
    public class IssueChecksTests
    {
        private IssueChecks? issueChecksChecks;

        private static Config? config;

        /// <summary>
        /// Creates an instance of the issuechecks class
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            issueChecksChecks = new IssueChecks();
            config = Config.GetCurrentConfig;

            config.ResetScoringData();
        }

        /// <summary>
        /// Removes any reference to the issuechecks class
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            issueChecksChecks = null;
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
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int points = 5;
            string description = "TestPointsScoredPositivePointsTriggered";
            bool triggered = true;

            issueChecksChecks.PointsScored(points, description, triggered);

            Assert.Multiple(() =>
            {
                Assert.That(config.PointsGainedTotal,
                            Is.EqualTo(points),
                            "The points scored should be added to the PointsGainedTotal integer");
                Assert.That(config.PointsLostTotal,
                            Is.Zero,
                            "Points should not be added to the PointsLostTotal when positive");
                Assert.That(config.PointsGainedDescriptions.Contains(description + " - " + points + " points"),
                            Is.True,
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
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int points = 5;
            string description = "TestPointsScoredPositivePointsNotTriggered";
            bool triggered = false;

            issueChecksChecks.PointsScored(points, description, triggered);

            Assert.Multiple(() =>
            {
                Assert.That(config.PointsGainedTotal,
                            Is.EqualTo(points),
                            "The points scored should be added to the PointsGainedTotal integer");
                Assert.That(config.PointsLostTotal,
                            Is.Zero,
                            "Points should not be added to the PointsLostTotal when positive");
                Assert.That(config.PointsGainedDescriptions.Contains(description + " - " + points + " points"),
                            Is.True,
                            "The description and number of points should be added to the PointsGainedDescriptions list");
                Assert.That(config.pointsStatus.HasFlag(Config.PointsStatus.Gained),
                            Is.True,
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
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int points = -5;
            string description = "TestPointsScoredNegativePointsTriggered";
            bool triggered = true;

            issueChecksChecks.PointsScored(points, description, triggered);

            Assert.Multiple(() =>
            {
                Assert.That(config.PointsLostTotal,
                            Is.EqualTo(points),
                            "The points lost should be added to the PointsLostTotal integer");
                Assert.That(config.PointsGainedTotal, Is.Zero, "Points should not be added to the PointsGainedTotal when negative");
                Assert.That(config.PointsLostDescriptions.Contains(description + " - " + points.ToString().Replace("-", "") + " points"),
                            Is.True,
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
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int points = -5;
            string description = "TestPointsScoredNegativePointsNotTriggered";
            bool triggered = false;

            issueChecksChecks.PointsScored(points, description, triggered);

            Assert.Multiple(() =>
            {
                Assert.That(config.PointsLostTotal, Is.EqualTo(points), "The points scored should be added to the PointsLostTotal integer");
                Assert.That(config.PointsGainedTotal, Is.Zero, "Points should not be added to the PointsGainedTotal when negative");
                Assert.That(config.PointsLostDescriptions.Contains(description + " - " + points.ToString()
                            .Replace("-", "") + " points"),
                            Is.True,
                            "The description and number of points should be added to the PointsLostDescriptions list");
                Assert.That(config.pointsStatus.HasFlag(Config.PointsStatus.Lost), Is.True,
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
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int points = 5;
            string description = "TestPointsLostPositivePoints";

            issueChecksChecks.PointsLost(points, description);

            Assert.That(config.pointsStatus.HasFlag(Config.PointsStatus.Lost), Is.True, "The 'Lost' pointsStatus enum should be set if the issue has been solved but broken again");
        }

        /// <summary>
        /// When the points passed to PointsScored are negative, only the
        /// points lost description list in the config should be updated,
        /// along with the pointsStatus enum being set to Gained
        /// </summary>
        [Test]
        public void TestPointsLostNegativePoints()
        {
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int points = -5;
            string description = "TestPointsLostNegativePoints";

            issueChecksChecks.PointsLost(points, description);

            Assert.That(config.pointsStatus.HasFlag(Config.PointsStatus.Gained), Is.True,
                        "The 'Gained' pointsStatus enum should be set if the penalty has been resolved");
        }

        /// <summary>
        /// When multiple positive points have been scored, the points and
        /// points scored description list in the config should be updated
        /// </summary>
        [Test]
        public void TestMultiplePointsScoredPositivePoints()
        {
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int pointsOne = 5;
            string descriptionOne = "TestMultiplePointsScoredPositivePointsIssueOne";
            bool triggeredOne = true;

            issueChecksChecks.PointsScored(pointsOne, descriptionOne, triggeredOne);

            int pointsTwo = 10;
            string descriptionTwo = "TestMultiplePointsScoredPositivePointsIssueTwo";
            bool triggeredTwo = true;

            issueChecksChecks.PointsScored(pointsTwo, descriptionTwo, triggeredTwo);

            List<string> descriptionList = new List<string>
            {
                descriptionOne + " - " + pointsOne + " points",
                descriptionTwo + " - " + pointsTwo + " points"
            };

            Assert.Multiple(() =>
            {
                Assert.That(config.PointsGainedTotal,
                            Is.EqualTo(pointsOne + pointsTwo),
                            "The points scored should be added to the PointsGainedTotal integer");
                Assert.That(config.PointsLostTotal,
                            Is.Zero,
                            "Points should not be added to the PointsLostTotal when positive");
                Assert.That(config.PointsGainedDescriptions,
                            Is.EqualTo(descriptionList),
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
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int pointsOne = -5;
            string descriptionOne = "TestMultiplePointsScoredNegativePointsIssueOne";
            bool triggeredOne = true;

            issueChecksChecks.PointsScored(pointsOne, descriptionOne, triggeredOne);

            int pointsTwo = -10;
            string descriptionTwo = "TestMultiplePointsScoredNegativePointsIssueTwo";
            bool triggeredTwo = true;

            issueChecksChecks.PointsScored(pointsTwo, descriptionTwo, triggeredTwo);

            List<string> descriptionList = new List<string>
            {
                descriptionOne + " - " + pointsOne.ToString().Replace("-", "") + " points",
                descriptionTwo + " - " + pointsTwo.ToString().Replace("-", "") + " points"
            };

            Assert.Multiple(() =>
            {
                Assert.That(config.PointsLostTotal, Is.EqualTo(pointsOne + pointsTwo), "The points scored should be added to the PointsLostTotal integer");
                Assert.That(config.PointsGainedTotal, Is.Zero, "Points should not be added to the PointsGainedTotal when negative");
                Assert.That(config.PointsLostDescriptions, Is.EqualTo(descriptionList), "The description and number of points should be added to the PointsLostDescriptions list");
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
            Assert.That(issueChecksChecks, Is.Not.Null);
            Assert.That(config, Is.Not.Null);

            int pointsPositive = 5;
            string descriptionPositive = "TestMultiplePointsScoredPositivePointsIssueOne";
            bool triggeredPositive = true;

            issueChecksChecks.PointsScored(pointsPositive, descriptionPositive, triggeredPositive);

            int pointsNegative = -10;
            string descriptionNegative = "TestMultiplePointsScoredNegativePointsIssueTwo";
            bool triggeredNegative = true;

            issueChecksChecks.PointsScored(pointsNegative, descriptionNegative, triggeredNegative);

            Assert.Multiple(() =>
            {
                Assert.That(config.PointsGainedTotal, Is.EqualTo(pointsPositive), "The points gained should be added to the PointsGainedTotal integer");
                Assert.That(config.PointsLostTotal, Is.EqualTo(pointsNegative), "The points lost should be added to the PointsLostTotal integer");
                Assert.That(config.PointsGainedDescriptions.Contains(descriptionPositive + " - " + pointsPositive + " points"),
                            Is.True,
                            "The description and number of points should be added to the PointsGainedDescriptions list");
                Assert.That(config.PointsLostDescriptions.Contains(descriptionNegative + " - " + pointsNegative.ToString().Replace("-", "") + " points"),
                            Is.True,
                            "The description and number of points should be added to the PointsLostDescriptions list");
            });
        }
    }
}
