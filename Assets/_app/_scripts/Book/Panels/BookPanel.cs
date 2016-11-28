﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using EA4S.Db;

namespace EA4S
{

    public struct GenericCategoryData
    {
        public PlayerBookPanel area;
        public string Id;
        public string Title;
        public WordDataCategory wordCategory;
    }

    public class BookPanel : MonoBehaviour
    {

        [Header("Prefabs")]
        public GameObject WordItemPrefab;
        public GameObject LetterItemPrefab;
        public GameObject MinigameItemPrefab;
        public GameObject PhraseItemPrefab;
        public GameObject CategoryItemPrefab;

        [Header("References")]
        public GameObject SubmenuContainer;
        public GameObject ElementsContainer;
        public TextRender ArabicText;
        public TMPro.TextMeshProUGUI Drawing;

        public LetterObjectView LLText;
        public LetterObjectView LLDrawing;

        PlayerBookPanel currentArea = PlayerBookPanel.None;
        GameObject btnGO;
        string currentCategory;
        WordDataCategory currentWordCategory;

        void Start()
        {
        }

        void OnEnable()
        {
            OpenArea(PlayerBookPanel.BookLetters);
        }

        void OpenArea(PlayerBookPanel newArea)
        {
            if (newArea != currentArea) {
                currentArea = newArea;
                activatePanel(currentArea, true);
            }
        }

        void activatePanel(PlayerBookPanel panel, bool status)
        {
            switch (panel) {
                case PlayerBookPanel.BookLetters:
                    AudioManager.I.PlayDialog("Book_Letters");
                    LettersPanel();
                    break;
                case PlayerBookPanel.BookWords:
                    AudioManager.I.PlayDialog("Book_Words");
                    WordsPanel();
                    break;
                case PlayerBookPanel.BookPhrases:
                    AudioManager.I.PlayDialog("Book_Phrases");
                    PhrasesPanel();
                    break;
            }
        }

        void LettersPanel(string _category = "")
        {
            currentCategory = _category;
            List<LetterData> list;
            switch (currentCategory) {
                case "combo":
                    list = AppManager.I.DB.FindLetterData((x) => (x.Kind == LetterDataKind.DiacriticCombo || x.Kind == LetterDataKind.LetterVariation));
                    break;
                case "symbol":
                    list = AppManager.I.DB.FindLetterData((x) => (x.Kind == LetterDataKind.Symbol));
                    break;
                default:
                    list = AppManager.I.DB.GetAllLetterData();
                    break;
            }

            emptyListContainers();
            foreach (LetterData item in list) {
                btnGO = Instantiate(LetterItemPrefab);
                btnGO.transform.SetParent(ElementsContainer.transform, false);
                btnGO.GetComponent<ItemLetter>().Init(this, item);
            }

            //btnGO = Instantiate(CategoryItemPrefab);
            //btnGO.transform.SetParent(SubmenuContainer.transform, false);
            //btnGO.GetComponent<MenuItemCategory>().Init(this, new CategoryData { Id = "all", Title = "All" });

            btnGO = Instantiate(CategoryItemPrefab);
            btnGO.transform.SetParent(SubmenuContainer.transform, false);
            btnGO.GetComponent<MenuItemCategory>().Init(this, new GenericCategoryData { area = PlayerBookPanel.BookLetters, Id = "letter", Title = "Letters" });

            btnGO = Instantiate(CategoryItemPrefab);
            btnGO.transform.SetParent(SubmenuContainer.transform, false);
            btnGO.GetComponent<MenuItemCategory>().Init(this, new GenericCategoryData { area = PlayerBookPanel.BookLetters, Id = "symbol", Title = "Symbols" });

            btnGO = Instantiate(CategoryItemPrefab);
            btnGO.transform.SetParent(SubmenuContainer.transform, false);
            btnGO.GetComponent<MenuItemCategory>().Init(this, new GenericCategoryData { area = PlayerBookPanel.BookLetters, Id = "combo", Title = "Combinations" });

        }

