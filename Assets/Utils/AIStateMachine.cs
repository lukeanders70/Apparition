using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    private AIState currentState = null;
    private bool currentStateFirstUpdate = false;
    private Dictionary<string, AIState> states = new Dictionary<string, AIState>();
    public void RegisterState(string name, AIState state)
    {
        states[name] = state;
    }

    public void EnterState(string name)
    {
        if(states.ContainsKey(name))
        {
            if(currentState != null)
            {
                currentState.StopState();
            }
            currentState = states[name];
            currentStateFirstUpdate = true;
        } else
        {
            Debug.LogError("tried to start state: " + name + " which does not exist");
        }
    }

    /**
     * use case: damage triggers state change and collision also triggers.
     * a collision that damages might try to cause two different state changes
     * in the same frame. This helps decide which to use independent of the order
     * the events fire
     */
    public void EnterStateLowPriority(string name)
    {
        if (!currentStateFirstUpdate)
        {
            EnterState(name);
        }
    }
    public void Update() { 
        if(currentState != null) {
            if(currentStateFirstUpdate)
            {
                currentStateFirstUpdate = false;
                currentState.StartState();
            }
            currentState.Update(); 
        } 
    }

    public void OnCollision(Collision2D collision) { if (currentState != null) { currentState.OnCollision(collision); } }
}
