using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransitionContoller : MonoBehaviour
{
    public float FadeTime;
    [SerializeField]
    private SpriteRenderer sr;

    public void Start()
    {
        GameObject transitions = GameObject.Find("Transitions");
        if(transitions == null)
        {
            Debug.LogError("Failed to find Transitions Canvas to child fade transition to");
        } else
        {
            transform.parent = transitions.transform;
            transform.localPosition = Vector3.zero;
        }
    }
    public void StartFadeToBlack(Action callback)
    {
        sr.color = Color.clear;
        StartCoroutine(Fade(1.0f, callback));
    }

    public void StartFadeToTransparent(Action callback)
    {
        sr.color = Color.black;
        StartCoroutine(Fade(0.0f, callback));
    }

    private IEnumerator Fade(float desiredTransparency, Action callback)
    {
        var elapsedTime = 0.0f;
        var startingTransparency = sr.color.a;
        while(elapsedTime < FadeTime)
        {
            var percentTime = elapsedTime / FadeTime;
            var newTransparency = ((1 - percentTime) * startingTransparency) + (percentTime * desiredTransparency);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newTransparency);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, desiredTransparency);
        callback();
        yield break;
    }
}
