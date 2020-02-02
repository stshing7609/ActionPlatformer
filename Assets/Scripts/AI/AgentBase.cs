using UnityEngine;
using MLAgents;

public abstract class AgentBase : Agent
{

    private States states; // This stores the GameObject's action state.
    private Rigidbody2D rBody; // We need a reference to this to perform kinematic operations on the GameObject.
    public Vector2 initial_position = new Vector2(0, 0);
    public float moveInterval = 30;
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

    public abstract void ComputeReward();
    public override void AgentAction(float[] vectorAction)
    {

        ComputeReward();
        states.horizontal_tilt = vectorAction[0];
        states.vertical_tilt = vectorAction[1];
        if (vectorAction[2] > .5)
        {
            states.jump_button_pressed = true;
        }
        else
        {
            states.jump_button_pressed = false;
        }
        if (vectorAction[3] > .5)
        {
            states.action_button_pressed = true;
        }
        else
        {
            states.action_button_pressed = false;
        }
    }
    public void Update()
    /* For some reason, the AgentAction method isn't being called automatically
     * like it should be. I don't know why this is, but I temporarily added an Update call
     * to simulate that behavior.
     */
    {
        if (framecount % moveInterval == 0)
        {
            lastAction = Heuristic();
        }
        AgentAction(lastAction);
        framecount++;
    }
}