using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / InventoryUI.Instance.GetCanvasScale();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 clickPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        Collider2D hit = Physics2D.OverlapPoint(clickPos, LayerMask.GetMask("Interactable"));

        if (hit != null)
        {
            var interactable = hit.GetComponent<Interactable>();
            var door = hit.GetComponent<DoorController>();

            if (interactable != null && door != null)
            {
                if (interactable.requiredItem != null && interactable.requiredItem == item)
                {
                    Debug.Log("Correct item dropped on " + interactable.name);
                    PlayerController.Instance.MoveToInteract(interactable);

                    PlayerController.Instance.OnArrival = () =>
                    {
                        door.OpenDoor(
                            item.itemName,
                            item.consumeOnUse,
                            item.successMessage
                        );

                        if (item.consumeOnUse)
                        {
                            InventoryManager.Instance.RemoveItem(item.itemId);
                            InventoryUI.Instance.ToggleInventory();
                        }
                    };
                }
                else
                {
                    InteractionUI.Instance.ShowPopup("That doesn't work.");
                }
            }
            else
            {
                InteractionUI.Instance.ShowPopup("That doesn't work.");
            }
        }

        InventoryUI.Instance.RefreshUI();
    }
}
