﻿using System.Collections.Generic;
using DG.Tweening;
using EA4S.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EA4S.UI
{
    /// <summary>
    /// Manages the activation/deactivation of UIButtons on certain conditions
    /// </summary>
    public static class UIDirector
    {
        static bool initialized;
        static readonly List<UIButton> allActiveUIButtons = new List<UIButton>();

        public static void Init()
        {
            if (initialized) return;

            initialized = true;
            AppManager.Instance.NavigationManager.OnSceneStartTransition += OnSceneStartTransition;
        }

        #region Public Methods

        public static void Add(UIButton button)
        {
            if (!allActiveUIButtons.Contains(button)) allActiveUIButtons.Add(button);
        }

        public static void Remove(UIButton button)
        {
            allActiveUIButtons.Remove(button);
        }

        #endregion

        #region Methods

        static void DeactivateAllUI()
        {
            foreach (UIButton bt in allActiveUIButtons) bt.Bt.interactable = false;
            // NOTE: would be nicer and faster to just set EventSystem.current.enabled to FALSE, but there's non-UI elements that rely on it apparently,
            // and will throw a NullReferenceException if it's disabled (for example the MAP)
//            EventSystem.current.enabled = false;
        }

        #endregion

        #region Callbacks

        static void OnSceneStartTransition()
        {
            DeactivateAllUI();
        }

        #endregion
    }
}