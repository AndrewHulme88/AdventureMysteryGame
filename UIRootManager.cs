using System.Runtime.CompilerServices;
using UnityEngine;

public class UIRootManager : MonoBehaviour
{
    public static UIRootManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
