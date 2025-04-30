using UnityEngine;

public class PickupInteractable : Interactable
{
    public Item itemToGive;

    public override void Interact()
    {
        if(itemToGive != null)
        {
            InventoryManager.Instance.AddItem(itemToGive);
            InteractionUI.Instance.ShowPopup($"You picked up: {itemToGive.itemName}");
            Destroy(gameObject);
        }
    }
}
