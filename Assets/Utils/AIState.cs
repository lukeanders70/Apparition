using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState
{
    public class Invokable
    {
        public System.Action action;
        public float timeLeft;
        public bool complete = false;
        public Invokable(System.Action a, float t)
        {
            action = a;
            timeLeft = t;
        }
    }

    private List<Invokable> invokables = new List<Invokable>();

    public void Invoke(System.Action action, float waitTime)
    {
        invokables.Add(new Invokable(action, waitTime));
    }
    virtual public void Update()
    {
        var newInvokables = new List<Invokable>();
        foreach (Invokable invokeable in invokables)
        {
            invokeable.timeLeft -= Time.deltaTime;
            if(invokeable.timeLeft <= 0)
            {
                invokeable.action();
            } else
            {
                newInvokables.Add(invokeable);
            }
        }
        invokables = newInvokables;
    }
    virtual public void StartState()
    {

    }
    virtual public void StopState()
    {

    }

    virtual public void OnCollision(Collision2D collision)
    {

    }
}
