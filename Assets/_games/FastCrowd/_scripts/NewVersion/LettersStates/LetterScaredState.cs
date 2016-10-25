﻿using UnityEngine;
using System.Collections.Generic;
using System;

namespace EA4S.FastCrowd
{
    public class LetterScaredState : LetterState
    {
        public float ScaredDuration = 1.0f;
        public Vector3 ScareSource;

        const float SCARED_RUN_SPEED = 8.0f;

        float scaredTimer;

        FastCrowdLetterMovement movement;
        Vector3 reenterTarget;
            
        public LetterScaredState(FastCrowdLivingLetter letter) : base(letter)
        {
            movement = letter.GetComponent<FastCrowdLetterMovement>();
        }

        public override void EnterState()
        {
            scaredTimer = ScaredDuration;

            // set letter animation
            letter.gameObject.GetComponent<LetterObjectView>().Model.State = LLAnimationStates.LL_run_fear;
            reenterTarget = letter.walkableArea.GetRandomPosition();
        }

        public override void ExitState()
        {
        }

        public override void Update(float delta)
        {
            // Stay scared if danger is near
            if (Vector3.Distance(letter.transform.position, letter.antura.transform.position) < 20.0f)
            {
                ScaredDuration = 3;
                ScareSource = letter.antura.transform.position;
            }
            else if (Vector3.Distance(letter.transform.position, ScareSource) > 10.0f)
            {
                scaredTimer = Mathf.Min(0.5f, scaredTimer);
            }

            scaredTimer -= delta;

            if (scaredTimer <= 0)
            {
                letter.SetCurrentState(letter.WalkingState);
                return;
            }

            // Run-away from danger!
            Vector3 runDirection = letter.transform.position - ScareSource;
            runDirection.y = 0;
            runDirection.Normalize();

            movement.MoveAmount(runDirection * SCARED_RUN_SPEED * delta);
            movement.LerpLookAt(letter.transform.position + runDirection, 4 * delta);
        }

        public override void UpdatePhysics(float delta)
        {
        }
    }
}
