using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MLAgents;

public class WalkingAgent : Agent
    /* Implements an agent that walks around randomly.
     * This is just to demonstrate attaching an agent class script
     * to a character in our game.
     */ 
{
    private States states; // This stores the GameObject's action state.
    private Rigidbody2D rBody; // We need a reference to this to perform kinematic operations on the GameObject.
    public Vector2 initial_position = new Vector2(0,0);
    private float[] lastAction;
    private int framecount = 0;
    private void Start()
    {
        states = GetComponent<States>();
        rBody = GetComponent<Rigidbody2D>();
    }

    public Transform Target; // Stores the GameObject's position, orientation, and scale.

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        AgentReset();
    }
    public override void AgentReset()
    {
        Target.position = initial_position;
    }

    public override void CollectObservations()
    {
        print("hiii");
        // Target and Agent positions
        AddVectorObs(Target.position);
        AddVectorObs(this.transform.position);

        // Agent Velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.y);
    }

    public void ComputeReward()
    {
        float distanceToTarget = Vector2.Distance(
            this.transform.position,
            Target.position);

        SetReward(Mathf.Min(10f, 10f / distanceToTarget));
    }

    public float speed = 3;
    public override void AgentAction(float[] vectorAction)
    {

        ComputeReward();
        states.horizontal_tilt = vectorAction[0];
        states.vertical_tilt = vectorAction[1];
        print(states.horizontal_tilt);
        if (vectorAction[2] > .5)
        {
            states.jump_button_pressed = true;
        } else
        {
            states.jump_button_pressed = false;
        }
        if (vectorAction[3] > .5)
        {
            states.action_button_pressed = true;
        } else 
        {
            states.action_button_pressed = false;
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[4];

        action[0] = Random.Range(0, 3)-1f;
        action[1] = 3;
        action[2] = Random.Range(0, 3)-1f;
        action[3] = 0;
        return action;
    }

    public void Update()
    {
        if (framecount % 20 == 0) {
            lastAction = Heuristic();
        } 
        AgentAction(lastAction);
        framecount++;
    }
}

