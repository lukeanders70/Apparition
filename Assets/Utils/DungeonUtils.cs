using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUtils
{
    public static GameObject GetRoom(GameObject roomObject)
    {
        GameObject parent = getParentObjectIfExists(roomObject);

        while (parent != null)
        {
            if (parent.GetComponent<RoomController>() != null)
            {
                return parent;
            } else
            {
                parent = getParentObjectIfExists(parent);
            }
        }
        return null;
    }

    public static GameObject getParentObjectIfExists(GameObject o)
    {
        return o.transform != null && o.transform.parent != null ?
            o.transform.parent.gameObject :
            null;
    }
}
