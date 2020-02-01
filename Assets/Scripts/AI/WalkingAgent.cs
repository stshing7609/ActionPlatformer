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

    private void Start()
    {
        states = GetComponent<States>();
        rBody = GetComponent<Rigidbody2D>();
    }

    public Transform Target; // Stores the GameObject's position, orientation, and scale.

    public override void AgentReset()
    {
        Target.position = initial_position;
    }

    public override void CollectObservations()
    {

    }

    public float speed = 3;
    public override void AgentAction(float[] vectorAction)
    {
        print(vectorAction);
        states.horizontal_tilt = vectorAction[0];
        states.vertical_tilt = vectorAction[1];
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
        action[0] = 1;  Random.Range(-1,1);
        action[1] = 0;
        action[2] = 0;//Random.Range(0,1);
        action[3] = 0;

        return action;
    }

}

