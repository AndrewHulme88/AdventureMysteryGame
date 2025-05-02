using UnityEngine;

public class DoorInteractable : Interactable
{
    public DoorController door;

    public override void Interact()
    {
        if(door != null)
        {
            door.TryOpen();
        }
    }
}
