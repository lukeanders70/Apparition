using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LadderCollision : MonoBehaviour
{
    [SerializeField]
    Transitions transitionHandler;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.tag == "Player")
        {
            GameObject dungeon = GameObject.Find("Dungeon");
            GameObject camera = GameObject.Find("Main Camera");
            GameObject transitions = GameObject.Find("Transitions");
            PlayerHandler playerHandler = collidedObject.transform.parent.GetComponent<PlayerHandler>();
            if (dungeon != null)
            {
                Time.timeScale = 0;
                StartCoroutine(transitions.GetComponent<Transitions>().LevelTransition(
                    () =>
                    {
                        bool movedDown = dungeon.GetComponent<DungeonGenerator>().MoveDown();
                        if (movedDown)
                        {
                            camera.GetComponent<CameraMoveController>().Recenter();
                            GameObject Player = collidedObject.transform.parent.gameObject;
                            Player.GetComponent<PlayerHandler>().resetPosition();
                        }
                        Time.timeScale = 1;
                    }
                ));
            } else
            {
                Debug.LogError("No gameobject 'Dungeon' found");
            }

        }
    }
}
