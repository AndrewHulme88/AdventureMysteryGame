using UnityEngine;

public class ConditionalSprite : MonoBehaviour
{
    public string flag;
    public bool requireFlagToBeTrue = true;
    public SpriteRenderer targetRenderer;
    public Sprite defaultSprite;
    public Sprite alternateSprite;

    private void Start()
    {
        bool hasFlag = ProgressManager.Instance.HasFlag(flag);
        targetRenderer.sprite = hasFlag == requireFlagToBeTrue ? alternateSprite : defaultSprite;
    }
}
