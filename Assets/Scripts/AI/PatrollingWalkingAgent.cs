using UnityEngine;
using UnityEditor;

public class PatrollingWalkingAgent : WalkingAgent
{
    public float elasticity = 1;
    public override float[] Heuristic()
    {
        var action = new float[4];
        Vector2 patrolCenter = initial_position;
        Vector2 currentPosition;
        currentPosition.x = transform.position.x;
        currentPosition.y = transform.position.y;
        Vector2 delta = currentPosition - patrolCenter;
        float intensity = Random.Range(0, 100f) / 100;
        action[0] = Random.Range(0, 3) - 1f - (1+intensity) * elasticity * delta.x;
        action[1] = Random.Range(0, 3) - 1f - (1+intensity) * elasticity * delta.y;
        action[2] = 0;
        action[3] = 0;
        return action;
    }

}