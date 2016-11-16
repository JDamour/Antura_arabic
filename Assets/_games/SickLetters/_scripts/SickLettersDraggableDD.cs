﻿using UnityEngine;
using System.Collections;
//using ArabicSupport;
using TMPro;
using System;

namespace EA4S.SickLetters
{
	public class SickLettersDraggableDD : MonoBehaviour {

		private Vector3 screenPoint;
		private Vector3 offset;

        public SickLettersGame game;
		public bool isDot, deattached;
		[Range (0, 3)] public int dots;

		public Diacritic diacritic;

		public Vector3 fingerOffset;
		public TextMeshPro draggableText;

        public bool isCorrect;
        public bool isNeeded = false, isInVase = false, isTouchingVase = false;

        public bool isDragging = false;

        bool isLeftOver = true;
		bool overDestinationMarker = false;
		bool overPlayermarker = false;
        bool shake = false;
        bool release = false;

        [HideInInspector]
        public Rigidbody thisRigidBody;
        [HideInInspector]
        public BoxCollider boxCollider;
        Transform origParent;
        Vector3 correctStartPos, origPosition, origLocalPosition, origRotation, origLocalRotation, origBoxColliderSize, origBoxColliderCenter;

        float startX;
        float startY;
        float startZ;

        void Start()
        {
            thisRigidBody = GetComponent<Rigidbody>();
            boxCollider = GetComponent<BoxCollider>();
            startX = transform.position.x;
            startY = transform.position.y;
            startZ = transform.position.z;
            //Reset();
        }

        void OnMouseDown()
		{
            if (game.disableInput)
                return;

            release = false;
            isDragging = true;

			screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            origParent = transform.parent;
            origRotation = transform.eulerAngles;
            origPosition = transform.position;
            origLocalRotation = transform.localEulerAngles;
            origLocalPosition = transform.localPosition;
            origBoxColliderSize = boxCollider.bounds.size;
            origBoxColliderCenter = boxCollider.bounds.center;

            transform.parent = null;

            if (isCorrect)
            {
                game.wrongDraggCount++;
                shake = true;
                correctStartPos = draggableText.transform.localPosition;
                draggableText.transform.parent = transform;
            }

            else
                offset = gameObject.transform.position - 
				    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

		}

		void OnMouseDrag()
		{
            if (game.disableInput)
                releaseDD();

            if (release)
                return;

            game.tut.repeatConter = game.tut.repeatMax;

            //transform.eulerAngles = origRotation;
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = new Vector3 (curPosition.x + fingerOffset.x, curPosition.y + fingerOffset.y, curPosition.z+ fingerOffset.z);

		}

		void OnMouseUp()
		{
            releaseDD();
        }

        public void releaseDD()
        {
            release = true;
            isDragging = shake = false;
            
            if (overPlayermarker)//pointer Still over LL
            {
                if (isCorrect)
                {
                    resetCorrectDD();
                }
                else
                {
                    resetWronDD();
                }
            }
            else //pointer isn't over LL
            {
                transform.parent = null;
                //if (!isTouchingVase)
                {
                    thisRigidBody.isKinematic = false;
                    thisRigidBody.useGravity = true;
                    boxCollider.isTrigger = false;
                }

                boxCollider.center = Vector3.zero;
                boxCollider.size = new Vector3(0.1f, 0.25f, 0.1f);

                isLeftOver = false;

                //if (isTouchingVase)
                  //  game.scale.addNewDDToVas(this);
                
            //if (game.scale.vaseCollider.bounds.Contains(transform.position))
                    
            }

            overPlayermarker = false;
            overDestinationMarker = false;
        }

		public void Reset()
		{
            transform.parent = origParent;

            if(transform.parent)
                transform.localPosition = new Vector3(startX, startY, startZ);
            else
                transform.position = new Vector3(startX, startY, startZ);

            isDragging = false;

			gameObject.SetActive(true);
		}

