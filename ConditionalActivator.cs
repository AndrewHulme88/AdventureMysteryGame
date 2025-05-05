using UnityEngine;

public class ConditionalActivator : MonoBehaviour
{
    public string requiredFlag;
    public bool requireFlagToBeTrue = true;
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    private void Start()
    {
        ProgressManager.Instance.OnFlagChanged += HandleFlagChanged;
        CheckFlag();
    }

    private void OnDestroy()
    {
        if(ProgressManager.Instance != null)
        {
            ProgressManager.Instance.OnFlagChanged -= HandleFlagChanged;
        }
    }

    private void HandleFlagChanged(string flagName, bool value)
    {
        if(flagName == this.requiredFlag)
        {
            CheckFlag();
        }
    }

    private void CheckFlag()
    {
        bool flagState = ProgressManager.Instance.HasFlag(requiredFlag);

        if (flagState == requireFlagToBeTrue)
        {
            foreach (var go in objectsToEnable) go.SetActive(true);
            foreach (var go in objectsToDisable) go.SetActive(false);
        }
        else
        {
            foreach (var go in objectsToEnable) go.SetActive(false);
            foreach (var go in objectsToDisable) go.SetActive(true);
        }
    }
}
