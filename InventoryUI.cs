using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] GameObject itemDescriptionPanel;
    [SerializeField] TMP_Text itemDescriptionText;

    public GameObject inventoryPanel;
    public Transform itemListContainer;
    public GameObject itemSlotPrefab;
    public Canvas canvas;
    
    private bool isInventoryActive = false;
    private PlayerController playerController;
    private bool isDescriptionActive = false;

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
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            ToggleInventory();
        }

        if(isDescriptionActive && Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            itemDescriptionPanel.SetActive(false);
            isDescriptionActive = false;
        }
    }

    public void ShowItemDescription(Item item)
    {
        itemDescriptionText.text = item.description;
        itemDescriptionPanel.SetActive(true);
        isDescriptionActive = true;
    }

    public void ToggleInventory()
    {
        RefreshUI();
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
            var handler = slot.GetComponent<ItemDragManager>();
            if (handler != null)
            {
                handler.item = item;
            }
        }
    }

    public bool IsInventoryActive()
    {
        return isInventoryActive;
    }

    public float GetCanvasScale()
    {
        return canvas.scaleFactor;
    }
}
