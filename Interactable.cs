using UnityEngine;

public class Interactable : MonoBehaviour
{
    [TextArea(2, 5)] public string interactionText;
    [TextArea(3, 10)] public string[] dialogueLines;
    public bool requireWalk = false;
    public bool isDialogue = false;

    public void Interact()
    {
        if (DialogueUI.Instance.IsDialogueActive())
        {
            // Prevent starting new dialogue while old one is active
            Debug.Log("Dialogue already active, ignoring new interaction.");
            return;
        }
        if (isDialogue)
        {
            if(dialogueLines.Length > 0)
            {
                DialogueUI.Instance.StartDialogue(dialogueLines);
            }
            else
            {
                Debug.Log("No dialog lines are set");
            }
        }
        else
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

    //public string GetDescription()
    //{
    //    return dialogueLines[0];
    //}
}
