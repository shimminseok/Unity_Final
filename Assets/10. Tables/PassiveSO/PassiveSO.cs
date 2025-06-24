using UnityEngine;


public class PassiveSO : ScriptableObject
{
    public JobType JobType;
    public int PassiveID;
    public string Description;

    public virtual bool CanTrigger(BaseEmotion currentEmotion) => true;

    public Unit Owner { get; private set; }

    public void Initialize(Unit owner)
    {
        Owner = owner;
    }
}