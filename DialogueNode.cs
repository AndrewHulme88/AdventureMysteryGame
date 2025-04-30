using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueNode
{
    public string id;
    public string speakerName;
    [TextArea(3, 10)] public string dialogueText;
    public string nextNodeId;
    public List<DialogueChoice> choices;
    public bool isEndNode = false;
}
