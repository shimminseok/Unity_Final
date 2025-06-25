using UnityEngine;


public class PassiveSO : ScriptableObject
{
    public JobType JobType;
    public int PassiveID;
    public string PassiveName;
    public string Description;
    public EmotionType TriggerEmotion;
    public virtual bool CanTrigger(BaseEmotion currentEmotion) => true;

    public Unit Owner { get; private set; }

    public void Initialize(Unit owner)
    {
        Owner = owner;
    }
}