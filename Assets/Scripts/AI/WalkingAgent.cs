using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MLAgents;

public class WalkingAgent : AgentBase
    /* Implements an agent that walks around randomly.
     * This is just to demonstrate attaching an agent class script
     * to a character in our game.
     */ 
{
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

        action[0] = Random.Range(0, 3)-1f;
        action[1] = Random.Range(0, 3)-1f;
        action[2] = 0;
        action[3] = 0;
        return action;
    }

}

