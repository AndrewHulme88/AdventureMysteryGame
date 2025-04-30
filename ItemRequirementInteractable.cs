using UnityEngine;

public class ItemRequirementInteractable : Interactable
{
    [Header("Item Requirement")]
    public Item requiredItem;
    [TextArea] public string missingItemMessage = "It's locked!";

    [Header("On Success")]
    [TextArea] public string successMessage = "You unlocked the door with {itemName}.";
    public DialogueTree successDialogue;
    public bool consumeItemOnUse = false;
    public GameObject[] activateAfterSuccess;
    public bool disableAferUse = true;

    public override void Interact()
    {
        if(DialogueUI.Instance != null && DialogueUI.Instance.dialoguePanel.activeInHierarchy)
        {
            return;
        }

        if(requiredItem == null)
        {
            Debug.LogWarning("No required item set");
            return;
        }

        if (!InventoryManager.Instance.HasItem(requiredItem.itemId))
        {
            InteractionUI.Instance.ShowPopup(missingItemMessage);
            return;
        }

        if(successDialogue != null)
        {
            FindFirstObjectByType<DialogueManager>().StartDialogue(successDialogue);
        }

        if(!string.IsNullOrEmpty(successMessage))
        {
            string finalMessage = successMessage.Replace("{itemName}", requiredItem.itemName);
            InteractionUI.Instance.ShowPopup(finalMessage);
        }

        foreach(var obj in activateAfterSuccess)
        {
            if(obj != null)
            {
                obj.SetActive(true);
            }
        }

        if(consumeItemOnUse)
        {
            InventoryManager.Instance.RemoveItem(requiredItem.itemId);
        }

        if(disableAferUse)
        {
            gameObject.SetActive(false);
        }
    }
}
