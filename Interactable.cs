using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    public enum InteractableType { Inspect, Door, Talk, None }

    [Header("Highlighting")]
    public InteractableType interactableType = InteractableType.Inspect;
    private GameObject iconInstance;

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
    public string setFlagOnItemUse;
    public bool flagValue = true;

    [Header("Optional Item To Give")]
    public Item itemToGive;
    public bool isPickup = false;
    [TextArea] public string pickupInspectMessage = "This might be useful.";
    public bool requireConfirmationToPickUp = true;

    [Header("Linked Door")]
    public DoorController linkedDoor;

    [Header("Optional Image Display")]
    public Sprite interactionImage;
    public bool showImageBeforePickup = false;
    public bool showImageOnly = false;

    [Header("Pushable")]
    public bool isPushable = false;
    public string pushFlag = "objectPushed";
    public Vector3 pushedPosition;
    public GameObject hiddenDoorToReveal;
    [TextArea] public string pushableInspectMessage = "This might be useful.";

    public List<ConditionalInteraction> conditionalInteractions;

    private void Start()
    {
        var iconPrefab = InteractableIconManager.Instance.sharedIconPrefab;
        if (iconPrefab == null || interactableType == InteractableType.None) return;

        iconInstance = Instantiate(iconPrefab, transform.position, Quaternion.identity, transform);
        iconInstance.SetActive(false);

        var iconRenderer = iconInstance.GetComponentInChildren<SpriteRenderer>();

        if(iconRenderer != null)
        {
            iconRenderer.sprite = GetIconForType(interactableType);
        }
    }

    public virtual void Interact()
    {
        if(isPushable)
        {
            if(ProgressManager.Instance.HasFlag(pushFlag))
            {
                InteractionUI.Instance.ShowPopup("It has already been moved.");
                return;
            }

            InteractionUI.Instance.ShowYesNo(pushableInspectMessage, () =>
            {
                ProgressManager.Instance.SetFlag(pushFlag, true);
                transform.position = pushedPosition;

                if (hiddenDoorToReveal != null)
                {
                    hiddenDoorToReveal.SetActive(true);
                }
            });
        }

        foreach(var condition in conditionalInteractions)
        {
            bool hasFlag = ProgressManager.Instance.HasFlag(condition.requiredFlag);

            if(hasFlag == condition.requireFlagToBeTrue)
            {
                if(condition.alternateDialogue != null)
                {
                    DialogueManager.Instance.StartDialogue(condition.alternateDialogue);
                }
                else if(!string.IsNullOrEmpty(condition.alternateInteractionText))
                {
                    InteractionUI.Instance.ShowPopup(condition.alternateInteractionText);
                }

                foreach(string flag in condition.flagsToSetOnInteract)
                {
                    ProgressManager.Instance.SetFlag(flag, true);
                }
            }
        }

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

        if (interactionImage != null && showImageOnly)
        {
            InteractionUI.Instance.ShowImagePrompt(interactionText, interactionImage, null);
            return;
        }

        if (itemToGive != null && isPickup)
        {
            if (interactionImage != null && showImageBeforePickup)
            {
                string combinedMessage = $"{interactionText}\n\n{pickupInspectMessage}";

                InteractionUI.Instance.ShowImageWithYesNo(
                    combinedMessage,
                    interactionImage,
                    HandlePickupConfirmed,
                    () => { /* Optional cancel logic */ }
                );
                return;
            }
            else
            {
                InteractionUI.Instance.ShowYesNo(pickupInspectMessage, HandlePickupConfirmed);
            }

            return;
        }

        if (linkedDoor != null)
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

    private void HandlePickupConfirmed()
    {
        InventoryManager.Instance.AddItem(itemToGive);

        string finalMessage = string.IsNullOrWhiteSpace(itemToGive.pickupMessage)
            ? $"You picked up: {itemToGive.itemName}"
            : itemToGive.pickupMessage.Replace("{itemName}", itemToGive.itemName);

        InteractionUI.Instance.ShowPopup(finalMessage);

        if (!string.IsNullOrWhiteSpace(setFlagOnItemUse))
        {
            ProgressManager.Instance.SetFlag(setFlagOnItemUse);
        }

        Destroy(gameObject);
    }


    private Sprite GetIconForType(InteractableType type)
    {
        return InteractableIconManager.Instance.GetIconSprite(type);
    }

    public void ShowIcon(bool show)
    {
        if (iconInstance == null || interactableType == InteractableType.None) return;

        iconInstance.SetActive(show);
    }
}

[System.Serializable]
public class ConditionalInteraction
{
    public string requiredFlag;
    public bool requireFlagToBeTrue = true;
    [TextArea] public string alternateInteractionText;
    public DialogueTree alternateDialogue;
    public string[] flagsToSetOnInteract;
}
