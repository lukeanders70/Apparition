using UnityEngine;
using UnityEngine.SceneManagement;

public class LadderCollision : MonoBehaviour
{
    [SerializeField]
    GameObject FadeTransition;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.tag == "Player")
        {
            var fadeTransition = Instantiate(FadeTransition);
            var transitionController = fadeTransition.GetComponent<FadeTransitionContoller>();
            transitionController.StartFadeToBlack(() =>
            {
                DungeonStateInfo.levelIndex += 1;
                SceneManager.LoadScene("LevelTransition");
            });
        }
    }
}
