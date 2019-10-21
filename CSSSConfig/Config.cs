//  CSSS - CyberSecurity Scoring System Config
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

using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CSSSConfig
{
    /// <summary>
    /// The main config class for the whole CSSS project
    /// 
    /// <para>This is implemented as a singleton class, so that
    /// settings that are put in place by the Init class are
    /// preserved to be accessed by any other class. The fourth
    /// version from csharpindepth.com/Articles/General/Singleton.aspx
    /// has been chosen as the model</para>
    /// 
    /// <para>All public functions are created as getters and setters
    /// so that a corresponding private member does not need to be
    /// created to store the values</para>
    /// </summary>
    public sealed class Config
    {
        private static readonly Config configInstance = new Config();

        static Config()
        {
        }

        private Config()
        {
        }

        /// <summary>
        /// Gets the get current config
        /// </summary>
        /// <value>The current config instance for CSSS</value>
        public static Config GetCurrentConfig
        {
            get
            {
                return configInstance;
            }
        }




        // **********************************************************
        //   CSSS Modes
        // **********************************************************

        /// <summary>
        /// CSSS can perform a variety of tasks depending on what options
        /// are passed to it. The valid program modes are set in the
        /// <see cref="T:CSSS.Bootstrap"/> bootstrap class and used by
        /// the <see cref="T:CSSS.Kernel"/> kernel
        /// </summary>
        [Flags]
        public enum CSSSModes
        {
            Help = 0x0,
            Check = 0x1,
            Prepare = 0x2,
            Observe = 0x4,
            Start = 0x8,
            MultipleInstances = 0x10,
            Shutdown = 0x20
        }

        /// <summary>
        /// The result of the bootstrap argument checks, used to decide what
        /// mode CSSS is going to be running in
        /// </summary>
        public CSSSModes CSSSProgramMode { get; set; }




        // **********************************************************
        //   Init Checks
        // **********************************************************

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="T:CSSS.Init"/> init
        /// tasks have been completed, used by the <see cref="T:CSSS.Kernel"/> kernel
        /// to see if it should start
        /// 
        /// <para>It is advised to set this value only via the <see cref="T:CSSS.Init"/> init
        /// class to prevent any unexpected functionality</para>
        /// </summary>
        /// <value><c>true</c> if init tasks completed; otherwise, <c>false</c>.</value>
        public bool InitTasksCompleted { get; set; }

        /// <summary>
        /// To prevent multiple CSSS instances being run, a TCP port is
        /// reserved when CSSS is run
        /// 
        /// When another instance of CSSS tries to run, it checks to see
        /// if the port is in use, and if so, that instance of CSSS exits
        /// </summary>
        public TcpListener CSSSRuntimeLockServer;




        // **********************************************************
        //   Operating System
        // **********************************************************

        /// <summary>
        /// The Operating Systems that CSSS supports to run checks on
        /// </summary>
        [Flags]
        public enum OperatingSystemType
        {
            Unknown,
            Linux,
            WinNT
        }

        /// <summary>
        /// Sets or gets the current Operating System that CSSS is
        /// running on
        /// </summary>
        /// <value>The type of the operating system</value>
        public OperatingSystemType operatingSystemType { get; set; }

        /// <summary>
        /// Gets or sets the name of the operating system
        /// </summary>
        /// <value>The name of the operating system</value>
        public string OperatingSystemName { get; set; }

        /// <summary>
        /// Gets or sets the version of the operating system
        /// </summary>
        /// <value>The name of the operating system</value>
        public string OperatingSystemVersion { get; set; }




        // **********************************************************
        //   Runtime Environment
        // **********************************************************

        /// <summary>
        /// The various runtime environments that CSSS can run on
        /// </summary>
        public enum RuntimeEnvironment
        {
            Unknown,
            DotNet,
            Mono
        }

        /// <summary>
        /// Gets or sets the runtime environment
        /// </summary>
        /// <value>The runtime environment</value>
        public RuntimeEnvironment runtimeEnvironment { get; set; }




        // **********************************************************
        //   Issue Files
        // **********************************************************

        /// <summary>
        /// A dictionary of the issues from the JSON files
        /// 
        /// <para>The first value in the dictionary is the category
        /// that the issue is from, which should be unique and included
        /// in each JSON file. The second value is dynamic, which holds
        /// the actual contents of the JSON file, that is used by CSSS
        /// to check the current state of the computer against what is
        /// to be expected</para>
        /// </summary>
        private Dictionary<string, dynamic> IssueFileList = new Dictionary<string, dynamic>();

        /// <summary>
        /// Adds an issue file to the dictionary of available issues,
        /// so it can be accessed when performing checks
        /// </summary>
        /// <param name="issueFileCategory">The category the issue file is for</param>
        /// <param name="issueFileJSON">The JSON contents of the whole issue file</param>
        public void AddIssueFile(string issueFileCategory, dynamic issueFileJSON)
        {
            try
            {
                IssueFileList.Add(issueFileCategory, issueFileJSON);
            }
            catch (ArgumentException)
            {
                throw new OperationCanceledException("An issue file with a category of \"" + issueFileCategory + "\" already exists... skipping adding");
            }
        }

        /// <summary>
        /// Gets the issue file JSON of the issue category
        /// </summary>
        /// <returns>The issue file JSON object</returns>
        /// <param name="issueFileCategory">The category of the issue file</param>
        public dynamic GetIssueFile(string issueFileCategory)
        {
            dynamic issueFileJSON;
            if (IssueFileList.TryGetValue(issueFileCategory, out issueFileJSON))
            {
                return issueFileJSON;
            }
            else
            {
                throw new KeyNotFoundException("An issue file with a category of \"" + issueFileCategory + "\" could not be found");
            }
        }




        // **********************************************************
        //   Scoring
        // **********************************************************

        /// <summary>
        /// Resets the points scores and descriptions that are stored
        /// so that the information presented in the scoring report
        /// is a valid representation of the current status
        /// </summary>
        public void ResetScoringData()
        {
            // Enums can't just be bitwise not against an empty
            // value, as if the flag isn't set it will be.
            // Therefore, the and-not bitwise operation should be used
            // See: http://stackoverflow.com/a/11011051
            pointsStatus &= ~PointsStatus.Gained;
            pointsStatus &= ~PointsStatus.Lost;

            TotalIssues = 0;

            PointsGainedTotal = 0;
            PointsGainedDescriptions.Clear();

            PointsLostTotal = 0;
            PointsLostDescriptions.Clear();
        }

        /// <summary>
        /// Gets or sets the total number of issues that need to
        /// be fixed
        /// </summary>
        /// <value>The total number of issues</value>
        public int TotalIssues { get; set; }

        /// <summary>
        /// The current status of the points gained during the current
        /// issue check run, used to notify the user of any changes:
        ///   * Unchanged: The amount of points scored is the same as last run
        ///   * Gained: 
        /// 
        /// <para>Enum flags are used here rather than a stright enum so
        /// that it can be used if points have both been gained and lost
        /// during the issue checks without needing to check the status
        /// of this each time, but is instead checked before the user
        /// is notified. Points can be scored and lost in the same issue
        /// check run, so both options can be active at the same time</para>
        /// </summary>
        [Flags]
        public enum PointsStatus
        {
            Unchanged = 0x1,
            Gained = 0x2,
            Lost = 0x4
        }

        /// <summary>
        /// Gets or sets the points status so the correct notification
        /// can be shown if needed
        /// </summary>
        /// <value>The points status</value>
        public PointsStatus pointsStatus { get; set; }

        /// <summary>
        /// Gets or sets the current amount of points gained during this
        /// issue check run
        /// </summary>
        /// <value>The points gained total</value>
        public int PointsGainedTotal { get; set; }

        /// <summary>
        /// A list of the issues that have been fixed, including the
        /// description and points value
        /// </summary>
        public List<string> PointsGainedDescriptions = new List<string>();

        /// <summary>
        /// Adds a description and point amount to the points gained
        /// descriptions array, and increases the points gained total
        /// value with amount scored
        /// </summary>
        /// <param name="points">The amount of points gained for the issue</param>
        /// <param name="description">The description of the issue</param>
        public void UpdatePointsGained(int points, string description)
        {
            PointsGainedTotal += points;
            PointsGainedDescriptions.Add(description + " - " + points + " points");
        }

        /// <summary>
        /// Gets or sets the current amount of points lost during this
        /// issue check run from penalties
        /// </summary>
        /// <value>The points lost total</value>
        public int PointsLostTotal { get; set; }

        /// <summary>
        /// A list of the penalties that have been given, including the
        /// description and points value
        /// </summary>
        public List<string> PointsLostDescriptions = new List<string>();

        /// <summary>
        /// Adds a description and point amount to the points lost
        /// descriptions array, and increases the points lost total
        /// value with amount penalised
        /// </summary>
        /// <param name="points">The amount of points lost for the penalty</param>
        /// <param name="description">The description of the penalty</param>
        public void UpdatePointsLost(int points, string description)
        {
            PointsLostTotal += points;
            PointsLostDescriptions.Add(description + " - " + points.ToString().Replace("-", "") + " points");
        }
    }
}
