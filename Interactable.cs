using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string message = "You find something.";

    public void OnInteract()
    {
        Debug.Log(message);
    }
}
