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
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program. If not, see <http://www.gnu.org/licenses/>.

using CSSSConfig;
using HtmlAgilityPack;
using NLog;
using System;
using System.IO;
using System.Text;

namespace CSSS
{
    /// <summary>
    /// The scoring report is a HTML file that is shown to the
    /// competitor to let them know some information about the
    /// current state of the computer
    /// 
    /// <para>Information that is shown on the scoring report:
    ///   * Points gained
    ///   * Points lost
    ///   * Total points
    ///   * Running time
    ///   * Gained points descriptions
    ///   * Lost points (penalties) descriptions
    /// </para>
    /// </summary>
    public class ScoringReport
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creating an instance of the CSSS config class, to be
        /// able to read and set values for it
        /// </summary>
        private static Config config = Config.GetCurrentConfig;

        /// <summary>
        /// The scoring report HTML, to be modified and saved
        /// </summary>
        private HtmlDocument scoringReportHTML;

        /// <summary>
        /// The filename of the ScoringReport.html file, set as a
        /// constant to prevent any accidental typos when using it
        /// in this class
        /// </summary>
        private const string scoringReportHTMLFilename = "ScoringReport.html";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSSS.ScoringReport"/> class,
        /// or throws an exception if the ScoringReport.html file can't be found
        /// </summary>
        public ScoringReport()
        {
            // While it is unlikely the scoring report goes missing,
            // it's best to check it's still there before making any
            // changes
            try
            {
                scoringReportHTML = new HtmlDocument();
                scoringReportHTML.Load(scoringReportHTMLFilename);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("The \"" + scoringReportHTMLFilename + "\" file could not be found");
            }
        }

        /// <summary>
        /// Updates the scoring report HTML file
        /// 
        /// <para>This function calls other functions to update
        /// the relevant sections of the scoring report</para>
        /// </summary>
        public void UpdateScoringReport()
        {
            logger.Info("Scoring Report");
            logger.Info("--------------");

            UpdatePointsGained();
            UpdatePointsLost();
            UpdatePointsTotal();

            UpdateRunningTime();

            UpdateGainedPointsTitle();
            UpdateGainedPointsDetails();

            UpdatePenaltyPointsTitle();
            UpdatePenaltyPointsDetails();

            SaveScoringReport();
        }

        /// <summary>
        /// Saves the scoring report to disk
        /// </summary>
        private void SaveScoringReport()
        {
            scoringReportHTML.Save(scoringReportHTMLFilename);
        }

        /// <summary>
        /// Updates the points gained value on the scoring report
        /// </summary>
        private void UpdatePointsGained()
        {
            var pointsGained = scoringReportHTML.DocumentNode.SelectSingleNode("//div[@id='overview-points-scored']");
            pointsGained.InnerHtml = config.PointsGainedTotal.ToString();
            logger.Info("  Points gained: {0}", pointsGained.InnerHtml);
        }

        /// <summary>
        /// Updates the points lost (penalties) value on the scoring report.
        /// The '-' symbol is removed before adding to the report so it
        /// doesn't read '-5 penalty points'
        /// </summary>
        private void UpdatePointsLost()
        {
            var pointsLost = scoringReportHTML.DocumentNode.SelectSingleNode("//div[@id='overview-points-lost']");
            pointsLost.InnerHtml = config.PointsLostTotal.ToString().Replace("-", "");
            logger.Info("  Penalty points: {0}", pointsLost.InnerHtml);
        }

        /// <summary>
        /// Updates the total number of points the competitor currently has,
        /// after lost points from penalties are taken away from those gained
        /// </summary>
        private void UpdatePointsTotal()
        {
            var pointsGained = config.PointsGainedTotal;
            var pointsLost = config.PointsLostTotal;

            var pointsTotal = scoringReportHTML.DocumentNode.SelectSingleNode("//div[@id='overview-points-total']");
            pointsTotal.InnerHtml = (pointsLost + pointsGained).ToString();
            logger.Info("  Total score: {0}", pointsTotal.InnerHtml);
        }

        /// <summary>
        /// Updates the amount of time since CSSS was initially started
        /// 
        /// <para>To get around reboots, where CSSS will have it's running
        /// time reset, on the very first time this function is called a
        /// hidden value is updated with the time it was updated at. From
        /// then onwards, this value can be compared to the current time to
        /// see the overall running time of the image</para>
        /// </summary>
        private void UpdateRunningTime()
        {
            var startTime = scoringReportHTML.DocumentNode.SelectSingleNode("//span[@id='image-start-time']");

            // If the saved start time is empty then CSSS is running on a
            // fresh image (e.g. only just started) and not from a restart
            // of the computer image
            if (string.IsNullOrEmpty(startTime.InnerHtml))
            {
                // Save the current time to the HTML node for future use
                startTime.InnerHtml = DateTime.UtcNow.ToString();
            }

            // Seeing the timespan between the image start and now
            // See: http://stackoverflow.com/a/9017567
            DateTime currentTime = DateTime.UtcNow;

            TimeSpan runningTime = currentTime.Subtract(Convert.ToDateTime(startTime.InnerHtml));

            var runtimeOverview = scoringReportHTML.DocumentNode.SelectSingleNode("//div[@id='overview-runtime']");
            runtimeOverview.InnerHtml = runningTime.Hours + ":" + runningTime.Minutes.ToString("00");
            logger.Info("  Running time: {0}", runtimeOverview.InnerHtml);
        }

        /// <summary>
        /// Updates the gained points title in the HTML file
        /// </summary>
        private void UpdateGainedPointsTitle()
        {
            var issuesSolved = scoringReportHTML.DocumentNode.SelectSingleNode("//span[@id='score-information-title-scored-total']");
            issuesSolved.InnerHtml = config.PointsGainedDescriptions.Count.ToString();

            var issuesTotal = scoringReportHTML.DocumentNode.SelectSingleNode("//span[@id='score-information-title-scored-total-issues']");
            issuesTotal.InnerHtml = config.TotalIssues.ToString();
        }

        /// <summary>
        /// Updates the gained points details in the HTML file
        /// </summary>
        private void UpdateGainedPointsDetails()
        {
            StringBuilder issueDescriptions = new StringBuilder();

            foreach (var issueSolved in config.PointsGainedDescriptions)
            {
                issueDescriptions.AppendLine(issueSolved + "<br>");
            }

            var detailScored = scoringReportHTML.DocumentNode.SelectSingleNode("//p[@id='score-information-detail-scored']");
            detailScored.InnerHtml = issueDescriptions.ToString();
        }

        /// <summary>
        /// Updates the lost points (penalties) title in the HTML file
        /// </summary>
        private void UpdatePenaltyPointsTitle()
        {
            var penaltiesIssued = config.PointsLostDescriptions.Count;

            var penaltiesTitleCount = scoringReportHTML.DocumentNode.SelectSingleNode("//span[@id='score-information-title-penalties-total']");
            penaltiesTitleCount.InnerHtml = penaltiesIssued.ToString();
        }

        /// <summary>
        /// Updates the lost points (penalties) details in the HTML file
        /// </summary>
        private void UpdatePenaltyPointsDetails()
        {
            StringBuilder penaltyDescriptions = new StringBuilder();

            foreach (var penalties in config.PointsLostDescriptions)
            {
                penaltyDescriptions.AppendLine(penalties + "<br>");
            }

            var detailPenalties = scoringReportHTML.DocumentNode.SelectSingleNode("//p[@id='score-information-detail-penalties']");
            detailPenalties.InnerHtml = penaltyDescriptions.ToString();
        }
    }
}
