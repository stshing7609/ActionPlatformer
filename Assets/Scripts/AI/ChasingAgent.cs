using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MLAgents;

public class ChasingAgent : AgentBase
    /* Implements an agent that chases a target.
     * This is just to demonstrate attaching an agent class script
     * to a character in our game.
     */ 
{
    public float elasticity = 1; // Multiplier on the random walk of the agent.
    public float patrolRadius = 3; // Radius in which to chase player.
    public float airGap = .3f; // Distance to scan for wall.

    public override void ComputeReward()
    {
        float distanceToTarget = Vector2.Distance(
            this.transform.position,
            Target.position);

        SetReward(Mathf.Min(10f, 10f / distanceToTarget));
    }

    public override float[] Heuristic()
    {
        var action = new float[4];
        Vector2 patrolCenter = Target.position;
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        bool pathClear = false;
        Vector2 perturbation = new Vector2(0, 0);
        Vector2 displacement = patrolCenter - currentPosition;
        float distance = Mathf.Abs(displacement.magnitude);
        displacement = displacement / displacement.magnitude;
        float displacementAngle = Mathf.Atan2(displacement.y, displacement.x); // angle between you and the target
        float lambda;
        while (!pathClear)
        {
            float randomTheta = Random.Range(0, 100f) / 100 * 2 * Mathf.PI;
            float delta_theta = Mathf.Abs(randomTheta - displacementAngle) / (2 * Mathf.PI);
            float acceptReject = 1f;
            if (distance < patrolRadius)
            {
                acceptReject = Random.Range(0, 100f) / 100 + .2f;
            }
            if (acceptReject > delta_theta) // Randomly bias movement towards displacement angle if you are within patrol.
            {
                float theta = randomTheta; // lambda * randomTheta + (1 - lambda) * displacementAngle; // Movement is biased towards the displacement angle
                float dx = Mathf.Cos(theta) + displacement.x;// + (1-lambda)*.5f*displacement.x;
                float dy = Mathf.Sin(theta) + displacement.y;
                perturbation = new Vector2(dx, dy);
                Vector2 targetPosition = currentPosition + perturbation;
                RaycastHit2D hit = Physics2D.Raycast(targetPosition, currentPosition, airGap, ~LayerMask.NameToLayer("Player"));
                pathClear = !hit.collider;
            }
        }
        float intensity = 0; // Random.Range(0, 100f) / 100;
        action[0] = Random.Range(0, 3) - 1f + (1 + intensity) * elasticity * Mathf.Min(.5f, 2*distance / patrolRadius) * perturbation.x;
        action[1] = Random.Range(0, 3) - 1f + (1 + intensity) * elasticity * Mathf.Min(.5f, 2*distance / patrolRadius) * perturbation.y;
        action[2] = 0;
        action[3] = 0;

        return action;
    }

}

