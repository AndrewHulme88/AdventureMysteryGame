using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [SerializeField] string targetScene;
    [SerializeField] Vector2 targetPositionInNewScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneTransitionManager.Instance.TransitionToScene(targetScene, targetPositionInNewScene);
        }
    }
}
