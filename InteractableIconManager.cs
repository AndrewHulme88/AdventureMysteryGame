using UnityEngine;

public class InteractableIconManager : MonoBehaviour
{
    public static InteractableIconManager Instance { get; private set; }

    public GameObject sharedIconPrefab;
    public Sprite inspectIcon;
    public Sprite doorIcon;
    public Sprite talkIcon;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Sprite GetIconSprite(Interactable.InteractableType type)
    {
        return type switch
        {
            Interactable.InteractableType.Door => doorIcon,
            Interactable.InteractableType.Talk => talkIcon,
            _ => inspectIcon
        };
    }
}
