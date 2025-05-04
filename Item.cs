using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemId;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public bool consumeOnUse = true;
    [TextArea] public string successMessage = "You used {itemName}.";
    [TextArea] public string pickupMessage = "You picked up {itemName}.";
    public List<ItemCombination> combinations = new();
}

[System.Serializable]
public class ItemCombination
{
    public Item linkedItem;
    public Item resultItem;
    [TextArea] public string combinedSuccessMessage;
    [TextArea] public string combinedFailureMessage;
};
