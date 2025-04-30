using UnityEngine;
using System.Collections.Generic;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetFlag(string flagName, bool value = true)
    {
        flags[flagName] = value;
        Debug.Log($"Flag set: {flagName} = {value}");
    }

    public bool HasFlag(string flagName)
    {
        return flags.ContainsKey(flagName) && flags[flagName];
    }

    public void ClearFlag(string flagName)
    {
        if(flags.ContainsKey(flagName))
        {
            flags.Remove(flagName);
        }
    }
}
