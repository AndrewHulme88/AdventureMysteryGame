using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("Cursor Textures")]
    public Texture2D defaultCursor;
    public Texture2D interactCursor;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, LayerMask.GetMask("Interactable"));

        if(hit != null && hit.CompareTag("Interactable"))
        {
            Vector2 hotspot = new Vector2(interactCursor.width / 2f, interactCursor.height / 2f);
            Cursor.SetCursor(interactCursor, hotspot, CursorMode.Auto);
        }
        else
        {
            Vector2 center = new Vector2(defaultCursor.width / 2f, defaultCursor.height / 2f);
            Cursor.SetCursor(defaultCursor, center, CursorMode.Auto);
        }
    }
}
