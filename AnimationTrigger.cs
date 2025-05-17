using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public GameObject targetObject;
    public Animator anim;
    public string flagName;
    public string triggerName;
    public string boolName;
    public string fallbackStateName;
    public bool isTrigger = true;

    private void Start()
    {
        if(ProgressManager.Instance.HasFlag(flagName))
        {
            if(!string.IsNullOrEmpty(fallbackStateName))
            {
                anim.Play(fallbackStateName);
            }
            else
            {
                if(isTrigger)
                {
                    anim.SetTrigger(triggerName);
                }
                else
                {
                    anim.SetBool(boolName, true);
                }
            }
        }
    }
}
