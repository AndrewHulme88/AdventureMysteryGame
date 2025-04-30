using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    private Vector2 targetPositionInNextScene;
    private bool shouldPlayerMove = false;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void TransitionToScene(string sceneName, Vector2 spawnPosition)
    {
        targetPositionInNextScene = spawnPosition;
        shouldPlayerMove = true;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!shouldPlayerMove) return;

        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            player.transform.position = targetPositionInNextScene;
        }
    }
}
