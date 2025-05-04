using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance { get; private set; }

    [SerializeField] GameObject popupPanel;
    [SerializeField] TMP_Text popupText;
    [SerializeField] GameObject yesNoPanel;
    [SerializeField] TMP_Text yesNoText;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    private bool isPopupActive = false;
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
        popupPanel.SetActive(false);
        isPopupActive = false;
        playerController = FindFirstObjectByType<PlayerController>();
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
        playerController.suppressNextClick = true;
        popupText.text = "";
        popupPanel.SetActive(false);
        isPopupActive = false;
    }

    public void ShowYesNo(string message, System.Action onYes, System.Action onNo = null)
    {
        yesNoText.text = message;
        yesNoPanel.SetActive(true);
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            yesNoPanel.SetActive(false);         
            onYes.Invoke();
        });

        noButton.onClick.AddListener(() =>
        {
            yesNoPanel.SetActive(false);
            onNo.Invoke();
        });
    }

    public bool IsPopupActive()
    {
        return isPopupActive;
    }
}
