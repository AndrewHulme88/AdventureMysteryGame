using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [TextArea]
    public string message = "You find something.";
    public bool requireWalk = false;

    public void Interact()
    {
        InteractionUI.Instance.ShowPopup(message);
    }

    public string GetDescription()
    {
        return message;
    }
}
