using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] DialogueTree currentTree;

    private DialogueNode currentNode;
    private bool waitingForChoice = false;

    private const string EndDialogueChoiceId = "END_DIALOGUE_CHOICE";


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!waitingForChoice) return;

        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            waitingForChoice = false;

            if(!string.IsNullOrEmpty(currentNode.nextNodeId))
            {
                currentNode = currentTree.GetNodeById(currentNode.nextNodeId);
                ShowCurrentNode();
            }
            else if(currentNode.choices != null && currentNode.choices.Count > 0)
            {
                DialogueUI.Instance.speakerText.text = "";
                DialogueUI.Instance.dialogueText.text = "";
                DialogueUI.Instance.ShowChoices(currentNode.choices.ToArray(), HandleChoice);
            }
            else if(currentNode.isEndNode)
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(DialogueTree tree)
    {
        currentTree = tree;
        currentNode = currentTree.GetNodeById(tree.startingNodeId);
        ShowCurrentNode();
    }

    private void ShowCurrentNode()
    {
        if(currentNode == null)
        {
            EndDialogue();
            return;
        }

        DialogueUI.Instance.HideDialogue();
        DialogueUI.Instance.ShowDialogue(currentNode.speakerName, currentNode.dialogueText);

        if(!string.IsNullOrEmpty(currentNode.nextNodeId))
        {
            waitingForChoice = true;
        }
        else if (currentNode.choices != null && currentNode.choices.Count > 0)
        {
            var tempChoices = new List<DialogueChoice>(currentNode.choices);

            tempChoices.Add(new DialogueChoice
            {
                choiceText = "Exit.",
                nextNodeId = EndDialogueChoiceId
            });

            DialogueUI.Instance.speakerText.text = "";
            DialogueUI.Instance.dialogueText.text = "";
            DialogueUI.Instance.ShowChoices(tempChoices.ToArray(), HandleChoice);

            waitingForChoice = false; 
        }

        else if (currentNode.isEndNode)
        {
            waitingForChoice = true;
        }
    }

    private void HandleChoice(string nextNodeId)
    {
        if (nextNodeId == EndDialogueChoiceId)
        {
            EndDialogue();
            return;
        }

        currentNode = currentTree.GetNodeById(nextNodeId);
        ShowCurrentNode();
    }

    public void EndDialogue()
    {
        currentNode = null;
        currentTree = null;
        DialogueUI.Instance.HideDialogue();
    }
}
