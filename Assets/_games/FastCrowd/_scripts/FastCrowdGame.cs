﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EA4S.FastCrowd
{
    public class FastCrowdGame : MiniGame
    {
        public QuestionManager QuestionManager;

        public AnturaRunnerController antura;

        public List<ILivingLetterData> CurrentChallenge = new List<ILivingLetterData>();
        public List<ILivingLetterData> NoiseData = new List<ILivingLetterData>();

        public IQuestionPack CurrentQuestion = null; // optional

        private int currentScore;
        public int CurrentScore
        {
            get
            {
                return currentScore;
            }
            private set
            {
                currentScore = value;
                Context.GetOverlayWidget().SetStarsScore(currentScore);
            }
        }

        public int QuestionNumber = 0;

        [HideInInspector]
        public bool isTimesUp;

        int stars1Threshold
        {
            get
            {
                switch (FastCrowdConfiguration.Instance.Variation)
                {
                    case FastCrowdVariation.Words:
                        return 8;
                    case FastCrowdVariation.Counting:
                        return 5;
                    case FastCrowdVariation.Alphabet:
                        return 5;
                    default:
                        return 3;
                }
            }
        }

        int stars2Threshold
        {
            get
            {
                switch (FastCrowdConfiguration.Instance.Variation)
                {
                    case FastCrowdVariation.Words:
                        return 12;
                    case FastCrowdVariation.Counting:
                        return 10;
                    case FastCrowdVariation.Alphabet:
                        return 10;
                    default:
                        return 5;
                }
            }
        }

        int stars3Threshold
        {
            get
            {
                switch (FastCrowdConfiguration.Instance.Variation)
                {
                    case FastCrowdVariation.Words:
                        return 16;
                    case FastCrowdVariation.Counting:
                        return 15;
                    case FastCrowdVariation.Alphabet:
                        return 15;
                    default:
                        return 7;
                }
            }
        }


        public int CurrentStars
        {
            get
            {
                if (CurrentScore < stars1Threshold)
                    return 0;
                if (CurrentScore < stars2Threshold)
                    return 1;
                if (CurrentScore < stars3Threshold)
                    return 2;
                return 3;
            }
        }

        public FastCrowdIntroductionState IntroductionState { get; private set; }
        public FastCrowdQuestionState QuestionState { get; private set; }
        public FastCrowdPlayState PlayState { get; private set; }
        public FastCrowdResultState ResultState { get; private set; }
        public FastCrowdEndState EndState { get; private set; }

        public void ResetScore()
        {
            CurrentScore = 0;
        }

        public void IncrementScore()
        {
            ++CurrentScore;
        }

        protected override IGameConfiguration GetConfiguration()
        {
            return FastCrowdConfiguration.Instance;
        }

        protected override IGameState GetInitialState()
        {
            return IntroductionState;
        }

        protected override void OnInitialize(IGameContext context)
        {
            //float difficulty = FastCrowdConfiguration.Instance.Difficulty;

            IntroductionState = new FastCrowdIntroductionState(this);
            QuestionState = new FastCrowdQuestionState(this);
            PlayState = new FastCrowdPlayState(this);
            ResultState = new FastCrowdResultState(this);
            EndState = new FastCrowdEndState(this);

            QuestionManager.wordComposer.gameObject.SetActive(
                FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Spelling
                );

            Physics.gravity = Vector3.up * -40;

            Context.GetOverlayWidget().Initialize(true, true, false);
            Context.GetOverlayWidget().SetStarsThresholds(stars1Threshold, stars2Threshold, stars3Threshold);
        }


        public void ShowChallengePopupWidget(bool showAsGoodAnswer, Action callback)
        {
            var popupWidget = Context.GetPopupWidget();
            popupWidget.Show();
            popupWidget.SetButtonCallback(callback);

            if (showAsGoodAnswer)
            {
                popupWidget.SetTitle(TextID.WELL_DONE);
                popupWidget.SetMark(true, true);
            }
            else
                popupWidget.SetTitle("" + QuestionNumber, false);

            if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Spelling)
            {
                var question = CurrentQuestion.GetQuestion();
                popupWidget.SetWord((LL_WordData)question);
                Context.GetAudioManager().PlayLetterData(question);
            }
            else if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Words)
            {
                var stringListOfWords = "";
                for (int i = 0, count = CurrentChallenge.Count; i < count; ++i)
                {
                    Debug.Log(CurrentChallenge[i]);

                    var word = ((LL_WordData)CurrentChallenge[i]).Data.Arabic;

                    if (i == 0)
                        stringListOfWords = word;
                    else
                        stringListOfWords = word + " " + stringListOfWords;
                }

                popupWidget.SetMessage(stringListOfWords, true);
            }
            else if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Alphabet)
            {
                popupWidget.SetTitle("", false);
                popupWidget.SetMessage("Alphabet!", false);
            }
            else if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Letter)
            {
                popupWidget.SetTitle("Identify the shapes of the letter", false);

                var question = CurrentQuestion.GetQuestion();
                popupWidget.SetMessage(question.TextForLivingLetter, true);
                Context.GetAudioManager().PlayLetterData(question);
            }
            else if (FastCrowdConfiguration.Instance.Variation == FastCrowdVariation.Counting)
            {
                popupWidget.SetTitle("", false);
                var question = CurrentQuestion.GetQuestion();
                popupWidget.SetMessage("Number " + QuestionNumber, true);
                Context.GetAudioManager().PlayLetterData(question);
            }
        }
    }
}