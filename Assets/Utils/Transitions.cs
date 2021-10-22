using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions: MonoBehaviour
{
    [SerializeField]
    private TransitionController levelTransition;
    public void Start()
    {
        
    }
    public TransitionController LevelTransition()
    {
        return levelTransition;
    }

    public enum TransitionState
    {
        Transparent,
        Start,
        Load,
        End
    }
}


