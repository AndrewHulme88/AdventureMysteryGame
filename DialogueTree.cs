using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/DialogueTree")]
public class DialogueTree : ScriptableObject
{
    public string startingNodeId;
    public List<DialogueNode> nodes;

    public DialogueNode GetNodeById(string id)
    {
        return nodes.Find(n => n.id == id);
    }
}
