using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public string nextNodeId;

    [Header("Optional Conditions")]
    public string requiredFlag;
    public bool requiredFlagToBeTrue = false;
}
