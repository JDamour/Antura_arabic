﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;
using ArabicSupport;
using DG.Tweening;
using Google2u;

namespace EA4S
{
    public class WidgetPopupWindow : MonoBehaviour
    {
        public static WidgetPopupWindow I;

        public static bool IsShown { get; private set; }

        [Header("Options")]
        public bool timeIndependent = true;
        [Header("References")]
        public GameObject Window;
        public GameObject TitleGO;
        public GameObject TitleEnglishGO;
        public GameObject DrawingImageGO;
        public GameObject WordTextGO;
        public GameObject ButtonGO;
        public GameObject TutorialImageGO;
        public GameObject MarkOK;
        public GameObject MarkKO;
        public Sprite gameTimeUpSprite;

        bool clicked;
        Action currentCallback;
        Tween showTween;

        void Awake()
        {
            I = this;

            showTween = this.GetComponent<RectTransform>().DOAnchorPosY(-800, 0.5f).From().SetUpdate(timeIndependent)
                .SetEase(Ease.OutBack).SetAutoKill(false).Pause()
                .OnPlay(() => this.gameObject.SetActive(true))
                .OnRewind(() => this.gameObject.SetActive(false));

            this.gameObject.SetActive(false);
        }

        void ResetContents()
        {
            clicked = false;
            TutorialImageGO.SetActive(false);
            SetTitle("");
            SetWord("", "");
            MarkOK.SetActive(false);
            MarkKO.SetActive(false);
        }

        public void Close(bool _immediate = false)
        {
            if (IsShown || _immediate)
                Show(false, _immediate);
        }

        public void Show(bool _doShow, bool _immediate = false)
        {
            GlobalUI.Init();
            clicked = false;

            IsShown = _doShow;
            if (_doShow) {
                if (_immediate)
                    I.showTween.Complete();
                else
                    I.showTween.PlayForward();
            } else {
                if (_immediate)
                    I.showTween.Rewind();
                else
                    I.showTween.PlayBackwards();
            }
        }


        public void ShowTextDirect(Action callback, string myText)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);

            TitleGO.GetComponent<TextMeshProUGUI>().text = myText;
            TitleEnglishGO.GetComponent<TextMeshProUGUI>().text = "";

            Show(true);
        }

        public void ShowArabicTextDirect(Action callback, string myArabicText)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);

            TitleGO.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(myArabicText, false, false);
            TitleEnglishGO.GetComponent<TextMeshProUGUI>().text = "";

            Show(true);
        }

        public void ShowSentence(Action callback, string SentenceId)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);

            LocalizationDataRow row = LocalizationData.Instance.GetRow(SentenceId);
            TitleGO.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(row.GetStringData("Arabic"), false, false);
            TitleEnglishGO.GetComponent<TextMeshProUGUI>().text = row.GetStringData("English");

            AudioManager.I.PlayDialog(SentenceId);

            Show(true);
        }

        public void ShowSentenceWithMark(Action callback, string sentenceId, bool result, Sprite image2show)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);

            MarkOK.SetActive(result);
            MarkKO.SetActive(!result);

            if (image2show != null) {
                TutorialImageGO.GetComponent<Image>().sprite = image2show;
                TutorialImageGO.SetActive(true);
            }

            LocalizationDataRow row = LocalizationData.Instance.GetRow(sentenceId);
            TitleGO.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(row.GetStringData("Arabic"), false, false);
            TitleEnglishGO.GetComponent<TextMeshProUGUI>().text = row.GetStringData("English");

            AudioManager.I.PlayDialog(sentenceId);

            Show(true);
        }

        public void ShowStringAndWord(Action callback, string text, WordData wordData)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);

            TitleGO.GetComponent<TextMeshProUGUI>().text = text;
            TitleEnglishGO.GetComponent<TextMeshProUGUI>().text = "";

            //AudioManager.I.PlayDialog(SentenceId);

            SetWord(wordData.Key, wordData.Word);

            Show(true);
        }

        public void ShowSentenceAndWord(Action callback, string SentenceId, WordData wordData)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);

            LocalizationDataRow row = LocalizationData.Instance.GetRow(SentenceId);
            TitleGO.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(row.GetStringData("Arabic"), false, false);
            TitleEnglishGO.GetComponent<TextMeshProUGUI>().text = row.GetStringData("English");

            //AudioManager.I.PlayDialog(SentenceId);

            SetWord(wordData.Key, wordData.Word);

            Show(true);
        }

        public void ShowSentenceAndWordWithMark(Action callback, string SentenceId, WordData wordData, bool result)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);

            MarkOK.SetActive(result);
            MarkKO.SetActive(!result);

            LocalizationDataRow row = LocalizationData.Instance.GetRow(SentenceId);
            TitleGO.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(row.GetStringData("Arabic"), false, false);
            TitleEnglishGO.GetComponent<TextMeshProUGUI>().text = row.GetStringData("English");

            //AudioManager.I.PlayDialog(SentenceId);

            SetWord(wordData.Key, wordData.Word);

            Show(true);
        }

        public void ShowTimeUp(Action callback)
        {
            ShowSentenceWithMark(callback, "game_time_up", false, gameTimeUpSprite);
        }

    public void Init(string introText, string wordCode, string arabicWord)
        {
            Init(null, introText, wordCode, arabicWord);
        }

        public void ShowTutorial(Action callback, Sprite tutorialImage)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);
            TutorialImageGO.GetComponent<Image>().sprite = tutorialImage;
            TutorialImageGO.SetActive(true);

            AudioManager.I.PlaySfx(Sfx.UIPopup);
            Show(true);
        }

        public void Init(Action callback, string introText, string wordCode, string arabicWord)
        {
            ResetContents();

            currentCallback = callback;
            ButtonGO.SetActive(callback != null);
            TutorialImageGO.SetActive(false);

            SetTitle(introText);
            SetWord(wordCode, arabicWord);
            //            Window.SetActive(true);
        }

        public void SetTitle(string text)
        {
            TitleGO.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(text, false, false);
            TitleEnglishGO.GetComponent<TextMeshProUGUI>().text = text;
        }

        public void SetWord(string wordCode, string arabicWord)
        {
            if (wordCode != "") {
                WordTextGO.SetActive(true);
                DrawingImageGO.SetActive(true);
                // here set both word and drawing 
                WordTextGO.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(arabicWord, false, false);
                DrawingImageGO.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/LivingLetters/Drawings/drawing-" + wordCode);
            } else {
                WordTextGO.SetActive(false);
                DrawingImageGO.SetActive(false);
            }
        }

        public void OnPressButtonPanel()
        {
            //Debug.Log("OnPressButtonPanel() " + clicked);
            OnPressButton();
        }

        public void OnPressButton()
        {
            //Debug.Log("OnPressButton() " + clicked);
            if (clicked)
                return;

            clicked = true;
            AudioManager.I.PlaySfx(Sfx.UIButtonClick);
            currentCallback();
        }
    }
}