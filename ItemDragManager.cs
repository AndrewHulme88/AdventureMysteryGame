using System.Collections.Generic;
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
        rectTransform.anchoredPosition = Vector2.zero;

        //GameObject dropTarget = eventData.pointerEnter;

        //if(dropTarget != null)
        //{
        //    var dropHandler = dropTarget.GetComponent<ItemDragManager>();
        //    Debug.Log("Pointer entered: " + eventData.pointerEnter?.name);


        //    if (dropHandler != null && dropHandler != this)
        //    {
        //        TryCombineWith(dropHandler.item);
        //        InventoryUI.Instance.RefreshUI();
        //        return;
        //    }
        //}

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
        else
        {
            Debug.Log("Nothing interactable hit.");
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
