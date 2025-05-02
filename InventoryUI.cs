using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    public GameObject inventoryPanel;
    public Transform itemListContainer;
    public GameObject itemSlotPrefab;
    
    private bool isInventoryActive = false;
    private PlayerController playerController;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!isInventoryActive);
        isInventoryActive = inventoryPanel.activeSelf;
        playerController.suppressNextClick = true;

        if(!isInventoryActive)
        {
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        foreach(Transform child in itemListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(Item item in InventoryManager.Instance.GetAllItems())
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemListContainer);
            slot.GetComponentInChildren<TMP_Text>().text = item.itemName;
            Image icon = slot.GetComponentInChildren<Image>();

            if(icon != null && item.icon != null)
            {
                icon.sprite = item.icon;
            }
        }
    }

    public bool IsInventoryActive()
    {
        return isInventoryActive;
    }
}