        public void setInitPos(Vector3 initPos)
        {
            startX = initPos.x;
            startY = initPos.y;
            startZ = initPos.z;
        }

        public void resetWronDD() {
            transform.parent = origParent;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = origLocalRotation;
            isTouchingVase = false;
            thisRigidBody.isKinematic = true;
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(0.6f, 3.89f, 0.6f);
            boxCollider.center = Vector3.zero + Vector3.up * -1.62f;
        }

        IEnumerator Coroutine_ScaleOverTime(float time)
		{
			Vector3 originalScale = transform.localScale;
			Vector3 destinationScale = new Vector3(4.0f, 1.0f, 4.0f);

			float currentTime = 0.0f;
			do
			{
				transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
				currentTime += Time.deltaTime;
				yield return null;
			} while (currentTime <= time);
		}
			
		void Update() {
  
            if (shake && game.wrongDraggCount <= 1)
                shakeTransform(game.scale.transform, 20, 10, game.scale.vaseStartPose);
        }

        void Dance()
		{
			transform.position = new Vector3(
				startX + Mathf.PerlinNoise(Time.time, startX) * 3 + 1, 
				startY + Mathf.PerlinNoise(Time.time, startY) * 3 + 1, 
				startZ + Mathf.PerlinNoise(Time.time, startZ) * 3 + 1);
		}

        void Setmarker(Collider other, bool markerStatus)
        {
            if (other.tag == "Player")
                overPlayermarker = markerStatus;
		}

        public void resetCorrectDD()
        {
            transform.parent = origParent;
            thisRigidBody.isKinematic = true;
            thisRigidBody.useGravity = false;

            draggableText.transform.parent = origParent;
            draggableText.transform.localPosition = new Vector3(-0.5f, 0.5f,0);
            draggableText.transform.localEulerAngles = new Vector3(90, 0.0f, 90);

            boxCollider.size = new Vector3(1,1,1.21f);
            boxCollider.isTrigger = true;

            isTouchingVase = false;
        }


        void OnTriggerEnter(Collider other)
		{
			Setmarker(other, true);
        }

		void OnTriggerStay(Collider other)
		{
			Setmarker(other, true);
		}

		void OnTriggerExit(Collider other)
		{
			Setmarker(other, false);
		}

        void OnCollisionEnter(Collision coll)
        {
            if (coll.gameObject.tag == "Obstacle")
                poofOnCollision(coll);
            else if (!isDragging &&!isInVase && coll.gameObject.tag == "Finish")
            {
                game.scale.addNewDDToVas(this);
                thisRigidBody.isKinematic = true;
                
            }
        }
        void OnCollisionStay(Collision coll)
        {
            if(deattached)
                poofOnCollision(coll);
        }

        void poofOnCollision(Collision coll)
        {
            if (coll.gameObject.tag == "Obstacle")
            {
                game.Poof(transform.position);

                if (game.roundsCount == 0 && !isInVase)
                {
                    resetWronDD();
                    game.tut.doTutorial();
                    return;
                }

                if (!isInVase)
                {
                    game.onWrongMove();
                }

                if (isCorrect)
                {
                    StartCoroutine(game.scale.onDroppingCorrectDD());
                    resetCorrectDD();
                }
                else
                {
                    if (!deattached)
                    {
                        deattached = true;
                        game.checkForNextRound();
                    }

                    Destroy(gameObject, 0.0f);
                }
            }
        }

        void checkDDtoVaseCollision(Collision coll)
        {
            Debug.Log(coll.gameObject.name.ToLower());
            if (coll.gameObject.name.ToLower().Contains("vase"))
                releaseDD();
        }

        void shakeTransform(Transform t, float speed, float amount, Vector2 startPose)
        {
            t.position = new Vector3(startPose.x + Mathf.Sin(Time.time * 20f) / 10, t.position.y, t.position.z);

        }

        
    }

}
