﻿using UnityEngine;
using System.Collections;

namespace EA4S.ThrowBalls
{
    public class CratePileController : MonoBehaviour
    {
        public CrateController bottomCrate;
        public CrateController middleCrate;
        public CrateController topCrate;

        public LetterController letter;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnSwerveUpdate(CrateController crate, float rotateByAngle, Vector3 rotationPivot, Vector3 zVector)
        {
            if (crate == topCrate)
            {
                letter.transform.RotateAround(rotationPivot, zVector, rotateByAngle);
            }
        }

        public void OnCrateHit(CrateController crate)
        {
            crate.Launch(new Vector3(0, 0, 1), 40);
            crate.VanishAfterDelay(0.15f);

            if (crate != middleCrate)
            {
                middleCrate.ApplyCustomGravity();
                middleCrate.SetIsKinematic(false);
                middleCrate.VanishAfterDelay(1.5f);
                middleCrate.StopSwerving();
            }

            if (crate != topCrate)
            {
                topCrate.ApplyCustomGravity();
                topCrate.SetIsKinematic(false);
                topCrate.VanishAfterDelay(1.5f);
                topCrate.StopSwerving();
            }

            if (crate != bottomCrate)
            {
                bottomCrate.ApplyCustomGravity();
                bottomCrate.SetIsKinematic(false);
                bottomCrate.VanishAfterDelay(1.5f);
                bottomCrate.StopSwerving();
            }

            letter.ApplyCustomGravity();
            
            PokeballController.instance.Reset();
        }

        public void SetSwerving()
        {
            Vector3 pivot = bottomCrate.transform.position;

            Vector3 leftPivot = bottomCrate.transform.position;
            leftPivot.x += -2.2f;
            leftPivot.y += -2.2f;

            Vector3 rightPivot = bottomCrate.transform.position;
            rightPivot.x += 2.2f;
            rightPivot.y += -2.2f;

            topCrate.SetSwerving(leftPivot, rightPivot, 1.5f);
            middleCrate.SetSwerving(leftPivot, rightPivot, 1.25f);
            bottomCrate.SetSwerving(leftPivot, rightPivot, 1f);
        }

        public void Reset()
        {
            bottomCrate.Reset();
            middleCrate.Reset();
            topCrate.Reset();

            Vector3 letterPos = letter.transform.position;

            bottomCrate.transform.position = new Vector3(letterPos.x, letterPos.y - 11.0462f, letterPos.z);
            middleCrate.transform.position = new Vector3(letterPos.x, letterPos.y - 6.646f, letterPos.z);
            topCrate.transform.position = new Vector3(letterPos.x, letterPos.y - 2.27622f, letterPos.z);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);

            bottomCrate.Enable();
            middleCrate.Enable();
            topCrate.Enable();
        }
    }
}