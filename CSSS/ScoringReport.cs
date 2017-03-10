﻿//  CSSS - CyberSecurity Scoring System
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
        private HtmlDocument ScoringReportHTML;

        /// <summary>
        /// The filename of the ScoringReport.html file, set as a
        /// constant to prevent any accidental typos when using it
        /// in this class
        /// </summary>
        private const string ScoringReportHTMLFilename = "ScoringReport.html";

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
                ScoringReportHTML = new HtmlDocument();
                ScoringReportHTML.Load(ScoringReportHTMLFilename);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("The \"" + ScoringReportHTMLFilename + "\" file could not be found");
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
            UpdatePointsGained();

            SaveScoringReport();
        }

        /// <summary>
        /// Saves the scoring report to disk
        /// </summary>
        private void SaveScoringReport()
        {
            ScoringReportHTML.Save(ScoringReportHTMLFilename);
        }

        /// <summary>
        /// Updates the points gained value on the scoring report
        /// </summary>
        private void UpdatePointsGained()
        {
            var pointsGained = ScoringReportHTML.DocumentNode.SelectSingleNode("//div[@id='overview-points-scored']");
            pointsGained.InnerHtml = config.PointsGainedTotal.ToString();
        }
    }
}
