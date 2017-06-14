﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.DeExtensions;
using DG.Tweening;
using EA4S.Audio;
using EA4S.Core;
using EA4S.LivingLetters;
using EA4S.Profile;
using UnityEngine;
using EA4S.Scenes;

namespace EA4S.UI
{
    /// <summary>
    /// General controller for the interface of the Profile Selector.
    /// Used in the Home (_Start) scene.
    /// </summary>
    public class ProfileSelectorUI : MonoBehaviour
    {
        [Header("References")]
        public UIButton BtAdd;
        public UIButton BtPlay;
        public GameObject ProfilesPanel;
        [Header("Audio")]
        public Sfx SfxOpenCreateProfile;
        public Sfx SfxCreateNewProfile;
        public Sfx SfxSelectProfile;

        public PlayerProfileManager ProfileManager { get { return AppManager.Instance.PlayerProfileManager; } }
        int maxProfiles;
        List<PlayerIconData> playerIconDatas;
        PlayerIcon[] playerIcons;
        Tween btAddTween, btPlayTween;

        public LetterObjectView LLObjectView; // ?

        #region Unity

        void Awake()
        {
            playerIcons = ProfilesPanel.GetComponentsInChildren<PlayerIcon>(true);
            maxProfiles = playerIcons.Length;
        }

        void Start()
        {
            // By default, the letter shows a truly random letter
            LLObjectView.Initialize(AppManager.Instance.Teacher.GetRandomTestLetterLL(useMaxJourneyData: true));
            //LLObjectView.Initialize(new EA4S.MinigamesAPI.LL_LetterData("alef_fathah_tanwin"));

            Setup();

            btAddTween = BtAdd.transform.DORotate(new Vector3(0, 0, -45), 0.3f).SetAutoKill(false).Pause()
                .SetEase(Ease.OutBack)
                .OnRewind(() => {
                    if (AppManager.Instance.AppSettings.SavedPlayers == null || AppManager.Instance.AppSettings.SavedPlayers.Count == 0) BtAdd.Pulse();
                });
            btPlayTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(BtPlay.RectT.DOAnchorPosY(-210, 0.2f).From(true))
                .OnPlay(() => BtPlay.gameObject.SetActive(true))
                .OnRewind(() => BtPlay.gameObject.SetActive(false))
                .OnComplete(() => BtPlay.Pulse());

            BtPlay.gameObject.SetActive(false);

            // Listeners
            BtAdd.Bt.onClick.AddListener(() => OnClick(BtAdd));
            BtPlay.Bt.onClick.AddListener(() => {
                AudioManager.I.PlaySound(Sfx.UIButtonClick);
                HomeScene.I.Play();
            });
            foreach (PlayerIcon pIcon in playerIcons) {
                PlayerIcon p = pIcon;
                p.UIButton.Bt.onClick.AddListener(() => OnSelectProfile(p));
            }
        }

        void OnDestroy()
        {
            btAddTween.Kill();
            btPlayTween.Kill();
            BtAdd.Bt.onClick.RemoveAllListeners();
            foreach (PlayerIcon pIcon in playerIcons) pIcon.UIButton.Bt.onClick.RemoveAllListeners();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects the profile.
        /// </summary>
        internal void SelectProfile(PlayerIconData playerIconData)
        {
            ProfileManager.SetPlayerAsCurrentByUUID(playerIconData.Uuid);
            AudioManager.I.PlaySound(SfxSelectProfile);
            LLObjectView.Initialize(AppManager.Instance.Teacher.GetRandomTestLetterLL(useMaxJourneyData: true));
            Setup();
        }

        #endregion

        #region Methods

        // Layout with current profiles
        void Setup()
        {
            ActivatePlayerIcons(true);
            if (playerIconDatas == null) playerIconDatas = ProfileManager.GetSavedPlayers();
            int totProfiles = playerIconDatas == null ? 0 : playerIconDatas.Count;
            int len = playerIcons.Length;
            for (int i = 0; i < len; ++i) {
                PlayerIcon playerIcon = playerIcons[i];
                if (i >= totProfiles) playerIcon.gameObject.SetActive(false);
                else {
                    PlayerIconData data = playerIconDatas[i];
                    playerIcon.gameObject.SetActive(true);
                    playerIcon.Init(data);
                    playerIcon.Select(AppManager.Instance.Player.Uuid);
                }
            }

            if (totProfiles == 0) {
                BtAdd.Pulse();
                BtPlay.StopPulsing();
                btPlayTween.PlayBackwards();
            } else {
                // Set play button position
                this.StartCoroutine(CO_SetupPlayButton());
            }
            if (totProfiles >= maxProfiles) {
                btAddTween.Rewind();
                BtAdd.gameObject.SetActive(false);
            }
        }

        // Used to set play button position after one frame, so grid is set correctly
        IEnumerator CO_SetupPlayButton()
        {
            yield return null;

            BtPlay.gameObject.SetActive(true);
            // PLAYER REFACTORING WITH UUID
            PlayerIcon activePlayerIcon = GetPlayerIconByUUID(AppManager.Instance.Player.Uuid);
            if (activePlayerIcon != null) BtPlay.RectT.SetAnchoredPosX(activePlayerIcon.UIButton.RectT.anchoredPosition.x);
            btPlayTween.PlayForward();
        }

        void ActivatePlayerIcons(bool _activate)
        {
            foreach (PlayerIcon pIcon in playerIcons) pIcon.UIButton.Bt.interactable = _activate;
        }

        PlayerIcon GetPlayerIconByUUID(string uuid)
        {
            foreach (PlayerIcon pIcon in playerIcons) {
                if (pIcon.Uuid == uuid) return pIcon;
            }
            return null;
        }

        #endregion

        #region Callbacks

        void OnClick(UIButton _bt)
        {
            if (_bt == BtAdd) {
                // Bt Add
                _bt.StopPulsing();
                AppManager.Instance.NavigationManager.GoToPlayerCreation();
            }
        }

        void OnSelectProfile(PlayerIcon playerIcon)
        {
            int index = Array.IndexOf(playerIcons, playerIcon);
            PlayerIconData playerData = playerIconDatas[index];
            SelectProfile(playerData);
        }

        #endregion
    }
}