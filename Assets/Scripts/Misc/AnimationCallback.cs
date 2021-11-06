using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : StateMachineBehaviour
{
    public List<StateCallback> enterCallbacks = new List<StateCallback>();
    public List<StateCallback> exitCallbacks = new List<StateCallback>();
    public Transitions.TransitionState transitionState;

    public delegate void StateCallback(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach(StateCallback callback in enterCallbacks)
        {
            callback(animator, stateInfo, layerIndex);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (StateCallback callback in exitCallbacks)
        {
            callback(animator, stateInfo, layerIndex);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
