using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Dictionary<Transitions.TransitionState, AnimationCallback> transitionStates = new Dictionary<Transitions.TransitionState, AnimationCallback>();


    // Start is called before the first frame update
    void Start()
    {
        var states = animator.GetBehaviours<AnimationCallback>();
        foreach(AnimationCallback state in states)
        {
            transitionStates.Add(state.transitionState, state);
        }
        transitionStates[Transitions.TransitionState.Load]?.enterCallbacks.Add((Animator a, AnimatorStateInfo stateInfo, int layerIndex) => {
            a.SetTrigger("End");
        });
    }
    public void AddLoadingCallback(AnimationCallback.StateCallback callback)
    {
        transitionStates[Transitions.TransitionState.Load]?.enterCallbacks.Insert(0, callback);
    }

    public void StartTransition()
    {
        animator.SetTrigger("Start");
    }
}
