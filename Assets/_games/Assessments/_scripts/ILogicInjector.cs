﻿namespace EA4S.Assessment
{
    /// <summary>
    /// This class adds game-specific logic to LivingLetters Objects
    /// </summary>
    public interface ILogicInjector
    {
        void Wire( IQuestion question, IAnswer[] answers);
        void EnableGamePlay();
        bool AllAnswersCorrect();
        void DisableGamePlay();
    }
}