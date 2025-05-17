using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Item item;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool isDragging = false;
    private Vector2 mouseDownPos;
    private bool pointerDown = false;
    private float dragThreshold = 2f;
    private float timeThreshold = 0.2f;
    private float mouseDownTime;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        mouseDownPos = eventData.position;
        mouseDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!pointerDown) return;
        pointerDown = false;

        float dist = Vector2.Distance(eventData.position, mouseDownPos);
        float heldTime = Time.time - mouseDownTime;

        if (!isDragging && dist < dragThreshold && heldTime < timeThreshold)
        {
            Debug.Log("Item clicked");
            InventoryUI.Instance.ShowItemDescription(item); 
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = false;
        mouseDownPos = eventData.position;
        mouseDownTime = Time.time;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            float dist = Vector2.Distance(eventData.position, mouseDownPos);
            if (dist > dragThreshold || Time.time - mouseDownTime > timeThreshold)
            {
                isDragging = true;
            }
        }

        if (isDragging)
        {
            rectTransform.anchoredPosition += eventData.delta / InventoryUI.Instance.GetCanvasScale();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = Vector2.zero;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Debug.Log("Raycast hit: " + result.gameObject.name);

            var dropHandler = result.gameObject.GetComponentInParent<ItemDragManager>();
            if (dropHandler != null && dropHandler != this)
            {
                TryCombineWith(dropHandler.item);
                InventoryUI.Instance.RefreshUI();
                return;
            }
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 clickPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        Collider2D hit = Physics2D.OverlapPoint(clickPos);

        if (hit != null)
        {
            Debug.Log("Hit: " + hit.name);

            Interactable interactable = hit.GetComponent<Interactable>();
            if (interactable != null && interactable.requiresItem && interactable.requiredItem != null)
            {
                if (item.itemId == interactable.requiredItem.itemId)
                {
                    PlayerController.Instance.MoveToInteract(interactable);
                    PlayerController.Instance.OnArrival = () =>
                    {
                        string messageTemplate = string.IsNullOrWhiteSpace(item.successMessage)
                            ? "You used {itemName}."
                            : item.successMessage;

                        string finalMessage = messageTemplate.Replace("{itemName}", item.itemName);
                        InteractionUI.Instance.ShowPopup(finalMessage);

                        if (!string.IsNullOrWhiteSpace(interactable.setFlagOnItemUse))
                        {
                            ProgressManager.Instance.SetFlag(interactable.setFlagOnItemUse, interactable.flagValue);
                        }

                        if(interactable.linkedDoor != null)
                        {
                            interactable.linkedDoor.OpenDoor();
                        }

                        if (interactable.consumeItemOnUse)
                        {
                            InventoryManager.Instance.RemoveItem(item.itemId);
                        }

                        if (interactable.itemToGive != null)
                        {
                            InventoryManager.Instance.AddItem(interactable.itemToGive);
                        }

                        InventoryUI.Instance.ToggleInventory();
                        PlayerController.Instance.OnArrival = null;
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

    private void TryCombineWith(Item otherItem)
    {
        foreach(var combo in item.combinations)
        {
            if(combo.linkedItem == otherItem)
            {
                InventoryManager.Instance.RemoveItem(item.itemId);
                InventoryManager.Instance.RemoveItem(otherItem.itemId);
                InventoryManager.Instance.AddItem(combo.resultItem);

                string message = string.IsNullOrWhiteSpace(combo.combinedSuccessMessage)
                    ? $"You combined {item.itemName} with {otherItem.itemName}."
                    : combo.combinedSuccessMessage;

                InteractionUI.Instance.ShowPopup(message);
                return;
            }
        }

        string failMessage = item.combinations.Find(c => c.linkedItem == otherItem)?.combinedFailureMessage
            ?? $"That doesn't seem to work.";

        InteractionUI.Instance.ShowPopup(failMessage);
    }
}
