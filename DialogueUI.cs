using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;

    private Queue<string> dialogueQueue = new Queue<string>();
    private bool isDialogueActive = false;

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
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if(isDialogueActive && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            DisplayNextLine();
        }
    }

    public void StartDialogue(string[] lines)
    {
        EndDialogue();
        dialogueQueue.Clear();

        foreach(var line in lines)
        {
            dialogueQueue.Enqueue(line);
        }

        dialoguePanel.SetActive(true);
        isDialogueActive = true;

        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string nextLine = dialogueQueue.Dequeue();
        dialogueText.text = nextLine;
    }

    public void EndDialogue()
    {
        dialogueQueue.Clear();
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
