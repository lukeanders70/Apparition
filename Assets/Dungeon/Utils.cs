using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticDungeon
{
    public class ObjectProbability<T>
    {
        public T obj;
        public float probability;
    }

    static class Utils
    {
        public static T ChooseFromObjectProbability<T>(ObjectProbability<T>[] objProbs)
        {
            var sumAll = 0.0f;
            foreach (ObjectProbability<T> objProb in objProbs)
            {
                sumAll += objProb.probability;
            }
            float r = Random.Range(0f, sumAll);
            var sum = 0f;
            foreach (ObjectProbability<T> objProb in objProbs)
            {
                sum += objProb.probability;
                if (sum > r)
                {
                    return objProb.obj;
                }
            }
            return default;
        }
    }
}
