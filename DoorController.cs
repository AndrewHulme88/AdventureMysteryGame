using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Sprites")]
    [SerializeField] SpriteRenderer doorSprite;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;

    [Header("Door Logic")]
    public bool isLocked = false;
    public Item requiredItem;
    public string lockedMessage = "It's locked.";

    [Header("Optional Trigger On Open")]
    public GameObject[] activeateWhenOpened;

    private bool isOpen = false;

    public void TryOpen()
    {
        if (isOpen) return;

        if(isLocked && requiredItem != null && !InventoryManager.Instance.HasItem(requiredItem.itemId))
        {
            InteractionUI.Instance.ShowPopup(lockedMessage);
            return;
        }

        OpenDoor();
    }

    public void OpenDoor()
    {
        isOpen = true;

        if(doorSprite != null && openSprite != null)
        {
            doorSprite.sprite = openSprite;
        }

        foreach(GameObject go in activeateWhenOpened)
        {
            if(go != null)
            {
                go.SetActive(true);
            }
        }
    }
    
}
