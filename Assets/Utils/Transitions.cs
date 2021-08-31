using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions: MonoBehaviour
{
    [SerializeField]
    private Animator levelTransition;
    public IEnumerator LevelTransition(System.Action callback)
    {
        levelTransition.updateMode = AnimatorUpdateMode.UnscaledTime;
        levelTransition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(1);
        callback();
        levelTransition.SetTrigger("End");
    }
}
