using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemId;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public bool consumeOnUse = true;
    [TextArea] public string successMessage = "You used {itemName}.";
}