        void WordsPanel(WordDataCategory _category = WordDataCategory.None)
        {
            currentWordCategory = _category;
            List<WordData> list;
            switch (currentWordCategory) {

                case WordDataCategory.None:
                    //list = AppManager.I.DB.GetAllWordData();
                    list = new List<WordData>();
                    break;
                default:
                    list = AppManager.I.DB.FindWordDataByCategory(currentWordCategory);
                    break;
            }
            emptyListContainers();

            foreach (WordData item in list) {
                btnGO = Instantiate(WordItemPrefab);
                btnGO.transform.SetParent(ElementsContainer.transform, false);
                btnGO.GetComponent<ItemWord>().Init(this, item);
            }
            Drawing.text = "";

            //btnGO = Instantiate(CategoryItemPrefab);
            //btnGO.transform.SetParent(SubmenuContainer.transform, false);
            //btnGO.GetComponent<MenuItemCategory>().Init(this, new GenericCategoryData { Id = WordDataCategory.None.ToString(), Title = "All" });

            foreach (WordDataCategory cat in GenericUtilities.SortEnums<WordDataCategory>()) {
                btnGO = Instantiate(CategoryItemPrefab);
                btnGO.transform.SetParent(SubmenuContainer.transform, false);
                btnGO.GetComponent<MenuItemCategory>().Init(this, new GenericCategoryData { area = PlayerBookPanel.BookWords, wordCategory = cat, Title = cat.ToString() });
            }

        }

        void PhrasesPanel()
        {
            emptyListContainers();

            foreach (PhraseData item in AppManager.I.DB.GetAllPhraseData()) {
                btnGO = Instantiate(PhraseItemPrefab);
                btnGO.transform.SetParent(ElementsContainer.transform, false);
                btnGO.GetComponent<ItemPhrase>().Init(this, item);
            }
        }


        public void SelectSubCategory(GenericCategoryData _category)
        {
            switch (_category.area) {
                case PlayerBookPanel.BookLetters:
                    LettersPanel(_category.Id);
                    break;
                case PlayerBookPanel.BookWords:
                    WordsPanel(_category.wordCategory);
                    break;
            }
        }

        public void DetailWord(WordData word)
        {
            Debug.Log("Detail Word :" + word.Id);
            AudioManager.I.PlayWord(word.Id);

            var output = "";

            var splittedLetters = ArabicAlphabetHelper.SplitWordIntoLetters(word.Arabic);
            foreach (var letter in splittedLetters) {
                output += letter.GetChar() + " ";
            }
            output += "\n";
            output += word.Arabic;

            output += "\n";

            foreach (var letter in splittedLetters) {
                output += letter.GetChar();
            }

            ArabicText.text = output;

            LLText.Init(new LL_WordData(word));

            if (word.Drawing != "") {
                var drawingChar = AppManager.I.Teacher.wordHelper.GetWordDrawing(word);
                Drawing.text = drawingChar;
                LLDrawing.Init(new LL_ImageData(word));
                Debug.Log("Drawing: " + word.Drawing + " / " + ArabicAlphabetHelper.GetLetterFromUnicode(word.Drawing));
            } else {
                Drawing.text = "";
                LLDrawing.Init(new LL_ImageData(word));
            }
        }

        void ResetLL()
        {

        }

        public void DetailLetter(LetterData letter)
        {
            Debug.Log("Detail Letter :" + letter.Id);
            AudioManager.I.PlayLetter(letter.Id);

            ArabicText.text = letter.GetChar(LetterPosition.Isolated);
            ArabicText.text += " " + letter.GetChar(LetterPosition.Final);
            ArabicText.text += " " + letter.GetChar(LetterPosition.Medial);
            ArabicText.text += " " + letter.GetChar(LetterPosition.Initial);

            LLText.Init(new LL_LetterData(letter));
        }

        public void DetailPhrase(PhraseData phrase)
        {
            Debug.Log("Detail Phrase :" + phrase.Id);
            AudioManager.I.PlayPhrase(phrase.Id);
        }


        void emptyListContainers()
        {
            foreach (Transform t in ElementsContainer.transform) {
                Destroy(t.gameObject);
            }
            foreach (Transform t in SubmenuContainer.transform) {
                Destroy(t.gameObject);
            }
        }

        public void BtnOpenLetters()
        {
            OpenArea(PlayerBookPanel.BookLetters);
        }

        public void BtnOpenWords()
        {
            OpenArea(PlayerBookPanel.BookWords);
        }

        public void BtnOpenPhrases()
        {
            OpenArea(PlayerBookPanel.BookPhrases);
        }

    }
}