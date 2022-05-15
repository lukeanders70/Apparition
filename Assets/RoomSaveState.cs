using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSaveState : MonoBehaviour
{
    public Dictionary<string, string> stringMap = new Dictionary<string, string>();

    public Dictionary<string, float> floatMap = new Dictionary<string, float>();

    public Dictionary<string, bool> boolMap = new Dictionary<string, bool>();


    public void AddString(string key, string value)
    {
        stringMap[key] = value;
    }

    public string GetString(string key)
    {
        if (stringMap.ContainsKey(key))
        {
            return stringMap[key];
        }
        return null;
    }

    public void AddFloat(string key, float value)
    {
        floatMap[key] = value;
    }

    public float? GetFloat(string key)
    {
        if (stringMap.ContainsKey(key))
        {
            return floatMap[key];
        }
        return null;
    }

    public void AddBool(string key, bool value)
    {
        boolMap[key] = value;
    }

    public bool? GetBool(string key)
    {
        if (boolMap.ContainsKey(key))
        {
            return boolMap[key];
        }
        return null;
    }
}
