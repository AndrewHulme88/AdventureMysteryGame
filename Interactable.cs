using UnityEngine;

public class Interactable : MonoBehaviour
{
    [TextArea(2, 5)] public string interactionText;

    public bool requireWalk = false;
    public DialogueTree dialogueTree;

    public virtual void Interact()
    {
        if (DialogueUI.Instance != null && DialogueUI.Instance.dialoguePanel.activeInHierarchy)
        {
            Debug.Log("Dialogue already active, ignoring new interaction.");
            return;
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
    }
}
