﻿using System;
using UnityEngine;

namespace EA4S.Db
{
    [Serializable]
    public class PhraseData : IData, IConvertibleToLivingLetterData
    {
        public string Id;
        public string English;
        public string Arabic;
        public PhraseDataCategory Category;
        public string Linked;
        public string[] Words; // TODO @Michele : parse list of words tht are in the phrase

        public override string ToString()
        {
            return Id + ": " + English;
        }

        public string GetId()
        {
            return Id;
        }

        public ILivingLetterData ConvertToLivingLetterData()
        {
            throw new NotImplementedException("PhraseData should be convertible to its LL_data counterpart, which does not exist yet.");
           // return new LL_PhraseData(GetId(), this);
        }
    }
}