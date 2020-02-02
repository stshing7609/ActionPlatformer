using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MLAgents;

public class PatrollingJumpingAgent : JumpingAgent
    /* Implements an agent that walks around randomly.
     * This is just to demonstrate attaching an agent class script
     * to a character in our game.
     */ 
{
    public float elasticity = 1;
    public override void ComputeReward()
    {
        float distanceToTarget = Vector2.Distance(
            this.transform.position,
            Target.position);

        SetReward(Mathf.Min(10f, 10f / distanceToTarget));
    }

    public override float[] Heuristic()
    {
        Vector2 patrolCenter = initial_position;
        var action = new float[4];
        Vector2 currentPosition;
        currentPosition.x = transform.position.x;
        currentPosition.y = transform.position.y;
        Vector2 delta = currentPosition - patrolCenter;
        float intensity = Random.Range(0, 100f) / 100;

        action[0] = Random.Range(0, 3)-1f - (1+intensity) * elasticity * delta.x;
        action[1] = Random.Range(0, 3)-1f - (1+intensity) * elasticity * delta.y;
        action[2] = Random.Range(0, 10f) / 10 - .1f;
        action[3] = Random.Range(0, 10f) / 10 - .1f;
        return action;
    }

}

