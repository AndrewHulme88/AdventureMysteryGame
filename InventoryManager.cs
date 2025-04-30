using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private HashSet<string> collectedItemIds = new HashSet<string>();
    private List<Item> collectedItems = new List<Item>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddItem(Item item)
    {
        if(!collectedItemIds.Contains(item.itemId))
        {
            collectedItemIds.Add(item.itemId);
            collectedItems.Add(item);
            Debug.Log("Item collected: " + item.name);
        }
    }

    public void RemoveItem(string itemId)
    {
        collectedItemIds.Remove(itemId);
        Debug.Log("Item removed " + itemId);
    }

    public bool HasItem(string itemId)
    {
        return collectedItemIds.Contains(itemId);
    }

    public List<Item> GetAllItems()
    {
        return new List<Item>(collectedItems);
    }
}
