using UnityEngine;

public class Interactable : MonoBehaviour
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

    public virtual void Interact()
    {
        if (DialogueUI.Instance != null && DialogueUI.Instance.dialoguePanel.activeInHierarchy)
        {
            Debug.Log("Dialogue already active, ignoring new interaction.");
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
}
