using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    [SerializeField] Transform choicesContainer;
    [SerializeField] GameObject choiceButtonPrefab;

    private bool isDialogueActive = false;
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
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }

    public void ShowDialogue(string speaker, string text)
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        speakerText.text = speaker;
        dialogueText.text = text;
    }

    public void HideDialogue()
    {
        playerController.suppressNextClick = true;
        dialogueText.text = "";
        speakerText.text = "";
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        ClearChoices();
    }

    public void ShowChoices(DialogueChoice[] choices, System.Action<string> onChoiceSelected)
    {
        ClearChoices();

        foreach(var choice in choices)
        {
            if(!string.IsNullOrEmpty(choice.requiredFlag))
            {
                bool flagState = ProgressManager.Instance.HasFlag(choice.requiredFlag);
                if(flagState != choice.requiredFlagToBeTrue)
                {
                    continue;
                }
            }

            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer);
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = choice.choiceText;

            Button button = buttonObj.GetComponent<Button>();
            string nextNodeId = choice.nextNodeId;

            button.onClick.AddListener(() => onChoiceSelected(nextNodeId));
        }
    }

    public void ClearChoices()
    {
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
