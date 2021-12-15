using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState
{
    public class Invokable
    {
        public System.Action action;
        public float timeLeft;
        private bool cancel = false;
        public Invokable(System.Action a, float t)
        {
            action = a;
            timeLeft = t;
        }

        public void Cancel()
        {
            cancel = true;
        }
        public bool Canceled()
        {
            return cancel;
        }
    }

    protected List<Invokable> invokables = new List<Invokable>();

    public Invokable Invoke(System.Action action, float waitTime)
    {
        var newInvoke = new Invokable(action, waitTime);
        invokables.Add(newInvoke);
        return newInvoke;
    }
    virtual public void Update()
    {
        var newInvokables = new List<Invokable>();
        foreach (Invokable invokeable in invokables.ToArray())
        {
            invokeable.timeLeft -= Time.deltaTime;
            if (!invokeable.Canceled())
            {
                if (invokeable.timeLeft <= 0)
                {
                    invokeable.action();
                }
                else
                {
                    newInvokables.Add(invokeable);
                }
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
