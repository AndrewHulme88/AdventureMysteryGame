using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [SerializeField] string targetScene;
    [SerializeField] Vector2 targetPositionInNewScene;

    private bool playerInTrigger = false;

    private void Update()
    {
        if(playerInTrigger && Input.GetMouseButtonDown(0))
        {
            SceneTransitionManager.Instance.TransitionToScene(targetScene, targetPositionInNewScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}
