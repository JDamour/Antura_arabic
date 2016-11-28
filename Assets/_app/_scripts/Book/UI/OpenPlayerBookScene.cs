﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EA4S
{
    public class OpenPlayerBookScene : MonoBehaviour, IPointerClickHandler
    {
        void Start()
        {
            Debug.Log("current player avatar is: " + AppManager.I.Player.AvatarId);
            GetComponent<Image>().sprite = AppManager.I.Player.GetAvatar();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            NavigationManager.I.OpenPlayerBook();
        }

    }
}