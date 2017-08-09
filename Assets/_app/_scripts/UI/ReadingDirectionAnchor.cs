﻿using Antura.Core;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.UI
{
    /// <summary>
    /// Changes anchor based on reading direction
    /// </summary>
    public class ReadingDirectionAnchor : MonoBehaviour
    {
        void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();

            switch (AppConstants.ReadingDirection)
            {
                case ReadingDirection.LeftToRight:
                    // Default
                    break;
                case ReadingDirection.RightToLeft:
                    rectTransform.anchorMin = new Vector2(Mathf.Abs(1 - rectTransform.anchorMin.x), rectTransform.anchorMin.y);
                    rectTransform.anchorMax = new Vector2(Mathf.Abs(1 - rectTransform.anchorMax.x), rectTransform.anchorMax.y);
                    rectTransform.SetAnchoredPosX(- rectTransform.anchoredPosition.x);
                    break;
            }
        }
    }
}