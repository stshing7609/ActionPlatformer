using UnityEngine;
using UnityEditor;

public class States : MonoBehaviour
/* Represents states that the agent can be in.
 * TODO: Make this an FSM?
 */
{
    public float horizontal_tilt = 0;
    public float vertical_tilt = 0;
    public bool jump_button_pressed = false;
    public bool action_button_pressed = false;

}