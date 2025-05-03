using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Sprites")]
    [SerializeField] SpriteRenderer doorSprite;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;

    [Header("Optional Trigger On Open")]
    public GameObject[] activeateWhenOpened;

    private bool isOpen = false;
    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    public void OpenDoor(string itemName = null, bool consumeItem = false, string successMessage = "")
    {
        if (isOpen) return;

        isOpen = true;

        if (!string.IsNullOrEmpty(successMessage))
        {
            string finalMessage = itemName != null
                ? successMessage.Replace("{itemName}", itemName)
                : successMessage;
            InteractionUI.Instance.ShowPopup(finalMessage);
        }

        if (consumeItem && itemName != null)
        {
            InventoryManager.Instance.RemoveItem(itemName);
        }

        if (doorSprite != null && openSprite != null)
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

        Destroy(col);
    }
    
}
