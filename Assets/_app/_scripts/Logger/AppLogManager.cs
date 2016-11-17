﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EA4S.Log {
    /// <summary>
    /// App Log Manager.
    /// </summary>
    public class AppLogManager {

        private string _session = "ToBeSet";
        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public string Session {
            get { return _session; }
            private set { _session = value; }
        }

        /// <summary>
        /// The minigame log manager concrete instance.
        /// </summary>
        public ILogManager MinigameLogManager = new MinigameLogManager();

        #region Proxy From Minigame log manager provider To App Log Intellingence

        protected internal void LogMinigameScore(MiniGameCode miniGameCode, float score) {
            AppManager.Instance.Teacher.logIntelligence.LogMiniGameScore(miniGameCode, score);
        }
        
        protected internal void LogPlay(string playSession, MiniGameCode miniGameCode, List<Teacher.LogIntelligence.PlayResultParameters> resultsList) {
            AppManager.Instance.Teacher.logIntelligence.LogPlay(Session, playSession, miniGameCode, resultsList);
        }

        protected internal void LogLearn(string playSession, MiniGameCode miniGameCode, List<Teacher.LogIntelligence.LearnResultParameters> resultsList) {
            AppManager.Instance.Teacher.logIntelligence.LogLearn(Session, playSession, miniGameCode, resultsList);
        }

        #endregion

        #region public API        
        /// <summary>
        /// Logs the play session score.
        /// </summary>
        /// <param name="playSessionId">The play session identifier.</param>
        /// <param name="score">The score.</param>
        public void LogPlaySessionScore(string playSessionId, float score) {
            AppManager.Instance.Teacher.logIntelligence.LogPlaySessionScore(playSessionId, score);
        }

        /// <summary>
        /// Logs the learning block score.
        /// </summary>
        /// <param name="learningBlock">The learning block.</param>
        /// <param name="score">The score.</param>
        public void LogLearningBlockScore(int learningBlock, float score) {
            AppManager.Instance.Teacher.logIntelligence.LogLearningBlockScore(learningBlock, score);
        }

        /// <summary>
        /// Logs the generic information.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="infoEvent">The information event.</param>
        /// <param name="parametersString">The parameters string.</param>
        public void LogInfo(InfoEvent infoEvent, string parametersString = "") {
            AppManager.Instance.Teacher.logIntelligence.LogInfo(Session, infoEvent, parametersString);
        }

        /// <summary>
        /// Logs the mood.
        /// </summary>
        /// <param name="mood">The mood.</param>
        public void LogMood(int mood) {
            AppManager.Instance.Teacher.logIntelligence.LogMood(mood);
        }

        public void StartApp() {
            LogInfo(InfoEvent.AppStarted);
        }
        #endregion
    }
}