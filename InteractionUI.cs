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
    [SerializeField] GameObject imagePanel;
    [SerializeField] TMP_Text imageText;
    [SerializeField] Image displayImage;
    [SerializeField] GameObject yesNoPanelImage;
    [SerializeField] TMP_Text yesNoTextImage;
    [SerializeField] Button yesButtonImage;
    [SerializeField] Button noButtonImage;

    private bool isPopupActive = false;
    private PlayerController playerController;
    private bool isImageActive = false;
    private System.Action onImageClosed;

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
            PlayerController.Instance.stopMovement = false;
        }

        if (isImageActive && Input.GetMouseButtonDown(0) && !yesNoPanelImage.activeSelf)
        {
            PlayerController.Instance.stopMovement = false;
            CloseImagePrompt();
        }
    }

    public void ShowPopup(string text)
    {
        PlayerController.Instance.stopMovement = true;
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
        PlayerController.Instance.stopMovement = true;
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

    public void ShowImageWithYesNo(string message, Sprite image, System.Action onYes, System.Action onNo = null)
    {
        PlayerController.Instance.stopMovement = true;
        yesNoTextImage.text = message;
        displayImage.sprite = image;
        imagePanel.SetActive(true); 
        isImageActive = false; 

        yesNoTextImage.text = message;
        yesNoPanelImage.SetActive(true);

        yesButtonImage.onClick.RemoveAllListeners();
        noButtonImage.onClick.RemoveAllListeners();

        yesButtonImage.onClick.AddListener(() =>
        {
            imagePanel.SetActive(false);
            yesNoPanelImage.SetActive(false);
            onYes?.Invoke();
        });

        noButtonImage.onClick.AddListener(() =>
        {
            imagePanel.SetActive(false);
            yesNoPanelImage.SetActive(false);
            onNo?.Invoke();
        });
    }


    public void ShowImagePrompt(string text, Sprite image, System.Action onClose)
    {
        PlayerController.Instance.stopMovement = true;
        imageText.text = text;
        displayImage.sprite = image;
        imagePanel.SetActive(true);
        isImageActive = true;
        onImageClosed = onClose;
    }

    private void CloseImagePrompt()
    {
        playerController.suppressNextClick = true;
        imagePanel.SetActive(false);
        isImageActive = false;

        if(onImageClosed != null)
        {
            onImageClosed.Invoke();
            onImageClosed = null;
        }
    }

    public bool IsPopupActive()
    {
        return isPopupActive;
    }
}
