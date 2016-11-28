﻿using UnityEngine;
using System.Collections;
using EA4S;
using DG.Tweening;
namespace EA4S
{
    public class StageManager : MonoBehaviour
    {
        public GameObject[] stages;
        public GameObject[] cameras;
        public GameObject[] miniMaps;
        public GameObject letter;
        public int s, i;
        void Awake()
        {
            /*AppManager.I.Player.MaxJourneyPosition.Stage = 2;
            AppManager.I.Player.MaxJourneyPosition.LearningBlock = 3;
            AppManager.I.Player.MaxJourneyPosition.PlaySession = 1;*/

            s = AppManager.I.Player.MaxJourneyPosition.Stage;
            for (i = 1; i <= (s - 1); i++)
            {
                miniMaps[i].GetComponent<MiniMap>().isAvailableTheWholeMap = true;
                miniMaps[i].GetComponent<MiniMap>().CalculateSettingsStageMap();
            }
            miniMaps[i].GetComponent<MiniMap>().CalculateSettingsStageMap();


            //ChangeCamera(cameras[AppManager.I.Player.CurrentJourneyPosition.Stage]);
            stages[AppManager.I.Player.CurrentJourneyPosition.Stage].SetActive(true);
            letter.GetComponent<LetterMovement>().miniMapScript = miniMaps[AppManager.I.Player.CurrentJourneyPosition.Stage].GetComponent<MiniMap>();

            StartCoroutine("ResetPosLetter");
        }
        public void StageLeft()
        {
            int numberStage = AppManager.I.Player.CurrentJourneyPosition.Stage;
            if (numberStage < s)
            {
                stages[numberStage].SetActive(false);
                stages[numberStage + 1].SetActive(true);
                ChangeCamera(cameras[numberStage + 1]);

                ChangePinDotToBlack();
                AppManager.I.Player.CurrentJourneyPosition.Stage++;
                letter.GetComponent<LetterMovement>().miniMapScript = miniMaps[numberStage + 1].GetComponent<MiniMap>();
                letter.GetComponent<LetterMovement>().ResetPosLetterAfterChangeStage();

            }
        }
        public void StageRight()
        {
            int numberStage = AppManager.I.Player.CurrentJourneyPosition.Stage;
            if (numberStage > 1)
            {
                stages[numberStage].SetActive(false);
                stages[numberStage - 1].SetActive(true);
                ChangeCamera(cameras[numberStage - 1]);

                ChangePinDotToBlack();
                AppManager.I.Player.CurrentJourneyPosition.Stage--;
                letter.GetComponent<LetterMovement>().miniMapScript = miniMaps[numberStage - 1].GetComponent<MiniMap>();
                letter.GetComponent<LetterMovement>().ResetPosLetterAfterChangeStage();
            }
        }
        public void ChangeCamera(GameObject ZoomCameraGO)
        {

            CameraGameplayController.I.MoveToPosition(ZoomCameraGO.transform.position, ZoomCameraGO.transform.rotation);

        }
        IEnumerator ResetPosLetter()
        {
            yield return new WaitForSeconds(0.2f);
            letter.GetComponent<LetterMovement>().ResetPosLetter();
            letter.SetActive(true);
            CameraGameplayController.I.transform.position = cameras[AppManager.I.Player.CurrentJourneyPosition.Stage].transform.position;
        }
        void ChangePinDotToBlack()
        {
            if (AppManager.I.Player.CurrentJourneyPosition.PlaySession == 100)//change color pin to black
            {
                letter.GetComponent<LetterMovement>().miniMapScript.posPines[AppManager.I.Player.CurrentJourneyPosition.LearningBlock].GetComponent<MapPin>().ChangeMaterialPinToBlack();
                letter.GetComponent<LetterMovement>().miniMapScript.posPines[AppManager.I.Player.CurrentJourneyPosition.LearningBlock].transform.GetChild(0).gameObject.SetActive(true); //activate pin
            }
            else
                letter.GetComponent<LetterMovement>().ChangeMaterialDotToBlack(letter.GetComponent<LetterMovement>().miniMapScript.posDots[letter.GetComponent<LetterMovement>().pos]);
        }
    }
}