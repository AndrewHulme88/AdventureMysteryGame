using UnityEngine;

public class FlagBasedObjectState : MonoBehaviour
{
    [Header("Flag Settings")]
    public string flagName;
    public bool requireFlagTrue = true;

    [Header("Object State Changes")]
    public Vector3 newPosition;
    public bool moveObject = false;

    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    private void Start()
    {
        bool hasFlag = ProgressManager.Instance.HasFlag(flagName);
        
        if(hasFlag == requireFlagTrue)
        {
            if(moveObject)
            {
                transform.position = newPosition;
            }

            foreach(var obj in objectsToEnable)
            {
                if (obj != null) obj.SetActive(true);
            }

            foreach(var obj in objectsToDisable)
            {
                if (obj != null) obj.SetActive(false);
            }
        }
    }
}
