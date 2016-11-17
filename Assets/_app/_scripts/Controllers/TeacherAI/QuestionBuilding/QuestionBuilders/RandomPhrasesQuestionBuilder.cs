﻿using EA4S.Teacher;
using System.Collections.Generic;

namespace EA4S
{

    public class RandomPhrasesQuestionBuilder : IQuestionBuilder
    {
        private int nPacks;
        private int nCorrect;
        private int nWrong;
        private bool firstCorrectIsQuestion;
        private PackListHistory packListHistory;

        public RandomPhrasesQuestionBuilder(int nPacks, int nCorrect = 1, int nWrong = 0, 
            bool firstCorrectIsQuestion = false,
            PackListHistory packListHistory = PackListHistory.NoFilter)
        {
            this.nPacks = nPacks;
            this.nCorrect = nCorrect;
            this.nWrong = nWrong;
            this.firstCorrectIsQuestion = firstCorrectIsQuestion;
            this.packListHistory = packListHistory;
        }

        private List<string> previousPacksIDs = new List<string>();

        public List<QuestionPackData> CreateAllQuestionPacks()
        {
            previousPacksIDs.Clear();

            List<QuestionPackData> packs = new List<QuestionPackData>();
            for (int pack_i = 0; pack_i < nPacks; pack_i++)
            {
                var pack = CreateSingleQuestionPackData();
                packs.Add(pack);
            }

            return packs;
        }

        private QuestionPackData CreateSingleQuestionPackData()
        {
            var teacher = AppManager.Instance.Teacher;

           /* var correctPhrases = teacher.wordAI.SelectData(
                () => teacher.wordHelper.GetPhrasesWithWords(category, drawingNeeded), 
                    new SelectionParameters(SelectionSeverity.AsManyAsPossible, nCorrect, 
                        packListHistory: this.packListHistory, filteringIds:previousPacksIDs)
                );
            previousPacksIDs.AddRange(correctWords.ConvertAll(x => x.GetId()).ToArray());

            var wrongWords = teacher.wordAI.SelectData(
                () => teacher.wordHelper.GetWordsNotIn(correctWords.ToArray()), 
                    new SelectionParameters(SelectionSeverity.AsManyAsPossible, nWrong, ignoreJourney: true)
                );

            var question = firstCorrectIsQuestion ? correctWords[0] : null;
            */

            // Debug
            if (ConfigAI.verboseTeacher)
            {
                //string debugString = "--------- TEACHER: question pack result ---------";
                //debugString += "\nCorrect Words: " + correctWords.Count;
                //foreach (var l in correctWords) debugString += " " + l;
                //debugString += "\nWrong Words: " + wrongWords.Count;
                //foreach (var l in wrongWords) debugString += " " + l;
                //UnityEngine.Debug.Log(debugString);
            }

            return null;// QuestionPackData.Create(null, null, null);
        }

    }
}