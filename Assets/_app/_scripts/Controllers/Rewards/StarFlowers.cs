﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using ModularFramework.Core;
using ModularFramework.Components;

namespace EA4S
{
    public class StarFlowers : MonoBehaviour
    {
        public Image Flower1, Flower2, Flower3, Japan1, Japan2, Bbackground;

        string nextSceneName = string.Empty;

        void Awake() {
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            foreach (Image img in GetComponentsInChildren<Image>()) {
                img.DOFade(0, 0);
            }
        }

        public void Show(int _stars) {
            //if(_stars > 0)
            nextSceneName = AppManager.Instance.MiniGameDone();

            this.gameObject.SetActive(true);
            // Reset zone
            Vector2 f1pos = Flower1.rectTransform.anchoredPosition;
            Flower1.rectTransform.anchoredPosition = new Vector2(f1pos.x, -f1pos.y);
            Vector2 f2pos = Flower2.rectTransform.anchoredPosition;
            Flower2.rectTransform.anchoredPosition = new Vector2(f2pos.x, -f2pos.y);
            Vector2 f3pos = Flower3.rectTransform.anchoredPosition;
            Flower3.rectTransform.anchoredPosition = new Vector2(f3pos.x, -f3pos.y);


            Sequence sequence = DOTween.Sequence();
            TweenParams tParms = new TweenParams()
                .SetEase(Ease.InOutBack);

            sequence.Append(Bbackground.DOFade(1, 0.3f))
                .Insert(0f, Japan1.DOFade(1, 0.3f).SetAs(tParms))
                .Insert(0f, Japan2.DOFade(1, 0.3f).SetAs(tParms));
            //.Insert(0.5f, Japan1.transform.DORotate(new Vector2(360, 0), 15).SetLoops(-1));;

            if (_stars > 0) { 
                sequence.Append(Flower3.DOFade(1, 0.1f));
                sequence.Append(Flower3.rectTransform.DOAnchorPos(f3pos, 0.3f).SetAs(tParms));
            }

            if (_stars > 1) {
                sequence.Append(Flower2.DOFade(1, 0.1f));
                sequence.Append(Flower2.rectTransform.DOAnchorPos(f2pos, 0.3f).SetAs(tParms));
            }

            if (_stars > 2) {
                sequence.Append(Flower1.DOFade(1, 0.1f));
                sequence.Append(Flower1.rectTransform.DOAnchorPos(f1pos, 0.3f).SetAs(tParms));
            }

            //sequence.Append(Japan1.rectTransform.DORotate(new Vector2(360, 0), 15).SetLoops(-1));
            sequence.Play().OnComplete(delegate ()
                {
                    GetComponent<OnClickButtonChangeScene>().enabled = true;
                });

            ContinueScreen.Show(Continue, ContinueScreenMode.Button);
        }

        public void Continue() {
            GameManager.Instance.Modules.SceneModule.LoadSceneWithTransition(nextSceneName);
            nextSceneName = string.Empty;
        }


    }
}