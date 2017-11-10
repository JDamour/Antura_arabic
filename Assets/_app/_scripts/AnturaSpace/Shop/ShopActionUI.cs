﻿using Antura.AnturaSpace.UI;
using Antura.Audio;
using Antura.Database;
using Antura.Helpers;
using Antura.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Antura.AnturaSpace
{
    public class ShopActionUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Image iconUI;
        public TextMeshProUGUI amountUI;
        public UIButton buttonUI;

        private ShopAction shopAction;

        public void SetAction(ShopAction shopAction)
        {
            this.shopAction = shopAction;
            iconUI.sprite = shopAction.iconSprite;
            amountUI.text = shopAction.bonesCost.ToString();
            UpdateAction();
        }

        public void UpdateAction()
        {
            bool isLocked = shopAction.IsLocked;
            buttonUI.Lock(isLocked);
        }

        public void OnClick()
        {
            if (( ShopDecorationsManager.I.ShopContext == ShopContext.Purchase || shopAction.CanPurchaseAnywhere)
                && shopAction.IsClickButton)
            {
                if (!shopAction.IsLocked)
                {
                    shopAction.PerformAction();
                }
                else
                {
                    ErrorFeedback();
                }
            }
        }

        private int minHeightForDragAction = 50;
        public ScrollRect scrollRect;

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Push the drag action to the scroll rect too
            scrollRect.OnBeginDrag(eventData);

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Push the drag action to the scroll rect too
            scrollRect.OnEndDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Push the drag action to the scroll rect too
            scrollRect.OnDrag(eventData);

            if (ShopDecorationsManager.I.ShopContext == ShopContext.Purchase)
            {
                if (!shopAction.IsLocked)
                {
                    var mousePos = AnturaSpaceUI.I.ScreenToUIPoint(Input.mousePosition);
                    var buttonPos = AnturaSpaceUI.I.WorldToUIPoint(transform.position);
                    if (mousePos.y - buttonPos.y > minHeightForDragAction)
                    {
                        shopAction.PerformDrag();
                    }
                }
                else
                {
                    ErrorFeedback();
                }
            }
        }

        void ErrorFeedback()
        {
            AudioManager.I.PlaySound(Sfx.KO);

            if (shopAction.NotEnoughBones)
            {
                // TODO: change this
                AudioManager.I.PlayDialogue(LocalizationDataId.ReservedArea_SectionDescription_Error);
            }
            else
            {
                AudioManager.I.PlayDialogue(shopAction.errorLocalizationID);
            }
        }

    }
}