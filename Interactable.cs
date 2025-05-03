using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IDropHandler
{
    [TextArea(2, 5)] public string interactionText;

    public bool requireWalk = false;
    public DialogueTree dialogueTree;

    [Header("Flag Conditions")]
    public string requiredFlag = "";
    public bool requireFlagToBeTrue = false;
    public string blockedMessage = "Nothing happened...";

    [Header("Set Flags On Interaction")]
    public string[] flagsToSet;

    [Header("Walk to Point")]
    public Transform walkToPoint;

    [Header("Optional Required Item")]
    public bool requiresItem = false;
    public Item requiredItem;
    [TextArea] public string successMessage = "You used {itemName}.";
    [TextArea] public string itemRequiredMessage = "";
    public bool consumeItemOnUse = false;

    [Header("Linked Door")]
    public DoorController linkedDoor;

    public virtual void Interact()
    {
        if (DialogueUI.Instance != null && DialogueUI.Instance.dialoguePanel.activeInHierarchy)
        {
            Debug.Log("Dialogue already active, ignoring new interaction.");
            return;
        }

        if(requiresItem)
        {
            InteractionUI.Instance.ShowPopup(itemRequiredMessage);
            return;
        }

        if(!string.IsNullOrEmpty(requiredFlag))
        {
            bool hasFlag = ProgressManager.Instance.HasFlag(requiredFlag);

            if(hasFlag != requireFlagToBeTrue)
            {
                InteractionUI.Instance.ShowPopup(blockedMessage);
                return;
            }
        }

        if(linkedDoor != null)
        {
            linkedDoor.OpenDoor();
        }

        if (dialogueTree != null)
        {
            FindFirstObjectByType<DialogueManager>().StartDialogue(dialogueTree);
        }
        else if(!string.IsNullOrEmpty(interactionText))
        {
            if(!string.IsNullOrEmpty(interactionText))
            {
                InteractionUI.Instance.ShowPopup(interactionText);
            }
            else
            {
                Debug.Log("No interaction text is set");
            }
        }

        foreach(string flag in flagsToSet)
        {
            if(!string.IsNullOrWhiteSpace(flag))
            {
                ProgressManager.Instance.SetFlag(flag, true);
                Debug.Log("HOHoh");
            }
        }
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        var dragHandler = eventData.pointerDrag?.GetComponent<ItemDragManager>();

        if(dragHandler != null && dragHandler.item == requiredItem)
        {
            PlayerController.Instance.MoveToInteract(this);
        }
        else
        {
            InteractionUI.Instance.ShowPopup("That doesn't work.");
        }
    }
}
