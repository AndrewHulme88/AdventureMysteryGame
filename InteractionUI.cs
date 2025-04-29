using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance { get; private set; }

    [SerializeField] GameObject popupPanel;
    [SerializeField] TMP_Text popupText;

    private bool isPopupActive = false;

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
        popupPanel.SetActive(false);
    }

    private void Update()
    {
        if(isPopupActive && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            HidePopup();
        }
    }

    public void ShowPopup(string text)
    {
        popupPanel.SetActive(true);
        popupText.text = text;
        isPopupActive = true;
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
        isPopupActive = false;
    }
}
